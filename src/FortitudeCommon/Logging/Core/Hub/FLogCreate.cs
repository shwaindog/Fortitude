using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Appending;
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
        MakeRootLogger = DefaultCreateRootLogger;

        MakeDescendantLoggerConfig       = DefaultCreateDescendantLoggerConfig;
        MakeClonedDescendantLoggerConfig = DefaultClonedDescendantLoggerConfig;
        MakeRootLoggerConfig             = DefaultCreateRootLoggerConfig;

        MakeAsyncQueueLocator = DefaultAsyncQueueLocator;
        MakeSynchroniseQueue  = DefaultSynchroniseQueue;
        MakeProxyAsyncQueue   = DefaultAsyncProxyQueue;
        MakeAsyncQueue        = DefaultCreateQueueFromConfig;
    }

    public static Func<IMutableLogEntryPoolsInitializationConfig, IMutableFLogEntryPoolRegistry> MakeLogEntryPoolRegistry { get; set; }

    public static Func<IFLogConfigRegistry> MakeConfigRegistry { get; set; }

    public static Func<Dictionary<string, IMutableAppenderDefinitionConfig>, IFLogAppenderRegistry> MakeAppenderRegistry { get; set; }

    public static Func<IFLoggerRootConfig, IFLogEntryPoolRegistry, IMutableFLogLoggerRegistry> MakeLoggerRegistry { get; set; }

    public static Func<IMutableAsyncQueuesInitConfig, IMutableFLoggerAsyncRegistry> MakeAsyncRegistry { get; set; }

    public static Func<IMutableAppenderDefinitionConfig, IFLogContext, IMutableFLogAppender?> MakeAppender { get; set; }

    public static Func<IConfigurationRoot, string, IMutableAppenderReferenceConfig?> MakeAppenderConfig { get; set; }

    public static Func<IFLoggerDescendantConfig, IFLoggerCommon, IFLogLoggerRegistry, IMutableFLogger> MakeLogger { get; set; }

    public static Func<IConfigurationRoot, string, IMutableFLoggerDescendantConfig> MakeDescendantLoggerConfig { get; set; }

    public static Func<IFLoggerTreeCommonConfig, IMutableFLoggerDescendantConfig> MakeClonedDescendantLoggerConfig { get; set; }

    public static Func<IFLoggerRootConfig, IFLogLoggerRegistry, IMutableFLoggerRoot> MakeRootLogger { get; set; }

    public static Func<IConfigurationRoot, string, IMutableFLoggerRootConfig> MakeRootLoggerConfig { get; set; }

    public static Func<IMutableAsyncQueuesInitConfig, IAsyncQueueLocator> MakeAsyncQueueLocator { get; set; }

    public static Func<int, IFLogAsyncQueue> MakeSynchroniseQueue { get; set; }

    public static Func<int, IFLogAsyncQueue, IFLogAsyncQueue> MakeProxyAsyncQueue { get; set; }

    public static Func<IAsyncQueueConfig, IFLogAsyncQueue?> MakeAsyncQueue { get; set; }

}
