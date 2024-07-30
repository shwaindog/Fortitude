// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes;

public class TickInstant : ReusableObject<ITickInstant>, IMutableTickInstant, ITimeSeriesEntry<TickInstant>
  , ICloneable<TickInstant>, IDoublyLinkedListNode<TickInstant>
{
    protected static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TickInstant));

    public TickInstant() { }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public TickInstant
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null,
        bool isReplay = false, FeedSyncStatus syncStatus = FeedSyncStatus.Good, decimal singlePrice = 0m, DateTime? clientReceivedTime = null)
    {
        SourceTickerInfo = sourceTickerInfo is SourceTickerInfo
            ? sourceTickerInfo
            : new SourceTickerInfo(sourceTickerInfo);
        SourceTime      = sourceTime ?? DateTimeConstants.UnixEpoch;
        IsReplay        = isReplay;
        FeedSyncStatus  = syncStatus;
        SingleTickValue = singlePrice;

        ClientReceivedTime = clientReceivedTime ?? DateTimeConstants.UnixEpoch;
    }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public TickInstant(ITickInstant toClone)
    {
        SourceTickerInfo = toClone.SourceTickerInfo is SourceTickerInfo
            ? toClone.SourceTickerInfo
            : new SourceTickerInfo(toClone.SourceTickerInfo!);
        SourceTime      = toClone.SourceTime;
        IsReplay        = toClone.IsReplay;
        FeedSyncStatus  = toClone.FeedSyncStatus;
        SingleTickValue = toClone.SingleTickValue;

        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    public override TickInstant Clone() => Recycler?.Borrow<TickInstant>().CopyFrom(this) as TickInstant ?? new TickInstant(this);

    public TickInstant? Previous
    {
        get => ((IDoublyLinkedListNode<ITickInstant>)this).Previous as TickInstant;
        set => ((IDoublyLinkedListNode<ITickInstant>)this).Previous = value;
    }
    public TickInstant? Next
    {
        get => ((IDoublyLinkedListNode<ITickInstant>)this).Next as TickInstant;
        set => ((IDoublyLinkedListNode<ITickInstant>)this).Next = value;
    }

    public virtual TickerDetailLevel TickerDetailLevel => TickerDetailLevel.SingleValue;

    public ISourceTickerInfo? SourceTickerInfo { get; set; }

    ISourceTickerInfo? ITickInstant.SourceTickerInfo => SourceTickerInfo;

    public FeedSyncStatus FeedSyncStatus { get; set; }

    public bool IsReplay { get; set; }

    public virtual DateTime SourceTime      { get; set; }
    public virtual decimal  SingleTickValue { get; set; }

    public DateTime ClientReceivedTime { get; set; }

    DateTime ITimeSeriesEntry<ITickInstant>.StorageTime(IStorageTimeResolver<ITickInstant>? resolver) => StorageTime(resolver);

    public override ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ClientReceivedTime = source.ClientReceivedTime;

        if (SourceTickerInfo == null)
            SourceTickerInfo = source.SourceTickerInfo is SourceTickerInfo
                ? source.SourceTickerInfo
                : new SourceTickerInfo(source.SourceTickerInfo!);
        else
            SourceTickerInfo.CopyFrom(source.SourceTickerInfo!, copyMergeFlags);

        SourceTime      = source.SourceTime;
        IsReplay        = source.IsReplay;
        FeedSyncStatus  = source.FeedSyncStatus;
        SingleTickValue = source.SingleTickValue;
        return this;
    }

    ITickInstant ICloneable<ITickInstant>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();

    ITickInstant? IDoublyLinkedListNode<ITickInstant>.Previous { get; set; }
    ITickInstant? IDoublyLinkedListNode<ITickInstant>.Next     { get; set; }

    public virtual bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var srcTickersAreEquivalent =
            SourceTickerInfo?.AreEquivalent(other.SourceTickerInfo, exactTypes)
         ?? other.SourceTickerInfo == null;

        var sourceTimesSame        = SourceTime.Equals(other.SourceTime);
        var replayIsSame           = IsReplay == other.IsReplay;
        var staleIsSame            = FeedSyncStatus == other.FeedSyncStatus;
        var singlePriceSame        = SingleTickValue == other.SingleTickValue;
        var clientReceivedTimeSame = ClientReceivedTime.Equals(other.ClientReceivedTime);

        var allEquivalent = srcTickersAreEquivalent && sourceTimesSame && replayIsSame && staleIsSame && singlePriceSame && clientReceivedTimeSame;
        if (!allEquivalent) Debugger.Break();
        return allEquivalent;
    }

    public DateTime StorageTime(IStorageTimeResolver<TickInstant>? resolver = null)
    {
        resolver ??= QuoteStorageTimeResolver.Instance;
        return resolver.ResolveStorageTime(this);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickInstant, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SourceTickerInfo != null ? SourceTickerInfo.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ SourceTime.GetHashCode();
            hashCode = (hashCode * 397) ^ IsReplay.GetHashCode();
            hashCode = (hashCode * 397) ^ FeedSyncStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ SingleTickValue.GetHashCode();
            hashCode = (hashCode * 397) ^ ClientReceivedTime.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(TickInstant)} {{{nameof(SourceTickerInfo)}: {SourceTickerInfo}, {nameof(SourceTime)}: {SourceTime:O}, " +
        $"{nameof(IsReplay)}: {IsReplay}, {nameof(FeedSyncStatus)}: {FeedSyncStatus}, {nameof(SingleTickValue)}: " +
        $"{SingleTickValue:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O} }}";
}
