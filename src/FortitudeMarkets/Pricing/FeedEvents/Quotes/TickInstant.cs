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
        (decimal singlePrice = 0m, DateTime? sourceTime = null)
    {
        SourceTime      = sourceTime ?? DateTime.MinValue;
        SingleTickValue = singlePrice;
    }

    public TickInstant(ITickInstant toClone)
    {
        SourceTime      = toClone.SourceTime;
        SingleTickValue = toClone.SingleTickValue;
    }

    public virtual decimal  SingleTickValue { get; set; }
    public virtual DateTime SourceTime      { get; set; }

    public virtual IMutableTickInstant ResetWithTracking()
    {
        SingleTickValue = 0m;
        SourceTime      = DateTime.MinValue;
        return this;
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ITickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
        return QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        SourceTime += toChangeBy;
    }

    ITickInstant ICloneable<ITickInstant>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();

    public override TickInstant Clone() => Recycler?.Borrow<TickInstant>().CopyFrom(this) ?? new TickInstant(this);

    public override TickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SingleTickValue = source.SingleTickValue;
        if (!copyMergeFlags.HasExcludeCoreTimeStamp())
        {
            SourceTime = source.SourceTime;
        }
        return this;
    }

    public virtual bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var singlePriceSame  = SingleTickValue == other.SingleTickValue;
        var isSourceTimeSame = SourceTime == other.SourceTime;
        return singlePriceSame && isSourceTimeSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickInstant, true);

    public override int GetHashCode()
    {
        var hashCode = SingleTickValue.GetHashCode();
        return hashCode;
    }

    public virtual string QuoteToStringMembers => $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(SingleTickValue)}: {SingleTickValue:N5}";

    public override string ToString() => $"{nameof(TickInstant)} {{{QuoteToStringMembers}}}";
}

public class PublishableTickInstant : FeedEventStatusUpdate, IMutablePublishableTickInstant, ICloneable<PublishableTickInstant>
  , IDoublyLinkedListNode<PublishableTickInstant>
{
    protected static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PublishableTickInstant));

    protected uint NumOfUpdatesSinceEmpty = uint.MaxValue;

    protected IMutableTickInstant QuoteContainer;

    public PublishableTickInstant()
    {
        QuoteContainer = CreateEmptyQuoteContainerInstant();
    }

    public PublishableTickInstant
    (ISourceTickerInfo sourceTickerInfo, decimal singlePrice = 0m, DateTime? sourceTime = null, FeedSyncStatus syncStatus = FeedSyncStatus.Good,
        FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None)
        : this(new TickInstant(singlePrice, sourceTime), sourceTickerInfo, syncStatus, feedConnectivityStatus)
    {
        if (GetType() == typeof(PublishableTickInstant)) NumOfUpdatesSinceEmpty = 0;
    }


    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    protected PublishableTickInstant
    (IMutableTickInstant? initialisedQuoteContainer, ISourceTickerInfo sourceTickerInfo,
        FeedSyncStatus syncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None)
        : base(syncStatus, feedConnectivityStatus)
    {
        QuoteContainer = initialisedQuoteContainer ?? CreateQuoteContainerFromTickerInfo(sourceTickerInfo);

        SourceTickerInfo = sourceTickerInfo is SourceTickerInfo
            ? sourceTickerInfo
            : new SourceTickerInfo(sourceTickerInfo);

        if (GetType() == typeof(PublishableTickInstant)) NumOfUpdatesSinceEmpty = 0;
    }

    public PublishableTickInstant(IPublishableTickInstant toClone) : this(toClone, null) { }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    protected PublishableTickInstant(IPublishableTickInstant toClone, IMutableTickInstant? initialisedQuoteContainer) : base(toClone)
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

        if (GetType() == typeof(PublishableTickInstant)) NumOfUpdatesSinceEmpty = 0;
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

    ITickInstant IPublishableTickInstant.AsNonPublishable => AsNonPublishable;

    public virtual IMutableTickInstant   AsNonPublishable => QuoteContainer;

    public override DateTime SourceTime
    {
        get => QuoteContainer.SourceTime;
        set => QuoteContainer.SourceTime = value;
    }

    public virtual decimal SingleTickValue
    {
        get => QuoteContainer.SingleTickValue;
        set => QuoteContainer.SingleTickValue = value;
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        QuoteContainer.IncrementTimeBy(toChangeBy);

        AdapterReceivedTime += toChangeBy;
        AdapterSentTime     += toChangeBy;
        ClientReceivedTime  += toChangeBy;
    }

    IMutableTickInstant ITrackableReset<IMutableTickInstant>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableTickInstant ITrackableReset<IMutablePublishableTickInstant>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableTickInstant IMutablePublishableTickInstant.ResetWithTracking() => ResetWithTracking();

    public virtual PublishableTickInstant ResetWithTracking()
    {
        QuoteContainer.ResetWithTracking();
        ClientReceivedTime = DateTime.MinValue;
        FeedSyncStatus     = FeedSyncStatus.Good;
        return this;
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

    ITransferState IPublishableTickInstant.CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
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

    IReusableObject<IPublishableTickInstant> ITransferState<IReusableObject<IPublishableTickInstant>>.CopyFrom
        (IReusableObject<IPublishableTickInstant> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    IPublishableTickInstant ITransferState<IPublishableTickInstant>.CopyFrom
        (IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IPublishableTickInstant IPublishableTickInstant.CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public virtual PublishableTickInstant CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        QuoteContainer.CopyFrom(source, copyMergeFlags | CopyMergeFlags.ExcludeCoreTimeStamp);

        if (SourceTickerInfo == null)
            SourceTickerInfo = source.SourceTickerInfo is SourceTickerInfo
                ? source.SourceTickerInfo
                : new SourceTickerInfo(source.SourceTickerInfo!);
        else
            SourceTickerInfo.CopyFrom(source.SourceTickerInfo!, copyMergeFlags);
        return this;
    }

    public override PublishableTickInstant CopyFrom(IFeedEventStatusUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPublishableTickInstant pubTickInstant)
        {
            CopyFrom(pubTickInstant, copyMergeFlags);
        }
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

        var baseSame        = base.AreEquivalent(other, exactTypes);
        var quoteValuesSame = QuoteContainer.AreEquivalent((ITickInstant?)(other as PublishableTickInstant)?.QuoteContainer ?? other, exactTypes);
        var srcTickersAreEquivalent =
            SourceTickerInfo?.AreEquivalent(other.SourceTickerInfo, exactTypes)
         ?? other.SourceTickerInfo == null;

        var staleIsSame            = FeedSyncStatus == other.FeedSyncStatus;
        var clientReceivedTimeSame = ClientReceivedTime.Equals(other.ClientReceivedTime);

        var allEquivalent = srcTickersAreEquivalent && staleIsSame && quoteValuesSame &&
                            clientReceivedTimeSame && baseSame;
        return allEquivalent;
    }

    bool IInterfacesComparable<IFeedEventStatusUpdate>.AreEquivalent(IFeedEventStatusUpdate? other, bool exactTypes) =>
        AreEquivalent(other as IPublishableLevel1Quote, exactTypes);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPublishableTickInstant, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SourceTickerInfo != null ? SourceTickerInfo.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ base.GetHashCode();
            hashCode = (hashCode * 397) ^ QuoteContainer.GetHashCode();
            hashCode = (hashCode * 397) ^ FeedSyncStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ SingleTickValue.GetHashCode();
            hashCode = (hashCode * 397) ^ ClientReceivedTime.GetHashCode();
            return hashCode;
        }
    }

    public virtual string QuoteToStringMembers =>
        $"{nameof(SourceTickerInfo)}: {SourceTickerInfo}, {nameof(FeedSyncStatus)}: {FeedSyncStatus}, " +
        $"{nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, {AllFeedEventStatusToStringMembers}";

    public override string ToString() => 
        $"{nameof(PublishableTickInstant)} {{{QuoteToStringMembers}, {JustFeedSyncConnectivityStatusToStringMembers}, {QuoteContainer.QuoteToStringMembers}}}";
}
