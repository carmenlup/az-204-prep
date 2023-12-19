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
        [TestMethod]
        public void TestOpenPage()
        {
            _driver.NavigateToUrl(Configuration["Website"]!);
        }
    }
}