// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

public interface IPQPricingInstrumentId : IPQSourceTickerId, IPricingInstrumentId, ICloneable<IPQPricingInstrumentId>
{
    bool IsMarketClassificationUpdated { get; set; }

    new ushort SourceId { get; set; }
    new ushort TickerId { get; set; }
    new string Source   { get; set; }
    new string Ticker   { get; set; }

    new IPQPricingInstrumentId Clone();
}

public class PQPricingInstrument : PQSourceTickerId, IPQPricingInstrumentId
{
    private MarketClassification marketClassification;

    private string[]? optionalKeys;
    private string[]? requiredKeys;

    private InstrumentType? timeSeriesType;
    public PQPricingInstrument() { }

    public PQPricingInstrument
    (ushort sourceId, ushort tickerId, string source, string ticker, TimeSeriesPeriod period, InstrumentType instrumentType
      , MarketClassification marketClassification, string? category = null) : base(sourceId, source, tickerId, ticker)
    {
        EntryPeriod          = period;
        MarketClassification = marketClassification;
        Category             = category;
        InstrumentType       = instrumentType;
    }

    public PQPricingInstrument(IPricingInstrumentId toClone) : base(toClone)
    {
        EntryPeriod          = toClone.EntryPeriod;
        MarketClassification = toClone.MarketClassification;
        Category             = toClone.Category;
        InstrumentType       = toClone.InstrumentType;

        foreach (var instrumentFields in toClone) this[instrumentFields.Key] = instrumentFields.Value;

        if (toClone is IPQPricingInstrumentId pubToClone) IsMarketClassificationUpdated = pubToClone.IsMarketClassificationUpdated;
    }

    string IInstrument.InstrumentName   => Ticker;
    string IInstrument.InstrumentSource => Source;

    public InstrumentType InstrumentType
    {
        get => timeSeriesType ?? InstrumentType.Price;
        set => timeSeriesType = value;
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

    public TimeSeriesPeriod EntryPeriod { get; set; } = TimeSeriesPeriod.Tick;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
        if (MarketClassification.MarketType != MarketType.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
        if (MarketClassification.ProductType != ProductType.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.ToString());
        if (MarketClassification.MarketRegion != MarketRegion.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
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
        Source.IsNotNullOrEmpty()
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
            yield return new PQFieldUpdate(PQFieldKeys.MarketClassification, MarketClassification.CompoundedClassification);
    }

    public override int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQFieldKeys.MarketClassification:
                MarketClassification = new MarketClassification(fieldUpdate.Value);
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
            EntryPeriod          = source.EntryPeriod;
            MarketClassification = source.MarketClassification;
            InstrumentType       = source.InstrumentType;
        }
        foreach (var instrumentFields in source) this[instrumentFields.Key] = instrumentFields.Value;

        return this;
    }

    IPricingInstrumentId ICloneable<IPricingInstrumentId>.Clone() => Clone();

    IPricingInstrumentId IPricingInstrumentId.Clone() => Clone();

    public override IPQPricingInstrumentId Clone() =>
        Recycler?.Borrow<PQPricingInstrument>().CopyFrom(this) as PQPricingInstrument ?? new PQPricingInstrument(this);

    IReusableObject<IPricingInstrumentId> IStoreState<IReusableObject<IPricingInstrumentId>>.CopyFrom
        (IReusableObject<IPricingInstrumentId> source, CopyMergeFlags copyMergeFlags) =>
        (IPQPricingInstrumentId)CopyFrom(source, copyMergeFlags);

    public override bool AreEquivalent(ISourceTickerQuoteInfo? other, bool exactTypes = false)
    {
        if (other == null || (exactTypes && other is not IPQSourceTickerQuoteInfo srcTkrQtInfo)) return false;

        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var marketClassificationSame = Equals(MarketClassification, other.MarketClassification);

        var entryPeriodSame    = true;
        var instrumentTypeSame = true;
        if (exactTypes)
        {
            var pricingInstrument = other as IPQPricingInstrumentId;
            entryPeriodSame    = EntryPeriod == pricingInstrument?.EntryPeriod;
            instrumentTypeSame = InstrumentType == pricingInstrument?.InstrumentType;
        }

        var allAreSame = baseIsSame && marketClassificationSame && entryPeriodSame && instrumentTypeSame;
        return allAreSame;
    }
}
