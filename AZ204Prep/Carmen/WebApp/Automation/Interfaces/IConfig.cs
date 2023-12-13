using Automation.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Interfaces
{
    public interface IConfig
    {
        BrowserType GetBrowser();
        string GetWebsite();
        string GetDatabaseConnectionString();
    }
}
