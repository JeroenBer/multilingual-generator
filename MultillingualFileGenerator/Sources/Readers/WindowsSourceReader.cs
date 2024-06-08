using MultillingualFileGenerator.FileFormats;
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
    internal class WindowsSourceReader : ISourceReader
    {
        private XmlSerializer _xmlSerializer = new XmlSerializer(typeof(WindowsResourceRoot));

        public IndexedList<SourceLine> Read(string sourceFilePath)
        {
            using (var fs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            {
                var windowsResources = (WindowsResourceRoot)_xmlSerializer.Deserialize(fs);

                var sourceLines = windowsResources.Data.Select(x => new SourceLine
                {
                    Name = x.Name,
                    Value = x.Value,
                    Comment = x.Comment,
                }).ToList();

                var indexedSourceList = new IndexedList<SourceLine>(sl => sl.Name);
                foreach (var sourceLine in sourceLines)
                {
                    indexedSourceList.Add(sourceLine);
                }

                return indexedSourceList;
            }
        }
    }
}
