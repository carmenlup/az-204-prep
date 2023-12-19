using OpenQA.Selenium;

namespace WebApp.TestAutomation.Helpers
{
    public static class NavigationHelper
    {
        public static void NavigateToUrl(this IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
        }
    }
}
