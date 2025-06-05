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
using FortitudeMarkets.Pricing.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;

public struct HistoricalCandleStreamRequest
{
    public HistoricalCandleStreamRequest
    (SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod candlePeriod, ChannelPublishRequest<Candle> channelRequest
      , UnboundedTimeRange? timeRange = null, bool inReverseChronologicalOrder = false)
    {
        SourceTickerIdentifier      = sourceTickerIdentifier;
        InReverseChronologicalOrder = inReverseChronologicalOrder;

        CandlePeriod  = candlePeriod;
        TimeRange      = timeRange;
        ChannelRequest = channelRequest;
    }

    public HistoricalCandleStreamRequest
    (PricingInstrumentIdValue pricingInstrument, ChannelPublishRequest<Candle> channelRequest
      , UnboundedTimeRange? timeRange = null, bool inReverseChronologicalOrder = false)
    {
        SourceTickerIdentifier      = pricingInstrument;
        InReverseChronologicalOrder = inReverseChronologicalOrder;

        CandlePeriod  = pricingInstrument.CoveringPeriod.Period;
        TimeRange      = timeRange;
        ChannelRequest = channelRequest;
    }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public TimeBoundaryPeriod  CandlePeriod { get; }
    public UnboundedTimeRange? TimeRange     { get; }

    public bool InReverseChronologicalOrder { get; }

    public ChannelPublishRequest<Candle> ChannelRequest { get; }
}

public struct HistoricalCandleRequestResponse
{
    public HistoricalCandleRequestResponse
    (SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod candlePeriod, UnboundedTimeRange? timeRange = null
      , bool inReverseChronologicalOrder = false)
    {
        SourceTickerIdentifier      = sourceTickerIdentifier;
        InReverseChronologicalOrder = inReverseChronologicalOrder;

        CandlePeriod = candlePeriod;
        TimeRange     = timeRange;
    }

    public HistoricalCandleRequestResponse
        (PricingInstrumentIdValue pricingInstrument, UnboundedTimeRange? timeRange = null, bool inReverseChronologicalOrder = false)
    {
        SourceTickerIdentifier      = pricingInstrument;
        InReverseChronologicalOrder = inReverseChronologicalOrder;

        CandlePeriod = pricingInstrument.CoveringPeriod.Period;
        TimeRange     = timeRange;
    }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public TimeBoundaryPeriod  CandlePeriod { get; }
    public UnboundedTimeRange? TimeRange     { get; }

    public bool InReverseChronologicalOrder { get; }
}

public class HistoricalCandlesRetrievalRule : TimeSeriesRepositoryAccessRule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalCandlesRetrievalRule));

    private ISubscription? requestResponseSubscription;
    private ISubscription? requestStreamSubscription;

    public HistoricalCandlesRetrievalRule
        (TimeSeriesRepositoryParams repositoryParams) : base(repositoryParams, nameof(HistoricalCandlesRetrievalRule)) { }

    public HistoricalCandlesRetrievalRule
        (IRepositoryBuilder repoBuilder) : base(repoBuilder, nameof(HistoricalCandlesRetrievalRule)) { }

    public HistoricalCandlesRetrievalRule
        (ITimeSeriesRepository existingRepository) : base(existingRepository, nameof(HistoricalCandlesRetrievalRule)) { }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        requestStreamSubscription = await this.RegisterRequestListenerAsync<HistoricalCandleStreamRequest, bool>
            (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoStreamRequest, HandleHistoricalPriceStreamRequest);
        requestResponseSubscription = await this.RegisterRequestListenerAsync<HistoricalCandleRequestResponse, List<Candle>>
            (HistoricalQuoteTimeSeriesRepositoryConstants.CandleRepoRequestResponse, HandleHistoricalPricePublishRequestResponse);
        Logger.Info("Started {0} ", nameof(HistoricalCandlesRetrievalRule));
    }

    public override async ValueTask StopAsync()
    {
        await requestStreamSubscription.NullSafeUnsubscribe();
        await requestResponseSubscription.NullSafeUnsubscribe();
        Logger.Info("Stopped {0} ", nameof(HistoricalCandlesRetrievalRule));
        await base.StopAsync();
    }

    private bool HandleHistoricalPriceStreamRequest(IBusRespondingMessage<HistoricalCandleStreamRequest, bool> streamRequestMessage)
    {
        var quoteRequest = streamRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private List<Candle> HandleHistoricalPricePublishRequestResponse
        (IBusRespondingMessage<HistoricalCandleRequestResponse, List<Candle>> requestResponseMessage)
    {
        var quoteRequest = requestResponseMessage.Payload.Body();
        return GetResultsImmediately(quoteRequest).ToList();
    }

    private IInstrument? FindInstrumentFor(SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod candlePeriod)
    {
        var matchingInstruments =
            TimeSeriesRepository
                !.InstrumentFilesMap.Keys
                 .Where(i => i.InstrumentName == sourceTickerIdentifier.InstrumentName
                          && i.SourceName == sourceTickerIdentifier.SourceName && i.CoveringPeriod == candlePeriod)
                 .ToList();
        return matchingInstruments.Count != 1 ? null : matchingInstruments[0];
    }

    private IEnumerable<Candle> GetResultsImmediately(HistoricalCandleRequestResponse responseRequest)
    {
        var instrument = FindInstrumentFor(responseRequest.SourceTickerIdentifier, responseRequest.CandlePeriod);
        if (instrument == null) return Enumerable.Empty<Candle>();
        var readerSession = TimeSeriesRepository!.GetReaderSession<Candle>(instrument, responseRequest.TimeRange);
        if (readerSession == null) return Enumerable.Empty<Candle>();
        var readerContext = !responseRequest.InReverseChronologicalOrder
            ? readerSession.ChronologicalEntriesBetweenTimeRangeReader<Candle>
                (Context.PooledRecycler, responseRequest.TimeRange, EntryResultSourcing.FromRecycler, ReaderOptions.ReadFastAsPossible)
            : readerSession.ReverseChronologicalEntriesBetweenTimeRangeReader<Candle>
                (Context.PooledRecycler, responseRequest.TimeRange);

        return readerContext.ResultEnumerable;
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults(HistoricalCandleStreamRequest streamRequest)
    {
        var instrument = FindInstrumentFor(streamRequest.SourceTickerIdentifier, streamRequest.CandlePeriod);
        if (instrument == null) return false;
        var readerSession = TimeSeriesRepository!.GetReaderSession<Candle>(instrument, streamRequest.TimeRange);
        if (readerSession == null) return false;

        var readerContext = !streamRequest.InReverseChronologicalOrder
            ? readerSession.ChronologicalEntriesBetweenTimeRangeReader<Candle>
                (Context.PooledRecycler, streamRequest.TimeRange, EntryResultSourcing.FromRecycler, ReaderOptions.ReadFastAsPossible)
            : readerSession.ReverseChronologicalEntriesBetweenTimeRangeReader<Candle>(Context.PooledRecycler, streamRequest.TimeRange);
        if (streamRequest.ChannelRequest.BatchSize > 1) readerContext.BatchLimit   = streamRequest.ChannelRequest.BatchSize;
        if (streamRequest.ChannelRequest.ResultLimit > 0) readerContext.MaxResults = streamRequest.ChannelRequest.ResultLimit;
        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextAction<IReaderContext<Candle>, HistoricalCandleStreamRequest>(this);

        launchContext.Execute(ProcessResults, readerContext, streamRequest);
        return true;
    }

    private void ProcessResults(IReaderContext<Candle> readerContext, HistoricalCandleStreamRequest streamRequest)
    {
        var channel = streamRequest.ChannelRequest.PublishChannel;
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
            Logger.Warn("Caught exception when attempting to run {0} on TimeSeriesRepository {1}. Got {2}", streamRequest
                      , TimeSeriesRepository!.RepoRootDirectory.DirPath, ex);
        }
        finally
        {
            readerContext.CloseReaderSession();
            channel.PublishComplete(this);
        }
    }
}
