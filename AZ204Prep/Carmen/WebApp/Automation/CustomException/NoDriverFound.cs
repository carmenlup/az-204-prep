using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.CustomException
{
    public class NoDriverFound : Exception
    {
        public NoDriverFound(string message) : base(message)
        {

        }
    }
}
