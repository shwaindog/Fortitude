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
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

public class HistoricalPricePeriodSummaryRetrievalStubRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalPricePeriodSummaryRetrievalRule));

    private ISubscription? requestResponseSubscription;
    private ISubscription? requestStreamSubscription;

    private Func<SourceTickerId, TimeSeriesPeriod, UnboundedTimeRange?, IEnumerable<PricePeriodSummary>> summariesCallback;

    public HistoricalPricePeriodSummaryRetrievalStubRule
        (Func<SourceTickerId, TimeSeriesPeriod, UnboundedTimeRange?, IEnumerable<PricePeriodSummary>> stubRetrieveSummariesCallback)
        : base(nameof(HistoricalPricePeriodSummaryRetrievalStubRule)) =>
        summariesCallback = stubRetrieveSummariesCallback;

    public Func<SourceTickerId, TimeSeriesPeriod, UnboundedTimeRange?, IEnumerable<PricePeriodSummary>> SummariesCallback
    {
        get => summariesCallback;
        set => summariesCallback = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        requestStreamSubscription = await this.RegisterRequestListenerAsync<HistoricalPricePeriodSummaryStreamRequest, bool>
            (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoStreamRequest, HandleHistoricalPriceStreamRequest);
        requestResponseSubscription = await this.RegisterRequestListenerAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
            (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, HandleHistoricalPricePublishRequestResponse);
    }

    protected virtual bool HandleHistoricalPriceStreamRequest
        (IBusRespondingMessage<HistoricalPricePeriodSummaryStreamRequest, bool> streamRequestMessage)
    {
        var quoteRequest = streamRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual List<PricePeriodSummary> HandleHistoricalPricePublishRequestResponse
        (IBusRespondingMessage<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>> requestResponseMessage)
    {
        var summaryRequest = requestResponseMessage.Payload.Body();
        return summariesCallback(summaryRequest.TickerId, summaryRequest.EntryPeriod, summaryRequest.TimeRange).ToList();
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults(HistoricalPricePeriodSummaryStreamRequest streamRequest)
    {
        var results = summariesCallback(streamRequest.TickerId, streamRequest.EntryPeriod, streamRequest.TimeRange);

        if (!results.Any()) return false;

        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextAction<IEnumerable<PricePeriodSummary>, HistoricalPricePeriodSummaryStreamRequest>(this);

        launchContext.Execute(ProcessResults, results, streamRequest);
        return true;
    }

    private void ProcessResults(IEnumerable<PricePeriodSummary> results, HistoricalPricePeriodSummaryStreamRequest streamRequest)
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
