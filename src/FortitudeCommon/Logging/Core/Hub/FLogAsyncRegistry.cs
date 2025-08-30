// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLoggerAsyncRegistry
{
    IAsyncQueuesInitConfig AsyncBufferingConfig { get; }

    AsyncProcessingType AsyncProcessingType { get; }

    IAsyncQueueLocator AsyncQueueLocator { get; }

    IUpdateableTimer LoggerTimers { get; }

    void ScheduleRecycleDecrement(IRecyclableObject toDecrementRecyclableObject);

    void StartAsyncQueues();

    void ShutdownAsyncQueues();
}

public interface IMutableFLoggerAsyncRegistry : IFLoggerAsyncRegistry
{
    new IAsyncQueuesInitConfig AsyncBufferingConfig { get; set; }

    new AsyncProcessingType AsyncProcessingType { get; set; }

    new IAsyncQueueLocator AsyncQueueLocator { get; set; }
}

internal record struct RecyclableObjectRequestTime(IRecyclableObject ToDecrementRefCount, DateTime DecrementAt);

public class FLogAsyncRegistry : IMutableFLoggerAsyncRegistry
{
    private static readonly TimeSpan WaitTimeBeforeDecrement = TimeSpan.FromSeconds(4);

    private readonly Action checkItemsForDecrement;

    private readonly List<RecyclableObjectRequestTime> queuedToDecrementRefCounts = new();


    private IMutableAsyncQueuesInitConfig asyncBufferingConfig;

    private ITimerUpdate? decrementRecyclableTimerUpdate;

    public FLogAsyncRegistry(IMutableAsyncQueuesInitConfig asyncInitConfig)
    {
        asyncBufferingConfig = asyncInitConfig;
        AsyncProcessingType  = asyncInitConfig.AsyncProcessingType;
        AsyncQueueLocator    = FLogCreate.MakeAsyncQueueLocator(asyncInitConfig);

        checkItemsForDecrement = CheckQueuedToDecrementObjectsAndRescheduleTimerIfTimeNotReached;
    }

    public IAsyncQueuesInitConfig AsyncBufferingConfig
    {
        get => asyncBufferingConfig;
        set => asyncBufferingConfig = (IMutableAsyncQueuesInitConfig)value;
    }

    public void ScheduleRecycleDecrement(IRecyclableObject toDecrementRecyclableObject)
    {
        queuedToDecrementRefCounts.Add
            (new RecyclableObjectRequestTime
                (toDecrementRecyclableObject
               , TimeContext.UtcNow.Add(WaitTimeBeforeDecrement)));

        decrementRecyclableTimerUpdate ??= LoggerTimers.RunIn(WaitTimeBeforeDecrement, checkItemsForDecrement);
    }

    public IUpdateableTimer LoggerTimers { get; } = new UpdateableTimer("FLog Timers");

    public AsyncProcessingType AsyncProcessingType { get; set; }

    public IAsyncQueueLocator AsyncQueueLocator { get; set; }

    public void StartAsyncQueues()
    {
        AsyncQueueLocator.StartAsyncQueues();
    }

    public void ShutdownAsyncQueues()
    {
        AsyncQueueLocator.ShutdownAllAsyncQueues();
    }

    private void CheckQueuedToDecrementObjectsAndRescheduleTimerIfTimeNotReached()
    {
        decrementRecyclableTimerUpdate = null;
        var queuedCount = queuedToDecrementRefCounts.Count;
        if (queuedCount > 0)
        {
            var currentTime = TimeContext.UtcNow;
            for (var i = 0; i < queuedCount; i++)
            {
                var recyclableDecrementTime = queuedToDecrementRefCounts[i];
                if (recyclableDecrementTime.DecrementAt < currentTime)
                {
                    queuedToDecrementRefCounts.RemoveAt(i);
                    recyclableDecrementTime.ToDecrementRefCount.DecrementRefCount();
                }
                else
                {
                    break;
                }
            }
            queuedCount = queuedToDecrementRefCounts.Count;
            if (queuedCount > 0) decrementRecyclableTimerUpdate = LoggerTimers.RunIn(WaitTimeBeforeDecrement, checkItemsForDecrement);
        }
    }
}

public static class FLogAsyncRegistryExtensions
{
    public static IFLoggerAsyncRegistry? UpdateConfig(this IFLoggerAsyncRegistry? maybeCreated, IAsyncQueuesInitConfig asyncBufferingConfig)
    {
        var mutableMaybe = maybeCreated as IMutableFLoggerAsyncRegistry;

        if (mutableMaybe != null) mutableMaybe.AsyncBufferingConfig = asyncBufferingConfig;
        return mutableMaybe;
    }
}
