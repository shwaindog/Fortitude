// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.TimeSeries;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes;

public class TickInstant : ReusableObject<ITickInstant>, IMutableTickInstant, ICloneable<TickInstant>
{
    protected static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TickInstant));

    public TickInstant() { }

    public TickInstant
        (decimal singlePrice = 0m, bool isReplay = false, DateTime? sourceTime = null)
    {
        SourceTime      = sourceTime ?? DateTime.MinValue;
        SingleTickValue = singlePrice;
        IsReplay        = isReplay;
    }

    public TickInstant(ITickInstant toClone)
    {
        SourceTime      = toClone.SourceTime;
        SingleTickValue = toClone.SingleTickValue;
        IsReplay        = toClone.IsReplay;
    }

    public override TickInstant Clone() => Recycler?.Borrow<TickInstant>().CopyFrom(this) ?? new TickInstant(this);

    public virtual decimal  SingleTickValue { get; set; }
    public         bool     IsReplay        { get; set; }
    public virtual DateTime SourceTime      { get; set; }
    

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ITickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
        return QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        SourceTime += toChangeBy;
    }

    public override TickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SingleTickValue = source.SingleTickValue;
        IsReplay        = source.IsReplay;
        SourceTime      = source.SourceTime;
        return this;
    }

    ITickInstant ICloneable<ITickInstant>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();


    public virtual bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var singlePriceSame  = SingleTickValue == other.SingleTickValue;
        var isReplaySame     = IsReplay == other.IsReplay;
        var isSourceTimeSame = SourceTime == other.SourceTime;
        return singlePriceSame && isReplaySame && isSourceTimeSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickInstant, true);

    public override int GetHashCode()
    {
        var hashCode = SingleTickValue.GetHashCode();
        return hashCode;
    }

    public virtual string QuoteToStringMembers =>
        $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(SingleTickValue)}: {SingleTickValue:N5}, " +
        $"{nameof(IsReplay)}: {IsReplay}";

    public override string ToString() => $"{nameof(TickInstant)} {{{QuoteToStringMembers}}}";
}

public class PublishableTickInstant : ReusableObject<IPublishableTickInstant>, IMutablePublishableTickInstant, ICloneable<PublishableTickInstant>
  , IDoublyLinkedListNode<PublishableTickInstant>
{
    protected static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PublishableTickInstant));

    protected IMutableTickInstant QuoteContainer;

    public PublishableTickInstant()
    {
        QuoteContainer = CreateEmptyQuoteContainerInstant();
    }

    public PublishableTickInstant
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null,
        bool isReplay = false, FeedSyncStatus syncStatus = FeedSyncStatus.Good, decimal singlePrice = 0m, DateTime? clientReceivedTime = null)
        : this(new TickInstant(singlePrice, isReplay, sourceTime), sourceTickerInfo, syncStatus, clientReceivedTime) { }


    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    protected PublishableTickInstant
    (IMutableTickInstant? initialisedQuoteContainer, ISourceTickerInfo sourceTickerInfo,
        FeedSyncStatus syncStatus = FeedSyncStatus.Good, DateTime? clientReceivedTime = null)
    {
        QuoteContainer = initialisedQuoteContainer ?? CreateQuoteContainerFromTickerInfo(sourceTickerInfo);

        SourceTickerInfo = sourceTickerInfo is SourceTickerInfo
            ? sourceTickerInfo
            : new SourceTickerInfo(sourceTickerInfo);
        FeedSyncStatus = syncStatus;

        ClientReceivedTime = clientReceivedTime ?? DateTime.MinValue;
    }

    public PublishableTickInstant(IPublishableTickInstant toClone) : this(toClone, null) { }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    protected PublishableTickInstant(IPublishableTickInstant toClone, IMutableTickInstant? initialisedQuoteContainer)
    {
        if (toClone is PublishableTickInstant pubTickInstant)
        {
            QuoteContainer = initialisedQuoteContainer ?? pubTickInstant.QuoteContainer.Clone();
        }
        else
        {
            QuoteContainer = initialisedQuoteContainer ?? CreateCloneQuoteContainerInstant(toClone);
        }

        SourceTickerInfo = toClone.SourceTickerInfo is SourceTickerInfo
            ? toClone.SourceTickerInfo
            : new SourceTickerInfo(toClone.SourceTickerInfo!);
        FeedSyncStatus = toClone.FeedSyncStatus;

        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    protected virtual IMutableTickInstant CreateEmptyQuoteContainerInstant() => new TickInstant();

    protected virtual IMutableTickInstant CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new TickInstant(tickInstant);

    protected virtual IMutableTickInstant CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new TickInstant();

    public override PublishableTickInstant Clone() =>
        Recycler?.Borrow<PublishableTickInstant>().CopyFrom(this) ?? new PublishableTickInstant(this, QuoteContainer.Clone());


    [JsonIgnore]
    public PublishableTickInstant? Previous
    {
        get => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Previous as PublishableTickInstant;
        set => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public PublishableTickInstant? Next
    {
        get => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Next as PublishableTickInstant;
        set => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Next = value;
    }

    [JsonIgnore] public virtual TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.SingleValue;

    [JsonIgnore] ISourceTickerInfo? IPublishableTickInstant.SourceTickerInfo => SourceTickerInfo;

    public ISourceTickerInfo? SourceTickerInfo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FeedSyncStatus FeedSyncStatus { get; set; }

    public DateTime ClientReceivedTime { get; set; }

    ITickInstant IPublishableTickInstant.AsNonPublishable => AsNonPublishable;
    public virtual IMutableTickInstant   AsNonPublishable => QuoteContainer;

    public DateTime SourceTime
    {
        get => QuoteContainer.SourceTime;
        set => QuoteContainer.SourceTime = value;
    }

    public virtual decimal SingleTickValue
    {
        get => QuoteContainer.SingleTickValue;
        set => QuoteContainer.SingleTickValue = value;
    }

    public bool IsReplay
    {
        get => QuoteContainer.IsReplay;
        set => QuoteContainer.IsReplay = value;
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        QuoteContainer.IncrementTimeBy(toChangeBy);

        ClientReceivedTime += toChangeBy;
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<IPublishableTickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
        return QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);
    }

    IReusableObject<ITickInstant> ITransferState<IReusableObject<ITickInstant>>.CopyFrom
        (IReusableObject<ITickInstant> source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPublishableTickInstant publishableTickInstant)
        {
            CopyFrom(publishableTickInstant, copyMergeFlags);
        }
        else
        {
            QuoteContainer.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }

    ITickInstant ITransferState<ITickInstant>.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPublishableTickInstant publishableTickInstant)
        {
            CopyFrom(publishableTickInstant, copyMergeFlags);
        }
        else
        {
            QuoteContainer.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }


    public override PublishableTickInstant CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        QuoteContainer.CopyFrom(source, copyMergeFlags);
        ClientReceivedTime = source.ClientReceivedTime;

        if (SourceTickerInfo == null)
            SourceTickerInfo = source.SourceTickerInfo is SourceTickerInfo
                ? source.SourceTickerInfo
                : new SourceTickerInfo(source.SourceTickerInfo!);
        else
            SourceTickerInfo.CopyFrom(source.SourceTickerInfo!, copyMergeFlags);

        FeedSyncStatus = source.FeedSyncStatus;
        return this;
    }


    IPublishableTickInstant ICloneable<IPublishableTickInstant>.Clone() => Clone();

    IMutablePublishableTickInstant IMutablePublishableTickInstant.Clone() => Clone();

    ITickInstant ICloneable<ITickInstant>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();

    IPublishableTickInstant IPublishableTickInstant.Clone() => Clone();

    IPublishableTickInstant? IDoublyLinkedListNode<IPublishableTickInstant>.Previous { get; set; }
    IPublishableTickInstant? IDoublyLinkedListNode<IPublishableTickInstant>.Next     { get; set; }

    bool IInterfacesComparable<ITickInstant>.AreEquivalent(ITickInstant? other, bool exactTypes) =>
        AreEquivalent(other as IPublishableLevel1Quote, exactTypes);

    public virtual bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (other is null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var quoteValuesSame = QuoteContainer.AreEquivalent((ITickInstant?)(other as PublishableTickInstant)?.QuoteContainer ?? other, exactTypes);
        var srcTickersAreEquivalent =
            SourceTickerInfo?.AreEquivalent(other.SourceTickerInfo, exactTypes)
         ?? other.SourceTickerInfo == null;

        var staleIsSame            = FeedSyncStatus == other.FeedSyncStatus;
        var clientReceivedTimeSame = ClientReceivedTime.Equals(other.ClientReceivedTime);

        var allEquivalent = srcTickersAreEquivalent && staleIsSame && quoteValuesSame &&
                            clientReceivedTimeSame;
        if (!allEquivalent) Debugger.Break();
        return allEquivalent;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPublishableTickInstant, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SourceTickerInfo != null ? SourceTickerInfo.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ QuoteContainer.GetHashCode();
            hashCode = (hashCode * 397) ^ FeedSyncStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ SingleTickValue.GetHashCode();
            hashCode = (hashCode * 397) ^ ClientReceivedTime.GetHashCode();
            return hashCode;
        }
    }

    public virtual string QuoteToStringMembers =>
        $"{nameof(SourceTickerInfo)}: {SourceTickerInfo}, {nameof(FeedSyncStatus)}: {FeedSyncStatus}, " +
        $"{nameof(ClientReceivedTime)}: {ClientReceivedTime:O}";

    public override string ToString() => $"{nameof(PublishableTickInstant)} {{{QuoteToStringMembers}, {QuoteContainer.QuoteToStringMembers}}}";
}
