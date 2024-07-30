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
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

public struct HistoricalPricePeriodSummaryStreamRequest
{
    public HistoricalPricePeriodSummaryStreamRequest
    (SourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod entryPeriod, ChannelPublishRequest<PricePeriodSummary> channelRequest
      , UnboundedTimeRange? timeRange = null)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;

        EntryPeriod    = entryPeriod;
        TimeRange      = timeRange;
        ChannelRequest = channelRequest;
    }

    public HistoricalPricePeriodSummaryStreamRequest
    (PricingInstrumentId pricingInstrument, ChannelPublishRequest<PricePeriodSummary> channelRequest
      , UnboundedTimeRange? timeRange = null)
    {
        SourceTickerIdentifier = pricingInstrument;

        EntryPeriod    = pricingInstrument.EntryPeriod;
        TimeRange      = timeRange;
        ChannelRequest = channelRequest;
    }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public TimeSeriesPeriod    EntryPeriod { get; }
    public UnboundedTimeRange? TimeRange   { get; }

    public ChannelPublishRequest<PricePeriodSummary> ChannelRequest { get; }
}

public struct HistoricalPricePeriodSummaryRequestResponse
{
    public HistoricalPricePeriodSummaryRequestResponse
        (SourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod entryPeriod, UnboundedTimeRange? timeRange = null)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;

        EntryPeriod = entryPeriod;
        TimeRange   = timeRange;
    }

    public HistoricalPricePeriodSummaryRequestResponse
        (PricingInstrumentId pricingInstrument, UnboundedTimeRange? timeRange = null)
    {
        SourceTickerIdentifier = pricingInstrument;

        EntryPeriod = pricingInstrument.EntryPeriod;
        TimeRange   = timeRange;
    }

    public SourceTickerIdentifier SourceTickerIdentifier { get; }

    public TimeSeriesPeriod    EntryPeriod { get; }
    public UnboundedTimeRange? TimeRange   { get; }
}

public class HistoricalPricePeriodSummaryRetrievalRule : TimeSeriesRepositoryAccessRule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalPricePeriodSummaryRetrievalRule));

    private ISubscription? requestResponseSubscription;
    private ISubscription? requestStreamSubscription;

    public HistoricalPricePeriodSummaryRetrievalRule
        (TimeSeriesRepositoryParams repositoryParams) : base(repositoryParams, nameof(HistoricalPricePeriodSummaryRetrievalRule)) { }

    public HistoricalPricePeriodSummaryRetrievalRule
        (IRepositoryBuilder repoBuilder) : base(repoBuilder, nameof(HistoricalPricePeriodSummaryRetrievalRule)) { }

    public HistoricalPricePeriodSummaryRetrievalRule
        (ITimeSeriesRepository existingRepository) : base(existingRepository, nameof(HistoricalPricePeriodSummaryRetrievalRule)) { }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();
        requestStreamSubscription = await this.RegisterRequestListenerAsync<HistoricalPricePeriodSummaryStreamRequest, bool>
            (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoStreamRequest, HandleHistoricalPriceStreamRequest);
        requestResponseSubscription = await this.RegisterRequestListenerAsync<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>>
            (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, HandleHistoricalPricePublishRequestResponse);
        Logger.Info("Started {0} ", nameof(HistoricalPricePeriodSummaryRetrievalRule));
    }

    public override async ValueTask StopAsync()
    {
        await requestStreamSubscription.NullSafeUnsubscribe();
        await requestResponseSubscription.NullSafeUnsubscribe();
        Logger.Info("Stopped {0} ", nameof(HistoricalPricePeriodSummaryRetrievalRule));
        await base.StopAsync();
    }

    private bool HandleHistoricalPriceStreamRequest
        (IBusRespondingMessage<HistoricalPricePeriodSummaryStreamRequest, bool> streamRequestMessage)
    {
        var quoteRequest = streamRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private List<PricePeriodSummary> HandleHistoricalPricePublishRequestResponse
        (IBusRespondingMessage<HistoricalPricePeriodSummaryRequestResponse, List<PricePeriodSummary>> requestResponseMessage)
    {
        var quoteRequest = requestResponseMessage.Payload.Body();
        return GetResultsImmediately(quoteRequest).ToList();
    }

    private IInstrument? FindInstrumentFor(SourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod entryPeriod)
    {
        var matchingInstruments =
            TimeSeriesRepository
                !.InstrumentFilesMap.Keys
                 .Where(i => i.InstrumentName == sourceTickerIdentifier.Ticker
                          && i.InstrumentSource == sourceTickerIdentifier.Source && i.EntryPeriod == entryPeriod)
                 .ToList();
        return matchingInstruments.Count != 1 ? null : matchingInstruments[0];
    }

    private IEnumerable<PricePeriodSummary> GetResultsImmediately(HistoricalPricePeriodSummaryRequestResponse responseRequest)
    {
        var instrument = FindInstrumentFor(responseRequest.SourceTickerIdentifier, responseRequest.EntryPeriod);
        if (instrument == null) return Enumerable.Empty<PricePeriodSummary>();
        var readerSession = TimeSeriesRepository!.GetReaderSession<PricePeriodSummary>(instrument, responseRequest.TimeRange);
        if (readerSession == null) return Enumerable.Empty<PricePeriodSummary>();
        var readerContext = readerSession.ChronologicalEntriesBetweenTimeRangeReader
            (Context.PooledRecycler, responseRequest.TimeRange, EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible
           , () => new PricePeriodSummary());

        return readerContext.ResultEnumerable;
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults(HistoricalPricePeriodSummaryStreamRequest streamRequest)
    {
        var instrument = FindInstrumentFor(streamRequest.SourceTickerIdentifier, streamRequest.EntryPeriod);
        if (instrument == null) return false;
        var readerSession = TimeSeriesRepository!.GetReaderSession<PricePeriodSummary>(instrument, streamRequest.TimeRange);
        if (readerSession == null) return false;
        var readerContext = readerSession.ChronologicalEntriesBetweenTimeRangeReader
            (Context.PooledRecycler, streamRequest.TimeRange, EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible
           , streamRequest.ChannelRequest.PublishChannel.GetChannelEventOrNew<PricePeriodSummary>());

        if (streamRequest.ChannelRequest.BatchSize > 1) readerContext.BatchLimit   = streamRequest.ChannelRequest.BatchSize;
        if (streamRequest.ChannelRequest.ResultLimit > 0) readerContext.MaxResults = streamRequest.ChannelRequest.ResultLimit;
        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextAction<IReaderContext<PricePeriodSummary>, HistoricalPricePeriodSummaryStreamRequest>(this);

        launchContext.Execute(ProcessResults, readerContext, streamRequest);
        return true;
    }

    private void ProcessResults(IReaderContext<PricePeriodSummary> readerContext, HistoricalPricePeriodSummaryStreamRequest streamRequest)
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
