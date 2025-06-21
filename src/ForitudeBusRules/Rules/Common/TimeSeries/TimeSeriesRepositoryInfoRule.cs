// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeCommon.Chronometry;
using FortitudeIO.Storage.TimeSeries.FileSystem.Config;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using static FortitudeBusRules.Rules.Common.TimeSeries.TimeSeriesRepositoryConstants;

#endregion

namespace FortitudeBusRules.Rules.Common.TimeSeries;

public struct TimeSeriesRepositoryInstrumentFileInfoRequest
{
    public TimeSeriesRepositoryInstrumentFileInfoRequest
    (string instrumentName, string instrumentSource, InstrumentType? instrumentType = null, DiscreetTimePeriod? coveringPeriod = null
      , Dictionary<string, string>? matchFields = null)
    {
        InstrumentName   = instrumentName;
        InstrumentSource = instrumentSource;
        CoveringPeriod   = coveringPeriod;
        InstrumentType   = instrumentType;
        MatchFields      = matchFields;
    }

    public string              InstrumentName   { get; }
    public string              InstrumentSource { get; }
    public DiscreetTimePeriod? CoveringPeriod   { get; }
    public InstrumentType?     InstrumentType   { get; }

    public Dictionary<string, string>? MatchFields { get; }
}

public struct TimeSeriesRepositoryInstrumentFileEntryInfoRequest
{
    public TimeSeriesRepositoryInstrumentFileEntryInfoRequest
    (string instrumentName, string instrumentSource, InstrumentType? instrumentType = null, DiscreetTimePeriod? coveringPeriod = null
      , UnboundedTimeRange? limitEntryRangeResults = null, Dictionary<string, string>? matchFields = null)
    {
        InstrumentName    = instrumentName;
        InstrumentSource  = instrumentSource;
        CoveringPeriod    = coveringPeriod;
        InstrumentType    = instrumentType;
        LimitResultPeriod = limitEntryRangeResults;
        MatchFields       = matchFields;
    }

    public string              InstrumentName    { get; }
    public string              InstrumentSource  { get; }
    public DiscreetTimePeriod? CoveringPeriod    { get; }
    public InstrumentType?     InstrumentType    { get; }
    public UnboundedTimeRange? LimitResultPeriod { get; }

    public Dictionary<string, string>? MatchFields { get; }
}

public class TimeSeriesRepositoryInfoRule : TimeSeriesRepositoryAccessRule
{
    private ISubscription? instrumentFileEntriesInfoRequestListenSubscription;
    private ISubscription? instrumentsFileInfoRequestListenSubscription;
    private ISubscription? instrumentsListPublishRequestListenSubscription;
    private ISubscription? instrumentsListRequestListenSubscription;

    public TimeSeriesRepositoryInfoRule(IRepositoryBuilder repoBuilder) : base(repoBuilder, nameof(TimeSeriesRepositoryInfoRule)) { }
    public TimeSeriesRepositoryInfoRule(ITimeSeriesRepository existingRepository) : base(existingRepository, nameof(TimeSeriesRepositoryInfoRule)) { }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();

        instrumentsListPublishRequestListenSubscription = await this.RegisterListenerAsync<ResponsePublishParams>
            (TimeSeriesAllAvailableInstrumentsPublishRequest, HandleRequestListAllAvailableInstruments);
        instrumentsListRequestListenSubscription = await this.RegisterRequestListenerAsync<string, List<IInstrument>>
            (TimeSeriesAllAvailableInstrumentsRequestResponse, HandleRequestResponseListAllAvailableInstruments);
        instrumentsFileInfoRequestListenSubscription
            = await this.RegisterRequestListenerAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>>
                (TimeSeriesInstrumentFileInfoRequestResponse, HandleInstrumentFileInfo);
        instrumentFileEntriesInfoRequestListenSubscription
            = await this.RegisterRequestListenerAsync<TimeSeriesRepositoryInstrumentFileEntryInfoRequest, List<InstrumentFileEntryInfo>>
                (TimeSeriesInstrumentEntryFileInfoRequestResponse, HandleInstrumentFileEntryInfoRequest);
    }

    public override async ValueTask StopAsync()
    {
        await instrumentsListPublishRequestListenSubscription.NullSafeUnsubscribe();
        await instrumentsListRequestListenSubscription.NullSafeUnsubscribe();
        await instrumentsFileInfoRequestListenSubscription.NullSafeUnsubscribe();
        await instrumentFileEntriesInfoRequestListenSubscription.NullSafeUnsubscribe();

        await base.StopAsync();
    }

    private async ValueTask<List<InstrumentFileEntryInfo>> HandleInstrumentFileEntryInfoRequest
        (IBusRespondingMessage<TimeSeriesRepositoryInstrumentFileEntryInfoRequest, List<InstrumentFileEntryInfo>> instrumentRequestMsg)
    {
        var req = instrumentRequestMsg.Payload.Body();

        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextResult<List<InstrumentFileEntryInfo>, TimeSeriesRepositoryInstrumentFileEntryInfoRequest>(this);

        var result = await launchContext.Execute(GetFileEntryInfo, req);

        return result;
    }

    private List<InstrumentFileEntryInfo> GetFileEntryInfo
        (TimeSeriesRepositoryInstrumentFileEntryInfoRequest req) =>
        InstrumentFileEntryInfos(req.InstrumentName, req.InstrumentSource, req.InstrumentType, req.CoveringPeriod);


    private async ValueTask<List<InstrumentFileInfo>> HandleInstrumentFileInfo
        (IBusRespondingMessage<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileInfo>> instrumentRequestMsg)
    {
        var req = instrumentRequestMsg.Payload.Body();

        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextResult<List<InstrumentFileInfo>, TimeSeriesRepositoryInstrumentFileInfoRequest>(this);

        var result = await launchContext.Execute(GetFileInfo, req);

        return result;
    }

    private List<InstrumentFileInfo> GetFileInfo
        (TimeSeriesRepositoryInstrumentFileInfoRequest req) =>
        InstrumentFileInfos(req.InstrumentName, req.InstrumentSource, req.InstrumentType, req.CoveringPeriod);

    private void HandleRequestListAllAvailableInstruments(IBusMessage<ResponsePublishParams> publishParamsMsg)
    {
        var responsePublish = publishParamsMsg.Payload.Body();

        if (responsePublish.ResponsePublishMethod == ResponsePublishMethod.ListenerDefaultBroadcastAddress)
        {
            this.Publish(TimeSeriesAllAvailableInstrumentsDefaultResponse, GetAllRepositoryAvailableInstruments()
                       , responsePublish.PublishDispatchOptions);
        }
        else if (responsePublish.ResponsePublishMethod is ResponsePublishMethod.AlternativeBroadcastAddress)
        {
            this.Publish(responsePublish.AlternativePublishAddress!, GetAllRepositoryAvailableInstruments(), responsePublish.PublishDispatchOptions);
        }
        else
        {
            var publishChannel = (IChannel<List<IInstrument>>)responsePublish.ChannelRequest!.Channel;
            publishChannel.Publish(this, GetAllRepositoryAvailableInstruments());
            publishChannel.PublishComplete(this);
        }
    }

    private List<IInstrument> HandleRequestResponseListAllAvailableInstruments(IBusMessage<string> noParamsMsg) =>
        GetAllRepositoryAvailableInstruments();

    private List<IInstrument> GetAllRepositoryAvailableInstruments() => TimeSeriesRepository!.InstrumentFilesMap.Keys.ToList();
}
