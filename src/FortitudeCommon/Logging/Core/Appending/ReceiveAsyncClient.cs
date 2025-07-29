using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending;

public interface IAppenderAsyncClient
{
    int AppenderReceiveQueueNum { get; set; }

    void ReceiveLogEntryEventOnConfiguredQueue(LogEntryPublishEvent logEntryEvent);

    void RunJobOnAppenderQueue(Action job);

    ITimerUpdate RunJobOnFromTimer(Action job, int waitMs);
}

public class ReceiveAsyncClient : IAppenderAsyncClient
{
    private readonly IFLogAsyncTargetReceiveQueueAppender asyncAppender;

    protected readonly IFLoggerAsyncRegistry AsyncRegistry;

    private IFLogAsyncQueuePublisher appenderReceiveQueuePublisher = null!;

    private int appenderReceiveQueueNum = -1;

    public int AppenderReceiveQueueNum
    {
        get => appenderReceiveQueueNum;
        set
        {
            var wasChanged = value != appenderReceiveQueueNum;
            appenderReceiveQueueNum = value;
            if (wasChanged)
            {
                appenderReceiveQueuePublisher = AsyncRegistry.AsyncQueueLocator.GetClientPublisherQueue(appenderReceiveQueueNum);
            }
        }
    }

    public ReceiveAsyncClient(IFLogAsyncTargetReceiveQueueAppender asyncAppender, int appenderReceiveQueueNum
      , IFLoggerAsyncRegistry asyncRegistry)
    {
        this.asyncAppender  = asyncAppender;
        AsyncRegistry  = asyncRegistry;

        AppenderReceiveQueueNum = appenderReceiveQueueNum;
    }

    public void ReceiveLogEntryEventOnConfiguredQueue(LogEntryPublishEvent logEntryEvent)
    {
        if (AppenderReceiveQueueNum == 0 || AppenderReceiveQueueNum == FLogAsyncQueue.MyCallingQueueNumber)
        {
            asyncAppender.ProcessReceivedLogEntryEvent(logEntryEvent);
            return;
        }
        appenderReceiveQueuePublisher.SendLogEntryEventTo(logEntryEvent, asyncAppender);
    }


    public void RunJobOnAppenderQueue(Action job)
    {
        if (AppenderReceiveQueueNum == 0 || AppenderReceiveQueueNum == FLogAsyncQueue.MyCallingQueueNumber)
        {
            asyncAppender.ExecuteJob(job);
            return;
        }
        appenderReceiveQueuePublisher.Execute(job);
    }

    public ITimerUpdate RunJobOnFromTimer(Action job, int waitMs)
    {
        return AsyncRegistry.LoggerTimers.RunIn(waitMs, job);
    }
}
