// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLoggerAsyncRegistry
{
    IAsyncQueuesInitConfig AsyncBufferingConfig { get; }

    AsyncProcessingType AsyncProcessingType { get; }

    IAsyncQueueLocator AsyncQueueLocator { get; }

    void StartAsyncQueues();
}
public interface IMutableFLoggerAsyncRegistry : IFLoggerAsyncRegistry
{
    new IAsyncQueuesInitConfig AsyncBufferingConfig { get; set; }

    new AsyncProcessingType AsyncProcessingType { get; set; }

    new IAsyncQueueLocator AsyncQueueLocator { get; set; }
}

public class FLogAsyncRegistry : IMutableFLoggerAsyncRegistry
{
    private IAsyncQueuesInitConfig asyncBufferingConfig;

    public FLogAsyncRegistry(IMutableAsyncQueuesInitConfig asyncInitConfig)
    {
        asyncBufferingConfig = asyncInitConfig;
        AsyncQueueLocator    = FLogCreate.MakeAsyncQueueLocator(asyncInitConfig);
        AsyncProcessingType  = asyncInitConfig.AsyncProcessingType;

        FLogCreate.InitializeAsyncServices(asyncBufferingConfig, this);
    }

    public IAsyncQueuesInitConfig AsyncBufferingConfig
    {
        get => asyncBufferingConfig;
        set => asyncBufferingConfig = value;
    }

    public AsyncProcessingType AsyncProcessingType { get; set; }

    public IAsyncQueueLocator AsyncQueueLocator { get; set; }

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



