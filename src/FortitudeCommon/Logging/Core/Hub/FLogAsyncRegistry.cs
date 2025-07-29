// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLoggerAsyncRegistry
{
    IAsyncQueuesInitConfig AsyncBufferingConfig { get; }

    AsyncProcessingType AsyncProcessingType { get; }

    IAsyncQueueLocator AsyncQueueLocator { get; }

    IUpdateableTimer   LoggerTimers      { get; }

    void StartAsyncQueues();
}

public interface IMutableFLoggerAsyncRegistry : IFLoggerAsyncRegistry
{
    new IAsyncQueuesInitConfig AsyncBufferingConfig { get; set; }

    new AsyncProcessingType AsyncProcessingType { get; set; }

    new IAsyncQueueLocator AsyncQueueLocator { get; set; }
}

public class FLogAsyncRegistry(IMutableAsyncQueuesInitConfig asyncInitConfig) : IMutableFLoggerAsyncRegistry
{
    private IMutableAsyncQueuesInitConfig asyncBufferingConfig = asyncInitConfig;

    public IAsyncQueuesInitConfig AsyncBufferingConfig
    {
        get => asyncBufferingConfig;
        set => asyncBufferingConfig = (IMutableAsyncQueuesInitConfig)value;
    }

    public IUpdateableTimer LoggerTimers { get; } = new UpdateableTimer("FLog Timers");

    public AsyncProcessingType AsyncProcessingType { get; set; } = asyncInitConfig.AsyncProcessingType;

    public IAsyncQueueLocator AsyncQueueLocator { get; set; } = FLogCreate.MakeAsyncQueueLocator(asyncInitConfig);

    public void StartAsyncQueues()
    {
        AsyncQueueLocator.StartAsyncQueues();
    }
}

public static class FLogAsyncRegistryExtensions
{
    public static IFLoggerAsyncRegistry? UpdateConfig(this IFLoggerAsyncRegistry? maybeCreated, IAsyncQueuesInitConfig asyncBufferingConfig)
    {
        var mutableMaybe = maybeCreated as IMutableFLoggerAsyncRegistry;
        if (mutableMaybe != null)
        {
            mutableMaybe.AsyncBufferingConfig = asyncBufferingConfig;
        }
        return mutableMaybe;
    }
}
