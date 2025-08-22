using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.ExampleConfig;

public record struct FLogExampleConfig(string ExampleConfig)
{
    public static explicit operator FLogExampleConfig(string exampleConfig) => new(exampleConfig);
};

public static class FLogConfigExtractor
{
    private const string ExamplesNameSpace = "FortitudeCommon.Logging.Config.ExampleConfig";

    public static readonly FLogExampleConfig AsyncDailyDblBufferedFileExample
        = (FLogExampleConfig)$"{ExamplesNameSpace}.AsyncDailyDblBufferedFile.json";

    public static readonly FLogExampleConfig SyncDailyFileExample = (FLogExampleConfig)$"{ExamplesNameSpace}.SyncDailyFile.json";

    public static readonly FLogExampleConfig SyncFileAndColoredConsoleExample
        = (FLogExampleConfig)$"{ExamplesNameSpace}.SyncFileAndColoredConsole.json";


    public static FileInfo? ExtractExampleTo(this FLogExampleConfig fullExampleNameSpacePath, string? destPath = null, string? destFileName = null)
    {
        destPath     ??= FLogConfigFile.DefaultConfigPath;
        destFileName ??= FLogConfigFile.DefaultConfigFileName;
        var resourceStream = GetResourceStream(fullExampleNameSpacePath.ExampleConfig);
        if (resourceStream == null) return null;
        var destFilePath = Path.Combine(destPath, destFileName);
        using (var destFile = File.Create(destFilePath))
        {
            resourceStream.CopyTo(destFile);
        }
        return new FileInfo(destFilePath);
    }

    public static FLogAppConfig? LoadExampleTo(this FLogExampleConfig fullExampleNameSpacePath)
    {
        var resourceStream = GetResourceStream(fullExampleNameSpacePath.ExampleConfig);
        if (resourceStream == null) return null;

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddIniStream(resourceStream);
        var config        = configBuilder.Build();
        var fLogAppConfig = new FLogAppConfig(config, IFLogAppConfig.DefaultFLogAppConfigPath);
        return fLogAppConfig;
    }

    private static Stream? GetResourceStream(string resourceName)
    {
        //The Dll that you want to Load
        var assembly = Assembly.GetExecutingAssembly();
        //var names = assembly.GetManifestResourceNames();
        var stream = assembly.GetManifestResourceStream(resourceName);
        return stream;
    }
}
