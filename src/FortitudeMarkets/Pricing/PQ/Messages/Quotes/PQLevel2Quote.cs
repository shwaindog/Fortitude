// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public interface IPQLevel2Quote : IPQLevel1Quote, IMutableLevel2Quote, IDoublyLinkedListNode<IPQLevel2Quote>
{
    new IPQOrderBookSide BidBookSide { get; set; }
    new IPQOrderBookSide AskBookSide { get; set; }


    new IPQLevel2Quote? Next     { get; set; }
    new IPQLevel2Quote? Previous { get; set; }

    new IPQLevel2Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);

    new IPQLevel2Quote Clone();
}

public class PQLevel2Quote : PQLevel1Quote, IPQLevel2Quote, ICloneable<PQLevel2Quote>
  , IDoublyLinkedListNode<PQLevel2Quote>
{
    // ReSharper disable once UnusedMember.Local
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQLevel2Quote));

    private IPQOrderBookSide askBookSide;
    private IPQOrderBookSide bidBookSide;

    public PQLevel2Quote()
    {
        bidBookSide = new PQOrderBookSide(BookSide.BidBook, LayerType.PriceVolume);
        askBookSide = new PQOrderBookSide(BookSide.AskBook, LayerType.PriceVolume);

        if (GetType() == typeof(PQLevel2Quote)) NumOfUpdates = 0;
    }

    public PQLevel2Quote(ISourceTickerInfo sourceTickerInfo)
        : base(sourceTickerInfo)
    {
        bidBookSide = new PQOrderBookSide(BookSide.BidBook, PQSourceTickerInfo!);
        askBookSide = new PQOrderBookSide(BookSide.AskBook, PQSourceTickerInfo!);
        // ReSharper disable once VirtualMemberCallInConstructor
        EnsureRelatedItemsAreConfigured(this);

        if (GetType() == typeof(PQLevel2Quote)) NumOfUpdates = 0;
    }

    public PQLevel2Quote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is IPQLevel2Quote l2QToClone)
        {
            bidBookSide = l2QToClone.BidBookSide.Clone();
            askBookSide = l2QToClone.AskBookSide.Clone();

            IsBidPriceTopUpdated = l2QToClone.IsBidPriceTopUpdated;
            IsAskPriceTopUpdated = l2QToClone.IsAskPriceTopUpdated;
            IsBidPriceTopChanged = l2QToClone.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2QToClone.IsAskPriceTopChanged;

            IsBidPriceTopChangedUpdated = l2QToClone.IsBidPriceTopChangedUpdated;
            IsAskPriceTopChangedUpdated = l2QToClone.IsAskPriceTopChangedUpdated;

        }
        else if (toClone is ILevel2Quote l2Q)
        {
            bidBookSide = new PQOrderBookSide(l2Q.BidBookSide);
            askBookSide = new PQOrderBookSide(l2Q.AskBookSide);
            
            IsBidPriceTopChanged = l2Q.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2Q.IsAskPriceTopChanged;
        }
        else
        {
            bidBookSide = new PQOrderBookSide(BookSide.BidBook, PQSourceTickerInfo!);
            askBookSide = new PQOrderBookSide(BookSide.AskBook, PQSourceTickerInfo!);
        }

        // ReSharper disable once VirtualMemberCallInConstructor
        EnsureRelatedItemsAreConfigured(toClone);
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLevel2Quote)) NumOfUpdates = 0;
    }

    protected string Level2ToStringMembers =>
        $"{Level1ToStringMembers}, {nameof(IsBidBookChanged)}: {IsBidBookChanged}, {nameof(IsAskBookChanged)}: {IsAskBookChanged}, {nameof(BidBookSide)}: {BidBookSide}, {nameof(AskBookSide)}: {AskBookSide}, {nameof(BidPriceTop)}: {BidPriceTop}, {nameof(AskPriceTop)}: {AskPriceTop}";

    public override PQLevel2Quote Clone() =>
        Recycler?.Borrow<PQLevel2Quote>().CopyFrom(this, CopyMergeFlags.FullReplace)  ?? new PQLevel2Quote(this);

    [JsonIgnore]
    public new PQLevel2Quote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PQLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new PQLevel2Quote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PQLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQLevel2Quote? IPQLevel2Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQLevel2Quote? IPQLevel2Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel2Quote? ILevel2Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel2Quote? ILevel2Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel2Quote? IDoublyLinkedListNode<ILevel2Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel2Quote? IDoublyLinkedListNode<ILevel2Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQLevel2Quote? IDoublyLinkedListNode<IPQLevel2Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQLevel2Quote? IDoublyLinkedListNode<IPQLevel2Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore] public override TickerDetailLevel TickerDetailLevel => TickerDetailLevel.Level2Quote;

    [JsonIgnore]
    public bool IsBidBookChanged
    {
        get => bidBookSide.HasUpdates;
        set => bidBookSide.HasUpdates = value;
    }

    [JsonIgnore]
    public bool IsAskBookChanged
    {
        get => askBookSide.HasUpdates;
        set => askBookSide.HasUpdates = value;
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates || bidBookSide.HasUpdates || askBookSide.HasUpdates;
        set
        {
            base.HasUpdates        = value;
            bidBookSide.HasUpdates = value;
            askBookSide.HasUpdates = value;
        }
    }

    [JsonIgnore] IOrderBookSide ILevel2Quote.BidBookSide => BidBookSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.AskBookSide => AskBookSide;

    [JsonIgnore]
    IMutableOrderBookSide IMutableLevel2Quote.BidBookSide
    {
        get => BidBookSide;
        set => BidBookSide = (IPQOrderBookSide)value;
    }

    [JsonIgnore]
    IMutableOrderBookSide IMutableLevel2Quote.AskBookSide
    {
        get => AskBookSide;
        set => AskBookSide = (IPQOrderBookSide)value;
    }

    public IPQOrderBookSide BidBookSide
    {
        get => bidBookSide;
        set
        {
            bidBookSide = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    public IPQOrderBookSide AskBookSide
    {
        get => askBookSide;
        set
        {
            askBookSide = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => BidBookSide.Count > 0 ? BidBookSide[0]?.Price ?? 0 : 0;
        set
        {
            IsBidPriceTopUpdated = BidBookSide[0]!.Price != value || NumOfUpdates == 0;
            if (BidBookSide[0]!.Price == value) return;
            BidBookSide[0]!.Price = value;
            IsBidPriceTopUpdated  = true;
        }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => AskBookSide.Count > 0 ? AskBookSide[0]?.Price ?? 0 : 0;
        set
        {
            IsAskPriceTopUpdated = AskBookSide[0]!.Price != value || NumOfUpdates == 0;
            if (AskBookSide[0]!.Price == value) return;
            AskBookSide[0]!.Price = value;
            IsAskPriceTopUpdated  = true;
        }
    }

    public override void UpdateComplete()
    {
        AskBookSide.UpdateComplete();
        BidBookSide.UpdateComplete();
        base.UpdateComplete();
    }

    public override void ResetFields()
    {
        bidBookSide.StateReset();
        askBookSide.StateReset();

        base.ResetFields();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        quotePublicationPrecisionSetting = quotePublicationPrecisionSetting ?? PQSourceTickerInfo;
        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                               quotePublicationPrecisionSetting))
            yield return updatedField;
        foreach (var bidFields in bidBookSide.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                   quotePublicationPrecisionSetting))
            yield return bidFields;

        foreach (var askField in askBookSide.GetDeltaUpdateFields(snapShotTime,
                                                                  messageFlags, quotePublicationPrecisionSetting))
            yield return askField.SetIsAsk();
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand) return (int)pqFieldUpdate.Payload;
        if (pqFieldUpdate.Id is >= PQQuoteFields.ShiftBookByLayers and <= PQQuoteFields.AllLayersRangeEnd)
        {
            // logger.Info("Received PQLevel2Quote Book pqFieldUpdate: {0}", pqFieldUpdate);
            var result      = pqFieldUpdate.IsBid() ? bidBookSide.UpdateField(pqFieldUpdate) : askBookSide.UpdateField(pqFieldUpdate);
            // pass Best Price through to Level 1 quote 
            if (!(pqFieldUpdate.Id == PQQuoteFields.Price && pqFieldUpdate.DepthId.KeyToDepth() == 0))
            {
                return result;
            }
        }

        return base.UpdateField(pqFieldUpdate);
    }

    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => Clone();

    ILevel2Quote ILevel2Quote.Clone() => Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => Clone();

    IPQLevel2Quote IPQLevel2Quote.Clone() => Clone();

    public override void EnsureRelatedItemsAreConfigured(ITickInstant? quote)
    {
        base.EnsureRelatedItemsAreConfigured(quote);

        if (ReferenceEquals(quote, this) && quote is IPQLevel2Quote pqLevel2Quote)
        {
            BidBookSide.EnsureRelatedItemsAreConfigured(pqLevel2Quote.BidBookSide.LayersSupportsLayerFlags, pqLevel2Quote.BidBookSide.NameIdLookup);
            AskBookSide.EnsureRelatedItemsAreConfigured(pqLevel2Quote.AskBookSide.LayersSupportsLayerFlags, pqLevel2Quote.AskBookSide.NameIdLookup);
        }
        else
        {
            BidBookSide.EnsureRelatedItemsAreConfigured(PQSourceTickerInfo);
            AskBookSide.EnsureRelatedItemsAreConfigured(PQSourceTickerInfo);
        }
    }

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (!(other is ILevel2Quote otherL2)) return false;
        var baseSame           = base.AreEquivalent(other, exactTypes);
        var bidBooksSame       = BidBookSide.AreEquivalent(otherL2.BidBookSide, exactTypes);
        var askBookSame        = AskBookSide.AreEquivalent(otherL2.AskBookSide, exactTypes);
        var bidBookChangedSame = true;
        var askBookChangedSame = true;
        if (exactTypes)
        {
            bidBookChangedSame = IsBidBookChanged == otherL2.IsBidBookChanged;
            askBookChangedSame = IsAskBookChanged == otherL2.IsAskBookChanged;
        }

        var allAreSame = baseSame && bidBooksSame && askBookSame && bidBookChangedSame && askBookChangedSame;
        return allAreSame;
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
        foreach (var pqFieldStringUpdate in bidBookSide.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
        foreach (var pqFieldStringUpdate in askBookSide.GetStringUpdates(snapShotTime, messageFlags))
        {
            var updatedStringUpdate = pqFieldStringUpdate.SetIsAsk();
            yield return updatedStringUpdate;
        }
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        var found = base.UpdateFieldString(stringUpdate);
        if (found) return true;

        if (stringUpdate.Field.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand)
            if (stringUpdate.IsBid())
                return BidBookSide.UpdateFieldString(stringUpdate);
            else
                return AskBookSide.UpdateFieldString(stringUpdate);

        return false;
    }


    IPQLevel2Quote IPQLevel2Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQLevel2Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (!(source is ILevel2Quote l2Q)) return this;
        bidBookSide.CopyFrom(l2Q.BidBookSide, copyMergeFlags);
        askBookSide.CopyFrom(l2Q.AskBookSide, copyMergeFlags);
        if (source is not IPQLevel1Quote pq1)
        {
            BidPriceTop          = l2Q.BidPriceTop;
            AskPriceTop          = l2Q.AskPriceTop;
            IsBidPriceTopChanged = l2Q.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2Q.IsAskPriceTopChanged;
        }
        else
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            
            if (pq1.IsBidPriceTopUpdated || isFullReplace)
            {
                BidPriceTop                 = pq1.BidPriceTop;
                IsBidPriceTopUpdated        = true;
                IsBidPriceTopChanged        = pq1.IsBidPriceTopChanged;
                IsBidPriceTopChangedUpdated = pq1.IsBidPriceTopChangedUpdated;
            }
            if (pq1.IsAskPriceTopUpdated || isFullReplace)
            {
                AskPriceTop                 = pq1.AskPriceTop;
                IsAskPriceTopUpdated        = true;
                IsAskPriceTopChanged        = pq1.IsAskPriceTopChanged;
                IsAskPriceTopChangedUpdated = pq1.IsAskPriceTopChangedUpdated;
            }
            if (pq1.IsBidPriceTopChangedUpdated || isFullReplace)
            {
                IsBidPriceTopChanged        = pq1.IsBidPriceTopChanged;
                IsBidPriceTopChangedUpdated = true;
            }
            if (pq1.IsAskPriceTopChangedUpdated || isFullReplace)
            {
                IsAskPriceTopChanged        = pq1.IsAskPriceTopChanged;
                IsAskPriceTopChangedUpdated = true;
            }

            if (isFullReplace && pq1 is PQLevel2Quote pq2) UpdatedFlags = pq2.UpdatedFlags;
        }
        return this;
    }

    public override PQTickInstant SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        SourceTickerInfo = toSet;
        EnsureRelatedItemsAreConfigured(this);
        return this;
    }

    protected override IEnumerable<PQFieldUpdate> GetDeltaUpdateTopBookPriceFields
    (DateTime snapShotTime,
        bool updatedOnly, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        yield break;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ITickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            // ReSharper disable NonReadonlyMemberInGetHashCode
            hashCode = (hashCode * 397) ^ BidBookSide.GetHashCode();
            hashCode = (hashCode * 397) ^ AskBookSide.GetHashCode();
            // ReSharper restore NonReadonlyMemberInGetHashCode
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({Level2ToStringMembers}, {UpdatedFlagsToString})";
}
