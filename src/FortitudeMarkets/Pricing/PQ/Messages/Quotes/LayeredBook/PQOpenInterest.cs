using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook
{
    public interface IPQOpenInterest : IMutableOpenInterest, IPQSupportsFieldUpdates<IPQOpenInterest>, ICloneable<IPQOpenInterest>
    {
        bool IsDataSourceUpdated         { get; set; }
        bool IsVolumeUpdated             { get; set; }
        bool IsVwapUpdated               { get; set; }
        bool IsUpdatedDateUpdated        { get; set; }
        bool IsUpdatedSub2MinTimeUpdated { get; set; }

        new IPQOpenInterest Clone();
    }

    [Flags]
    public enum PQOpenInterestUpdatedFlags : byte
    {
        None                        = 0
      , IsDataSourceUpdated         = 0x01
      , IsUpdatedDateUpdated        = 0x02
      , IsUpdatedSub2MinTimeUpdated = 0x04
      , IsVolumeUpdated             = 0x08
      , IsVwapUpdated               = 0x10
    }


    public class PQOpenInterest : ReusableObject<IOpenInterest>, IPQOpenInterest
    {
        protected PQOpenInterestUpdatedFlags UpdatedFlags;

        protected uint NumUpdatesSinceEmpty = uint.MaxValue;

        private MarketDataSource dataSource = MarketDataSource.None;

        private DateTime updateTime = DateTime.MinValue;
        private decimal  volume;
        private decimal  vwap;

        public PQOpenInterest()
        {
            if (GetType() == typeof(PQOpenInterest)) NumUpdatesSinceEmpty = 0;
        }

        public PQOpenInterest(MarketDataSource dataSource, decimal volume, decimal vwap, DateTime? updateTime = null)
        {
            DataSource = dataSource;
            UpdateTime = updateTime ?? DateTime.MinValue;
            Volume     = volume;
            Vwap       = vwap;

            if (GetType() == typeof(PQOpenInterest)) NumUpdatesSinceEmpty = 0;
        }

        public PQOpenInterest(IOpenInterest toClone)
        {
            DataSource = toClone.DataSource;
            UpdateTime = toClone.UpdateTime;
            Volume     = toClone.Volume;
            Vwap       = toClone.Vwap;

            SetFlagsSame(toClone);

            if (GetType() == typeof(PQOpenInterest)) NumUpdatesSinceEmpty = 0;
        }

        public MarketDataSource DataSource
        {
            get => dataSource;
            set
            {
                IsDataSourceUpdated |= value != dataSource || NumUpdatesSinceEmpty == 0;

                dataSource = value;
            }
        }

        public DateTime UpdateTime
        {
            get => updateTime;
            set
            {
                IsUpdatedDateUpdated |= updateTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() ||
                                        NumUpdatesSinceEmpty == 0;
                IsUpdatedSub2MinTimeUpdated |= updateTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
                updateTime                  =  value;
                if(updateTime == DateTime.UnixEpoch) updateTime = DateTime.MinValue;
            }
        }

        public decimal Volume
        {
            get => volume;
            set
            {
                IsVolumeUpdated |= value != volume || NumUpdatesSinceEmpty == 0;

                volume = value;
            }
        }
        public decimal Vwap
        {
            get => vwap;
            set
            {
                IsVwapUpdated |= value != vwap || NumUpdatesSinceEmpty == 0;

                vwap = value;
            }
        }

        public bool IsDataSourceUpdated
        {
            get => (UpdatedFlags & PQOpenInterestUpdatedFlags.IsDataSourceUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQOpenInterestUpdatedFlags.IsDataSourceUpdated;

                else if (IsDataSourceUpdated) UpdatedFlags ^= PQOpenInterestUpdatedFlags.IsDataSourceUpdated;
            }
        }
        public bool IsVolumeUpdated
        {
            get => (UpdatedFlags & PQOpenInterestUpdatedFlags.IsVolumeUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQOpenInterestUpdatedFlags.IsVolumeUpdated;

                else if (IsVolumeUpdated) UpdatedFlags ^= PQOpenInterestUpdatedFlags.IsVolumeUpdated;
            }
        }
        public bool IsVwapUpdated
        {
            get => (UpdatedFlags & PQOpenInterestUpdatedFlags.IsVwapUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQOpenInterestUpdatedFlags.IsVwapUpdated;

                else if (IsVwapUpdated) UpdatedFlags ^= PQOpenInterestUpdatedFlags.IsVwapUpdated;
            }
        }
        public bool IsUpdatedDateUpdated
        {
            get => (UpdatedFlags & PQOpenInterestUpdatedFlags.IsUpdatedDateUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQOpenInterestUpdatedFlags.IsUpdatedDateUpdated;

                else if (IsUpdatedDateUpdated) UpdatedFlags ^= PQOpenInterestUpdatedFlags.IsUpdatedDateUpdated;
            }
        }

        public bool IsUpdatedSub2MinTimeUpdated
        {
            get => (UpdatedFlags & PQOpenInterestUpdatedFlags.IsUpdatedSub2MinTimeUpdated) > 0;
            set
            {
                if (value)
                    UpdatedFlags |= PQOpenInterestUpdatedFlags.IsUpdatedSub2MinTimeUpdated;

                else if (IsUpdatedSub2MinTimeUpdated) UpdatedFlags ^= PQOpenInterestUpdatedFlags.IsUpdatedSub2MinTimeUpdated;
            }
        }

        public bool HasUpdates
        {
            get => UpdatedFlags != PQOpenInterestUpdatedFlags.None;
            set
            {
                if (value) return;
                UpdatedFlags         = PQOpenInterestUpdatedFlags.None;
                NumUpdatesSinceEmpty = 0;
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

        public uint UpdateCount => NumUpdatesSinceEmpty;

        public void UpdateComplete()
        {
            if (HasUpdates && !IsEmpty)
            {
                NumUpdatesSinceEmpty++;
            }
            HasUpdates = false;
        }

        public int UpdateField(PQFieldUpdate fieldUpdate)
        {
            switch (fieldUpdate.SubId)
            {
                case PQSubFieldKeys.OpenInterestSource:
                    IsDataSourceUpdated = true;
                    DataSource          = (MarketDataSource)fieldUpdate.Payload;
                    return 0;
                case PQSubFieldKeys.OpenInterestUpdateDate:
                    IsUpdatedDateUpdated = true; // incase of reset and sending 0;
                    PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref updateTime, fieldUpdate.Payload);
                    if (updateTime == DateTime.UnixEpoch) updateTime = default;
                    return 0;
                case PQSubFieldKeys.OpenInterestUpdateSub2MinTime:
                    IsUpdatedSub2MinTimeUpdated = true; // incase of reset and sending 0;
                    PQFieldConverters.UpdateSub2MinComponent
                        (ref updateTime, fieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(fieldUpdate.Payload));
                    if (updateTime == DateTime.UnixEpoch) updateTime = default;
                    return 0;
                case PQSubFieldKeys.OpenInterestVolume:
                    IsVolumeUpdated = true; // incase of reset and sending 0;
                    Volume          = PQScaling.Unscale(fieldUpdate.Payload, fieldUpdate.Flag);
                    return 0;
                case PQSubFieldKeys.OpenInterestVwap:
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
                yield return new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestSource, (uint)DataSource);

            if (!updatedOnly || IsUpdatedDateUpdated)
                yield return new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestUpdateDate
                                             , updateTime.Get2MinIntervalsFromUnixEpoch());
            if (!updatedOnly || IsUpdatedSub2MinTimeUpdated)
            {
                var extended = updateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
                yield return new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestUpdateSub2MinTime, value, extended);
            }
            if (!updatedOnly || IsVolumeUpdated)
                yield return new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestVolume, Volume,
                                               quotePublicationPrecisionSettings?.VolumeScalingPrecision ?? (PQFieldFlags)6);

            if (!updatedOnly || IsVwapUpdated)
            {
                yield return new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestVwap, Vwap,
                                               quotePublicationPrecisionSettings?.PriceScalingPrecision ?? (PQFieldFlags)2);
            }
        }

        public override void StateReset()
        {
            IsEmpty      = true;
            UpdatedFlags = PQOpenInterestUpdatedFlags.None;
            base.StateReset();
        }

        IMutableOpenInterest ICloneable<IMutableOpenInterest>.Clone() => Clone();

        IMutableOpenInterest IMutableOpenInterest.Clone() => Clone();

        IPQOpenInterest ICloneable<IPQOpenInterest>.Clone() => Clone();

        IPQOpenInterest IPQOpenInterest.Clone() => Clone();

        public override PQOpenInterest Clone() => Recycler?.Borrow<PQOpenInterest>().CopyFrom(this) ?? new PQOpenInterest(this);

        IPQOpenInterest ITransferState<IPQOpenInterest>.CopyFrom
            (IPQOpenInterest source, CopyMergeFlags copyMergeFlags) =>
            CopyFrom(source, copyMergeFlags);


        public override PQOpenInterest CopyFrom(IOpenInterest source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            if (source is IPQOpenInterest pqSource)
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

        public bool AreEquivalent(IOpenInterest? other, bool exactTypes = false)
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
                var pqOpenInterest = other as PQOpenInterest;
                flagsSame = UpdatedFlags == pqOpenInterest?.UpdatedFlags;
            }

            var allSame = dataSourceSame && updateTimeSame && volumeSame && vwapSame && flagsSame;
            if (!allSame)
            {
                Console.Out.WriteLine("");
            }

            return allSame;
        }

        public bool AreEquivalent(IMutableOpenInterest? other, bool exactTypes = false) => AreEquivalent((IOpenInterest?)other, exactTypes);

        protected void SetFlagsSame(IOpenInterest toCopyFlags)
        {
            if (toCopyFlags is PQOpenInterest pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOpenInterest, true);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = new HashCode();
                hashCode.Add(DataSource);
                hashCode.Add(UpdateTime);
                hashCode.Add(Volume);
                hashCode.Add(Vwap);

                return hashCode.ToHashCode();
            }
        }

        protected string PQOpenInterestToStringMembers =>
            $"{nameof(DataSource)}: {DataSource}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(Volume)}: {Volume:N2}, " +
            $"{nameof(Vwap)}: {Vwap:N5}, {nameof(UpdatedFlags)}: {UpdatedFlags}";

        public override string ToString() => $"{nameof(PQOpenInterest)}{{{PQOpenInterestToStringMembers}}}";
    }
}
