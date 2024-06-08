using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.FileFormats;

[XmlRoot(ElementName = "string")]
public class AndroidString
{

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }

    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "resources")]
public class AndroidResources
{

    [XmlElement(ElementName = "string")]
    public List<AndroidString> AndroidStrings { get; set; }
}
