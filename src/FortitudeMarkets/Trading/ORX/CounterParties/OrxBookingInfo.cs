// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Counterparties;

#endregion

namespace FortitudeMarkets.Trading.ORX.CounterParties;

public class OrxBookingInfo : ReusableObject<IBookingInfo>, IBookingInfo
{
    public OrxBookingInfo() { }

    public OrxBookingInfo(IBookingInfo toClone)
    {
        Portfolio    = toClone.Portfolio != null ? new MutableString(toClone.Portfolio) : null;
        SubPortfolio = toClone.SubPortfolio != null ? new MutableString(toClone.SubPortfolio) : null;
    }

    public OrxBookingInfo(string portfolio, string subPortfolio)
        : this((MutableString)portfolio, (MutableString)subPortfolio) { }

    public OrxBookingInfo(IMutableString portfolio, IMutableString subPortfolio)
    {
        Portfolio    = (MutableString)portfolio;
        SubPortfolio = (MutableString)subPortfolio;
    }

    [OrxMandatoryField(0)] public MutableString? Portfolio { get; set; }

    [OrxOptionalField(1)] public MutableString? SubPortfolio { get; set; }

    IMutableString? IBookingInfo.Portfolio
    {
        get => Portfolio;
        set => Portfolio = value as MutableString;
    }

    IMutableString? IBookingInfo.SubPortfolio
    {
        get => SubPortfolio;
        set => SubPortfolio = value as MutableString;
    }

    public override IBookingInfo Clone() => Recycler?.Borrow<OrxBookingInfo>().CopyFrom(this) ?? new OrxBookingInfo(this);

    public override IBookingInfo CopyFrom
    (IBookingInfo bookingInfo
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Portfolio    = bookingInfo.Portfolio.SyncOrRecycle(Portfolio);
        SubPortfolio = bookingInfo.SubPortfolio.SyncOrRecycle(SubPortfolio);
        return this;
    }

    protected bool Equals(OrxBookingInfo other)
    {
        var portfolioSame    = Equals(Portfolio, other.Portfolio);
        var subPortfolioSame = Equals(SubPortfolio, other.SubPortfolio);

        return portfolioSame && subPortfolioSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxBookingInfo)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Portfolio != null ? Portfolio.GetHashCode() : 0) * 397) ^
                   (SubPortfolio != null ? SubPortfolio.GetHashCode() : 0);
        }
    }
}
