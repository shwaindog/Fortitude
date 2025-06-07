// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes;

public interface ITickInstant : IReusableObject<ITickInstant>, IInterfacesComparable<ITickInstant>, ITimeSeriesEntry
{
    DateTime SourceTime      { get; }
    decimal  SingleTickValue { get; }

    public string QuoteToStringMembers { get; }
}

public interface IPublishableTickInstant : IReusableObject<IPublishableTickInstant>, ITickInstant,
    IInterfacesComparable<IPublishableTickInstant>, IDoublyLinkedListNode<IPublishableTickInstant>, IFeedEventStatusUpdate
{
    new DateTime SourceTime { get; }

    [JsonIgnore] TickerQuoteDetailLevel TickerQuoteDetailLevel { get; }

    public ITickInstant AsNonPublishable { get; }

    [JsonIgnore] ISourceTickerInfo? SourceTickerInfo { get; }

    new IPublishableTickInstant CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
    new ITransferState          CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    new IPublishableTickInstant Clone();
}

public struct TickInstantValue : ITimeSeriesEntry
{
    public TickInstantValue(DateTime sourceTime, decimal singleTickValue, DateTime clientReceivedTime, bool isSourceReplay = false)
    {
        IsSourceReplay     = isSourceReplay;
        SourceTime         = sourceTime;
        ClientReceivedTime = clientReceivedTime;
        SingleTickValue    = singleTickValue;
    }

    public bool     IsSourceReplay;
    public DateTime SourceTime;
    public DateTime ClientReceivedTime;
    public decimal  SingleTickValue;
    public DateTime StorageTime(IStorageTimeResolver? resolver = null) => SourceTime;
}

public interface IMutableTickInstant : ITickInstant, ITrackableReset<IMutableTickInstant>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new DateTime SourceTime { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal SingleTickValue { get; set; }

    void IncrementTimeBy(TimeSpan toChangeBy);

    new IMutableTickInstant Clone();
}

public interface IMutablePublishableTickInstant : IPublishableTickInstant, IMutableTickInstant, IMutableFeedEventStatusUpdate
  , ITrackableReset<IMutablePublishableTickInstant>
{
    new DateTime           SourceTime       { get; set; }
    new ISourceTickerInfo? SourceTickerInfo { get; set; }

    new IMutableTickInstant AsNonPublishable { get; }

    new IMutablePublishableTickInstant Clone();
    new IMutablePublishableTickInstant ResetWithTracking();
}
