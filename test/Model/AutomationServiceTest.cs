using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoApp.Gui.Test.Model
{
    /// <summary>
    ///This is a test class for IAutomationServiceTest and is intended
    ///to contain all IAutomationServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class AutomationServiceTest
    {
        private readonly LocalServiceLocator _loc = new LocalServiceLocator();

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            dynamic auto = _loc.AutomationService;
            auto.Reset();
        }
        //

        #endregion

      
        /// <summary>
        ///A test for TurnOffAutomation
        ///</summary>
        [TestMethod]
        public void TurnOffAutomationTest()
        {
            IAutomationService target = _loc.AutomationService;// TODO: Initialize to an appropriate value
            Assert.IsFalse(target.IsAutomated);
            target.TurnOnAutomation();

            Assert.IsTrue(target.IsAutomated);
            var automationTurnedOffEventHappened = false;
            target.AutomationTurnedOff += () => automationTurnedOffEventHappened = true;

            target.TurnOnAutomation();

            Assert.IsTrue(target.IsAutomated);

            Assert.IsFalse(automationTurnedOffEventHappened);

            target.TurnOffAutomation();

            Assert.IsFalse(target.IsAutomated);

            Assert.IsTrue(automationTurnedOffEventHappened);


        }

        /// <summary>
        ///A test for TurnOnAutomation
        ///</summary>
        [TestMethod]
        public void TurnOnAutomationTest()
        {
            IAutomationService target = _loc.AutomationService;// TODO: Initialize to an appropriate value
            Assert.IsFalse(target.IsAutomated);
            target.TurnOnAutomation();

            Assert.IsTrue(target.IsAutomated);
            var automationTurnedOffEventHappened = false;
            target.AutomationTurnedOff += () => automationTurnedOffEventHappened = true;

            target.TurnOnAutomation();

            Assert.IsTrue(target.IsAutomated);

            Assert.IsFalse(automationTurnedOffEventHappened);
        }
    }
}