// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.Config;
using static FortitudeBusRules.Rules.Common.TimeSeries.TimeSeriesRepositoryConstants;

#endregion

namespace FortitudeBusRules.Rules.Common.TimeSeries;

public struct TimeSeriesRepositoryInstrumentFileInfoRequest
{
    public TimeSeriesRepositoryInstrumentFileInfoRequest
    (string instrumentName, InstrumentType? instrumentType = null, TimeSeriesPeriod? entryPeriod = null
      , Dictionary<string, string>? matchFields = null)
    {
        InstrumentName = instrumentName;
        EntryPeriod    = entryPeriod;
        InstrumentType = instrumentType;
        MatchFields    = matchFields;
    }

    public string            InstrumentName { get; }
    public TimeSeriesPeriod? EntryPeriod    { get; }
    public InstrumentType?   InstrumentType { get; }

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
            = await this.RegisterRequestListenerAsync<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileEntryInfo>>
                (TimeSeriesInstrumentEntryFileInfoRequestResponse, HandleInstrumentFileEntryInfoRequest);
    }

    public override async ValueTask StopAsync()
    {
        if (instrumentsListPublishRequestListenSubscription != null) await instrumentsListPublishRequestListenSubscription.UnsubscribeAsync();
        if (instrumentsListRequestListenSubscription != null) await instrumentsListRequestListenSubscription.UnsubscribeAsync();
        if (instrumentsFileInfoRequestListenSubscription != null) await instrumentsFileInfoRequestListenSubscription.UnsubscribeAsync();
        if (instrumentFileEntriesInfoRequestListenSubscription != null) await instrumentFileEntriesInfoRequestListenSubscription.UnsubscribeAsync();

        await base.StopAsync();
    }

    private async ValueTask<List<InstrumentFileEntryInfo>> HandleInstrumentFileEntryInfoRequest
        (IBusRespondingMessage<TimeSeriesRepositoryInstrumentFileInfoRequest, List<InstrumentFileEntryInfo>> instrumentRequestMsg)
    {
        var req = instrumentRequestMsg.Payload.Body();

        var launchContext =
            Context.GetEventQueues(MessageQueueType.Worker)
                   .SelectEventQueue(QueueSelectionStrategy.EarliestStarted)
                   .GetExecutionContextResult<List<InstrumentFileEntryInfo>, TimeSeriesRepositoryInstrumentFileInfoRequest>(this);

        var result = await launchContext.Execute(GetFileEntryInfo, req);

        return result;
    }

    private List<InstrumentFileEntryInfo> GetFileEntryInfo
        (TimeSeriesRepositoryInstrumentFileInfoRequest req) =>
        InstrumentFileEntryInfos(req.InstrumentName, req.InstrumentType, req.EntryPeriod);


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
        InstrumentFileInfos(req.InstrumentName, req.InstrumentType, req.EntryPeriod);

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
