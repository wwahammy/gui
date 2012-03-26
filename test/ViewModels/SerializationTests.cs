using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CoApp.Updater.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoApp.Gui.Test.ViewModels
{
    [TestClass]
    public class SerializationTests
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
        [TestCleanup()]
        public void MyTestCleanup()
        {
         
        }


        [TestMethod]
        public void PrimaryViewModel()
        {
            var xml = new XmlSerializer(typeof(PrimaryViewModel));
            var p = new PrimaryViewModel();
            XDocument doc = new XDocument();
            using (var w = doc.CreateWriter())
            {
                xml.Serialize(w, p);
            }


        }
    }
}
