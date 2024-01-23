using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;

namespace WorkTests
{
    /// <summary>
    /// Test account creation/ email validation / password reset
    /// </summary>
    [TestClass]
    public class UserAccountTest
    {
        public UserAccountTest()
        {
            
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AccountEmailVerification_CreateEmail_ShouldReturnTrue()
        {
            User user = new User();
            user.Email = "johnnybravo@localhost.localdomain";
            user.EmailVerificationGuid = Guid.NewGuid();
            user.FirstName = "Johhny";
            user.LastName = "Bravo";
            XsltTemplating templating = new XsltTemplating();

            Dictionary<string, string> stringParameters = new Dictionary<string, string>();
            stringParameters.Add("VerificationLink", "http://google.com");
            string result = templating.GetTransformedTemplate(XsltTemplating.TemplatePath.Email, Email.EmailTemplates.VerifyEmail.ToString(), stringParameters, user);

            Assert.AreEqual<bool>(true, result.Contains("Johhny"));
        }
    }
}
