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
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;

public struct HistoricalPricePeriodSummaryStreamRequest
{
    public HistoricalPricePeriodSummaryStreamRequest
    (ISourceTickerIdentifier tickerId, TimeSeriesPeriod entryPeriod, ChannelPublishRequest<IPricePeriodSummary> channelRequest
      , UnboundedTimeRange? timeRange = null)
    {
        TickerId       = tickerId;
        EntryPeriod    = entryPeriod;
        TimeRange      = timeRange;
        ChannelRequest = channelRequest;
    }

    public ISourceTickerIdentifier TickerId    { get; }
    public TimeSeriesPeriod        EntryPeriod { get; }
    public UnboundedTimeRange?     TimeRange   { get; }

    public ChannelPublishRequest<IPricePeriodSummary> ChannelRequest { get; }
}

public struct HistoricalPricePeriodSummaryRequestResponse
{
    public HistoricalPricePeriodSummaryRequestResponse
        (ISourceTickerIdentifier tickerId, TimeSeriesPeriod entryPeriod, UnboundedTimeRange? timeRange = null)
    {
        TickerId    = tickerId;
        EntryPeriod = entryPeriod;
        TimeRange   = timeRange;
    }

    public ISourceTickerIdentifier TickerId    { get; }
    public TimeSeriesPeriod        EntryPeriod { get; }
    public UnboundedTimeRange?     TimeRange   { get; }
}

public class HistoricalPricePeriodSummaryRetrievalRule : TimeSeriesRepositoryAccessRule
{
    private static readonly IFLogger       Logger = FLoggerFactory.Instance.GetLogger(typeof(HistoricalPricePeriodSummaryRetrievalRule));
    private                 ISubscription? requestResponseSubscription;

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
        requestResponseSubscription = await this.RegisterRequestListenerAsync<HistoricalPricePeriodSummaryRequestResponse, List<IPricePeriodSummary>>
            (TimeSeriesBusRulesConstants.PricePeriodSummaryRepoRequestResponse, HandleHistoricalPricePublishRequestResponse);
        Logger.Info("Started {0} ", nameof(HistoricalPricePeriodSummaryRetrievalRule));
    }

    public override async ValueTask StopAsync()
    {
        if (requestStreamSubscription != null) await requestStreamSubscription.UnsubscribeAsync();
        if (requestResponseSubscription != null) await requestResponseSubscription.UnsubscribeAsync();
        Logger.Info("Stopped {0} ", nameof(HistoricalPricePeriodSummaryRetrievalRule));
        await base.StopAsync();
    }

    private bool HandleHistoricalPriceStreamRequest
        (IBusRespondingMessage<HistoricalPricePeriodSummaryStreamRequest, bool> streamRequestMessage)
    {
        var quoteRequest = streamRequestMessage.Payload.Body();
        return MakeTimeSeriesRepoCallReturnExpectResults(quoteRequest);
    }

    private List<IPricePeriodSummary> HandleHistoricalPricePublishRequestResponse
        (IBusRespondingMessage<HistoricalPricePeriodSummaryRequestResponse, List<IPricePeriodSummary>> requestResponseMessage)
    {
        var quoteRequest = requestResponseMessage.Payload.Body();
        return GetResultsImmediately(quoteRequest).ToList();
    }

    private IInstrument? FindInstrumentFor(ISourceTickerIdentifier tickerId, TimeSeriesPeriod entryPeriod)
    {
        var matchingInstruments =
            TimeSeriesRepository!.InstrumentFilesMap.Keys
                                 .Where(i => i.InstrumentName == tickerId.Ticker && i[nameof(RepositoryPathName.SourceName)] == tickerId.Source &&
                                             i.EntryPeriod == entryPeriod)
                                 .ToList();
        return matchingInstruments.Count != 1 ? null : matchingInstruments[0];
    }

    private IEnumerable<IPricePeriodSummary> GetResultsImmediately(HistoricalPricePeriodSummaryRequestResponse responseRequest)
    {
        var instrument = FindInstrumentFor(responseRequest.TickerId, responseRequest.EntryPeriod);
        if (instrument == null) return Enumerable.Empty<IPricePeriodSummary>();
        var readerSession = TimeSeriesRepository!.GetReaderSession<IPricePeriodSummary>(instrument, responseRequest.TimeRange);
        if (readerSession == null) return Enumerable.Empty<IPricePeriodSummary>();
        var readerContext = readerSession.GetEntriesBetweenReader(responseRequest.TimeRange, EntryResultSourcing.FromFactoryFuncUnlimited
                                                                , () => new PricePeriodSummary());

        return readerContext.ResultEnumerable;
    }

    private bool MakeTimeSeriesRepoCallReturnExpectResults(HistoricalPricePeriodSummaryStreamRequest streamRequest)
    {
        var instrument = FindInstrumentFor(streamRequest.TickerId, streamRequest.EntryPeriod);
        if (instrument == null) return false;
        var readerSession = TimeSeriesRepository!.GetReaderSession<IPricePeriodSummary>(instrument, streamRequest.TimeRange);
        if (readerSession == null) return false;
        var readerContext = readerSession.GetEntriesBetweenReader
            (streamRequest.TimeRange, EntryResultSourcing.FromFactoryFuncUnlimited
           , streamRequest.ChannelRequest.PublishChannel.GetChannelEventOrNew<PricePeriodSummary>());

        if (streamRequest.ChannelRequest.BatchSize > 1) readerContext.BatchLimit   = streamRequest.ChannelRequest.BatchSize;
        if (streamRequest.ChannelRequest.ResultLimit > 0) readerContext.MaxResults = streamRequest.ChannelRequest.ResultLimit;
        var launchContext = Context.GetEventQueues(MessageQueueType.Worker)
                                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                                   .GetExecutionContextAction<IReaderContext<IPricePeriodSummary>, HistoricalPricePeriodSummaryStreamRequest>(this);

        launchContext.Execute(ProcessResults, readerContext, streamRequest);
        return true;
    }

    private void ProcessResults(IReaderContext<IPricePeriodSummary> readerContext, HistoricalPricePeriodSummaryStreamRequest streamRequest)
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
