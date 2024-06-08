using MultillingualFileGenerator.FileFormats;
using MultillingualFileGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.Xliff
{
    public class XliffWriter
    {
        private static readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(XliffFile));

        public void Write(string xliffFilePath, XliffFile xliffFile)
        {
            var indexedXliffTransUnitElements = new IndexedList<XliffTransUnitElement>(e => e.ID);

            using (var fs = new FileStream(xliffFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.SetLength(0);

                XmlWriterSettings xmlWriterSettings = new()
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                // use stream writer and reader, because we need to set the encoding (that will be part of the generated xml)

                using (var writer = XmlWriter.Create(fs, xmlWriterSettings))
                {
                    _xmlSerializer.Serialize(new ExtendedXmlWriter(writer), xliffFile);
                }
            }
        }
    }
}
