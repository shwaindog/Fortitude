// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.PricingConfig;

public interface IMarketClassificationConfig : IInterfacesComparable<IMarketClassificationConfig>, IStringBearer
{
    MarketClassification MarketClassification { get; set; }

    AssetType     AssetType     { get; set; }
    AssetCategory AssetCategory { get; set; }
    MarketRegion  MarketRegion  { get; set; }
    ProductType   ProductType   { get; set; }
}

public class MarketClassificationConfig : ConfigSection, IMarketClassificationConfig
{
    public MarketClassificationConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MarketClassificationConfig() { }


    public MarketClassificationConfig(IMarketClassificationConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        AssetType   = toClone.AssetType;
        MarketRegion = toClone.MarketRegion;
        ProductType  = toClone.ProductType;
    }

    public MarketClassificationConfig(IMarketClassificationConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public MarketClassificationConfig(MarketClassification marketClassification) : this(InMemoryConfigRoot, InMemoryPath) =>
        MarketClassification = marketClassification;

    public MarketClassificationConfig
        (MarketClassification marketClassification, IConfigurationRoot root, string path) : base(root, path) =>
        MarketClassification = marketClassification;

    public MarketClassification MarketClassification
    {
        get => new(AssetType, AssetCategory, ProductType, MarketRegion);
        set
        {
            AssetType   = value.AssetType;
            AssetCategory = value.AssetCategory;
            MarketRegion = value.MarketRegion;
            ProductType  = value.ProductType;
        }
    }

    public AssetType AssetType
    {
        get
        {
            var checkValue = this[nameof(AssetType)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<AssetType>(checkValue!) : AssetType.Unknown;
        }
        set => this[nameof(AssetType)] = value.ToString();
    }

    public AssetCategory AssetCategory
    {
        get
        {
            var checkValue = this[nameof(AssetCategory)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<AssetCategory>(checkValue!) : AssetCategory.Unknown;
        }
        set => this[nameof(AssetCategory)] = value.ToString();
    }

    public MarketRegion MarketRegion
    {
        get
        {
            var checkValue = this[nameof(MarketRegion)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<MarketRegion>(checkValue!) : MarketRegion.Unknown;
        }
        set => this[nameof(MarketRegion)] = value.ToString();
    }

    public ProductType ProductType
    {
        get
        {
            var checkValue = this[nameof(ProductType)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<ProductType>(checkValue!) : ProductType.Unknown;
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(AssetType), AssetType)
            .Field.AlwaysAdd(nameof(AssetCategory), AssetCategory)
            .Field.AlwaysAdd(nameof(MarketRegion), MarketRegion)
            .Field.AlwaysAdd(nameof(ProductType), ProductType)
            .Complete();
    
    public override string ToString() =>
        $"{nameof(MarketClassificationConfig)}({nameof(AssetType)}: {AssetType}, {nameof(AssetCategory)}: {AssetCategory}, " +
        $"{nameof(MarketRegion)}: {MarketRegion}, {nameof(ProductType)}: {ProductType})";
}
