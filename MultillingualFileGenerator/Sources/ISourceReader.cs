using MultillingualFileGenerator.Sources.Model;
using MultillingualFileGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Sources
{
    public interface ISourceReader
    {
        IndexedList<SourceLine> Read(string sourceFilePath);
    }
}
