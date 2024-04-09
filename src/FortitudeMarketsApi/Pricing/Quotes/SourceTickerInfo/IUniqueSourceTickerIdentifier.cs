#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

public interface IUniqueSourceTickerIdentifier : ICloneable<IUniqueSourceTickerIdentifier>,
    IInterfacesComparable<IUniqueSourceTickerIdentifier>
{
    uint Id { get; }
    ushort SourceId { get; }
    ushort TickerId { get; }
    string Source { get; }
    string Ticker { get; }
}

public interface IMutableUniqueSourceTickerIdentifier : IUniqueSourceTickerIdentifier
{
    new uint Id { get; set; }
    new string Source { get; set; }
    new string Ticker { get; set; }
}
