// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public interface IPQLevel2Quote : IPQLevel1Quote, IMutableLevel2Quote, IDoublyLinkedListNode<IPQLevel2Quote>
{
    new IPQOrderBook BidBook { get; set; }
    new IPQOrderBook AskBook { get; set; }


    new IPQLevel2Quote? Next     { get; set; }
    new IPQLevel2Quote? Previous { get; set; }

    new IPQLevel2Quote Clone();
}

public class PQLevel2Quote : PQLevel1Quote, IPQLevel2Quote, ICloneable<PQLevel2Quote>
  , IDoublyLinkedListNode<PQLevel2Quote>
{
    // ReSharper disable once UnusedMember.Local
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQLevel2Quote));

    private IPQOrderBook askBook;
    private IPQOrderBook bidBook;

    public PQLevel2Quote()
    {
        bidBook = new PQOrderBook(BookSide.BidBook, LayerType.PriceVolume);
        askBook = new PQOrderBook(BookSide.AskBook, LayerType.PriceVolume);
    }

    public PQLevel2Quote(ISourceTickerInfo sourceTickerInfo)
        : base(sourceTickerInfo)
    {
        bidBook = new PQOrderBook(BookSide.BidBook, PQSourceTickerInfo!);
        askBook = new PQOrderBook(BookSide.AskBook, PQSourceTickerInfo!);
        // ReSharper disable once VirtualMemberCallInConstructor
        EnsureRelatedItemsAreConfigured(this);
    }

    public PQLevel2Quote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is IPQLevel2Quote l2QToClone)
        {
            bidBook = l2QToClone.BidBook.Clone();
            askBook = l2QToClone.AskBook.Clone();

            IsBidPriceTopUpdated        = l2QToClone.IsBidPriceTopUpdated;
            IsAskPriceTopUpdated        = l2QToClone.IsAskPriceTopUpdated;
            IsBidPriceTopUpdatedChanged = l2QToClone.IsBidPriceTopUpdated;
            IsAskPriceTopUpdatedChanged = l2QToClone.IsAskPriceTopUpdated;
        }
        else if (toClone is ILevel2Quote l2Q)
        {
            bidBook = new PQOrderBook(l2Q.BidBook);
            askBook = new PQOrderBook(l2Q.AskBook);

            IsBidPriceTopUpdated = l2Q.IsBidPriceTopUpdated;
            IsAskPriceTopUpdated = l2Q.IsAskPriceTopUpdated;
        }
        else
        {
            bidBook = new PQOrderBook(BookSide.BidBook, PQSourceTickerInfo!);
            askBook = new PQOrderBook(BookSide.AskBook, PQSourceTickerInfo!);
        }

        // ReSharper disable once VirtualMemberCallInConstructor
        EnsureRelatedItemsAreConfigured(toClone);
        SetFlagsSame(toClone);
    }

    protected string Level2ToStringMembers =>
        $"{Level1ToStringMembers}, {nameof(IsBidBookChanged)}: {IsBidBookChanged}, {nameof(IsAskBookChanged)}: {IsAskBookChanged}, {nameof(BidBook)}: {BidBook}, {nameof(AskBook)}: {AskBook}, {nameof(BidPriceTop)}: {BidPriceTop}, {nameof(AskPriceTop)}: {AskPriceTop}";

    public override PQLevel2Quote Clone() =>
        Recycler?.Borrow<PQLevel2Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) as PQLevel2Quote ?? new PQLevel2Quote(this);

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
        get => bidBook.HasUpdates;
        set => bidBook.HasUpdates = value;
    }

    [JsonIgnore]
    public bool IsAskBookChanged
    {
        get => askBook.HasUpdates;
        set => askBook.HasUpdates = value;
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates || bidBook.HasUpdates || askBook.HasUpdates;
        set
        {
            base.HasUpdates    = value;
            bidBook.HasUpdates = value;
            askBook.HasUpdates = value;
        }
    }

    [JsonIgnore] IOrderBook ILevel2Quote.BidBook => BidBook;
    [JsonIgnore] IOrderBook ILevel2Quote.AskBook => AskBook;

    [JsonIgnore]
    IMutableOrderBook IMutableLevel2Quote.BidBook
    {
        get => BidBook;
        set => BidBook = (IPQOrderBook)value;
    }

    [JsonIgnore]
    IMutableOrderBook IMutableLevel2Quote.AskBook
    {
        get => AskBook;
        set => AskBook = (IPQOrderBook)value;
    }

    public IPQOrderBook BidBook
    {
        get => bidBook;
        set
        {
            bidBook = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    public IPQOrderBook AskBook
    {
        get => askBook;
        set
        {
            askBook = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => BidBook.Count > 0 ? BidBook[0]?.Price ?? 0 : 0;
        set
        {
            if (BidBook[0]!.Price == value) return;
            BidBook[0]!.Price    = value;
            IsBidPriceTopUpdated = true;
        }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => AskBook.Count > 0 ? AskBook[0]?.Price ?? 0 : 0;
        set
        {
            if (AskBook[0]!.Price == value) return;
            AskBook[0]!.Price    = value;
            IsAskPriceTopUpdated = true;
        }
    }

    public override void ResetFields()
    {
        bidBook.StateReset();
        askBook.StateReset();
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
        foreach (var bidFields in bidBook.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                               quotePublicationPrecisionSetting))
            yield return bidFields;

        foreach (var askField in askBook.GetDeltaUpdateFields(snapShotTime,
                                                              messageFlags, quotePublicationPrecisionSetting))
            yield return askField.SetIsAsk();
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand) return (int)pqFieldUpdate.Payload;
        if (pqFieldUpdate.Id is >= PQQuoteFields.ShiftBookByLayers and <= PQQuoteFields.AllLayersRangeEnd)
        {
            // logger.Info("Received PQLevel2Quote Book pqFieldUpdate: {0}", pqFieldUpdate);
            var topBidPrice = BidPriceTop;
            var topAskPrice = AskPriceTop;
            var result      = pqFieldUpdate.IsBid() ? bidBook.UpdateField(pqFieldUpdate) : askBook.UpdateField(pqFieldUpdate);
            if (pqFieldUpdate.Id == PQQuoteFields.Price && pqFieldUpdate.DepthId.KeyToDepth() == 0)
            {
                if (pqFieldUpdate.IsBid())
                    IsBidPriceTopUpdatedChanged = topBidPrice != BidPriceTop;
                else
                    IsAskPriceTopUpdatedChanged = topAskPrice != AskPriceTop;
            }

            return result;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
        foreach (var pqFieldStringUpdate in bidBook.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
        foreach (var pqFieldStringUpdate in askBook.GetStringUpdates(snapShotTime, messageFlags))
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
                return BidBook.UpdateFieldString(stringUpdate);
            else
                return AskBook.UpdateFieldString(stringUpdate);

        return false;
    }

    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => Clone();

    ILevel2Quote ILevel2Quote.Clone() => Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => Clone();

    IPQLevel2Quote IPQLevel2Quote.Clone() => Clone();

    public override ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (!(source is ILevel2Quote l2Q)) return this;
        bidBook.CopyFrom(l2Q.BidBook, copyMergeFlags);
        askBook.CopyFrom(l2Q.AskBook, copyMergeFlags);
        if (source is not IPQLevel1Quote pq1)
        {
            IsBidPriceTopUpdated = l2Q.IsBidPriceTopUpdated;
            IsAskPriceTopUpdated = l2Q.IsAskPriceTopUpdated;
        }
        else
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pq1.IsBidPriceTopUpdatedChanged || isFullReplace)
            {
                IsBidPriceTopUpdated        = pq1.IsBidPriceTopUpdated;
                IsBidPriceTopUpdatedChanged = pq1.IsBidPriceTopUpdatedChanged;
            }
            if (pq1.IsAskPriceTopUpdatedChanged || isFullReplace)
            {
                IsAskPriceTopUpdated        = pq1.IsAskPriceTopUpdated;
                IsAskPriceTopUpdatedChanged = pq1.IsAskPriceTopUpdatedChanged;
            }

            if (isFullReplace && pq1 is PQLevel2Quote pq2) UpdatedFlags = pq2.UpdatedFlags;
        }
        return this;
    }

    public override void EnsureRelatedItemsAreConfigured(ITickInstant? quote)
    {
        base.EnsureRelatedItemsAreConfigured(quote);

        if (ReferenceEquals(quote, this) && quote is IPQLevel2Quote pqLevel2Quote)
        {
            BidBook.EnsureRelatedItemsAreConfigured(pqLevel2Quote.BidBook.LayersSupportsLayerFlags, pqLevel2Quote.BidBook.NameIdLookup);
            AskBook.EnsureRelatedItemsAreConfigured(pqLevel2Quote.AskBook.LayersSupportsLayerFlags, pqLevel2Quote.AskBook.NameIdLookup);
        }
        else
        {
            BidBook.EnsureRelatedItemsAreConfigured(PQSourceTickerInfo);
            AskBook.EnsureRelatedItemsAreConfigured(PQSourceTickerInfo);
        }
    }

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (!(other is ILevel2Quote otherL2)) return false;
        var baseSame           = base.AreEquivalent(other, exactTypes);
        var bidBooksSame       = BidBook.AreEquivalent(otherL2.BidBook, exactTypes);
        var askBookSame        = AskBook.AreEquivalent(otherL2.AskBook, exactTypes);
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
            hashCode = (hashCode * 397) ^ BidBook.GetHashCode();
            hashCode = (hashCode * 397) ^ AskBook.GetHashCode();
            // ReSharper restore NonReadonlyMemberInGetHashCode
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({Level2ToStringMembers}, {UpdatedFlagsToString})";
}
