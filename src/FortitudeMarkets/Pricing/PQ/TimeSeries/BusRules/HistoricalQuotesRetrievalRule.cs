// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using static FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules.HistoricalQuoteTimeSeriesRepositoryConstants;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;

public struct HistoricalQuotesRequest<TEntry> where TEntry : class, ITimeSeriesEntry, ITickInstant
{
    public HistoricalQuotesRequest
    (SourceTickerIdentifier sourceTickerIdentifier, ChannelPublishRequest<TEntry> channelRequest, UnboundedTimeRange? timeRange = null
      , bool inReverseChronologicalOrder = false)
    {
        SourceTickerIdentifier      = sourceTickerIdentifier;
        InReverseChronologicalOrder = inReverseChronologicalOrder;

        TimeRange      = timeRange;
        ChannelRequest = channelRequest;
    }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public UnboundedTimeRange? TimeRange { get; }

    public ChannelPublishRequest<TEntry> ChannelRequest { get; }

    public bool InReverseChronologicalOrder { get; }

    public string RequestAddress { get; } = CalculateRequestAddress();

    public static string CalculateRequestAddress()
    {
        var typeOfTEntry = typeof(TEntry);
        if (typeOfTEntry == typeof(TickInstant)) return PricingRepoRetrieveTickInstantRequest;
        if (typeOfTEntry == typeof(PublishableLevel1PriceQuote)) return PricingRepoRetrieveL1QuoteRequest;
        if (typeOfTEntry == typeof(PublishableLevel2PriceQuote)) return PricingRepoRetrieveL2QuoteRequest;
        if (typeOfTEntry == typeof(PublishableLevel3PriceQuote)) return PricingRepoRetrieveL3QuoteRequest;
        if (typeOfTEntry == typeof(PQPublishableTickInstant)) return PricingRepoRetrievePqTickInstantRequest;
        if (typeOfTEntry == typeof(PQPublishableLevel1Quote)) return PricingRepoRetrievePqL1QuoteRequest;
        if (typeOfTEntry == typeof(PQPublishableLevel2Quote)) return PricingRepoRetrievePqL2QuoteRequest;
        if (typeOfTEntry == typeof(PQPublishableLevel3Quote)) return PricingRepoRetrievePqL3QuoteRequest;
        throw new Exception("Unexpected Quote type received");
    }
}

public class HistoricalQuotesRetrievalRule : TimeSeriesRepositoryAccessRule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalQuotesRetrievalRule));

    private ISubscription? l1RequestSubscription;
    private ISubscription? l2RequestSubscription;
    private ISubscription? l3RequestSubscription;
    private ISubscription? pql1RequestSubscription;
    private ISubscription? pql2RequestSubscription;
    private ISubscription? pql3RequestSubscription;

    private ISubscription? pqTickInstantRequestSubscription;
    private ISubscription? tickInstantRequestSubscription;

    public HistoricalQuotesRetrievalRule
        (TimeSeriesRepositoryParams repositoryParams) : base(repositoryParams, nameof(HistoricalQuotesRetrievalRule)) { }

    public HistoricalQuotesRetrievalRule(IRepositoryBuilder repoBuilder) : base(repoBuilder, nameof(HistoricalQuotesRetrievalRule)) { }

    public HistoricalQuotesRetrievalRule
        (ITimeSeriesRepository existingRepository) : base(existingRepository, nameof(HistoricalQuotesRetrievalRule)) { }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        tickInstantRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<TickInstant>, bool>
            (PricingRepoRetrieveTickInstantRequest, HandleTickInstantHistoricalPriceRequest);
        l1RequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PublishableLevel1PriceQuote>, bool>
            (PricingRepoRetrieveL1QuoteRequest, HandleL1HistoricalPriceRequest);
        l2RequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PublishableLevel2PriceQuote>, bool>
            (PricingRepoRetrieveL2QuoteRequest, HandleL2HistoricalPriceRequest);
        l3RequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PublishableLevel3PriceQuote>, bool>
            (PricingRepoRetrieveL3QuoteRequest, HandleL3HistoricalPriceRequest);
        pqTickInstantRequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableTickInstant>, bool>
            (PricingRepoRetrievePqTickInstantRequest, HandlePqTickInstantHistoricalPriceRequest);
        pql1RequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableLevel1Quote>, bool>
            (PricingRepoRetrievePqL1QuoteRequest, HandlePqL1HistoricalPriceRequest);
        pql2RequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableLevel2Quote>, bool>
            (PricingRepoRetrievePqL2QuoteRequest, HandlePqL2HistoricalPriceRequest);
        pql3RequestSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQPublishableLevel3Quote>, bool>
            (PricingRepoRetrievePqL3QuoteRequest, HandlePqL3HistoricalPriceRequest);
        Logger.Info("Started {0} ", nameof(HistoricalQuotesRetrievalRule));
    }

    public override void Stop()
    {
        tickInstantRequestSubscription?.Unsubscribe();
        l1RequestSubscription?.Unsubscribe();
        l2RequestSubscription?.Unsubscribe();
        l3RequestSubscription?.Unsubscribe();
        pqTickInstantRequestSubscription?.Unsubscribe();
        pql1RequestSubscription?.Unsubscribe();
        pql2RequestSubscription?.Unsubscribe();
        pql3RequestSubscription?.Unsubscribe();
        Logger.Info("Stopped {0} ", nameof(HistoricalQuotesRetrievalRule));
        base.Stop();
    }

    private IInstrument? FindInstrumentFor(SourceTickerIdentifier sourceTickerIdentifier)
    {
        var matchingInstruments =
            TimeSeriesRepository!.InstrumentFilesMap.Keys
                                 .Where(i => i.InstrumentName == sourceTickerIdentifier.Ticker && i.SourceName == sourceTickerIdentifier.Source)
                                 .ToList();
        return matchingInstruments.Count != 1 ? null : matchingInstruments[0];
    }

    private bool HandleTickInstantHistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<TickInstant>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandleL1HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PublishableLevel1PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandleL2HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PublishableLevel2PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandleL3HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PublishableLevel3PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandlePqTickInstantHistoricalPriceRequest
        (IBusRespondingMessage<HistoricalQuotesRequest<PQPublishableTickInstant>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandlePqL1HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PQPublishableLevel1Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandlePqL2HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PQPublishableLevel2Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandlePqL3HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PQPublishableLevel3Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults<TEntry>
        (HistoricalQuotesRequest<TEntry> request) where TEntry : class, ITimeSeriesEntry, ITickInstant, new()
    {
        var instrument = FindInstrumentFor(request.SourceTickerIdentifier);
        if (instrument == null) return false;
        var readerSession = TimeSeriesRepository!.GetReaderSession<TEntry>(instrument, request.TimeRange);
        if (readerSession == null) return false;

        var readerContext = !request.InReverseChronologicalOrder
            ? readerSession.ChronologicalEntriesBetweenTimeRangeReader<TEntry>
                (Context.PooledRecycler, request.TimeRange, EntryResultSourcing.FromRecycler, ReaderOptions.ReadFastAsPossible)
            : readerSession.ReverseChronologicalEntriesBetweenTimeRangeReader<TEntry>(Context.PooledRecycler, request.TimeRange);

        if (request.ChannelRequest.BatchSize > 1) readerContext.BatchLimit   = request.ChannelRequest.BatchSize;
        if (request.ChannelRequest.ResultLimit > 0) readerContext.MaxResults = request.ChannelRequest.ResultLimit;
        var launchContext = Context.GetEventQueues(MessageQueueType.Worker)
                                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                                   .GetExecutionContextAction<IReaderContext<TEntry>, HistoricalQuotesRequest<TEntry>>(this);

        launchContext.Execute(ProcessResults, readerContext, request);
        return true;
    }

    private void ProcessResults<TEntry>(IReaderContext<TEntry> readerContext, HistoricalQuotesRequest<TEntry> request)
        where TEntry : class, ITimeSeriesEntry, ITickInstant, new()
    {
        var channel = request.ChannelRequest.PublishChannel;
        try
        {
            if (readerContext.BatchLimit > 1)
                foreach (var batchResults in readerContext.BatchedResultEnumerable)
                    channel.Publish(this, batchResults);
            else
                foreach (var entry in readerContext.ResultEnumerable)
                    channel.Publish(this, entry);
        }
        catch (Exception ex)
        {
            Logger.Warn("Caught exception when attempting to run {0} on TimeSeriesRepository {1}. Got {2}", request
                      , TimeSeriesRepository!.RepoRootDirectory.DirPath, ex);
        }
        finally
        {
            channel.PublishComplete(this);
        }
    }
}
