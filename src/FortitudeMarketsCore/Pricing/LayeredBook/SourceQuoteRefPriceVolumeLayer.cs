using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;

namespace FortitudeMarketsCore.Pricing.LayeredBook
{
    public class SourceQuoteRefPriceVolumeLayer : SourcePriceVolumeLayer, IMutableSourceQuoteRefPriceVolumeLayer
    {
        public SourceQuoteRefPriceVolumeLayer(decimal price = 0m, decimal volume = 0m, 
            string sourceName = null, bool executable = false, uint quoteReference = 0u) 
            : base(price, volume, sourceName, executable)
        {
            SourceQuoteReference = quoteReference;
        }

        public SourceQuoteRefPriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
        {
            if (toClone is ISourceQuoteRefPriceVolumeLayer srcQtRefPvLayer)
            {
                SourceQuoteReference = srcQtRefPvLayer.SourceQuoteReference;
            }
        }
        
        public uint SourceQuoteReference { get; set; }
        public override bool IsEmpty => base.IsEmpty && SourceQuoteReference == 0u;
        
        public override void Reset()
        {
            base.Reset();
            SourceQuoteReference = 0;
        }

        public override void CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            base.CopyFrom(source, copyMergeFlags);
            if (source is ISourceQuoteRefPriceVolumeLayer sourceSourcePriceVolumeLayer)
            {
                SourceQuoteReference = sourceSourcePriceVolumeLayer.SourceQuoteReference;
            }
        }

        IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone()
        {
            return (IMutableSourceQuoteRefPriceVolumeLayer)Clone();
        }

        IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone()
        {
            return (IMutableSourceQuoteRefPriceVolumeLayer)Clone();
        }
        
        ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone()
        {
            return (ISourceQuoteRefPriceVolumeLayer)Clone();
        }

        ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone()
        {
            return (ISourceQuoteRefPriceVolumeLayer)Clone();
        }

        public override IPriceVolumeLayer Clone()
        {
            return new SourceQuoteRefPriceVolumeLayer(this);
        }

        public override bool AreEquivalent(IPriceVolumeLayer other, bool exactTypes = false)
        {
            if (!(other is ISourceQuoteRefPriceVolumeLayer otherISourcePvLayer)) return false;
            var baseSame = base.AreEquivalent(otherISourcePvLayer, exactTypes);
            var quoteRefSame = SourceQuoteReference == otherISourcePvLayer.SourceQuoteReference;

            return baseSame && quoteRefSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer) obj, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (int)SourceQuoteReference;
            }
        }

        public override string ToString()
        {
            return $"SourceQuoteRefPriceVolumeLayer{{{nameof(Price)}: {Price:N5}, {nameof(Volume)}: " +
                   $"{Volume:N2} , {nameof(SourceName)}: {SourceName}, " +
                   $"{nameof(Executable)}: {Executable}, " +
                   $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0} }}";
        }
    }
}