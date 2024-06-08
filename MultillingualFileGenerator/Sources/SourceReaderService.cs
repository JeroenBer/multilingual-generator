using MultillingualFileGenerator.Config;
using MultillingualFileGenerator.Sources.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Sources
{
    public class SourceReaderService
    {
        private readonly SourceReaderFactory _sourceReaderFactory;

        public SourceReaderService(SourceReaderFactory sourceReaderFactory)
        {
            _sourceReaderFactory = sourceReaderFactory;
        }

        public SourceInput GetSourceInput(SourceSettings sourceSettings, string workingDir)
        {
            var sourceFilePath = Path.Combine(workingDir, sourceSettings.SourceFile);

            var sourceReader = _sourceReaderFactory.GetSourceReader(sourceSettings.SourceFileFormat);

            var sourceLines = sourceReader.Read(sourceFilePath);

            return new SourceInput(
                sourceSettings.SourceFile,
                sourceSettings.SourceLanguage,
                sourceLines,
                sourceSettings.ApplicationName);
        }
    }
}
