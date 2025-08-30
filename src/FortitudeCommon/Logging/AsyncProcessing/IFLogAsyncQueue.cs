// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing;

public interface IFLogAsyncQueuePublisher
{
    int QueueNumber { get; }

    AsyncProcessingType QueueType { get; }

    int QueueCapacity { get; }

    void Execute(Action job);

    void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IForkingFLogEntrySink> logEntrySinks
      , ITargetingFLogEntrySource publishSource);

    void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink, ITargetingFLogEntrySource publishSource);

    void FlushBufferToAppender(IBufferedFormatWriter toFlush);
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
    int QueueBackLogSize { get; }

    int EmptyQueueSleepMs { get; set; }
    void StartQueueReceiver();

    void StopQueueReceiver();
}

public abstract class FLogAsyncQueue(int queueNumber, AsyncProcessingType queueType, int queueCapacity) : IFLogAsyncQueue
{
    [field: ThreadStatic] public static int MyCallingQueueNumber { get; private set; }

    public bool ThreadIsOnQueue => MyCallingQueueNumber == QueueNumber;

    public int EmptyQueueSleepMs { get; set; }

    public int QueueNumber { get; protected set; } = queueNumber;

    public AsyncProcessingType QueueType { get; protected set; } = queueType;

    public int QueueCapacity { get; protected set; } = queueCapacity;

    public abstract void Execute(Action job);

    public abstract void FlushBufferToAppender(IBufferedFormatWriter toFlush);

    public abstract int QueueBackLogSize { get; }

    public abstract void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IForkingFLogEntrySink> logEntrySinks
      , ITargetingFLogEntrySource publishSource);

    public abstract void SendLogEntryEventTo(LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink
      , ITargetingFLogEntrySource publishSource);

    public abstract void StartQueueReceiver();

    public abstract void StopQueueReceiver();

    public static void SetCurrentThreadToQueueNumber(int queueNumber)
    {
        MyCallingQueueNumber = queueNumber;
    }
}
