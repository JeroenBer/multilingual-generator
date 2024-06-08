using MultillingualFileGenerator.Targets.Model;
using MultillingualFileGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Targets
{
    public interface ITargetWriter
    {
        void Write(string targetFilePath, List<TargetLine> targetLines);
    }
}
