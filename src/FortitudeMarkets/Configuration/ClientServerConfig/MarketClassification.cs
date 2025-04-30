// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeMarkets.Configuration.ClientServerConfig;

[JsonConverter(typeof(JsonStringEnumConverter<MarketRegion>))]
public enum MarketRegion : byte
{
    Unknown = 0
  , Any
  , Global
  , Local
  , US
  , EU
  , Japan
  , UK
  , Australia
  , Canada
  , NZ
  , China
  , SouthKorea
  , Singapore
  , Switzerland
  , India
  , Pakistan
  , Israel
  , Mexico
  , Brazil
  , Indonesia
  , SouthAfrica
  , Malaysia
  , Philippines
  , Argentina
}

[JsonConverter(typeof(JsonStringEnumConverter<MarketType>))]
public enum MarketType : byte
{
    Unknown = 0
  , Any
  , Internal
  , None
  , Other
  , FXMajor
  , FXEmerging
  , FXExotic
  , Rates
  , IndexMixed
  , EquityMajor
  , EquityMinor
  , EquityIndex
  , Reserved1
  , Reserved2
  , CommodityMetals
  , CommodityEnergy
  , CommodityAgriculture
  , Reserved3
  , Reserved4
  , BondGovernment
  , BondCorporate
  , Reserved5
  , Reserved6
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductType>))]
public enum ProductType : byte
{
    Unknown = 0
  , Any
  , Internal
  , None
  , Spot
  , Option
  , Forward
  , Swap
  , Future
  , FutureOption
}

public struct MarketClassification
{
    public uint CompoundedClassification;

    public MarketClassification() => CompoundedClassification = 0;
    public MarketClassification(uint compoundedClassification) => CompoundedClassification = compoundedClassification;

    public MarketClassification(MarketType marketType, ProductType productType, MarketRegion marketRegion = MarketRegion.Global) =>
        CompoundedClassification = ((uint)marketRegion << 16) | ((uint)productType << 8) | (uint)marketType;

    public MarketType   MarketType   => (MarketType)(CompoundedClassification & 0xFF);
    public ProductType  ProductType  => (ProductType)((CompoundedClassification >> 8) & 0xFF);
    public MarketRegion MarketRegion => (MarketRegion)((CompoundedClassification >> 16) & 0xFF);

    public override string ToString() =>
        $"{nameof(MarketClassification)}({nameof(MarketType)}: {MarketType}, " +
        $"{nameof(ProductType)}: {ProductType}, {nameof(MarketRegion)}: {MarketRegion})";
}

public static class MarketClassificationExtensions
{
    public static readonly MarketClassification Unknown    = new(0);
    public static readonly MarketClassification FxMajor    = new(MarketType.FXMajor, ProductType.Spot);
    public static readonly MarketClassification FxEmerging = new(MarketType.FXEmerging, ProductType.Spot);
    public static readonly MarketClassification FxExotic   = new(MarketType.FXExotic, ProductType.Spot);
    public static readonly MarketClassification EquityUs   = new(MarketType.EquityMajor, ProductType.Spot, MarketRegion.US);
    public static readonly MarketClassification EquityUk   = new(MarketType.EquityMajor, ProductType.Spot, MarketRegion.UK);
    public static readonly MarketClassification EquityAu   = new(MarketType.EquityMajor, ProductType.Spot, MarketRegion.Australia);

    public static string MarketExerciseType(this MarketClassification marketClassification, string delimiter = "-") =>
        marketClassification.MarketType + delimiter + marketClassification.ProductType;

    public static string MarketExerciseRegionType(this MarketClassification marketClassification, string delimiter = "-") =>
        marketClassification.MarketType + delimiter + marketClassification.ProductType + delimiter + marketClassification.MarketRegion;

    public static MarketClassification SetMarketType(this MarketClassification marketClassification, MarketType marketType) =>
        new(marketType, marketClassification.ProductType, marketClassification.MarketRegion);

    public static MarketClassification SetProductType(this MarketClassification marketClassification, ProductType productType) =>
        new(marketClassification.MarketType, productType, marketClassification.MarketRegion);

    public static MarketClassification SetMarketRegion(this MarketClassification marketClassification, MarketRegion marketRegion) =>
        new(marketClassification.MarketType, marketClassification.ProductType, marketRegion);
}
