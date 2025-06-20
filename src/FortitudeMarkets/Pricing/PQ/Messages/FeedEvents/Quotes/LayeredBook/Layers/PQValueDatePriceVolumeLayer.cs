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

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

public interface IPQValueDatePriceVolumeLayer : IMutableValueDatePriceVolumeLayer, IPQPriceVolumeLayer, ITrackableReset<IPQValueDatePriceVolumeLayer>
{
    bool IsValueDateUpdated { get; set; }

    new IPQValueDatePriceVolumeLayer Clone();
    new IPQValueDatePriceVolumeLayer ResetWithTracking();
}

public class PQValueDatePriceVolumeLayer : PQPriceVolumeLayer, IPQValueDatePriceVolumeLayer
{
    private DateTime valueDate = DateTime.MinValue;

    public PQValueDatePriceVolumeLayer
        (decimal price = 0m, decimal volume = 0m, DateTime? valueDate = null)
        : base(price, volume)
    {
        ValueDate = valueDate ?? DateTime.MinValue;
        if (GetType() == typeof(PQValueDatePriceVolumeLayer)) SequenceId = 0;
    }

    public PQValueDatePriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is IValueDatePriceVolumeLayer valueDateToClone) ValueDate = valueDateToClone.ValueDate;

        SetFlagsSame(toClone);
        if (GetType() == typeof(PQValueDatePriceVolumeLayer)) SequenceId = 0;
    }

    protected string PQValueDatePriceVolumeLayerToStringMembers => $"{PQPriceVolumeLayerToStringMembers}, {nameof(ValueDate)}: {ValueDate}";

    [JsonIgnore] public override LayerType  LayerType          => LayerType.ValueDatePriceVolume;
    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalValueDateFlags | base.SupportsLayerFlags;


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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && ValueDate == DateTime.MinValue;
        set
        {
            if (!value) return;
            ValueDate    = DateTime.MinValue;
            base.IsEmpty = true;
        }
    }
    

    IMutableValueDatePriceVolumeLayer ITrackableReset<IMutableValueDatePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IPQValueDatePriceVolumeLayer ITrackableReset<IPQValueDatePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQValueDatePriceVolumeLayer IPQValueDatePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    public override PQValueDatePriceVolumeLayer ResetWithTracking()
    {
        ValueDate = DateTime.MinValue;
        base.ResetWithTracking();
        return this;
    }


    public override void StateReset()
    {
        ValueDate = DateTime.MinValue;
        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var fullPicture = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) > 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (fullPicture || IsValueDateUpdated) yield return new PQFieldUpdate(PQFeedFields.QuoteLayerValueDate, valueDate.Get2MinIntervalsFromUnixEpoch());
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id == PQFeedFields.QuoteLayerValueDate)
        {
            var originalValueDate = valueDate;
            PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref valueDate, pqFieldUpdate.Payload);
            IsValueDateUpdated = originalValueDate != valueDate; // in-case of reset and sending 0;
            return 0;
        }
        return base.UpdateField(pqFieldUpdate);
    }

    public override PQValueDatePriceVolumeLayer CopyFrom(IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, behaviorFlags, copyMergeFlags);
        var pqValueDate   = source as IPQValueDatePriceVolumeLayer;
        var isFullReplace = copyMergeFlags.HasFullReplace();
        if (source is IValueDatePriceVolumeLayer vlDtPvLayer && pqValueDate == null)
        {
            ValueDate = vlDtPvLayer.ValueDate;
        }
        else if (pqValueDate != null)
        {
            if (pqValueDate.IsValueDateUpdated || isFullReplace)
            {
                IsValueDateUpdated = true;

                ValueDate = pqValueDate.ValueDate;
            }
            if (isFullReplace) SetFlagsSame(pqValueDate);
        }
        return this;
    }

    IPQValueDatePriceVolumeLayer IPQValueDatePriceVolumeLayer.Clone() => (IPQValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => (IMutableValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => (IMutableValueDatePriceVolumeLayer)Clone();

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => (IValueDatePriceVolumeLayer)Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => (IValueDatePriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQValueDatePriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IValueDatePriceVolumeLayer valueDateOther)) return false;

        var baseSame      = base.AreEquivalent(other, exactTypes);
        var valueDateSame = ValueDate == valueDateOther.ValueDate;

        return baseSame && valueDateSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ valueDate.GetHashCode();
        }
    }

    public override string ToString() => $"{GetType().Name}({PQValueDatePriceVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
