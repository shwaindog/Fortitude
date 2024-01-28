#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public interface IPQSourceQuoteRefTraderValueDatePriceVolumeLayer : IPQTraderPriceVolumeLayer,
    IPQValueDatePriceVolumeLayer, IPQSourceQuoteRefPriceVolumeLayer,
    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
{
    new IPQSourceQuoteRefTraderValueDatePriceVolumeLayer Clone();
}

public class PQSourceQuoteRefTraderValueDatePriceVolumeLayer : PQTraderPriceVolumeLayer,
    IPQSourceQuoteRefTraderValueDatePriceVolumeLayer
{
    protected LayerBooleanFlags LayerBooleanFlags;
    private ushort sourceId;
    private uint sourceQuoteReference;
    private DateTime valueDate = DateTimeConstants.UnixEpoch;

    public PQSourceQuoteRefTraderValueDatePriceVolumeLayer(IPQNameIdLookupGenerator initialDict)
        : base(initialDict) =>
        SourceNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
            PQFieldFlags.SourceNameIdLookupSubDictionaryKey);

    public PQSourceQuoteRefTraderValueDatePriceVolumeLayer(decimal price = 0m, decimal volume = 0m,
        IPQNameIdLookupGenerator? sourceIdToNameIdLookup = null,
        IPQNameIdLookupGenerator? traderIdToNameLookup = null,
        DateTime? valueDate = null, string? sourceName = null,
        bool executable = false, uint sourceQuoteReference = 0u)
        : base(price, volume, traderIdToNameLookup)
    {
        SourceNameIdLookup = sourceIdToNameIdLookup ??
                             new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                 PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        ValueDate = valueDate ?? DateTimeConstants.UnixEpoch;
        SourceName = sourceName;
        Executable = executable;
        SourceQuoteReference = sourceQuoteReference;
    }

    public PQSourceQuoteRefTraderValueDatePriceVolumeLayer(IPriceVolumeLayer toClone)
        : base(toClone)
    {
        if (toClone is IPQSourcePriceVolumeLayer pqSourcePvl)
            SourceNameIdLookup = pqSourcePvl.SourceNameIdLookup ??
                                 new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                     PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        else
            SourceNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        if (toClone is ISourcePriceVolumeLayer srcPvl)
        {
            SourceName = srcPvl.SourceName;
            Executable = srcPvl.Executable;
        }

        if (toClone is ISourceQuoteRefPriceVolumeLayer srcQtRefPvl)
            SourceQuoteReference = srcQtRefPvl.SourceQuoteReference;
        if (toClone is IValueDatePriceVolumeLayer valueDatePvl) ValueDate = valueDatePvl.ValueDate;
        SetFlagsSame(toClone);
    }

    protected string PQSourceQuoteRefTraderValueDatePriceVolumeLayerToStringMembers =>
        $"{nameof(Price)}: {Price:N5}, " +
        $"{nameof(Volume)}: {Volume:N2}, {nameof(SourceName)}: {SourceName}, {nameof(Executable)}: {Executable}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}, {nameof(ValueDate)}: {ValueDate}, " +
        $"{nameof(Count)}: {Count} {PQTraderPriceVolumeLayerToStringMembers}";

    public DateTime ValueDate
    {
        get => valueDate;
        set
        {
            if (valueDate == value) return;
            IsValueDateUpdated = true;
            valueDate = value;
        }
    }

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

    public uint SourceQuoteReference
    {
        get => sourceQuoteReference;
        set
        {
            if (sourceQuoteReference == value) return;
            IsSourceQuoteReferenceUpdated = true;
            sourceQuoteReference = value;
        }
    }

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

    public ushort SourceId
    {
        get => sourceId;
        set
        {
            if (SourceId == value) return;
            IsSourceNameUpdated = true;
            sourceId = value;
        }
    }

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

    public string? SourceName
    {
        get => SourceNameIdLookup[SourceId];
        set
        {
            var dictionaryId = SourceNameIdLookup.GetOrAddId(value);
            if (dictionaryId <= 0 && value != null)
                throw new Exception("Error attempted to set the Source Name to something " +
                                    "not defined in the source name to Id table.");
            SourceId = (ushort)dictionaryId;
        }
    }

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

    public IPQNameIdLookupGenerator SourceNameIdLookup { get; set; }

    public override bool IsEmpty =>
        base.IsEmpty && ValueDate == DateTimeConstants.UnixEpoch
                     && SourceName == null && SourceQuoteReference == 0u && !Executable;

    public override bool HasUpdates
    {
        get => base.HasUpdates || SourceNameIdLookup!.HasUpdates;
        set
        {
            base.HasUpdates = value;
            SourceNameIdLookup!.HasUpdates = value;
        }
    }

    public override void StateReset()
    {
        ValueDate = DateTimeConstants.UnixEpoch;
        SourceName = null;
        Executable = false;
        SourceQuoteReference = 0u;
        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
                     quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsValueDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerDateOffset, valueDate.GetHoursFromUnixEpoch(),
                PQFieldFlags.IsExtendedFieldId);
        if (!updatedOnly || IsSourceNameUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset, SourceId);
        if (!updatedOnly || IsExecutableUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerBooleanFlagsOffset,
                Executable ? PQFieldFlags.LayerExecutableFlag : 0);
        if (!updatedOnly || IsSourceQuoteReferenceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset, sourceQuoteReference);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id >= PQFieldKeys.LayerDateOffset && pqFieldUpdate.Id <
            PQFieldKeys.LayerDateOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            PQFieldConverters.UpdateHoursFromUnixEpoch(ref valueDate, pqFieldUpdate.Value);
            IsValueDateUpdated = true;
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

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle)
    {
        foreach (var baseUpdates in base.GetStringUpdates(snapShotTime, updatedStyle)) yield return baseUpdates;
        if (SourceNameIdLookup is IPQNameIdLookupGenerator pqNameIdLookupGenerator)
            foreach (var stringUpdate in pqNameIdLookupGenerator.GetStringUpdates(snapShotTime, updatedStyle))
                yield return stringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        base.UpdateFieldString(updates);
        if (updates.Field.Id != PQFieldKeys.LayerNameDictionaryUpsertCommand) return false;
        if (SourceNameIdLookup != null) return SourceNameIdLookup.UpdateFieldString(updates);
        return false;
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source);
        switch (source)
        {
            case IPQSourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPv:
                SourceNameIdLookup.CopyFrom(srcQtRefTrdrVlDtPv.SourceNameIdLookup);
                if (srcQtRefTrdrVlDtPv.IsValueDateUpdated) ValueDate = srcQtRefTrdrVlDtPv.ValueDate;
                if (srcQtRefTrdrVlDtPv.IsSourceQuoteReferenceUpdated)
                    SourceQuoteReference = srcQtRefTrdrVlDtPv.SourceQuoteReference;
                if (srcQtRefTrdrVlDtPv.IsSourceNameUpdated) SourceId = srcQtRefTrdrVlDtPv.SourceId;
                if (srcQtRefTrdrVlDtPv.IsExecutableUpdated) Executable = srcQtRefTrdrVlDtPv.Executable;
                SetFlagsSame(srcQtRefTrdrVlDtPv);
                break;
            case IPQSourceQuoteRefPriceVolumeLayer pqSrcQtRefPvLayer:
                SourceNameIdLookup.CopyFrom(pqSrcQtRefPvLayer.SourceNameIdLookup);
                if (pqSrcQtRefPvLayer.IsSourceQuoteReferenceUpdated)
                    SourceQuoteReference = pqSrcQtRefPvLayer.SourceQuoteReference;
                if (pqSrcQtRefPvLayer.IsSourceNameUpdated) SourceId = pqSrcQtRefPvLayer.SourceId;
                if (pqSrcQtRefPvLayer.IsExecutableUpdated) Executable = pqSrcQtRefPvLayer.Executable;
                SetFlagsSame(pqSrcQtRefPvLayer);
                break;
            case IPQSourcePriceVolumeLayer pqSourcePvLayer:
                SourceNameIdLookup.CopyFrom(pqSourcePvLayer.SourceNameIdLookup);
                if (pqSourcePvLayer.IsSourceNameUpdated) SourceId = pqSourcePvLayer.SourceId;
                if (pqSourcePvLayer.IsExecutableUpdated) Executable = pqSourcePvLayer.Executable;
                SetFlagsSame(pqSourcePvLayer);
                break;
            case IPQValueDatePriceVolumeLayer pqValueDate:
                if (pqValueDate.IsValueDateUpdated) ValueDate = pqValueDate.ValueDate;
                SetFlagsSame(pqValueDate);
                break;
            case ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlPvLayer:
                ValueDate = srcQtRefTrdrVlPvLayer.ValueDate;
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
            case IValueDatePriceVolumeLayer valueDatePvLayer:
                ValueDate = valueDatePvLayer.ValueDate;
                break;
        }

        return this;
    }

    public override void EnsureRelatedItemsAreConfigured(ISourceTickerQuoteInfo? referenceInstance)
    {
        base.EnsureRelatedItemsAreConfigured(referenceInstance);
        if (referenceInstance is IPQSourceTickerQuoteInfo pqSrcTkrQtInfo)
            if (ReferenceEquals(pqSrcTkrQtInfo.SourceNameIdLookup, SourceNameIdLookup))
                SourceNameIdLookup = pqSrcTkrQtInfo.SourceNameIdLookup.Clone();
        if (SourceNameIdLookup == null)
            SourceNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                1);
    }

    public override void EnsureRelatedItemsAreConfigured(IPQPriceVolumeLayer? referenceInstance)
    {
        base.EnsureRelatedItemsAreConfigured(referenceInstance);
        if (referenceInstance is IPQSourcePriceVolumeLayer pqSrcPvLayer)
            SourceNameIdLookup = pqSrcPvLayer.SourceNameIdLookup;
    }

    IPQSourceQuoteRefTraderValueDatePriceVolumeLayer IPQSourceQuoteRefTraderValueDatePriceVolumeLayer.Clone() =>
        (IPQSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IPQValueDatePriceVolumeLayer IPQValueDatePriceVolumeLayer.Clone() =>
        (IPQSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.Clone() =>
        (IPQSourceQuoteRefPriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => (ISourcePriceVolumeLayer)Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => (IMutableSourcePriceVolumeLayer)Clone();

    IPQSourcePriceVolumeLayer IPQSourcePriceVolumeLayer.Clone() => (IPQSourcePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => (ISourcePriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() =>
        (ISourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefTraderValueDatePriceVolumeLayer
        ICloneable<ISourceQuoteRefTraderValueDatePriceVolumeLayer>.Clone() =>
        (ISourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    ISourceQuoteRefTraderValueDatePriceVolumeLayer ISourceQuoteRefTraderValueDatePriceVolumeLayer.Clone() =>
        (ISourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
        ICloneable<IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
        IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer.Clone() =>
        (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() =>
        (IMutableValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() =>
        (IMutableValueDatePriceVolumeLayer)Clone();

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => (IValueDatePriceVolumeLayer)Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => (IValueDatePriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvLayer))
            return false;

        var baseSame = base.AreEquivalent(other, exactTypes);
        var valueDateSame = ValueDate == srcQtRefTrdrVlDtPvLayer.ValueDate;
        var sourceQuoteRefSame = SourceQuoteReference == srcQtRefTrdrVlDtPvLayer.SourceQuoteReference;
        var sourceNameSame = SourceName == srcQtRefTrdrVlDtPvLayer.SourceName;
        var executableSame = Executable == srcQtRefTrdrVlDtPvLayer.Executable;

        return baseSame && valueDateSame && sourceQuoteRefSame && sourceNameSame && executableSame;
    }

    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

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
        $"{GetType().Name}({PQSourceQuoteRefTraderValueDatePriceVolumeLayerToStringMembers})";
}
