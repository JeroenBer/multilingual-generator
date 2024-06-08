using MultillingualFileGenerator.FileFormats;
using MultillingualFileGenerator.Targets.Model;
using MultillingualFileGenerator.Util;
using MultillingualFileGenerator.Xliff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.Targets.Writers
{
    internal class WindowsTargetWriter : ITargetWriter
    {
        private static readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(WindowsResourceRoot));

        public void Write(string targetFilePath, List<TargetLine> targetLines)
        {
            var androidResource = new WindowsResourceRoot
            {
                Resheader = new List<WindowsResheader>
                {
                    new WindowsResheader
                    {
                        Name = "resmimetype",
                        Value = "text/microsoft-resx"
                    },
                    new WindowsResheader
                    {
                        Name = "version",
                        Value = "2.0"
                    },
                    new WindowsResheader
                    {
                        Name = "reader",
                        Value = "System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
                    },
                    new WindowsResheader
                    {
                        Name = "writer",
                        Value = "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
                    }
                },
                Data = targetLines.Select(tl => new WindowsData
                {
                    Space = "preserve",
                    Name = tl.Name,
                    Value = Escape(tl.Value) ?? "",
                }).ToList()
            };

            using (var fs = new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.SetLength(0);

                XmlWriterSettings xmlWriterSettings = new()
                {
                    Indent = true,
                    IndentChars = "  ",
                    Encoding = Encoding.UTF8,
                };

                // use stream writer and reader, because we need to set the encoding (that will be part of the generated xml)

                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                using (var writer = XmlWriter.Create(fs, xmlWriterSettings))
                {
                    _xmlSerializer.Serialize(new ExtendedXmlWriter(writer), androidResource, ns);
                }
            }
        }

        private string Escape(string text)
            => text?.Replace("%%amp;", "&");
    }
}
