// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.AsyncProcessing;

public interface IFLogAsyncQueue
{
    int QueueNumber { get; }

    AsyncProcessingType QueueType { get; }

    int QueueCapacity { get; }

    void StartQueueReceiver();

    void StopQueueReceiver();

    int QueueBackLogSize { get; }

    void Execute(Action job);

    void SendLogEntryTo(IFLogEntry logEntry, IFLogAppender appender);

    void SendLogEntriesTo(IReusableList<IFLogEntry> batchLogEntries, IFLogAppender appender);

    void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender);

    int EmptyQueueSleepMs { get; set; }
}

public abstract class FLogAsyncQueue(int queueNumber, AsyncProcessingType queueType, int queueCapacity) : IFLogAsyncQueue
{
    [ThreadStatic] private static int currentThreadsQueue;

    public static int MyCallingQueueNumber => currentThreadsQueue;

    public static void SetCurrentThreadToQueueNumber(int queueNumber)
    {
        currentThreadsQueue = queueNumber;
    }

    public int EmptyQueueSleepMs { get; set; }

    public int QueueNumber { get; protected set; } = queueNumber;

    public AsyncProcessingType QueueType { get; protected set; } = queueType;

    public int QueueCapacity { get; protected set; } = queueCapacity;

    public virtual void Execute(Action job)
    {
        ExecuteImmediately(job);
    }

    public bool ThreadIsOnQueue => MyCallingQueueNumber == QueueNumber;

    protected void ExecuteImmediately(Action job) => job();

    public abstract void FlushBufferToAppender(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender);

    protected void FlushBufferToAppenderImmediately(IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender)
    {
        fromAppender.FlushBufferToAppender(toFlush);
    }

    public abstract int QueueBackLogSize { get; }

    public abstract void SendLogEntriesTo(IReusableList<IFLogEntry> batchLogEntries, IFLogAppender appender);

    protected void SendLogEntriesToImmediately(IReusableList<IFLogEntry> batchLogEntries, IFLogAppender appender)
    {
        appender.Append(batchLogEntries);
    }

    public abstract void SendLogEntryTo(IFLogEntry logEntry, IFLogAppender appender);

    protected void SendLogEntryToImmediately(IFLogEntry logEntry, IFLogAppender appender)
    {
        appender.Append(logEntry);
    }

    public abstract void StartQueueReceiver();
    public abstract void StopQueueReceiver();
}
