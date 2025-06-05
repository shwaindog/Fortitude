// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

public interface IPQSourceQuoteRefPriceVolumeLayer : IMutableSourceQuoteRefPriceVolumeLayer,
    IPQSourcePriceVolumeLayer, ITrackableReset<IPQSourceQuoteRefPriceVolumeLayer>

{
    bool IsSourceQuoteReferenceUpdated { get; set; }

    new IPQSourceQuoteRefPriceVolumeLayer Clone();
    new IPQSourceQuoteRefPriceVolumeLayer ResetWithTracking();
}

public class PQSourceQuoteRefPriceVolumeLayer : PQSourcePriceVolumeLayer, IPQSourceQuoteRefPriceVolumeLayer
{
    private uint sourceQuoteReference;

    public PQSourceQuoteRefPriceVolumeLayer
    (IPQNameIdLookupGenerator sourceIdToNameIdLookup, decimal price = 0m, decimal volume = 0m,
        string? sourceName = null, bool executable = false, uint quoteRef = 0u)
        : base(sourceIdToNameIdLookup, price, volume, sourceName, executable)
    {
        SourceQuoteReference = quoteRef;
        if (GetType() == typeof(PQSourceQuoteRefPriceVolumeLayer)) SequenceId = 0;
    }

    public PQSourceQuoteRefPriceVolumeLayer(IPriceVolumeLayer toClone, IPQNameIdLookupGenerator nameIdLookupGenerator) : base(toClone
   , nameIdLookupGenerator)
    {
        if (toClone is ISourceQuoteRefPriceVolumeLayer sourceQtRefPvLayer) SourceQuoteReference = sourceQtRefPvLayer.SourceQuoteReference;
        SetFlagsSame(toClone);
        if (GetType() == typeof(PQSourceQuoteRefPriceVolumeLayer)) SequenceId = 0;
    }

    protected string PQSourceQuoteRefPriceVolumeLayerToStringMembers =>
        $"{PQSourcePriceVolumeLayerToStringMembers}, {nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}";

    [JsonIgnore] public override LayerType  LayerType          => LayerType.SourceQuoteRefPriceVolume;
    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalSourceQuoteRefFlags | base.SupportsLayerFlags;

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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && SourceQuoteReference == 0;
        set
        {
            if (!value) return;
            SourceQuoteReference = 0;

            base.IsEmpty = true;
        }
    }

    IMutableSourceQuoteRefPriceVolumeLayer ITrackableReset<IMutableSourceQuoteRefPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IPQSourceQuoteRefPriceVolumeLayer ITrackableReset<IPQSourceQuoteRefPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    public override PQSourceQuoteRefPriceVolumeLayer ResetWithTracking()
    {
        SourceQuoteReference = 0;
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        SourceQuoteReference = 0;
        base.StateReset();
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id == PQFeedFields.QuoteLayerSourceQuoteRef)
        {
            IsSourceQuoteReferenceUpdated = true; // in-case of reset and sending 0;
            SourceQuoteReference          = pqFieldUpdate.Payload;
            return 0;
        }
        return base.UpdateField(pqFieldUpdate);
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) == 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsSourceQuoteReferenceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerSourceQuoteRef, SourceQuoteReference);
    }

    public override PQSourceQuoteRefPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, behaviorFlags, copyMergeFlags);
        var pqSqrPvl   = source as IPQSourceQuoteRefPriceVolumeLayer;
        var isFullReplace = copyMergeFlags.HasFullReplace();
        if (source is ISourceQuoteRefPriceVolumeLayer sqrPvl && pqSqrPvl == null)
        {
            SourceQuoteReference = sqrPvl.SourceQuoteReference;
        }
        else if (pqSqrPvl != null)
        {
            if (pqSqrPvl.IsSourceQuoteReferenceUpdated || isFullReplace)
            {
                IsSourceQuoteReferenceUpdated = true;

                SourceQuoteReference = pqSqrPvl.SourceQuoteReference;
            }
            if (isFullReplace) SetFlagsSame(pqSqrPvl);
        }

        return this;
    }

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.Clone() => (IPQSourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQSourceQuoteRefPriceVolumeLayer(this, NameIdLookup);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourceQuoteRefPriceVolumeLayer sourceQtRefOther)) return false;
        var baseSame           = base.AreEquivalent(other, exactTypes);
        var sourceQuoteRefSame = SourceQuoteReference == sourceQtRefOther.SourceQuoteReference;

        return baseSame && sourceQuoteRefSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ISourcePriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ sourceQuoteReference.GetHashCode();
        }
    }

    public override string ToString() => $"{GetType().Name}({PQSourceQuoteRefPriceVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
