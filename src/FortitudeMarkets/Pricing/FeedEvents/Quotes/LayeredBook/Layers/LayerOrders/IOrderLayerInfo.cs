// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

[Flags]
public enum LayerOrderFlags : uint
{
    None                               = 0x00_00_00_00 //
  , ExplicitlyDefinedFromSource        = 0x00_00_00_01 //                     
  , ImpliedFromSourceData              = 0x00_00_00_02 //                     
  , TrackedOnAdapter                   = 0x00_00_00_04 //                    
  , CreatedByAggregator                = 0x00_00_00_08 //                  
  , CreatedBySimulator                 = 0x00_00_00_10 //                      
  , IsExternalCounterPartyOrder        = 0x00_00_00_20 //                   
  , IsInternallyCreatedOrder           = 0x00_00_00_40 // expected to be paired with NotExternalVolume
  , IsSyntheticTrackingOrder           = 0x00_00_00_80 // expected to be paired with NotLayerVolume and CreatedBySimulator
  , HasExternalCounterPartyInfo        = 0x00_00_01_00 //                   
  , HasInternalPartyInfo               = 0x00_00_02_00 //                      
  , HasTrackingId                      = 0x00_00_04_00 //                      
  , EstimatedFromOpenSnapshot          = 0x00_00_08_00 //                     
  , EstimatedFromIntervalSnapshot      = 0x00_00_10_00 //                    
  , ImpliedFromDeltaLayeredBookFeed    = 0x00_00_20_00 //                   
  , FromLayerVolumeDivideByOrdersCount = 0x00_00_40_00 // Filler order created to ensure layer  volume orders
  , CalculatedAggregate                = 0x00_00_80_00 // Used to aggregate many individual orders 
  , CreatedToMatchLayerVolume          = 0x00_01_00_00 // Used to aggregate many individual orders 
  , IsBacktestSubmittedOrder           = 0x00_02_00_00 // expected to be paired with either CreatedFromSimulator or IsSyntheticTrackingOrder
  , NotExternalVolume                  = 0x00_04_00_00 // expected to be paired with IsInternallyCreatedOrder or IsSyntheticTrackingOrder
  , NotLayerVolume                     = 0x00_08_00_00 // expected to be paired with IsSyntheticTrackingOrder
  , CancelRequested                    = 0x00_10_00_00 //                   
  , AmendRequested                     = 0x00_20_00_00 //                    
  , WasAmended                         = 0x00_40_00_00 //                     
  , ConfirmedPartialFill               = 0x00_80_00_00 //                      
  , ImpliedPartialFill                 = 0x01_00_00_00 // when layer volume changes and order at the front of the queue
  , HasGoneUnknown                     = 0x02_00_00_00 //                    
  , HasClosingLinkedOrderInfo          = 0x04_00_00_00 //                         
  , IsInternalPositionDecreasing       = 0x08_00_00_00 //                         
  , IsInternalPositionIncreasing       = 0x10_00_00_00 //                   
  , IsInternalTakeProfit               = 0x20_00_00_00 //                   
  , IsInternalStopLoss                 = 0x40_00_00_00 //                   
  , IsPartialLayeredOrder              = 0x80_00_00_00 //                   
  , ValidFlagsMask                     = 0xFF_FF_FF_FF //
}

public static class LayerOrderFlagsExtensions
{
    public static uint ToUint(this LayerOrderFlags flags) => (uint)flags;

    public static LayerOrderFlags ToLayerOrderFlags(this uint uintflags) => (LayerOrderFlags)uintflags & LayerOrderFlags.ValidFlagsMask;

    public static bool HasExplicitlyDefinedFromSource(this LayerOrderFlags flags)   => (flags & LayerOrderFlags.ExplicitlyDefinedFromSource) > 0;
    public static bool HasImpliedFromSourceData(this LayerOrderFlags flags)         => (flags & LayerOrderFlags.ImpliedFromSourceData) > 0;
    public static bool HasImpliedCreatedByAdapter(this LayerOrderFlags flags)       => (flags & LayerOrderFlags.TrackedOnAdapter) > 0;
    public static bool HasImpliedCreatedAggregator(this LayerOrderFlags flags)      => (flags & LayerOrderFlags.CreatedByAggregator) > 0;
    public static bool HasCreatedFromSimulator(this LayerOrderFlags flags)          => (flags & LayerOrderFlags.CreatedBySimulator) > 0;
    public static bool HasIsExternalCounterPartyOrder(this LayerOrderFlags flags)   => (flags & LayerOrderFlags.IsExternalCounterPartyOrder) > 0;
    public static bool HasIsInternallyCreatedOrder(this LayerOrderFlags flags)      => (flags & LayerOrderFlags.IsInternallyCreatedOrder) > 0;
    public static bool HasIsSyntheticTrackingOrder(this LayerOrderFlags flags)      => (flags & LayerOrderFlags.IsSyntheticTrackingOrder) > 0;
    public static bool HasExternalCounterPartyInfo(this LayerOrderFlags flags)      => (flags & LayerOrderFlags.HasExternalCounterPartyInfo) > 0;
    public static bool HasInternalPartyInfo(this LayerOrderFlags flags)             => (flags & LayerOrderFlags.HasInternalPartyInfo) > 0;
    public static bool HasTrackingId(this LayerOrderFlags flags)                    => (flags & LayerOrderFlags.HasTrackingId) > 0;
    public static bool HasEstimatedFromOpenSnapshot(this LayerOrderFlags flags)     => (flags & LayerOrderFlags.EstimatedFromOpenSnapshot) > 0;
    public static bool HasEstimatedFromIntervalSnapshot(this LayerOrderFlags flags) => (flags & LayerOrderFlags.EstimatedFromIntervalSnapshot) > 0;

    public static bool HasImpliedFromDeltaLayeredBookFeed
        (this LayerOrderFlags flags) =>
        (flags & LayerOrderFlags.ImpliedFromDeltaLayeredBookFeed) > 0;

    public static bool HasFromLayerVolumeDivideByOrdersCount
        (this LayerOrderFlags flags) =>
        (flags & LayerOrderFlags.FromLayerVolumeDivideByOrdersCount) > 0;

    public static bool HasCalculatedAggregate(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CalculatedAggregate) > 0;

    public static bool HasCreatedToMatchLayerVolume(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CreatedToMatchLayerVolume) > 0;
    public static bool HasIsBacktestSubmittedOrder(this LayerOrderFlags flags) => (flags & LayerOrderFlags.IsBacktestSubmittedOrder) > 0;
    public static bool HasNotExternalVolume(this LayerOrderFlags flags) => (flags & LayerOrderFlags.NotExternalVolume) > 0;
    public static bool HasNotLayerVolume(this LayerOrderFlags flags) => (flags & LayerOrderFlags.NotLayerVolume) > 0;
    public static bool HasConfirmedPartialFill(this LayerOrderFlags flags) => (flags & LayerOrderFlags.ConfirmedPartialFill) > 0;
    public static bool HasImpliedPartialFill(this LayerOrderFlags flags) => (flags & LayerOrderFlags.ImpliedPartialFill) > 0;
    public static bool HasCancelRequested(this LayerOrderFlags flags) => (flags & LayerOrderFlags.CancelRequested) > 0;
    public static bool HasAmendRequested(this LayerOrderFlags flags) => (flags & LayerOrderFlags.AmendRequested) > 0;
    public static bool HasWasAmended(this LayerOrderFlags flags) => (flags & LayerOrderFlags.WasAmended) > 0;
    public static bool HasHasGoneUnknown(this LayerOrderFlags flags) => (flags & LayerOrderFlags.HasGoneUnknown) > 0;
    public static bool HasClosingLinkedOrderInfo(this LayerOrderFlags flags) => (flags & LayerOrderFlags.HasClosingLinkedOrderInfo) > 0;
    public static bool HasIsInternalPositionDecreasing(this LayerOrderFlags flags) => (flags & LayerOrderFlags.IsInternalPositionDecreasing) > 0;
    public static bool HasIsInternalPositionIncreasing(this LayerOrderFlags flags) => (flags & LayerOrderFlags.IsInternalPositionIncreasing) > 0;
    public static bool HasIsInternalTakeProfit(this LayerOrderFlags flags) => (flags & LayerOrderFlags.IsInternalTakeProfit) > 0;
    public static bool HasIsInternalStopLoss(this LayerOrderFlags flags) => (flags & LayerOrderFlags.IsInternalStopLoss) > 0;
    public static bool HasIsPartialLayeredOrder(this LayerOrderFlags flags) => (flags & LayerOrderFlags.IsPartialLayeredOrder) > 0;
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
    uint            TrackingId           { get; }
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
    new uint TrackingId { get;          set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal OrderVolume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal OrderRemainingVolume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new bool IsEmpty { get; set; }

    new IMutableAnonymousOrderLayerInfo Clone();
}

public interface IExternalCounterPartyOrderInfo : IReusableObject<IExternalCounterPartyOrderInfo>
  , IInterfacesComparable<IExternalCounterPartyOrderInfo>
  , IAnonymousOrderLayerInfo
{
    string? CounterPartyName { get; }
    string? TraderName       { get; }

    new ICounterPartyOrderLayerInfo Clone();
}

public interface IInternalPartyOrderInfo : IReusableObject<IInternalPartyOrderInfo>, IInterfacesComparable<IInternalPartyOrderInfo>
  , IAnonymousOrderLayerInfo
{
    decimal  TargetPrice                 { get; }
    DateTime CreateDecisionTime          { get; }
    DateTime LastAmendDecisionTime       { get; }
    uint     ParentOrderId               { get; }
    uint     ClosingOrderId              { get; }
    decimal  ClosingOrderOpenPrice       { get; }
    uint     DivisionId                  { get; }
    string?  DivisionName                { get; }
    uint     DeskId                      { get; }
    string?  DeskName                    { get; }
    uint     StrategyId                  { get; }
    string   StrategyName                { get; }
    uint     StrategyDecisionId          { get; }
    string?  StrategyDecisionDescription { get; }
    uint     PortfolioId                 { get; }
    string?  PortfolioName               { get; }
    uint     InternalTraderId            { get; }
    string?  InternalTraderName          { get; }
    decimal  MarginConsumed              { get; }

    new ICounterPartyOrderLayerInfo Clone();
}

public interface ICounterPartyOrderLayerInfo : IReusableObject<ICounterPartyOrderLayerInfo>, IInterfacesComparable<ICounterPartyOrderLayerInfo>
  , IAnonymousOrderLayerInfo
{
    uint ExternalCounterPartyId { get; }
    string? ExternalCounterPartyName { get; }
    uint ExternalTraderId       { get; }
    string? ExternalTraderName       { get; }

    new ICounterPartyOrderLayerInfo Clone();
}

public interface IMutableCounterPartyOrderLayerInfo : ICounterPartyOrderLayerInfo, IMutableAnonymousOrderLayerInfo
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new uint ExternalCounterPartyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    new string? ExternalCounterPartyName { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new uint ExternalTraderId { get;      set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    new string? ExternalTraderName { get; set; }

    new IMutableCounterPartyOrderLayerInfo Clone();
}
