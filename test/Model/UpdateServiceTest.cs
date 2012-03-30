using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoApp.Gui.Test.TestSupportFiles;
using CoApp.Toolkit.Engine.Client;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Messages;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CoApp.Toolkit.Extensions;

namespace CoApp.Gui.Test.Model
{
    /// <summary>
    ///This is a test class for UpdateServiceTest and is intended
    ///to contain all UpdateServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class UpdateServiceTest
    {
        private ICoAppService _coapp;
        private Mock<ICoAppService> _mockCoapp;
        private UpdateService _updateService;

        private const string UPGRADABLE_AND_UPDATABLE = "UpgradableAndUpdatable";
        private const string UPGRADABLE1 = "Upgradable1";
        private const string UPDATABLE1 = "Updatable1";
        [TestInitialize()]
        public void InstanceInitialize()
        {
            _mockCoapp = CreateMockCoApp();
            _coapp = _mockCoapp.Object;
            _updateService = new UpdateService {CoApp = _coapp};
            Messenger.OverrideDefault(new Messenger());
        }

        private Mock<ICoAppService> CreateMockCoApp()
        {
            var coapp = new Mock<ICoAppService>();
            return coapp;
        }

        /// <summary>
        ///A test for Warnings
        ///</summary>
        //[TestMethod()]
        public void WarningsTest()
        {
            var target = new UpdateService(); // TODO: Initialize to an appropriate value
            IEnumerable<string> actual;
            actual = target.Warnings;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateTimeAndDay
        ///</summary>
        [TestMethod]
        public void UpdateTimeAndDayTestMonday()
        {
            _mockCoapp.Setup((i) => i.GetScheduledTask("coapp_update")).Returns(
                () =>
                Task.Factory.StartNew(
                    () =>
                    new ScheduledTask {DayOfWeek = DayOfWeek.Monday, Hour = 4, Minutes = 10, Name = "coapp_update"}));

            UpdateTimeAndDay actual;
            actual = _updateService.UpdateTimeAndDay.Result;
            Assert.AreEqual(4, actual.Time);
            Assert.AreEqual(UpdateDayOfWeek.Monday, actual.DayOfWeek);
        }


        /// <summary>
        ///A test for UpdateTimeAndDay
        ///</summary>
        [TestMethod]
        public void UpdateTimeAndDayTestEveryday()
        {
            _mockCoapp.Setup((i) => i.GetScheduledTask("coapp_update")).Returns(
                () =>
                Task.Factory.StartNew(
                    () => new ScheduledTask {DayOfWeek = null, Hour = 4, Minutes = 10, Name = "coapp_update"}));

            UpdateTimeAndDay actual;
            actual = _updateService.UpdateTimeAndDay.Result;
            Assert.AreEqual(4, actual.Time);
            Assert.AreEqual(UpdateDayOfWeek.Everyday, actual.DayOfWeek);
        }

        /// <summary>
        ///A test for UpdateTimeAndDay
        ///</summary>
        [TestMethod]
        public void UpdateTimeAndDayTestDefault()
        {
            _mockCoapp.Setup((i) => i.GetScheduledTask("coapp_update")).Returns(
                () => Task<ScheduledTask>.Factory.StartNew(() => { throw new Exception(); }));

            UpdateTimeAndDay actual;
            actual = _updateService.UpdateTimeAndDay.Result;
            Assert.AreEqual(3, actual.Time);
            Assert.AreEqual(UpdateDayOfWeek.Everyday, actual.DayOfWeek);
        }


        /// <summary>
        ///A test for UpdateChoice
        ///</summary>
        [TestMethod]
        public void UpdateChoiceTest()
        {
            foreach (
                UpdateChoice member in
                    Enum.GetNames(typeof (UpdateChoice)).Select(s => Enum.Parse(typeof (UpdateChoice), s)).Cast
                        <UpdateChoice>())
            {
                _mockCoapp.Setup(i => i.UpdateChoice).Returns(member);
                UpdateChoice mem = member;
                Task<bool> c = _updateService.UpdateChoice.ContinueWith(t => t.Result == mem);
                c.Wait();
            }
        }

        [TestMethod]
        public void SetUpdateChoiceTest()
        {
            foreach (
                UpdateChoice member in
                    Enum.GetNames(typeof (UpdateChoice)).Select(s => Enum.Parse(typeof (UpdateChoice), s)).Cast
                        <UpdateChoice>())
            {
                UpdateChoice mem = member;
                _mockCoapp.SetupSet(i => i.UpdateChoice = mem);
                Task c =
                    _updateService.SetUpdateChoice(mem).ContinueWith(
                        t => _mockCoapp.VerifySet(m => m.UpdateChoice = mem, Times.Once()));
                c.Wait();
            }
        }

        [TestMethod]
        public void SetUpdateChoiceTestFaulted()
        {
            _mockCoapp.SetupSet(i => i.UpdateChoice = UpdateChoice.Notify).Throws<Exception>();
            Task c = _updateService.SetUpdateChoice(UpdateChoice.Notify).ContinueWith(t => Assert.IsTrue(t.IsFaulted));

            c.Wait();
        }

        [TestMethod]
        public void UpdateChoiceTestFaulted()
        {
            _mockCoapp.SetupGet(i => i.UpdateChoice).Throws<Exception>();
            Task c = _updateService.UpdateChoice.ContinueWith(t => Assert.IsTrue(t.IsFaulted));

            c.Wait();
        }

        /// <summary>
        ///A test for UnselectProduct
        ///</summary>
        [TestMethod]
        public void UnselectProductTest()
        {
            SetupValidCheck();
            var t = _updateService.CheckForUpdates();
            t.Wait();

            var p = _updateService.AllPossibleProducts.Where(
                kp => kp.Value).First().Key;

            _updateService.UnselectProduct(p);

            Assert.IsFalse(_updateService.AllPossibleProducts[p]);

            _updateService.UnselectProduct(p);

            Assert.IsFalse(_updateService.AllPossibleProducts[p]);
        }

        /// <summary>
        ///A test for SetUpdateTimeAndDay
        ///</summary>
        [TestMethod]
        public void SetUpdateTimeAndDayTest()
        {
            _mockCoapp.Setup(m => m.SetScheduledTask("coapp_update", It.IsAny<string>(), "--quiet", 5, 0, null, 60)).
                Returns(() => Task.Factory.StartNew(() =>
                                                        {
                                                        }));

            var c = _updateService.SetUpdateTimeAndDay(5, UpdateDayOfWeek.Everyday).ContinueWith(t =>
                                                                                             {
                                                                                                 Assert.IsFalse(
                                                                                                     t.IsFaulted);

                                                                                                 _mockCoapp.Verify(
                                                                                                     m =>
                                                                                                     m.SetScheduledTask(
                                                                                                         "coapp_update",
                                                                                                         It.IsAny
                                                                                                             <string>(),
                                                                                                         "--quiet", 5, 0,
                                                                                                         null, 60));
                                                                                             });

            c.Wait();



        }


        [TestMethod]
        public void SetUpdateTimeAndDayTestFaulted()
        {
            _mockCoapp.Setup(m => m.SetScheduledTask("coapp_update", It.IsAny<string>(), "--quiet", 5, 0, null, 60)).
                Returns(() => Task.Factory.StartNew(() =>
                                                        { throw new Exception();
                                                        }));

            var c = _updateService.SetUpdateTimeAndDay(5, UpdateDayOfWeek.Everyday).ContinueWith(t =>
            {
                Assert.IsTrue(
                    t.IsFaulted);

        
            });

            c.Wait();



        }

        /// <summary>
        ///A test for SetAutoTrim
        ///</summary>
        [TestMethod]
        public void SetAutoTrimTestTrue()
        {
            _mockCoapp.SetupSet(i => i.TrimOnUpdate = true);
            Task actual = _updateService.SetAutoTrim(true);

            Task c = actual.ContinueWith(t => Assert.IsFalse(t.IsFaulted));
            c.Wait();
        }

        /// <summary>
        ///A test for SetAutoTrim
        ///</summary>
        [TestMethod]
        public void SetAutoTrimTestFalse()
        {
            _mockCoapp.SetupSet(i => i.TrimOnUpdate = false);
            Task actual = _updateService.SetAutoTrim(false);

            Task c = actual.ContinueWith(t => Assert.IsFalse(t.IsFaulted));
            c.Wait();
        }


        /// <summary>
        ///A test for SetAutoTrim
        ///</summary>
        [TestMethod]
        public void SetAutoTrimTestFaulted()
        {
            _mockCoapp.SetupSet(i => i.TrimOnUpdate = true).Throws<Exception>();
            Task actual = _updateService.SetAutoTrim(true);
            Task c = actual.ContinueWith(t => Assert.IsTrue(t.IsFaulted));
        }

        /// <summary>
        ///A test for SelectProduct
        ///</summary>
        [TestMethod]
        public void SelectProductTest()
        {
            SetupValidCheck();
            var t = _updateService.CheckForUpdates();
            t.Wait();

            var p = _updateService.AllPossibleProducts.Where(
                kp => !kp.Value).First().Key;

            _updateService.SelectProduct(p);

            Assert.IsTrue(_updateService.AllPossibleProducts[p]);

            _updateService.SelectProduct(p);

            Assert.IsTrue(_updateService.AllPossibleProducts[p]);
        }


        /// <summary>
        ///A test for PerformInstallation
        ///</summary>
        [TestMethod]
        public void PerformInstallationTest()
        {
            SetupValidCheck();

            _mockCoapp.Setup(m => m.UpdateChoice).Returns(UpdateChoice.AutoInstallAll);

            _updateService.CheckForUpdates().Wait();
            _mockCoapp.Setup(
                m =>
                m.UpdateExistingPackage(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<Action<string, int, int>>(),
                                        It.IsAny<Action<string>>())).Returns(() => Task.Factory.StartNew(() => { }));

            _mockCoapp.Setup(
                m =>
                m.UpgradeExistingPackage(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<Action<string, int, int>>(),
                                        It.IsAny<Action<string>>())).Returns(() => Task.Factory.StartNew(() => { }));
            int timesWeGotInstallation = 0;
            var timesWeGotFailure = 0;
            var locker = new object();
            var recip = new object();
            
            Messenger.Default.Register<InstallationFinishedMessage>(recip, (m) =>
                                                                              {
                                                                                  Assert.AreEqual(4, m.NumberOfProductsInstalled);
                                                                                  lock (locker)
                                                                                  {
                                                                                      timesWeGotInstallation++;
                                                                                      
                                                                                  }
                                                                              });

            Messenger.Default.Register<InstallationFailedMessage>(recip, (m) =>
            {
                lock (locker)
                {
                    timesWeGotFailure++;

                }
            });

            var t = _updateService.PerformInstallation();

            t.Wait();

            _mockCoapp.Verify(m => m.UpdateExistingPackage(UPDATABLE1, It.IsAny<bool?>(), It.IsAny<Action<string,int, int>>(), It.IsAny<Action<string>>()),Times.Once());

            _mockCoapp.Verify(m => m.UpdateExistingPackage(UPGRADABLE_AND_UPDATABLE, It.IsAny<bool?>(), It.IsAny<Action<string, int, int>>(), It.IsAny<Action<string>>()));


            _mockCoapp.Verify(m => m.UpgradeExistingPackage(UPGRADABLE1, It.IsAny<bool?>(), It.IsAny<Action<string, int, int>>(), It.IsAny<Action<string>>()), Times.Once());

            _mockCoapp.Verify(m => m.UpgradeExistingPackage(UPGRADABLE_AND_UPDATABLE, It.IsAny<bool?>(), It.IsAny<Action<string, int, int>>(), It.IsAny<Action<string>>()));

            Assert.AreEqual(1, timesWeGotInstallation);
            Assert.AreEqual(0, timesWeGotFailure);

            Assert.AreEqual(0, _updateService.NumberOfProducts);
            Assert.AreEqual(0, _updateService.NumberOfProductsSelected);
        }


        [TestMethod]
        public void PerformInstallationTestFaulted()
        {
            

            SetupValidCheck();
            _mockCoapp.Setup(i => i.UpdateChoice).Returns(() => UpdateChoice.AutoInstallAll);
            

            _updateService.CheckForUpdates().Wait();

           

            _mockCoapp.Setup(
                m =>
                m.UpdateExistingPackage(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<Action<string, int, int>>(),
                                        It.IsAny<Action<string>>())).Returns(() => Task.Factory.StartNew(() => { }));

            _mockCoapp.Setup(
                m =>
                m.UpgradeExistingPackage(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<Action<string, int, int>>(),
                                        It.IsAny<Action<string>>())).Returns(() => Task.Factory.StartNew(() => { }));

            _mockCoapp.Setup(m => m.UpgradeExistingPackage(UPGRADABLE_AND_UPDATABLE, It.IsAny<bool?>(), It.IsAny<Action<string, int, int>>(), It.IsAny<Action<string>>())).Returns(() => Task.Factory.StartNew(() =>
                                                                                                                                                                                                                   {
                                                                                                                                                                                                                       throw new Exception("1");
                                                                                                                                                                                                                   }));
            int timesWeGotInstallation = 0;
            var timesWeGotFailure = 0;
            var locker = new object();
            Messenger.Default.Register<InstallationFinishedMessage>(this, (m) =>
            {
                Assert.AreEqual(3, m.NumberOfProductsInstalled);
                lock (locker)
                {
                    timesWeGotInstallation++;

                }
            });

            Messenger.Default.Register<InstallationFailedMessage>(this, (m) =>
            {
                lock (locker)
                {
                    timesWeGotFailure++;

                }
            });

            var t = _updateService.PerformInstallation();
            var c = t.ContinueWith(
                t1 =>
                    {
                        Logger.Message(t1.Exception.Unwrap().ToString());
                        Assert.AreEqual("1", t1.Exception.Unwrap().Message);


                    }


                );

            c.Wait();
            Assert.AreEqual(1, timesWeGotInstallation);
            Assert.AreEqual(1, timesWeGotFailure);

            Assert.AreEqual(1, _updateService.NumberOfProductsSelected);
            

        }

        /// <summary>
        ///A test for CheckForUpdates
        ///</summary>
        [TestMethod]
        public void CheckForUpdatesTest()
        {
            var timesReceivedSelectedProductsChangedMessage = 0;
            var changeLock = new object();
            Messenger.Default.Register<SelectedProductsChangedMessage>(this, (m) =>
                                                 {
                                                     lock (changeLock)
                                                     {
                                                         timesReceivedSelectedProductsChangedMessage++;
                                                     }
                                                 });
            _mockCoapp.Setup(i => i.SetAllFeedsStale()).Returns(() => Task.Factory.StartNew(() => { }));

            _mockCoapp.Setup(i => i.GetUpdatablePackages()).Returns(
                () => Task.Factory.StartNew(() => UpdatablePackagesForCheckForUpdates()));

            _mockCoapp.Setup(i => i.GetPackageSet(It.IsAny<string>())).Returns(
                (string i) => Task.Factory.StartNew(() => GetPackageSetForTestPackages(i)));


            _mockCoapp.Setup(i => i.GetUpgradablePackages()).Returns(() =>
                                                                     Task.Factory.StartNew(
                                                                         () => UpgradablePackagesForCheckForUpdates()));
            _mockCoapp.Setup(i => i.GetPackageDetails(It.IsAny<Package>())).Returns(
                (Package p) => Task.Factory.StartNew(
                    () => GetPackageDetails(p)));

            _mockCoapp.SetupGet(i => i.UpdateChoice).Returns(() => UpdateChoice.AutoInstallJustUpdates);


            Task c =
                _updateService.CheckForUpdates().ContinueWith(t => VerifyFeedsAreStale(Times.Once())).ContinueWith(t =>

                                                                                                                       {
                                                                                                                           Assert
                                                                                                                               .
                                                                                                                               IsFalse
                                                                                                                               (
                                                                                                                                   t
                                                                                                                                       .
                                                                                                                                       IsFaulted);
                                                                                                                           Assert
                                                                                                                               .
                                                                                                                               AreEqual
                                                                                                                               (
                                                                                                                                   4,
                                                                                                                                   _updateService
                                                                                                                                       .
                                                                                                                                       NumberOfProducts);

                                                                                                                           Assert
                                                                                                                               .
                                                                                                                               AreEqual
                                                                                                                               (
                                                                                                                                   2,
                                                                                                                                   _updateService
                                                                                                                                       .
                                                                                                                                       NumberOfProductsSelected);
                                                                                                                       }
                    );

            c.Wait();


            _mockCoapp.SetupGet(i => i.UpdateChoice).Returns(() => UpdateChoice.AutoInstallAll);

            c =
                _updateService.CheckForUpdates().ContinueWith(t => VerifyFeedsAreStale(Times.Exactly(2))).ContinueWith(
                    t =>
                        {
                            Assert.
                                IsFalse(
                                    t.
                                        IsFaulted);
                            Assert.
                                AreEqual(
                                    4,
                                    _updateService
                                        .
                                        NumberOfProducts);

                            Assert.
                                AreEqual(
                                    4,
                                    _updateService
                                        .
                                        NumberOfProductsSelected);
                        }
                    );

            c.Wait();

            _mockCoapp.SetupGet(i => i.UpdateChoice).Returns(() => UpdateChoice.Notify);
            c =
                _updateService.CheckForUpdates().ContinueWith(t => VerifyFeedsAreStale(Times.Exactly(3))).ContinueWith(
                    t =>
                        {
                            Assert.
                                IsFalse(
                                    t.
                                        IsFaulted);
                            Assert.
                                AreEqual(
                                    4,
                                    _updateService
                                        .
                                        NumberOfProducts);

                            Assert.
                                AreEqual(
                                    4,
                                    _updateService
                                        .
                                        NumberOfProductsSelected);
                        }
                    );

            c.Wait();

            _mockCoapp.SetupGet(i => i.UpdateChoice).Returns(() => UpdateChoice.Dont);
            c =
                _updateService.CheckForUpdates().ContinueWith(t => VerifyFeedsAreStale(Times.Exactly(4))).ContinueWith(
                    t =>
                        {
                            Assert.
                                IsFalse(
                                    t.
                                        IsFaulted);
                            Assert.
                                AreEqual(
                                    4,
                                    _updateService
                                        .
                                        NumberOfProducts);

                            Assert.
                                AreEqual(
                                    0,
                                    _updateService
                                        .
                                        NumberOfProductsSelected);
                        }
                    );
            c = c.ContinueWith(t =>
                                   {
                                       lock (changeLock)
                                       {
                                           Assert.AreEqual(4, timesReceivedSelectedProductsChangedMessage);
                                       }
                                   });
            c.Wait();
        }

        private void SetupValidCheck()
        {
            _mockCoapp.Setup(i => i.SetAllFeedsStale()).Returns(() => Task.Factory.StartNew(() => { }));

            _mockCoapp.Setup(i => i.GetUpdatablePackages()).Returns(
                () => Task.Factory.StartNew(() => UpdatablePackagesForCheckForUpdates()));

            _mockCoapp.Setup(i => i.GetPackageSet(It.IsAny<string>())).Returns(
                (string i) => Task.Factory.StartNew(() => GetPackageSetForTestPackages(i)));


            _mockCoapp.Setup(i => i.GetUpgradablePackages()).Returns(() =>
                                                                     Task.Factory.StartNew(
                                                                         () => UpgradablePackagesForCheckForUpdates()));
            _mockCoapp.Setup(i => i.GetPackageDetails(It.IsAny<Package>())).Returns(
                (Package p) => Task.Factory.StartNew(
                    () => GetPackageDetails(p)));

            _mockCoapp.SetupGet(i => i.UpdateChoice).Returns(() => UpdateChoice.AutoInstallJustUpdates);


        }

        [TestMethod]
        public void CheckForUpdatesTestFaulted()
        {
            var timesReceivedSelectedProductsChangedMessage = 0;
            var changeLock = new object();
            Messenger.Default.Register<SelectedProductsChangedMessage>(this, (m) =>
            {
                lock (changeLock)
                {
                    timesReceivedSelectedProductsChangedMessage++;
                }
            });


            _mockCoapp.Setup(i => i.SetAllFeedsStale()).Returns(
                () => Task.Factory.StartNew(() => { throw new Exception(); }));

            Task c = _updateService.CheckForUpdates().ContinueWith(t => Assert.IsTrue(t.IsFaulted));
            c.Wait();

            _mockCoapp.Setup(i => i.SetAllFeedsStale()).Returns(() => Task.Factory.StartNew(() => { }));

            _mockCoapp.Setup(i => i.GetUpgradablePackages()).Returns(() =>
                                                                     Task.Factory.StartNew(
                                                                         () => UpgradablePackagesForCheckForUpdates()));
            _mockCoapp.Setup(i => i.GetUpdatablePackages()).Returns(
                () => Task<IEnumerable<Package>>.Factory.StartNew(() => { throw new Exception("1"); }));

            c = _updateService.CheckForUpdates().ContinueWith(t =>
                                                                  {
                                                                      Assert.IsTrue(t.IsFaulted);
                                                                      Assert.AreEqual("1", t.Exception.Unwrap().Message);
                                                                  });
            c.Wait();

            _mockCoapp.Setup(i => i.GetUpdatablePackages()).Returns(
                () => Task.Factory.StartNew(() => UpdatablePackagesForCheckForUpdates()));

            _mockCoapp.Setup(i => i.GetUpgradablePackages()).Returns(() =>
                                                                     Task<IEnumerable<Package>>.Factory.StartNew(() =>
                                                                                                                     {
                                                                                                                         throw
                                                                                                                             new Exception
                                                                                                                                 ("2");
                                                                                                                     }));
            c = _updateService.CheckForUpdates().ContinueWith(t =>
                                                                  {
                                                                      Assert.IsTrue(t.IsFaulted);

                                                                      Assert.AreEqual("2", t.Exception.Unwrap().Message);
                                                                  }
                );
            c.Wait();

            _mockCoapp.Setup(i => i.GetUpgradablePackages()).Returns(() =>
                                                                     Task.Factory.StartNew(
                                                                         () => UpgradablePackagesForCheckForUpdates()));
            _mockCoapp.Setup(i => i.GetPackageSet(It.IsAny<string>())).Returns(
                (string i) => Task<PackageSet>.Factory.StartNew(() => { throw new Exception("3"); }));

            c = _updateService.CheckForUpdates().ContinueWith(t =>
                                                                  {
                                                                      Assert.IsTrue(t.IsFaulted);
                                                                      Assert.AreEqual("3", t.Exception.Unwrap().Message);
                                                                  }
                );
            c.Wait();

            _mockCoapp.Setup(i => i.GetPackageSet(It.IsAny<string>())).Returns(
                (string i) => Task.Factory.StartNew(() => GetPackageSetForTestPackages(i)));


            _mockCoapp.Setup(i => i.GetPackageDetails(It.IsAny<Package>())).Returns(
                () => Task<Package>.Factory.StartNew(() => { throw new Exception("4"); }));

            c = _updateService.CheckForUpdates().ContinueWith(t =>
            {
                Assert.IsTrue(t.IsFaulted);
                Assert.AreEqual("4", t.Exception.Unwrap().Message);
            }
                );
            c.Wait();


            _mockCoapp.Setup(i => i.GetPackageDetails(It.IsAny<Package>())).Returns(
                (Package p) => Task.Factory.StartNew(
                    () => GetPackageDetails(p)));

            _mockCoapp.SetupGet(i => i.UpdateChoice).Throws(new Exception("5"));

            c = _updateService.CheckForUpdates().ContinueWith(t =>
            {
                Assert.IsTrue(t.IsFaulted);
                Assert.AreEqual("5", t.Exception.Unwrap().Message);
            }
                );
            
            c = c.ContinueWith(t =>
            {
                lock (changeLock)
                {
                    Assert.AreEqual(0, timesReceivedSelectedProductsChangedMessage);
                }
            });

            c.Wait();

        }

        private void VerifyFeedsAreStale(Times times)
        {
            _mockCoapp.Verify(i => i.SetAllFeedsStale(), times);
        }


        private IEnumerable<Package> UpgradablePackagesForCheckForUpdates()
        {
            return new[]
                       {
                           new TestPackage {CanonicalName = UPGRADABLE1},
                           new TestPackage {CanonicalName = UPGRADABLE_AND_UPDATABLE}
                       };
        }


        private IEnumerable<Package> UpdatablePackagesForCheckForUpdates()
        {
            return new[]
                       {
                           new TestPackage {CanonicalName = UPDATABLE1},
                           new TestPackage {CanonicalName = UPGRADABLE_AND_UPDATABLE}
                       };
        }

        private PackageSet GetPackageSetForTestPackages(string canonicalName)
        {
            switch (canonicalName)
            {
                case UPGRADABLE1:
                    return new PackageSet
                               {
                                   AvailableNewer =
                                       new TestPackage
                                           {
                                               CanonicalName = "Upgrade1",
                                               DisplayName = "Upgrade 1",
                                               Summary = "Upgrade 1",
                                               PublishDate = DateTime.Now.ToString()
                                           }
                               };
                case "Updatable1":
                    return new PackageSet
                               {
                                   AvailableNewerCompatible =
                                       new TestPackage
                                           {
                                               CanonicalName = "Update1",
                                               DisplayName = "Update 1",
                                               Summary = "Update 1",
                                               PublishDate = DateTime.Today.ToString()
                                           }
                               };
                case UPGRADABLE_AND_UPDATABLE:

                    return new PackageSet
                               {
                                   AvailableNewer =
                                       new TestPackage
                                           {
                                               CanonicalName = "Upgrade2",
                                               DisplayName = "Upgrade 2",
                                               Summary = "Upgrade 2",
                                               PublishDate = DateTime.Now.ToString()
                                           },
                                   AvailableNewerCompatible =
                                       new TestPackage
                                           {
                                               CanonicalName = "Update2",
                                               DisplayName = "Update 2",
                                               Summary = "Update 2",
                                               PublishDate = DateTime.Today.ToString()
                                           }
                               };
            }

            throw new Exception();
        }

        private Package GetPackageDetails(Package p)
        {
            switch (p.CanonicalName)
            {
                case "Upgrade1":
                    return new TestPackage
                               {
                                   CanonicalName = "Upgrade1",
                                   DisplayName = "Upgrade 1",
                                   Summary = "Upgrade 1",
                                   PublishDate = DateTime.Now.ToString()
                               };
                case "Update1":
                    return new TestPackage
                               {
                                   CanonicalName = "Update1",
                                   DisplayName = "Update 1",
                                   Summary = "Update 1",
                                   PublishDate = DateTime.Today.ToString()
                               };
                case "Upgrade2":
                    return new TestPackage
                               {
                                   CanonicalName = "Upgrade2",
                                   DisplayName = "Upgrade 2",
                                   Summary = "Upgrade 2",
                                   PublishDate = DateTime.Now.ToString()
                               };
                case "Update2":
                    return new TestPackage
                               {
                                   CanonicalName = "Update2",
                                   DisplayName = "Update 2",
                                   Summary = "Update 2",
                                   PublishDate = DateTime.Today.ToString()
                               };
            }

            throw new Exception();
        }

        /// <summary>
        ///A test for BlockProduct
        ///</summary>
        [TestMethod]
        public void BlockProductTest()
        {
            var testProduct = new Product();
            testProduct.OldId = "test";
            testProduct.NewId = "test_update";

            _mockCoapp.Setup(i => i.BlockPackage("test_update")).Returns(Task.Factory.StartNew(() => { }));

            Task blocktest = _updateService.BlockProduct(testProduct);
            Task c = blocktest.
                ContinueWith(t =>
                                 {
                                     Assert.IsFalse(t.IsFaulted);
                                     _mockCoapp.Verify(i => i.BlockPackage("test_update"), Times.Once());
                                     _mockCoapp.Verify(i => i.BlockPackage("test"), Times.Never());
                                 }
                );
            c.Wait();
        }

        [TestMethod]
        public void BlockProductFailed()
        {
            var testProduct = new Product();
            testProduct.OldId = "test";
            testProduct.NewId = "test_update";

            _mockCoapp.Setup(i => i.BlockPackage("test_update")).Returns(
                () => Task.Factory.StartNew(() => { throw new Exception(); }));
            Task blocktest = _updateService.BlockProduct(testProduct);
            Task c = blocktest.
                ContinueWith(t => Assert.IsTrue(t.IsFaulted));
        }

        /// <summary>
        ///A test for AutoTrim
        ///</summary>
        [TestMethod]
        public void AutoTrimTestTrue()
        {
            _mockCoapp.Setup(i => i.TrimOnUpdate).Returns(() => true);

            Task<bool> actual = _updateService.AutoTrim;
            Assert.AreEqual(true, actual.Result);
        }

        /// <summary>
        ///A test for AutoTrim
        ///</summary>
        [TestMethod]
        public void AutoTrimTestFalse()
        {
            _mockCoapp.Setup(i => i.TrimOnUpdate).Returns(() => false);

            Task<bool> actual = _updateService.AutoTrim;
            Assert.AreEqual(false, actual.Result);
        }


        /// <summary>
        ///A test for AutoTrim
        ///</summary>
        [TestMethod]
        public void AutoTrimTestFaulted()
        {
            _mockCoapp.Setup(i => i.TrimOnUpdate).Throws<Exception>();

            Task<bool> actual = _updateService.AutoTrim;
            Task task = actual.ContinueWith(t => Assert.AreEqual(true, t.IsFaulted));
            task.Wait();
        }

    }
}