﻿using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook
{
    [Flags]
    public enum OrderBookUpdatedFlags : byte
    {
        None                    = 0
      , IsDailyTickCountUpdated = 1
    }

    public interface IPQOrderBook : IMutableOrderBook, IPQSupportsNumberPrecisionFieldUpdates<IOrderBook>, IPQSupportsStringUpdates<IOrderBook>
      , IRelatedItems<ISourceTickerInfo, IOrderBook>, ICloneable<IPQOrderBook>
      , ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQOrderBook>
    {
        new IPQOrderBookSide BidSide { get; set; }
        new IPQOrderBookSide AskSide { get; set; }

        new bool HasUpdates { get; set; }

        new IPQNameIdLookupGenerator NameIdLookup { get; set; }

        bool IsDailyTickUpdateCountUpdated { get; set; }

        new IPQMarketAggregate? OpenInterest { get; set; }

        new IPQOrderBook Clone();
        new IPQOrderBook ResetWithTracking();
    }

    public class PQOrderBook : ReusableObject<IOrderBook>, IPQOrderBook
    {
        private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrderBook));

        private IPQNameIdLookupGenerator nameIdLookupGenerator;

        protected uint SequenceId = uint.MaxValue;

        private IPQMarketAggregate? pqOpenInterest = null;

        protected OrderBookUpdatedFlags UpdatedFlags;

        private ushort     maxPublishDepth;
        private LayerFlags layerFlags;
        private uint       dailyTickUpdateCount;

        public PQOrderBook() : this(LayerType.PriceVolume)
        {
            if (GetType() == typeof(PQOrderBook)) SequenceId = 0;
        }

        public PQOrderBook
        (LayerType layerType = LayerType.PriceVolume,
            ushort numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers,
            bool isLadder = false)
        {
            layerFlags =  layerType.SupportedLayerFlags();
            layerFlags |= LayersSupportedType.SupportedLayerFlags();
            layerFlags |= isLadder ? LayerFlags.Ladder : LayerFlags.None;

            MaxAllowedSize = numBookLayers;

            nameIdLookupGenerator = SourceOtherExistingOrNewPQNameIdNameLookup(this);

            AskSide = new PQOrderBookSide(BookSide.AskBook, layerType, numBookLayers, isLadder, nameIdLookupGenerator);
            BidSide = new PQOrderBookSide(BookSide.BidBook, layerType, numBookLayers, isLadder, nameIdLookupGenerator);

            if (GetType() == typeof(PQOrderBook)) SequenceId = 0;
        }

        public PQOrderBook(ISourceTickerInfo srcTickerInfo)
        {
            layerFlags =  srcTickerInfo.LayerFlags;
            layerFlags |= LayersSupportedType.SupportedLayerFlags();

            MaxAllowedSize = srcTickerInfo.MaximumPublishedLayers;
            nameIdLookupGenerator ??= srcTickerInfo is IPQSourceTickerInfo pqSrcTkrInfo
                ? InitializeNewIdLookupGenerator(pqSrcTkrInfo.NameIdLookup)
                : InitializeNewIdLookupGenerator();

            AskSide = new PQOrderBookSide(BookSide.AskBook, srcTickerInfo, nameIdLookupGenerator);
            BidSide = new PQOrderBookSide(BookSide.BidBook, srcTickerInfo, nameIdLookupGenerator);

            if (GetType() == typeof(PQOrderBook)) SequenceId = 0;
        }

        public PQOrderBook
            (IEnumerable<IPriceVolumeLayer>? bidBookLayers = null, IEnumerable<IPriceVolumeLayer>? askBookLayers = null, bool isLadder = false)
        {
            layerFlags |= (bidBookLayers ?? []).Concat(askBookLayers ?? []).FirstOrDefault()?.SupportsLayerFlags ??
                          LayerFlagsExtensions.PriceVolumeLayerFlags;
            layerFlags |= LayersSupportedType.SupportedLayerFlags();
            layerFlags |= isLadder ? LayerFlags.Ladder : LayerFlags.None;

            nameIdLookupGenerator = SourceOtherExistingOrNewPQNameIdNameLookup(bidBookLayers);

            AskSide = new PQOrderBookSide(BookSide.AskBook, askBookLayers, IsLadder, nameIdLookupGenerator);
            BidSide = new PQOrderBookSide(BookSide.BidBook, bidBookLayers, IsLadder, nameIdLookupGenerator);

            if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
        }

        public PQOrderBook(IOrderBookSide bidSide, IOrderBookSide askBookSide, uint dailyTickCount = 0, bool isLadder = false)
        {
            DailyTickUpdateCount = dailyTickCount;
            nameIdLookupGenerator ??= bidSide is IPQOrderBookSide pqBookSide
                ? InitializeNewIdLookupGenerator(pqBookSide.NameIdLookup)
                : InitializeNewIdLookupGenerator();
            if (bidSide is IPQOrderBookSide pqOrderBookBid)
            {
                BidSide = pqOrderBookBid;
            }
            else
            {
                BidSide = new PQOrderBookSide(bidSide, nameIdLookupGenerator);
            }
            if (askBookSide is IPQOrderBookSide pqOrderBookAsk)
            {
                nameIdLookupGenerator.CopyFrom(pqOrderBookAsk.NameIdLookup);
                AskSide = pqOrderBookAsk;
            }
            else
            {
                AskSide = new PQOrderBookSide(askBookSide, nameIdLookupGenerator);
            }
            BidSide.NameIdLookup = nameIdLookupGenerator;
            AskSide.NameIdLookup = nameIdLookupGenerator;

            layerFlags =  bidSide.LayerSupportedFlags | askBookSide.LayerSupportedFlags;
            layerFlags |= LayersSupportedType.SupportedLayerFlags();
            layerFlags |= isLadder ? LayerFlags.Ladder : LayerFlags.None;

            MaxAllowedSize = Math.Max(((IOrderBookSide)BidSide).MaxAllowedSize, ((IOrderBookSide)AskSide).MaxAllowedSize);
        }

        public PQOrderBook(IOrderBook toClone)
        {
            layerFlags =  toClone.LayerSupportedFlags;
            layerFlags |= LayersSupportedType.SupportedLayerFlags();

            MaxAllowedSize      = toClone.MaxAllowedSize;
            DailyTickUpdateCount = toClone.DailyTickUpdateCount;

            nameIdLookupGenerator = SourceOtherExistingOrNewPQNameIdNameLookup(toClone);
            if (toClone.HasNonEmptyOpenInterest)
            {
                pqOpenInterest = new PQMarketAggregate(toClone.OpenInterest);
            }

            AskSide = new PQOrderBookSide(toClone.AskSide, nameIdLookupGenerator);
            BidSide = new PQOrderBookSide(toClone.BidSide, nameIdLookupGenerator);

            SetFlagsSame(toClone);

            if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
        }

        protected string PQOrderBookToStringMembers =>
            $"{nameof(LayersSupportedType)}: {LayersSupportedType}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
            $"{nameof(MaxAllowedSize)}, {MaxAllowedSize}, {nameof(OpenInterest)}: {OpenInterest}, " +
            $"{nameof(IsAskBookChanged)}: {IsAskBookChanged}, {nameof(IsBidBookChanged)}: {IsBidBookChanged}, {nameof(AskSide)}: {AskSide}, " +
            $"{nameof(BidSide)}: {BidSide}, {nameof(IsLadder)}: {IsLadder}";

        protected string JustOrderBookUpdatedFlags => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

        INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

        public LayerType LayersSupportedType
        {
            get => LayerSupportedFlags.MostCompactLayerType();
            set => LayerSupportedFlags = value.SupportedLayerFlags();
        }
        public LayerFlags LayerSupportedFlags
        {
            get => layerFlags;
            set
            {
                layerFlags = layerFlags.Unset(LayerFlags.Ladder) | value;

                AskSide.LayerSupportedFlags = layerFlags;
                BidSide.LayerSupportedFlags = layerFlags;
            }
        }

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

        public bool IsLadder
        {
            get => LayerSupportedFlags.HasLadder();
            set => LayerSupportedFlags = value ? LayerFlags.Ladder : LayerFlags.None;
        }

        public ushort MaxAllowedSize
        {
            get => maxPublishDepth;
            private set => maxPublishDepth = Math.Max((byte)1, Math.Min(value, PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth));
        }

        public decimal? MidPrice => (BidSide[0]?.Price ?? 0 + AskSide[0]?.Price ?? 0) / 2;

        public uint DailyTickUpdateCount
        {
            get => dailyTickUpdateCount;
            set
            {
                IsDailyTickUpdateCountUpdated |= value != dailyTickUpdateCount || SequenceId == 0;
                dailyTickUpdateCount          =  value;
            }
        }

        public bool IsDailyTickUpdateCountUpdated
        {
            get => (UpdatedFlags & OrderBookUpdatedFlags.IsDailyTickCountUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= OrderBookUpdatedFlags.IsDailyTickCountUpdated;

                else if (IsDailyTickUpdateCountUpdated) UpdatedFlags ^= OrderBookUpdatedFlags.IsDailyTickCountUpdated;
            }
        }

        public bool HasUpdates
        {
            get =>
                UpdatedFlags != OrderBookUpdatedFlags.None || BidSide.HasUpdates || AskSide.HasUpdates
             || (HasNonEmptyOpenInterest
              && pqOpenInterest is { DataSource: not (MarketDataSource.None or MarketDataSource.Published), HasUpdates: true });
            set
            {
                BidSide.HasUpdates = value;
                AskSide.HasUpdates = value;
                if (pqOpenInterest != null)
                {
                    pqOpenInterest.HasUpdates = value;
                }
                if (value) return;
                UpdatedFlags = OrderBookUpdatedFlags.None;
            }
        }

        public bool HasNonEmptyOpenInterest
        {
            get => pqOpenInterest is { IsEmpty: false };
            set
            {
                if (value) return;
                if (pqOpenInterest != null)
                {
                    pqOpenInterest.IsEmpty = true;
                }
            }
        }

        IMarketAggregate IOrderBook.OpenInterest => OpenInterest!;
        IMutableMarketAggregate? IMutableOrderBook.OpenInterest
        {
            get => OpenInterest;
            set => OpenInterest = (PQMarketAggregate?)value;
        }

        public IPQMarketAggregate? OpenInterest
        {
            get
            {
                if (HasNonEmptyOpenInterest && pqOpenInterest is not { DataSource: MarketDataSource.Published or MarketDataSource.None })
                    return pqOpenInterest;

                var bidVwapResult = BidSide.CalculateVwap();
                var askVwapResult = AskSide.CalculateVwap();

                var totalVolume = bidVwapResult.VolumeAchieved + askVwapResult.VolumeAchieved;
                var totalVolPrice = (bidVwapResult.VolumeAchieved * bidVwapResult.AchievedVwap) +
                                    (askVwapResult.VolumeAchieved * askVwapResult.AchievedVwap);

                var bidAskVwap = totalVolume > 0 ? totalVolPrice / totalVolume : 0m;

                pqOpenInterest ??= new PQMarketAggregate();

                pqOpenInterest.DataSource = MarketDataSource.Published;
                pqOpenInterest.UpdateTime = DateTime.Now;
                pqOpenInterest.Volume     = totalVolume;
                pqOpenInterest.Vwap       = bidAskVwap;
                return pqOpenInterest;
            }
            set
            {
                if (value != null)
                {
                    pqOpenInterest ??= new PQMarketAggregate();

                    pqOpenInterest.DataSource = value.DataSource;
                    pqOpenInterest.UpdateTime = value.UpdateTime;
                    pqOpenInterest.Volume     = value.Volume;
                    pqOpenInterest.Vwap       = value.Vwap;
                }
                else if (pqOpenInterest != null)
                {
                    pqOpenInterest.IsEmpty = true;
                }
            }
        }

        public uint UpdateSequenceId => SequenceId;

        public void UpdateStarted(uint updateSequenceId)
        {
            SequenceId = updateSequenceId;
            AskSide.UpdateStarted(updateSequenceId);
            BidSide.UpdateStarted(updateSequenceId);
            pqOpenInterest?.UpdateStarted(updateSequenceId);
        }

        public void UpdateComplete(uint updateSequenceId = 0)
        {
            AskSide.UpdateComplete(updateSequenceId);
            BidSide.UpdateComplete(updateSequenceId);
            NameIdLookup.UpdateComplete(updateSequenceId);
            pqOpenInterest?.UpdateComplete(updateSequenceId);
            if (HasUpdates)
            {
                SequenceId++;
                HasUpdates = false;
            }
        }

        public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags,
            IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
        {
            var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
            if (!updatedOnly || IsDailyTickUpdateCountUpdated)
            {
                yield return new PQFieldUpdate(PQFeedFields.QuoteDailyTotalTickCount, DailyTickUpdateCount);
            }
            if (pqOpenInterest != null)
            {
                foreach (var oiFields in pqOpenInterest.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
                    yield return oiFields.WithFieldId(PQFeedFields.QuoteOpenInterestTotal);
            }
            foreach (var bidFields in BidSide.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
                yield return bidFields;

            foreach (var askField in AskSide.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
                yield return askField.SetIsAsk();
        }

        public int UpdateField(PQFieldUpdate pqFieldUpdate)
        {
            if (pqFieldUpdate.Id == PQFeedFields.QuoteDailyTotalTickCount)
            {
                IsDailyTickUpdateCountUpdated = true;

                DailyTickUpdateCount = pqFieldUpdate.Payload;
                return 0;
            }
            if (pqFieldUpdate.Id == PQFeedFields.QuoteOpenInterestTotal)
            {
                pqOpenInterest ??= new PQMarketAggregate();
                return pqOpenInterest.UpdateField(pqFieldUpdate);
            }
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
            LayerSupportedFlags = source.LayerSupportedFlags;
            MaxAllowedSize     = source.MaxAllowedSize;
            if (source.HasNonEmptyOpenInterest)
            {
                pqOpenInterest ??= new PQMarketAggregate();
                pqOpenInterest.CopyFrom(source.OpenInterest, copyMergeFlags);
            }
            else if (pqOpenInterest != null)
            {
                pqOpenInterest.IsEmpty = true;
            }
            BidSide.CopyFrom(source.BidSide, copyMergeFlags.AddSkipReferenceLookups());
            AskSide.CopyFrom(source.AskSide, copyMergeFlags.AddSkipReferenceLookups());
            if (source is IPQOrderBook pqOrderBook)
            {
                NameIdLookup.CopyFrom(pqOrderBook.NameIdLookup, copyMergeFlags);

                var hasFullReplace = copyMergeFlags.HasFullReplace();

                if (pqOrderBook.IsDailyTickUpdateCountUpdated || hasFullReplace)
                {
                    IsDailyTickUpdateCountUpdated = true;
                    DailyTickUpdateCount          = pqOrderBook.DailyTickUpdateCount;
                }

                if (hasFullReplace) SetFlagsSame(pqOrderBook);
            }
            else
            {
                DailyTickUpdateCount = source.DailyTickUpdateCount;
            }
            return this;
        }

        public void EnsureRelatedItemsAreConfigured(INameIdLookupGenerator? otherNameIdLookupGenerator)
        {
            if (otherNameIdLookupGenerator != null && !ReferenceEquals(NameIdLookup, otherNameIdLookupGenerator))
            {
                NameIdLookup.CopyFrom(otherNameIdLookupGenerator);
                if (NameIdLookup.Count != otherNameIdLookupGenerator.Count)
                    NameIdLookup.CopyFrom(otherNameIdLookupGenerator, CopyMergeFlags.FullReplace);
            }
        }

        public void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? refSrcTickerInfo, IOrderBook? refOrderBook = null)
        {
            if (ReferenceEquals(refOrderBook, this)) return;
            LayerSupportedFlags = (LayerSupportedFlags.Unset(LayerFlags.Ladder) | refSrcTickerInfo?.LayerFlags ??
                                   refOrderBook?.LayerSupportedFlags ?? LayerSupportedFlags);
            layerFlags      |= LayersSupportedType.SupportedLayerFlags();
            MaxAllowedSize =  refSrcTickerInfo?.MaximumPublishedLayers ?? refOrderBook?.MaxAllowedSize ?? MaxAllowedSize;
            IsLadder        =  refSrcTickerInfo?.LayerFlags.HasLadder() ?? refOrderBook?.IsLadder ?? IsLadder;
            if (refOrderBook is IPQOrderBook pqOrderBook)
            {
                EnsureRelatedItemsAreConfigured(pqOrderBook.NameIdLookup);
            }
            BidSide.NameIdLookup = NameIdLookup;
            AskSide.NameIdLookup = NameIdLookup;

            BidSide.EnsureRelatedItemsAreConfigured(LayerSupportedFlags, MaxAllowedSize);
            AskSide.EnsureRelatedItemsAreConfigured(LayerSupportedFlags, MaxAllowedSize);
        }

        bool IInterfacesComparable<IMutableOrderBook>.AreEquivalent(IMutableOrderBook? other, bool exactTypes) => AreEquivalent(other, exactTypes);

        public virtual bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;

            var layerTypesSame     = LayersSupportedType == other.LayersSupportedType;
            var isLadderSame       = IsLadder == other.IsLadder;
            var maxDepthSame       = MaxAllowedSize == other.MaxAllowedSize;
            var dailyTickCountSame = DailyTickUpdateCount == other.DailyTickUpdateCount;
            var openInterestSame   = HasNonEmptyOpenInterest == other.HasNonEmptyOpenInterest;
            if (openInterestSame && other.HasNonEmptyOpenInterest && HasNonEmptyOpenInterest)
            {
                openInterestSame = pqOpenInterest?.AreEquivalent(other.OpenInterest, exactTypes) ?? false;
            }

            var bidBooksSame       = BidSide.AreEquivalent(other.BidSide, exactTypes);
            var askBookSame        = AskSide.AreEquivalent(other.AskSide, exactTypes);
            var bidBookChangedSame = true;
            var askBookChangedSame = true;
            if (exactTypes)
            {
                bidBookChangedSame = IsBidBookChanged == other.IsBidBookChanged;
                askBookChangedSame = IsAskBookChanged == other.IsAskBookChanged;
            }

            var allSame = layerTypesSame && isLadderSame && maxDepthSame && dailyTickCountSame && openInterestSame
                       && bidBooksSame && askBookSame && bidBookChangedSame && askBookChangedSame;
            return allSame;
        }

        IMutableOrderBook ITrackableReset<IMutableOrderBook>.ResetWithTracking() => ResetWithTracking();

        IPQOrderBook ITrackableReset<IPQOrderBook>.ResetWithTracking() => ResetWithTracking();

        IPQOrderBook IPQOrderBook.ResetWithTracking() => ResetWithTracking();

        public PQOrderBook ResetWithTracking()
        {
            pqOpenInterest?.ResetWithTracking();
            BidSide.ResetWithTracking();
            AskSide.ResetWithTracking();
            DailyTickUpdateCount = 0;

            return this;
        }

        public override void StateReset()
        {
            NameIdLookup.Clear();

            pqOpenInterest?.StateReset();

            BidSide.StateReset();
            AskSide.StateReset();
            DailyTickUpdateCount = 0;

            HasUpdates = false;

            UpdatedFlags = OrderBookUpdatedFlags.None;

            base.StateReset();
        }

        public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
        {
            // Both bid and ask books share the same NameIdLookup as do their layers or should do anyway
            return NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);
        }

        public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
        {
            // Both bid and ask books share the same NameIdLookup as do their layers or should do anyway
            if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
            return NameIdLookup.UpdateFieldString(stringUpdate);
        }

        private IPQNameIdLookupGenerator SourceOtherExistingOrNewPQNameIdNameLookup(IOrderBook? source)
        {
            IPQNameIdLookupGenerator thisLayDict;
            if (source is IPQOrderBook pqOrderBook)
                thisLayDict = InitializeNewIdLookupGenerator(pqOrderBook.NameIdLookup);
            else
                thisLayDict = InitializeNewIdLookupGenerator();

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
                           .Where(pvl => pvl is { NameIdLookup: not null })
                           .Select(pvl => pvl.NameIdLookup)
                           .FirstOrDefault());

            return thisLayDict;
        }

        public IPQNameIdLookupGenerator InitializeNewIdLookupGenerator(IPQNameIdLookupGenerator? optionalExisting = null)
        {
            IPQNameIdLookupGenerator thisBookNameIdLookupGenerator = optionalExisting != null
                ? new PQNameIdLookupGenerator(optionalExisting, PQFeedFields.QuoteLayerStringUpdates)
                : new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
            return thisBookNameIdLookupGenerator;
        }

        protected void SetFlagsSame(IOrderBook toCopyFlags)
        {
            if (toCopyFlags is PQOrderBook pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBook, true);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = new HashCode();
                hashCode.Add(LayersSupportedType);
                hashCode.Add(IsLadder);
                hashCode.Add(MaxAllowedSize);
                hashCode.Add(IsBidBookChanged);
                hashCode.Add(IsAskBookChanged);
                hashCode.Add(DailyTickUpdateCount);
                hashCode.Add(OpenInterest);
                hashCode.Add(AskSide);
                hashCode.Add(BidSide);

                return hashCode.ToHashCode();
            }
        }

        public override string ToString() => $"{GetType().Name}({PQOrderBookToStringMembers}, {JustOrderBookUpdatedFlags})";
    }
}
