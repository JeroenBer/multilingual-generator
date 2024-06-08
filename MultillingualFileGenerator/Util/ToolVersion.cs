using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Util
{
    internal class ToolVersion
    {
        public static string GetVersion()
            => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
