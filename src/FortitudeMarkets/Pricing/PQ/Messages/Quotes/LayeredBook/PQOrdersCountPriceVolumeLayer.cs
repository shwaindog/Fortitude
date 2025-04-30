// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQOrdersCountPriceVolumeLayer : IMutableOrdersCountPriceVolumeLayer, IPQPriceVolumeLayer
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrdersCountUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsInternalVolumeUpdated { get; set; }

    new IPQOrdersCountPriceVolumeLayer Clone();
}

public class PQOrdersCountPriceVolumeLayer : PQPriceVolumeLayer, IPQOrdersCountPriceVolumeLayer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrdersPriceVolumeLayer));

    private decimal internalVolume;
    private uint    ordersCount;

    public PQOrdersCountPriceVolumeLayer()
    {
        if (GetType() == typeof(PQOrdersCountPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    public PQOrdersCountPriceVolumeLayer
        (decimal price = 0m, decimal volume = 0m, uint ordersCount = 0u, decimal internalVolume = 0m) : base(price, volume)
    {
        OrdersCount    = ordersCount;
        InternalVolume = internalVolume;
        if (GetType() == typeof(PQOrdersCountPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    public PQOrdersCountPriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is IOrdersCountPriceVolumeLayer ordersCountPvl)
        {
            OrdersCount    = ordersCountPvl.OrdersCount;
            InternalVolume = ordersCountPvl.InternalVolume;
        }
        SetFlagsSame(toClone);
        if (GetType() == typeof(PQOrdersCountPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQOrdersCountVolumeLayerToStringMembers =>
        $"{PQPriceVolumeLayerToStringMembers}, {nameof(OrdersCount)}: {OrdersCount}, {nameof(InternalVolume)}: {InternalVolume:N2}";


    IOrdersCountPriceVolumeLayer ICloneable<IOrdersCountPriceVolumeLayer>.Clone() => (IOrdersCountPriceVolumeLayer)Clone();


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual uint OrdersCount
    {
        get => ordersCount;
        set
        {
            IsOrdersCountUpdated |= ordersCount != value;
            ordersCount          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual decimal InternalVolume
    {
        get => internalVolume;
        set
        {
            IsInternalVolumeUpdated |= internalVolume != value;
            internalVolume          =  value;
        }
    }

    [JsonIgnore] public decimal ExternalVolume => Volume - InternalVolume;


    [JsonIgnore]
    public bool IsOrdersCountUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.OrdersCountUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.OrdersCountUpdatedFlag;

            else if (IsOrdersCountUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.OrdersCountUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsInternalVolumeUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.OrdersInternalVolumeUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.OrdersInternalVolumeUpdatedFlag;

            else if (IsInternalVolumeUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.OrdersInternalVolumeUpdatedFlag;
        }
    }

    [JsonIgnore] public override LayerType LayerType => LayerType.OrdersCountPriceVolume;

    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalOrdersCountFlags | base.SupportsLayerFlags;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => OrdersCount == 0 && InternalVolume == 0m && base.IsEmpty;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            OrdersCount    = 0;
            InternalVolume = 0m;
        }
    }

    public override void StateReset()
    {
        OrdersCount    = 0;
        InternalVolume = 0m;
        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;

        if (!updatedOnly || IsOrdersCountUpdated) yield return new PQFieldUpdate(PQQuoteFields.OrdersCount, OrdersCount);

        if (!updatedOnly || IsInternalVolumeUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.InternalVolume, InternalVolume,
                                           quotePublicationPrecisionSetting?.VolumeScalingPrecision ?? (PQFieldFlags)6);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id == PQQuoteFields.OrdersCount)
        {
            IsOrdersCountUpdated = true; // incase of reset and sending 0;
            OrdersCount          = pqFieldUpdate.Payload;
            return 0;
        }

        if (pqFieldUpdate.Id == PQQuoteFields.InternalVolume)
        {
            IsInternalVolumeUpdated = true; // incase of reset and sending 0;
            InternalVolume          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var isFullReplace = copyMergeFlags.HasFullReplace();
        var pqocpvl       = source as IPQOrdersCountPriceVolumeLayer;
        if (source is IOrdersCountPriceVolumeLayer ocpvl && pqocpvl == null)
        {
            OrdersCount    = ocpvl.OrdersCount;
            InternalVolume = ocpvl.InternalVolume;
        }
        else if (pqocpvl != null)
        {
            if (pqocpvl.IsOrdersCountUpdated || isFullReplace) OrdersCount       = pqocpvl.OrdersCount;
            if (pqocpvl.IsInternalVolumeUpdated || isFullReplace) InternalVolume = pqocpvl.InternalVolume;
        }
        if (pqocpvl != null && isFullReplace) SetFlagsSame(source);

        return this;
    }

    IMutableOrdersCountPriceVolumeLayer IMutableOrdersCountPriceVolumeLayer.Clone() => Clone();

    IOrdersCountPriceVolumeLayer IOrdersCountPriceVolumeLayer.Clone() => Clone();

    IPQOrdersCountPriceVolumeLayer IPQOrdersCountPriceVolumeLayer.Clone() => Clone();

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IOrdersCountPriceVolumeLayer ordersCountPvLayer)) return false;
        var baseSame           = base.AreEquivalent(other, exactTypes);
        var countSame          = OrdersCount == ordersCountPvLayer.OrdersCount;
        var internalVolumeSame = InternalVolume == ordersCountPvLayer.InternalVolume;

        var allAreSame = baseSame && countSame && internalVolumeSame;
        return allAreSame;
    }

    public override PQOrdersCountPriceVolumeLayer Clone() => new(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = ((int)OrdersCount * 397) ^ hashCode;
            hashCode = (InternalVolume.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PQOrdersCountPriceVolumeLayer)}({PQOrdersCountVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
