// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Config;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IBookingAccountDistributionConfig : IInterfacesComparable<IBookingAccountDistributionConfig>
  , ICloneable<IBookingAccountDistributionConfig>
{
    decimal BookingWeight { get; set; }

    uint PortfolioId { get; set; }
}

public class BookingAccountDistributionConfig : ConfigSection, IBookingAccountDistributionConfig
{
    public BookingAccountDistributionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public BookingAccountDistributionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public BookingAccountDistributionConfig(uint portfolioId, decimal bookingWeight)
        : this(InMemoryConfigRoot, InMemoryPath, portfolioId, bookingWeight) { }

    public BookingAccountDistributionConfig
        (IConfigurationRoot root, string path, uint portfolioId, decimal bookingWeight) : this(root, path)
    {
        BookingWeight = bookingWeight;
        PortfolioId   = portfolioId;
    }

    public BookingAccountDistributionConfig(IBookingAccountDistributionConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        BookingWeight = toClone.BookingWeight;
        PortfolioId   = toClone.PortfolioId;
    }

    public BookingAccountDistributionConfig(IBookingAccountDistributionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public decimal BookingWeight
    {
        get => decimal.TryParse(this[nameof(BookingWeight)], out var fixedTickerOpenPosition) ? fixedTickerOpenPosition : 1m;
        set => this[nameof(BookingWeight)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public uint PortfolioId
    {
        get => uint.TryParse(this[nameof(PortfolioId)], out var fixedTickerOpeningOrderSize) ? fixedTickerOpeningOrderSize : 0u;
        set => this[nameof(PortfolioId)] = value.ToString();
    }


    object ICloneable.Clone() => Clone();

    IBookingAccountDistributionConfig ICloneable<IBookingAccountDistributionConfig>.Clone() => Clone();

    public virtual BookingAccountDistributionConfig Clone() => new(this);

    public virtual bool AreEquivalent(IBookingAccountDistributionConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var bookingWeightSame               = BookingWeight == other.BookingWeight;
        var portfolioIdSame                 = PortfolioId == other.PortfolioId;

        var allAreSame = bookingWeightSame && portfolioIdSame;

        return allAreSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(BookingWeight)}"]                   = null;
        root[$"{path}{Split}{nameof(PortfolioId)}"]                     = null;
    }

    protected bool Equals(IBookingAccountDistributionConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IBookingAccountDistributionConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = BookingWeight.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)PortfolioId;
            return hashCode;
        }
    }

    public string BookingAccountDistributionConfigToStringMembers =>
        $"{nameof(BookingWeight)}: {BookingWeight}, {nameof(PortfolioId)}: {PortfolioId}";

    public override string ToString() => $"{nameof(BookingAccountDistributionConfig)}{{{BookingAccountDistributionConfigToStringMembers}}}";
}
