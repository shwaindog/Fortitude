using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo
{
    public class UniqueSourceTickerIdentifier : IMutableUniqueSourceTickerIdentifier
    {
        public UniqueSourceTickerIdentifier(uint id, string source, string ticker)
        {
            Id = id;
            Source = source;
            Ticker = ticker;
        }

        public UniqueSourceTickerIdentifier(string source, string ticker, ushort sourceId, ushort tickerId)
        {
            Id = GenerateUniqueSourceTickerId(sourceId, tickerId);
            Source = source;
            Ticker = ticker;
        }

        public UniqueSourceTickerIdentifier(IUniqueSourceTickerIdentifier toClone)
        {
            Id = toClone.Id;
            Source = toClone.Source;
            Ticker = toClone.Ticker;
        }

        public uint Id { get; set; }
        public string Source { get; set; }
        public string Ticker { get; set; }

        public static uint GenerateUniqueSourceTickerId(ushort sourceId, ushort tickerId)
        {
            return ((uint)sourceId << 16) + tickerId;
        }

        public virtual object Clone()
        {
            return new UniqueSourceTickerIdentifier(this);
        }

        IUniqueSourceTickerIdentifier ICloneable<IUniqueSourceTickerIdentifier>.Clone()
        {
            return (IUniqueSourceTickerIdentifier) Clone();
        }

        public virtual bool AreEquivalent(IUniqueSourceTickerIdentifier other, bool exactTypes = false)
        {
            if(other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;
            var idSame = Id == other.Id;
            var sourceSame = string.Equals(Source, other.Source);
            var tickerSame = string.Equals(Ticker, other.Ticker);

            return idSame && sourceSame && tickerSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent(obj as IUniqueSourceTickerIdentifier, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Id;
                hashCode = (hashCode * 397) ^ (Source != null ? Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Ticker != null ? Ticker.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"UniqueSourceTickerIdentifier {{{nameof(Id)}: {Id}, {nameof(Source)}: {Source}, " +
                   $"{nameof(Ticker)}: {Ticker} }}";
        }
    }
}