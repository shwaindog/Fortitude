using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;

namespace FortitudeMarketsCore.Pricing.LayeredBook
{
    public class SourceQuoteRefTraderValueDatePriceVolumeLayer : TraderPriceVolumeLayer,
        IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
    {
        public SourceQuoteRefTraderValueDatePriceVolumeLayer(decimal price = 0m, decimal volume = 0m, 
            DateTime? valueDate = null, string sourceName = null, bool executable = false, 
            uint quoteRef = 0u) : base(price, volume)
        {
            SourceName = sourceName;
            Executable = executable;
            SourceQuoteReference = quoteRef;
            ValueDate = valueDate ?? DateTimeConstants.UnixEpoch;
        }

        public SourceQuoteRefTraderValueDatePriceVolumeLayer(IPriceVolumeLayer toClone) 
            : base(toClone)
        {
            if (toClone is ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer)
            {
                SourceName = srcQtRefTrdrVlDtPriceVolumeLayer.SourceName;
                Executable = srcQtRefTrdrVlDtPriceVolumeLayer.Executable;
                SourceQuoteReference = srcQtRefTrdrVlDtPriceVolumeLayer.SourceQuoteReference;
                ValueDate = srcQtRefTrdrVlDtPriceVolumeLayer.ValueDate;
            }
            else if (toClone is ISourceQuoteRefPriceVolumeLayer srcQtRefPriceVolumeLayer)
            {
                SourceName = srcQtRefPriceVolumeLayer.SourceName;
                Executable = srcQtRefPriceVolumeLayer.Executable;
                SourceQuoteReference = srcQtRefPriceVolumeLayer.SourceQuoteReference;
            }
            else if (toClone is ISourcePriceVolumeLayer srcPriceVolumeLayer)
            {
                SourceName = srcPriceVolumeLayer.SourceName;
                Executable = srcPriceVolumeLayer.Executable;
            }
            else if (toClone is IValueDatePriceVolumeLayer valueDatePvLayer)
            {
                ValueDate = valueDatePvLayer.ValueDate;
            }
        }

        public string SourceName { get; set; }
        public bool Executable { get; set; }
        public DateTime ValueDate { get; set; }
        public uint SourceQuoteReference { get; set; }

        public override bool IsEmpty => base.IsEmpty && SourceName == null && !Executable 
            && ValueDate == DateTimeConstants.UnixEpoch && SourceQuoteReference == 0;

        public override void Reset()
        {
            base.Reset();
            SourceName = null;
            Executable = false;
            ValueDate = DateTimeConstants.UnixEpoch;
            SourceQuoteReference = 0;
        }
        
        public override void CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            base.CopyFrom(source, copyMergeFlags);
            if (source is ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer)
            {
                SourceName = srcQtRefTrdrVlDtPriceVolumeLayer.SourceName;
                Executable = srcQtRefTrdrVlDtPriceVolumeLayer.Executable;
                SourceQuoteReference = srcQtRefTrdrVlDtPriceVolumeLayer.SourceQuoteReference;
                ValueDate = srcQtRefTrdrVlDtPriceVolumeLayer.ValueDate;
            } else if (source is ISourceQuoteRefPriceVolumeLayer srcQtRefPriceVolumeLayer)
            {
                SourceName = srcQtRefPriceVolumeLayer.SourceName;
                Executable = srcQtRefPriceVolumeLayer.Executable;
                SourceQuoteReference = srcQtRefPriceVolumeLayer.SourceQuoteReference;
            }
            else if (source is ISourcePriceVolumeLayer srcPriceVolumeLayer)
            {
                SourceName = srcPriceVolumeLayer.SourceName;
                Executable = srcPriceVolumeLayer.Executable;
            } else if (source is IValueDatePriceVolumeLayer valueDatePvLayer)
            {
                ValueDate = valueDatePvLayer.ValueDate;
            }
        }

        ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone()
        {
            return (ISourceQuoteRefPriceVolumeLayer)Clone();
        }

        IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone()
        {
            return (IMutableSourcePriceVolumeLayer)Clone();
        }

        ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone()
        {
            return (ISourceQuoteRefPriceVolumeLayer)Clone();
        }

        IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone()
        {
            return (IValueDatePriceVolumeLayer)Clone();
        }

        IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone()
        {
            return (IValueDatePriceVolumeLayer)Clone();
        }

        ISourceQuoteRefTraderValueDatePriceVolumeLayer ICloneable<ISourceQuoteRefTraderValueDatePriceVolumeLayer>.Clone()
        {
            return (ISourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();
        }

        ISourceQuoteRefTraderValueDatePriceVolumeLayer ISourceQuoteRefTraderValueDatePriceVolumeLayer.Clone()
        {
            return (ISourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();
        }

        IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone()
        {
            return (IMutableSourceQuoteRefPriceVolumeLayer)Clone();
        }

        IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone()
        {
            return (IMutableSourceQuoteRefPriceVolumeLayer)Clone();
        }

        IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone()
        {
            return (IMutableValueDatePriceVolumeLayer)Clone();
        }

        IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone()
        {
            return (IMutableValueDatePriceVolumeLayer)Clone();
        }

        IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer 
            ICloneable<IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer>.Clone()
        {
            return (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();
        }

        IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer.Clone()
        {
            return (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();
        }

        ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone()
        {
            return (ISourcePriceVolumeLayer)Clone();
        }

        ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone()
        {
            return (ISourcePriceVolumeLayer)Clone();
        }

        public override IPriceVolumeLayer Clone()
        {
            return new SourceQuoteRefTraderValueDatePriceVolumeLayer(this);
        }

        public override bool AreEquivalent(IPriceVolumeLayer other, bool exactTypes = false)
        {
            if (!(other is ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvLayer)) return false;

            var baseSame = base.AreEquivalent(other, exactTypes);
            var sourceNameSame = SourceName == srcQtRefTrdrVlDtPvLayer.SourceName;
            var executableSame = Executable == srcQtRefTrdrVlDtPvLayer.Executable;
            var sourceQuoteReferenceSame = SourceQuoteReference == srcQtRefTrdrVlDtPvLayer.SourceQuoteReference;
            var valueDateSame = ValueDate == srcQtRefTrdrVlDtPvLayer.ValueDate;

            return baseSame && sourceNameSame && executableSame && sourceQuoteReferenceSame && valueDateSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer) obj, true);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (SourceName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Executable.GetHashCode();
                hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) SourceQuoteReference;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"SourceQuoteRefTraderValueDatePriceVolumeLayer {{{nameof(Price)}: {Price:N5}, " +
                   $"{nameof(Volume)}: {Volume:N2}, {nameof(TraderDetails)}:[ " +
                   $"{string.Join(",", TraderDetails)}], {nameof(Count)}: {Count}, " + 
                   $"{ nameof(SourceName)}: {SourceName}, {nameof(Executable)}: {Executable}, " +
                   $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}, {nameof(ValueDate)}: " +
                   $"{ValueDate} }}";
        }
    }
}