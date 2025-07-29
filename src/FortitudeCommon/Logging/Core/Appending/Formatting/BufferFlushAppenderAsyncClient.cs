using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IBufferFlushAppenderAsyncClient : IAppenderAsyncClient
{
    int BufferFlushQueueNum { get; set; }

    public void SendToFlushBufferToAppender(IBufferedFormatWriter bufferToFlush, IFLogAsyncTargetFlushBufferAppender formatAppender);
}

public class BufferFlushAppenderAsyncClient : ReceiveAsyncClient, IBufferFlushAppenderAsyncClient
{
    private int bufferFlushQueueNum = -1;

    private IFLogAsyncQueuePublisher bufferToFlushQueuePublisher = null!;

    public int BufferFlushQueueNum
    {
        get => bufferFlushQueueNum;
        set
        {
            var wasChanged = value != bufferFlushQueueNum;
            bufferFlushQueueNum = value;
            if (wasChanged)
            {
                bufferToFlushQueuePublisher = AsyncRegistry.AsyncQueueLocator.GetClientPublisherQueue(bufferFlushQueueNum);
            }
        }
    }

    public BufferFlushAppenderAsyncClient
    (IFLogAsyncTargetReceiveQueueAppender asyncAppender, int appenderReceiveQueueNum
      , IFLoggerAsyncRegistry asyncRegistry, int bufferFlushQueueNum)
        : base(asyncAppender, appenderReceiveQueueNum, asyncRegistry)
    {
        BufferFlushQueueNum = bufferFlushQueueNum;
    }

    public void SendToFlushBufferToAppender(IBufferedFormatWriter bufferToFlush, IFLogAsyncTargetFlushBufferAppender formatAppender)
    {
        if (BufferFlushQueueNum == AppenderReceiveQueueNum || BufferFlushQueueNum == FLogAsyncQueue.MyCallingQueueNumber)
        {
            formatAppender.FlushBufferToAppender(bufferToFlush);
            return;
        }
        
        bufferToFlushQueuePublisher.FlushBufferToAppender(bufferToFlush, formatAppender);
    }
}