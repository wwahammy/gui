using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support;
using CoApp.Gui.Toolkit.Support.Converters;
using CoApp.Toolkit.Engine.Client;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Messages;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.Support;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using RegistryView = CoApp.Toolkit.Configuration.RegistryView;

namespace CoApp.Updater.Model
{
    public class UpdateService : IUpdateService
    {
        internal ICoAppService CoApp;
        internal IUpdateSettingsService UpdateSettings;

        public UpdateService()
        {
            var loc = new LocalServiceLocator();
            CoApp = loc.CoAppService;
            UpdateSettings = loc.UpdateSettingsService;
            AllPossibleProducts = new Dictionary<Product, bool>();
        }

        public IEnumerable<string> Warnings
        {
            get { throw new NotImplementedException(); }
        }

        #region IUpdateService Members

// ReSharper disable UnusedAutoPropertyAccessor.Local
        public Task<DateTime?> LastTimeChecked { get; private set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local

        public Task<DateTime?> LastTimeInstalled
        {
            get { return Task.Factory.StartNew(() => CoApp.LastTimeInstalled); }
        }

        public Task CheckForUpdates()
        {
            return CoApp.SetAllFeedsStale().ContinueWith(t => RunCheckForUpdates(null));
        }

        public Task CheckForUpdates(CancellationToken token)
        {
            return CoApp.SetAllFeedsStale().ContinueWith(t => RunCheckForUpdates(token));
        }

        public int NumberOfProducts
        {
            get
            {
                lock (AllPossibleProducts)
                {
                    return AllPossibleProducts.Count;
                }
            }
        }

        public int NumberOfProductsSelected
        {
            get
            {
                lock (AllPossibleProducts)
                {
                    return AllPossibleProducts.Count(kp => kp.Value);
                }
            }
        }


        public Task PerformInstallation()
        {
            return Task.Factory.StartNew(HandleInstall).ContinueWith(t =>
                                                                         {

                                                                             if (!t.IsFaulted)
                                                                             {
                                                                                 var trim = UpdateSettings.AutoTrim.Result;
                                                                                 if (trim)
                                                                                 {
                                                                                     var task = CoApp.TrimAll();
                                                                                     if (task.IsFaulted)
                                                                                     {
                                                                                         // TODO do something?
                                                                                     }
                                                                                 }
                                                                             }
                                                                             else
                                                                             {
                                                                                 Logger.Message(t.Exception.Unwrap().ToString());
                                                                                 throw t.Exception.Unwrap();
                                                                             }
                                                                         }

                );
        }


        public Task BlockProduct(Product product)
        {
            return CoApp.BlockPackage(product.NewId);
        }

        

        public Task<DateTime> LastScheduledTaskRealRun
        {
            get { return Task.Factory.StartNew(() =>
                                                   {
                                                       var lastRun =
                                                           new DateTime(
                                                               RegistryView.ApplicationUser["coapp_update#last_run"].LongValue);
                                                       Logger.Message("Last run is {0}", lastRun.ToString(CultureInfo.InvariantCulture));

                                                       return lastRun;
                                                   }); }
        }

        public Task SetScheduledTaskToRunNow()
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 var now = DateTime.Now;
                                                 Logger.Message("Setting last run to right now - {0}", now.ToString(CultureInfo.InvariantCulture));
                                                 RegistryView.ApplicationUser["coapp_update#last_run"].LongValue =
                                                     now.Ticks;

                                             });
        }


        public void SelectProduct(Product p)
        {
            lock (AllPossibleProducts)
            {
                AllPossibleProducts[p] = true;
                Messenger.Default.Send(new SelectedProductsChangedMessage());
            }
        }

        public void UnselectProduct(Product p)
        {
            lock (AllPossibleProducts)
            {
                AllPossibleProducts[p] = false;
                Messenger.Default.Send(new SelectedProductsChangedMessage());
            }
        }

        public Product CurrentInstallingProduct { get; set; }


        public IDictionary<Product, bool> AllPossibleProducts { get; private set; }

        #endregion

        private void HandleInstall()
        {
            lock (AllPossibleProducts)
            {
                var exceptions = new List<Exception>();

                KeyValuePair<Product, bool>[] selectedProds = AllPossibleProducts.Where(kv => kv.Value).ToArray();


                foreach (
                    var prod in selectedProds.OrderBy(p => p.Key.IsUpgrade).Select((kvp, num) => new {kvp.Key, num}))
                {
                    CurrentInstallingProduct = prod.Key;
                    Logger.Message("Installing {0}" + Environment.NewLine, CurrentInstallingProduct.DisplayName);
                    Messenger.Default.Send(new InstallationProgressMessage
                                               {
                                                   CurrentProduct = prod.Key,
                                                   CurrentProductNumber = prod.num + 1,
                                                   ProductProgressCompleted = 0,
                                                   TotalNumberOfProducts =
                                                       selectedProds.Count(),
                                                   TotalProgressCompleted =
                                                       GetTotalProgress(
                                                           selectedProds.Count(),
                                                           prod.num, 0)
                                               });
                    Task t;
                    var realProd = prod;
                    if (prod.Key.IsUpgrade)
                    {
                        t = CoApp.UpgradeExistingPackage(prod.Key.OldId, true, (ignore1, ignore2, totalPackageProgress) =>
                                                                            Messenger.Default.Send(new InstallationProgressMessage
                                                                                                       {
                                                                                                           CurrentProduct
                                                                                                               =
                                                                                                               realProd.
                                                                                                               Key,
                                                                                                           CurrentProductNumber
                                                                                                               =
                                                                                                               realProd.
                                                                                                                   num +
                                                                                                               1,
                                                                                                           ProductProgressCompleted
                                                                                                               =
                                                                                                               totalPackageProgress,
                                                                                                           TotalNumberOfProducts
                                                                                                               =
                                                                                                               selectedProds
                                                                                                               .Count(),
                                                                                                           TotalProgressCompleted
                                                                                                               =
                                                                                                               GetTotalProgress
                                                                                                               (
                                                                                                                   selectedProds
                                                                                                                       .
                                                                                                                       Count
                                                                                                                       (),
                                                                                                                   realProd
                                                                                                                       .
                                                                                                                       num,
                                                                                                                   totalPackageProgress)
                                                                                                       }), null);
                    }
                    else
                    {
                        t = CoApp.UpdateExistingPackage(prod.Key.OldId, true, (ignore1, ignore2, totalPackageProgress) =>
                                                                           Messenger.Default.Send(new InstallationProgressMessage
                                                                                                      {
                                                                                                          CurrentProduct
                                                                                                              =
                                                                                                              realProd.
                                                                                                              Key,
                                                                                                          CurrentProductNumber
                                                                                                              =
                                                                                                              realProd.
                                                                                                                  num +
                                                                                                              1,
                                                                                                          ProductProgressCompleted
                                                                                                              =
                                                                                                              totalPackageProgress,
                                                                                                          TotalNumberOfProducts
                                                                                                              =
                                                                                                              selectedProds
                                                                                                              .Count(),
                                                                                                          TotalProgressCompleted
                                                                                                              =
                                                                                                              GetTotalProgress
                                                                                                              (
                                                                                                                  selectedProds
                                                                                                                      .
                                                                                                                      Count
                                                                                                                      (),
                                                                                                                  realProd
                                                                                                                      .
                                                                                                                      num,
                                                                                                                  totalPackageProgress)
                                                                                                      }), null);
                    }

                    //wait for THIS update/upgrade to finish
                    var product = prod;
                    var c = t.ContinueWith(t1 =>
                                       {
                                           if (t1.IsFaulted)
                                           {
                                               Logger.Message("Failed to install {0}" + Environment.NewLine,
                                                              CurrentInstallingProduct.DisplayName);
                                               //uh oh, it didn't add it to our list
                                               exceptions.Add(t.Exception);
                                           }
                                           else
                                           {
                                               AllPossibleProducts.Remove(product.Key);
                                           }
                                       });

                    c.Wait();
                    //did it work?

                }


                Messenger.Default.Send(new InstallationFinishedMessage { NumberOfProductsInstalled = selectedProds.Count() - exceptions.Count});
                

                //we've gone through all of them, do we have any exceptions?
                


                if (exceptions.Any())
                {
                    //dang it, things are broken
                    Messenger.Default.Send(new InstallationFailedMessage());    
                    throw new AggregateException(exceptions);
                }

            }
        }

        private void RunCheckForUpdates(CancellationToken? token)
        {
            var tasks = new[]
                            {
                                CoApp.GetUpdatablePackages(),
                                CoApp.GetUpgradablePackages()
                            };

            //TODO this won't get all the errors, just one of them

            if (token != null)
            {
                Task.WaitAll(tasks, (CancellationToken) token);
                ((CancellationToken) token).ThrowIfCancellationRequested();
            }
            else
            {
                Task.WaitAll(tasks);
            }

            var updatableResults = HandleGetUpdatablePackages(tasks[0], token);
            var upgradableResults = HandleGetUpdatablePackages(tasks[1], token, true);

            if (token != null)
            {
                ((CancellationToken) token).ThrowIfCancellationRequested();
            }
            //you had your chance, you can't cancel

            lock (AllPossibleProducts)
            {
                AllPossibleProducts.Clear();
                var choice = CoApp.UpdateChoice;
                foreach (var product in updatableResults.Concat(upgradableResults))
                {
                    AllPossibleProducts[product] = (choice == Gui.Toolkit.Model.Interfaces.UpdateChoice.AutoInstallAll) ||
                                                   (choice == Gui.Toolkit.Model.Interfaces.UpdateChoice.Notify) ||
                                                   (choice == Gui.Toolkit.Model.Interfaces.UpdateChoice.AutoInstallJustUpdates &&
                                                    !product.IsUpgrade);
                }
            }
            Messenger.Default.Send(new SelectedProductsChangedMessage());
        }

        private IEnumerable<Product> HandleGetUpdatablePackages(Task<IEnumerable<Package>> task,
                                                                CancellationToken? token, bool isUpgrade = false)
        {
            if (task.IsFaulted)
            {
                //send some type of error
                throw task.Exception.Unwrap();
            }

            var all = task.Result.Select(p => new { Package = p, Set = GetDetailedPackageSet(p)}).ToArray();
            var tasks = all.Select(a => a.Set).ToArray();

          
            //TODO this won't get all the errors, just one of them
            if (token != null)
            {
                Task.WaitAll(tasks, (CancellationToken) token);
                ((CancellationToken) token).ThrowIfCancellationRequested();
            }
            else
            {
                Task.WaitAll(tasks);
            }

            var products = new List<Product>();
            var exceptions = tasks.FindAllExceptions();
            if (exceptions.Length != 0)
                throw new AggregateException(exceptions);
            //find if there are any faults. If so we report
            foreach (var t in all)
            {

                Package ourUpdate = null;
                ourUpdate = isUpgrade ? t.Set.Result.AvailableNewer : t.Set.Result.AvailableNewerCompatible;

                products.Add(new Product
                                 {
                                     DisplayName = ourUpdate.DisplayName,
                                     OldId = t.Package.CanonicalName,
                                     NewId = ourUpdate.CanonicalName,
                                     IsUpgrade = isUpgrade,
                                     Summary = ourUpdate.Summary,
                                     UpdateTime = DateTime.Parse(ourUpdate.PublishDate)
                                 });
            }

            return products;
        }

        private static int GetTotalProgress(int totalNum, int currentNum, int progressOnProduct)
        {
            double totalPerProductPercentage = 1/(double) totalNum;
            double totalForAllPrevious = totalPerProductPercentage*currentNum;
            double progressForCurrent = (progressOnProduct/(double) 100)*totalPerProductPercentage;

            return (int) ((totalForAllPrevious + progressForCurrent)*100);
        }


        private Task<PackageSet> GetDetailedPackageSet(Package p)
        {
            return GetDetailedPackageSet(p.CanonicalName);
        }


        private Task<PackageSet> GetDetailedPackageSet(string canonicalName)
        {
            return Task.Factory.StartNew(() =>
                                      {
                                          var psT = CoApp.GetPackageSet(canonicalName);
                                          psT.Wait();
                                          if (psT.IsFaulted)
                                              throw psT.Exception.Unwrap();

                                          var ps = psT.Result;

                                          var tasks = new List<Task>();
                                          if (ps.AvailableNewer != null)
                                          {
                                              tasks.Add(
                                                  CoApp.GetPackageDetails(ps.AvailableNewer).ContinueWith(
                                                      t => ps.AvailableNewer = t.Result));
                                          }
                                          if (ps.AvailableNewerCompatible != null)
                                          {
                                              tasks.Add(
                                                  CoApp.GetPackageDetails(ps.AvailableNewerCompatible).ContinueWith(
                                                      t => ps.AvailableNewerCompatible = t.Result));
                                          }

                                          Task.WaitAll(tasks.ToArray());

                                          var exceptions = tasks.FindAllExceptions();
                                          if (exceptions.Length != 0)
                                              throw new AggregateException(exceptions);


                                          return ps;
                                      }




                );
        }


        private static ScheduledTask DefaultTask()
        {
            return new ScheduledTask { CommandLine = "--quiet", DayOfWeek = null, Executable = "Coapp.Update", Hour = 3, IntervalInMinutes = 60, Minutes = 0, Name = "coapp_update"};
        }

    }
}