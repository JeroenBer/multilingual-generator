using MultillingualFileGenerator.Targets.Model;
using MultillingualFileGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.Targets.Writers
{
    // See https://developer.apple.com/library/archive/documentation/Cocoa/Conceptual/LoadingResources/Strings/Strings.html
    // TODO Support for comments
    internal class AppleTargetWriter : ITargetWriter
    {
        public void Write(string targetFilePath, List<TargetLine> targetLines)
        {
            using (var fs = new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.SetLength(0);


                using (var writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var targetLine in targetLines)
                    {
                        var appleTextLine = CreateAppleTextLine(targetLine);

                        writer.WriteLine(appleTextLine);
                    }
                }
            }
        }

        private string CreateAppleTextLine(TargetLine targetLine)
        {
            var line = $"\"{AppleEscape(targetLine.Name)}\" = \"{AppleEscape(targetLine.Value)}\";";
            return line;
        }

        private string AppleEscape(string text)
            => text?.Replace("\"", "\\\"");
    }
}
