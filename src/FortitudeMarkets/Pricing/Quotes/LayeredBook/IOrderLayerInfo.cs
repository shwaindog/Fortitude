// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

[Flags]
public enum LayerOrderFlags : uint
{
    None                       = 0
  , CreatedFromSource          = 0x00_00_00_01
  , CreatedFromAdapter         = 0x00_00_00_02
  , CreatedFromSimulator       = 0x00_00_00_04
  , CreatedFromRouter          = 0x00_00_00_08
  , CreatedFromVirtualSource   = 0x00_00_00_10
  , CreatedLocally             = 0x00_00_00_20
  , CreatedFromOpenSnapshot    = 0x00_00_00_40
  , CreatedFromOrderFeed       = 0x00_00_00_80
  , CreatedFromLayeredBookFeed = 0x00_00_01_00
  , GeneratedToMeetLayerTotals
        = 0x00_00_02_00 // Filler order created to ensure layer InternalVolume and OrdersCount adds up to sum of all layer volume orders
  , GeneratedFromCount = 0x00_00_04_00 // Orders are implied from layer volume / number of orders 
  , GeneratedToSimulateQueueOrder = 0x00_00_08_00 // Generated to simulate queue order
  , CalculatedAggregate = 0x00_00_10_00 // Used to aggregate many individual orders infront of and behind synthetic/virtual simulation orders
  , CalculatedVolumeFromAverages = 0x00_00_20_00
  , NotSourceOrderId = 0x00_00_40_00 // expected to be paired with either GeneratedFromCount or GeneratedToSimulateQueueOrder
  , IsSyntheticTrackingOrder = 0x00_00_80_00 // expected to be paired with NotLayerVolume
  , IsInternallyCreatedOrder = 0x00_01_00_00 // expected to be paired with NotExternalVolume
  , NotExternalVolume = 0x00_02_00_00 // expected to be paired with IsInternallyCreatedOrder
  , NotLayerVolume = 0x00_04_00_00 // expected to be paired with IsSyntheticTrackingOrder
  , CancelRequested = 0x00_08_00_00
  , AmendRequested = 0x00_10_00_00
  , VolumeAmendedBySource = 0x00_20_00_00
  , ConfirmPartialFill = 0x00_40_00_00
  , CalculatedPartialFill = 0x00_80_00_00 // when layer volume changes and order at the front of the queue
  , FullPendingExecution = 0x01_00_00_00
  , PartialPendingExecution = 0x02_00_00_00
  , Stale = 0x04_00_00_00
  , Unknown = 0x08_00_00_00

  , ValidFlagsMask = 0x08_FF_FF_FF
}

public static class LayerOrderFlagsExtensions
{
    public static uint ToUint(this LayerOrderFlags flags) => (uint)flags;

    public static LayerOrderFlags ToLayerOrderFlags(this uint uintflags) => (LayerOrderFlags)uintflags & LayerOrderFlags.ValidFlagsMask;

    public static bool HasCreatedFromSource(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedFromSource) > 0;
    public static bool HasCreatedFromAdapter(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedFromAdapter) > 0;
    public static bool HasCreatedFromSimulator(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedFromSimulator) > 0;
    public static bool HasCreatedFromRouter(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedFromRouter) > 0;
    public static bool HasCreatedFromVirtualSource(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedFromVirtualSource) > 0;
    public static bool HasCreatedLocally(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedLocally) > 0;
    public static bool HasCreatedFromOpenSnapshot(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedFromOpenSnapshot) > 0;
    public static bool HasCreatedFromOrderFeed(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedFromOrderFeed) > 0;
    public static bool HasCreatedFromLayeredBookFeed(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedFromLayeredBookFeed) > 0;
    public static bool HasGeneratedToMeetLayerTotals(this LayerOrderFlags flags) => (flags & LayerOrderFlags.GeneratedToMeetLayerTotals) > 0;
    public static bool HasGeneratedFromCount(this LayerOrderFlags flags) => (flags & LayerOrderFlags.GeneratedFromCount) > 0;
    public static bool HasGeneratedToSimulateQueueOrder(this LayerOrderFlags flags) => (flags & LayerOrderFlags.GeneratedToSimulateQueueOrder) > 0;
    public static bool HasCalculatedAggregate(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CalculatedAggregate) > 0;
    public static bool HasCalculatedVolumeFromAverages(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CalculatedVolumeFromAverages) > 0;
    public static bool HasNotSourceOrderId(this LayerOrderFlags flags) => (flags & LayerOrderFlags.NotSourceOrderId) > 0;
    public static bool HasIsSyntheticTrackingOrder(this LayerOrderFlags flags) => (flags & LayerOrderFlags.IsSyntheticTrackingOrder) > 0;
    public static bool HasIsInternallyCreatedOrder(this LayerOrderFlags flags) => (flags & LayerOrderFlags.IsInternallyCreatedOrder) > 0;
    public static bool HasNotExternalVolume(this LayerOrderFlags flags) => (flags & LayerOrderFlags.NotExternalVolume) > 0;
    public static bool HasNotLayerVolume(this LayerOrderFlags flags) => (flags & LayerOrderFlags.NotLayerVolume) > 0;
    public static bool HasCancelRequested(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CancelRequested) > 0;
    public static bool HasAmendRequested(this LayerOrderFlags flags) => (flags & LayerOrderFlags.AmendRequested) > 0;
    public static bool HasVolumeAmendedBySource(this LayerOrderFlags flags) => (flags & LayerOrderFlags.VolumeAmendedBySource) > 0;
    public static bool HasConfirmPartialFill(this LayerOrderFlags flags) => (flags & LayerOrderFlags.ConfirmPartialFill) > 0;
    public static bool HasCalculatedPartialFill(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CalculatedPartialFill) > 0;
    public static bool HasFullPendingExecution(this LayerOrderFlags flags) => (flags & LayerOrderFlags.FullPendingExecution) > 0;
    public static bool HasPartialPendingExecution(this LayerOrderFlags flags) => (flags & LayerOrderFlags.PartialPendingExecution) > 0;
    public static bool HaStale(this LayerOrderFlags flags) => (flags & LayerOrderFlags.Stale) > 0;
    public static bool HasUnknown(this LayerOrderFlags flags) => (flags & LayerOrderFlags.Unknown) > 0;
    public static bool HasAllOf(this LayerOrderFlags flags, LayerOrderFlags checkAllFound) => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this LayerOrderFlags flags, LayerOrderFlags checkNonAreSet) => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this LayerOrderFlags flags, LayerOrderFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this LayerOrderFlags flags, LayerOrderFlags checkAllFound) => flags == checkAllFound;
}

[JsonDerivedType(typeof(AnonymousOrderLayerInfo))]
[JsonDerivedType(typeof(CounterPartyOrderLayerInfo))]
[JsonDerivedType(typeof(PQAnonymousOrderLayerInfo))]
[JsonDerivedType(typeof(PQCounterPartyOrderLayerInfo))]
public interface IAnonymousOrderLayerInfo : IReusableObject<IAnonymousOrderLayerInfo>, IInterfacesComparable<IAnonymousOrderLayerInfo>
{
    int             OrderId              { get; }
    LayerOrderFlags OrderFlags           { get; }
    DateTime        CreatedTime          { get; }
    DateTime        UpdatedTime          { get; }
    decimal         OrderVolume          { get; }
    decimal         OrderRemainingVolume { get; }
    bool            IsEmpty              { get; }
}

public interface IMutableAnonymousOrderLayerInfo : IAnonymousOrderLayerInfo, IInterfacesComparable<IMutableAnonymousOrderLayerInfo>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new int OrderId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new LayerOrderFlags OrderFlags { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new DateTime CreatedTime { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new DateTime UpdatedTime { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal OrderVolume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal OrderRemainingVolume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new bool IsEmpty { get; set; }

    new IMutableAnonymousOrderLayerInfo Clone();
}

public interface ICounterPartyOrderLayerInfo : IReusableObject<ICounterPartyOrderLayerInfo>, IInterfacesComparable<ICounterPartyOrderLayerInfo>
  , IAnonymousOrderLayerInfo
{
    string? CounterPartyName { get; }
    string? TraderName       { get; }

    new ICounterPartyOrderLayerInfo Clone();
}

public interface IMutableCounterPartyOrderLayerInfo : ICounterPartyOrderLayerInfo, IMutableAnonymousOrderLayerInfo
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    new string? CounterPartyName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    new string? TraderName { get; set; }

    new IMutableCounterPartyOrderLayerInfo Clone();
}
