using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Visitor.AppenderVisitors;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Core.Hub;

public class FLogBootstrap
{
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

    public FLogContext StartFlogSetAsCurrentContext(FLogContext toStart)
    {
        if (!toStart.HasStarted)
        {
            toStart.AsyncRegistry.StartAsyncQueues();
        
            FLoggerRoot.ImmortalInstance.InitializeFinalStepSetContext(toStart);

            toStart.HasStarted = true;
        }
        return toStart;
    }

    public FLogContext DefaultInitializeContext(FLogContext toInitialize)
    {
        var defaultMachineConfigFilePath = FLogConfigFile.ConfigHostFileName(Environment.MachineName);
        if (File.Exists(defaultMachineConfigFilePath))
        {
            Console.Out.WriteLine("Found FLog config " + defaultMachineConfigFilePath);
        }

        var defaultUserMachineConfigFilePath = FLogConfigFile.ConfigLoginHostFullFilePath(Environment.UserName, Environment.MachineName);
        if (File.Exists(defaultMachineConfigFilePath))
        {
            Console.Out.WriteLine("Found FLog config " + defaultMachineConfigFilePath);
        }
        var flogResolvedConfigFilePath = FLogConfigFile.ConfigFullFilePath;
        var fileInfo = new FileInfo(flogResolvedConfigFilePath);
        var fLogAppConfig = LoadMandatoryFileConfig(null, [flogResolvedConfigFilePath], defaultMachineConfigFilePath, defaultUserMachineConfigFilePath);

        return Initialize(fLogAppConfig, toInitialize);
    }

    private FLogAppConfig LoadMandatoryFileConfig(string? workingDirPath, string[] mustFindOneOfFiles, params string[] allOptionalOverrideFiles)
    {
        FLogAppConfig fLogAppConfig;
        
        var existsMandatory = mustFindOneOfFiles.Where(fPath => File.Exists(fPath)).Select(fPath => new FileInfo(fPath)).ToList();
        var existsOptional  = allOptionalOverrideFiles.Where(fPath => File.Exists(fPath)).Select(fPath => new FileInfo(fPath)).ToList();
        if (existsMandatory.Any())
        {
            foreach (var mandatoryFile in existsMandatory.Concat(existsOptional))
            {
                Console.Out.WriteLine("Found FLog config " + mandatoryFile);
            }

            IConfigurationBuilder configBuilder = new ConfigurationBuilder();
            if (workingDirPath != null)
            {
                configBuilder.SetBasePath(workingDirPath);
            }
            existsMandatory.Aggregate(configBuilder, (cfgBld, mandFile) => cfgBld.AddJsonFile(mandFile.FullName));
            existsOptional.Aggregate(configBuilder, (cfgBld, optFile) => cfgBld.AddJsonFile(optFile.FullName, true));

            var config = configBuilder.Build();
            if (config.GetChildren().All(cs => cs.Key != IFLogAppConfig.DefaultFLogAppConfigPath))
            {
                Console.Out.WriteLine
                    ($"Found FLog config files \n[{existsMandatory.Concat(existsOptional).JoinToString(",\n")}\n] but did not find " +
                     $"any FLog config at {{ \"{IFLogAppConfig.DefaultFLogAppConfigPath}\": ... }} in default config files " +
                     $"will use default console logging");
                fLogAppConfig = FLogAppConfig.BuildDefaultAppConfig();
            }
            else
            {
                fLogAppConfig = new FLogAppConfig(config, IFLogAppConfig.DefaultFLogAppConfigPath);
            }
        }
        else
        {
            Console.Out.WriteLine($"Did not find FLog config files '{mustFindOneOfFiles.JoinToString(",")}' file will default to console logging");
            fLogAppConfig = FLogAppConfig.BuildDefaultAppConfig();
        }
        return fLogAppConfig;
    }

    public virtual FLogContext InitializeContextFromWorkingDirFilePath(FLogContext toInitialize, string workingDirPath, string filePath)
    {
        var fLogAppConfig = LoadMandatoryFileConfig( workingDirPath, [filePath]);

        return Initialize(fLogAppConfig, toInitialize);
    }

    public virtual FLogContext InitializeContextFromFile(FLogContext toInitialize, string filePath)
    {
        var fLogAppConfig = LoadMandatoryFileConfig( null, [filePath]);

        return Initialize(fLogAppConfig, toInitialize);
    }

    public virtual FLogContext InitializeContextFromFiles (FLogContext toInitialize, string[] atLeastOneMandatoryConfigFiles
      , params string[] allOptionalOverrideFiles)
    {
        var fLogAppConfig = LoadMandatoryFileConfig(null, atLeastOneMandatoryConfigFiles, allOptionalOverrideFiles);

        return Initialize(fLogAppConfig, toInitialize);
    }

    public virtual FLogContext InitializeContextFromConfig(FLogContext toInitialize, IFLogAppConfig config)
    {
        return Initialize((IMutableFLogAppConfig)config, toInitialize);
    }
    
    protected virtual FLogContext Initialize(IMutableFLogAppConfig config, FLogContext toBeUpdated)
    {
        toBeUpdated.ConfigRegistry = toBeUpdated.ConfigRegistry?.UpdateConfig(config) ?? FLogCreate.MakeConfigRegistry(config);
        var initConfig = config.Initialization;

        var logEntryPoolInitConfig = initConfig.LogEntryPoolsInit;
        var logEntryRegistry =
            toBeUpdated.LogEntryPoolRegistry.UpdateConfig(logEntryPoolInitConfig) ?? FLogCreate.MakeLogEntryPoolRegistry(logEntryPoolInitConfig);

        var asyncConfig   = initConfig.AsyncBufferingInit;
        var asyncRegistry = toBeUpdated.AsyncRegistry.UpdateConfig(asyncConfig) ?? FLogCreate.MakeAsyncRegistry(asyncConfig);
        toBeUpdated.AsyncRegistry = asyncRegistry;

        var allAppenderDefinitions = config.Visit(new AllAppenderDefinitions()).Appenders;
        var appendersConfigsDict =
            allAppenderDefinitions.ToDictionary(a => a.AppenderName, a => a);
        var appenderReg = toBeUpdated.AppenderRegistry?.UpdateConfig(toBeUpdated, appendersConfigsDict)
                       ?? FLogCreate.MakeAppenderRegistry(toBeUpdated, appendersConfigsDict);
        toBeUpdated.AppenderRegistry ??= appenderReg;

        var rootLoggerConfig = config.RootLogger;
        var loggerReg = (toBeUpdated.LoggerRegistry?.UpdateConfig(rootLoggerConfig, appenderReg) ??
                         FLogCreate.MakeLoggerRegistry(rootLoggerConfig, appenderReg, logEntryRegistry));

        toBeUpdated.LoggerRegistry ??= loggerReg;
        toBeUpdated.IsInitialized  =   true;

        return toBeUpdated;
    }
}

public static class FLogBootstrapExtensions
{
    public static IFLogContext DefaultInitializeContext(this IFLogContext toInitialize)
    {
        if (toInitialize.IsInitialized)
        {
            throw new InvalidOperationException("Attempting to initialize an already initialized context");
        }
        return  FLogBootstrap.Instance.DefaultInitializeContext((FLogContext)toInitialize);
    }

    public static IFLogContext InitializeContextFromFile(this IFLogContext toInitialize, string filePath)
    {
        if (toInitialize.IsInitialized)
        {
            throw new InvalidOperationException("Attempting to initialize an already initialized context");
        }
        return FLogBootstrap.Instance.InitializeContextFromFile((FLogContext)toInitialize, filePath);
    }

    public static IFLogContext InitializeContextFromConfig(this IFLogContext toInitialize, IFLogAppConfig config)
    {
        if (toInitialize.IsInitialized)
        {
            throw new InvalidOperationException("Attempting to initialize an already initialized context");
        }
        return FLogBootstrap.Instance.InitializeContextFromConfig((FLogContext)toInitialize, config);
    }

    public static IFLogContext InitializeContextFromFiles(this IFLogContext toInitialize, string filePath, params string[] optionalOverrideFiles)
    {
        return FLogBootstrap.Instance.InitializeContextFromFiles((FLogContext)toInitialize, [filePath], optionalOverrideFiles);
    }

    public static IFLogContext InitializeContextFromWorkingDirFilePath(this IFLogContext toInitialize, string workingDirPath, string filePath)
    {
        return FLogBootstrap.Instance.InitializeContextFromWorkingDirFilePath((FLogContext)toInitialize, workingDirPath, filePath);
    }

    public static IFLogContext StartFlogSetAsCurrentContext(this IFLogContext toStart)
    {
        if (!toStart.IsInitialized)
        {
            throw new InvalidOperationException("Attempting to start an uninitialized FLog context");
        }
        return !toStart.HasStarted ? FLogBootstrap.Instance.StartFlogSetAsCurrentContext((FLogContext)toStart) : toStart;
    }

    public static IFLogContext InitializeStartAndSetAsCurrentContext(this IFLogContext toInitializeAndStart)
    {
        if (!toInitializeAndStart.IsInitialized)
        {
            toInitializeAndStart.DefaultInitializeContext();
        }
        if (!toInitializeAndStart.HasStarted)
        {
            toInitializeAndStart.StartFlogSetAsCurrentContext();
        }
        return toInitializeAndStart;
    }
    
    public static IFLogContext InitializeStartAndSetAsCurrentContext(this IFLogContext toInitializeAndStart, IFLogAppConfig config)
    {
        if (toInitializeAndStart.IsInitialized)
        {
            throw new InvalidOperationException("Attempting to initialize an already initialized context");
        }
        toInitializeAndStart.InitializeContextFromConfig(config);
        return FLogBootstrap.Instance.StartFlogSetAsCurrentContext((FLogContext)toInitializeAndStart);
    }

    public static IFLogContext InitializeContextFromFiles (this IFLogContext toInitialize, string[] atLeastOneMandatoryConfigFiles
      , params string[] optionalOverrideFiles)
    {
        return FLogBootstrap.Instance.InitializeContextFromFiles
            ((FLogContext)toInitialize, atLeastOneMandatoryConfigFiles, optionalOverrideFiles);
    }
}
