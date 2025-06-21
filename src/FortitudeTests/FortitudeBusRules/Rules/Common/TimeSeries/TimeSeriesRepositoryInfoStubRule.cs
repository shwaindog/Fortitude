// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.BusMessaging.Routing.Response;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using static FortitudeBusRules.Rules.Common.TimeSeries.TimeSeriesRepositoryConstants;

#endregion

namespace FortitudeTests.FortitudeBusRules.Rules.Common.TimeSeries;

public class TimeSeriesRepositoryInfoStubRule : Rule
{
    private List<IInstrument> allInstrumentsResult;

    private Func<string, InstrumentType?, DiscreetTimePeriod?, List<InstrumentFileEntryInfo>> fileEntryInfosCallback;
    private Func<string, InstrumentType?, DiscreetTimePeriod?, List<InstrumentFileInfo>>      fileInfosCallback;

    private ISubscription? instrumentFileEntriesInfoRequestListenSubscription;
    private ISubscription? instrumentsFileInfoRequestListenSubscription;
    private ISubscription? instrumentsListPublishRequestListenSubscription;
    private ISubscription? instrumentsListRequestListenSubscription;

    public TimeSeriesRepositoryInfoStubRule
    (Func<string, InstrumentType?, DiscreetTimePeriod?, List<InstrumentFileInfo>> fileInfosCallback
      , Func<string, InstrumentType?, DiscreetTimePeriod?, List<InstrumentFileEntryInfo>>? fileEntryInfosCallback = null,
        List<IInstrument>? allInstrumentsResult = null)
        : base(nameof(TimeSeriesRepositoryInfoStubRule))
    {
        this.fileInfosCallback      = fileInfosCallback;
        this.fileEntryInfosCallback = fileEntryInfosCallback ?? ((_, _, _) => new List<InstrumentFileEntryInfo>());
        this.allInstrumentsResult   = allInstrumentsResult ?? new List<IInstrument>();
    }

    public Func<string, InstrumentType?, DiscreetTimePeriod?, List<InstrumentFileInfo>> FileInfosCallback
    {
        get => fileInfosCallback;
        set => fileInfosCallback = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Func<string, InstrumentType?, DiscreetTimePeriod?, List<InstrumentFileEntryInfo>> FileEntryInfosCallback
    {
        get => fileEntryInfosCallback;
        set => fileEntryInfosCallback = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<IInstrument> AllInstrumentsResult
    {
        get => allInstrumentsResult;
        set => allInstrumentsResult = value ?? throw new ArgumentNullException(nameof(value));
    }

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

    private List<InstrumentFileEntryInfo> GetFileEntryInfo(TimeSeriesRepositoryInstrumentFileEntryInfoRequest req) =>
        fileEntryInfosCallback(req.InstrumentName, req.InstrumentType, req.CoveringPeriod);


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

    private List<InstrumentFileInfo> GetFileInfo(TimeSeriesRepositoryInstrumentFileInfoRequest req) =>
        fileInfosCallback(req.InstrumentName, req.InstrumentType, req.CoveringPeriod);

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

    private List<IInstrument> HandleRequestResponseListAllAvailableInstruments(IBusMessage<string> noParamsMsg) => allInstrumentsResult;

    private List<IInstrument> GetAllRepositoryAvailableInstruments() => allInstrumentsResult;
}
