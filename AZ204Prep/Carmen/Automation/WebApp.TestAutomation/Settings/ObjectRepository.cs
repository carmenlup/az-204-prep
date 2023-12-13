using OpenQA.Selenium;
using WebApp.TestAutomation.Config;

namespace WebApp.TestAutomation.Settings
{
    public class ObjectRepository
    {
        public static BrowserType Browser { get; set; }
        public static IWebDriver Driver { get; set; }
    }
}
