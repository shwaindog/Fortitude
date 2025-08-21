using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.LoggerViews;
using Microsoft.Extensions.Configuration;
using static FortitudeCommon.Logging.Core.Hub.FLogCommonDefaultFactoryImplementations;

namespace FortitudeCommon.Logging.Core.Hub;

public static class FLogCreate
{
    static FLogCreate()
    {
        MakeLogEntryPoolRegistry = DefaultFLogEntryPoolRegistry;

        MakeConfigRegistry   = DefaultConfigRegistry;
        MakeAppenderRegistry = DefaultAppenderRegistry;
        MakeLoggerRegistry   = DefaultLoggerRegistry;
        MakeAsyncRegistry    = DefaultAsyncRegistry;

        MakeAppenderConfig = DefaultCreateAppenderConfig;
        MakeAppender       = DefaultCreatedAppenderFromConfig;

        MakeLogger     = DefaultCreateLogger;

        MakeDescendantLoggerConfig       = DefaultCreateDescendantLoggerConfig;
        MakeClonedDescendantLoggerConfig = DefaultClonedDescendantLoggerConfig;
        MakeRootLoggerConfig             = DefaultCreateRootLoggerConfig;

        MakeAsyncQueueLocator = DefaultAsyncQueueLocator;
        MakeSynchroniseQueue  = DefaultSynchroniseQueue;
        MakeProxyAsyncQueue   = DefaultAsyncProxyQueue;
        MakeAsyncQueue        = DefaultCreateQueueFromConfig;

        MakeFLoggerView = DefaultCreatedFLoggerView;
    }

    public static Func<IMutableLogEntryPoolsInitializationConfig, IMutableFLogEntryPoolRegistry> MakeLogEntryPoolRegistry { get; set; }

    public static Func<IMutableFLogAppConfig, IFLogConfigRegistry> MakeConfigRegistry { get; set; }

    public static Func<IFLogContext, Dictionary<string, IMutableAppenderDefinitionConfig>, IFLogAppenderRegistry> MakeAppenderRegistry { get; set; }

    public static 
        Func< IMutableFLoggerRootConfig , IFLogAppenderRegistry , IFLogEntryPoolRegistry , IMutableFLogLoggerRegistry> 
        MakeLoggerRegistry { get; set; }

    public static Func<IMutableAsyncQueuesInitConfig, IMutableFLoggerAsyncRegistry> MakeAsyncRegistry { get; set; }

    public static Func<IMutableAppenderDefinitionConfig, IFLogContext, IMutableFLogAppender?> MakeAppender { get; set; }

    public static Func<IConfigurationRoot, string, IMutableAppenderReferenceConfig?> MakeAppenderConfig { get; set; }

    public static Func<IMutableFLoggerDescendantConfig, IFLoggerCommon, IFLogLoggerRegistry, IMutableFLogger> MakeLogger { get; set; }

    public static Func<IConfigurationRoot, string, IMutableFLoggerDescendantConfig> MakeDescendantLoggerConfig { get; set; }

    public static Func<IFLoggerTreeCommonConfig, string, IMutableFLoggerDescendantConfig> MakeClonedDescendantLoggerConfig { get; set; }

    public static Func<IConfigurationRoot, string, IMutableFLoggerRootConfig> MakeRootLoggerConfig { get; set; }

    public static Func<IMutableAsyncQueuesInitConfig, IAsyncQueueLocator> MakeAsyncQueueLocator { get; set; }

    public static Func<int, IFLogAsyncQueue> MakeSynchroniseQueue { get; set; }

    public static Func<int, IFLogAsyncQueue, IFLogAsyncQueue> MakeProxyAsyncQueue { get; set; }

    public static Func<IAsyncQueueConfig, IFLogAsyncQueue?> MakeAsyncQueue  { get; set; }

    public static Func<IFLogger, Type, ISwitchFLoggerView> MakeFLoggerView { get; set; }


}
