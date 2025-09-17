// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Config;

#endregion

namespace FortitudeMarkets.Pricing;

public interface IPricingInstrumentId : IReusableObject<IPricingInstrumentId>, ISourceTickerId, IInstrument
{
    static readonly MarketClassification DefaultMarketClassification = new(0);

    [JsonIgnore] MarketClassification MarketClassification { get; set; }

    [JsonIgnore] string? Category { get; set; }

    new ushort SourceId       { get; set; }
    new ushort InstrumentId   { get; set; }
    new string SourceName     { get; set; }
    new string InstrumentName { get; set; }

    CountryCityCodes SourcePublishLocation  { get; set; }
    CountryCityCodes AdapterReceiveLocation { get; set; }
    CountryCityCodes ClientReceiveLocation  { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new DiscreetTimePeriod CoveringPeriod { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new InstrumentType InstrumentType { get; set; }

    new IPricingInstrumentId Clone();
}

public class PricingInstrumentId : SourceTickerId, IPricingInstrumentId
{
    private DiscreetTimePeriod? coveringPeriod;

    private string[]? optionalKeys;
    private string[]? requiredKeys;

    private InstrumentType? timeSeriesType;

    public PricingInstrumentId()
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        MarketClassification = IPricingInstrumentId.DefaultMarketClassification;
        InstrumentType       = InstrumentType.Price;
    }

    public PricingInstrumentId
    (ushort sourceId, ushort tickerId
      , string sourceName
      , string ticker
      , DiscreetTimePeriod coveringPeriod
      , InstrumentType instrumentType
      , MarketClassification marketClassification
      , string? category = null
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown
    ) :
        base(sourceId, tickerId, sourceName, ticker)
    {
        CoveringPeriod       = coveringPeriod;
        MarketClassification = marketClassification;
        Category             = category;
        InstrumentType       = instrumentType;

        SourcePublishLocation  = sourcePublishLocation;
        AdapterReceiveLocation = adapterReceiveLocation;
        ClientReceiveLocation  = clientReceiveLocation;
    }

    public PricingInstrumentId(IPricingInstrumentId toClone) : base(toClone)
    {
        CoveringPeriod       = toClone.CoveringPeriod;
        InstrumentType       = toClone.InstrumentType;
        MarketClassification = toClone.MarketClassification;
        Category             = toClone.Category;

        SourcePublishLocation  = toClone.SourcePublishLocation;
        AdapterReceiveLocation = toClone.AdapterReceiveLocation;
        ClientReceiveLocation  = toClone.ClientReceiveLocation;
    }

    public PricingInstrumentId(SourceTickerIdentifier toClone) : base(toClone)
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        MarketClassification = IPricingInstrumentId.DefaultMarketClassification;
        InstrumentType       = InstrumentType.Price;
    }

    public PricingInstrumentId(SourceTickerIdValue toClone) : base(toClone)
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        MarketClassification = IPricingInstrumentId.DefaultMarketClassification;
        InstrumentType       = InstrumentType.Price;
    }

    public PricingInstrumentId(PricingInstrumentIdValue toClone) : base(toClone)
    {
        CoveringPeriod       = toClone.CoveringPeriod;
        InstrumentType       = toClone.InstrumentType;
        MarketClassification = toClone.MarketClassification;
        Category             = toClone.Category;

        SourcePublishLocation  = toClone.SourcePublishLocation;
        AdapterReceiveLocation = toClone.AdapterReceiveLocation;
        ClientReceiveLocation  = toClone.ClientReceiveLocation;
    }

    public PricingInstrumentId(Instrument toClone)
    {
        CoveringPeriod = toClone.CoveringPeriod;
        InstrumentType = toClone.InstrumentType;
        var assetType     = toClone[nameof(RepositoryPathName.AssetType)] ?? "Unknown";
        var assetCategory = toClone[nameof(RepositoryPathName.AssetCategory)] ?? "Unknown";
        var productType   = toClone[nameof(RepositoryPathName.MarketProductType)] ?? "Unknown";
        var marketRegion  = toClone[nameof(RepositoryPathName.MarketRegion)] ?? "Unknown";
        var marketClassification = new MarketClassification
            (Enum.Parse<AssetType>(assetType), Enum.Parse<AssetCategory>(assetCategory)
           , productType.ExpandProductTypeToProductTypeFlags(), Enum.Parse<MarketRegion>(marketRegion));
        MarketClassification = marketClassification;
        Category             = toClone[nameof(RepositoryPathName.Category)];

        this[nameof(RepositoryPathName.MarketRoute)] = toClone[nameof(RepositoryPathName.MarketRoute)];
    }

    public DiscreetTimePeriod CoveringPeriod
    {
        get => coveringPeriod ?? new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        set => coveringPeriod = value;
    }

    [JsonIgnore] string IInstrument.InstrumentName => InstrumentName;

    [JsonIgnore] string IInstrument.SourceName     => SourceName;

    public CountryCityCodes SourcePublishLocation { get; set; }

    public CountryCityCodes AdapterReceiveLocation { get; set; }

    public CountryCityCodes ClientReceiveLocation { get; set; }

    public InstrumentType InstrumentType
    {
        get => timeSeriesType ?? InstrumentType.Price;
        set => timeSeriesType = value;
    }

    [JsonIgnore]
    public string? this[string key]
    {
        get
        {
            switch (key)
            {
                case nameof(RepositoryPathName.AssetType):         return MarketClassification.AssetType.ToString();
                case nameof(RepositoryPathName.AssetCategory):     return MarketClassification.AssetCategory.ToString();
                case nameof(RepositoryPathName.MarketProductType): return MarketClassification.ProductType.SquashProductTypeString();
                case nameof(RepositoryPathName.MarketRegion):      return MarketClassification.MarketRegion.ToString();
                case nameof(RepositoryPathName.MarketRoute):       return JoinMarketRouteCountryCityCodes();
                case nameof(RepositoryPathName.Category):          return Category;
            }
            return null;
        }
        set
        {
            switch (key)
            {
                case nameof(RepositoryPathName.AssetType):
                    if (Enum.TryParse<AssetType>(value, true, out var marketType))
                        if (MarketClassification.AssetType == AssetType.Unknown)
                            MarketClassification = MarketClassification.SetAssetType(marketType);
                    break;
                case nameof(RepositoryPathName.AssetCategory):
                    if (Enum.TryParse<AssetCategory>(value, true, out var assetCategory))
                        if (MarketClassification.AssetType == AssetType.Unknown)
                            MarketClassification = MarketClassification.SetAssetCategory(assetCategory);
                    break;
                case nameof(RepositoryPathName.MarketProductType):
                    if (value != null)
                    {
                        var productTypeFlags = value.ExpandProductTypeToProductTypeFlags();
                        if (MarketClassification.ProductType == ProductType.Unknown)
                            MarketClassification = MarketClassification.SetProductType(productTypeFlags);
                    }
                    break;
                case nameof(RepositoryPathName.MarketRegion):
                    if (Enum.TryParse<MarketRegion>(value, true, out var marketRegion))
                        if (MarketClassification.MarketRegion == MarketRegion.Unknown)
                            MarketClassification = MarketClassification.SetMarketRegion(marketRegion);
                    break;
                case nameof(RepositoryPathName.MarketRoute):
                    var split = value!.Split("-");
                    switch (split.Length)
                    {
                        case 3:
                            ClientReceiveLocation  = Enum.Parse<CountryCityCodes>(split[2]);
                            AdapterReceiveLocation = Enum.Parse<CountryCityCodes>(split[1]);
                            SourcePublishLocation  = Enum.Parse<CountryCityCodes>(split[0]);
                            break;
                        case 2:
                            ClientReceiveLocation  = Enum.Parse<CountryCityCodes>(split[1]);
                            AdapterReceiveLocation = Enum.Parse<CountryCityCodes>(split[1]);
                            SourcePublishLocation  = Enum.Parse<CountryCityCodes>(split[0]);
                            break;
                        case 1:
                            ClientReceiveLocation  = Enum.Parse<CountryCityCodes>(split[0]);
                            AdapterReceiveLocation = Enum.Parse<CountryCityCodes>(split[0]);
                            SourcePublishLocation  = Enum.Parse<CountryCityCodes>(split[0]);
                            break;
                    }
                    break;
                case nameof(RepositoryPathName.Category):
                    if (Category.IsNullOrEmpty()) Category = value ?? "";
                    break;
            }
        }
    }

    public IEnumerable<KeyValuePair<string, string>> FilledAttributes
    {
        get
        {
            if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
            if (MarketClassification.AssetType != AssetType.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.AssetType), MarketClassification.AssetType.ToString());
            if (MarketClassification.ProductType != ProductType.Unknown)
                yield return new KeyValuePair<string, string>
                    (nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.SquashProductTypeString());
            if (MarketClassification.MarketRegion != MarketRegion.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
            if (SourcePublishLocation != CountryCityCodes.Unknown || AdapterReceiveLocation != CountryCityCodes.Unknown ||
                ClientReceiveLocation != CountryCityCodes.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRoute), JoinMarketRouteCountryCityCodes());
        }
    }

    public void Add(KeyValuePair<string, string> instrumentAttribute)
    {
        this[instrumentAttribute.Key] = instrumentAttribute.Value;
    }

    public void Add(string name, string value)
    {
        this[name] = value;
    }

    public bool Remove(string name)
    {
        switch (name)
        {
            case nameof(RepositoryPathName.AssetType):
                MarketClassification = MarketClassification.SetAssetType(AssetType.Unknown);
                return true;
            case nameof(RepositoryPathName.AssetCategory):
                MarketClassification = MarketClassification.SetAssetCategory(AssetCategory.Unknown);
                return true;
            case nameof(RepositoryPathName.MarketProductType):
                MarketClassification = MarketClassification.SetProductType(ProductType.Unknown);
                return true;
            case nameof(RepositoryPathName.MarketRegion):
                MarketClassification = MarketClassification.SetMarketRegion(MarketRegion.Unknown);
                return true;
            case nameof(RepositoryPathName.MarketRoute):
                SourcePublishLocation  = CountryCityCodes.Unknown;
                AdapterReceiveLocation = CountryCityCodes.Unknown;
                ClientReceiveLocation  = CountryCityCodes.Unknown;
                return true;
            case nameof(RepositoryPathName.Category):
                Category = null;
                return true;
        }
        return false;
    }

    public IEnumerable<string> RequiredAttributeKeys
    {
        get => requiredKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiRequiredInstrumentKeys;
        set => requiredKeys = value.ToArray();
    }

    public IEnumerable<string> OptionalAttributeKeys
    {
        get => optionalKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiOptionalInstrumentKeys;
        set => optionalKeys = value.ToArray();
    }

    public IEnumerable<KeyValuePair<string, string>> AllAttributes
    {
        get
        {
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.AssetType), MarketClassification.AssetType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.SquashProductTypeString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
            if (SourcePublishLocation != CountryCityCodes.Unknown || AdapterReceiveLocation != CountryCityCodes.Unknown || ClientReceiveLocation != CountryCityCodes.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRoute), JoinMarketRouteCountryCityCodes());
            if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
        }
    }

    public IEnumerable<KeyValuePair<string, string>> RequiredAttributes
    {
        get
        {
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.AssetType), MarketClassification.AssetType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.SquashProductTypeString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
        }
    }

    public IEnumerable<KeyValuePair<string, string>> OptionalAttributes
    {
        get
        {
            if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
            if (SourcePublishLocation != CountryCityCodes.Unknown || AdapterReceiveLocation != CountryCityCodes.Unknown || ClientReceiveLocation != CountryCityCodes.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRoute), JoinMarketRouteCountryCityCodes());
        }
    }


    public bool HasAllRequiredKeys =>
        SourceName.IsNotNullOrEmpty()
     && MarketClassification.AssetType != AssetType.Unknown
     && MarketClassification.ProductType != ProductType.Unknown
     && MarketClassification.MarketRegion != MarketRegion.Unknown;

    public MarketClassification MarketClassification { get; set; }

    public string? Category { get; set; }

    public override void StateReset()
    {
        MarketClassification   = IPricingInstrumentId.DefaultMarketClassification;
        SourcePublishLocation  = CountryCityCodes.Unknown;
        AdapterReceiveLocation = CountryCityCodes.Unknown;
        ClientReceiveLocation  = CountryCityCodes.Unknown;

        Category       = null;
        CoveringPeriod = new DiscreetTimePeriod();

        base.StateReset();
    }

    private string JoinMarketRouteCountryCityCodes()
    {
        var sb = new StringBuilder(16);
        if (SourcePublishLocation != CountryCityCodes.Unknown)
        {
            sb.Append(SourcePublishLocation.ToString());
        }
        if (AdapterReceiveLocation != CountryCityCodes.Unknown && AdapterReceiveLocation != SourcePublishLocation)
        {
            if (sb.Length > 0) sb.Append("-");
            sb.Append(AdapterReceiveLocation.ToString());
        }
        if (ClientReceiveLocation != CountryCityCodes.Unknown && ClientReceiveLocation != AdapterReceiveLocation)
        {
            if (sb.Length > 0) sb.Append("-");
            sb.Append(ClientReceiveLocation.ToString());
        }
        return sb.ToString();
    }

    IPricingInstrumentId ICloneable<IPricingInstrumentId>.Clone() => Clone();

    public override IPricingInstrumentId Clone() =>
        Recycler?.Borrow<PricingInstrumentId>().CopyFrom(this) ?? new PricingInstrumentId((IPricingInstrumentId)this);

    IPricingInstrumentId ITransferState<IPricingInstrumentId>.CopyFrom(IPricingInstrumentId source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IReusableObject<IPricingInstrumentId> ITransferState<IReusableObject<IPricingInstrumentId>>.CopyFrom
        (IReusableObject<IPricingInstrumentId> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPricingInstrumentId)source, copyMergeFlags);

    public override PricingInstrumentId CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPricingInstrumentId pricingInstrumentId)
        {
            CoveringPeriod         = pricingInstrumentId.CoveringPeriod;
            MarketClassification   = pricingInstrumentId.MarketClassification;
            InstrumentType         = pricingInstrumentId.InstrumentType;
            SourcePublishLocation  = pricingInstrumentId.SourcePublishLocation;
            AdapterReceiveLocation = pricingInstrumentId.AdapterReceiveLocation;
            ClientReceiveLocation  = pricingInstrumentId.ClientReceiveLocation;
            Category               = pricingInstrumentId.Category;
        }
        else if (source is IInstrument instrument)
        {
            foreach (var sourceAttributes in instrument.FilledAttributes)
            {
                this[sourceAttributes.Key] = sourceAttributes.Value;
            }
        }
        return this;
    }

    public override bool AreEquivalent(ISourceTickerId? other, bool exactTypes = false)
    {
        if (other is not IPricingInstrumentId pricingInstrumentId) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);

        var marketClassificationSame   = Equals(MarketClassification, pricingInstrumentId.MarketClassification);
        var coveringPeriodSame         = coveringPeriod == pricingInstrumentId.CoveringPeriod;
        var instrumentTypeSame         = timeSeriesType == pricingInstrumentId.InstrumentType;

        var publishLocationSame        = true;
        var adapterReceiveLocationSame = true;
        var clientReceiveLocationSame  = true;
        var categorySame               = true;
        if (exactTypes)
        {
            categorySame               = Category == pricingInstrumentId.Category;
            publishLocationSame        = SourcePublishLocation == pricingInstrumentId.SourcePublishLocation;
            adapterReceiveLocationSame = AdapterReceiveLocation == pricingInstrumentId.AdapterReceiveLocation;
            clientReceiveLocationSame  = ClientReceiveLocation == pricingInstrumentId.ClientReceiveLocation;
        }

        var allAreSame = baseSame && coveringPeriodSame && marketClassificationSame && instrumentTypeSame && categorySame
                      && publishLocationSame && adapterReceiveLocationSame && clientReceiveLocationSame;

        return allAreSame;
    }

    protected bool Equals(PricingInstrumentId other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PricingInstrumentId)obj);
    }

    public override int GetHashCode() => HashCode.Combine(coveringPeriod, timeSeriesType, MarketClassification);

    protected string PricingInstrumentIdToStringMembers =>
        $"{SourceTickerIdToStringMembers}, {nameof(CoveringPeriod)}: {CoveringPeriod}, {nameof(InstrumentType)}: {InstrumentType}, " +
        $"{nameof(MarketClassification)}: {MarketClassification}, {nameof(Category)}: {Category}, {nameof(SourcePublishLocation)}: {SourcePublishLocation}, " +
        $"{nameof(AdapterReceiveLocation)}: {AdapterReceiveLocation}, {nameof(ClientReceiveLocation)}: {ClientReceiveLocation}";

    public override StateExtractStringRange RevealState(ITheOneString stsa) => 
        stsa.StartComplexType(this)
            .AddBaseStyledToStringFields(this)
            .Field.AlwaysAdd(nameof(CoveringPeriod), CoveringPeriod, DiscreetTimePeriod.Styler)
            .Field.AlwaysAdd(nameof(InstrumentType), InstrumentType)
            .Field.AlwaysAdd(nameof(MarketClassification), MarketClassification, MarketClassification.Styler)
            .Field.AlwaysAdd(nameof(Category), Category)
            .Field.WhenNonDefaultAdd(nameof(SourcePublishLocation), SourcePublishLocation)
            .Field.WhenNonDefaultAdd(nameof(AdapterReceiveLocation), AdapterReceiveLocation)
            .Field.WhenNonDefaultAdd(nameof(ClientReceiveLocation), ClientReceiveLocation)
            .Complete();

    public override string ToString() => $"{nameof(PricingInstrumentId)}{{{PricingInstrumentIdToStringMembers}}}";
}

public readonly struct PricingInstrumentIdValue // not inheriting from IPricingInstrumentId to prevent accidental boxing unboxing
{
    public PricingInstrumentIdValue(IPricingInstrumentId pricingInstrumentId)
    {
        SourceId       = pricingInstrumentId.SourceId;
        InstrumentId   = pricingInstrumentId.InstrumentId;
        CoveringPeriod = pricingInstrumentId.CoveringPeriod;
        Category       = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        InstrumentType = pricingInstrumentId.InstrumentType;

        SourcePublishLocation  = pricingInstrumentId.SourcePublishLocation;
        AdapterReceiveLocation = pricingInstrumentId.AdapterReceiveLocation;
        ClientReceiveLocation  = pricingInstrumentId.ClientReceiveLocation;
    }

    public PricingInstrumentIdValue
    (ushort sourceId, ushort instrumentId, DiscreetTimePeriod coveringPeriod, InstrumentType instrumentType
      , MarketClassification marketClassification = default, string? category = null
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown)
    {
        SourceId       = sourceId;
        InstrumentId   = instrumentId;
        CoveringPeriod = coveringPeriod;
        Category       = category;

        InstrumentType = instrumentType;

        MarketClassification = marketClassification;

        SourcePublishLocation  = sourcePublishLocation;
        AdapterReceiveLocation = adapterReceiveLocation;
        ClientReceiveLocation  = clientReceiveLocation;
    }

    public PricingInstrumentIdValue
    (SourceTickerIdentifier sourceTickerIdentifier, DiscreetTimePeriod coveringPeriod, InstrumentType instrumentType
      , MarketClassification marketClassification = default, string? category = null
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown)
    {
        SourceId       = sourceTickerIdentifier.SourceId;
        InstrumentId   = sourceTickerIdentifier.InstrumentId;
        CoveringPeriod = coveringPeriod;
        Category       = category;

        InstrumentType = instrumentType;

        MarketClassification = marketClassification;

        SourcePublishLocation  = sourcePublishLocation;
        AdapterReceiveLocation = adapterReceiveLocation;
        ClientReceiveLocation  = clientReceiveLocation;
    }

    public PricingInstrumentIdValue
    (ushort sourceId, ushort instrumentId, PeriodInstrumentTypePair periodInstrumentTypePair
      , MarketClassification marketClassification = default, string? category = null
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown)
    {
        SourceId       = sourceId;
        InstrumentId   = instrumentId;
        CoveringPeriod = periodInstrumentTypePair.CoveringPeriod;
        Category       = category;

        InstrumentType = periodInstrumentTypePair.InstrumentType;

        MarketClassification = marketClassification;

        SourcePublishLocation  = sourcePublishLocation;
        AdapterReceiveLocation = adapterReceiveLocation;
        ClientReceiveLocation  = clientReceiveLocation;
    }

    public PricingInstrumentIdValue
    (ISourceTickerId sourceTickerId, PeriodInstrumentTypePair periodInstrumentTypePair
      , MarketClassification marketClassification = default, string? category = null
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown)
    {
        SourceId       = sourceTickerId.SourceId;
        InstrumentId   = sourceTickerId.InstrumentId;
        CoveringPeriod = periodInstrumentTypePair.CoveringPeriod;
        Category       = category;

        InstrumentType = periodInstrumentTypePair.InstrumentType;

        MarketClassification = marketClassification;

        SourcePublishLocation  = sourcePublishLocation;
        AdapterReceiveLocation = adapterReceiveLocation;
        ClientReceiveLocation  = clientReceiveLocation;
    }

    public PricingInstrumentIdValue
    (SourceTickerIdentifier sourceTickerIdentifier, PeriodInstrumentTypePair periodInstrumentTypePair
      , MarketClassification marketClassification = default, string? category = null
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown
    )
    {
        SourceId       = sourceTickerIdentifier.SourceId;
        InstrumentId   = sourceTickerIdentifier.InstrumentId;
        CoveringPeriod = periodInstrumentTypePair.CoveringPeriod;
        Category       = category;

        InstrumentType = periodInstrumentTypePair.InstrumentType;

        MarketClassification = marketClassification;

        SourcePublishLocation  = sourcePublishLocation;
        AdapterReceiveLocation = adapterReceiveLocation;
        ClientReceiveLocation  = clientReceiveLocation;
    }

    public uint SourceTickerId => (uint)((SourceId << 16) | InstrumentId);

    public ushort SourceId     { get; }
    public ushort InstrumentId { get; }

    public DiscreetTimePeriod CoveringPeriod { get; }
    public InstrumentType     InstrumentType { get; }

    public CountryCityCodes SourcePublishLocation { get; }

    public CountryCityCodes AdapterReceiveLocation { get; }

    public CountryCityCodes ClientReceiveLocation { get; }

    public MarketClassification MarketClassification { get; }

    public string? Category { get; }

    public string InstrumentName => SourceTickerIdentifierExtensions.GetRegisteredInstrumentName(SourceTickerId);
    public string SourceName     => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);

    
    public static StringBearerRevealState<PricingInstrumentIdValue> Styler { get; } =
        (piiv, stsa) =>
            stsa.StartComplexType(piiv, nameof(piiv))
                .Field.AlwaysAdd(nameof(piiv.SourceTickerId), piiv.SourceTickerId)
                .Field.AlwaysAdd(nameof(piiv.SourceId), piiv.SourceId)
                .Field.AlwaysAdd(nameof(piiv.InstrumentId), piiv.InstrumentId)
                .Field.AlwaysAdd(nameof(piiv.InstrumentName), piiv.InstrumentName)
                .Field.AlwaysAdd(nameof(piiv.SourceName), piiv.SourceName)
                .Field.AlwaysAdd(nameof(piiv.CoveringPeriod), piiv.CoveringPeriod, DiscreetTimePeriod.Styler)
                .Field.AlwaysAdd(nameof(piiv.InstrumentType), piiv.InstrumentType)
                .Field.AlwaysAdd(nameof(piiv.MarketClassification), piiv.MarketClassification, MarketClassification.Styler)
                .Field.AlwaysAdd(nameof(piiv.Category), piiv.Category)
                .Field.WhenNonDefaultAdd(nameof(piiv.SourcePublishLocation), piiv.SourcePublishLocation)
                .Field.WhenNonDefaultAdd(nameof(piiv.AdapterReceiveLocation), piiv.AdapterReceiveLocation)
                .Field.WhenNonDefaultAdd(nameof(piiv.ClientReceiveLocation), piiv.ClientReceiveLocation)
                .Complete();
    
    public override string ToString() =>
        $"{nameof(PricingInstrumentIdValue)}({nameof(SourceId)}: {SourceId}, {nameof(InstrumentId)}: {InstrumentId}, {nameof(InstrumentName)}: {InstrumentName}, " +
        $"{nameof(SourceName)}: {SourceName}, {nameof(CoveringPeriod)}: {CoveringPeriod}, {nameof(InstrumentType)}: {InstrumentType}, " +
        $"{nameof(MarketClassification)}: {MarketClassification}, {nameof(Category)}: {Category}, {nameof(SourcePublishLocation)}: {SourcePublishLocation}, " +
        $"{nameof(AdapterReceiveLocation)}: {AdapterReceiveLocation} ,{nameof(ClientReceiveLocation)}: {ClientReceiveLocation})";


    public static implicit operator SourceTickerIdentifier(PricingInstrumentIdValue sourceTickerId) =>
        new(sourceTickerId.SourceId, sourceTickerId.InstrumentId);

    public static implicit operator PeriodInstrumentTypePair(PricingInstrumentIdValue sourceTickerId) =>
        new(sourceTickerId.InstrumentType, sourceTickerId.CoveringPeriod);
}

[Flags]
public enum CompoundLocationSetFlags : byte
{
    None              = 0
  , SourcePublishSet  = 0x01
  , AdapterReceiveSet = 0x02
  , ClientReceiveSet  = 0x04
}

public static class PricingInstrumentIdExtensions
{
    private static readonly ConcurrentMap<uint, string> SingleStringShortNameLookup = new();

    private static readonly ConcurrentMap<uint, PricingInstrumentIdValue> PricingInstrumentIdLookup = new();

    public static uint CompoundLocations(this IPricingInstrumentId pricingInstrumentId, CompoundLocationSetFlags? onlyThese = null)
    {
        CompoundLocationSetFlags setFlags = onlyThese ?? CompoundLocationSetFlags.None;
        if (onlyThese == null)
        {
            setFlags = pricingInstrumentId.SourcePublishLocation != CountryCityCodes.Unknown
                ? CompoundLocationSetFlags.SourcePublishSet
                : CompoundLocationSetFlags.None;
            setFlags |= pricingInstrumentId.AdapterReceiveLocation != CountryCityCodes.Unknown
                ? CompoundLocationSetFlags.AdapterReceiveSet
                : CompoundLocationSetFlags.None;
            setFlags |= pricingInstrumentId.ClientReceiveLocation != CountryCityCodes.Unknown
                ? CompoundLocationSetFlags.ClientReceiveSet
                : CompoundLocationSetFlags.None;
        }
        var payload = 0u;
        if ((setFlags & CompoundLocationSetFlags.ClientReceiveSet) > 0)
        {
            payload = (uint)pricingInstrumentId.ClientReceiveLocation;
        }
        payload <<= 8;
        if ((setFlags & CompoundLocationSetFlags.AdapterReceiveSet) > 0)
        {
            payload |= (uint)pricingInstrumentId.AdapterReceiveLocation;
        }
        payload <<= 8;
        if ((setFlags & CompoundLocationSetFlags.SourcePublishSet) > 0)
        {
            payload |= (uint)pricingInstrumentId.SourcePublishLocation;
        }
        payload <<= 8;
        payload |=  (uint)setFlags;
        return payload;
    }

    public static void ApplyCompoundLocations(this IPricingInstrumentId pricingInstrumentId, uint compoundedLocation)
    {
        var setFlags = (CompoundLocationSetFlags)(compoundedLocation & 0xFF);
        compoundedLocation >>= 8;
        if ((setFlags & CompoundLocationSetFlags.SourcePublishSet) > 0)
        {
            pricingInstrumentId.SourcePublishLocation  = (CountryCityCodes)(compoundedLocation & 0xFF);
        }
        compoundedLocation >>= 8;
        if ((setFlags & CompoundLocationSetFlags.AdapterReceiveSet) > 0)
        {
            pricingInstrumentId.AdapterReceiveLocation          = (CountryCityCodes)(compoundedLocation & 0xFF);
        }
        compoundedLocation >>= 8;
        if ((setFlags & CompoundLocationSetFlags.ClientReceiveSet) > 0)
        {
            pricingInstrumentId.ClientReceiveLocation = (CountryCityCodes)(compoundedLocation & 0xFF);
        }
    }

    public static bool Register(this IPricingInstrumentId id)
    {
        SourceTickerIdentifierExtensions.Register(id);
        if (!PricingInstrumentIdLookup.ContainsKey(id.SourceInstrumentId))
        {
            var pricingInstrumentId
                = new PricingInstrumentIdValue(id.SourceId, id.InstrumentId, new PeriodInstrumentTypePair(id.InstrumentType, id.CoveringPeriod));
            PricingInstrumentIdLookup.TryAdd(id.SourceInstrumentId, pricingInstrumentId);
        }
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceInstrumentId, out var shortName))
            if (id.SourceName != SourceTickerIdentifierExtensions.NoSourceNameValue &&
                id.InstrumentName != SourceTickerIdentifierExtensions.NoTickerNameValue)
            {
                shortName = $"{id.SourceName}-{id.InstrumentName}_{id.InstrumentType}-{id.CoveringPeriod.ShortName()}";
                SingleStringShortNameLookup.TryAdd(id.SourceInstrumentId, shortName);
                return true;
            }
        return false;
    }

    public static bool Register(this PricingInstrumentIdValue id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
            if (id.SourceName != SourceTickerIdentifierExtensions.NoSourceNameValue &&
                id.InstrumentName != SourceTickerIdentifierExtensions.NoTickerNameValue)
            {
                shortName = $"{id.SourceName}-{id.InstrumentName}_{id.InstrumentType}-{id.CoveringPeriod.ShortName()}";
                SingleStringShortNameLookup.TryAdd(id.SourceTickerId, shortName);
                return true;
            }
        return false;
    }

    public static string GetReferenceShortName(this IPricingInstrumentId id) => GetReferenceShortName(id.ToPricingInstrumentId());

    public static string GetReferenceShortName(this PricingInstrumentIdValue id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
        {
            if (id.SourceName != SourceTickerIdentifierExtensions.NoSourceNameValue &&
                id.InstrumentName != SourceTickerIdentifierExtensions.NoTickerNameValue)
            {
                shortName = $"{id.SourceName}-{id.InstrumentName}_{id.InstrumentType}-{id.CoveringPeriod.ShortName()}";
                SingleStringShortNameLookup.TryAdd(id.SourceTickerId, shortName);
            }
            else
            {
                shortName = $"{SourceTickerIdentifierExtensions.NoSourceTickerShortNameValue}_{id.InstrumentType}_{id.CoveringPeriod.ShortName()}";
            }
        }
        return shortName!;
    }

    public static PricingInstrumentIdValue ToPricingInstrumentId(this IPricingInstrumentId pricingInstrumentId) => new(pricingInstrumentId);

    public static PricingInstrumentIdValue ToPricingInstrumentId(this ISourceTickerId id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id, periodInstrumentType);

    public static PricingInstrumentIdValue ToPricingInstrumentId(this SourceTickerIdentifier id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id, periodInstrumentType);

    public static PricingInstrumentIdValue ToPricingInstrumentId(this SourceTickerIdValue id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id.SourceId, id.InstrumentId, periodInstrumentType);

    public static PricingInstrumentIdValue ToPricingInstrumentId
        (this SourceTickerIdentifier id, DiscreetTimePeriod period, InstrumentType instrumentType) =>
        new(id, period, instrumentType);

    public static PricingInstrumentIdValue ToPricingInstrumentId
        (this SourceTickerIdValue id, DiscreetTimePeriod period, InstrumentType instrumentType) =>
        new(id.SourceId, id.InstrumentId, period, instrumentType);
}
