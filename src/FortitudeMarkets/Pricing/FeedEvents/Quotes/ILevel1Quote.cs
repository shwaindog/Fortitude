// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes;

public interface ILevel1Quote : ITickInstant, IBidAskInstant, ICloneable<ILevel1Quote>
{
    [JsonInclude] BidAskPair BidAskTop { get; }

    DateTime SourceBidTime        { get; }
    decimal  BidPriceTop          { get; }
    bool     IsBidPriceTopChanged { get; }

    DateTime SourceAskTime        { get; }
    decimal  AskPriceTop          { get; }
    bool     IsAskPriceTopChanged { get; }

    bool     Executable { get; }
    DateTime ValidFrom  { get; }
    DateTime ValidTo    { get; }


    new ILevel1Quote Clone();
}

public interface IPublishableLevel1Quote : IPublishableTickInstant, ILevel1Quote, ICloneable<IPublishableLevel1Quote>
  , IDoublyLinkedListNode<IPublishableLevel1Quote>
{
    new IPublishableLevel1Quote? Next     { get; set; }
    new IPublishableLevel1Quote? Previous { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    ICandle? ConflatedTicksCandle { get; }

    
    new bool         AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);

    new ILevel1Quote AsNonPublishable { get; }

    new IPublishableLevel1Quote Clone();
}

public interface IMutableLevel1Quote : ILevel1Quote, IMutableTickInstant, ITrackableReset<IMutableLevel1Quote>
{
    new DateTime SourceBidTime        { get; set; }
    new decimal  BidPriceTop          { get; set; }
    new bool     IsBidPriceTopChanged { get; set; }
    new DateTime SourceAskTime        { get; set; }
    new decimal  AskPriceTop          { get; set; }
    new bool     IsAskPriceTopChanged { get; set; }
    new bool     Executable           { get; set; }
    new DateTime ValidFrom            { get; set; }
    new DateTime ValidTo              { get; set; }

    new bool AreEquivalent(ITickInstant? other, bool exactTypes = false);

    new IMutableLevel1Quote ResetWithTracking();

    new IMutableLevel1Quote Clone();
}

public interface IMutablePublishableLevel1Quote : IPublishableLevel1Quote, 
    IMutableLevel1Quote, IMutablePublishableTickInstant, ITrackableReset<IMutablePublishableLevel1Quote>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    new IMutableCandle? ConflatedTicksCandle { get; set; }

    new bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);

    new IMutableLevel1Quote            AsNonPublishable { get; }
    new IMutablePublishableLevel1Quote ResetWithTracking();

    new IMutablePublishableLevel1Quote Clone();
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
