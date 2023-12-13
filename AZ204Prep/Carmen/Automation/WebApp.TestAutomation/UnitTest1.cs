using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using WebApp.TestAutomation.BaseClasses;
using WebApp.TestAutomation.Config;
using WebApp.TestAutomation.Helpers;
using WebApp.TestAutomation.Settings;

namespace WebApp.TestAutomation
{
    [TestClass]
    public class UnitTest1 : BaseClass 
    {
        private static  IWebDriver _driver = null;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _driver = InitWebDriver();
        }


        [TestMethod]
        public void TestOpenPage()
        {
            _driver.NavigateToUrl(Congfiguration["Website"]!);
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            if (_driver != null)
            {
                _driver.Close();
                _driver.Quit();
            }
        }
    }


}