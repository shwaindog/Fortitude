// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;

public class HistoricalCandleRetrievalStubRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalCandlesRetrievalRule));

    private ISubscription? requestResponseSubscription;
    private ISubscription? requestStreamSubscription;

    private Func<SourceTickerIdentifier, TimeBoundaryPeriod, UnboundedTimeRange?, IEnumerable<Candle>> summariesCallback;

    public HistoricalCandleRetrievalStubRule
        (Func<SourceTickerIdentifier, TimeBoundaryPeriod, UnboundedTimeRange?, IEnumerable<Candle>> stubRetrieveSummariesCallback)
        : base(nameof(HistoricalCandleRetrievalStubRule)) =>
        summariesCallback = stubRetrieveSummariesCallback;

    public Func<SourceTickerIdentifier, TimeBoundaryPeriod, UnboundedTimeRange?, IEnumerable<Candle>> SummariesCallback
    {
        get => summariesCallback;
        set => summariesCallback = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        requestStreamSubscription = await this.RegisterRequestListenerAsync<HistoricalCandleStreamRequest, bool>
            (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoStreamRequest, HandleHistoricalPriceStreamRequest);
        requestResponseSubscription = await this.RegisterRequestListenerAsync<HistoricalCandleRequestResponse, List<Candle>>
            (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoRequestResponse, HandleHistoricalPricePublishRequestResponse);
    }

    protected virtual bool HandleHistoricalPriceStreamRequest
        (IBusRespondingMessage<HistoricalCandleStreamRequest, bool> streamRequestMessage)
    {
        var quoteRequest = streamRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual List<Candle> HandleHistoricalPricePublishRequestResponse
        (IBusRespondingMessage<HistoricalCandleRequestResponse, List<Candle>> requestResponseMessage)
    {
        var summaryRequest = requestResponseMessage.Payload.Body();
        return summariesCallback(summaryRequest.SourceTickerIdentifier, summaryRequest.CandlePeriod, summaryRequest.TimeRange).ToList();
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults(HistoricalCandleStreamRequest streamRequest)
    {
        var results = summariesCallback(streamRequest.SourceTickerIdentifier, streamRequest.CandlePeriod, streamRequest.TimeRange);

        if (!results.Any()) return false;

        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextAction<IEnumerable<Candle>, HistoricalCandleStreamRequest>(this);

        launchContext.Execute(ProcessResults, results, streamRequest);
        return true;
    }

    private void ProcessResults(IEnumerable<Candle> results, HistoricalCandleStreamRequest streamRequest)
    {
        var channel = streamRequest.ChannelRequest.PublishChannel;
        try
        {
            if (streamRequest.ChannelRequest.BatchSize > 1)

                foreach (var batchResults in results.Chunk(streamRequest.ChannelRequest.BatchSize))
                    channel.Publish(this, batchResults);
            else
                foreach (var batchResults in results)
                    channel.Publish(this, batchResults);
        }
        finally
        {
            channel.PublishComplete(this);
        }
    }
}
