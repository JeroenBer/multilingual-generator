using MultillingualFileGenerator.FileFormats;
using MultillingualFileGenerator.Sources;
using MultillingualFileGenerator.Sources.Model;
using MultillingualFileGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.Sources.Readers
{
    // See https://developer.apple.com/library/archive/documentation/Cocoa/Conceptual/LoadingResources/Strings/Strings.html
    // TODO Support for comments
    internal class AppleSourceReader : ISourceReader
    {
        private const string IsSign = "=";
        private const string DoubleQuote = "\"";
        private const string LineQuoteEnd = "\";";
        private const string NewLine = "\n";

        public IndexedList<SourceLine> Read(string sourceFilePath)
        {
            var indexedSourceList = new IndexedList<SourceLine>(sl => sl.Name);

            using (var fs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (line.Trim() == string.Empty)
                        {
                            continue;
                        }

                        // Check for multiline
                        while (!sr.EndOfStream && !line.TrimEnd().EndsWith(LineQuoteEnd))
                        {
                            var extraLine = sr.ReadLine();
                            line = line + NewLine + extraLine;
                        }

                        var sourceLine = ParseSourceLine(line);
                        indexedSourceList.Add(sourceLine);
                    }
                }
            }

            return indexedSourceList;
        }

        private SourceLine ParseSourceLine(string line)
        {
            var splitIndex = line.IndexOf(IsSign);
            if (splitIndex < 0)
                throw new Exception("= not found in Apple source file");

            var rawName = line.Substring(0, splitIndex).Trim();
            var rawValue = line.Substring(splitIndex + 1).Trim();

            if (!rawName.StartsWith(DoubleQuote))
                throw new Exception("Name should start with double quote");
            if (!rawName.EndsWith(DoubleQuote))
                throw new Exception("Name should end with double quote");
            if (!rawValue.StartsWith(DoubleQuote))
                throw new Exception("Value should start with double quote");
            if (!rawValue.EndsWith(LineQuoteEnd))
                throw new Exception("Value should end with double quote and comma");

            var name = rawName.Substring(1, rawName.Length - 2);
            var value = rawValue.Substring(1, rawValue.Length - 3);

            return new SourceLine
            {
                Name = name,
                Value = value,
            };
        }
    }
}
