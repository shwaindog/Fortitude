using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Visitor.AppenderVisitors;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Core.Hub;

public class FLogBootstrap
{
    private const string DefaultFLogConfigJsonPath       = ".";
    private const string DefaultFLogConfigFileNamePrefix = "FLog";
    private const string DefaultFLogConfigFileNameExt    = "json";
    private const string OnePartTemplate                 = "-{0}";
    private const string TwoPartTemplate                 = "-{0}-{1}";
    private const string DefaultFLogConfigFileName       = $"{DefaultFLogConfigFileNamePrefix}.{DefaultFLogConfigFileNameExt}";

    private const string DefaultFLogHostnameConfigFileNameTemplate        = $"{DefaultFLogConfigFileNamePrefix}{OnePartTemplate}.json";
    private const string DefaultFLogHostAndUsernameConfigFileNameTemplate = $"{DefaultFLogConfigFileNamePrefix}{TwoPartTemplate}.json";

    private static FLogBootstrap? singletonInstance;

    private static readonly object SyncLock = new();

    public static FLogBootstrap Instance
    {
        get
        {
            if (singletonInstance == null)
            {
                lock (SyncLock)
                {
                    singletonInstance ??= new FLogBootstrap();
                }
            }
            return singletonInstance;
        }
        set => singletonInstance = value;
    }

    private string Join(string path, string fileName) => Path.Combine(path, fileName);

    public FLogContext StartFlog(FLogContext toStart)
    {
        if (!toStart.HasStarted)
        {
            toStart.AsyncRegistry.StartAsyncQueues();

            // ...

            toStart.HasStarted = true;
        }
        return toStart;
    }

    public FLogContext DefaultInitializeContext(FLogContext toInitialize)
    {
        var defaultMachineConfigFilePath = Join(DefaultFLogConfigJsonPath, DefaultFLogHostnameConfigFileNameTemplate.Format(Environment.MachineName));
        if (File.Exists(defaultMachineConfigFilePath))
        {
            Console.Out.WriteLine("Found FLog config " + defaultMachineConfigFilePath);
        }

        var defaultUserMachineConfigFilePath =
            Join(DefaultFLogConfigJsonPath
               , DefaultFLogHostAndUsernameConfigFileNameTemplate.Format(Environment.UserName, Environment.MachineName));
        if (File.Exists(defaultMachineConfigFilePath))
        {
            Console.Out.WriteLine("Found FLog config " + defaultMachineConfigFilePath);
        }
        var defaultFlogConfigFilePath = Join(DefaultFLogConfigJsonPath, DefaultFLogConfigFileName);
        var fLogAppConfig = LoadMandatoryFileConfig([defaultFlogConfigFilePath], defaultMachineConfigFilePath, defaultUserMachineConfigFilePath);


        // check for FLoggerConfig.json, FLogger-Username-Config.json, FLogger-Machine-Config.json, FLogger-Username-Machine-Config.json, 

        // check Web / App.config

        // Do Default config

        return Initialize(fLogAppConfig, toInitialize);
    }

    private FLogAppConfig LoadMandatoryFileConfig
    (string[] mustFindOneOfFiles,
        params string[] allOptionalOverrideFiles)
    {
        FLogAppConfig fLogAppConfig;

        var existsMandatory = mustFindOneOfFiles.Where(fPath => File.Exists(fPath)).ToList();
        var existsOptional  = allOptionalOverrideFiles.Where(fPath => File.Exists(fPath)).ToList();
        if (existsMandatory.Any())
        {
            foreach (var mandatoryFile in existsMandatory.Concat(existsOptional))
            {
                Console.Out.WriteLine("Found FLog config " + mandatoryFile);
            }

            IConfigurationBuilder configBuilder = new ConfigurationBuilder();
            existsMandatory.Aggregate(configBuilder, (cfgBld, mandFile) => cfgBld.AddJsonFile(mandFile));
            existsOptional.Aggregate(configBuilder, (cfgBld, optFile) => cfgBld.AddJsonFile(optFile, true));

            var config = configBuilder.Build();
            if (config.GetChildren().All(cs => cs.Key != IFLogAppConfig.DefaultFLogAppConfigPath))
            {
                Console.Out.WriteLine($"Did not find default {IFLogAppConfig.DefaultFLogAppConfigPath} in default config files will use default console logging");
                fLogAppConfig = FLogAppConfig.BuildDefaultAppConfig();
            }
            else
            {
                fLogAppConfig = new FLogAppConfig(config, IFLogAppConfig.DefaultFLogAppConfigPath);
            }
        }
        else
        {
            Console.Out.WriteLine("Did not find default {} file will default to console logging");
            fLogAppConfig = FLogAppConfig.BuildDefaultAppConfig();
        }
        return fLogAppConfig;
    }

    public virtual FLogContext InitializeContextFromFile(FLogContext toInitialize, string filePath)
    {
        var fLogAppConfig = LoadMandatoryFileConfig([filePath]);

        return Initialize(fLogAppConfig, toInitialize);
    }

    public virtual FLogContext InitializeContextFromFiles
    (FLogContext toInitialize
      , string[] atLeastOneMandatoryConfigFiles, params string[] allOptionalOverrideFiles)
    {
        var fLogAppConfig = LoadMandatoryFileConfig(atLeastOneMandatoryConfigFiles, allOptionalOverrideFiles);

        return Initialize(fLogAppConfig, toInitialize);
    }

    public virtual FLogContext DefaultInitializeContext(FLogContext toInitialize, IFLogAppConfig config)
    {
        return Initialize((IMutableFLogAppConfig)config, toInitialize);
    }


    protected virtual FLogContext Initialize(IMutableFLogAppConfig config, FLogContext toBeUpdated)
    {
        var initConfig = config.Initialization;

        var logEntryPoolInitConfig = initConfig.LogEntryPoolsInit;
        var logEntryRegistry =
            toBeUpdated.LogEntryPoolRegistry.UpdateConfig(logEntryPoolInitConfig) ?? FLogCreate.MakeLogEntryPoolRegistry(logEntryPoolInitConfig);

        var allAppenderDefinitions = config.Visit(new AllAppenderDefinitions()).Appenders;
        var appendersConfigsDict =
            allAppenderDefinitions.ToDictionary(a => a.AppenderName, a => a);
        var appenderReg = toBeUpdated.AppenderRegistry?.UpdateConfig(appendersConfigsDict)
                       ?? FLogCreate.MakeAppenderRegistry(appendersConfigsDict);
        toBeUpdated.AppenderRegistry ??= appenderReg;

        var rootLoggerConfig = config.RootLogger;
        var loggerReg = (toBeUpdated.LoggerRegistry?.UpdateConfig(rootLoggerConfig, appenderReg) ??
                         FLogCreate.MakeLoggerRegistry(rootLoggerConfig, logEntryRegistry));

        toBeUpdated.LoggerRegistry ??= loggerReg;
        var rootLogger = loggerReg.Root ?? FLogCreate.MakeRootLogger(rootLoggerConfig, loggerReg);
        loggerReg.Root = rootLogger;
        var asyncConfig = initConfig.AsyncBufferingInit;

        var asyncRegistry = toBeUpdated.AsyncRegistry.UpdateConfig(asyncConfig) ?? FLogCreate.MakeAsyncRegistry(asyncConfig);
        toBeUpdated.AsyncRegistry = asyncRegistry;
        toBeUpdated.IsInitialized = true;

        return toBeUpdated;
    }
}

public static class FLogBootstrapExtensions
{
    public static IFLogContext DefaultInitializeContext(this IFLogContext toInitialize)
    {
        return FLogBootstrap.Instance.DefaultInitializeContext((FLogContext)toInitialize);
    }

    public static IFLogContext InitializeContextFromFile(this IFLogContext toInitialize, string filePath)
    {
        return FLogBootstrap.Instance.InitializeContextFromFile((FLogContext)toInitialize, filePath);
    }

    public static IFLogContext InitializeContextFromFiles(this IFLogContext toInitialize, string filePath, params string[] optionalOverrideFiles)
    {
        return FLogBootstrap.Instance.InitializeContextFromFiles((FLogContext)toInitialize, [filePath], optionalOverrideFiles);
    }

    public static IFLogContext InitializeContextFromFiles
    (this IFLogContext toInitialize
      , string[] atLeastOneMandatoryConfigFiles
      , params string[] optionalOverrideFiles)
    {
        return FLogBootstrap.Instance.InitializeContextFromFiles
            ((FLogContext)toInitialize, atLeastOneMandatoryConfigFiles, optionalOverrideFiles);
    }
}
