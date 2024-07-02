// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Indicators.Persistence;

public struct SummarizingPricePersisterParams
{
    public SummarizingPricePersisterParams
    (TimeSeriesRepositoryParams repositoryParams, ISourceTickerIdentifier tickerId, InstrumentType instrumentType, TimeSeriesPeriod entryPeriod
      , string appendListenAddress, TimeSpan? autoCloseAfterTimeSpan = null)
    {
        RepositoryParams       = repositoryParams;
        TickerId               = tickerId;
        InstrumentType         = instrumentType;
        EntryPeriod            = entryPeriod;
        AppendListenAddress    = appendListenAddress;
        AutoCloseAfterTimeSpan = autoCloseAfterTimeSpan ?? TimeSpan.FromSeconds(5);
    }

    public ISourceTickerIdentifier    TickerId            { get; }
    public TimeSeriesPeriod           EntryPeriod         { get; }
    public InstrumentType             InstrumentType      { get; }
    public TimeSeriesRepositoryParams RepositoryParams    { get; }
    public string                     AppendListenAddress { get; }

    public TimeSpan AutoCloseAfterTimeSpan { get; }
}

public class PriceSummarizingFilePersisterRule<TEntry> : TimeSeriesRepositoryAccessRule where TEntry : ITimeSeriesEntry<TEntry>
{
    private readonly TimeSpan                        autoCloseTimeSpan;
    private readonly TimeSeriesPeriod                entryPeriod;
    private readonly InstrumentType                  instrumentType;
    private readonly SummarizingPricePersisterParams persisterParams;
    private readonly ISourceTickerIdentifier         tickerId;

    private ISubscription? appendEntrySubscription;

    private bool autoCloseAfterWrite;

    private InstrumentFileInfo instrumentFileInfo;
    private DateTime?          lastAppendTime;

    private IWriterSession<TEntry>? writerSession;

    public PriceSummarizingFilePersisterRule(SummarizingPricePersisterParams persisterParams)
        : base(persisterParams.RepositoryParams
             , $"{nameof(PriceSummarizingFilePersisterRule<TEntry>)}_{persisterParams.TickerId.ShortName()}_{persisterParams.EntryPeriod}")
    {
        this.persisterParams = persisterParams;
        tickerId             = persisterParams.TickerId;
        entryPeriod          = persisterParams.EntryPeriod;
        instrumentType       = persisterParams.InstrumentType;
        autoCloseTimeSpan    = persisterParams.AutoCloseAfterTimeSpan;
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();

        instrumentFileInfo = InstrumentFileInfos
            (tickerId.Ticker, instrumentType, entryPeriod
           , new KeyValuePair<string, string>(nameof(RepositoryPathName.SourceName), tickerId.Source)).First();

        appendEntrySubscription = await this.RegisterListenerAsync<TEntry>(persisterParams.AppendListenAddress, ReceiveEntryToPersist);
    }

    public override async ValueTask StopAsync()
    {
        if (appendEntrySubscription != null) await appendEntrySubscription.UnsubscribeAsync();
        await base.StopAsync();
    }

    private void ReceiveEntryToPersist(IBusMessage<TEntry> appendEntryMsg)
    {
        if (writerSession == null) writerSession = TimeSeriesRepository.GetWriterSession<TEntry>(instrumentFileInfo.Instrument);
        if (!writerSession!.IsOpen) writerSession.Reopen();
        var entry = appendEntryMsg.Payload.Body();
        writerSession.AppendEntry(entry);
        var now                                                                                                             = DateTime.UtcNow;
        if (!autoCloseAfterWrite && lastAppendTime != null && now - lastAppendTime > autoCloseTimeSpan) autoCloseAfterWrite = true;
        lastAppendTime = now;
        if (autoCloseAfterWrite) writerSession.Close();
    }
}
