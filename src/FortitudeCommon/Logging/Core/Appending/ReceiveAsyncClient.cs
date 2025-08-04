using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending;

public interface IAppenderAsyncClient
{
    int AppenderReceiveQueueNum { get; set; }

    void ReceiveLogEntryEventOnConfiguredQueue(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource publishSource);

    void RunJobOnAppenderQueue(Action job);

    ITimerUpdate RunJobOnFromTimer(Action job, int waitMs);
}

public class ReceiveAsyncClient : IAppenderAsyncClient
{
    protected readonly IFLogAppender DestinationAppender;

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

    public ReceiveAsyncClient(IFLogAppender destinationAppender, int appenderReceiveQueueNum
      , IFLoggerAsyncRegistry asyncRegistry)
    {
        DestinationAppender  = destinationAppender;
        AsyncRegistry  = asyncRegistry;

        AppenderReceiveQueueNum = appenderReceiveQueueNum;
    }

    public void ReceiveLogEntryEventOnConfiguredQueue(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource publishSource)
    {
        if (AppenderReceiveQueueNum == 0 || AppenderReceiveQueueNum == FLogAsyncQueue.MyCallingQueueNumber)
        {
            if (DestinationAppender.ReceiveEndpoint == publishSource)
            {
                publishSource.FinalTarget!.InBoundListener(logEntryEvent, publishSource);
            }
            {
                DestinationAppender.ReceiveEndpoint.PublishLogEntryEvent(logEntryEvent, publishSource);
            }
            return;
        }
        appenderReceiveQueuePublisher.SendLogEntryEventTo(logEntryEvent, DestinationAppender.ReceiveEndpoint, publishSource);
    }


    public void RunJobOnAppenderQueue(Action job)
    {
        if (AppenderReceiveQueueNum == 0 || AppenderReceiveQueueNum == FLogAsyncQueue.MyCallingQueueNumber)
        {
            DestinationAppender.ExecuteJob(job);
            return;
        }
        appenderReceiveQueuePublisher.Execute(job);
    }

    public ITimerUpdate RunJobOnFromTimer(Action job, int waitMs)
    {
        return AsyncRegistry.LoggerTimers.RunIn(waitMs, job);
    }
}

public class NullAsyncClient : IAppenderAsyncClient
{
    public static NullAsyncClient NullAsyncClientInstance = new ();

    public int AppenderReceiveQueueNum
    {
        get => 0;
        set => _ = value;
    }

    public void ReceiveLogEntryEventOnConfiguredQueue(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource publishSource)
    {
    }

    public void RunJobOnAppenderQueue(Action job)
    {
    }

    public ITimerUpdate RunJobOnFromTimer(Action job, int waitMs) => null!;
}
