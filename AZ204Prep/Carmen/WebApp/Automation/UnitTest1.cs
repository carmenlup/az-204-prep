using Automation.Config;
using Automation.Helpers;
using Automation.Interfaces;
using Automation.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;

namespace Automation
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestOpenPage()
        {
            NavigationHelper.NavigateToUrl(ObjectRepository.Config.GetWebsite());
        }
    }
}
