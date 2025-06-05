// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

[JsonDerivedType(typeof(PQPriceVolumeLayer))]
[JsonDerivedType(typeof(PQSourcePriceVolumeLayer))]
[JsonDerivedType(typeof(PQSourceQuoteRefPriceVolumeLayer))]
[JsonDerivedType(typeof(PQFullSupportPriceVolumeLayer))]
[JsonDerivedType(typeof(PQValueDatePriceVolumeLayer))]
[JsonDerivedType(typeof(PQOrdersCountPriceVolumeLayer))]
[JsonDerivedType(typeof(PQOrdersPriceVolumeLayer))]
public interface IPQPriceVolumeLayer : IReusableQuoteElement<IPQPriceVolumeLayer>, IMutablePriceVolumeLayer
  , IPQSupportsNumberPrecisionFieldUpdates, ITrackableReset<IPQPriceVolumeLayer>
{
    [JsonIgnore] bool IsPriceUpdated  { get; set; }
    [JsonIgnore] bool IsVolumeUpdated { get; set; }

    new IPQPriceVolumeLayer Clone();
    new IPQPriceVolumeLayer ResetWithTracking();
}

public class PQPriceVolumeLayer : ReusableQuoteElement<IPriceVolumeLayer>, IPQPriceVolumeLayer
{
    protected uint    SequenceId = uint.MaxValue;
    private   decimal price;

    protected LayerFieldUpdatedFlags UpdatedFlags;

    private decimal volume;

    public PQPriceVolumeLayer()
    {
        if (GetType() == typeof(PQPriceVolumeLayer)) SequenceId = 0;
    }

    public PQPriceVolumeLayer(decimal price = 0m, decimal volume = 0m)
    {
        Price  = price;
        Volume = volume;

        if (GetType() == typeof(PQPriceVolumeLayer)) SequenceId = 0;
    }

    public PQPriceVolumeLayer(IPriceVolumeLayer toClone)
    {
        Price  = toClone.Price;
        Volume = toClone.Volume;
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQPriceVolumeLayer)) SequenceId = 0;
    }

    [JsonIgnore] public virtual LayerType LayerType => LayerType.PriceVolume;

    [JsonIgnore] public virtual LayerFlags SupportsLayerFlags => LayerFlagsExtensions.PriceVolumeLayerFlags;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal Price
    {
        get => price;
        set
        {
            IsPriceUpdated |= price != value || SequenceId == 0;
            price          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal Volume
    {
        get => volume;
        set
        {
            IsVolumeUpdated |= volume != value || SequenceId == 0;
            volume          =  value;
        }
    }

    [JsonIgnore]
    public bool IsPriceUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.PriceUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.PriceUpdatedFlag;

            else if (IsPriceUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.PriceUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsVolumeUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.VolumeUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.VolumeUpdatedFlag;

            else if (IsVolumeUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.VolumeUpdatedFlag;
        }
    }

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != LayerFieldUpdatedFlags.None;
        set => UpdatedFlags = value ? UpdatedFlags.AllFlags() : LayerFieldUpdatedFlags.None;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get => Price == 0m && Volume == 0m;
        set
        {
            if (!value) return;
            Price = Volume = 0m;

            SequenceId = 0;
        }
    }

    public uint UpdateSequenceId => SequenceId;

    public virtual void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public virtual void UpdateComplete(uint updateSequenceId = 0)
    {
        if (HasUpdates && !IsEmpty) SequenceId++;
        HasUpdates = false;
    }

    IMutablePriceVolumeLayer ITrackableReset<IMutablePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQPriceVolumeLayer ITrackableReset<IPQPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQPriceVolumeLayer IPQPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    public virtual PQPriceVolumeLayer ResetWithTracking()
    {
        Price  = 0m;
        Volume = 0m;
        return this;
    }

    public override void StateReset()
    {
        Price        = 0m;
        Volume       = 0m;
        UpdatedFlags = LayerFieldUpdatedFlags.None;

        SequenceId = 0;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) == 0;
        if (!updatedOnly || IsPriceUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, Price,
                                           quotePublicationPrecisionSetting?.PriceScalingPrecision ?? (PQFieldFlags)1);
        if (!updatedOnly || IsVolumeUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerVolume, Volume,
                                           quotePublicationPrecisionSetting?.VolumeScalingPrecision ?? (PQFieldFlags)6);
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id == PQFeedFields.QuoteLayerPrice)
        {
            IsPriceUpdated = true; // in-case of reset and sending 0;

            Price = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
            return 0;
        }
        else if (pqFieldUpdate.Id == PQFeedFields.QuoteLayerVolume)
        {
            IsVolumeUpdated = true; // in-case of reset and sending 0;

            Volume = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
            return 0;
        }
        return -1;
    }

    IMutablePriceVolumeLayer ICloneable<IMutablePriceVolumeLayer>.Clone() => Clone();

    IMutablePriceVolumeLayer IMutablePriceVolumeLayer.Clone() => Clone();

    IPriceVolumeLayer ICloneable<IPriceVolumeLayer>.Clone() => Clone();

    public override IPQPriceVolumeLayer Clone() =>
        (IPQPriceVolumeLayer?)Recycler?.Borrow<PQPriceVolumeLayer>().CopyFrom(this) ?? new PQPriceVolumeLayer(this);

    IReusableQuoteElement<IPQPriceVolumeLayer> ITransferQuoteState<IReusableQuoteElement<IPQPriceVolumeLayer>>.CopyFrom
        (IReusableQuoteElement<IPQPriceVolumeLayer> source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPriceVolumeLayer)source, behaviorFlags, copyMergeFlags);

    IPQPriceVolumeLayer ITransferQuoteState<IPQPriceVolumeLayer>.CopyFrom
        (IPQPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, behaviorFlags, copyMergeFlags);

    IReusableQuoteElement<IMutablePriceVolumeLayer> ITransferQuoteState<IReusableQuoteElement<IMutablePriceVolumeLayer>>.CopyFrom
        (IReusableQuoteElement<IMutablePriceVolumeLayer> source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPriceVolumeLayer)source, behaviorFlags, copyMergeFlags);

    IMutablePriceVolumeLayer ITransferQuoteState<IMutablePriceVolumeLayer>.CopyFrom
        (IMutablePriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, behaviorFlags, copyMergeFlags);

    public override PQPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not PQPriceVolumeLayer pqPvl)
        {
            Price  = source.Price;
            Volume = source.Volume;
        }
        else
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqPvl.IsPriceUpdated || isFullReplace)
            {
                IsPriceUpdated = true;

                Price = pqPvl.Price;
            }

            if (pqPvl.IsVolumeUpdated || isFullReplace)
            {
                IsVolumeUpdated = true;

                Volume = pqPvl.Volume;
            }

            if (isFullReplace) UpdatedFlags = pqPvl.UpdatedFlags;
        }

        return this;
    }

    bool IInterfacesComparable<IPQPriceVolumeLayer>.AreEquivalent(IPQPriceVolumeLayer? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    bool IInterfacesComparable<IMutablePriceVolumeLayer>.AreEquivalent(IMutablePriceVolumeLayer? other, bool exactTypes) =>
        AreEquivalent(other, exactTypes);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var priceSame  = Price == other.Price;
        var volumeSame = Volume == other.Volume;
        var flagsSame  = true;
        if (exactTypes)
        {
            var pqOther = (PQPriceVolumeLayer)other;
            flagsSame = UpdatedFlags == pqOther.UpdatedFlags;
        }

        var allAreSame = priceSame && volumeSame && flagsSame;
        return allAreSame;
    }

    protected void SetFlagsSame(IPriceVolumeLayer toCopyFlags)
    {
        if (toCopyFlags is PQPriceVolumeLayer pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = (price.GetHashCode() * 397) ^ UpdatedFlags.GetHashCode();
            hash = (hash * 397) ^ Volume.GetHashCode();
            return hash;
        }
    }

    protected string PQPriceVolumeLayerToStringMembers => $"{nameof(Price)}: {Price:N5}, {nameof(Volume)}: {Volume:N2}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() => $"{GetType().Name}({PQPriceVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
