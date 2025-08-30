using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using FortitudeCommon.Logging.Core.Hub;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.ExampleConfig;

public record struct FLogExampleConfig(string ExampleConfig)
{
    public static explicit operator FLogExampleConfig(string exampleConfig) => new(exampleConfig);
};

public static class FLogConfigExamples
{
    private const string ExamplesNameSpace = "FortitudeCommon.Logging.Config.ExampleConfig";

    public static readonly FLogExampleConfig AsyncDailyDblBufferedFileExample
        = (FLogExampleConfig)$"{ExamplesNameSpace}.AsyncDailyDblBufferedFile.json";

    public static readonly FLogExampleConfig SyncDailyFileExample = (FLogExampleConfig)$"{ExamplesNameSpace}.SyncDailyFile.json";

    public static readonly FLogExampleConfig SyncFileAndColoredConsoleExample
        = (FLogExampleConfig)$"{ExamplesNameSpace}.SyncFileAndColoredConsole.json";

    public static readonly FLogExampleConfig SyncColoredConsoleExample
        = (FLogExampleConfig)$"{ExamplesNameSpace}.SyncColoredConsole.json";

    public static readonly FLogExampleConfig AsyncDblBufferedColoredConsoleExample
        = (FLogExampleConfig)$"{ExamplesNameSpace}.AsyncDblBufferedColoredConsole.json";

    public static readonly FLogExampleConfig AsyncDblBufferedFileAndColoredConsoleExample
        = (FLogExampleConfig)$"{ExamplesNameSpace}.AsyncDblBufferedFileAndColoredConsole.json";
    
    public static FileInfo? ExtractExampleTo(this FLogExampleConfig fullExampleNameSpacePath, string? destPath = null, string? destFileName = null)
    {
        destPath     ??= FLogConfigFile.DefaultConfigPath;
        destFileName ??= FLogConfigFile.DefaultConfigFileName;
        var resourceStream = GetResourceStream(fullExampleNameSpacePath.ExampleConfig);
        var destFilePath = Path.Combine(destPath, destFileName);
        using (var destFile = File.Create(destFilePath))
        {
            resourceStream.CopyTo(destFile);
        }
        return new FileInfo(destFilePath);
    }

    public static FLogAppConfig? LoadExampleToAppConfig(this FLogExampleConfig fullExampleNameSpacePath)
    {
        var resourceStream = GetResourceStream(fullExampleNameSpacePath.ExampleConfig);

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonStream(resourceStream);
        var config        = configBuilder.Build();
        var fLogAppConfig = new FLogAppConfig(config, IFLogAppConfig.DefaultFLogAppConfigPath);
        return fLogAppConfig;
    }

    public static IFLogContext? LoadExampleAsCurrentContext(this FLogExampleConfig fullExampleNameSpacePath)
    {
        var exampleAppConfig = fullExampleNameSpacePath.LoadExampleToAppConfig();
        if(exampleAppConfig == null) return null;
        var newContext = FLogContext.NewUninitializedContext.InitializeContextFromConfig(exampleAppConfig);
        return newContext.StartFlogSetAsCurrentContext();
    }

    public static JsonObject LoadExampleAsJsonObject(this FLogExampleConfig fullExampleNameSpacePath)
    {
        return JsonSerializer.Deserialize<JsonObject>(GetResourceStream(fullExampleNameSpacePath.ExampleConfig)!)!;
    }

    private static Stream GetResourceStream(string resourceName)
    {
        //The Dll that you want to Load
        var assembly = Assembly.GetExecutingAssembly();
        //var names = assembly.GetManifestResourceNames();
        var stream = assembly.GetManifestResourceStream(resourceName);
        return stream!;
    }
}
