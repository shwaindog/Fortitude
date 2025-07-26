#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public interface IBookingInfo : IReusableObject<IBookingInfo>
{
    IMutableString? Portfolio { get; set; }
    IMutableString? SubPortfolio { get; set; }
}
