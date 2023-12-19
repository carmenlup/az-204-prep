using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebApp.TestAutomation.Config;
using WebApp.TestAutomation.CustomException;
using WebApp.TestAutomation.Settings;

namespace WebApp.TestAutomation.BaseClasses
{
   // [TestClass]
    public class BaseClass
    {
        public IConfiguration Configuration;
        public static IWebDriver Driver;

        public BaseClass()
        {
            Driver = InitWebDriver();
            Configuration = InitConfiguration();
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }

        private IWebDriver GetChromeDriver()
        {
            //TODO - run web proj for local env
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            return driver;
        }

        private static IWebDriver GetFireFoxDriver()
        {
            IWebDriver driver = new FirefoxDriver();
            return driver;
        }

        public IWebDriver InitWebDriver()
        {
            switch (ObjectRepository.Browser)
            {
                case BrowserType.Chrome:
                    ObjectRepository.Driver = GetChromeDriver();
                    break;
                case BrowserType.Firefox:
                    ObjectRepository.Driver = GetFireFoxDriver();
                    break;
                default:
                    throw new NoDriverFound("Driver not found: " + Configuration["Browser"]);
            }

            return ObjectRepository.Driver;
        }
    }
}
