using MultillingualFileGenerator.Config;
using MultillingualFileGenerator.Targets.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Targets
{
    public class TargetWriterFactory
    {
        public ITargetWriter GetTargetWriter(FileFormat fileFormat)
        {
            return fileFormat switch
            {
                FileFormat.Android => new AndroidTargetWriter(),
                FileFormat.Apple => new AppleTargetWriter(),
                FileFormat.Windows => new WindowsTargetWriter(),
                _ => throw new NotImplementedException($"Unimplemented file format {fileFormat}")
            };
        }
    }
}
