// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQSourceQuoteRefTraderValueDatePriceVolumeLayer : IPQTraderPriceVolumeLayer,
    IPQValueDatePriceVolumeLayer, IPQSourceQuoteRefPriceVolumeLayer,
    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
{
    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQSourceQuoteRefTraderValueDatePriceVolumeLayer Clone();
}

public class PQSourceQuoteRefTraderValueDatePriceVolumeLayer : PQTraderPriceVolumeLayer,
    IPQSourceQuoteRefTraderValueDatePriceVolumeLayer
{
    protected LayerBooleanFlags LayerBooleanFlags;

    private ushort   sourceId;
    private uint     sourceQuoteReference;
    private DateTime valueDate = DateTimeConstants.UnixEpoch;

    public PQSourceQuoteRefTraderValueDatePriceVolumeLayer(IPQNameIdLookupGenerator initialDict)
        : base(initialDict) { }

    public PQSourceQuoteRefTraderValueDatePriceVolumeLayer
    (IPQNameIdLookupGenerator nameIdLookup, decimal price = 0m, decimal volume = 0m, DateTime? valueDate = null
      , string? sourceName = null, bool executable = false, uint sourceQuoteReference = 0u)
        : base(nameIdLookup, price, volume)
    {
        ValueDate  = valueDate ?? DateTimeConstants.UnixEpoch;
        SourceName = sourceName;
        Executable = executable;

        SourceQuoteReference = sourceQuoteReference;
    }

    public PQSourceQuoteRefTraderValueDatePriceVolumeLayer(IPriceVolumeLayer toClone, IPQNameIdLookupGenerator ipNameIdLookupGenerator)
        : base(toClone, ipNameIdLookupGenerator)
    {
        if (toClone is ISourcePriceVolumeLayer srcPvl)
        {
            SourceName = srcPvl.SourceName;
            Executable = srcPvl.Executable;
        }

        if (toClone is ISourceQuoteRefPriceVolumeLayer srcQtRefPvl) SourceQuoteReference = srcQtRefPvl.SourceQuoteReference;
        if (toClone is IValueDatePriceVolumeLayer valueDatePvl) ValueDate                = valueDatePvl.ValueDate;
        SetFlagsSame(toClone);
    }

    protected string PQSourceQuoteRefTraderValueDatePriceVolumeLayerToStringMembers =>
        $"{nameof(Price)}: {Price:N5}, " +
        $"{nameof(Volume)}: {Volume:N2}, {nameof(SourceName)}: {SourceName}, {nameof(Executable)}: {Executable}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}, {nameof(ValueDate)}: {ValueDate}, " +
        $"{nameof(Count)}: {Count} {PQTraderPriceVolumeLayerToStringMembers}";

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

    [JsonIgnore] public override LayerType LayerType => LayerType.SourceQuoteRefTraderValueDatePriceVolume;

    [JsonIgnore]
    public override LayerFlags SupportsLayerFlags =>
        LayerFlags.SourceQuoteReference | LayerFlags.SourceName
                                        | LayerFlags.Executable | LayerFlags.ValueDate | base.SupportsLayerFlags;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate
    {
        get => valueDate;
        set
        {
            if (valueDate == value) return;
            IsValueDateUpdated = true;
            valueDate          = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference
    {
        get => sourceQuoteReference;
        set
        {
            if (sourceQuoteReference == value) return;
            IsSourceQuoteReferenceUpdated = true;
            sourceQuoteReference          = value;
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
            if (SourceId == value) return;
            IsSourceNameUpdated = true;
            sourceId            = value;
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
            if (Executable == value) return;
            IsExecutableUpdated = true;
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
        if (!updatedOnly || IsValueDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.FirstLayerDateOffset, valueDate.GetHoursFromUnixEpoch(),
                                           PQFieldFlags.IsExtendedFieldId);
        if (!updatedOnly || IsSourceNameUpdated) yield return new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset, SourceId);
        if (!updatedOnly || IsExecutableUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerBooleanFlagsOffset,
                                           Executable ? PQFieldFlags.LayerExecutableFlag : 0);
        if (!updatedOnly || IsSourceQuoteReferenceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset, sourceQuoteReference);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id >= PQFieldKeys.FirstLayerDateOffset && pqFieldUpdate.Id <
            PQFieldKeys.FirstLayerDateOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            var originalValueDate = valueDate;
            PQFieldConverters.UpdateHoursFromUnixEpoch(ref valueDate, pqFieldUpdate.Value);
            IsValueDateUpdated = originalValueDate != valueDate;
            return 0;
        }

        if (pqFieldUpdate.Id >= PQFieldKeys.LayerSourceIdOffset && pqFieldUpdate.Id <
            PQFieldKeys.LayerSourceIdOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            SourceId = (ushort)pqFieldUpdate.Value;
            return 0;
        }

        if (pqFieldUpdate.Id >= PQFieldKeys.LayerBooleanFlagsOffset && pqFieldUpdate.Id <
            PQFieldKeys.LayerBooleanFlagsOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            Executable = (pqFieldUpdate.Value & PQFieldFlags.LayerExecutableFlag) != 0;
            return 0;
        }

        if (pqFieldUpdate.Id >= PQFieldKeys.LayerSourceQuoteRefOffset && pqFieldUpdate.Id <
            PQFieldKeys.LayerSourceQuoteRefOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            SourceQuoteReference = pqFieldUpdate.Value;
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var baseUpdates in base.GetStringUpdates(snapShotTime, messageFlags)) yield return baseUpdates;
        if (NameIdLookup is IPQNameIdLookupGenerator pqNameIdLookupGenerator)
            foreach (var stringUpdate in pqNameIdLookupGenerator.GetStringUpdates(snapShotTime, messageFlags))
                yield return stringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        base.UpdateFieldString(stringUpdate);
        if (stringUpdate.Field.Id != PQFieldKeys.LayerNameDictionaryUpsertCommand) return false;
        if (NameIdLookup != null) return NameIdLookup.UpdateFieldString(stringUpdate);
        return false;
    }

    public override IPriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var isFullReplace = copyMergeFlags.HasFullReplace();
        switch (source)
        {
            case IPQSourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPv:
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
            case ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlPvLayer:

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

    IPQSourceQuoteRefTraderValueDatePriceVolumeLayer IPQSourceQuoteRefTraderValueDatePriceVolumeLayer.Clone() =>
        (IPQSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.Clone() => (IPQSourceQuoteRefPriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => (ISourcePriceVolumeLayer)Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => (IMutableSourcePriceVolumeLayer)Clone();

    IPQSourcePriceVolumeLayer IPQSourcePriceVolumeLayer.Clone() => (IPQSourcePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => (ISourcePriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefTraderValueDatePriceVolumeLayer
        ICloneable<ISourceQuoteRefTraderValueDatePriceVolumeLayer>.Clone() =>
        (ISourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    ISourceQuoteRefTraderValueDatePriceVolumeLayer ISourceQuoteRefTraderValueDatePriceVolumeLayer.Clone() =>
        (ISourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
        ICloneable<IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
        IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer.Clone() =>
        (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => (IMutableValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => (IMutableValueDatePriceVolumeLayer)Clone();

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => (IValueDatePriceVolumeLayer)Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => (IValueDatePriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(this, NameIdLookup);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvLayer)) return false;

        var baseSame           = base.AreEquivalent(other, exactTypes);
        var valueDateSame      = ValueDate == srcQtRefTrdrVlDtPvLayer.ValueDate;
        var sourceQuoteRefSame = SourceQuoteReference == srcQtRefTrdrVlDtPvLayer.SourceQuoteReference;
        var sourceNameSame     = SourceName == srcQtRefTrdrVlDtPvLayer.SourceName;
        var executableSame     = Executable == srcQtRefTrdrVlDtPvLayer.Executable;

        var allAreSame = baseSame && valueDateSame && sourceQuoteRefSame && sourceNameSame && executableSame;
        return allAreSame;
    }

    IPQValueDatePriceVolumeLayer IPQValueDatePriceVolumeLayer.Clone() => (IPQSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

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

    public override string ToString() => $"{GetType().Name}({PQSourceQuoteRefTraderValueDatePriceVolumeLayerToStringMembers})";
}
