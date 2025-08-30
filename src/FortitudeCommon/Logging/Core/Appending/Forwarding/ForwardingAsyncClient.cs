// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding;

public interface IAppenderForwardingAsyncClient : IAppenderAsyncClient
{
    void ForwardLogEntryEventToAppenders(int onQueue, LogEntryPublishEvent logEntryEvent, IReadOnlyList<IAppenderClient> appendersOnQueue);
}

public class ForwardingAsyncClient : ReceiveAsyncClient, IAppenderForwardingAsyncClient
{
    private readonly List<IFLogAsyncQueuePublisher?> publisherByQueueNumber = new();

    public ForwardingAsyncClient(FLogForwardingAppender destinationAppender, int appenderReceiveQueueNum
      , IFLoggerAsyncRegistry asyncRegistry)
        : base(destinationAppender, appenderReceiveQueueNum, asyncRegistry) { }


    public void ForwardLogEntryEventToAppenders(int onQueue, LogEntryPublishEvent logEntryEvent, IReadOnlyList<IAppenderClient> appendersOnQueue)
    {
        if (onQueue == 0 || onQueue == FLogAsyncQueue.MyCallingQueueNumber)
        {
            foreach (var appender in appendersOnQueue) appender.PublishLogEntryEvent(logEntryEvent);
            return;
        }

        var requiredLookupSize = onQueue + 1;

        if (publisherByQueueNumber.Count < requiredLookupSize) publisherByQueueNumber.Capacity = requiredLookupSize;
        for (var i = publisherByQueueNumber.Count; i < requiredLookupSize; i++) publisherByQueueNumber.Add(null);

        var checkHasPublisher = publisherByQueueNumber[onQueue];
        if (checkHasPublisher == null)
        {
            checkHasPublisher               = AsyncRegistry.AsyncQueueLocator.GetClientPublisherQueue(onQueue);
            publisherByQueueNumber[onQueue] = checkHasPublisher;
        }

        checkHasPublisher.SendLogEntryEventTo(logEntryEvent, appendersOnQueue, DestinationAppender.ReceiveEndpoint);
    }
}
