#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Counterparties;

#endregion

namespace FortitudeMarketsCore.Trading.Counterparties;

public class BookingInfo : IBookingInfo
{
    public BookingInfo(IBookingInfo toClone)
    {
        Portfolio = toClone.Portfolio;
        SubPortfolio = toClone.SubPortfolio;
    }

    public BookingInfo(string portfolio, string subPortfolio)
        : this((MutableString)portfolio, (MutableString)subPortfolio) { }

    public BookingInfo(IMutableString portfolio, IMutableString subPortfolio)
    {
        Portfolio = portfolio;
        SubPortfolio = subPortfolio;
    }

    public IMutableString? Portfolio { get; set; }
    public IMutableString? SubPortfolio { get; set; }

    public IBookingInfo Clone() => new BookingInfo(this);
}
