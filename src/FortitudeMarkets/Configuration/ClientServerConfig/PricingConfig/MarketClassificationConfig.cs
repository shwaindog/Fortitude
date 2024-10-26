// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;

public interface IMarketClassificationConfig : IInterfacesComparable<IMarketClassificationConfig>
{
    MarketClassification MarketClassification { get; set; }

    MarketType   MarketType   { get; set; }
    MarketRegion MarketRegion { get; set; }
    ProductType  ProductType  { get; set; }
}

public class MarketClassificationConfig : ConfigSection, IMarketClassificationConfig
{
    public MarketClassificationConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public MarketClassificationConfig() { }


    public MarketClassificationConfig(IMarketClassificationConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        MarketType   = toClone.MarketType;
        MarketRegion = toClone.MarketRegion;
        ProductType  = toClone.ProductType;
    }

    public MarketClassificationConfig(IMarketClassificationConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public MarketClassificationConfig
        (MarketClassification marketClassification) : this(InMemoryConfigRoot, InMemoryPath) =>
        MarketClassification = marketClassification;

    public MarketClassificationConfig
        (MarketClassification marketClassification, IConfigurationRoot root, string path) : base(root, path) =>
        MarketClassification = marketClassification;

    public MarketClassification MarketClassification
    {
        get => new(MarketType, ProductType, MarketRegion);
        set
        {
            MarketType   = value.MarketType;
            MarketRegion = value.MarketRegion;
            ProductType  = value.ProductType;
        }
    }

    public MarketType MarketType
    {
        get
        {
            var checkValue = this[nameof(MarketType)];
            return checkValue != null ? Enum.Parse<MarketType>(checkValue) : MarketType.Unknown;
        }
        set => this[nameof(MarketType)] = value.ToString();
    }

    public MarketRegion MarketRegion
    {
        get
        {
            var checkValue = this[nameof(MarketRegion)];
            return checkValue != null ? Enum.Parse<MarketRegion>(checkValue) : MarketRegion.Unknown;
        }
        set => this[nameof(MarketRegion)] = value.ToString();
    }

    public ProductType ProductType
    {
        get
        {
            var checkValue = this[nameof(ProductType)];
            return checkValue != null ? Enum.Parse<ProductType>(checkValue) : ProductType.Unknown;
        }
        set => this[nameof(ProductType)] = value.ToString();
    }

    public bool AreEquivalent
        (IMarketClassificationConfig? other, bool exactTypes = false) =>
        Equals(MarketClassification, other?.MarketClassification);


    protected bool Equals(MarketClassificationConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MarketClassificationConfig)obj);
    }

    public override int GetHashCode() => MarketClassification.GetHashCode();

    public override string ToString() =>
        $"{nameof(MarketClassificationConfig)}({nameof(MarketType)}: {MarketType}, " +
        $"{nameof(MarketRegion)}: {MarketRegion}, {nameof(ProductType)}: {ProductType})";
}
