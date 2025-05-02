// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQSourceQuoteRefOrdersValueDatePriceVolumeLayer : IPQOrdersPriceVolumeLayer,
    IPQValueDatePriceVolumeLayer, IPQSourceQuoteRefPriceVolumeLayer,
    IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer
{
    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQSourceQuoteRefOrdersValueDatePriceVolumeLayer Clone();
}

public class PQSourceQuoteRefOrdersValueDatePriceVolumeLayer : PQOrdersPriceVolumeLayer,
    IPQSourceQuoteRefOrdersValueDatePriceVolumeLayer
{
    protected LayerBooleanFlags LayerBooleanFlags;

    private ushort   sourceId;
    private uint     sourceQuoteReference;
    private DateTime valueDate = DateTimeConstants.UnixEpoch;

    public PQSourceQuoteRefOrdersValueDatePriceVolumeLayer(IPQNameIdLookupGenerator initialDict)
        : base(LayerType.SourceQuoteRefOrdersValueDatePriceVolume, initialDict)
    {
        if (GetType() == typeof(PQSourceQuoteRefOrdersValueDatePriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    public PQSourceQuoteRefOrdersValueDatePriceVolumeLayer
    (IPQNameIdLookupGenerator nameIdLookup, decimal price = 0m, decimal volume = 0m, DateTime? valueDate = null
      , string? sourceName = null, bool executable = false, uint sourceQuoteReference = 0u, uint ordersCount = 0, decimal internalVolume = 0m)
        : base(nameIdLookup, LayerType.SourceQuoteRefOrdersValueDatePriceVolume, price, volume, ordersCount, internalVolume)
    {
        ValueDate  = valueDate ?? DateTimeConstants.UnixEpoch;
        SourceName = sourceName;
        Executable = executable;

        SourceQuoteReference = sourceQuoteReference;

        if (GetType() == typeof(PQSourceQuoteRefOrdersValueDatePriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    public PQSourceQuoteRefOrdersValueDatePriceVolumeLayer(IPriceVolumeLayer toClone, IPQNameIdLookupGenerator ipNameIdLookupGenerator)
        : base(toClone, LayerType.SourceQuoteRefOrdersValueDatePriceVolume, ipNameIdLookupGenerator)
    {
        if (toClone is ISourcePriceVolumeLayer srcPvl)
        {
            SourceName = srcPvl.SourceName;
            Executable = srcPvl.Executable;
        }

        if (toClone is ISourceQuoteRefPriceVolumeLayer srcQtRefPvl) SourceQuoteReference = srcQtRefPvl.SourceQuoteReference;
        if (toClone is IValueDatePriceVolumeLayer valueDatePvl) ValueDate                = valueDatePvl.ValueDate;
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQSourceQuoteRefOrdersValueDatePriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQSourceQuoteRefTraderValueDatePriceVolumeLayerToStringMembers =>
        $"{PQOrdersCountVolumeLayerToStringMembers}, {nameof(SourceName)}: {SourceName}, {nameof(Executable)}: {Executable}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}, {nameof(ValueDate)}: {ValueDate}, {PQJustOrdersToStringMembers}";

    [JsonIgnore]
    public bool IsValueDateUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.ValueDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.ValueDateUpdatedFlag;

            else if (IsValueDateUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.ValueDateUpdatedFlag;
        }
    }

    [JsonIgnore] public override LayerType LayerType => LayerType.SourceQuoteRefOrdersValueDatePriceVolume;

    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalFullSupportLayerFlags | base.SupportsLayerFlags;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate
    {
        get => valueDate;
        set
        {
            IsValueDateUpdated |= valueDate != value || NumUpdatesSinceEmpty == 0;
            valueDate          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference
    {
        get => sourceQuoteReference;
        set
        {
            IsSourceQuoteReferenceUpdated |= sourceQuoteReference != value || NumUpdatesSinceEmpty == 0;
            sourceQuoteReference          =  value;
        }
    }

    [JsonIgnore]
    public bool IsSourceQuoteReferenceUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.SourceQuoteRefUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.SourceQuoteRefUpdatedFlag;

            else if (IsSourceQuoteReferenceUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.SourceQuoteRefUpdatedFlag;
        }
    }

    [JsonIgnore]
    public ushort SourceId
    {
        get => sourceId;
        set
        {
            IsSourceNameUpdated |= sourceId != value || NumUpdatesSinceEmpty == 0;
            sourceId            =  value;
        }
    }

    [JsonIgnore]
    public bool IsSourceNameUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.SourceNameUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.SourceNameUpdatedFlag;

            else if (IsSourceNameUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.SourceNameUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceName
    {
        get => NameIdLookup[SourceId];
        set
        {
            var dictionaryId = NameIdLookup.GetOrAddId(value);
            if (dictionaryId <= 0 && value != null)
                throw new Exception("Error attempted to set the Source Name to something " +
                                    "not defined in the source name to Id table.");
            SourceId = (ushort)dictionaryId;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Executable
    {
        get => (LayerBooleanFlags & LayerBooleanFlags.IsExecutableFlag) != 0;
        set
        {
            IsExecutableUpdated |= (LayerBooleanFlags & LayerBooleanFlags.IsExecutableFlag) > 0 != value || NumUpdatesSinceEmpty == 0;
            if (value)
                LayerBooleanFlags |= LayerBooleanFlags.IsExecutableFlag;

            else if (Executable) LayerBooleanFlags ^= LayerBooleanFlags.IsExecutableFlag;
        }
    }


    [JsonIgnore]
    public bool IsExecutableUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.ExecutableUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.ExecutableUpdatedFlag;

            else if (IsExecutableUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.ExecutableUpdatedFlag;
        }
    }

    [JsonIgnore]
    public override IPQNameIdLookupGenerator NameIdLookup
    {
        get => base.NameIdLookup;
        set
        {
            if (base.NameIdLookup == value) return;
            string? cacheSourceName           = null;
            if (sourceId > 0) cacheSourceName = SourceName;
            base.NameIdLookup = value;
            if (sourceId > 0) sourceId = (ushort)base.NameIdLookup.GetOrAddId(cacheSourceName);
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get =>
            base.IsEmpty && ValueDate == DateTimeConstants.UnixEpoch
                         && SourceName == null && SourceQuoteReference == 0u && !Executable;
        set
        {
            if (!value) return;

            ValueDate  = DateTimeConstants.UnixEpoch;
            SourceName = null;
            Executable = false;

            SourceQuoteReference = 0;

            base.IsEmpty = true;
        }
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates || NameIdLookup!.HasUpdates;
        set
        {
            base.HasUpdates          = value;
            NameIdLookup!.HasUpdates = value;
        }
    }

    public override void UpdateComplete()
    {
        NameIdLookup.UpdateComplete();
        base.UpdateComplete();
    }

    public override void StateReset()
    {
        ValueDate  = DateTimeConstants.UnixEpoch;
        SourceName = null;
        Executable = false;

        SourceQuoteReference = 0u;

        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsValueDateUpdated) yield return new PQFieldUpdate(PQQuoteFields.LayerValueDate, valueDate.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsSourceNameUpdated) yield return new PQFieldUpdate(PQQuoteFields.SourceId, SourceId);
        if (!updatedOnly || IsExecutableUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.LayerBooleanFlags
                                         , (uint)(Executable ? LayerBooleanFlags.IsExecutableFlag : LayerBooleanFlags.None));
        if (!updatedOnly || IsSourceQuoteReferenceUpdated) yield return new PQFieldUpdate(PQQuoteFields.LayerSourceQuoteRef, sourceQuoteReference);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id == PQQuoteFields.LayerValueDate)
        {
            var originalValueDate = valueDate;
            PQFieldConverters.UpdateHoursFromUnixEpoch(ref valueDate, pqFieldUpdate.Payload);
            IsValueDateUpdated = originalValueDate != valueDate;
            return 0;
        }

        if (pqFieldUpdate.Id == PQQuoteFields.SourceId)
        {
            IsSourceNameUpdated = true; // incase of reset and sending 0;
            SourceId            = (ushort)pqFieldUpdate.Payload;
            return 0;
        }

        if (pqFieldUpdate.Id == PQQuoteFields.LayerBooleanFlags)
        {
            IsExecutableUpdated = true; // incase of reset and sending 0;
            Executable          = ((LayerBooleanFlags)pqFieldUpdate.Payload & LayerBooleanFlags.IsExecutableFlag) != 0;
            return 0;
        }

        if (pqFieldUpdate.Id == PQQuoteFields.LayerSourceQuoteRef)
        {
            IsSourceQuoteReferenceUpdated = true; // incase of reset and sending 0;
            SourceQuoteReference          = pqFieldUpdate.Payload;
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var baseUpdates in base.GetStringUpdates(snapShotTime, messageFlags)) yield return baseUpdates;
        if (NameIdLookup is { } pqNameIdLookupGenerator)
            foreach (var stringUpdate in pqNameIdLookupGenerator.GetStringUpdates(snapShotTime, messageFlags))
                yield return stringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        base.UpdateFieldString(stringUpdate);
        if (stringUpdate.Field.Id != PQQuoteFields.LayerNameDictionaryUpsertCommand) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public override IPriceVolumeLayer CopyFrom
        (IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var isFullReplace = copyMergeFlags.HasFullReplace();
        switch (source)
        {
            // IPQOrdersPriceVolumeLayer && IPQOrdersCountPriceVolumeLayer are done in base
            case IPQSourceQuoteRefOrdersValueDatePriceVolumeLayer srcQtRefTrdrVlDtPv:
                NameIdLookup.CopyFrom(srcQtRefTrdrVlDtPv.NameIdLookup);
                if (srcQtRefTrdrVlDtPv.IsValueDateUpdated || isFullReplace) ValueDate                       = srcQtRefTrdrVlDtPv.ValueDate;
                if (srcQtRefTrdrVlDtPv.IsSourceQuoteReferenceUpdated || isFullReplace) SourceQuoteReference = srcQtRefTrdrVlDtPv.SourceQuoteReference;
                if (srcQtRefTrdrVlDtPv.IsSourceNameUpdated || isFullReplace)
                    SourceId = (ushort)NameIdLookup.GetOrAddId(srcQtRefTrdrVlDtPv.SourceName);
                if (srcQtRefTrdrVlDtPv.IsExecutableUpdated || isFullReplace) Executable = srcQtRefTrdrVlDtPv.Executable;
                if (isFullReplace) SetFlagsSame(srcQtRefTrdrVlDtPv);
                break;
            case IPQSourceQuoteRefPriceVolumeLayer pqSrcQtRefPvLayer:
                NameIdLookup.CopyFrom(pqSrcQtRefPvLayer.NameIdLookup);
                if (pqSrcQtRefPvLayer.IsSourceQuoteReferenceUpdated || isFullReplace) SourceQuoteReference = pqSrcQtRefPvLayer.SourceQuoteReference;
                if (pqSrcQtRefPvLayer.IsSourceNameUpdated || isFullReplace) SourceId = (ushort)NameIdLookup.GetOrAddId(pqSrcQtRefPvLayer.SourceName);
                if (pqSrcQtRefPvLayer.IsExecutableUpdated || isFullReplace) Executable = pqSrcQtRefPvLayer.Executable;
                if (isFullReplace) SetFlagsSame(pqSrcQtRefPvLayer);
                break;
            case IPQSourcePriceVolumeLayer pqSourcePvLayer:
                NameIdLookup.CopyFrom(pqSourcePvLayer.NameIdLookup);
                if (pqSourcePvLayer.IsSourceNameUpdated || isFullReplace) SourceId   = (ushort)NameIdLookup.GetOrAddId(pqSourcePvLayer.SourceName);
                if (pqSourcePvLayer.IsExecutableUpdated || isFullReplace) Executable = pqSourcePvLayer.Executable;
                if (isFullReplace) SetFlagsSame(pqSourcePvLayer);
                break;
            case IPQValueDatePriceVolumeLayer pqValueDate:
                if (pqValueDate.IsValueDateUpdated || isFullReplace) ValueDate = pqValueDate.ValueDate;
                if (isFullReplace) SetFlagsSame(pqValueDate);
                break;
            case ISourceQuoteRefOrdersValueDatePriceVolumeLayer srcQtRefTrdrVlPvLayer:

                ValueDate  = srcQtRefTrdrVlPvLayer.ValueDate;
                SourceName = srcQtRefTrdrVlPvLayer.SourceName;
                Executable = srcQtRefTrdrVlPvLayer.Executable;

                SourceQuoteReference = srcQtRefTrdrVlPvLayer.SourceQuoteReference;
                break;
            case ISourceQuoteRefPriceVolumeLayer srcQtRefPvLayer:
                SourceName = srcQtRefPvLayer.SourceName;
                Executable = srcQtRefPvLayer.Executable;

                SourceQuoteReference = srcQtRefPvLayer.SourceQuoteReference;
                break;
            case ISourcePriceVolumeLayer srcPvLayer:
                SourceName = srcPvLayer.SourceName;
                Executable = srcPvLayer.Executable;
                break;
            case IValueDatePriceVolumeLayer valueDatePvLayer: ValueDate = valueDatePvLayer.ValueDate; break;
        }

        return this;
    }

    IPQSourceQuoteRefOrdersValueDatePriceVolumeLayer IPQSourceQuoteRefOrdersValueDatePriceVolumeLayer.Clone() => Clone();

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => Clone();

    IPQSourcePriceVolumeLayer IPQSourcePriceVolumeLayer.Clone() => Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => Clone();

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    ISourceQuoteRefOrdersValueDatePriceVolumeLayer ICloneable<ISourceQuoteRefOrdersValueDatePriceVolumeLayer>.Clone() => Clone();

    ISourceQuoteRefOrdersValueDatePriceVolumeLayer ISourceQuoteRefOrdersValueDatePriceVolumeLayer.Clone() => Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() => Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer ICloneable<IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer>.Clone() => Clone();

    IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => Clone();

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => Clone();

    IPQValueDatePriceVolumeLayer IPQValueDatePriceVolumeLayer.Clone() => Clone();

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvLayer)) return false;

        var baseSame           = base.AreEquivalent(other, exactTypes);
        var valueDateSame      = ValueDate == srcQtRefTrdrVlDtPvLayer.ValueDate;
        var sourceQuoteRefSame = SourceQuoteReference == srcQtRefTrdrVlDtPvLayer.SourceQuoteReference;
        var sourceNameSame     = SourceName == srcQtRefTrdrVlDtPvLayer.SourceName;
        var executableSame     = Executable == srcQtRefTrdrVlDtPvLayer.Executable;

        var allAreSame = baseSame && valueDateSame && sourceQuoteRefSame && sourceNameSame && executableSame;
        return allAreSame;
    }

    public override PQSourceQuoteRefOrdersValueDatePriceVolumeLayer Clone() => new(this, NameIdLookup);


    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ sourceId.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)sourceQuoteReference;
            hashCode = (hashCode * 397) ^ valueDate.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{GetType().Name}({PQSourceQuoteRefTraderValueDatePriceVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
