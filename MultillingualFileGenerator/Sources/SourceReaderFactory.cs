using MultillingualFileGenerator.Config;
using MultillingualFileGenerator.Sources.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Sources
{
    public class SourceReaderFactory
    {
        public ISourceReader GetSourceReader(FileFormat fileFormat)
        {
            return fileFormat switch
            {
                FileFormat.Android => new AndroidSourceReader(),
                FileFormat.Apple => new AppleSourceReader(),
                FileFormat.Windows => new WindowsSourceReader(),
                _ => throw new NotImplementedException($"Unimplemented file format {fileFormat}")
            };
        }
    }
}
