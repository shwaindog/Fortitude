using FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;
using FortitudeCommon.Logging.AsyncProcessing.SingleBackground;
using FortitudeCommon.Logging.AsyncProcessing.ThreadPoolQueue;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

namespace FortitudeCommon.Logging.AsyncProcessing;

public static class FLogAsyncQueueFactoryMethods
{
    public static IFLogAsyncQueue? CreateQueue(this IAsyncQueueConfig queueConfig)
    {
        var queueType = queueConfig.QueueType;
        switch (queueType)
        {
            case AsyncProcessingType.ConfigDefinedAsyncThreads:
            case AsyncProcessingType.SingleBackgroundAsyncThread:
                return new DedicatedThreadAsyncQueue(queueConfig.QueueNumber, queueType, queueConfig.QueueCapacity);
            case AsyncProcessingType.Synchronise:
            case AsyncProcessingType.AllAsyncDisabled:
                return new FlogSynchroniseExecutionQueue(queueConfig.QueueNumber);
            case AsyncProcessingType.AsyncUsesThreadPool:
                return new ThreadPoolAsyncQueue(queueConfig.QueueNumber, queueConfig.QueueCapacity);
            default: return null;
        }
    }
}