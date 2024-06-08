using MultillingualFileGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Sources.Model
{
    public class SourceInput
    {
        public string RelativeSourcePath { get; private set; }
        public string SourceLanguage { get; private set; }
        public IndexedList<SourceLine> Lines { get; private set; }
        public string ApplicationName {  get; private set; }

        public SourceInput(string relativeSourcePath, string sourceLanguage, IndexedList<SourceLine> lines, string applicationName)
        {
            RelativeSourcePath = relativeSourcePath;
            SourceLanguage = sourceLanguage;
            Lines = lines;
            ApplicationName = applicationName;
        }
    }
}
