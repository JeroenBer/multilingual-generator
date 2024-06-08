using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Config;
public interface IConfigReader
{
    MultilingualConfig ReadConfig(string filePath);
}
