// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQPriceVolumeLayer : IMutablePriceVolumeLayer, IPQSupportsFieldUpdates<IPriceVolumeLayer>
{
    bool IsPriceUpdated  { get; set; }
    bool IsVolumeUpdated { get; set; }

    new IPQPriceVolumeLayer Clone();
}

public class PQPriceVolumeLayer : ReusableObject<IPriceVolumeLayer>, IPQPriceVolumeLayer
{
    private decimal price;

    protected LayerFieldUpdatedFlags UpdatedFlags;

    private decimal volume;

    public PQPriceVolumeLayer() { }

    public PQPriceVolumeLayer(decimal price = 0m, decimal volume = 0m)
    {
        Price  = price;
        Volume = volume;
    }

    public PQPriceVolumeLayer(IPriceVolumeLayer toClone)
    {
        Price  = toClone.Price;
        Volume = toClone.Volume;
        SetFlagsSame(toClone);
    }

    protected string PQPriceVolumeLayerToStringMembers => $"{nameof(Price)}: {Price:N5}, {nameof(Volume)}: {Volume:N2}";

    public virtual LayerType  LayerType          => LayerType.PriceVolume;
    public virtual LayerFlags SupportsLayerFlags => LayerFlags.Price | LayerFlags.Volume;

    public decimal Price
    {
        get => price;
        set
        {
            if (value == price) return;
            IsPriceUpdated = true;
            price          = value;
        }
    }

    public decimal Volume
    {
        get => volume;
        set
        {
            if (value == volume) return;
            IsVolumeUpdated = true;
            volume          = value;
        }
    }

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

    public virtual bool HasUpdates
    {
        get => UpdatedFlags != 0;
        set => UpdatedFlags = value ? UpdatedFlags.AllFlags() : LayerFieldUpdatedFlags.None;
    }

    public virtual bool IsEmpty
    {
        get => Price == 0m && Volume == 0m;
        set
        {
            if (!value) return;
            Price = Volume = 0m;
        }
    }

    public override void StateReset()
    {
        Price  = 0m;
        Volume = 0m;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsPriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, Price,
                                           quotePublicationPrecisionSetting?.PriceScalingPrecision ?? 1);
        if (!updatedOnly || IsVolumeUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerVolumeOffset, Volume,
                                           quotePublicationPrecisionSetting?.VolumeScalingPrecision ?? 6);
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id >= PQFieldKeys.LayerPriceOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LayerPriceOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            IsPriceUpdated = true;

            Price = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
            return 0;
        }

        if (pqFieldUpdate.Id >= PQFieldKeys.LayerVolumeOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LayerVolumeOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            IsVolumeUpdated = true;

            Volume = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
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

            if (pqpvl.IsPriceUpdated || isFullReplace) Price = pqpvl.Price;

            if (pqpvl.IsVolumeUpdated || isFullReplace) Volume = pqpvl.Volume;

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

    public override string ToString() => $"{GetType().Name}({PQPriceVolumeLayerToStringMembers})";
}
