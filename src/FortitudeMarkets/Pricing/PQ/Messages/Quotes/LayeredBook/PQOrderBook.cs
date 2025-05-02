using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook
{
    public interface IPQOrderBook : IMutableOrderBook, IPQSupportsFieldUpdates<IOrderBook>, IPQSupportsStringUpdates<IOrderBook>
      , IRelatedItem<IOrderBook>, ICloneable<IPQOrderBook>, IRelatedLinkedItem<LayerFlags, IPQNameIdLookupGenerator>,
        ISupportsPQNameIdLookupGenerator
    {
        new IPQOrderBookSide BidSide { get; set; }
        new IPQOrderBookSide AskSide { get; set; }

        new bool HasUpdates { get; set; }

        new IPQNameIdLookupGenerator NameIdLookup { get; set; }

        decimal? SourceOpenInterestVolume  { get; set; }
        decimal? SourceOpenInterestVwap    { get; set; }
        decimal  AdapterOpenInterestVolume { get; set; }
        decimal  AdapterOpenInterestVwap   { get; set; }

        bool IsSourceOpenInterestVolumeUpdated  { get; set; }
        bool IsSourceOpenInterestVwapUpdated    { get; set; }
        bool IsAdapterOpenInterestVolumeUpdated { get; set; }
        bool IsAdapterOpenInterestVwapUpdated   { get; set; }

        new IPQOrderBook Clone();
    }

    [Flags]
    public enum PQOrderBookUpdatedFlags
    {
        None                             = 0
      , SourceOpenInterestVolumeUpdated  = 1
      , SourceOpenInterestVwapUpdated    = 2
      , AdapterOpenInterestVolumeUpdated = 4
      , AdapterOpenInterestVwapUpdated   = 8
    }

    public class PQOrderBook : ReusableObject<IOrderBook>, IPQOrderBook
    {
        private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrderBookSide));

        private decimal adapterOpenInterestVolume;
        private decimal adapterOpenInterestVwap;

        private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;

        protected uint NumOfUpdates = uint.MaxValue;

        private decimal? sourceOpenInterestVolume;
        private decimal? sourceOpenInterestVwap;

        protected OrderBookSideUpdatedFlags UpdatedFlags;

        public PQOrderBook() : this(LayerType.PriceVolume)
        {
            if (GetType() == typeof(PQOrderBook)) NumOfUpdates = 0;
        }

        public PQOrderBook
        (LayerType layerType = LayerType.PriceVolume,
            ushort numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers,
            bool isLadder = false)
        {
            LayersOfType          = layerType;
            IsLadder              = isLadder;
            MaxPublishDepth       = numBookLayers;
            nameIdLookupGenerator = SourceOtherExistingOrNewPQNameIdNameLookup(this);

            AskSide = new PQOrderBookSide(BookSide.AskBook, layerType, numBookLayers, isLadder, nameIdLookupGenerator);
            BidSide = new PQOrderBookSide(BookSide.BidBook, layerType, numBookLayers, isLadder, nameIdLookupGenerator);

            if (GetType() == typeof(PQOrderBook)) NumOfUpdates = 0;
        }

        public PQOrderBook(IPQSourceTickerInfo srcTickerInfo)
        {
            IsLadder              = srcTickerInfo.LayerFlags.HasLadder();
            nameIdLookupGenerator = InitializeNewIdLookupGenerator(srcTickerInfo.NameIdLookup);

            AskSide = new PQOrderBookSide(BookSide.AskBook, srcTickerInfo, nameIdLookupGenerator);
            BidSide = new PQOrderBookSide(BookSide.BidBook, srcTickerInfo, nameIdLookupGenerator);

            EnsureRelatedItemsAreConfigured(srcTickerInfo);

            if (GetType() == typeof(PQOrderBook)) NumOfUpdates = 0;
        }

        public PQOrderBook
            (IEnumerable<IPriceVolumeLayer>? bidBookLayers = null, IEnumerable<IPriceVolumeLayer>? askBookLayers = null, bool isLadder = false)
        {
            IsLadder     = isLadder;
            LayersOfType = (bidBookLayers ?? []).Concat(askBookLayers ?? [])?.FirstOrDefault()?.LayerType ?? LayerType.PriceVolume;

            nameIdLookupGenerator = SourceOtherExistingOrNewPQNameIdNameLookup(bidBookLayers);

            AskSide = new PQOrderBookSide(BookSide.AskBook, askBookLayers, isLadder, nameIdLookupGenerator);
            BidSide = new PQOrderBookSide(BookSide.BidBook, bidBookLayers, isLadder, nameIdLookupGenerator);

            if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
        }

        public PQOrderBook(IOrderBook toClone)
        {
            IsLadder = toClone.IsLadder;

            nameIdLookupGenerator = SourceOtherExistingOrNewPQNameIdNameLookup(toClone);

            AskSide = new PQOrderBookSide(toClone.AskSide, nameIdLookupGenerator);
            BidSide = new PQOrderBookSide(toClone.BidSide, nameIdLookupGenerator);

            if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
        }

        protected string PQOrderBookToStringMembers =>
            $"{nameof(LayersOfType)}: {LayersOfType}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
            $"{nameof(MaxPublishDepth)}, {MaxPublishDepth}, {nameof(SourceOpenInterest)}: {SourceOpenInterest}, " +
            $"{nameof(AdapterOpenInterest)}: {AdapterOpenInterest}, {nameof(IsAskBookChanged)}: {IsAskBookChanged},  " +
            $"{nameof(IsBidBookChanged)}: {IsBidBookChanged}, {nameof(AskSide)}: {AskSide}, {nameof(BidSide)}: {BidSide}, " +
            $"{nameof(IsLadder)}: {IsLadder}";

        INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

        public LayerType  LayersOfType { get; set; }
        public LayerFlags LayerFlags   => LayersOfType.SupportedLayerFlags();

        public IPQOrderBookSide AskSide { get; set; }
        public IPQOrderBookSide BidSide { get; set; }

        IOrderBookSide IOrderBook.AskSide => AskSide;

        IOrderBookSide IOrderBook.BidSide => BidSide;

        IMutableOrderBookSide IMutableOrderBook.AskSide
        {
            get => AskSide;
            set => AskSide = (PQOrderBookSide)value;
        }

        IMutableOrderBookSide IMutableOrderBook.BidSide
        {
            get => BidSide;
            set => BidSide = (PQOrderBookSide)value;
        }

        public IPQNameIdLookupGenerator NameIdLookup
        {
            get => nameIdLookupGenerator;
            set
            {
                if (nameIdLookupGenerator == value) return;
                nameIdLookupGenerator = value;

                BidSide.NameIdLookup = value;
                AskSide.NameIdLookup = value;
            }
        }

        public bool IsBidBookChanged
        {
            get => BidSide.HasUpdates;
            set => BidSide.HasUpdates = value;
        }

        public bool IsAskBookChanged
        {
            get => AskSide.HasUpdates;
            set => AskSide.HasUpdates = value;
        }

        public bool IsLadder { get; set; }

        public ushort MaxPublishDepth { get; private set; }

        public decimal? MidPrice => (BidSide[0]?.Price ?? 0 + AskSide[0]?.Price ?? 0) / 2;

        public uint DailyTickUpdateCount { get; set; }

        public bool HasUpdates
        {
            get => BidSide.HasUpdates || AskSide.HasUpdates;
            set
            {
                BidSide.HasUpdates = value;
                AskSide.HasUpdates = value;
            }
        }

        public decimal? SourceOpenInterestVolume
        {
            get => sourceOpenInterestVolume;
            set
            {
                IsSourceOpenInterestVolumeUpdated |= sourceOpenInterestVolume != value || NumOfUpdates == 0;
                sourceOpenInterestVolume          =  value;
            }
        }

        public decimal? SourceOpenInterestVwap
        {
            get => sourceOpenInterestVwap;
            set
            {
                IsSourceOpenInterestVwapUpdated |= sourceOpenInterestVwap != value || NumOfUpdates == 0;
                sourceOpenInterestVwap          =  value;
            }
        }

        public decimal AdapterOpenInterestVolume
        {
            get => adapterOpenInterestVolume;
            set
            {
                IsAdapterOpenInterestVolumeUpdated |= adapterOpenInterestVolume != value || NumOfUpdates == 0;
                adapterOpenInterestVolume          =  value;
            }
        }

        public decimal AdapterOpenInterestVwap
        {
            get => adapterOpenInterestVwap;
            set
            {
                IsAdapterOpenInterestVwapUpdated |= adapterOpenInterestVwap != value || NumOfUpdates == 0;
                adapterOpenInterestVwap          =  value;
            }
        }

        public bool IsSourceOpenInterestVolumeUpdated
        {
            get => (UpdatedFlags & OrderBookSideUpdatedFlags.SourceOpenInterestVolumeUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= OrderBookSideUpdatedFlags.SourceOpenInterestVolumeUpdated;

                else if (IsSourceOpenInterestVolumeUpdated) UpdatedFlags ^= OrderBookSideUpdatedFlags.SourceOpenInterestVolumeUpdated;
            }
        }

        public bool IsSourceOpenInterestVwapUpdated
        {
            get => (UpdatedFlags & OrderBookSideUpdatedFlags.SourceOpenInterestVwapUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= OrderBookSideUpdatedFlags.SourceOpenInterestVwapUpdated;

                else if (IsSourceOpenInterestVwapUpdated) UpdatedFlags ^= OrderBookSideUpdatedFlags.SourceOpenInterestVwapUpdated;
            }
        }

        public bool IsAdapterOpenInterestVolumeUpdated
        {
            get => (UpdatedFlags & OrderBookSideUpdatedFlags.AdapterOpenInterestVolumeUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= OrderBookSideUpdatedFlags.AdapterOpenInterestVolumeUpdated;

                else if (IsAdapterOpenInterestVolumeUpdated) UpdatedFlags ^= OrderBookSideUpdatedFlags.AdapterOpenInterestVolumeUpdated;
            }
        }

        public bool IsAdapterOpenInterestVwapUpdated
        {
            get => (UpdatedFlags & OrderBookSideUpdatedFlags.AdapterOpenInterestVwapUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= OrderBookSideUpdatedFlags.AdapterOpenInterestVwapUpdated;

                else if (IsAdapterOpenInterestVwapUpdated) UpdatedFlags ^= OrderBookSideUpdatedFlags.AdapterOpenInterestVwapUpdated;
            }
        }

        public OpenInterest? SourceOpenInterest
        {
            get
            {
                if (sourceOpenInterestVolume != null || sourceOpenInterestVwap != null)
                    return new OpenInterest(MarketDataSource.Venue, sourceOpenInterestVolume ?? 0m, sourceOpenInterestVwap ?? 0);
                return null;
            }
            set
            {
                SourceOpenInterestVolume = value?.Volume;
                SourceOpenInterestVwap   = value?.Vwap;
            }
        }

        public OpenInterest AdapterOpenInterest
        {
            get => new(MarketDataSource.Venue, adapterOpenInterestVolume, adapterOpenInterestVwap);
            set
            {
                SourceOpenInterestVolume = value.Volume;
                SourceOpenInterestVwap   = value.Vwap;
            }
        }

        public OpenInterest PublishedOpenInterest
        {
            get
            {
                var bidVwapResult = BidSide.CalculateVwap();
                var askVwapResult = AskSide.CalculateVwap();

                var totalVolume = bidVwapResult.VolumeAchieved + askVwapResult.VolumeAchieved;
                var totalVolPrice = (bidVwapResult.VolumeAchieved * bidVwapResult.AchievedVwap) +
                                    (askVwapResult.VolumeAchieved * askVwapResult.AchievedVwap);

                var bidAskVwap = totalVolume > 0 ? totalVolPrice / totalVolume : 0m;

                return new OpenInterest(MarketDataSource.Published, totalVolume, bidAskVwap);
            }
        }

        public uint UpdateCount => NumOfUpdates;

        public void UpdateComplete()
        {
            AskSide.UpdateComplete();
            BidSide.UpdateComplete();
            NameIdLookup.UpdateComplete();
            if (HasUpdates)
            {
                NumOfUpdates++;
                HasUpdates = false;
            }
        }

        public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags,
            IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
        {
            foreach (var bidFields in BidSide.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
                yield return bidFields;

            foreach (var askField in AskSide.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
                yield return askField.SetIsAsk();
        }

        public int UpdateField(PQFieldUpdate pqFieldUpdate)
        {
            // logger.Info("Received PQLevel2Quote Book pqFieldUpdate: {0}", pqFieldUpdate);
            return pqFieldUpdate.IsBid() ? BidSide.UpdateField(pqFieldUpdate) : AskSide.UpdateField(pqFieldUpdate);
        }

        object ICloneable.Clone() => Clone();

        IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

        IMutableOrderBook IMutableOrderBook.Clone() => Clone();

        IPQOrderBook ICloneable<IPQOrderBook>.Clone() => Clone();

        IPQOrderBook IPQOrderBook.Clone() => Clone();

        public override PQOrderBook Clone() => Recycler?.Borrow<PQOrderBook>().CopyFrom(this) ?? new PQOrderBook(this);

        public override PQOrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            if (source is PQOrderBook sourcePqOrderBook) NameIdLookup.CopyFrom(sourcePqOrderBook.NameIdLookup, copyMergeFlags);
            LayersOfType         = source.LayersOfType;
            IsLadder             = source.IsLadder;
            MaxPublishDepth      = MaxPublishDepth;
            DailyTickUpdateCount = source.DailyTickUpdateCount;
            SourceOpenInterest   = source.SourceOpenInterest;
            AdapterOpenInterest  = source.AdapterOpenInterest;
            BidSide.CopyFrom(source.BidSide, copyMergeFlags);
            AskSide.CopyFrom(source.AskSide, copyMergeFlags);
            return this;
        }

        public void EnsureRelatedItemsAreConfigured(IOrderBook? referenceInstance)
        {
            LayersOfType    = referenceInstance?.LayersOfType ?? LayersOfType;
            MaxPublishDepth = referenceInstance?.MaxPublishDepth ?? MaxPublishDepth;
            IsLadder        = referenceInstance?.IsLadder ?? IsLadder;
            if (ReferenceEquals(referenceInstance, this) && referenceInstance is IPQOrderBook pqOrderBook)
            {
                BidSide.EnsureRelatedItemsAreConfigured(pqOrderBook.BidSide.LayersSupportsLayerFlags, pqOrderBook.BidSide.NameIdLookup);
                AskSide.EnsureRelatedItemsAreConfigured(pqOrderBook.AskSide.LayersSupportsLayerFlags, pqOrderBook.AskSide.NameIdLookup);
            }
            else
            {
                BidSide.EnsureRelatedItemsAreConfigured(referenceInstance.LayerFlags, NameIdLookup);
                AskSide.EnsureRelatedItemsAreConfigured(referenceInstance.LayerFlags, NameIdLookup);
            }
        }

        public void EnsureRelatedItemsAreConfigured(LayerFlags layerFlags, IPQNameIdLookupGenerator? otherNameIdLookupGenerator)
        {
            LayersOfType = layerFlags.MostCompactLayerType();
            BidSide.EnsureRelatedItemsAreConfigured(layerFlags, otherNameIdLookupGenerator);
            AskSide.EnsureRelatedItemsAreConfigured(layerFlags, otherNameIdLookupGenerator);
        }

        public void EnsureRelatedItemsAreConfigured(IPQSourceTickerInfo? referenceInstance)
        {
            LayersOfType    = referenceInstance?.LayerFlags.MostCompactLayerType() ?? LayersOfType;
            MaxPublishDepth = referenceInstance?.MaximumPublishedLayers ?? MaxPublishDepth;
            IsLadder        = referenceInstance?.LayerFlags.HasLadder() ?? false;
            BidSide.EnsureRelatedItemsAreConfigured(referenceInstance);
            AskSide.EnsureRelatedItemsAreConfigured(referenceInstance);
        }



        bool IInterfacesComparable<IMutableOrderBook>.AreEquivalent(IMutableOrderBook? other, bool exactTypes) => AreEquivalent(other, exactTypes);

        public virtual bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;

            var layerTypesSame          = LayersOfType == other.LayersOfType;
            var isLadderSame            = IsLadder == other.IsLadder;
            var maxDepthSame            = MaxPublishDepth == other.MaxPublishDepth;
            var dailyTickCountSame      = DailyTickUpdateCount == other.DailyTickUpdateCount;
            var sourceOpenInterestSame  = Equals(SourceOpenInterest, other.SourceOpenInterest);
            var adapterOpenInterestSame = Equals(AdapterOpenInterest, other.AdapterOpenInterest);

            var bidBooksSame       = BidSide.AreEquivalent(other.BidSide, exactTypes);
            var askBookSame        = AskSide.AreEquivalent(other.AskSide, exactTypes);
            var bidBookChangedSame = true;
            var askBookChangedSame = true;
            if (exactTypes)
            {
                bidBookChangedSame = IsBidBookChanged == other.IsBidBookChanged;
                askBookChangedSame = IsAskBookChanged == other.IsAskBookChanged;
            }

            var allSame = layerTypesSame && isLadderSame && maxDepthSame && dailyTickCountSame && sourceOpenInterestSame
                       && adapterOpenInterestSame && bidBooksSame && askBookSame && bidBookChangedSame && askBookChangedSame;

            return allSame;
        }

        public override void StateReset()
        {
            LayersOfType        = LayerType.PriceVolume;
            MaxPublishDepth     = SourceTickerInfo.DefaultMaximumPublishedLayers;
            IsLadder            = false;
            SourceOpenInterest  = null;
            AdapterOpenInterest = new OpenInterest(MarketDataSource.Adapter, 0m, 0m);
            AdapterOpenInterest = new OpenInterest(MarketDataSource.Adapter, 0m, 0m);

            BidSide.StateReset();
            AskSide.StateReset();

            base.StateReset();
        }

        public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
        {
            foreach (var pqFieldStringUpdate in BidSide.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
            foreach (var pqFieldStringUpdate in AskSide.GetStringUpdates(snapShotTime, messageFlags))
            {
                var updatedStringUpdate = pqFieldStringUpdate.SetIsAsk();
                yield return updatedStringUpdate;
            }
        }

        public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
        {
            if (stringUpdate.Field.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand) return NameIdLookup.UpdateFieldString(stringUpdate);
            return false;
        }

        private IPQNameIdLookupGenerator SourceOtherExistingOrNewPQNameIdNameLookup(IOrderBook? source)
        {
            IPQNameIdLookupGenerator thisLayDict;
            if (source is IPQOrderBook pqOrderBook)
                thisLayDict = InitializeNewIdLookupGenerator(pqOrderBook.NameIdLookup);
            else
                thisLayDict = InitializeNewIdLookupGenerator(null);

            return thisLayDict;
        }

        private IPQNameIdLookupGenerator SourceOtherExistingOrNewPQNameIdNameLookup(IEnumerable<IPriceVolumeLayer>? source)
        {
            IPQNameIdLookupGenerator thisLayDict;
            if (source is IPQOrderBookSide { NameIdLookup: not null } pqOrderBook)
                thisLayDict = InitializeNewIdLookupGenerator(pqOrderBook.NameIdLookup);
            else
                thisLayDict = InitializeNewIdLookupGenerator
                    (
                     source?.OfType<ISupportsPQNameIdLookupGenerator>()
                           ?.Where(pvl => pvl is { NameIdLookup: not null })
                           ?.Select(pvl => pvl.NameIdLookup)
                           ?.FirstOrDefault());

            return thisLayDict;
        }

        public IPQNameIdLookupGenerator InitializeNewIdLookupGenerator(IPQNameIdLookupGenerator? optionalExisting = null)
        {
            IPQNameIdLookupGenerator thisBookNameIdLookupGenerator = optionalExisting != null
                ? new PQNameIdLookupGenerator(optionalExisting, PQQuoteFields.LayerNameDictionaryUpsertCommand)
                : new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
            return thisBookNameIdLookupGenerator;
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBook, true);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = new HashCode();
                hashCode.Add(LayersOfType);
                hashCode.Add(IsLadder);
                hashCode.Add(MaxPublishDepth);
                hashCode.Add(IsBidBookChanged);
                hashCode.Add(IsAskBookChanged);
                hashCode.Add(DailyTickUpdateCount);
                hashCode.Add(SourceOpenInterest ?? default);
                hashCode.Add(AdapterOpenInterest);
                hashCode.Add(AskSide);
                hashCode.Add(BidSide);

                return hashCode.ToHashCode();
            }
        }

        public override string ToString() => $"{GetType().Name}({PQOrderBookToStringMembers})";
    }
}
