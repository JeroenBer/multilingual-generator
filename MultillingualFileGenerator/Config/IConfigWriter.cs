using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Config;
public interface IConfigWriter
{
    void WriteConfig(string filePath, MultilingualConfig config);
}
