using System;
using System.Numerics;
using System.Xml.Serialization;

namespace MultillingualFileGenerator.Xliff;

[Serializable()]
[XmlRoot("xliff", Namespace = "urn:oasis:names:tc:xliff:document:1.2")]
public class XliffFile
{
    [XmlAttribute("version")]
    public string Version { get; set; }

    [XmlAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    public string XsiSchemaLocation { get; set; }

    [XmlElement("file")]
    public XliffFileElement[] File { get; set; }
}

[Serializable()]
public class XliffFileElement
{
    [XmlAttribute("datatype")]
    public string DataType { get; set; }

    [XmlAttribute("source-language")]
    public string SourceLanguage { get; set; }

    [XmlAttribute("target-language")]
    public string TargetLanguage { get; set; }

    [XmlAttribute("original")]
    public string Original { get; set; }

    [XmlAttribute("tool-id")]
    public string ToolID { get; set; }

    [XmlAttribute("product-name")]
    public string ProductName { get; set; }

    [XmlAttribute("product-version")]
    public string ProductVersion { get; set; }

    [XmlAttribute("build-num")]
    public string BuildNum { get; set; }

    [XmlElement("header")]
    public XliffHeaderElement Header { get; set; }

    [XmlElement("body")]
    public XliffBodyElement Body { get; set; }
}

[Serializable()]
public class XliffHeaderElement
{
    [XmlElement("tool")]
    public XliffToolElement Tool { get; set; }
}

[Serializable()]
public class XliffToolElement
{
    [XmlAttribute("tool-id")]
    public string ToolID { get; set; }

    [XmlAttribute("tool-name")]
    public string ToolName { get; set; }

    [XmlAttribute("tool-version")]
    public string ToolVersion { get; set; }

    [XmlAttribute("tool-company")]
    public string ToolCompany { get; set; }
}

[Serializable()]
public class XliffBodyElement
{
    [XmlElement("group")]
    public XliffGroupElement Group { get; set; }
}

[Serializable()]
public class XliffGroupElement
{
    [XmlAttribute("id")]
    public string ID { get; set; }

    [XmlAttribute("datatype")]
    public string DataType { get; set; }

    [XmlElement("trans-unit")]
    //[XmlArrayItem("trans-unit", typeof(XliffTransUnitElement))]
    public XliffTransUnitElement[] TransUnits { get; set; }
}

[Serializable()]
public class XliffTransUnitElement
{
    [XmlAttribute("id")]
    public string ID { get; set; }

    [XmlAttribute("translate")]
    public string Translate { get; set; }

    [XmlAttribute("xml:space")]
    public string Space { get; set; }

    [XmlElement("source")]
    public XliffSourceElement Source { get; set; }

    [XmlElement("target")]
    public XliffTargetElement Target { get; set; }

    [XmlElement("note")]
    public XliffNoteElement Note{ get; set; }
}

[Serializable()]
public class XliffSourceElement
{
    [XmlText()]
    public string Value { get; set; }
}

[Serializable()]
public class XliffTargetElement
{
    [XmlAttribute("state")]
    public string State { get; set; }

    [XmlText()]
    public string Value { get; set; }
}

[Serializable()]
public class XliffNoteElement
{
    [XmlAttribute("from")]
    public string From { get; set; }
    [XmlAttribute("annotates")]
    public string Annotates { get; set; }
    [XmlAttribute("priority")]
    public string Priority { get; set; }

    // <note from = "MultilingualUpdate" annotates="source" priority="2">Please verify the translation’s accuracy as the source string was updated after it was translated.</note>

    [XmlText()]
    public string Value { get; set; }
}

