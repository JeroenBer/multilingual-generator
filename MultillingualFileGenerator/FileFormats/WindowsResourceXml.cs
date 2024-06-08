using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.FileFormats;

[XmlRoot(ElementName = "root")]
public class WindowsResourceRoot
{

    [XmlElement(ElementName = "resheader")]
    public List<WindowsResheader> Resheader { get; set; }

    [XmlElement(ElementName = "data")]
    public List<WindowsData> Data { get; set; }
}

[XmlRoot(ElementName = "resheader")]
public class WindowsResheader
{

    [XmlElement(ElementName = "value")]
    public string Value { get; set; }

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }
}

[XmlRoot(ElementName = "data")]
public class WindowsData
{

    [XmlElement(ElementName = "value")]
    public string Value { get; set; }
    
    [XmlElement(ElementName = "comment")]
    public string Comment { get; set; }

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }

    [XmlAttribute(AttributeName = "xml:space")]
    public string Space { get; set; }
}

