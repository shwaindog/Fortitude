// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public class BookingInfo : ReusableObject<IBookingInfo>, IBookingInfo
{
    public BookingInfo() { }

    public BookingInfo(IBookingInfo toClone)
    {
        Portfolio    = toClone.Portfolio;
        SubPortfolio = toClone.SubPortfolio;
    }

    public BookingInfo(string portfolio, string subPortfolio)
        : this((MutableString)portfolio, (MutableString)subPortfolio) { }

    public BookingInfo(IMutableString portfolio, IMutableString subPortfolio)
    {
        Portfolio    = portfolio;
        SubPortfolio = subPortfolio;
    }

    public IMutableString? Portfolio { get; set; }

    public IMutableString? SubPortfolio { get; set; }

    public override IBookingInfo Clone() => Recycler?.Borrow<BookingInfo>().CopyFrom(this) ?? new BookingInfo(this);

    public override IBookingInfo CopyFrom(IBookingInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Portfolio    = source.Portfolio?.CopyOrClone(Portfolio as MutableString);
        SubPortfolio = source.SubPortfolio?.CopyOrClone(Portfolio as MutableString);
        return this;
    }
}
