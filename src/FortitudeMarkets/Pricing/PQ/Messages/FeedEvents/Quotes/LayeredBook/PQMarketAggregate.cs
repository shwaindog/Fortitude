using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook
{
    public interface IPQMarketAggregate : IMutableMarketAggregate, IPQSupportsNumberPrecisionFieldUpdates<IPQMarketAggregate>
      , ICloneable<IPQMarketAggregate>, ITrackableReset<IPQMarketAggregate>
    {
        bool IsDataSourceUpdated         { get; set; }
        bool IsVolumeUpdated             { get; set; }
        bool IsVwapUpdated               { get; set; }
        bool IsUpdatedDateUpdated        { get; set; }
        bool IsUpdatedSub2MinTimeUpdated { get; set; }

        new IPQMarketAggregate Clone();
        new IPQMarketAggregate ResetWithTracking();
    }

    [Flags]
    public enum PQMarketAggregateUpdatedFlags : byte
    {
        None                        = 0
      , IsDataSourceUpdated         = 0x01
      , IsUpdatedDateUpdated        = 0x02
      , IsUpdatedSub2MinTimeUpdated = 0x04
      , IsVolumeUpdated             = 0x08
      , IsVwapUpdated               = 0x10
    }

    public class PQMarketAggregate : ReusableObject<IMarketAggregate>, IPQMarketAggregate
    {
        protected PQMarketAggregateUpdatedFlags UpdatedFlags;

        protected uint SequenceId = uint.MaxValue;

        private MarketDataSource dataSource = MarketDataSource.None;

        private DateTime updateTime = DateTime.MinValue;
        private decimal  volume;
        private decimal  vwap;

        public PQMarketAggregate()
        {
            if (GetType() == typeof(PQMarketAggregate)) SequenceId = 0;
        }

        public PQMarketAggregate(MarketDataSource dataSource, decimal volume, decimal vwap, DateTime? updateTime = null)
        {
            DataSource = dataSource;
            UpdateTime = updateTime ?? DateTime.MinValue;
            Volume     = volume;
            Vwap       = vwap;

            if (GetType() == typeof(PQMarketAggregate)) SequenceId = 0;
        }

        public PQMarketAggregate(IMarketAggregate toClone)
        {
            DataSource = toClone.DataSource;
            UpdateTime = toClone.UpdateTime;
            Volume     = toClone.Volume;
            Vwap       = toClone.Vwap;

            SetFlagsSame(toClone);

            if (GetType() == typeof(PQMarketAggregate)) SequenceId = 0;
        }

        public MarketDataSource DataSource
        {
            get => dataSource;
            set
            {
                IsDataSourceUpdated |= value != dataSource || SequenceId == 0;

                dataSource = value;
            }
        }

        public DateTime UpdateTime
        {
            get => updateTime;
            set
            {
                IsUpdatedDateUpdated |= updateTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() ||
                                        SequenceId == 0;
                IsUpdatedSub2MinTimeUpdated |= updateTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
                updateTime                  =  value;
                if(updateTime == DateTime.UnixEpoch) updateTime = DateTime.MinValue;
            }
        }

        public decimal Volume
        {
            get => volume;
            set
            {
                IsVolumeUpdated |= value != volume || SequenceId == 0;

                volume = value;
            }
        }
        public decimal Vwap
        {
            get => vwap;
            set
            {
                IsVwapUpdated |= value != vwap || SequenceId == 0;

                vwap = value;
            }
        }

        public bool IsDataSourceUpdated
        {
            get => (UpdatedFlags & PQMarketAggregateUpdatedFlags.IsDataSourceUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQMarketAggregateUpdatedFlags.IsDataSourceUpdated;

                else if (IsDataSourceUpdated) UpdatedFlags ^= PQMarketAggregateUpdatedFlags.IsDataSourceUpdated;
            }
        }
        public bool IsVolumeUpdated
        {
            get => (UpdatedFlags & PQMarketAggregateUpdatedFlags.IsVolumeUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQMarketAggregateUpdatedFlags.IsVolumeUpdated;

                else if (IsVolumeUpdated) UpdatedFlags ^= PQMarketAggregateUpdatedFlags.IsVolumeUpdated;
            }
        }
        public bool IsVwapUpdated
        {
            get => (UpdatedFlags & PQMarketAggregateUpdatedFlags.IsVwapUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQMarketAggregateUpdatedFlags.IsVwapUpdated;

                else if (IsVwapUpdated) UpdatedFlags ^= PQMarketAggregateUpdatedFlags.IsVwapUpdated;
            }
        }
        public bool IsUpdatedDateUpdated
        {
            get => (UpdatedFlags & PQMarketAggregateUpdatedFlags.IsUpdatedDateUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQMarketAggregateUpdatedFlags.IsUpdatedDateUpdated;

                else if (IsUpdatedDateUpdated) UpdatedFlags ^= PQMarketAggregateUpdatedFlags.IsUpdatedDateUpdated;
            }
        }

        public bool IsUpdatedSub2MinTimeUpdated
        {
            get => (UpdatedFlags & PQMarketAggregateUpdatedFlags.IsUpdatedSub2MinTimeUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQMarketAggregateUpdatedFlags.IsUpdatedSub2MinTimeUpdated;

                else if (IsUpdatedSub2MinTimeUpdated) UpdatedFlags ^= PQMarketAggregateUpdatedFlags.IsUpdatedSub2MinTimeUpdated;
            }
        }

        public bool HasUpdates
        {
            get => UpdatedFlags != PQMarketAggregateUpdatedFlags.None;
            set
            {
                if (value) return;
                UpdatedFlags         = PQMarketAggregateUpdatedFlags.None;
                SequenceId = 0;
            }
        }

        public bool IsEmpty
        {
            get => DataSource == MarketDataSource.None && UpdateTime == DateTime.MinValue && Volume == 0m && Vwap == 0m;
            set
            {
                if (!value) return;
                DataSource = MarketDataSource.None;
                UpdateTime = DateTime.MinValue;
                Volume     = 0m;
                Vwap       = 0m;
            }
        }

        public uint UpdateSequenceId => SequenceId;

        public void UpdateStarted(uint updateSequenceId)
        {
            SequenceId = updateSequenceId;
        }

        public void UpdateComplete(uint updateSequenceId = 0)
        {
            if (HasUpdates && !IsEmpty)
            {
                SequenceId++;
            }
            HasUpdates = false;
        }

        public int UpdateField(PQFieldUpdate fieldUpdate)
        {
            switch (fieldUpdate.PricingSubId)
            {
                case PQPricingSubFieldKeys.MarketAggregateSource:
                    IsDataSourceUpdated = true;
                    DataSource          = (MarketDataSource)fieldUpdate.Payload;
                    return 0;
                case PQPricingSubFieldKeys.MarketAggregateUpdateDate:
                    IsUpdatedDateUpdated = true; // incase of reset and sending 0;
                    PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref updateTime, fieldUpdate.Payload);
                    if (updateTime == DateTime.UnixEpoch) updateTime = default;
                    return 0;
                case PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime:
                    IsUpdatedSub2MinTimeUpdated = true; // incase of reset and sending 0;
                    PQFieldConverters.UpdateSub2MinComponent
                        (ref updateTime, fieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(fieldUpdate.Payload));
                    if (updateTime == DateTime.UnixEpoch) updateTime = default;
                    return 0;
                case PQPricingSubFieldKeys.MarketAggregateVolume:
                    IsVolumeUpdated = true; // incase of reset and sending 0;
                    Volume          = PQScaling.Unscale(fieldUpdate.Payload, fieldUpdate.Flag);
                    return 0;
                case PQPricingSubFieldKeys.MarketAggregateVwap:
                    IsVwapUpdated = true; // incase of reset and sending 0;
                    Vwap          = PQScaling.Unscale(fieldUpdate.Payload, fieldUpdate.Flag);
                    return 0;
            }
            return -1;
        }

        public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags
          , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
        {
            var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
            if (!updatedOnly || IsDataSourceUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateSource, (uint)DataSource);

            if (!updatedOnly || IsUpdatedDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateUpdateDate
                                             , updateTime.Get2MinIntervalsFromUnixEpoch());
            if (!updatedOnly || IsUpdatedSub2MinTimeUpdated)
            {
                var extended = updateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
                yield return new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime, value, extended);
            }
            if (!updatedOnly || IsVolumeUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateVolume, Volume,
                                               quotePublicationPrecisionSettings?.VolumeScalingPrecision ?? (PQFieldFlags)6);

            if (!updatedOnly || IsVwapUpdated)
            {
                yield return new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateVwap, Vwap,
                                               quotePublicationPrecisionSettings?.PriceScalingPrecision ?? (PQFieldFlags)2);
            }
        }

        IMutableMarketAggregate ITrackableReset<IMutableMarketAggregate>.ResetWithTracking() => ResetWithTracking();

        IPQMarketAggregate ITrackableReset<IPQMarketAggregate>.ResetWithTracking() => ResetWithTracking();

        IPQMarketAggregate IPQMarketAggregate.                 ResetWithTracking() => ResetWithTracking();

        public PQMarketAggregate ResetWithTracking()
        {
            IsEmpty = true;
            return this;
        }

        public override void StateReset()
        {
            IsEmpty      = true;
            UpdatedFlags = PQMarketAggregateUpdatedFlags.None;
            base.StateReset();
        }

        IMutableMarketAggregate ICloneable<IMutableMarketAggregate>.Clone() => Clone();

        IMutableMarketAggregate IMutableMarketAggregate.Clone() => Clone();

        IPQMarketAggregate ICloneable<IPQMarketAggregate>.Clone() => Clone();

        IPQMarketAggregate IPQMarketAggregate.Clone() => Clone();

        public override PQMarketAggregate Clone() => Recycler?.Borrow<PQMarketAggregate>().CopyFrom(this) ?? new PQMarketAggregate(this);

        IPQMarketAggregate ITransferState<IPQMarketAggregate>.CopyFrom
            (IPQMarketAggregate source, CopyMergeFlags copyMergeFlags) =>
            CopyFrom(source, copyMergeFlags);


        public override PQMarketAggregate CopyFrom(IMarketAggregate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            if (source is IPQMarketAggregate pqSource)
            {
                var hasFullReplace = copyMergeFlags.HasFullReplace();

                if (pqSource.IsDataSourceUpdated || hasFullReplace)
                {
                    IsDataSourceUpdated = true;   // incase of reset and sending 0;
                    DataSource          = source.DataSource;
                }
                if (pqSource.IsUpdatedDateUpdated || hasFullReplace)
                {
                    IsUpdatedDateUpdated = true; // incase of reset and sending 0;
                    PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref updateTime, pqSource.UpdateTime.Get2MinIntervalsFromUnixEpoch());
                }
                if (pqSource.IsUpdatedSub2MinTimeUpdated || hasFullReplace)
                {
                    IsUpdatedSub2MinTimeUpdated = true; // incase of reset and sending 0;
                    var extended = pqSource.UpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
                    PQFieldConverters.UpdateSub2MinComponent
                        (ref updateTime, extended.AppendScaleFlagsToUintToMakeLong(value));
                }
                if (pqSource.IsVolumeUpdated || hasFullReplace)
                {
                    IsVolumeUpdated = true;
                    Volume         = source.Volume;
                }
                if (pqSource.IsVwapUpdated || hasFullReplace)
                {
                    IsVwapUpdated = true;
                    Vwap = source.Vwap;
                }
            }
            else
            {
                DataSource = source.DataSource;
                UpdateTime = source.UpdateTime;
                Volume     = source.Volume;
                Vwap       = source.Vwap;
            }

            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (isFullReplace)
            {
                SetFlagsSame(source);
            }
            return this;
        }

        public bool AreEquivalent(IMarketAggregate? other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;

            var dataSourceSame = DataSource == other.DataSource;
            var updateTimeSame = true;
            if (dataSourceSame && DataSource != MarketDataSource.Published)
            {
                updateTimeSame = UpdateTime == other.UpdateTime;
            }
            var volumeSame     = Volume == other.Volume;
            var vwapSame       = Vwap == other.Vwap;

            var flagsSame = true;
            if (exactTypes)
            {
                var pqOpenInterest = other as PQMarketAggregate;
                flagsSame = UpdatedFlags == pqOpenInterest?.UpdatedFlags;
            }

            var allSame = dataSourceSame && updateTimeSame && volumeSame && vwapSame && flagsSame;
            if (!allSame)
            {
                Console.Out.WriteLine("");
            }

            return allSame;
        }

        public bool AreEquivalent(IMutableMarketAggregate? other, bool exactTypes = false) => AreEquivalent((IMarketAggregate?)other, exactTypes);

        protected void SetFlagsSame(IMarketAggregate toCopyFlags)
        {
            if (toCopyFlags is PQMarketAggregate pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMarketAggregate, true);

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(DataSource);
            hashCode.Add(UpdateTime);
            hashCode.Add(Volume);
            hashCode.Add(Vwap);

            return hashCode.ToHashCode();
        }

        protected string PQOpenInterestToStringMembers =>
            $"{nameof(DataSource)}: {DataSource}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(Volume)}: {Volume:N2}, " +
            $"{nameof(Vwap)}: {Vwap:N5}, {nameof(UpdatedFlags)}: {UpdatedFlags}";

        public override string ToString() => $"{nameof(PQMarketAggregate)}{{{PQOpenInterestToStringMembers}}}";
    }
}
