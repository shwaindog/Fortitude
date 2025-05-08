// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[JsonDerivedType(typeof(PQPriceVolumeLayer))]
[JsonDerivedType(typeof(PQSourcePriceVolumeLayer))]
[JsonDerivedType(typeof(PQSourceQuoteRefPriceVolumeLayer))]
[JsonDerivedType(typeof(PQFullSupportPriceVolumeLayer))]
[JsonDerivedType(typeof(PQValueDatePriceVolumeLayer))]
[JsonDerivedType(typeof(PQOrdersCountPriceVolumeLayer))]
[JsonDerivedType(typeof(PQOrdersPriceVolumeLayer))]
public interface IPQPriceVolumeLayer : IMutablePriceVolumeLayer, IPQSupportsFieldUpdates<IPriceVolumeLayer>
{
    [JsonIgnore] bool IsPriceUpdated  { get; set; }
    [JsonIgnore] bool IsVolumeUpdated { get; set; }

    new IPQPriceVolumeLayer Clone();
}

public class PQPriceVolumeLayer : ReusableObject<IPriceVolumeLayer>, IPQPriceVolumeLayer
{
    protected uint    NumUpdatesSinceEmpty = uint.MaxValue;
    private   decimal price;

    protected LayerFieldUpdatedFlags UpdatedFlags;

    private decimal volume;

    public PQPriceVolumeLayer()
    {
        if (GetType() == typeof(PQPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    public PQPriceVolumeLayer(decimal price = 0m, decimal volume = 0m)
    {
        Price  = price;
        Volume = volume;

        if (GetType() == typeof(PQPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    public PQPriceVolumeLayer(IPriceVolumeLayer toClone)
    {
        Price  = toClone.Price;
        Volume = toClone.Volume;
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQPriceVolumeLayerToStringMembers => $"{nameof(Price)}: {Price:N5}, {nameof(Volume)}: {Volume:N2}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    [JsonIgnore] public virtual LayerType  LayerType          => LayerType.PriceVolume;
    [JsonIgnore] public virtual LayerFlags SupportsLayerFlags => LayerFlagsExtensions.PriceVolumeLayerFlags;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal Price
    {
        get => price;
        set
        {
            IsPriceUpdated |= price != value || NumUpdatesSinceEmpty == 0;
            price          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal Volume
    {
        get => volume;
        set
        {
            IsVolumeUpdated |= volume != value || NumUpdatesSinceEmpty == 0;
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
        set
        {
            if (value)
            {
                UpdatedFlags = UpdatedFlags.AllFlags();
                return;
            }
            else
            {
                UpdatedFlags = LayerFieldUpdatedFlags.None;
            }
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get => Price == 0m && Volume == 0m;
        set
        {
            if (!value) return;
            Price = Volume = 0m;

            NumUpdatesSinceEmpty = 0;
        }
    }

    public uint UpdateCount => NumUpdatesSinceEmpty;

    public virtual void UpdateComplete()
    {
        if (HasUpdates && !IsEmpty) NumUpdatesSinceEmpty++;
        HasUpdates = false;
    }

    public override void StateReset()
    {
        Price        = 0m;
        Volume       = 0m;
        UpdatedFlags = LayerFieldUpdatedFlags.None;

        NumUpdatesSinceEmpty = 0;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsPriceUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.Price, Price,
                                           quotePublicationPrecisionSetting?.PriceScalingPrecision ?? (PQFieldFlags)1);
        if (!updatedOnly || IsVolumeUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.Volume, Volume,
                                           quotePublicationPrecisionSetting?.VolumeScalingPrecision ?? (PQFieldFlags)6);
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id == PQQuoteFields.Price)
        {
            IsPriceUpdated = true; // incase of reset and sending 0;

            Price = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
            return 0;
        }
        else if (pqFieldUpdate.Id == PQQuoteFields.Volume)
        {
            IsVolumeUpdated = true; // incase of reset and sending 0;

            Volume = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
            return 0;
        }
        return -1;
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not PQPriceVolumeLayer pqpvl)
        {
            Price  = source.Price;
            Volume = source.Volume;
        }
        else
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqpvl.IsPriceUpdated || isFullReplace)
            {
                IsPriceUpdated = true;

                Price = pqpvl.Price;
            }

            if (pqpvl.IsVolumeUpdated || isFullReplace)
            {
                IsVolumeUpdated = true;

                Volume = pqpvl.Volume;
            }

            if (isFullReplace) UpdatedFlags = pqpvl.UpdatedFlags;
        }

        return this;
    }


    public override IPQPriceVolumeLayer Clone() =>
        (IPQPriceVolumeLayer?)Recycler?.Borrow<PQPriceVolumeLayer>().CopyFrom(this) ?? new PQPriceVolumeLayer(this);

    IPriceVolumeLayer ICloneable<IPriceVolumeLayer>.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    public virtual bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
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

    public override string ToString() => $"{GetType().Name}({PQPriceVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
