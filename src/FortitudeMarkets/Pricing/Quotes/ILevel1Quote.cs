// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public interface ILevel1Quote : ITickInstant, IBidAskInstant, ICloneable<ILevel1Quote>, IDoublyLinkedListNode<ILevel1Quote>
{
    DateTime AdapterReceivedTime { get; }
    DateTime AdapterSentTime     { get; }
    DateTime SourceBidTime       { get; }

    BidAskPair BidAskTop            { get; }
    decimal    BidPriceTop          { get; }
    bool       IsBidPriceTopUpdated { get; }
    DateTime   SourceAskTime        { get; }
    decimal    AskPriceTop          { get; }
    bool       IsAskPriceTopUpdated { get; }
    bool       Executable           { get; }
    DateTime   ValidFrom            { get; }
    DateTime   ValidTo              { get; }

    new ILevel1Quote? Next     { get; set; }
    new ILevel1Quote? Previous { get; set; }

    IPricePeriodSummary? SummaryPeriod { get; }

    new bool AreEquivalent(ITickInstant? other, bool exactTypes = false);

    new ILevel1Quote Clone();
}

public interface IMutableLevel1Quote : ILevel1Quote, IMutableTickInstant
{
    new DateTime AdapterSentTime      { get; set; }
    new DateTime AdapterReceivedTime  { get; set; }
    new DateTime SourceBidTime        { get; set; }
    new decimal  BidPriceTop          { get; set; }
    new bool     IsBidPriceTopUpdated { get; set; }
    new DateTime SourceAskTime        { get; set; }
    new decimal  AskPriceTop          { get; set; }
    new bool     IsAskPriceTopUpdated { get; set; }
    new bool     Executable           { get; set; }
    new DateTime ValidFrom            { get; set; }
    new DateTime ValidTo              { get; set; }

    new IMutablePricePeriodSummary? SummaryPeriod { get; set; }

    new IMutableLevel1Quote Clone();
}

// last level of QuoteStructs as Level2+ has variable length data.
public struct Level1QuoteStruct : ITimeSeriesEntry
{
    public Level1QuoteStruct
    (DateTime sourceTime, decimal singleTickValue, DateTime clientReceivedTime,
        decimal bidPriceTop, decimal askPriceTop, bool executable, bool isReplay = false,
        DateTime? adapterReceiveTime = null, DateTime? adapterSentTime = null, DateTime? sourceBidTime = null,
        DateTime? sourceAskTime = null)
    {
        IsReplay            = isReplay;
        SourceTime          = sourceTime;
        ClientReceivedTime  = clientReceivedTime;
        SingleTickValue     = singleTickValue;
        BidPriceTop         = bidPriceTop;
        AskPriceTop         = askPriceTop;
        Executable          = executable;
        AdapterReceivedTime = adapterReceiveTime ?? DateTime.UnixEpoch;
        AdapterSentTime     = adapterSentTime ?? DateTime.UnixEpoch;
        SourceBidTime       = sourceBidTime ?? DateTime.UnixEpoch;
        SourceAskTime       = sourceAskTime ?? DateTime.UnixEpoch;
    }

    public DateTime SourceTime;
    public decimal  SingleTickValue;
    public bool     IsReplay;
    public DateTime ClientReceivedTime;

    public DateTime AdapterReceivedTime;
    public DateTime AdapterSentTime;
    public DateTime SourceBidTime;
    public DateTime SourceAskTime;
    public decimal  BidPriceTop;
    public decimal  AskPriceTop;
    public bool     Executable;
    public DateTime StorageTime(IStorageTimeResolver? resolver = null) => SourceTime;

    public override string ToString() =>
        $"{nameof(Level1QuoteStruct)}({nameof(SourceTime)}: {SourceTime}, {nameof(SingleTickValue)}: {SingleTickValue}, " +
        $"{nameof(IsReplay)}: {IsReplay}, {nameof(ClientReceivedTime)}: {ClientReceivedTime}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime}, {nameof(AdapterSentTime)}: {AdapterSentTime}, " +
        $"{nameof(SourceBidTime)}: {SourceBidTime}, {nameof(SourceAskTime)}: {SourceAskTime}, " +
        $"{nameof(BidPriceTop)}: {BidPriceTop}, {nameof(AskPriceTop)}: {AskPriceTop}, {nameof(Executable)}: {Executable})";
}
