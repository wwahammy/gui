using System;
using System.Security.Principal;
using System.Threading.Tasks;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CoApp.Gui.Test.Model
{
    
    
    /// <summary>
    ///This is a test class for PolicyServiceTest and is intended
    ///to contain all PolicyServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PolicyServiceTest
    {

        private TestContext testContextInstance;

        private PolicyService policy;
        private Mock<IWindowsUserService> userMock;
        private Mock<ICoAppService> coappMock;

        private IIdentity AdminUser = new GenericIdentity("blah");
        private IIdentity NonAdminUser = new GenericIdentity("blah1");
        private IIdentity UserWhoCanRemoveOnly = new GenericIdentity("blah2");

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
        [TestInitialize()]
        public void MyTestInitialize()
        {
           policy = new PolicyService();
            coappMock = CoAppMock();
            policy.CoApp = coappMock.Object;

            userMock = UserServiceMock();
            policy.UserService = userMock.Object;
        }


        private Mock<ICoAppService> CoAppMock()
        {
            var mock = new Mock<ICoAppService>();

            mock.Setup(c => c.RemovePrincipalFromPolicy(It.IsAny<PolicyType>(), "works")).Returns(
                () => Task.Factory.StartNew(() => { }));
            
            mock.Setup(c => c.RemovePrincipalFromPolicy(It.IsAny<PolicyType>(), "fails")).Returns(
                () => Task.Factory.StartNew(() => {throw new Exception(); }));

            mock.Setup(c => c.GetPolicy(It.IsAny<PolicyType>())).
                Returns((PolicyType p) => Task.Factory.StartNew(() => CreateResult(p)));
            
            return mock;
        }


        private Mock<IWindowsUserService> UserServiceMock()
        {
            var mock = new Mock<IWindowsUserService>();



            mock.Setup(u => u.GetCurrentUser()).Returns(() => NonAdminUser);
            /*
            mock.Setup(u => u.GetUserSid(NonAdminUser)).Returns(
                () =>
                    {
                        var wrapper = new Mock<SidWrapper>();

                    }

                );
            mock.Setup(u => u.GetUserSid(AdminUser)).Returns(() => new SecurityIdentifier("AdminUser"));
            mock.Setup(u => u.GetUserSid(UserWhoCanRemoveOnly)).Returns(
                () => new SecurityIdentifier("UserWhoCanRemoveOnly"));
            */
            return mock;
        }
        

        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


       
       

        /// <summary>
        ///A test for SetActivePolicy
        ///</summary>
        [TestMethod()]
        public void SetActivePolicyTest()
        {
            PolicyService target = policy;
            PolicyResult result = new PolicyResult(); // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = target.SetActivePolicy(result);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetBlockPolicy
        ///</summary>
        [TestMethod()]
        public void SetBlockPolicyTest()
        {
            PolicyService target = policy;
            PolicyResult result = new PolicyResult(); // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = target.SetBlockPolicy(result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetFreezePolicy
        ///</summary>
        [TestMethod()]
        public void SetFreezePolicyTest()
        {
            PolicyService target = policy;
            PolicyResult result = new PolicyResult(); // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = target.SetFreezePolicy(result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetInstallPolicy
        ///</summary>
        [TestMethod()]
        public void SetInstallPolicyTest()
        {
            PolicyService target = policy;
            PolicyResult result = new PolicyResult(); // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = target.SetInstallPolicy(result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetRemovePolicy
        ///</summary>
        [TestMethod()]
        public void SetRemovePolicyTest()
        {
            policy.SetRemovePolicy(PolicyResult.Everyone);


        }

        /// <summary>
        ///A test for SetRequirePolicy
        ///</summary>
        [TestMethod()]
        public void SetRequirePolicyTest()
        {
            PolicyService target = policy;
            PolicyResult result = new PolicyResult(); // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = target.SetRequirePolicy(result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetSessionFeedsPolicy
        ///</summary>
        [TestMethod()]
        public void SetSessionFeedsPolicyTest()
        {
            PolicyService target = policy;
            PolicyResult result = new PolicyResult(); // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = target.SetSessionFeedsPolicy(result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetSystemFeedsPolicy
        ///</summary>
        [TestMethod()]
        public void SetSystemFeedsPolicyTest()
        {
            PolicyService target = policy;
            PolicyResult result = new PolicyResult(); // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = target.SetSystemFeedsPolicy(result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetUpdatePolicy
        ///</summary>
        [TestMethod()]
        public void SetUpdatePolicyTest()
        {
            PolicyService target = policy;
            PolicyResult result = new PolicyResult(); // TODO: Initialize to an appropriate value
            Task expected = null; // TODO: Initialize to an appropriate value
            Task actual;
            actual = target.SetUpdatePolicy(result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }


        /// <summary>
        ///A test for ActivePolicy
        ///</summary>
        [TestMethod()]
        public void ActivePolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.ActivePolicy;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BlockPolicy
        ///</summary>
        [TestMethod()]
        public void BlockPolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.BlockPolicy;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanBlock
        ///</summary>
        [TestMethod()]
        public void CanBlockTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanBlock;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanChangeSettings
        ///</summary>
        [TestMethod()]
        public void CanChangeSettingsTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanChangeSettings;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanFreeze
        ///</summary>
        [TestMethod()]
        public void CanFreezeTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanFreeze;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanInstall
        ///</summary>
        [TestMethod()]
        public void CanInstallTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanInstall;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanRemove
        ///</summary>
        [TestMethod()]
        public void CanRemoveTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanRemove;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanRequire
        ///</summary>
        [TestMethod()]
        public void CanRequireTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanRequire;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSetActive
        ///</summary>
        [TestMethod()]
        public void CanSetActiveTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanSetActive;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSetSessionFeeds
        ///</summary>
        [TestMethod()]
        public void CanSetSessionFeedsTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanSetSessionFeeds;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanSetSystemFeeds
        ///</summary>
        [TestMethod()]
        public void CanSetSystemFeedsTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanSetSystemFeeds;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanUpdate
        ///</summary>
        [TestMethod()]
        public void CanUpdateTest()
        {
            PolicyService target = policy;
            Task<bool> actual;
            actual = target.CanUpdate;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FreezePolicy
        ///</summary>
        [TestMethod()]
        public void FreezePolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.FreezePolicy;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for InstallPolicy
        ///</summary>
        [TestMethod()]
        public void InstallPolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.InstallPolicy;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RemovePolicy
        ///</summary>
        [TestMethod()]
        public void RemovePolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.RemovePolicy;
            actual.Wait();
            //was the correct thing called?
            coappMock.Verify(c => c.GetPolicy(PolicyType.RemovePackage));
            
            

        }

        /// <summary>
        ///A test for RequirePolicy
        ///</summary>
        [TestMethod()]
        public void RequirePolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.RequirePolicy;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SessionFeedsPolicy
        ///</summary>
        [TestMethod()]
        public void SessionFeedsPolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.SessionFeedsPolicy;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SystemFeedsPolicy
        ///</summary>
        [TestMethod()]
        public void SystemFeedsPolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.SystemFeedsPolicy;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdatePolicy
        ///</summary>
        [TestMethod()]
        public void UpdatePolicyTest()
        {
            PolicyService target = policy;
            Task<PolicyResult> actual;
            actual = target.UpdatePolicy;

            actual.Wait();
            

            var policyResult = actual.Result;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UserName
        ///</summary>
        [TestMethod()]
        public void UserNameTest()
        {
            PolicyService target = policy;
            string actual;
            actual = target.UserName;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        private void SetBadMockForCoApp()
        {
            var mock = new Mock<ICoAppService>();
            mock.Setup((c) => c.GetPolicy(It.IsAny<PolicyType>())).
                Returns(
                () => Task<PolicyQueryResult>.Factory.StartNew(() => { throw new Exception(); }));

            policy.CoApp = mock.Object;
            coappMock = mock;
        }

        private PolicyQueryResult CreateResult(PolicyType p)
        {
            if (p == PolicyType.UpdatePackage)
            {
                return new PolicyQueryResult {NameOfPolicy = p.ToString(), GroupsInPolicy = new[] {"Everyone"}};
            }

            return new PolicyQueryResult {NameOfPolicy = p.ToString(), GroupsInPolicy = new[] {"Administrators"}};

        }

        
        
    }
}
