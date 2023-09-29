using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo
{
    public interface IPQUniqueSourceTickerIdentifier : IMutableUniqueSourceTickerIdentifier,
        IPQSupportsFieldUpdates<IUniqueSourceTickerIdentifier>, IPQSupportsStringUpdates<IUniqueSourceTickerIdentifier>
    {
        bool IsIdUpdated { get; set; }
        bool IsSourceUpdated { get; set; }
        bool IsTickerUpdated { get; set; }
    }
}