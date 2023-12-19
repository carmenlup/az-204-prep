using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.Data;
using WebApp.Service;
using WebApp.TestAutomation.BaseClasses;
using WebApp.TestAutomation.Helpers;
using WebApp.TestAutomation.Settings;

namespace WebApp.TestAutomation
{
    [TestClass]
    public class UnitTest1 : BaseClass
    {
        [TestMethod]
        public void TestOpenPage()
        {

            //_driver.NavigateToUrl(Configuration["Website"]!);
            var list = _productService.GetProduct();
        }
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _driver = InitWebDriver();
        }
    }
}