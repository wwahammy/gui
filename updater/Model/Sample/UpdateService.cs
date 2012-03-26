#if SAMPLEDATA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoApp.Toolkit.DynamicXml;
using CoApp.Updater.Messages;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Updater.Model.Sample
{

    /*
    public class UpdateService : IUpdateService
    {
        private const int MsToWait = 30;
        private readonly List<ProdDecl> _prods = new List<ProdDecl>();
        private readonly dynamic _root;

        private UpdateChoice _updateChoice = Interfaces.UpdateChoice.AutoInstallAll;
        private UpdateDayOfWeek _updateDayOfWeek = Interfaces.UpdateDayOfWeek.Everyday;
        private int _updateTime = 3;

        private bool _autoTrim = true;

        public UpdateService()
        {
            AllPossibleProducts = new Dictionary<Product, bool>();
            _root = new DynamicNode(XDocument.Load("SampleData.xml"));
        }

        #region IUpdateService Members

        public Task CheckForUpdates()
        {
            return CheckForUpdates(null);
        }

        public Task CheckForUpdates(CancellationToken token)
        {
            return CheckForUpdates((CancellationToken?)token);
        }

        Task<DateTime?> IUpdateService.LastTimeChecked
        {
            get { throw new NotImplementedException(); }
        }

        Task<DateTime?> IUpdateService.LastTimeInstalled
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime? LastTimeChecked { get;
            private set;
        }

        public DateTime? LastTimeInstalled
        {
            get; private set; }

        private Task CheckForUpdates(CancellationToken? token)
        {
            if (token == null)
            {
                return Task.Factory.StartNew(() =>
                                                 {
                                                     if (!_prods.Any())
                                                     {
                                                         foreach (dynamic item in _root.Products)
                                                         {
                                                             if (item.Element.Name == "Product")
                                                             {
                                                                 var product = new Product
                                                                                   {
                                                                                       DisplayName =
                                                                                           item.Attributes.Name,
                                                                                       Summary =
                                                                                           item.Summary.Element.Value,
                                                                                       IsUpgrade =
                                                                                           item.Attributes.Type ==
                                                                                           "upgrade"
                                                                                   };
                                                                 _prods.Add(new ProdDecl {p = product});
                                                             }
                                                         }

                                                         

                                                         CreateAllPossibleProds();

                                                         LastTimeInstalled =
                                                             DateTime.Parse(_root.LastInstalled.Element.Value);

                                                     }


                                                     Thread.Sleep(2000);
                                                     LastTimeChecked = DateTime.Now;
                                                     Messenger.Default.Send(new SelectedProductsChangedMessage());
                                                 });
            }
            else
            {
                return Task.Factory.StartNew(() =>
                {
                    if (!_prods.Any())
                    {
                        foreach (dynamic item in _root.Products)
                        {
                            if (item.Element.Name == "Product")
                            {
                                var product = new Product
                                {
                                    DisplayName =
                                        item.Attributes.Name,
                                    Summary =
                                        item.Summary.Element.Value,
                                    IsUpgrade =
                                        item.Attributes.Type ==
                                        "upgrade"
                                };
                                _prods.Add(new ProdDecl { p = product });
                            }
                        }

                        CreateAllPossibleProds();
                        LastTimeInstalled =
                                   DateTime.Parse(_root.LastInstalled.Element.Value);
                       
                    }
                    Thread.Sleep(2000);
                    LastTimeChecked = DateTime.Now;
                    Messenger.Default.Send(new SelectedProductsChangedMessage());
                    ((CancellationToken)token).ThrowIfCancellationRequested();
                }, (CancellationToken) token);
            }
        }

        public void SelectProduct(Product p)
        {
            lock (AllPossibleProducts)
            {
                AllPossibleProducts[p] = true;
                Messenger.Default.Send(new SelectedProductsChangedMessage());
            }
        }

       

        public Product CurrentInstallingProduct { get; private set; }


        public Task PerformInstallation()
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 var selectedProds =
                                                     AllPossibleProducts.Where((p) => p.Value).ToArray();

                                                 foreach (
                                                     var prod in selectedProds.Select((kvp, num) => new {kvp.Key, num}))
                                                 {
                                                     CurrentInstallingProduct = prod.Key;
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
                                                     
                                                         Messenger.Default.Send(new InstallationProgressMessage
                                                                                    {
                                                                                        CurrentProduct = prod.Key,
                                                                                        CurrentProductNumber =
                                                                                            prod.num + 1,
                                                                                        ProductProgressCompleted = i,
                                                                                        TotalNumberOfProducts =
                                                                                            selectedProds.Count(),
                                                                                        TotalProgressCompleted =
                                                                                            GetTotalProgress(
                                                                                                selectedProds.Count(),
                                                                                                prod.num, i)
                                                                                    });
                                                     }

                                                     AllPossibleProducts.Remove(prod.Key);
                                                 }

                                                 Messenger.Default.Send(new InstallationFinishedMessage
                                                                            {
                                                                                NumberOfProductsInstalled =
                                                                                    selectedProds.Count()
                                                                            });
                                                 Messenger.Default.Send(new SelectedProductsChangedMessage());
                                             }
                );
        }

        public Task BlockProduct(Product product)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 _prods.First(d => d.p == product).Blocked = true;
                                                 CreateAllPossibleProds();
                                                 Messenger.Default.Send(new SelectedProductsChangedMessage());
                                             });
        }

        public Task<UpdateChoice> UpdateChoice()
        {
            return Task.Factory.StartNew(() => _updateChoice);
        }

        public Task SetUpdateChoice(UpdateChoice choice)
        {
            return Task.Factory.StartNew(() => _updateChoice = choice);
        }

        public Task<UpdateTimeAndDay> UpdateTimeAndDay
        {
            get { throw new NotImplementedException(); }
        }

        public Task<int> UpdateTime()
        {
            return Task.Factory.StartNew(() => _updateTime);
        }

        public Task SetUpdateTime(int hour)
        {
            return Task.Factory.StartNew(() => _updateTime = hour);
        }

        public Task<UpdateDayOfWeek> UpdateDayOfWeek()
        {
            return Task.Factory.StartNew(() => _updateDayOfWeek);
        }

        public Task SetUpdateDayOfWeek(UpdateDayOfWeek day)
        {
            return Task.Factory.StartNew(() => _updateDayOfWeek = day);
        }

        public Task SetUpdateTimeAndDay(int hour, UpdateDayOfWeek day)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AutoTrim()
        {
            return Task.Factory.StartNew(() => _autoTrim);
        }

        public Task SetAutoTrim(bool autotrim)
        {
            return Task.Factory.StartNew(() => _autoTrim = autotrim);
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

        public IDictionary<Product, bool> AllPossibleProducts { get; private set; }

        #endregion

        private void CreateAllPossibleProds()
        {
            lock (AllPossibleProducts)
            {
                AllPossibleProducts.Clear();
                foreach (ProdDecl prod in _prods)
                {
                    if (!prod.Installed && !prod.Blocked)
                    {
                        AllPossibleProducts[prod.p] = !prod.p.IsUpgrade ||
                                                      prod.p.IsUpgrade &&
                                                      _root.UpgradeOrUpdate.Element.Value == "Upgrade";
                    }
                }
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

        private static int GetTotalProgress(int totalNum, int currentNum, int progressOnProduct)
        {
            double totalPerProductPercentage = 1/(double) totalNum;
            double totalForAllPrevious = totalPerProductPercentage*currentNum;
            double progressForCurrent = (progressOnProduct/(double) 100)*totalPerProductPercentage;

            return (int)((totalForAllPrevious + progressForCurrent) * 100);
        }

        #region Nested type: ProdDecl

        private class ProdDecl
        {
            public bool Blocked;
#pragma warning disable 649
            public bool Installed;

            public Product p;
#pragma warning restore 649
        }

        #endregion
    }
    */

}
#endif