using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.LoggerViews;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Core.Hub;

public static class FLogCommonDefaultFactoryImplementations
{
    public static IFLogAsyncQueue? DefaultCreateQueueFromConfig(IAsyncQueueConfig queueConfig)
    {
        return queueConfig.CreateQueue();
    }

    public static IFLogAsyncQueue DefaultAsyncProxyQueue (int queueNumber, IFLogAsyncQueue backingQueue)
    {
        return new FLogAsyncUncheckedProxyQueue(queueNumber, backingQueue);
    }

    public static IFLogAsyncQueue DefaultSynchroniseQueue (int queueNumber)
    {
        return new FlogSynchroniseExecutionQueue(queueNumber);
    }

    public static IAsyncQueueLocator DefaultAsyncQueueLocator (IMutableAsyncQueuesInitConfig asyncQueuesConfig)
    {
        return new AsyncQueueLocator(asyncQueuesConfig);
    }

    public static IMutableFLoggerRoot DefaultCreateRootLogger
        (IMutableFLoggerRootConfig rootLoggerConfig, IFLogLoggerRegistry loggerRegistry)
    {
        return new FLoggerRoot(rootLoggerConfig, loggerRegistry);
    }

    public static IMutableFLoggerRootConfig DefaultCreateRootLoggerConfig(IConfigurationRoot configRoot, string configPath)
    {
        return new FLoggerRootConfig(configRoot, configPath);
    }

    public static IMutableFLogger DefaultCreateLogger
        (IMutableFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLogLoggerRegistry loggerRegistry)
    {
        return new FLogger(loggerConsolidatedConfig, myParent, loggerRegistry);
    }

    public static IMutableFLoggerDescendantConfig DefaultClonedDescendantLoggerConfig(IFLoggerTreeCommonConfig toClone, string configPath)
    {
        return new FLoggerDescendantConfig(toClone, configPath);
    }

    public static IMutableFLoggerDescendantConfig DefaultCreateDescendantLoggerConfig(IConfigurationRoot configRoot, string configPath)
    {
        return new FLoggerDescendantConfig(configRoot, configPath);
    }

    public static IMutableAppenderReferenceConfig? DefaultCreateAppenderConfig(IConfigurationRoot configRoot, string configPath)
    {
        return configRoot.GetBuiltAppenderReferenceConfig(configPath);
    }

    public static IMutableFLogAppender? DefaultCreatedAppenderFromConfig(IMutableAppenderDefinitionConfig appenderConfig, IFLogContext context)
    {
        return appenderConfig.GetBuiltAppenderReferenceConfig(context);
    }

    public static IMutableFLogEntryPoolRegistry DefaultFLogEntryPoolRegistry(IMutableLogEntryPoolsInitializationConfig poolInitConfig)
    {
        return new FLogEntryPoolRegistry(poolInitConfig);
    }

    public static IMutableFLogConfigRegistry DefaultConfigRegistry(IMutableFLogAppConfig appConfig)
    {
        return new FLogConfigRegistry(appConfig);
    }

    public static IMutableFLogAppenderRegistry DefaultAppenderRegistry
    (IFLogContext context, Dictionary<string, IMutableAppenderDefinitionConfig> foundAppenderDefinitions)
    {
        return new FLogAppenderRegistry(context, foundAppenderDefinitions);
    }

    public static IMutableFLogLoggerRegistry DefaultLoggerRegistry
        (
            IMutableFLoggerRootConfig rootLogger
          , IFLogAppenderRegistry appenderRegistry
          , IFLogEntryPoolRegistry fLogEntryPoolRegistry)
    {
        return new FLogLoggerRegistry(rootLogger, appenderRegistry, fLogEntryPoolRegistry);
    }

    public static IMutableFLoggerAsyncRegistry DefaultAsyncRegistry(IMutableAsyncQueuesInitConfig asyncConfig)
    {
        return new FLogAsyncRegistry(asyncConfig);
    }

    public static ISwitchFLoggerView DefaultCreatedFLoggerView(IFLogger fLogger, Type requestedViewType)
    {
        if (requestedViewType == typeof(IFLogger) || requestedViewType == typeof(FLogger)) return fLogger;
        if (requestedViewType == typeof(SimplifiedFLogger) || requestedViewType == typeof(ISimplifiedFLogger))
        {
            return new SimplifiedFLogger(fLogger);
        }
        if (requestedViewType == typeof(TerseFLogger) || requestedViewType == typeof(ITerseFLogger))
        {
            return new TerseFLogger(fLogger);
        }
        if (requestedViewType == typeof(LegacyFLogger) || requestedViewType == typeof(ILegacyFLogger))
        {
            return new LegacyFLogger(fLogger);
        }
        if (requestedViewType == typeof(VersatileFLogger) || requestedViewType == typeof(IVersatileFLogger))
        {
            return new VersatileFLogger(fLogger);
        }

        throw new ArgumentException($"DefaultCreatedFLoggerView does not create ISwitchFLoggerView of type {requestedViewType.Name}");
    }
}