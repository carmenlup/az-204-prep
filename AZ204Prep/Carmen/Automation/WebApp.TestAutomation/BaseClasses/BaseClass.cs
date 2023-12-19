using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Reflection;
using WebApp.TestAutomation.Config;
using WebApp.TestAutomation.CustomException;
using WebApp.TestAutomation.Settings;
using Product = WebApp.TestAutomation.DbModel.Product;

namespace WebApp.TestAutomation.BaseClasses
{
    [TestClass]
    public class BaseClass
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IWebDriver _driver { get; set; }

        public static IConfigurationRoot  InitConfigurationRoot()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();
            return Configuration;
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

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _driver = InitWebDriver();
            Configuration = InitConfigurationRoot();
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
