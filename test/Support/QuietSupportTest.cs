using System;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.Support;
using FluentDateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoApp.Gui.Test.Support
{
    
    
    /// <summary>
    ///This is a test class for QuietSupportTest and is intended
    ///to contain all QuietSupportTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QuietSupportTest
    {
        private readonly DateTime TEST_DATE = new DateTime(2000, 9, 9);
        private const DayOfWeek TEST_DAY_OF_WEEK = DayOfWeek.Saturday;

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for WhenLastRunShouldHaveBeen
        ///</summary>
        [TestMethod()]
        public void WhenLastRunShouldHaveBeenTestEarlierToday()
        {

            QuietSupport target = new QuietSupport();
       
            UpdateTimeAndDay scheduledTimeAndDay = new UpdateTimeAndDay {DayOfWeek = UpdateDayOfWeek.Saturday, Time = 3};

            DateTime expected = TEST_DATE.AddHours(3);
            DateTime actual;
            actual = target.WhenLastRunShouldHaveBeen(scheduledTimeAndDay, TEST_DATE.Noon());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void WhenLastRunShouldHaveBeenTestEarlierTodayEveryday()
        {

            QuietSupport target = new QuietSupport();
       
            UpdateTimeAndDay scheduledTimeAndDay = new UpdateTimeAndDay {DayOfWeek = UpdateDayOfWeek.Everyday, Time = 3};

            DateTime expected = TEST_DATE.AddHours(3);
            DateTime actual;
            actual = target.WhenLastRunShouldHaveBeen(scheduledTimeAndDay, TEST_DATE.Noon());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for WhenLastRunShouldHaveBeen
        ///</summary>
        [TestMethod()]
        public void WhenLastRunShouldHaveBeenTestYesterday()
        {

            QuietSupport target = new QuietSupport();

            UpdateTimeAndDay scheduledTimeAndDay = new UpdateTimeAndDay { DayOfWeek = UpdateDayOfWeek.Friday, Time = 3 };

            DateTime expected = TEST_DATE.Subtract(TimeSpan.FromDays(1)).AddHours(3);
            DateTime actual;
            actual = target.WhenLastRunShouldHaveBeen(scheduledTimeAndDay, TEST_DATE.Noon());

            Assert.AreEqual(expected, actual);
        }




        /// <summary>
        ///A test for WhenLastRunShouldHaveBeen
        ///</summary>
        [TestMethod()]
        public void WhenLastRunShouldHaveBeenTestSameDayEarlierWeek()
        {

            QuietSupport target = new QuietSupport();

            UpdateTimeAndDay scheduledTimeAndDay = new UpdateTimeAndDay { DayOfWeek = UpdateDayOfWeek.Saturday, Time = 15};

            DateTime expected = TEST_DATE.Subtract(TimeSpan.FromDays(7)).AddHours(15);
            DateTime actual;
            actual = target.WhenLastRunShouldHaveBeen(scheduledTimeAndDay, TEST_DATE.Noon());

            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for WhenLastRunShouldHaveBeen
        ///</summary>
        [TestMethod()]
        public void WhenLastRunShouldHaveBeenTestEverydayEarlierDay()
        {

            QuietSupport target = new QuietSupport();

            UpdateTimeAndDay scheduledTimeAndDay = new UpdateTimeAndDay { DayOfWeek = UpdateDayOfWeek.Everyday, Time = 15 };

            DateTime expected = TEST_DATE.PreviousDay().AddHours(15);
            DateTime actual;
            actual = target.WhenLastRunShouldHaveBeen(scheduledTimeAndDay, TEST_DATE.Noon());

            Assert.AreEqual(expected, actual);
        }

       
    }
}
