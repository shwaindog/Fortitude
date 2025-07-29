// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing;

public interface IFLogAsyncQueuePublisher
{
    int QueueNumber { get; }

    AsyncProcessingType QueueType { get; }

    int  QueueCapacity { get; }

    void Execute(Action job);

    void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogAsyncTargetReceiveQueueAppender> appenders);

    void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogAsyncTargetReceiveQueueAppender appender);

    void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender);
}

public interface IReleaseBlockingDisposable : IDisposable
{
    bool IsBlocking { get; }
}

public interface IFLogAsyncSwitchableQueueClient : IFLogAsyncQueuePublisher
{
    bool IsBlocking { get; }

    IReleaseBlockingDisposable StartSwitchQueue(IFLogAsyncQueue switchToQueue);
}

public interface IFLogAsyncQueue : IFLogAsyncQueuePublisher
{
    void StartQueueReceiver();

    void StopQueueReceiver();

    int QueueBackLogSize { get; }

    int EmptyQueueSleepMs { get; set; }
}

public abstract class FLogAsyncQueue(int queueNumber, AsyncProcessingType queueType, int queueCapacity) : IFLogAsyncQueue
{
    [ThreadStatic] private static int currentThreadsQueue;

    public static int MyCallingQueueNumber => currentThreadsQueue;
    
    public bool ThreadIsOnQueue => MyCallingQueueNumber == QueueNumber;

    public int EmptyQueueSleepMs { get; set; }

    public int QueueNumber { get; protected set; } = queueNumber;

    public AsyncProcessingType QueueType { get; protected set; } = queueType;

    public int QueueCapacity { get; protected set; } = queueCapacity;

    public abstract void Execute(Action job);

    public abstract void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender);

    public abstract int QueueBackLogSize { get; }

    public abstract void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogAsyncTargetReceiveQueueAppender> appenders);

    public abstract void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogAsyncTargetReceiveQueueAppender appender);

    public abstract void StartQueueReceiver();

    public abstract void StopQueueReceiver();
    
    public static void SetCurrentThreadToQueueNumber(int queueNumber)
    {
        currentThreadsQueue = queueNumber;
    }
}
