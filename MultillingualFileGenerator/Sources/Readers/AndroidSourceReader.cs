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
    internal class AndroidSourceReader : ISourceReader
    {
        private XmlSerializer _xmlSerializer = new XmlSerializer(typeof(AndroidResources));

        public IndexedList<SourceLine> Read(string sourceFilePath)
        {
            using (var fs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            {
                var androidResources = (AndroidResources)_xmlSerializer.Deserialize(fs);

                var sourceLines = androidResources.AndroidStrings.Select(x => new SourceLine
                {
                    Name = x.Name,
                    Value = x.Text
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
