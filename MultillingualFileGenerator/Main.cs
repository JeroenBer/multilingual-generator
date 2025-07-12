using MultillingualFileGenerator.Config;
using MultillingualFileGenerator.Sources;
using MultillingualFileGenerator.Targets;
using MultillingualFileGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("MultillingualFileGenerator.Tests", AllInternalsVisible = true)]

namespace MultillingualFileGenerator;

internal class Main
{
    private readonly IConfigReader _configReader;
    private readonly SourceReaderService _sourceReaderService;
    private readonly TargetService _xliffService;
    private readonly SampleConfig _sampleConfig;

    public Main(IConfigReader configReader, SourceReaderService sourceReaderService, TargetService targetService, SampleConfig sampleConfig)
    {
        _configReader = configReader;
        _sourceReaderService = sourceReaderService;
        _xliffService = targetService;
        _sampleConfig = sampleConfig;
    }

    public async Task<int> Run(string[] args)
    {
        Console.WriteLine($"Multilingual file generator v{ToolVersion.GetVersion()}");

        FileInfo configFileInfo;
        if (args.Length != 1)
        {
            if (args.Length == 0)
            {
                // try to get config file from working directory
                configFileInfo = new FileInfo(MultilingualConfig.DefaultFile);
                if (!configFileInfo.Exists)
                {
                    WriteUsage();
                    return 1;
                }
            }
            else
            {
                if (args.Length == 3)
                {
                    if (args[0] == "create-sample-config")
                    {
                        var applicationName = args[2];
                        switch(args[1])
                        {
                            case "android":
                                _sampleConfig.WriteAndroidConfig(applicationName);
                                Console.WriteLine("Created sample config");
                                return 0;
                            case "ios":
                            case "macos":
                                _sampleConfig.WriteIosMacosConfig(applicationName);
                                Console.WriteLine("Created sample config");
                                return 0;
                            case "windows":
                                _sampleConfig.WriteWindowsConfig(applicationName);
                                Console.WriteLine("Created sample config");
                                return 0;
                            default:
                                break; // write usage
                        }
                    }
                }

                WriteUsage();
                return 1;
            }
        }
        else
        {
            configFileInfo = new FileInfo(args[0]);
            if (!configFileInfo.Exists)
            {
                Console.WriteLine($"File {args[0]} does not exist");
                return 1;
            }
        }

        var multilingualConfig = _configReader.ReadConfig(configFileInfo.FullName);
        var workingDir = Path.GetDirectoryName(configFileInfo.FullName);

        var sourceInput = _sourceReaderService.GetSourceInput(multilingualConfig.SourceSettings, workingDir);

        foreach(var target in multilingualConfig.Targets)
        {
            await _xliffService.ProcessTarget(sourceInput, multilingualConfig.TargetSettings, target, workingDir);
        }

        return 0;
    }
    private void WriteUsage()
    {
        Console.WriteLine($"No default {MultilingualConfig.DefaultFile} found");
        Console.WriteLine("Usage: mlgen");
        Console.WriteLine("Usage: mlgen <configfile>");
        Console.WriteLine("Usage: mlgen create-sample-config <android|ios|macos|windows> <ApplicationName>");
    }
}
