// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQOrdersPriceVolumeLayer : IMutableOrdersPriceVolumeLayer, IPQOrdersCountPriceVolumeLayer
  , IPQSupportsStringUpdates<IPriceVolumeLayer>, ISupportsPQNameIdLookupGenerator
{
    new IPQAnonymousOrderLayerInfo? this[int index] { get; set; }
    new IPQNameIdLookupGenerator                  NameIdLookup { get; set; }
    new IReadOnlyList<IPQAnonymousOrderLayerInfo> Orders       { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int OrdersShifted { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsOrdersShiftedUpdated { get; set; }

    new IPQOrdersPriceVolumeLayer Clone();
}

public class PQOrdersPriceVolumeLayer : PQOrdersCountPriceVolumeLayer, IPQOrdersPriceVolumeLayer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrdersPriceVolumeLayer));

    private static readonly IReadOnlyList<IPQAnonymousOrderLayerInfo> EmptyOrders = new List<IPQAnonymousOrderLayerInfo>().AsReadOnly();

    private readonly bool isCounterPartyOrders;

    private readonly IList<IPQAnonymousOrderLayerInfo>? orders;

    private IPQNameIdLookupGenerator nameIdLookup = null!;

    private int ordersShifted;


    public PQOrdersPriceVolumeLayer(LayerType layerType, IPQNameIdLookupGenerator initialDictionary)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };

        NameIdLookup = initialDictionary;
        orders       = new List<IPQAnonymousOrderLayerInfo>(0);
        if (GetType() == typeof(PQOrdersPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    public PQOrdersPriceVolumeLayer
    (IPQNameIdLookupGenerator traderIdToNameLookup, LayerType layerType, decimal price = 0m, decimal volume = 0m, uint ordersCount = 0
      , decimal internalVolume = 0,
        IEnumerable<IAnonymousOrderLayerInfo>? layerOrders = null)
        : base(price, volume, ordersCount, internalVolume)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };

        NameIdLookup = traderIdToNameLookup;
        orders       = new List<IPQAnonymousOrderLayerInfo>(0);
        if (layerOrders is not null)
            foreach (var orderLayerInfo in layerOrders)
                CopyAddLayer(orderLayerInfo);
        if (GetType() == typeof(PQOrdersPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    public PQOrdersPriceVolumeLayer(IPriceVolumeLayer toClone, LayerType layerType, IPQNameIdLookupGenerator ipqNameIdLookupGenerator) : base(toClone)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };
        NameIdLookup = ipqNameIdLookupGenerator;
        if (toClone is IOrdersPriceVolumeLayer ordersToClone)
        {
            orders = new List<IPQAnonymousOrderLayerInfo>((int)ordersToClone.OrdersCount);
            foreach (var orderLayerInfo in ordersToClone.Orders) CopyAddLayer(orderLayerInfo);
        }
        else
        {
            orders = new List<IPQAnonymousOrderLayerInfo>(0);
        }
        SetFlagsSame(toClone);
        if (GetType() == typeof(PQOrdersPriceVolumeLayer)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQJustOrdersToStringMembers => $"{nameof(Orders)}: [{string.Join(", ", Orders)}]";

    protected string PQOrdersPriceVolumeLayerToStringMembers => $"{PQOrdersCountVolumeLayerToStringMembers}, {PQJustOrdersToStringMembers}";

    private int SafeOrdersLength => orders?.Count ?? 0;


    [JsonIgnore]
    IReadOnlyList<IAnonymousOrderLayerInfo> IOrdersPriceVolumeLayer.Orders =>
        orders?.Where(aoli => !aoli.IsEmpty).ToList().AsReadOnly() ?? EmptyOrders;

    [JsonIgnore]
    IReadOnlyList<IMutableAnonymousOrderLayerInfo> IMutableOrdersPriceVolumeLayer.Orders =>
        orders?.Where(aoli => !aoli.IsEmpty).ToList().AsReadOnly() ?? EmptyOrders;

    public IReadOnlyList<IPQAnonymousOrderLayerInfo> Orders => orders?.Where(aoli => !aoli.IsEmpty).ToList().AsReadOnly() ?? EmptyOrders;


    [JsonIgnore] public override LayerType LayerType => isCounterPartyOrders ? LayerType.OrdersFullPriceVolume : LayerType.OrdersAnonymousPriceVolume;


    [JsonIgnore]
    public override LayerFlags SupportsLayerFlags =>
        base.SupportsLayerFlags |
        (isCounterPartyOrders
            ? LayerFlagsExtensions.AdditionalCounterPartyOrderFlags
            : LayerFlagsExtensions.AdditionalAnonymousOrderFlags);

    [JsonIgnore]
    IMutableAnonymousOrderLayerInfo? IMutableOrdersPriceVolumeLayer.this[int i]
    {
        get => this[i];
        set => this[i] = value as IPQAnonymousOrderLayerInfo;
    }

    [JsonIgnore] IAnonymousOrderLayerInfo? IOrdersPriceVolumeLayer.this[int i] => this[i];


    [JsonIgnore]
    public IPQAnonymousOrderLayerInfo? this[int i]
    {
        get
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = SafeOrdersLength; j <= i; j++) AppendLayer(CreateNewBookOrderLayer());
            return orders?[i];
        }
        set
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = SafeOrdersLength; j < i; j++) AppendLayer(CreateNewBookOrderLayer());
            if (i >= SafeOrdersLength && value != null)
            {
                orders?.Add(value);
                if (value is IPQCounterPartyOrderLayerInfo pqCounterPartyOrder) pqCounterPartyOrder.NameIdLookup = NameIdLookup;
            }
            else if (orders != null)
            {
                if (value != null)
                {
                    orders[i] = value;
                    if (value is IPQCounterPartyOrderLayerInfo pqCounterPartyOrder) pqCounterPartyOrder.NameIdLookup = NameIdLookup;
                }
                else
                {
                    orders[i].StateReset();
                }
            }
        }
    }

    public int OrdersShifted
    {
        get => ordersShifted;
        set
        {
            if (value == ordersShifted) return;
            IsOrdersShiftedUpdated = true;
            ordersShifted          = value;
        }
    }

    [JsonIgnore]
    public bool IsOrdersShiftedUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.OrdersIndexesShiftedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.OrdersIndexesShiftedFlag;

            else if (IsOrdersShiftedUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.OrdersIndexesShiftedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && (orders?.All(aoli => aoli.IsEmpty) ?? true);
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            foreach (var traderLayerInfo in Orders) traderLayerInfo.IsEmpty = true;
            ordersShifted = 0;
            base.IsEmpty  = true;
        }
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get { return base.HasUpdates || (orders?.Any(aoli => aoli.HasUpdates) ?? false); }
        set
        {
            base.HasUpdates = value;
            if (value) return;
            ordersShifted = 0;
            foreach (var pqaoli in orders ?? []) pqaoli.HasUpdates = value;
            NameIdLookup.HasUpdates = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override uint OrdersCount
    {
        get
        {
            var calcOrderCount = CountFromOrders();
            if (calcOrderCount > 0) return calcOrderCount;
            return base.OrdersCount;
        }
        set
        {
            for (var i = SafeOrdersLength - 1; i >= value; i--)
            {
                var layerAtLevel                                          = orders?[i];
                if (!layerAtLevel?.IsEmpty ?? true) layerAtLevel!.IsEmpty = true;
            }
            base.OrdersCount = value;
        }
    }

    public override decimal InternalVolume
    {
        get
        {
            var calcInternalVolume = InternalVolumeFromOrders();
            if (calcInternalVolume > 0) return calcInternalVolume;
            return base.InternalVolume;
        }
        set => base.InternalVolume = value;
    }

    [JsonIgnore] INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    [JsonIgnore]
    public virtual IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (nameIdLookup == value) return;
            nameIdLookup = value;
            foreach (var pqaoli in Orders.OfType<ISupportsPQNameIdLookupGenerator>()) pqaoli.NameIdLookup = nameIdLookup;
        }
    }

    public override void UpdateComplete()
    {
        NameIdLookup.UpdateComplete();
        base.UpdateComplete();
    }

    public bool RemoveAt(int index)
    {
        orders?[index].StateReset();
        return true;
    }

    public void ShiftOrders(int offset)
    {
        var nonEmptyOrders = (int)CountFromOrders();
        if (nonEmptyOrders == 0 || orders == null) return;
        OrdersShifted = offset;
        if (offset > 0)
            for (var i = 0; i < offset; i++)
            {
                IPQAnonymousOrderLayerInfo? toInsert;
                if (orders[^1].IsEmpty)
                {
                    var allOrdersCount = orders.Count;
                    toInsert = orders[allOrdersCount - 1];
                    orders.RemoveAt(allOrdersCount - 1);
                }
                else
                {
                    toInsert = CreateNewBookOrderLayer();
                }
                orders.Insert(0, toInsert);
            }
        else if (offset < 0 && orders != null)
            for (var i = offset; i < 0; i++)
            {
                var toResetAtEnd = orders[0];
                orders.RemoveAt(0);
                toResetAtEnd.StateReset();
                orders.Add(toResetAtEnd);
            }
    }

    public override void StateReset()
    {
        ordersShifted = 0;
        foreach (var pqTraderLayerInfo in Orders) pqTraderLayerInfo.StateReset();
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

        var numberOfTraderInfos = Math.Min(ushort.MaxValue, SafeOrdersLength);
        for (ushort i = 0; i < numberOfTraderInfos && i < SafeOrdersLength; i++)
        {
            var tli = orders?[i];
            if (tli == null) continue;
            foreach (var orderFu in tli.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
                yield return orderFu.WithAuxiliary(i);
            if (i + 1 == numberOfTraderInfos) break;
        }
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id is PQQuoteFields.LayerOrders)
        {
            var index          = pqFieldUpdate.AuxiliaryPayload;
            var orderLayerInfo = this[index]!;
            return orderLayerInfo.UpdateField(pqFieldUpdate);
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        var numberOfTraderInfos = Math.Min(ushort.MaxValue, SafeOrdersLength);
        for (ushort i = 0; i < numberOfTraderInfos && i < SafeOrdersLength; i++)
        {
            var tli = orders?[i] as IPQCounterPartyOrderLayerInfo;
            if (tli is null or { IsEmpty: true, HasUpdates: false } || i + 1 == ushort.MaxValue) continue;
            foreach (var stringUpdate in tli.GetStringUpdates(snapShotTime, messageFlags))
                yield return stringUpdate.WithAuxiliary(i);
            if (i + 1 == numberOfTraderInfos) break;
        }
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQQuoteFields.LayerNameDictionaryUpsertCommand) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    IPQOrdersPriceVolumeLayer IPQOrdersPriceVolumeLayer.Clone() => Clone();

    IOrdersPriceVolumeLayer ICloneable<IOrdersPriceVolumeLayer>.Clone() => Clone();

    IOrdersPriceVolumeLayer IOrdersPriceVolumeLayer.Clone() => Clone();

    IMutableOrdersPriceVolumeLayer IMutableOrdersPriceVolumeLayer.Clone() => Clone();

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IOrdersPriceVolumeLayer traderPvLayer)) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);
        var traderDetailsSame = Orders.Zip(traderPvLayer.Orders, (ftd, std) =>
                                               ftd.AreEquivalent(std, exactTypes)).All(same => same);

        var allAreSame = baseSame && traderDetailsSame;
        return allAreSame;
    }

    public void Add(IAnonymousOrderLayerInfo order)
    {
        var indexToUpdate = (int)OrdersCount;
        AssertMaxTraderSizeNotExceeded(indexToUpdate);
        if (indexToUpdate >= SafeOrdersLength)
        {
            CopyAddLayer(order);
        }
        else
        {
            var entryToUpdate = this[indexToUpdate]!;
            entryToUpdate.CopyFrom(order, CopyMergeFlags.FullReplace);
        }
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var isFullReplace = copyMergeFlags.HasFullReplace();
        var pqopvl        = source as IPQOrdersPriceVolumeLayer;
        var opvl          = source as IOrdersPriceVolumeLayer;
        if (pqopvl != null && !copyMergeFlags.HasSkipReferenceLookups()) NameIdLookup.CopyFrom(pqopvl.NameIdLookup, copyMergeFlags);
        if (opvl != null)
        {
            for (var j = 0; j < opvl.Orders.Count; j++)
            {
                var sourceOrder = opvl[j];
                var destOrder   = this[j];
                destOrder!.CopyFrom(sourceOrder!, copyMergeFlags);
            }
            for (var i = opvl.Orders.Count; i < SafeOrdersLength; i++)
                if (orders?[i] is { IsEmpty: false } makeEmpty)
                    makeEmpty.IsEmpty = true;
        }
        base.CopyFrom(source, copyMergeFlags);
        if (pqopvl != null && isFullReplace) SetFlagsSame(source);

        return this;
    }

    public override PQOrdersPriceVolumeLayer Clone() => new(this, LayerType, NameIdLookup);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ orders?.GetHashCode() ?? 0;
        }
    }

    public override string ToString() => $"{GetType().Name}({PQOrdersPriceVolumeLayerToStringMembers}, {UpdatedFlagsToString})";

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private void AssertMaxTraderSizeNotExceeded(int proposedNewIndex)
    {
        if (proposedNewIndex > ushort.MaxValue)
            throw new ArgumentOutOfRangeException($"Max Traders represented is {ushort.MaxValue}. Got {proposedNewIndex}");
    }

    private void AppendLayer(IPQAnonymousOrderLayerInfo toAdd)
    {
        orders?.Add(toAdd);
    }

    private void CopyAddLayer(IAnonymousOrderLayerInfo toAdd)
    {
        orders?.Add(ConvertToBookLayer(toAdd));
    }

    public IPQAnonymousOrderLayerInfo ConvertToBookLayer(IAnonymousOrderLayerInfo toAdd)
    {
        if (LayerType.SupportsOrdersFullPriceVolume())
            return new PQCounterPartyOrderLayerInfo(toAdd, NameIdLookup)
            {
                Recycler = Recycler
            };
        return new PQAnonymousOrderLayerInfo(toAdd)
        {
            Recycler = Recycler
        };
    }

    public IPQAnonymousOrderLayerInfo CreateNewBookOrderLayer()
    {
        if (LayerType.SupportsOrdersFullPriceVolume())
            return new PQCounterPartyOrderLayerInfo(NameIdLookup)
            {
                Recycler = Recycler
            };
        return new PQAnonymousOrderLayerInfo
        {
            Recycler = Recycler
        };
    }

    protected uint CountFromOrders()
    {
        for (var i = SafeOrdersLength - 1; i >= 0; i--)
        {
            var layerAtLevel = orders?[i];
            if (!layerAtLevel?.IsEmpty ?? true) return (uint)(i + 1);
        }
        return 0;
    }

    protected decimal InternalVolumeFromOrders()
    {
        return Orders
               .Where(aoli =>
                          aoli.OrderFlags.HasIsInternallyCreatedOrder()
                       && !aoli.OrderFlags.HasNotLayerVolume()
                       && !aoli.OrderFlags.HasIsSyntheticTrackingOrder())
               .Sum(aoli => aoli.OrderRemainingVolume);
    }
}
