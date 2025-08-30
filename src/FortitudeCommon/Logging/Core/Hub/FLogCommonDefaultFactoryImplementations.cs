// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
    public static IFLogAsyncQueue? DefaultCreateQueueFromConfig(IAsyncQueueConfig queueConfig) => queueConfig.CreateQueue();

    public static IFLogAsyncQueue DefaultAsyncProxyQueue(int queueNumber, IFLogAsyncQueue backingQueue) =>
        new FLogAsyncUncheckedProxyQueue(queueNumber, backingQueue);

    public static IFLogAsyncQueue DefaultSynchroniseQueue(int queueNumber) => new FlogSynchroniseExecutionQueue(queueNumber);

    public static IAsyncQueueLocator DefaultAsyncQueueLocator(IMutableAsyncQueuesInitConfig asyncQueuesConfig) =>
        new AsyncQueueLocator(asyncQueuesConfig);

    public static IMutableFLoggerRootConfig DefaultCreateRootLoggerConfig(IConfigurationRoot configRoot, string configPath) =>
        new FLoggerRootConfig(configRoot, configPath);

    public static IMutableFLogger DefaultCreateLogger
        (IMutableFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLogLoggerRegistry loggerRegistry) =>
        new FLogger(loggerConsolidatedConfig, myParent, loggerRegistry);

    public static IMutableFLoggerDescendantConfig DefaultClonedDescendantLoggerConfig(IFLoggerTreeCommonConfig toClone, string configPath) =>
        new FLoggerDescendantConfig(toClone, configPath);

    public static IMutableFLoggerDescendantConfig DefaultCreateDescendantLoggerConfig(IConfigurationRoot configRoot, string configPath) =>
        new FLoggerDescendantConfig(configRoot, configPath);

    public static IMutableAppenderReferenceConfig? DefaultCreateAppenderConfig(IConfigurationRoot configRoot, string configPath) =>
        configRoot.GetBuiltAppenderReferenceConfig(configPath);

    public static IMutableFLogAppender? DefaultCreatedAppenderFromConfig(IMutableAppenderDefinitionConfig appenderConfig, IFLogContext context) =>
        appenderConfig.GetBuiltAppenderReferenceConfig(context);

    public static IMutableFLogEntryPoolRegistry DefaultFLogEntryPoolRegistry(IMutableLogEntryPoolsInitializationConfig poolInitConfig) =>
        new FLogEntryPoolRegistry(poolInitConfig);

    public static IMutableFLogConfigRegistry DefaultConfigRegistry(IMutableFLogAppConfig appConfig) => new FLogConfigRegistry(appConfig);

    public static IMutableFLogAppenderRegistry DefaultAppenderRegistry
        (IFLogContext context, Dictionary<string, IMutableAppenderDefinitionConfig> foundAppenderDefinitions) =>
        new FLogAppenderRegistry(context, foundAppenderDefinitions);

    public static IMutableFLogLoggerRegistry DefaultLoggerRegistry
    (
        IMutableFLoggerRootConfig rootLogger
      , IFLogAppenderRegistry appenderRegistry
      , IFLogEntryPoolRegistry fLogEntryPoolRegistry) =>
        new FLogLoggerRegistry(rootLogger, appenderRegistry, fLogEntryPoolRegistry);

    public static IMutableFLoggerAsyncRegistry DefaultAsyncRegistry(IMutableAsyncQueuesInitConfig asyncConfig) => new FLogAsyncRegistry(asyncConfig);

    public static ISwitchFLoggerView DefaultCreatedFLoggerView(IFLogger fLogger, Type requestedViewType)
    {
        if (requestedViewType == typeof(IFLogger) || requestedViewType == typeof(FLogger)) return fLogger;
        if (requestedViewType == typeof(SimplifiedFLogger) || requestedViewType == typeof(ISimplifiedFLogger)) return new SimplifiedFLogger(fLogger);
        if (requestedViewType == typeof(TerseFLogger) || requestedViewType == typeof(ITerseFLogger)) return new TerseFLogger(fLogger);
        if (requestedViewType == typeof(LegacyFLogger) || requestedViewType == typeof(ILegacyFLogger)) return new LegacyFLogger(fLogger);
        if (requestedViewType == typeof(VersatileFLogger) || requestedViewType == typeof(IVersatileFLogger)) return new VersatileFLogger(fLogger);

        throw new ArgumentException($"DefaultCreatedFLoggerView does not create ISwitchFLoggerView of type {requestedViewType.Name}");
    }
}
