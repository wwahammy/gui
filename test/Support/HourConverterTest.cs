using System;
using System.Globalization;
using CoApp.Updater.Support;
using CoApp.Updater.Support.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoApp.Gui.Test.Support
{
    
    
    /// <summary>
    ///This is a test class for HourConverterTest and is intended
    ///to contain all HourConverterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HourConverterTest
    {


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
        ///A test for Convert
        ///</summary>
        [TestMethod()]
        public void ConvertTest()
        {
            var target = new HourConverter(); 
            object value = 1;
            object expected = "1 AM";
            object actual = target.Convert(value, null, null, null);
            Assert.AreEqual(expected, actual, "1 was not converted to 1 AM");

            value = 0;
            expected = "12 AM";
            actual = target.Convert(value, null, null, null);
            Assert.AreEqual(expected, actual, "0 was not converted to 12 AM");

            value = 13;
            expected = "1 PM";
            actual = target.Convert(value, null, null, null);
            Assert.AreEqual(expected, actual, "13 was not converted to 1 PM");
        }

        [TestMethod()]
        public void ConvertErrorsTest()
        {
            var target = new HourConverter(); 
            object value = "not valid";
            object expected = "not valid";
            object actual = target.Convert(value, null, null, null);
            Assert.AreEqual(expected, actual);

            value = null;
            expected = null;
            actual = target.Convert(value, null, null, null);
            Assert.AreEqual(expected, actual);


            value = -1;
            expected = -1;
            actual = target.Convert(value, null, null, null);
            Assert.AreEqual(expected, actual);

            value = 24;
            expected = 24;
            actual = target.Convert(value, null, null, null);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for ConvertBack
        ///</summary>
        [TestMethod()]
        public void ConvertBackTest()
        {
            HourConverter target = new HourConverter(); 
            object value = "12 AM"; 
            Type targetType = null; 
            object parameter = null; 
            CultureInfo culture = null; 
            object expected = 0; 
            object actual;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);


            value = "1 AM";
            expected = 1;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);

            value = "1 PM";
            expected = 13;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertBackErrors()
        {
            var target = new HourConverter(); 
            object value = "13 AM";
            object expected = "13 AM";
            object actual = target.ConvertBack(value, null, null, null);

            Assert.AreEqual(expected, actual);

            value = "1";
            expected = "1";
            actual = target.ConvertBack(value, null, null, null);
            Assert.AreEqual(expected, actual);

            value = null;
            expected = null;
            actual = target.ConvertBack(value, null, null, null);
            Assert.AreEqual(expected, actual);

            value = "ghalkghalgha";
            expected = "ghalkghalgha";
            actual = target.ConvertBack(value, null, null, null);
            Assert.AreEqual(expected, actual);
        }
    }
}
