# Multilingual File Generator (dotnet-mlgen)
A generator for .NET multilingual files for all native target platforms Windows, macOS, iOS, Android.

It can serve as a replacement for the Microsoft Multilingual App Toolkit.

## Introduction
This tool can be used to generate multilingual files: Xliff and translated resource files.

This can be platform specific resource files for net8.0-android, net8.0-ios, net8.0-macos or Windows.

The Microsoft Multilingual App Toolkit no longer seems to support these specific native platforms.

Other advantages of using this dotnet tool:
- It's not necessary to use Visual Studio since it's a .NET tool. Also Visual Studio upgrades won't break this tool.
- It works on all platforms (Windows, Mac, Linux).
- Since it needs to be triggered by hand it doesn't run every build which otherwise slows down the build since it updates files.
- It uses a config file, so there is no magic.
- The souce code is available at github.
- It's compatible with the Multilingual App Toolkit.

## Workflow

A configuration file needs to be in the project directory (Multilingual.config), for more info see below.

The config specifies the source resource (e.g. English language). 
The tool will create and update Xliff's depending on changes in the source resource. It will also generated localized target resources based on the Xliff.

Xliff can be editted with any Xliff editor or the Microsoft Multilingual App Toolkit Editor, see https://learn.microsoft.com/en-us/windows/apps/design/globalizing/multilingual-app-toolkit-editor-downloads.

## Dotnet Tool

For more information about local/global .NET tools see https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools

## Installation

### Global
```
dotnet tool install --global dotnet-mlgen 
```

### Local
```
dotnet new tool-manifest # if you are setting up this repo

dotnet tool install --local dotnet-mlgen
```

Restore in repo:
```
dotnet tool restore
```



## Usage

### Update translation files
1. Go to project directory with Multilingual.config file.
2. On prompt: `dotnet mlgen` (mlgen = Multilingual generator)

OR:
On prompt: `dotnet mlgen <full path to config file>`.

### Create first sample config file

The sample config file contains translations for the Spanish (es) and Dutch (nl) language.

#### Android
```
dotnet mlgen create-sample-config android AppName
```
#### iOS
```
dotnet mlgen create-sample-config ios AppName
```
#### macOS
```
dotnet mlgen create-sample-config macos AppName
```
#### Windows
```
dotnet mlgen create-sample-config windows AppName
```

## Config

### Example config for android
```
{
  "SourceSettings": {
    "ApplicationName": "AppName",
    "SourceFileFormat": "android",
    "SourceLanguage": "en-US",
    "SourceFile": "Resources/values/Strings.xml"
  },
  "TargetSettings": {
    "TargetFileFormat": "android",
    "XliffBaseDir": "MultilingualResources",
    "ResourcesBaseDir": "Resources"
  },
  "Targets": [
    {
      "TargetLanguage": "es",
      "TargetXliff": "AppName.es.xlf",
      "TargetResource": "values-es/Strings.xml"
    },
    {
      "TargetLanguage": "nl",
      "TargetXliff": "AppName.nl.xlf",
      "TargetResource": "values-nl/Strings.xml"
    }
  ]
}
```

### Config file description

All paths for file generation are relative to the location of the config file.

#### SourceSettings
Settings for the source, this file should already exist.

|Name|Description|
|--|--|
|ApplicationName|Name of the application, used in the Xliff for generating id's|
|SourceFileFormat|Describes the type of multilingual input resource: allowed values "android", "apple" or "windows". Android = "strings.xml", apple = "Localizable.strings", windows = "resources.resx".|
|SourceLanguage|The source language|
|SourceFile|location of the source resource file relative to the multilingual.json location|

### TargetSettings
Settings that apply to all Targets

|Name|Description|
|--|--|
|TargetFileFormat|Describes the type of multilingual input resource: allowed values "android", "apple" or "windows". Android = "strings.xml", apple = "Localizable.strings", windows = "resources.resx".|
|XliffBaseDir|Base directory for the Xliff files|
|ResourcesBaseDir|Base directory for the resource files|

### Targets
An array of target languages that need to be generated (Xliff and resouce in specific file format)

|Name|Description|
|--|--|
|TargetLanguage|Target language|
|TargetXliff|Filename of the target Xliff|
|TargetResource|File name of the target resource|

## Machine Translations
You can use either the OpenAI/ChatGPT API or the Azure Translator API to automatically translate untranslated strings.
Select one of the options by providing the necessary API credentials as described below. If both are provided, 
the OpenAI/ChatGPT option will take precedence.

### Automatic translation using OpenAI/ChatGPT
Untranslated strings can be automatically translated using the OpenAI/ChatGPT API.
The translated strings will get the status "NeedsReview".
The model is now hardcoded to "gpt-4.1-mini".

To use this feature, you can specify the OpenAI API key in one of the following ways:

1. **Environment Variable**: Define an environment variable `OpenAIKey` with your OpenAI API Key.
2. **application.json File**: Create an `application.json` file in the same directory as your `Multilingual.config` file and include the following entry:

```json
{
  "OpenAIKey": "your-openai-api-key"
}
```

If both are specified, the key in the `application.json` file takes precedence.

### Automatic translation using Azure Translator
Untranslated strings can be automatically translated using Azure Translator.
The translated strings will get the status "NeedsReview".

To use this feature, you can specify the Azure Translator credentials in one of the following ways:

1. **Environment Variables**: Define the following environment variables:
   - `AzureTranslatorKey`: Your Azure Translator API Key.
   - `AzureRegion`: The Azure region for your Translator service.

2. **application.json File**: Create an `application.json` file in the same directory as your `Multilingual.config` file and include the following entries:

```json
{
  "AzureTranslatorKey": "your-azure-translator-key",
  "AzureRegion": "your-azure-region"
}
```

If both are specified, the values in the `application.json` file take precedence.




