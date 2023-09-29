using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;

namespace FortitudeMarketsCore.Pricing.LayeredBook
{
    public class ValueDatePriceVolumeLayer : PriceVolumeLayer, IMutableValueDatePriceVolumeLayer
    {
        public ValueDatePriceVolumeLayer(decimal price = 0m, decimal volume = 0m, 
            DateTime? valueDate = null) : base(price, volume)
        {
            ValueDate = valueDate ?? DateTimeConstants.UnixEpoch;
        }

        public ValueDatePriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
        {
            if (toClone is IValueDatePriceVolumeLayer valueDatePriceVolumeLayer)
            {
                ValueDate = valueDatePriceVolumeLayer.ValueDate;
            }
            else
            {
                ValueDate = DateTimeConstants.UnixEpoch;
            }
        }

        public DateTime ValueDate { get; set; }
        public override bool IsEmpty => base.IsEmpty && ValueDate == DateTimeConstants.UnixEpoch;

        public override void Reset()
        {
            base.Reset();
            ValueDate = DateTimeConstants.UnixEpoch;
        }

        public override void CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            base.CopyFrom(source, copyMergeFlags);
            if (source is IValueDatePriceVolumeLayer sourceSourcePriceVolumeLayer)
            {
                ValueDate = sourceSourcePriceVolumeLayer.ValueDate;
            }
        }

        IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone()
        {
            return ((IValueDatePriceVolumeLayer)this).Clone();
        }

        IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone()
        {
            return (IValueDatePriceVolumeLayer)Clone();
        }

        IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone()
        {
            return (IMutableValueDatePriceVolumeLayer)Clone();
        }

        IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone()
        {
            return (IMutableValueDatePriceVolumeLayer)Clone();
        }

        public override IPriceVolumeLayer Clone()
        {
            return new ValueDatePriceVolumeLayer(this);
        }

        public override bool AreEquivalent(IPriceVolumeLayer other, bool exactTypes = false)
        {
            if (!(other is IValueDatePriceVolumeLayer otherValueDatePvLayer)) return false;
            var baseSame = base.AreEquivalent(otherValueDatePvLayer, exactTypes);
            var valueDateSame = Equals(ValueDate, otherValueDatePvLayer.ValueDate);

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
                return (base.GetHashCode() * 397) ^ ValueDate.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"ValueDatePriceVolumeLayer {{{nameof(Price)}: {Price:N5}, {nameof(Volume)}: " +
                   $"{Volume:N2} , {nameof(ValueDate)}: {ValueDate} }}";
        }
    }
}