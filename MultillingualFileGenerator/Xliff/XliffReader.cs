using MultillingualFileGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.Xliff
{
    public class XliffReader
    {
        private static readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(XliffFile));

        public IndexedList<XliffTransUnitElement> Read(string xliffFilePath)
        {
            var indexedXliffTransUnitElements = new IndexedList<XliffTransUnitElement>(e => e.ID);

            using (var fs = new FileStream(xliffFilePath, FileMode.Open, FileAccess.Read))
            {
                var xliffFile = (XliffFile)_xmlSerializer.Deserialize(fs);

                var elements = xliffFile.File.SelectMany(f => f.Body.Group.TransUnits ?? []);

                foreach(var element in elements)
                {
                    indexedXliffTransUnitElements.Add(element);
                }

                return indexedXliffTransUnitElements;
            }
        }
    }
}
