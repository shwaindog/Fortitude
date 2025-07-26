using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Appending;
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
        return new FLogAsyncProxyQueue(queueNumber, backingQueue);
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
        (IFLoggerRootConfig rootLoggerConfig, IFLogLoggerRegistry loggerRegistry)
    {
        return new FLoggerRoot(rootLoggerConfig, loggerRegistry);
    }

    public static IMutableFLoggerRootConfig DefaultCreateRootLoggerConfig(IConfigurationRoot configRoot, string configPath)
    {
        return new FLoggerRootConfig(configRoot, configPath);
    }

    public static IMutableFLogger DefaultCreateLogger
        (IFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLogLoggerRegistry loggerRegistry)
    {
        return new FLogger(loggerConsolidatedConfig, myParent, loggerRegistry);
    }

    public static IMutableFLoggerDescendantConfig DefaultClonedDescendantLoggerConfig(IFLoggerTreeCommonConfig toClone)
    {
        return new FLoggerDescendantConfig(toClone);
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

    public static IMutableFLogConfigRegistry DefaultConfigRegistry()
    {
        return new FLogConfigRegistry();
    }

    public static IMutableFLogAppenderRegistry DefaultAppenderRegistry
    (Dictionary<string, IMutableAppenderDefinitionConfig> foundAppenderDefinitions)
    {
        return new FLogAppenderRegistry(foundAppenderDefinitions);
    }

    public static IMutableFLogLoggerRegistry DefaultLoggerRegistry(IFLoggerRootConfig rootLogger, IFLogEntryPoolRegistry fLogEntryPoolRegistry)
    {
        return new FLogLoggerRegistry(rootLogger, fLogEntryPoolRegistry);
    }

    public static IMutableFLoggerAsyncRegistry DefaultAsyncRegistry(IMutableAsyncQueuesInitConfig asyncConfig)
    {
        return new FLogAsyncRegistry(asyncConfig);
    }
}