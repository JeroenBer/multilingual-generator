using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using MultillingualFileGenerator.Translators;
using Xunit;

namespace MultillingualFileGenerator.Translators.Tests;

public class TranslatorFactoryTests
{
    [Fact]
    public void GetTranslator_ReturnsChatGptTranslator_WhenOpenAIKeyIsProvided()
    {
        // Arrange
        var factory = new TranslatorFactory();
        Environment.SetEnvironmentVariable("OpenAIKey", "test-openai-key");

        // Act
        var translator = factory.GetTranslator("en","es");

        // Assert
        Assert.IsType<ChatGptTranslator>(translator);
    }

    [Fact]
    public void GetTranslator_ReturnsAzureTranslator_WhenAzureKeysAreProvided()
    {
        // Arrange
        var factory = new TranslatorFactory();
        Environment.SetEnvironmentVariable("OpenAIKey", null);
        Environment.SetEnvironmentVariable("AzureTranslatorKey", "test-azure-key");
        Environment.SetEnvironmentVariable("AzureRegion", "test-region");

        // Act
        var translator = factory.GetTranslator("en", "fr");

        // Assert
        Assert.IsType<AzureTranslator>(translator);
    }

    [Fact]
    public void GetTranslator_ReturnsNull_WhenNoKeysAreProvided()
    {
        // Arrange
        var factory = new TranslatorFactory();
        Environment.SetEnvironmentVariable("OpenAIKey", null);
        Environment.SetEnvironmentVariable("AzureTranslatorKey", null);
        Environment.SetEnvironmentVariable("AzureRegion", null);

        // Act
        var translator = factory.GetTranslator("en","de");

        // Assert
        Assert.Null(translator);
    }

    [Fact]
    public void GetTranslator_UsesKeyFromApplicationJson_WhenFileExists()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { "application.json", new MockFileData("{\"OpenAIKey\": \"test-openai-key\"}") }
        });

        var factory = new TranslatorFactory(mockFileSystem);

        // Act
        var translator = factory.GetTranslator("en", "es");

        // Assert
        Assert.IsType<ChatGptTranslator>(translator);
    }

    [Fact]
    public void GetTranslator_PrioritizesApplicationJsonKey_OverEnvironmentVariable()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { "application.json", new MockFileData("{\"OpenAIKey\": \"file-openai-key\"}") }
        });

        var factory = new TranslatorFactory(mockFileSystem);
        Environment.SetEnvironmentVariable("OpenAIKey", "env-openai-key");

        // Act
        var translator = factory.GetTranslator("en", "es");

        // Assert
        Assert.IsType<ChatGptTranslator>(translator);

        // Cleanup
        Environment.SetEnvironmentVariable("OpenAIKey", null);
    }

    [Fact]
    public void GetTranslator_FallsBackToEnvironmentVariable_WhenFileDoesNotExist()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        var factory = new TranslatorFactory(mockFileSystem);
        Environment.SetEnvironmentVariable("OpenAIKey", "env-openai-key");

        // Act
        var translator = factory.GetTranslator("en", "es");

        // Assert
        Assert.IsType<ChatGptTranslator>(translator);

        // Cleanup
        Environment.SetEnvironmentVariable("OpenAIKey", null);
    }
}