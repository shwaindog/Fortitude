using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending;

public interface IAppenderAsyncClient
{
    int AppenderReceiveQueueNum { get; set; }

    AsyncProcessingType AsyncProcessingType { get; set; }

    bool AllowInvokerToProcessLogEntries { get; }

    void ProcessLogEntryWithValidAsyncSettings(IFLogEntry logEntry);

    void ProcessBatchLogEntriesWithValidAsyncSettings(IReusableList<IFLogEntry> batchLogEntries);
}

public class AppenderAsyncClient : IAppenderAsyncClient
{
    private readonly IFLogAsyncTargetReceiveQueueAppender asyncAppender;
    private readonly IFLoggerAsyncRegistry                asyncRegistry;

    private AsyncProcessingType asyncProcessingType;

    public bool AllowInvokerToProcessLogEntries { get; protected set; }

    public int AppenderReceiveQueueNum { get; set; }

    public AsyncProcessingType AsyncProcessingType
    {
        get => asyncProcessingType;
        set
        {
            if (value == asyncProcessingType) return;
            asyncProcessingType = value;

            switch (asyncProcessingType)
            {
                case AsyncProcessingType.AllAsyncDisabled:
                case AsyncProcessingType.SingleBackgroundAsyncThread:
                    AllowInvokerToProcessLogEntries = true;
                    break;
                default: AllowInvokerToProcessLogEntries = false; break;
            }
        }
    }

    public AppenderAsyncClient(IFLogAsyncTargetReceiveQueueAppender asyncAppender, int appenderReceiveQueueNum, IFLoggerAsyncRegistry asyncRegistry)
    {
        this.asyncAppender = asyncAppender;
        this.asyncRegistry = asyncRegistry;

        AppenderReceiveQueueNum = appenderReceiveQueueNum;
        AsyncProcessingType     = asyncRegistry.AsyncProcessingType;
    }

    public void ProcessLogEntryWithValidAsyncSettings(IFLogEntry logEntry)
    {
        if (AllowInvokerToProcessLogEntries || FLogAsyncQueue.MyCallingQueueNumber == AppenderReceiveQueueNum)
        {
            asyncAppender.ProcessReceivedLogEntry(logEntry);
        }
        else
        {
            asyncRegistry.AsyncQueueLocator.GetOrCreateQueue(AppenderReceiveQueueNum).SendLogEntryTo(logEntry, asyncAppender);
        }
    }

    public void ProcessBatchLogEntriesWithValidAsyncSettings(IReusableList<IFLogEntry> batchLogEntries)
    {
        if (AllowInvokerToProcessLogEntries || FLogAsyncQueue.MyCallingQueueNumber == AppenderReceiveQueueNum)
        {
            asyncAppender.ProcessReceiveBatchLogEntries(batchLogEntries);
        }
        else
        {
            asyncRegistry.AsyncQueueLocator.GetOrCreateQueue(AppenderReceiveQueueNum).SendLogEntriesTo(batchLogEntries, asyncAppender);
        }
    }
}
