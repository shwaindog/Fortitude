// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;

public interface IPQPricingInstrumentId : IPQSourceTickerId, IPricingInstrumentId, ICloneable<IPQPricingInstrumentId>
{
    bool IsMarketClassificationUpdated { get; set; }

    new ushort SourceId       { get; set; }
    new ushort InstrumentId   { get; set; }
    new string SourceName     { get; set; }
    new string InstrumentName { get; set; }

    new IPQPricingInstrumentId Clone();
}

public class PQPricingInstrument : PQSourceTickerId, IPQPricingInstrumentId
{
    private InstrumentType?      instrumentType;
    private MarketClassification marketClassification;

    private string[]? optionalKeys;
    private string[]? requiredKeys;

    public PQPricingInstrument()
    {
        marketClassification = MarketClassificationExtensions.Unknown;
        instrumentType       = InstrumentType.Price;
    }

    public PQPricingInstrument
    (ushort sourceId, ushort tickerId, string sourceName, string ticker, DiscreetTimePeriod period, InstrumentType instrumentType
      , MarketClassification marketClassification, string? category = null) : base(sourceId, sourceName, tickerId, ticker)
    {
        CoveringPeriod       = period;
        MarketClassification = marketClassification;
        Category             = category;
        InstrumentType       = instrumentType;
    }

    public PQPricingInstrument(IPricingInstrumentId toClone) : base(toClone)
    {
        CoveringPeriod       = toClone.CoveringPeriod;
        MarketClassification = toClone.MarketClassification;
        Category             = toClone.Category;
        InstrumentType       = toClone.InstrumentType;

        foreach (var instrumentFields in toClone.FilledAttributes) this[instrumentFields.Key] = instrumentFields.Value;

        if (toClone is IPQPricingInstrumentId pubToClone) IsMarketClassificationUpdated = pubToClone.IsMarketClassificationUpdated;
    }

    public PQPricingInstrument(SourceTickerIdentifier toClone) : base(toClone)
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        marketClassification = MarketClassificationExtensions.Unknown;
        instrumentType       = InstrumentType.Price;
    }

    public PQPricingInstrument(SourceTickerIdValue toClone) : base(toClone)
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        marketClassification = MarketClassificationExtensions.Unknown;
        instrumentType       = InstrumentType.Price;
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
            if (MarketClassification.MarketType != MarketType.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
            if (MarketClassification.ProductType != ProductType.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType)
                                                            , MarketClassification.ProductType.ToString());
            if (MarketClassification.MarketRegion != MarketRegion.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
        }
    }

    public string? this[string key]
    {
        get
        {
            switch (key)
            {
                case nameof(RepositoryPathName.MarketType):        return MarketClassification.MarketType.ToString();
                case nameof(RepositoryPathName.MarketProductType): return MarketClassification.ProductType.ToString();
                case nameof(RepositoryPathName.MarketRegion):      return MarketClassification.MarketRegion.ToString();
                case nameof(RepositoryPathName.Category):          return Category;
            }
            return null;
        }
        set
        {
            switch (key)
            {
                case nameof(RepositoryPathName.MarketType):
                    if (Enum.TryParse<MarketType>(value, true, out var marketType))
                        if (MarketClassification.MarketType == MarketType.Unknown)
                            MarketClassification = MarketClassification.SetMarketType(marketType);
                    break;
                case nameof(RepositoryPathName.MarketProductType):
                    if (Enum.TryParse<ProductType>(value, true, out var productType))
                        if (MarketClassification.ProductType == ProductType.Unknown)
                            MarketClassification = MarketClassification.SetProductType(productType);
                    break;
                case nameof(RepositoryPathName.MarketRegion):
                    if (Enum.TryParse<MarketRegion>(value, true, out var marketRegion))
                        if (MarketClassification.MarketRegion == MarketRegion.Unknown)
                            MarketClassification = MarketClassification.SetMarketRegion(marketRegion);
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
            case nameof(RepositoryPathName.MarketType):
                MarketClassification = MarketClassification.SetMarketType(MarketType.Unknown);
                return true;
            case nameof(RepositoryPathName.MarketProductType):
                MarketClassification = MarketClassification.SetProductType(ProductType.Unknown);
                return true;
            case nameof(RepositoryPathName.MarketRegion):
                MarketClassification = MarketClassification.SetMarketRegion(MarketRegion.Unknown);
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
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
            if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
        }
    }

    public IEnumerable<KeyValuePair<string, string>> RequiredAttributes
    {
        get
        {
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
        }
    }

    public IEnumerable<KeyValuePair<string, string>> OptionalAttributes
    {
        get
        {
            if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
        }
    }

    public bool HasAllRequiredKeys =>
        SourceName.IsNotNullOrEmpty()
     && MarketClassification.MarketType != MarketType.Unknown
     && MarketClassification.ProductType != ProductType.Unknown && MarketClassification.MarketRegion != MarketRegion.Unknown;


    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags updateStyle,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, updateStyle, quotePublicationPrecisionSettings))
            yield return deltaUpdateField;
        var updatedOnly = (updateStyle & StorageFlags.Complete) == 0;

        if (!updatedOnly || IsMarketClassificationUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.MarketClassification, MarketClassification.CompoundedClassification);
    }

    public override int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQQuoteFields.MarketClassification:
                MarketClassification = new MarketClassification(fieldUpdate.Payload);
                return 0;
        }

        return base.UpdateField(fieldUpdate);
    }

    public override IPQSourceTickerId CopyFrom(IPQSourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQPricingInstrumentId pqPricingInstrumentId) return (IPQSourceTickerId)CopyFrom(pqPricingInstrumentId, copyMergeFlags);
        return base.CopyFrom(source, copyMergeFlags);
    }

    public IPricingInstrumentId CopyFrom(IPricingInstrumentId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ((IPQSourceTickerId)this).CopyFrom(source, copyMergeFlags);
        if (source is IPQPricingInstrumentId pqPricingInstrumentId && copyMergeFlags == CopyMergeFlags.JustDifferences)
        {
            if (pqPricingInstrumentId.IsMarketClassificationUpdated) MarketClassification = pqPricingInstrumentId.MarketClassification;
        }
        else
        {
            CoveringPeriod       = source.CoveringPeriod;
            MarketClassification = source.MarketClassification;
            InstrumentType       = source.InstrumentType;
        }
        foreach (var instrumentFields in source.FilledAttributes) this[instrumentFields.Key] = instrumentFields.Value;

        return this;
    }

    IPricingInstrumentId ICloneable<IPricingInstrumentId>.Clone() => Clone();

    IPricingInstrumentId IPricingInstrumentId.Clone() => Clone();

    public override IPQPricingInstrumentId Clone() =>
        Recycler?.Borrow<PQPricingInstrument>().CopyFrom(this) as PQPricingInstrument ?? new PQPricingInstrument(this);

    IReusableObject<IPricingInstrumentId> ITransferState<IReusableObject<IPricingInstrumentId>>.CopyFrom
        (IReusableObject<IPricingInstrumentId> source, CopyMergeFlags copyMergeFlags) =>
        (IPQPricingInstrumentId)CopyFrom(source, copyMergeFlags);

    public override bool AreEquivalent(ISourceTickerInfo? other, bool exactTypes = false)
    {
        if (other == null || (exactTypes && other is not IPQSourceTickerInfo srcTkrInfo)) return false;

        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var marketClassificationSame = Equals(MarketClassification, other.MarketClassification);

        var coveringPeriodSame = true;
        var instrumentTypeSame = true;
        if (exactTypes)
        {
            var pricingInstrument = other as IPQPricingInstrumentId;
            coveringPeriodSame = CoveringPeriod == pricingInstrument?.CoveringPeriod;
            instrumentTypeSame = InstrumentType == pricingInstrument?.InstrumentType;
        }

        var allAreSame = baseIsSame && marketClassificationSame && coveringPeriodSame && instrumentTypeSame;
        return allAreSame;
    }
}
