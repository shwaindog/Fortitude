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
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public interface IPQLevel2Quote : IPQLevel1Quote, IMutableLevel2Quote, IDoublyLinkedListNode<IPQLevel2Quote>
{
    new IPQOrderBook OrderBook { get; set; }

    new IPQOrderBookSide BidBook { get; set; }
    new IPQOrderBookSide AskBook { get; set; }

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

    private IPQOrderBook     orderBook;

    public PQLevel2Quote()
    {
        orderBook   = new PQOrderBook();
        if (GetType() == typeof(PQLevel2Quote)) NumOfUpdates = 0;
    }
    
    // Reflection invoked constructor (PQServer<T>)
    public PQLevel2Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m)
    {
    }

    public PQLevel2Quote(ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false
      , FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, decimal singlePrice = 0m, DateTime? clientReceivedTime = null
      , DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null, DateTime? sourceBidTime = null 
      , bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null, bool isAskPriceTopChanged = false, bool executable = true, IPricePeriodSummary? periodSummary = null
      , IOrderBook? orderBook = null)
        : base(sourceTickerInfo, sourceTime, isReplay, feedSyncStatus, singlePrice, clientReceivedTime, adapterReceivedTime,
               adapterSentTime, sourceBidTime, isBidPriceTopChanged, sourceAskTime, validFrom, validTo, isAskPriceTopChanged, executable,
               periodSummary)
    {
        if (orderBook is IPQOrderBook pqOrderBook)
        {
            this.orderBook = pqOrderBook;
        } 
        else if (orderBook != null)
        {
            this.orderBook = new PQOrderBook(orderBook);
        }
        else
        {
            this.orderBook = new PQOrderBook(PQSourceTickerInfo!);
        }
        OrderBook.EnsureRelatedItemsAreConfigured(SourceTickerInfo, null);

        if (GetType() == typeof(PQLevel2Quote)) NumOfUpdates = 0;
    }

    public PQLevel2Quote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is IPQLevel2Quote l2QToClone)
        {
            orderBook = l2QToClone.OrderBook.Clone();

            IsBidPriceTopUpdated = l2QToClone.IsBidPriceTopUpdated;
            IsAskPriceTopUpdated = l2QToClone.IsAskPriceTopUpdated;
            IsBidPriceTopChanged = l2QToClone.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2QToClone.IsAskPriceTopChanged;

            IsBidPriceTopChangedUpdated = l2QToClone.IsBidPriceTopChangedUpdated;
            IsAskPriceTopChangedUpdated = l2QToClone.IsAskPriceTopChangedUpdated;
        }
        else if (toClone is ILevel2Quote l2Q)
        {
            orderBook            = new PQOrderBook(l2Q.OrderBook);

            IsBidPriceTopChanged = l2Q.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2Q.IsAskPriceTopChanged;
        }
        else
        {
            orderBook   = new PQOrderBook();
        }

        // ReSharper disable once VirtualMemberCallInConstructor
        EnsureRelatedItemsAreConfigured(toClone);
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLevel2Quote)) NumOfUpdates = 0;
    }

    protected string Level2ToStringMembers =>
        $"{Level1ToStringMembers}, {nameof(BidBook)}: {BidBook}, {nameof(AskBook)}: {AskBook}, {nameof(BidPriceTop)}: {BidPriceTop}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop}";

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
    public override bool HasUpdates
    {
        get => base.HasUpdates || orderBook.HasUpdates;
        set
        {
            base.HasUpdates              = value;
            orderBook.HasUpdates = value;
        }
    }

    IOrderBook ILevel2Quote.OrderBook => orderBook;
    IMutableOrderBook IMutableLevel2Quote.OrderBook
    {
        get => orderBook;
        set => orderBook = (IPQOrderBook)value;
    }

    public IPQOrderBook OrderBook
    {
        get => orderBook;
        set
        {
            orderBook = value;
            orderBook.EnsureRelatedItemsAreConfigured(SourceTickerInfo!, null);
        }
    }

    [JsonIgnore] IOrderBookSide ILevel2Quote.BidBook => BidBook;
    [JsonIgnore] IOrderBookSide ILevel2Quote.AskBook => AskBook;
    
    public IPQOrderBookSide BidBook
    {
        get => orderBook.BidSide;
        set
        {
            orderBook.BidSide = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    public IPQOrderBookSide AskBook
    {
        get => orderBook.AskSide;
        set
        {
            orderBook.AskSide = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => BidBook.Count > 0 ? BidBook[0]?.Price ?? 0 : 0;
        set
        {
            IsBidPriceTopUpdated = BidBook[0]!.Price != value || NumOfUpdates == 0;
            if (BidBook[0]!.Price == value) return;
            BidBook[0]!.Price = value;
            IsBidPriceTopUpdated  = true;
        }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => AskBook.Count > 0 ? AskBook[0]?.Price ?? 0 : 0;
        set
        {
            IsAskPriceTopUpdated = AskBook[0]!.Price != value || NumOfUpdates == 0;
            if (AskBook[0]!.Price == value) return;
            AskBook[0]!.Price = value;
            IsAskPriceTopUpdated  = true;
        }
    }

    public override void UpdateComplete()
    {
        OrderBook.UpdateComplete();
        base.UpdateComplete();
    }

    public override void ResetFields()
    {
        orderBook.StateReset();

        base.ResetFields();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        quotePublicationPrecisionSetting ??= PQSourceTickerInfo;
        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                               quotePublicationPrecisionSetting))
            yield return updatedField;
        foreach (var bidFields in orderBook.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                   quotePublicationPrecisionSetting))
            yield return bidFields;
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand) return (int)pqFieldUpdate.Payload;
        if (pqFieldUpdate.Id is >= PQQuoteFields.OpenInterestTotal and <= PQQuoteFields.AllLayersRangeEnd)
        {
            var result      = orderBook.UpdateField(pqFieldUpdate);
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
        if (quote is ILevel2Quote l2Quote)
        {
            OrderBook.EnsureRelatedItemsAreConfigured(l2Quote.SourceTickerInfo, l2Quote.OrderBook);
        } else if (quote?.SourceTickerInfo != null)
        {
            EnsureOrderBookRelatedItemsAreConfigured(quote.SourceTickerInfo);
        }
    }

    public virtual void EnsureOrderBookRelatedItemsAreConfigured(ISourceTickerInfo? sourceTickerInfo)
    {
        orderBook.EnsureRelatedItemsAreConfigured(sourceTickerInfo, null);
    }

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (!(other is ILevel2Quote otherL2)) return false;
        var baseSame           = base.AreEquivalent(other, exactTypes);
        var orderBookSame       = OrderBook.AreEquivalent(otherL2.OrderBook, exactTypes);

        var allAreSame = baseSame && orderBookSame;
        return allAreSame;
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
        foreach (var pqFieldStringUpdate in orderBook.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
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

    IPQLevel2Quote IPQLevel2Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQLevel2Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (!(source is ILevel2Quote l2Q)) return this;
        orderBook.CopyFrom(l2Q.OrderBook, copyMergeFlags);
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
        ((IMutableTickInstant)this).SourceTickerInfo = toSet;
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
