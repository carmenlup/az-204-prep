using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebApp.TestAutomation.Config;
using WebApp.TestAutomation.CustomException;
using WebApp.TestAutomation.Settings;

namespace WebApp.TestAutomation.BaseClasses
{
    public class BaseClass
    {
        public static IConfiguration Congfiguration { get; set; }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }

        private static IWebDriver GetChromeDriver()
        {
            IWebDriver driver = new ChromeDriver();
            return driver;
        }

        private static IWebDriver GetFireFoxDriver()
        {
            IWebDriver driver = new FirefoxDriver();
            return driver;
        }

        public static IWebDriver InitWebDriver()
        {
            Congfiguration = InitConfiguration();
            switch (ObjectRepository.Browser)
            {
                case BrowserType.Chrome:
                    ObjectRepository.Driver = GetChromeDriver();
                    break;
                case BrowserType.Firefox:
                    ObjectRepository.Driver = GetFireFoxDriver();
                    break;
                default:
                    throw new NoDriverFound("Driver not found: " + Congfiguration["Browser"]);
            }

            return ObjectRepository.Driver;
        }
    }
}
