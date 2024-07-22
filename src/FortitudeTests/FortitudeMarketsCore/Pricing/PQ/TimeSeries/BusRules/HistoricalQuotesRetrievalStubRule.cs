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
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeTests.FortitudeCommon.Types;
using static FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules.TimeSeriesBusRulesConstants;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

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

    private Func<SourceTickerIdentifier, UnboundedTimeRange?, IEnumerable<ITickInstant>> quotesCallback;

    public HistoricalQuotesRetrievalStubRule(Func<SourceTickerIdentifier, UnboundedTimeRange?, IEnumerable<ITickInstant>> quotesCallback)
        : base(nameof(HistoricalQuotesRetrievalStubRule)) =>
        this.quotesCallback = quotesCallback;

    public Func<SourceTickerIdentifier, UnboundedTimeRange?, IEnumerable<ITickInstant>> QuotesCallback
    {
        get => quotesCallback;
        set => quotesCallback = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        l0RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<TickInstant>, bool>
            (PricingRepoRetrieveTickInstantRequest, HandleL0HistoricalPriceRequest);
        l1RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<Level1PriceQuote>, bool>
            (PricingRepoRetrieveL1QuoteRequest, HandleL1HistoricalPriceRequest);
        l2RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<Level2PriceQuote>, bool>
            (PricingRepoRetrieveL2QuoteRequest, HandleL2HistoricalPriceRequest);
        l3RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<Level3PriceQuote>, bool>
            (PricingRepoRetrieveL3QuoteRequest, HandleL3HistoricalPriceRequest);
        pql0RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQTickInstant>, bool>
            (PricingRepoRetrievePqTickInstantRequest, HandlePqL0HistoricalPriceRequest);
        pql1RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQLevel1Quote>, bool>
            (PricingRepoRetrievePqL1QuoteRequest, HandlePqL1HistoricalPriceRequest);
        pql2RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQLevel2Quote>, bool>
            (PricingRepoRetrievePqL2QuoteRequest, HandlePqL2HistoricalPriceRequest);
        pql3RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQLevel3Quote>, bool>
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
        (IBusRespondingMessage<HistoricalQuotesRequest<TickInstant>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandleL1HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<Level1PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandleL2HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<Level2PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandleL3HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<Level3PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandlePqL0HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQTickInstant>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandlePqL1HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQLevel1Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandlePqL2HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQLevel2Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    protected virtual bool HandlePqL3HistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQLevel3Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults<TEntry>
        (HistoricalQuotesRequest<TEntry> request) where TEntry : class, ITimeSeriesEntry<TEntry>, ITickInstant, new()
    {
        var results = quotesCallback(request.SourceTickerIdentifier, request.TimeRange);

        if (!results.Any()) return false;

        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextAction<IEnumerable<ITickInstant>, HistoricalQuotesRequest<TEntry>>(this);

        launchContext.Execute(ProcessResults, results, request);
        return true;
    }

    private void ProcessResults<TEntry>(IEnumerable<ITickInstant> results, HistoricalQuotesRequest<TEntry> request)
        where TEntry : class, ITimeSeriesEntry<TEntry>, ITickInstant, new()
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
