﻿#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarketsApi.Trading.Counterparties;

public interface IBookingInfo : IReusableObject<IBookingInfo>
{
    IMutableString? Portfolio { get; set; }
    IMutableString? SubPortfolio { get; set; }
}
