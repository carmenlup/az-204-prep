using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.Data;
using WebApp.TestAutomation.BaseClasses;
using WebApp.TestAutomation.Helpers;
using WebApp.TestAutomation.Settings;

namespace WebApp.TestAutomation
{
    [TestClass]
    public class UnitTest1 : BaseClass
    {
        [TestMethod]
        public void TestMainPage()
        {
            Driver.NavigateToUrl(Configuration["Website"]!);
        }

        [TestMethod]
        public void TestDbValues()
        {
            var list = new DatabaseConnection(Configuration).GetProduct();
            Assert.AreEqual(3, list.Count);

        }

    }
}