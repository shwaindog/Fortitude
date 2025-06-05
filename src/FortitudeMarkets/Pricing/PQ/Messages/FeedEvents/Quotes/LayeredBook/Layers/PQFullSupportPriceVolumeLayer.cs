// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

public interface IPQFullSupportPriceVolumeLayer : IPQOrdersPriceVolumeLayer,
    IPQValueDatePriceVolumeLayer, IPQSourceQuoteRefPriceVolumeLayer,
    IMutableFullSupportPriceVolumeLayer, ITrackableReset<IPQFullSupportPriceVolumeLayer>
{
    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQFullSupportPriceVolumeLayer Clone();
    new IPQFullSupportPriceVolumeLayer ResetWithTracking();
}

public class PQFullSupportPriceVolumeLayer : PQOrdersPriceVolumeLayer, IPQFullSupportPriceVolumeLayer
{
    private ushort   sourceId;
    private uint     sourceQuoteReference;
    private DateTime valueDate = DateTime.MinValue;

    public PQFullSupportPriceVolumeLayer(IPQNameIdLookupGenerator initialDict, QuoteLayerInstantBehaviorFlags layerBehavior)
        : base(LayerType.FullSupportPriceVolume, initialDict, layerBehavior)
    {
        if (GetType() == typeof(PQFullSupportPriceVolumeLayer)) SequenceId = 0;
    }

    public PQFullSupportPriceVolumeLayer
    (IPQNameIdLookupGenerator nameIdLookup, decimal price = 0m, decimal volume = 0m, DateTime? valueDate = null
      , string? sourceName = null, bool executable = false, uint sourceQuoteReference = 0u, uint ordersCount = 0, decimal internalVolume = 0m
      , IEnumerable<IAnonymousOrder>? layerOrders = null,
        QuoteLayerInstantBehaviorFlags layerBehavior = QuoteLayerInstantBehaviorFlags.None)
        : base(nameIdLookup, LayerType.FullSupportPriceVolume, price, volume, ordersCount, internalVolume, layerOrders, layerBehavior)
    {
        ValueDate  = valueDate ?? DateTime.MinValue;
        SourceName = sourceName;
        Executable = executable;

        SourceQuoteReference = sourceQuoteReference;

        if (GetType() == typeof(PQFullSupportPriceVolumeLayer)) SequenceId = 0;
    }

    public PQFullSupportPriceVolumeLayer(IPriceVolumeLayer toClone, IPQNameIdLookupGenerator ipNameIdLookupGenerator)
        : base(toClone, LayerType.FullSupportPriceVolume, ipNameIdLookupGenerator)
    {
        if (toClone is ISourcePriceVolumeLayer srcPvl)
        {
            SourceName = srcPvl.SourceName;
            Executable = srcPvl.Executable;
        }

        if (toClone is ISourceQuoteRefPriceVolumeLayer srcQtRefPvl) SourceQuoteReference = srcQtRefPvl.SourceQuoteReference;
        if (toClone is IValueDatePriceVolumeLayer valueDatePvl) ValueDate                = valueDatePvl.ValueDate;
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQFullSupportPriceVolumeLayer)) SequenceId = 0;
    }

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

    [JsonIgnore] public override LayerType LayerType => LayerType.FullSupportPriceVolume;

    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalFullSupportLayerFlags | base.SupportsLayerFlags;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate
    {
        get => valueDate;
        set
        {
            IsValueDateUpdated |= valueDate != value || SequenceId == 0;
            valueDate          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference
    {
        get => sourceQuoteReference;
        set
        {
            IsSourceQuoteReferenceUpdated |= sourceQuoteReference != value || SequenceId == 0;
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
            IsSourceNameUpdated |= sourceId != value || SequenceId == 0;
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
        get => (BooleanValues & LayerBooleanValues.Executable) > 0;
        set
        {
            IsExecutableUpdated |= Executable != value || SequenceId == 0;
            if (value)
                BooleanValues |= LayerBooleanValues.Executable;

            else if (Executable) BooleanValues ^= LayerBooleanValues.Executable;
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
            base.IsEmpty && ValueDate == DateTime.MinValue
                         && SourceName == null && SourceQuoteReference == 0u && !Executable;
        set
        {
            if (!value) return;

            ValueDate  = DateTime.MinValue;
            SourceName = null;
            Executable = false;

            SourceQuoteReference = 0;

            base.IsEmpty = true;
        }
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates;
        set
        {
            base.HasUpdates          = value;
            NameIdLookup.HasUpdates = value;
        }
    }

    public override void UpdateComplete(uint updateSequenceId = 0)
    {
        NameIdLookup.UpdateComplete(updateSequenceId);
        base.UpdateComplete(updateSequenceId);
    }

    IMutableValueDatePriceVolumeLayer ITrackableReset<IMutableValueDatePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IPQValueDatePriceVolumeLayer ITrackableReset<IPQValueDatePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQValueDatePriceVolumeLayer IPQValueDatePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IMutableSourcePriceVolumeLayer ITrackableReset<IMutableSourcePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IMutableSourceQuoteRefPriceVolumeLayer ITrackableReset<IMutableSourceQuoteRefPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IPQSourcePriceVolumeLayer ITrackableReset<IPQSourcePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQSourcePriceVolumeLayer IPQSourcePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IPQSourceQuoteRefPriceVolumeLayer ITrackableReset<IPQSourceQuoteRefPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IPQFullSupportPriceVolumeLayer ITrackableReset<IPQFullSupportPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQFullSupportPriceVolumeLayer IPQFullSupportPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IMutableFullSupportPriceVolumeLayer ITrackableReset<IMutableFullSupportPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableFullSupportPriceVolumeLayer IMutableFullSupportPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    public override PQFullSupportPriceVolumeLayer ResetWithTracking()
    {
        ValueDate  = DateTime.MinValue;
        SourceName = null;
        Executable = false;

        SourceQuoteReference = 0u;
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        ValueDate  = DateTime.MinValue;
        SourceName = null;
        Executable = false;

        SourceQuoteReference = 0u;

        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var fullPicture = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) > 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (fullPicture || IsValueDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, valueDate.Get2MinIntervalsFromUnixEpoch());
        if (fullPicture || IsSourceNameUpdated) yield return new PQFieldUpdate(PQFeedFields.QuoteLayerSourceId, SourceId);
        if (fullPicture || IsExecutableUpdated)
        {
            var boolValues = LayerBehavior.HasPublishQuoteInstantBehaviorFlagsFlag()
                ? (uint)BooleanValues
                : BooleanValues.JustLayerBooleanValuesMask();
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerBooleanFlags, boolValues);
        }
        if (fullPicture || IsSourceQuoteReferenceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerSourceQuoteRef, sourceQuoteReference);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.QuoteLayerValueDate:
                var originalValueDate = valueDate;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref valueDate, pqFieldUpdate.Payload);
                IsValueDateUpdated = originalValueDate != valueDate;
                return 0;
            case PQFeedFields.QuoteLayerSourceId:
                IsSourceNameUpdated = true; // in-case of reset and sending 0;
                SourceId            = (ushort)pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.QuoteLayerBooleanFlags:
                IsExecutableUpdated = true; // in-case of reset and sending 0;
                var newFlags = pqFieldUpdate.Payload.JustLayerBooleanValuesMask();
                Executable = (newFlags & LayerBooleanValues.Executable) != 0;
                return 0;
            case PQFeedFields.QuoteLayerSourceQuoteRef:
                IsSourceQuoteReferenceUpdated = true; // in-case of reset and sending 0;
                SourceQuoteReference          = pqFieldUpdate.Payload;
                return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags)
    {
        foreach (var baseUpdates in base.GetStringUpdates(snapShotTime, messageFlags)) yield return baseUpdates;
        if (NameIdLookup is { } pqNameIdLookupGenerator)
            foreach (var stringUpdate in pqNameIdLookupGenerator.GetStringUpdates(snapShotTime, messageFlags))
                yield return stringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        base.UpdateFieldString(stringUpdate);
        if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public override PQFullSupportPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, behaviorFlags, copyMergeFlags);
        var isFullReplace    = copyMergeFlags.HasFullReplace();
        var isSkipRefLookups = copyMergeFlags.HasSkipReferenceLookups();
        switch (source)
        {
            // IPQOrdersPriceVolumeLayer && IPQOrdersCountPriceVolumeLayer are done in base
            case IPQFullSupportPriceVolumeLayer fullSupportPvLayer:
                if (!isSkipRefLookups) NameIdLookup.CopyFrom(fullSupportPvLayer.NameIdLookup);
                if (fullSupportPvLayer.IsValueDateUpdated || isFullReplace)
                {
                    IsValueDateUpdated = true;
                    ValueDate          = fullSupportPvLayer.ValueDate;
                }
                if (fullSupportPvLayer.IsSourceQuoteReferenceUpdated || isFullReplace)
                {
                    IsSourceQuoteReferenceUpdated = true;

                    SourceQuoteReference = fullSupportPvLayer.SourceQuoteReference;
                }
                if (fullSupportPvLayer.IsSourceNameUpdated || isFullReplace)
                {
                    IsSourceNameUpdated = true;

                    SourceId = fullSupportPvLayer.SourceId;
                }
                if (fullSupportPvLayer.IsExecutableUpdated || isFullReplace)
                {
                    IsExecutableUpdated = true;
                    Executable          = fullSupportPvLayer.Executable;
                }
                if (isFullReplace) SetFlagsSame(fullSupportPvLayer);
                break;
            case IPQSourceQuoteRefPriceVolumeLayer pqSrcQtRefPvLayer:
                if (!isSkipRefLookups) NameIdLookup.CopyFrom(pqSrcQtRefPvLayer.NameIdLookup);
                if (pqSrcQtRefPvLayer.IsSourceQuoteReferenceUpdated || isFullReplace)
                {
                    IsSourceQuoteReferenceUpdated = true;

                    SourceQuoteReference = pqSrcQtRefPvLayer.SourceQuoteReference;
                }
                if (pqSrcQtRefPvLayer.IsSourceNameUpdated || isFullReplace)
                {
                    IsSourceNameUpdated = true;

                    SourceId = pqSrcQtRefPvLayer.SourceId;
                }
                if (pqSrcQtRefPvLayer.IsExecutableUpdated || isFullReplace)
                {
                    IsExecutableUpdated = true;

                    Executable = pqSrcQtRefPvLayer.Executable;
                }
                if (isFullReplace) SetFlagsSame(pqSrcQtRefPvLayer);
                break;
            case IPQSourcePriceVolumeLayer pqSourcePvLayer:
                if (!isSkipRefLookups) NameIdLookup.CopyFrom(pqSourcePvLayer.NameIdLookup);
                if (pqSourcePvLayer.IsSourceNameUpdated || isFullReplace)
                {
                    IsSourceNameUpdated = true;

                    SourceId = pqSourcePvLayer.SourceId;
                }
                if (pqSourcePvLayer.IsExecutableUpdated || isFullReplace)
                {
                    IsExecutableUpdated = true;

                    Executable = pqSourcePvLayer.Executable;
                }
                if (isFullReplace) SetFlagsSame(pqSourcePvLayer);
                break;
            case IPQValueDatePriceVolumeLayer pqValueDate:
                if (pqValueDate.IsValueDateUpdated || isFullReplace)
                {
                    IsValueDateUpdated = true;

                    ValueDate = pqValueDate.ValueDate;
                }
                if (isFullReplace) SetFlagsSame(pqValueDate);
                break;
            case IFullSupportPriceVolumeLayer fullSupportPvLayer:

                ValueDate  = fullSupportPvLayer.ValueDate;
                SourceName = fullSupportPvLayer.SourceName;
                Executable = fullSupportPvLayer.Executable;

                SourceQuoteReference = fullSupportPvLayer.SourceQuoteReference;
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

    IPQFullSupportPriceVolumeLayer IPQFullSupportPriceVolumeLayer.Clone() => Clone();

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => Clone();

    IPQSourcePriceVolumeLayer IPQSourcePriceVolumeLayer.Clone() => Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => Clone();

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    IFullSupportPriceVolumeLayer ICloneable<IFullSupportPriceVolumeLayer>.Clone() => Clone();

    IFullSupportPriceVolumeLayer IFullSupportPriceVolumeLayer.Clone() => Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() => Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    IMutableFullSupportPriceVolumeLayer ICloneable<IMutableFullSupportPriceVolumeLayer>.Clone() => Clone();

    IMutableFullSupportPriceVolumeLayer IMutableFullSupportPriceVolumeLayer.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => Clone();

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => Clone();

    IPQValueDatePriceVolumeLayer IPQValueDatePriceVolumeLayer.Clone() => Clone();

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IMutableFullSupportPriceVolumeLayer fullSupportPvLayer)) return false;

        var baseSame           = base.AreEquivalent(other, exactTypes);
        var valueDateSame      = ValueDate == fullSupportPvLayer.ValueDate;
        var sourceQuoteRefSame = SourceQuoteReference == fullSupportPvLayer.SourceQuoteReference;
        var sourceNameSame     = SourceName == fullSupportPvLayer.SourceName;
        var executableSame     = Executable == fullSupportPvLayer.Executable;

        var allAreSame = baseSame && valueDateSame && sourceQuoteRefSame && sourceNameSame && executableSame;
        return allAreSame;
    }

    public override PQFullSupportPriceVolumeLayer Clone() => new(this, NameIdLookup);


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

    protected string PQFullSupportVolumeLayerToStringMembers =>
        $"{PQOrdersCountVolumeLayerToStringMembers}, {nameof(SourceId)}: {SourceId}, {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(Executable)}: {Executable}, {nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}, {nameof(ValueDate)}: {ValueDate}, " +
        $"{PQJustOrdersToStringMembers}";

    public override string ToString() => $"{GetType().Name}({PQFullSupportVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
