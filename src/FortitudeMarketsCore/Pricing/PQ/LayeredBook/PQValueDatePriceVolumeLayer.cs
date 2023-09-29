using System;
using System.Collections.Generic;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook
{
    public class PQValueDatePriceVolumeLayer : PQPriceVolumeLayer, IPQValueDatePriceVolumeLayer
    {
        private DateTime valueDate = DateTimeConstants.UnixEpoch;

        public PQValueDatePriceVolumeLayer(decimal price = 0m, decimal volume = 0m, 
            DateTime? valueDate = null) 
            : base(price, volume)
        {
            ValueDate = valueDate ?? DateTimeConstants.UnixEpoch;
        }

        public PQValueDatePriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
        {
            if (toClone is IValueDatePriceVolumeLayer valueDateToClone)
            {
                ValueDate = valueDateToClone.ValueDate;
            }

            SetFlagsSame(toClone);
        }

        public DateTime ValueDate
        {
            get => valueDate;
            set
            {
                if (valueDate == value) return;
                IsValueDateUpdated = true;
                valueDate = value;
            }
        }

        public bool IsValueDateUpdated
        {
            get => (UpdatedFlags & LayerFieldUpdatedFlags.ValueDateUpdatedFlag) > 0;
            set
            {
                if (value)
                {
                    UpdatedFlags |= LayerFieldUpdatedFlags.ValueDateUpdatedFlag;
                }
                else if (IsValueDateUpdated)
                {
                    UpdatedFlags ^= LayerFieldUpdatedFlags.ValueDateUpdatedFlag;
                }
            }
        }

        public override bool IsEmpty => base.IsEmpty && ValueDate == DateTimeConstants.UnixEpoch;

        public override void Reset()
        {
            ValueDate = DateTimeConstants.UnixEpoch;
            base.Reset();
        }

        public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
            IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSetting = null)
        {
            var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
            foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
                quotePublicationPrecisionSetting))
            {
                yield return pqFieldUpdate;
            }
            if (!updatedOnly || IsValueDateUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.LayerDateOffset, valueDate.GetHoursFromUnixEpoch(),
                    PQFieldFlags.IsExtendedFieldId);
            }
        }

        public override int UpdateField(PQFieldUpdate pqFieldUpdate)
        {
            // assume the book has already forwarded this through to the correct layer
            if (pqFieldUpdate.Id < PQFieldKeys.LayerDateOffset || pqFieldUpdate.Id >=
                (PQFieldKeys.LayerDateOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth))
                return base.UpdateField(pqFieldUpdate);
            PQFieldConverters.UpdateHoursFromUnixEpoch(ref valueDate, pqFieldUpdate.Value);
            IsValueDateUpdated = true;
            return 0;
        }

        public override void CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            base.CopyFrom(source);
            var pqValueDate = source as IPQValueDatePriceVolumeLayer;
            if (source is IValueDatePriceVolumeLayer vlDtPvLayer && pqValueDate == null)
            {
                ValueDate = vlDtPvLayer.ValueDate;
            }
            else if (pqValueDate != null)
            {
                if (pqValueDate.IsValueDateUpdated)
                {
                    ValueDate = pqValueDate.ValueDate;
                }    
            }
        }

        IPQValueDatePriceVolumeLayer IPQValueDatePriceVolumeLayer.Clone()
        {
            return (IPQValueDatePriceVolumeLayer)Clone();
        }

        IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone()
        {
            return (IMutableValueDatePriceVolumeLayer)Clone();
        }

        IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone()
        {
            return (IMutableValueDatePriceVolumeLayer)Clone();
        }

        IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone()
        {
            return (IValueDatePriceVolumeLayer)Clone();
        }

        IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone()
        {
            return (IValueDatePriceVolumeLayer)Clone();
        }

        public override IPQPriceVolumeLayer Clone()
        {
            return new PQValueDatePriceVolumeLayer(this);
        }

        public override bool AreEquivalent(IPriceVolumeLayer other, bool exactTypes = false)
        {
            if (!(other is IValueDatePriceVolumeLayer valueDateOther)) return false;

            var baseSame = base.AreEquivalent(other, exactTypes);
            var valueDateSame = ValueDate == valueDateOther.ValueDate;

            return baseSame && valueDateSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer)obj, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ valueDate.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"PQValueDatePriceVolumeLayer {{ {nameof(Price)}: {Price:N5}, " +
                   $"{nameof(Volume)}: {Volume:N2}, {nameof(ValueDate)}: {ValueDate} }}";
        }
    }
}