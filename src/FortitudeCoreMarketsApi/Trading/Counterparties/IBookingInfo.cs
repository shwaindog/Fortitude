using FortitudeCommon.Types.Mutable;

namespace FortitudeMarketsApi.Trading.Counterparties
{
    public interface IBookingInfo
    {
        IMutableString Portfolio { get; set; }
        IMutableString SubPortfolio { get; set; }
        IBookingInfo Clone();
    }
}