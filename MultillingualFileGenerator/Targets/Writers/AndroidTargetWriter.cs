using MultillingualFileGenerator.FileFormats;
using MultillingualFileGenerator.Targets.Model;
using MultillingualFileGenerator.Util;
using MultillingualFileGenerator.Xliff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.Targets.Writers
{
    internal class AndroidTargetWriter : ITargetWriter
    {
        private static readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(AndroidResources));
        private static Regex _escapeRegex = new Regex(@"(?<!\\)'", RegexOptions.Compiled);


        public void Write(string targetFilePath, List<TargetLine> targetLines)
        {
            var androidResource = new AndroidResources
            {
                AndroidStrings = targetLines.Select(tl => new AndroidString
                {
                    Name = tl.Name,
                    Text = Escape(tl.Value),
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

        internal string Escape(string text)
        {
            if (text == null)
                return null;

            return _escapeRegex.Replace(text, "\\'");
        }
    }
}
