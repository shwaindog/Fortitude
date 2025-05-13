// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;
using FortitudeTests.FortitudeCommon.Types;
using static FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules.HistoricalQuoteTimeSeriesRepositoryConstants;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;

[NoMatchingProductionClass]
public class HistoricalQuotesRetrievalStubRule : Rule
{
    private ISubscription? l0RequestListenerSubscription;
    private ISubscription? l1RequestListenerSubscription;
    private ISubscription? l2RequestListenerSubscription;
    private ISubscription? l3RequestListenerSubscription;
    private ISubscription? pql0RequestListenerSubscription;
    private ISubscription? pql1RequestListenerSubscription;
    private ISubscription? pql2RequestListenerSubscription;
    private ISubscription? pql3RequestListenerSubscription;

    private Func<SourceTickerIdentifier, UnboundedTimeRange?, IEnumerable<IPublishableTickInstant>> quotesCallback;

    public HistoricalQuotesRetrievalStubRule(Func<SourceTickerIdentifier, UnboundedTimeRange?, IEnumerable<IPublishableTickInstant>> quotesCallback)
        : base(nameof(HistoricalQuotesRetrievalStubRule)) =>
        this.quotesCallback = quotesCallback;

    public Func<SourceTickerIdentifier, UnboundedTimeRange?, IEnumerable<IPublishableTickInstant>> QuotesCallback
    {
        get => quotesCallback;
        set => quotesCallback = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        l0RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PublishableTickInstant>, bool>
            (PricingRepoRetrieveTickInstantRequest, HandleL0HistoricalPriceRequest);
        l1RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PublishableLevel1PriceQuote>, bool>
            (PricingRepoRetrieveL1QuoteRequest, HandleL1HistoricalPriceRequest);
        l2RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PublishableLevel2PriceQuote>, bool>
            (PricingRepoRetrieveL2QuoteRequest, HandleL2HistoricalPriceRequest);
        l3RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PublishableLevel3PriceQuote>, bool>
            (PricingRepoRetrieveL3QuoteRequest, HandleL3HistoricalPriceRequest);
        pql0RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableTickInstant>, bool>
            (PricingRepoRetrievePqTickInstantRequest, HandlePqL0HistoricalPriceRequest);
        pql1RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableLevel1Quote>, bool>
            (PricingRepoRetrievePqL1QuoteRequest, HandlePqL1HistoricalPriceRequest);
        pql2RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableLevel2Quote>, bool>
            (PricingRepoRetrievePqL2QuoteRequest, HandlePqL2HistoricalPriceRequest);
        pql3RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableLevel3Quote>, bool>
            (PricingRepoRetrievePqL3QuoteRequest, HandlePqL3HistoricalPriceRequest);
    }

    public override void Stop()
    {
        l0RequestListenerSubscription?.Unsubscribe();
        l1RequestListenerSubscription?.Unsubscribe();
        l2RequestListenerSubscription?.Unsubscribe();
        l3RequestListenerSubscription?.Unsubscribe();
        pql0RequestListenerSubscription?.Unsubscribe();
        pql1RequestListenerSubscription?.Unsubscribe();
        pql2RequestListenerSubscription?.Unsubscribe();
        pql3RequestListenerSubscription?.Unsubscribe();
        base.Stop();
    }

    protected virtual bool HandleL0HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PublishableTickInstant>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandleL1HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PublishableLevel1PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandleL2HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PublishableLevel2PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandleL3HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PublishableLevel3PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandlePqL0HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQPublishableTickInstant>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandlePqL1HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQPublishableLevel1Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandlePqL2HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQPublishableLevel2Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandlePqL3HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQPublishableLevel3Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults<TEntry>
        (HistoricalQuotesRequest<TEntry> request) where TEntry : class, ITimeSeriesEntry, IPublishableTickInstant, new()
    {
        var results = quotesCallback(request.SourceTickerIdentifier, request.TimeRange);

        if (!results.Any()) return false;

        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextAction<IEnumerable<IPublishableTickInstant>, HistoricalQuotesRequest<TEntry>>(this);

        launchContext.Execute(ProcessResults, results, request);
        return true;
    }

    private void ProcessResults<TEntry>(IEnumerable<IPublishableTickInstant> results, HistoricalQuotesRequest<TEntry> request)
        where TEntry : class, ITimeSeriesEntry, IPublishableTickInstant, new()
    {
        var channel = request.ChannelRequest.PublishChannel;
        try
        {
            if (request.ChannelRequest.BatchSize > 1)

                foreach (var batchResults in results.Select(q => new TEntry().CopyFrom(q)).OfType<TEntry>().Chunk(request.ChannelRequest.BatchSize))
                    channel.Publish(this, batchResults);
            else
                foreach (var batchResults in results.Select(q => new TEntry().CopyFrom(q)).OfType<TEntry>())
                    channel.Publish(this, batchResults);
        }
        finally
        {
            channel.PublishComplete(this);
        }
    }
}
