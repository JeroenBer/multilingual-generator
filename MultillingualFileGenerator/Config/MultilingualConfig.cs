using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Config;

public enum FileFormat
{
    Windows = 1,
    Android = 2,
    Apple = 3,
}

public class MultilingualConfig
{
    public const string DefaultFile = "Multilingual.json";

    public SourceSettings SourceSettings { get; set; }
    public TargetSettings TargetSettings { get; set; }
    public List<Target> Targets { get; set; }
}

public class SourceSettings
{
    public string ApplicationName { get; set; }
    public FileFormat SourceFileFormat { get; set; }
    public string SourceLanguage { get; set; }
    public string SourceFile { get; set; }
}

public class TargetSettings
{
    public FileFormat TargetFileFormat { get; set; }
    public string XliffBaseDir { get; set; }
    public string ResourcesBaseDir { get; set; }
}


public class Target
{
    public string TargetLanguage { get; set; }
    public string TargetXliff { get; set; }
    public string TargetResource { get; set; }
}


