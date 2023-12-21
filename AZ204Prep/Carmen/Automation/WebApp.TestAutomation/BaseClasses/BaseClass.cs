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
        /// <summary>
        /// This property is responsible for holding the IConfigurationRoot object.
        /// </summary>
        public static IConfigurationRoot Configuration { get; set; }
        /// <summary>
        /// This property is responsible for holding the IWebDriver object.
        /// </summary>
        public static IWebDriver Driver { get; set; }

        /// <summary>
        /// This constructor is responsible for injecting the IConfigurationRoot object into the class.
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot  InitConfigurationRoot()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();
            return Configuration;
        }

        /// <summary>
        /// This method is responsible for creating and initializing a new instance of the ChromeDriver class.
        /// </summary>
        /// <returns>Returns an instance of the IWebDriver interface, specifically a ChromeDriver object.</returns>
        private static IWebDriver GetChromeDriver()
        {
            IWebDriver driver = new ChromeDriver();
            return driver;
        }

        /// <summary>
        /// This method is responsible for creating and initializing a new instance of the FirefoxDriver class.
        /// </summary>
        /// <returns>Returns an instance of the IWebDriver interface, specifically a FirefoxDriver object.</returns>
        private static IWebDriver GetFireFoxDriver()
        {
            IWebDriver driver = new FirefoxDriver();
            return driver;
        }

        /// <summary>
        /// This method is responsible for initializing and returning an instance of IWebDriver interface based on the value of the Browser property.
        /// </summary>
        /// <returns>Returns the ObjectRepository.Driver property, which holds the initialized driver.</returns>
        /// <exception cref="NoDriverFound"></exception>
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

        /// <summary>
        /// This method is responsible for initializing the driver and configuration objects.
        /// </summary>
        /// <param name="context"></param>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Driver = InitWebDriver();
            Configuration = InitConfigurationRoot();
        }

        /// <summary>
        /// This method ensures that the driver is closed and quit after the test run.
        /// </summary>
        [AssemblyCleanup]
        public static void TearDown()
        {
            if (Driver != null)
            {
                Driver.Close();
                Driver.Quit();
            }
        }
    }
}
