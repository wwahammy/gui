using System.Linq;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CoApp.Gui.Test.Model
{
    
    
    /// <summary>
    ///This is a test class for NavigationServiceTest and is intended
    ///to contain all NavigationServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NavigationServiceTest
    {

        LocalServiceLocator loc = new LocalServiceLocator();
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
        [TestCleanup()]
        public void MyTestCleanup()
        {
            dynamic nav = loc.NavigationService;

            try
            {
                nav.EmptyStack();
            }
            catch
            {
                
            }
        }

       
        #endregion



        [TestMethod()]
        public void GoToTest()
        {
            
            INavigationService target = loc.NavigationService; // TODO: Initialize to an appropriate value




            Mock<ScreenViewModel> viewModel = new Mock<ScreenViewModel>();
            Assert.IsNull(target.Current);
            Assert.AreEqual(true, target.StackEmpty);
            target.GoTo(viewModel.Object);
            viewModel.Verify((s) => s.FireLoad());
            //check for the gotomessages!!!

            Assert.AreSame(viewModel.Object, target.Current);

            Assert.AreEqual(true, target.StackEmpty);


            var newTest = new Mock<ScreenViewModel>();
            target.GoTo(newTest.Object);
            viewModel.Verify(s => s.FireUnload());

            newTest.Verify(s => s.FireLoad());

            //check for the gotomessages!!!

            Assert.AreSame(newTest.Object, target.Current);

            Assert.AreEqual(false, target.StackEmpty);
            Assert.AreEqual(1, target.Stack.Count);
            Assert.AreSame(viewModel.Object, target.Stack.First());


            target.Back();
            newTest.Verify(s => s.FireUnload());
             //check for the gotomessages!!!
            viewModel.Verify((s) => s.FireLoad(), Times.Exactly(2));

           Assert.AreSame(viewModel.Object, target.Current);

            Assert.AreEqual(true, target.StackEmpty);
            Assert.AreEqual(0, target.Stack.Count);


            
            

         
           
        }





        [TestMethod()]
        public void GoToTest2()
        {

            INavigationService target = loc.NavigationService; // TODO: Initialize to an appropriate value




            var viewModel = new Mock<ScreenViewModel>();
            
            Assert.IsNull(target.Current);
            Assert.AreEqual(true, target.StackEmpty);
            target.GoTo(viewModel.Object, false);

            viewModel.Verify((s) => s.FireLoad());

            //check for the gotomessages!!!

            Assert.AreSame(viewModel.Object, target.Current);

            Assert.AreEqual(true, target.StackEmpty);


            var newTest = new Mock<ScreenViewModel>();
            target.GoTo(newTest.Object);
            newTest.Verify((s) => s.FireLoad());

            viewModel.Verify((s) => s.FireUnload());

            //check for the gotomessages!!!

            Assert.AreSame(newTest.Object, target.Current);

            Assert.AreEqual(true, target.StackEmpty);
            Assert.AreEqual(0, target.Stack.Count);
            //Assert.AreSame(viewModel, target.Stack.First());


            var test2 = new Mock<ScreenViewModel>();
            target.GoTo(test2.Object, false);

            test2.Verify(s => s.FireLoad());
            newTest.Verify((s) => s.FireUnload());

            Assert.AreSame(test2.Object, target.Current);


            Assert.AreEqual(false, target.StackEmpty);
            Assert.AreEqual(1, target.Stack.Count);
            Assert.AreSame(newTest.Object, target.Stack.First());

            var test3 = new Mock<ScreenViewModel>();
            target.GoTo(test3.Object);

            test2.Verify(s => s.FireUnload());
            test3.Verify(s => s.FireLoad());

            Assert.AreSame(test3.Object, target.Current);

            Assert.AreEqual(false, target.StackEmpty);
            Assert.AreEqual(1, target.Stack.Count);
            Assert.AreSame(newTest.Object, target.Stack.First());

            target.Back();
            test3.Verify(s => s.FireUnload());
            newTest.Verify(s => s.FireLoad(), Times.Exactly(2));

            Assert.AreSame(newTest.Object, target.Current);

            Assert.IsTrue(target.StackEmpty);
            Assert.AreEqual(0, target.Stack.Count);





            //target.GoTo(viewModel);
            //target.





        }

       
    }
}
