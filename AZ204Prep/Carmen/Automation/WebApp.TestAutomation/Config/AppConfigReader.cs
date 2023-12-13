using Automation.Interfaces;
using Automation.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Config
{
    public class AppConfigReader : IConfig
    {
        public BrowserType GetBrowser()
        {
            var config =  ConfigurationManager.AppSettings.Get(AppConfigKeys.Browser);
            string browser = ConfigurationManager.AppSettings[AppConfigKeys.Browser];
            return (BrowserType)Enum.Parse(typeof(BrowserType), browser);
        }

        public string GetWebsite()
        {
            return ConfigurationManager.AppSettings.Get(AppConfigKeys.Website);
        }

        public string GetDatabaseConnectionString()
        {
            return ConfigurationManager.AppSettings.Get(AppConfigKeys.Database);
        }
    }
}
