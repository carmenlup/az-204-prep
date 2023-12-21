using OpenQA.Selenium;

namespace WebApp.TestAutomation.Helpers
{
    public static class NavigationHelper
    {
        /// <summary>
        /// This method allows the driver to navigate to the specified URL and handles any exceptions that may occur.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="url"></param>
        public static void NavigateToUrl(this IWebDriver driver, string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
            }
            catch (WebDriverException ex)
            {
                // Handle navigation failure
                throw new Exception("Navigation failed: " + ex.Message);
            }
            catch (UriFormatException ex)
            {
                // Handle invalid URL
                throw new Exception("Invalid URL: " + ex.Message);
            }
        }
    }
}
