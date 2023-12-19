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
            //var DeviceDriver = ChromeDriverService.CreateDefaultService();
            //DeviceDriver.HideCommandPromptWindow = true;
            //ChromeOptions options = new ChromeOptions();
            //options.AddArguments("--disable-infobars");
            //options.AddArguments("incognito");
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

           // IWebDriver driver = new ChromeDriver();
            return driver;
        }

        private static IWebDriver GetFireFoxDriver()
        {
            IWebDriver driver = new FirefoxDriver();
            return driver;
        }

        public IWebDriver InitWebDriver()
        {
            //Configuration = InitConfiguration();
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

        /*[AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _driver = InitWebDriver();
        }*/

        /*[AssemblyCleanup]
        public static void TearDown()
        {
            if (Driver != null)
            {
                Driver.Close();
                Driver.Quit();
            }
        }*/
    }
}
