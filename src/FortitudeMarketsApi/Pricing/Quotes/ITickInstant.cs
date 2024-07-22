// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ITickInstant : IReusableObject<ITickInstant>, IInterfacesComparable<ITickInstant>
  , ITimeSeriesEntry<ITickInstant>, IDoublyLinkedListNode<ITickInstant>
{
    TickerDetailLevel TickerDetailLevel { get; }

    bool           IsReplay           { get; }
    DateTime       SourceTime         { get; }
    DateTime       ClientReceivedTime { get; }
    FeedSyncStatus FeedSyncStatus     { get; }

    ISourceTickerInfo? SourceTickerInfo { get; }

    decimal SingleTickValue { get; }

    new ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public struct TickInstantValue : ITimeSeriesEntry<TickInstantValue>
{
    public TickInstantValue(DateTime sourceTime, decimal singleTickValue, DateTime clientReceivedTime, bool isReplay = false)
    {
        IsReplay           = isReplay;
        SourceTime         = sourceTime;
        ClientReceivedTime = clientReceivedTime;
        SingleTickValue    = singleTickValue;
    }

    public bool     IsReplay;
    public DateTime SourceTime;
    public DateTime ClientReceivedTime;
    public decimal  SingleTickValue;
    public DateTime StorageTime(IStorageTimeResolver<TickInstantValue>? resolver = null) => SourceTime;
}

public interface IMutableTickInstant : ITickInstant
{
    new bool     IsReplay           { get; set; }
    new DateTime SourceTime         { get; set; }
    new DateTime ClientReceivedTime { get; set; }

    new ISourceTickerInfo? SourceTickerInfo { get; set; }

    new decimal SingleTickValue { get; set; }

    new IMutableTickInstant Clone();
}
