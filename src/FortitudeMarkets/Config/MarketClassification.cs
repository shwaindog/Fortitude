// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using System.Text.Json.Serialization;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Config;

[JsonConverter(typeof(JsonStringEnumConverter<MarketRegion>))]
public enum MarketRegion : byte
{
    Unknown = 0
  , Any
  , Global
  , Local
  , Internal
  , Australia
  , UnitedStates
  , UnitedKingdom
  , EuropeanUnion
  , Switzerland
  , ChinaMainland
  , ChinaHongKong
  , ChinaTaiwan
  , Singapore
  , Japan
  , NewZealand
  , Norway
  , Canada
  , India
  , Belgium
  , Netherlands
  , Ireland
  , Portugal
  , Brazil
  , Bulgaria
  , Czechia
  , Croatia
  , France
  , Germany
  , Italy
  , Spain
  , Sweden
  , Poland
  , Slovakia
  , Serbia
  , NorthMacedonia
  , Greece
  , Turkey
  , Hungary
  , Finland
  , SouthKorea
  , Argentina
  , Paraguay
  , Pakistan
  , Israel
  , Mexico
  , Indonesia
  , SouthAfrica
  , Malaysia
  , Philippines
  , Russia
  , Ukraine
  , Belarus
  , Latvia
  , Lithuania
  , Estonia
  , UAE
  , Qatar
  , SaudiArabia
  , Iran
}

[JsonConverter(typeof(JsonStringEnumConverter<AssetType>))]
public enum AssetType : byte
{
    Unknown = 0
  , Any
  , Internal
  , None
  , Other
  , Basket
  , FX
  , Cryptocurrencies
  , CashRate
  , Bonds
  , Commodity
  , WeatherFuture
  , RealEstateFuture
  , Equity
}

// https://en.wikipedia.org/wiki/Industry_Classification_Benchmark
[JsonConverter(typeof(JsonStringEnumConverter<AssetType>))]
public enum AssetCategory : byte
{
    Unknown = 0
  , Any
  , None
  , Other
  , Major                  // G10
  , Emerging               // >G10+  
  , Aggregate              // Index, Basket
  , Government             // Bonds US Treasuries, German Bunds, 
  , Corporate              // Bonds, Corporate
  , Soft                   // Grown - Pork Bellies, Soybean, Corn
  , MetalsBase             // Fe, Cu, Pb, Ni, Zn, Al
  , MetalsPrecious         // AU, Ag, Pt, Pd
  , HardOther              // BHP, Forestry Wood
  , EnergyNonRenewable     // Oil, Coal, Natural Gas
  , EnergyRenewable        // BioFuels, Wind Farms
  , ManufacturingMaterials // Process Industries
  , ManufacturingProducer  // Caterpillar
  , Industrials            // Construction and Materials, Defense/Aerospace, machinery, paint, 
  , ConsumerStaples        // Beverages, Food Producers, Tobacco, Personal Care, Drug, Grocery Stores 
  , ConsumerServices       // Fast Food, Disney, Airbnb
  , ConsumerDurables       // Cars, TVs, toys
  , Healthcare             // Private Hospitals
  , HealthTechnology       // Pharmaceuticals
  , Financials             // Banks, Brokers, Insurance
  , Technology             // MS, Intel, Nvidia, Oracle, Social Media
  , Telecommunication      // Telephone, Internet, Undersea cables
  , Transportation         // Logistics, Railroads
  , RetailTrade            // Amazon, Walmart
  , Utilities              // Electricity, Gas, Water, Waste Disposal
  , Weather
  , RealEstate
  , EquityIndexDJIAverage
  , EquityIndexNasdaqComposite // US - all stocks listed on the Nasdaq stock exchange. Along with the Dow Jones Industrial Average and S&P 500
  , EquityIndexNasdaq100 // US - 100 of the largest non-financial companies
  , EquityIndexSnP500 // US - 500 leading companies listed on stock exchanges in the United States
  , EquityIndexNyseComposite // US - All common stock on NYSE
  , EquityIndexSnPTSXComposite // US CAD - Roughly 70% Toronto Stock Exchange
  , EquityIndexEuroStoxx50 // EU - 50 stocks from 11 countries in the Eurozone
  , EquityIndexEuroNext100 // EU - 100 most liquid stocks in the Eurozone
  , EquityIndexNikkei225 // Japan - 225 largest companies
  , EquityIndexVolatility // generally CBOE Vix
  , EquityIndexFTSE100 // UK Top 100
  , EquityIndexFTSEChinaA50 // UK-China - 50 companies from Shanghai and Shenzhen SE
  , EquityIndexDAXPerformance // Germany - Top 40
  , EquityIndexCAC40 // France - Top 40
  , EquityIndexHangSeng // China - Hong Kong - Top 82
  , EquityIndexSSEComposite // China - All stock traded on Shanghai Stock exchange
  , EquityIndexSZSEComposite // China - Shenzhen Top 500 companies
  , EquityIndexSnPASX200 // Australia - Top 200 largest companies
  , EquityIndexAllOrdinaries // Australia - Top 500
  , EquityIndexKOSPI // South Korea -  all common traded stock  
  , EquityIndexNIFTY50 // India - 50 largest companies
  , EquityIndexBEL20 // Belgium - Largest 20 companies
  , EquityIndexSwissMarket // Switzerland - Top 20
  , EquityIndexIBOVESPA // Brazil - Top 86
  , EquityIndexCapitalizationWeighted //  stock market index whose components are weighted according to the total market value of their outstanding share
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductType>))]
[Flags]
public enum ProductType : byte
{
    Unknown = 0
  , Spot    = 1
  , Index   = 2
  , Swap    = 4
  , Future  = 8
  , Forward = 16
  , Option  = 32
  , CFD     = 64 // Contract For Difference/ Non-Deliverable Forward (NDF) / Spread Bet
}

public static class ProductTypeExtensions
{
    public static bool IsSpot(this ProductType flags)    => (flags & ProductType.Spot) > 0;
    public static bool IsIndex(this ProductType flags)   => (flags & ProductType.Index) > 0;
    public static bool IsSwap(this ProductType flags)    => (flags & ProductType.Swap) > 0;
    public static bool IsFuture(this ProductType flags)  => (flags & ProductType.Future) > 0;
    public static bool IsForward(this ProductType flags) => (flags & ProductType.Forward) > 0;
    public static bool IsOption(this ProductType flags)  => (flags & ProductType.Option) > 0;
    public static bool IsCFD(this ProductType flags)     => (flags & ProductType.CFD) > 0;

    public static string SquashProductTypeString(this ProductType productTypeFlags)
    {
        var sb = new StringBuilder(32);
        if (productTypeFlags.IsSpot())
        {
            sb.Append(ProductType.Spot);
        }
        if (productTypeFlags.IsIndex())
        {
            sb.Append(ProductType.Index);
        }
        if (productTypeFlags.IsSwap())
        {
            sb.Append(ProductType.Swap);
        }
        if (productTypeFlags.IsFuture())
        {
            sb.Append(ProductType.Future);
        }
        if (productTypeFlags.IsForward())
        {
            sb.Append(ProductType.Forward);
        }
        if (productTypeFlags.IsOption())
        {
            sb.Append(ProductType.Option);
        }
        if (productTypeFlags.IsCFD())
        {
            sb.Append(ProductType.CFD);
        }
        return sb.ToString();
    }

    public static ProductType ExpandProductTypeToProductTypeFlags(this string squashedString)
    {
        var returnProductTypeFlags = ProductType.Unknown;

        var sb = new StringBuilder(squashedString);
        for (int i = 1; i < sb.Length; i++)
        {
            char charAtIndex = sb[i];
            if(charAtIndex < 'A' || charAtIndex > 'Z') continue;
            returnProductTypeFlags |= Enum.Parse<ProductType>(sb.ToString(0, i));
            sb.Remove(0, i);
            i = 1;
        }
        return returnProductTypeFlags;
    }
}

public struct MarketClassification : IEquatable<MarketClassification>
{
    public static readonly MarketClassification Unknown = IPricingInstrumentId.DefaultMarketClassification;

    public uint CompoundedClassification;

    public MarketClassification() => CompoundedClassification = 0;

    public MarketClassification(uint compoundedClassification) => CompoundedClassification = compoundedClassification;

    public MarketClassification
        (AssetType assetType, AssetCategory assetCategory, ProductType productType, MarketRegion marketRegion = MarketRegion.Global) =>
        CompoundedClassification = ((uint)marketRegion << 24) | ((uint)productType << 16) | ((uint)assetCategory << 8) | (uint)assetType;

    public AssetType     AssetType     => (AssetType)(CompoundedClassification & 0xFF);
    public AssetCategory AssetCategory => (AssetCategory)((CompoundedClassification >> 8) & 0xFF);
    public ProductType   ProductType   => (ProductType)((CompoundedClassification >> 16) & 0xFF);
    public MarketRegion  MarketRegion  => (MarketRegion)((CompoundedClassification >> 24) & 0xFF);

    public bool Equals(MarketClassification other) => CompoundedClassification == other.CompoundedClassification;

    public override bool Equals(object? obj) => obj is MarketClassification other && Equals(other);

    public override int GetHashCode() => (int)CompoundedClassification;

    public static PalantírReveal<MarketClassification> Styler { get; } =
        (mc, stsa) =>
            stsa.StartComplexType(mc)
                .Field.AlwaysAddObject(nameof(mc.AssetType), mc.AssetType)
                .Field.AlwaysAdd(nameof(mc.AssetCategory), mc.AssetCategory)
                .Field.AlwaysAdd(nameof(mc.ProductType), mc.ProductType)
                .Field.AlwaysAdd(nameof(mc.MarketRegion), mc.MarketRegion)
                .Complete();
    
    public override string ToString() =>
        $"{nameof(MarketClassification)}({nameof(AssetType)}: {AssetType}, {nameof(AssetCategory)}: {AssetCategory}, " +
        $"{nameof(ProductType)}: {ProductType}, {nameof(MarketRegion)}: {MarketRegion})";
}

public static class MarketClassificationExtensions
{
    public static readonly MarketClassification FxMajor    = new(AssetType.FX, AssetCategory.Major, ProductType.Spot);
    public static readonly MarketClassification FxEmerging = new(AssetType.FX, AssetCategory.Emerging, ProductType.Spot);
    public static readonly MarketClassification EquityUs   = new(AssetType.Equity, AssetCategory.Major, ProductType.Spot, MarketRegion.UnitedStates);
    public static readonly MarketClassification EquityUk   = new(AssetType.Equity, AssetCategory.Major, ProductType.Spot, MarketRegion.UnitedKingdom);
    public static readonly MarketClassification EquityAu = new(AssetType.Equity, AssetCategory.EquityIndexAllOrdinaries, ProductType.Spot
                                                             , MarketRegion.Australia);

    public static string MarketExerciseType(this MarketClassification marketClassification, string delimiter = "-") =>
        marketClassification.AssetType + delimiter + marketClassification.ProductType;

    public static string MarketExerciseRegionType(this MarketClassification marketClassification, string delimiter = "-") =>
        marketClassification.AssetType + delimiter + marketClassification.ProductType + delimiter + marketClassification.MarketRegion;

    public static MarketClassification SetAssetType(this MarketClassification marketClassification, AssetType assetType) =>
        new(assetType, marketClassification.AssetCategory, marketClassification.ProductType, marketClassification.MarketRegion);

    public static MarketClassification SetAssetCategory(this MarketClassification marketClassification, AssetCategory assetCategory) =>
        new(marketClassification.AssetType, assetCategory, marketClassification.ProductType, marketClassification.MarketRegion);

    public static MarketClassification SetProductType(this MarketClassification marketClassification, ProductType productType) =>
        new(marketClassification.AssetType, marketClassification.AssetCategory, productType, marketClassification.MarketRegion);

    public static MarketClassification SetMarketRegion(this MarketClassification marketClassification, MarketRegion marketRegion) =>
        new(marketClassification.AssetType, marketClassification.AssetCategory, marketClassification.ProductType, marketRegion);
}
