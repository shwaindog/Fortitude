// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Indicators.Persistence;

public struct CandlePersisterParams
{
    public CandlePersisterParams
    (TimeSeriesRepositoryParams repositoryParams, ISourceTickerInfo tickerId, InstrumentType instrumentType, DiscreetTimePeriod coveringPeriod
      , string appendListenAddress, TimeSpan? autoCloseAfterTimeSpan = null)
    {
        RepositoryParams       = repositoryParams;
        TickerId               = tickerId;
        InstrumentType         = instrumentType;
        CoveringPeriod         = coveringPeriod;
        AppendListenAddress    = appendListenAddress;
        AutoCloseAfterTimeSpan = autoCloseAfterTimeSpan ?? TimeSpan.FromSeconds(5);
    }

    public ISourceTickerInfo  TickerId       { get; }
    public DiscreetTimePeriod CoveringPeriod { get; }
    public InstrumentType     InstrumentType { get; }

    public TimeSeriesRepositoryParams RepositoryParams { get; }

    public string AppendListenAddress { get; }

    public TimeSpan AutoCloseAfterTimeSpan { get; }
}

public class CandleFilePersisterRule<TEntry> : TimeSeriesRepositoryAccessRule where TEntry : ITimeSeriesEntry
{
    private readonly TimeSpan           autoCloseTimeSpan;
    private readonly DiscreetTimePeriod coveringPeriod;
    private readonly InstrumentType     instrumentType;

    private readonly CandlePersisterParams persisterParams;

    private readonly ISourceTickerInfo tickerInfo;

    private ISubscription? appendEntrySubscription;

    private bool autoCloseAfterWrite;

    private InstrumentFileInfo instrumentFileInfo;

    private DateTime? lastAppendTime;

    private IWriterSession<TEntry>? writerSession;

    public CandleFilePersisterRule(CandlePersisterParams persisterParams)
        : base(persisterParams.RepositoryParams
             , $"{nameof(CandleFilePersisterRule<TEntry>)}_{persisterParams.TickerId.SourceInstrumentShortName()}_{persisterParams.CoveringPeriod.ShortName()}")
    {
        this.persisterParams = persisterParams;
        tickerInfo           = persisterParams.TickerId;
        coveringPeriod       = persisterParams.CoveringPeriod;
        instrumentType       = persisterParams.InstrumentType;
        autoCloseTimeSpan    = persisterParams.AutoCloseAfterTimeSpan;
    }

    public override async ValueTask StartAsync()
    {
        await base.StartAsync();

        var existingInstruments = InstrumentFileInfos
            (tickerInfo.InstrumentName, tickerInfo.SourceName, instrumentType, coveringPeriod).ToList();
        if (existingInstruments.Any())
        {
            if (existingInstruments.Count == 1)
                instrumentFileInfo = existingInstruments[0];
            else
                throw new
                    Exception($"More than one instrument exists for {tickerInfo.InstrumentName}, {tickerInfo.SourceName}, {instrumentType}, {coveringPeriod}");
        }
        else
        {
            var candleTickerInstrument = new SourceTickerInfo(tickerInfo)
            {
                CoveringPeriod = coveringPeriod
              , InstrumentType = instrumentType
            };
            var fileInfo = TimeSeriesRepository.GetInstrumentFileInfo(candleTickerInstrument);

            if (fileInfo.FilePeriod > TimeBoundaryPeriod.Tick)
                instrumentFileInfo = new InstrumentFileInfo(candleTickerInstrument, fileInfo.FilePeriod);
        }
        if (Equals(instrumentFileInfo, default(InstrumentFileInfo)))
            throw new
                Exception($"Could not locate a repository structure for {tickerInfo.InstrumentName}, {tickerInfo.SourceName}, {instrumentType}, {coveringPeriod}");


        appendEntrySubscription = await this.RegisterListenerAsync<TEntry>(persisterParams.AppendListenAddress, ReceiveEntryToPersist);
    }

    public override async ValueTask StopAsync()
    {
        if (writerSession?.IsOpen == true) writerSession.Close();
        await appendEntrySubscription.NullSafeUnsubscribe();
        await base.StopAsync();
    }

    private void ReceiveEntryToPersist(IBusMessage<TEntry> appendEntryMsg)
    {
        if (writerSession == null) writerSession = TimeSeriesRepository.GetWriterSession<TEntry>(instrumentFileInfo.Instrument);
        if (!writerSession!.IsOpen) writerSession.Reopen();
        var entry = appendEntryMsg.Payload.Body();
        writerSession.AppendEntry(entry);

        var now = DateTime.UtcNow;

        if (!autoCloseAfterWrite && lastAppendTime != null && now - lastAppendTime > autoCloseTimeSpan) autoCloseAfterWrite = true;
        lastAppendTime = now;
        if (autoCloseAfterWrite) writerSession.Close();
    }
}
