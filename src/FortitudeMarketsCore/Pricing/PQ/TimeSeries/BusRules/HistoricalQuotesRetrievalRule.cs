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
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules.TimeSeriesBusRulesConstants;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

public struct HistoricalQuotesRequest<TEntry> where TEntry : class, ITimeSeriesEntry<TEntry>, ILevel0Quote
{
    public HistoricalQuotesRequest
        (SourceTickerIdentifier sourceTickerIdentifier, ChannelPublishRequest<TEntry> channelRequest, UnboundedTimeRange? timeRange = null)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;

        TimeRange      = timeRange;
        ChannelRequest = channelRequest;
    }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public UnboundedTimeRange? TimeRange { get; }

    public ChannelPublishRequest<TEntry> ChannelRequest { get; }

    public string RequestAddress { get; } = CalculateRequestAddress();

    public static string CalculateRequestAddress()
    {
        var typeOfTEntry = typeof(TEntry);
        if (typeOfTEntry == typeof(Level0PriceQuote)) return PricingRepoRetrieveL0Request;
        if (typeOfTEntry == typeof(Level1PriceQuote)) return PricingRepoRetrieveL1Request;
        if (typeOfTEntry == typeof(Level2PriceQuote)) return PricingRepoRetrieveL2Request;
        if (typeOfTEntry == typeof(Level3PriceQuote)) return PricingRepoRetrieveL3Request;
        if (typeOfTEntry == typeof(PQLevel0Quote)) return PricingRepoRetrievePqL0Request;
        if (typeOfTEntry == typeof(PQLevel1Quote)) return PricingRepoRetrievePqL1Request;
        if (typeOfTEntry == typeof(PQLevel2Quote)) return PricingRepoRetrievePqL2Request;
        if (typeOfTEntry == typeof(PQLevel3Quote)) return PricingRepoRetrievePqL3Request;
        throw new Exception("Unexpected Quote type received");
    }
}

public class HistoricalQuotesRetrievalRule : TimeSeriesRepositoryAccessRule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalQuotesRetrievalRule));

    private ISubscription? l0RequestListenerSubscription;
    private ISubscription? l1RequestListenerSubscription;
    private ISubscription? l2RequestListenerSubscription;
    private ISubscription? l3RequestListenerSubscription;
    private ISubscription? pql0RequestListenerSubscription;
    private ISubscription? pql1RequestListenerSubscription;
    private ISubscription? pql2RequestListenerSubscription;
    private ISubscription? pql3RequestListenerSubscription;

    public HistoricalQuotesRetrievalRule
        (TimeSeriesRepositoryParams repositoryParams) : base(repositoryParams, nameof(HistoricalQuotesRetrievalRule)) { }

    public HistoricalQuotesRetrievalRule(IRepositoryBuilder repoBuilder) : base(repoBuilder, nameof(HistoricalQuotesRetrievalRule)) { }

    public HistoricalQuotesRetrievalRule
        (ITimeSeriesRepository existingRepository) : base(existingRepository, nameof(HistoricalQuotesRetrievalRule)) { }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        l0RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<Level0PriceQuote>, bool>
            (PricingRepoRetrieveL0Request, HandleL0HistoricalPriceRequest);
        l1RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<Level1PriceQuote>, bool>
            (PricingRepoRetrieveL1Request, HandleL1HistoricalPriceRequest);
        l2RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<Level2PriceQuote>, bool>
            (PricingRepoRetrieveL2Request, HandleL2HistoricalPriceRequest);
        l3RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<Level3PriceQuote>, bool>
            (PricingRepoRetrieveL3Request, HandleL3HistoricalPriceRequest);
        pql0RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQLevel0Quote>, bool>
            (PricingRepoRetrievePqL0Request, HandlePqL0HistoricalPriceRequest);
        pql1RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQLevel1Quote>, bool>
            (PricingRepoRetrievePqL1Request, HandlePqL1HistoricalPriceRequest);
        pql2RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQLevel2Quote>, bool>
            (PricingRepoRetrievePqL2Request, HandlePqL2HistoricalPriceRequest);
        pql3RequestListenerSubscription = await this.RegisterRequestListenerAsync<HistoricalQuotesRequest<PQLevel3Quote>, bool>
            (PricingRepoRetrievePqL3Request, HandlePqL3HistoricalPriceRequest);
        Logger.Info("Started {0} ", nameof(HistoricalQuotesRetrievalRule));
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
        Logger.Info("Stopped {0} ", nameof(HistoricalQuotesRetrievalRule));
        base.Stop();
    }

    private IInstrument? FindInstrumentFor(SourceTickerIdentifier sourceTickerIdentifier)
    {
        var matchingInstruments =
            TimeSeriesRepository!.InstrumentFilesMap.Keys
                                 .Where(i => i.InstrumentName == sourceTickerIdentifier.Ticker && i.InstrumentSource == sourceTickerIdentifier.Source)
                                 .ToList();
        return matchingInstruments.Count != 1 ? null : matchingInstruments[0];
    }

    private bool HandleL0HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<Level0PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandleL1HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<Level1PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandleL2HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<Level2PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandleL3HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<Level3PriceQuote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandlePqL0HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PQLevel0Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandlePqL1HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PQLevel1Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandlePqL2HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PQLevel2Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool HandlePqL3HistoricalPriceRequest(IBusRespondingMessage<HistoricalQuotesRequest<PQLevel3Quote>, bool> historicalQuotesRequestMessage)
    {
        var quoteRequest = historicalQuotesRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults<TEntry>
        (HistoricalQuotesRequest<TEntry> request) where TEntry : class, ITimeSeriesEntry<TEntry>, ILevel0Quote, new()
    {
        var instrument = FindInstrumentFor(request.SourceTickerIdentifier);
        if (instrument == null) return false;
        var readerSession = TimeSeriesRepository!.GetReaderSession<TEntry>(instrument, request.TimeRange);
        if (readerSession == null) return false;
        var readerContext = readerSession.GetEntriesBetweenReader
            (request.TimeRange, EntryResultSourcing.FromFactoryFuncUnlimited, request.ChannelRequest.PublishChannel.GetChannelEventOrNew<TEntry>());

        if (request.ChannelRequest.BatchSize > 1) readerContext.BatchLimit   = request.ChannelRequest.BatchSize;
        if (request.ChannelRequest.ResultLimit > 0) readerContext.MaxResults = request.ChannelRequest.ResultLimit;
        var launchContext = Context.GetEventQueues(MessageQueueType.Worker)
                                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                                   .GetExecutionContextAction<IReaderContext<TEntry>, HistoricalQuotesRequest<TEntry>>(this);

        launchContext.Execute(ProcessResults, readerContext, request);
        return true;
    }

    private void ProcessResults<TEntry>(IReaderContext<TEntry> readerContext, HistoricalQuotesRequest<TEntry> request)
        where TEntry : class, ITimeSeriesEntry<TEntry>, ILevel0Quote, new()
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
