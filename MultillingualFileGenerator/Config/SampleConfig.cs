using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Config
{
    internal class SampleConfig
    {
        private readonly IConfigWriter _configWriter;

        public SampleConfig(IConfigWriter configWriter) 
        {
            _configWriter = configWriter;
        }

        public void WriteAndroidConfig(string applicationName)
        {
            var config = new MultilingualConfig
            {
                SourceSettings = new SourceSettings
                {
                    ApplicationName = applicationName,
                    SourceFile = "Resources/values/Strings.xml",
                    SourceFileFormat = FileFormat.Android,
                    SourceLanguage = "en-US",
                },
                TargetSettings = new TargetSettings
                {
                    TargetFileFormat = FileFormat.Android,
                    XliffBaseDir = "MultilingualResources",
                    ResourcesBaseDir = "Resources"
                },
                Targets = new List<Target>
                {
                    new Target
                    {
                        TargetLanguage = "es",
                        TargetXliff = $"{applicationName}.es.xlf",
                        TargetResource = "values-es/Strings.xml"
                    },
                    new Target
                    {
                        TargetLanguage = "nl",
                        TargetXliff = $"{applicationName}.nl.xlf",
                        TargetResource = "values-nl/Strings.xml"
                    }
                }
            };

            _configWriter.WriteConfig(MultilingualConfig.DefaultFile, config);
        }

        public void WriteIosMacosConfig(string applicationName)
        {
            var config = new MultilingualConfig
            {
                SourceSettings = new SourceSettings
                {
                    ApplicationName = applicationName,
                    SourceFile = "Resources/en.lproj/Localizable.strings",
                    SourceFileFormat = FileFormat.Apple,
                    SourceLanguage = "en",
                },
                TargetSettings = new TargetSettings
                {
                    TargetFileFormat = FileFormat.Apple,
                    XliffBaseDir = "MultilingualResources",
                    ResourcesBaseDir = "Resources"
                },
                Targets = new List<Target>
                {
                    new Target
                    {
                        TargetLanguage = "es",
                        TargetXliff = $"{applicationName}.es.xlf",
                        TargetResource = "es.lproj/Localizable.strings"
                    },
                    new Target
                    {
                        TargetLanguage = "nl",
                        TargetXliff = $"{applicationName}.nl.xlf",
                        TargetResource = "nl.lproj/Localizable.strings"
                    }
                }
            };

            _configWriter.WriteConfig(MultilingualConfig.DefaultFile, config);
        }

        public void WriteWindowsConfig(string applicationName)
        {
            var config = new MultilingualConfig
            {
                SourceSettings = new SourceSettings
                {
                    ApplicationName = applicationName,
                    SourceFile = "MultilingualResources/en/resources.resw",
                    SourceFileFormat = FileFormat.Windows,
                    SourceLanguage = "en",
                },
                TargetSettings = new TargetSettings
                {
                    TargetFileFormat = FileFormat.Windows,
                    XliffBaseDir = "MultilingualResources",
                    ResourcesBaseDir = "MultilingualResources"
                },
                Targets = new List<Target>
                {
                    new Target
                    {
                        TargetLanguage = "es",
                        TargetXliff = $"{applicationName}.es.xlf",
                        TargetResource = "es/resources.resx"
                    },
                    new Target
                    {
                        TargetLanguage = "nl",
                        TargetXliff = $"{applicationName}.nl.xlf",
                        TargetResource = "nl/resources.resx"
                    }
                }
            };

            _configWriter.WriteConfig(MultilingualConfig.DefaultFile, config);
        }

    }
}
