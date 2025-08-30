// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IBufferFlushAppenderAsyncClient : IAppenderAsyncClient
{
    int BufferFlushQueueNum { get; set; }

    public void SendToFlushBufferToAppender(IBufferedFormatWriter bufferToFlush);
}

public class BufferFlushAppenderAsyncClient : ReceiveAsyncClient, IBufferFlushAppenderAsyncClient
{
    private int bufferFlushQueueNum = -1;

    private IFLogAsyncQueuePublisher bufferToFlushQueuePublisher = null!;

    public BufferFlushAppenderAsyncClient
    (IFLogBufferingFormatAppender destinationAppender, int appenderReceiveQueueNum
      , IFLoggerAsyncRegistry asyncRegistry, int bufferFlushQueueNum)
        : base(destinationAppender, appenderReceiveQueueNum, asyncRegistry) =>
        BufferFlushQueueNum = bufferFlushQueueNum;

    public int BufferFlushQueueNum
    {
        get => bufferFlushQueueNum;
        set
        {
            var wasChanged = value != bufferFlushQueueNum;
            bufferFlushQueueNum = value;
            if (wasChanged) bufferToFlushQueuePublisher = AsyncRegistry.AsyncQueueLocator.GetClientPublisherQueue(bufferFlushQueueNum);
        }
    }

    public void SendToFlushBufferToAppender(IBufferedFormatWriter bufferToFlush)
    {
        if (BufferFlushQueueNum == AppenderReceiveQueueNum || BufferFlushQueueNum == FLogAsyncQueue.MyCallingQueueNumber)
        {
            bufferToFlush.Flush();
            return;
        }

        bufferToFlushQueuePublisher.FlushBufferToAppender(bufferToFlush);
    }
}
