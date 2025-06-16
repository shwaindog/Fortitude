// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

public interface IPQPricingInstrumentId : IPQSourceTickerId, IPricingInstrumentId, ICloneable<IPQPricingInstrumentId>
{
    bool IsMarketClassificationUpdated      { get; set; }
    bool IsSourcePublicationLocationUpdated { get; set; }
    bool IsAdapterReceiveLocationUpdated    { get; set; }
    bool IsClientReceiveLocationUpdated     { get; set; }

    new ushort SourceId       { get; set; }
    new ushort InstrumentId   { get; set; }
    new string SourceName     { get; set; }
    new string InstrumentName { get; set; }

    new IPQPricingInstrumentId Clone();
}

public class PQPricingInstrumentId : PQSourceTickerId, IPQPricingInstrumentId
{
    private InstrumentType?      instrumentType;
    private MarketClassification marketClassification;

    private CountryCityCodes sourcePublishLocation  = CountryCityCodes.Unknown;
    private CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown;
    private CountryCityCodes clientReceiveLocation  = CountryCityCodes.Unknown;

    private string[]? optionalKeys;
    private string[]? requiredKeys;

    public PQPricingInstrumentId()
    {
        marketClassification = MarketClassification.Unknown;
        instrumentType       = InstrumentType.Price;

        if (GetType() == typeof(PQPricingInstrumentId)) SequenceId = 0;
    }

    public PQPricingInstrumentId
    (ushort sourceId, ushort tickerId
      , string sourceName
      , string ticker
      , DiscreetTimePeriod period
      , InstrumentType instrumentType
      , MarketClassification marketClassification
      , string? category = null
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown) 
        : base(sourceId, sourceName, tickerId, ticker)
    {
        CoveringPeriod       = period;
        MarketClassification = marketClassification;
        Category             = category;
        InstrumentType       = instrumentType;

        SourcePublishLocation  = sourcePublishLocation;
        AdapterReceiveLocation = adapterReceiveLocation;
        ClientReceiveLocation  = clientReceiveLocation;

        if (GetType() == typeof(PQPricingInstrumentId)) SequenceId = 0;
    }

    public PQPricingInstrumentId(IPricingInstrumentId toClone) : base(toClone)
    {
        CoveringPeriod       = toClone.CoveringPeriod;
        MarketClassification = toClone.MarketClassification;
        Category             = toClone.Category;
        InstrumentType       = toClone.InstrumentType;

        SourcePublishLocation  = toClone.SourcePublishLocation;
        AdapterReceiveLocation = toClone.AdapterReceiveLocation;
        ClientReceiveLocation  = toClone.ClientReceiveLocation;

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQPricingInstrumentId)) SequenceId = 0;
    }

    public PQPricingInstrumentId(SourceTickerIdentifier toClone) : base(toClone)
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        marketClassification = MarketClassification.Unknown;
        instrumentType       = InstrumentType.Price;

        if (GetType() == typeof(PQPricingInstrumentId)) SequenceId = 0;
    }

    public PQPricingInstrumentId(SourceTickerIdValue toClone) : base(toClone)
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        marketClassification = MarketClassification.Unknown;
        instrumentType       = InstrumentType.Price;

        if (GetType() == typeof(PQPricingInstrumentId)) SequenceId = 0;
    }

    public DiscreetTimePeriod CoveringPeriod { get; set; } = new(TimeBoundaryPeriod.Tick);

    string IInstrument.InstrumentName => InstrumentName;
    string IInstrument.SourceName     => SourceName;

    public InstrumentType InstrumentType
    {
        get => instrumentType ?? InstrumentType.Price;
        set => instrumentType = value;
    }

    public MarketClassification MarketClassification
    {
        get => marketClassification;
        set
        {
            if (Equals(marketClassification, value)) return;
            IsMarketClassificationUpdated = true;
            marketClassification          = value;
        }
    }

    public string? Category { get; set; }

    public CountryCityCodes SourcePublishLocation
    {
        get => sourcePublishLocation;
        set
        {
            IsSourcePublicationLocationUpdated |= value != sourcePublishLocation || SequenceId == 0;
            sourcePublishLocation              =  value;
        }
    }

    public CountryCityCodes AdapterReceiveLocation
    {
        get => adapterReceiveLocation;
        set
        {
            IsAdapterReceiveLocationUpdated |= value != adapterReceiveLocation || SequenceId == 0;
            adapterReceiveLocation          =  value;
        }
    }

    public CountryCityCodes ClientReceiveLocation
    {
        get => clientReceiveLocation;
        set
        {
            IsClientReceiveLocationUpdated |= value != clientReceiveLocation || SequenceId == 0;
            clientReceiveLocation          =  value;
        }
    }

    public bool IsSourcePublicationLocationUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.SourcePublishLocation) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.SourcePublishLocation;

            else if (IsSourcePublicationLocationUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.SourcePublishLocation;
        }
    }

    public bool IsAdapterReceiveLocationUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.AdapterReceiveLocation) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.AdapterReceiveLocation;

            else if (IsAdapterReceiveLocationUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.AdapterReceiveLocation;
        }
    }

    public bool IsClientReceiveLocationUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.ClientReceiveLocation) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.ClientReceiveLocation;

            else if (IsClientReceiveLocationUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.ClientReceiveLocation;
        }
    }

    public bool IsMarketClassificationUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.MarketClassification) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.MarketClassification;

            else if (IsMarketClassificationUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.MarketClassification;
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
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType)
                                                            , MarketClassification.ProductType.SquashProductTypeString());
            if (MarketClassification.MarketRegion != MarketRegion.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
            if (SourcePublishLocation != CountryCityCodes.Unknown || AdapterReceiveLocation != CountryCityCodes.Unknown ||
                ClientReceiveLocation != CountryCityCodes.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRoute), JoinMarketRouteCountryCityCodes());
        }
    }

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
            if (SourcePublishLocation != CountryCityCodes.Unknown || AdapterReceiveLocation != CountryCityCodes.Unknown ||
                ClientReceiveLocation != CountryCityCodes.Unknown)
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
            if (SourcePublishLocation != CountryCityCodes.Unknown || AdapterReceiveLocation != CountryCityCodes.Unknown ||
                ClientReceiveLocation != CountryCityCodes.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRoute), JoinMarketRouteCountryCityCodes());
        }
    }

    public bool HasAllRequiredKeys =>
        SourceName.IsNotNullOrEmpty()
     && MarketClassification.AssetType != AssetType.Unknown
     && MarketClassification.ProductType != ProductType.Unknown
     && MarketClassification.MarketRegion != MarketRegion.Unknown;

    public override bool IsEmpty
    {
        get => base.IsEmpty && Equals(MarketClassification, MarketClassification.Unknown);
        set => base.IsEmpty = value;
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

    public override PQSourceTickerId ResetWithTracking()
    {
        MarketClassification = MarketClassification.Unknown;
        base.ResetWithTracking();

        return this;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, PQMessageFlags updateStyle,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, updateStyle, quotePublicationPrecisionSettings))
            yield return deltaUpdateField;
        var fullPicture = updateStyle.HasCompleteFlag();

        if (fullPicture || IsMarketClassificationUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.MarketClassification
                                         , MarketClassification.CompoundedClassification);
        
        var includeReceiverTimes = updateStyle.HasIncludeReceiverTimesFlag();
        if (fullPicture || IsSourcePublicationLocationUpdated || IsAdapterReceiveLocationUpdated || (includeReceiverTimes && IsClientReceiveLocationUpdated))
        {
            var setFlags = IsSourcePublicationLocationUpdated || fullPicture && SourcePublishLocation != CountryCityCodes.Unknown
                ? CompoundLocationSetFlags.SourcePublishSet 
                : CompoundLocationSetFlags.None;
            setFlags |= IsAdapterReceiveLocationUpdated || fullPicture && AdapterReceiveLocation != CountryCityCodes.Unknown
                ? CompoundLocationSetFlags.AdapterReceiveSet 
                : CompoundLocationSetFlags.None;
            setFlags |= includeReceiverTimes && (IsClientReceiveLocationUpdated || fullPicture && ClientReceiveLocation != CountryCityCodes.Unknown)
                ? CompoundLocationSetFlags.ClientReceiveSet 
                : CompoundLocationSetFlags.None;
            var payload = this.CompoundLocations(setFlags);

            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.MarketRoute, payload);
        }
    }

    public override int UpdateField(PQFieldUpdate fieldUpdate)
    {
        if (fieldUpdate.Id == PQFeedFields.SourceTickerDefinition)
        {
            switch (fieldUpdate.DefinitionSubId)
            {
                case PQTickerDefSubFieldKeys.MarketClassification:
                    MarketClassification = new MarketClassification(fieldUpdate.Payload);
                    return 0;
                case PQTickerDefSubFieldKeys.MarketRoute:
                    var payload  = fieldUpdate.Payload;
                    var setFlags = (CompoundLocationSetFlags)(payload & 0xFF);
                    payload >>= 8;
                    if ((setFlags & CompoundLocationSetFlags.SourcePublishSet) > 0)
                    {
                        SourcePublishLocation              = (CountryCityCodes)(payload & 0xFF);
                        IsSourcePublicationLocationUpdated = true;
                    }
                    payload >>= 8;
                    if ((setFlags & CompoundLocationSetFlags.AdapterReceiveSet) > 0)
                    {
                        AdapterReceiveLocation          = (CountryCityCodes)(payload & 0xFF);
                        IsAdapterReceiveLocationUpdated = true;
                    }
                    payload >>= 8;
                    if ((setFlags & CompoundLocationSetFlags.ClientReceiveSet) > 0)
                    {
                        ClientReceiveLocation = (CountryCityCodes)(payload & 0xFF);
                        IsClientReceiveLocationUpdated = true;
                    }

                    return 0;
            }
        }

        return base.UpdateField(fieldUpdate);
    }

    public override PQPricingInstrumentId CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPQPricingInstrumentId pqPricingInstrumentId)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqPricingInstrumentId.IsMarketClassificationUpdated || isFullReplace)
            {
                IsMarketClassificationUpdated = true;

                MarketClassification = pqPricingInstrumentId.MarketClassification;
            }
            if (pqPricingInstrumentId.IsSourcePublicationLocationUpdated || isFullReplace)
            {
                IsSourcePublicationLocationUpdated = true;

                SourcePublishLocation = pqPricingInstrumentId.SourcePublishLocation;
            }
            if (pqPricingInstrumentId.IsAdapterReceiveLocationUpdated || isFullReplace)
            {
                IsAdapterReceiveLocationUpdated = true;

                AdapterReceiveLocation = pqPricingInstrumentId.AdapterReceiveLocation;
            }
            if (pqPricingInstrumentId.IsClientReceiveLocationUpdated || isFullReplace)
            {
                IsClientReceiveLocationUpdated = true;

                ClientReceiveLocation = pqPricingInstrumentId.ClientReceiveLocation;
            }

            CoveringPeriod = pqPricingInstrumentId.CoveringPeriod;
            InstrumentType = pqPricingInstrumentId.InstrumentType;

            if (isFullReplace) SetFlagsSame(pqPricingInstrumentId);
        }
        else if (source is IPricingInstrumentId pricingInstrumentId)
        {
            CoveringPeriod         = pricingInstrumentId.CoveringPeriod;
            MarketClassification   = pricingInstrumentId.MarketClassification;
            InstrumentType         = pricingInstrumentId.InstrumentType;
            SourcePublishLocation  = pricingInstrumentId.SourcePublishLocation;
            AdapterReceiveLocation = pricingInstrumentId.AdapterReceiveLocation;
            ClientReceiveLocation  = pricingInstrumentId.ClientReceiveLocation;
        }
        else if (source is IInstrument instrument)
        {
            foreach (var instrumentFields in instrument.FilledAttributes) this[instrumentFields.Key] = instrumentFields.Value;
        }
        return this;
    }

    IPricingInstrumentId ITransferState<IPricingInstrumentId>.CopyFrom(IPricingInstrumentId source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IPricingInstrumentId ICloneable<IPricingInstrumentId>.Clone() => Clone();

    IPricingInstrumentId IPricingInstrumentId.Clone() => Clone();

    public override IPQPricingInstrumentId Clone() =>
        Recycler?.Borrow<PQPricingInstrumentId>().CopyFrom(this) as PQPricingInstrumentId ?? new PQPricingInstrumentId(this);

    IReusableObject<IPricingInstrumentId> ITransferState<IReusableObject<IPricingInstrumentId>>.CopyFrom
        (IReusableObject<IPricingInstrumentId> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerId)source, copyMergeFlags);

    public override bool AreEquivalent(ISourceTickerId? other, bool exactTypes = false)
    {
        if (other is not IPricingInstrumentId pricingInstrumentId) return false;
        if (exactTypes && pricingInstrumentId is not IPQPricingInstrumentId) return false;
        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var marketClassificationSame = Equals(MarketClassification, pricingInstrumentId.MarketClassification);

        var publishLocationSame        = true;
        var adapterReceiveLocationSame = true;
        var clientReceiveLocationSame  = true;
        var coveringPeriodSame         = true;
        var instrumentTypeSame         = true;
        if (exactTypes)
        {
            var pricingInstrument = other as IPQPricingInstrumentId;
            coveringPeriodSame         = CoveringPeriod == pricingInstrument?.CoveringPeriod;
            instrumentTypeSame         = InstrumentType == pricingInstrument?.InstrumentType;
            publishLocationSame        = SourcePublishLocation == pricingInstrumentId.SourcePublishLocation;
            adapterReceiveLocationSame = AdapterReceiveLocation == pricingInstrumentId.AdapterReceiveLocation;
            clientReceiveLocationSame  = ClientReceiveLocation == pricingInstrumentId.ClientReceiveLocation;
        }

        var allAreSame = baseIsSame && marketClassificationSame && coveringPeriodSame && instrumentTypeSame
                      && publishLocationSame && adapterReceiveLocationSame && clientReceiveLocationSame;
        return allAreSame;
    }

    protected string PQPricingInstrumentIdToStringMembers =>
        $"{PQSourceTickerIdToStringMembers}, {nameof(CoveringPeriod)}: {CoveringPeriod}, {nameof(InstrumentType)}: {InstrumentType}, " +
        $"{nameof(MarketClassification)}: {MarketClassification}, {nameof(Category)}: {Category}, {nameof(SourcePublishLocation)}: {SourcePublishLocation}, " +
        $"{nameof(AdapterReceiveLocation)}: {AdapterReceiveLocation}, {nameof(ClientReceiveLocation)}: {ClientReceiveLocation}";

    public override string ToString() => $"{nameof(PQPricingInstrumentId)}{{{PQPricingInstrumentIdToStringMembers}, {UpdateFlagsToString}}}";
}
