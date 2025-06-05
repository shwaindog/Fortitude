
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

[Flags]
public enum LayerBooleanValues : uint
{
    None = 0
   , LayerBehaviorFlagsReserved = 0x00_00_FF_FF
   , Executable = 0x00_01_00_00

   , ValidBoolValuesMask = 0xFF_FF_00_00
}

public static class LayerBooleanValuesExtensions
{
    public static bool HasExecutable(this LayerBooleanValues flags)   => (flags & LayerBooleanValues.Executable) > 0;

    public static QuoteLayerInstantBehaviorFlags ExtractLayerBehaviorFlags(this LayerBooleanValues flags) => 
        (QuoteLayerInstantBehaviorFlags)(flags & LayerBooleanValues.LayerBehaviorFlagsReserved);

    public static LayerBooleanValues ToLayerBooleanValues(this uint uintFlags) => (LayerBooleanValues)uintFlags;

    public static LayerBooleanValues JustLayerBooleanValuesMask(this uint uintFlags) => (LayerBooleanValues)uintFlags & LayerBooleanValues.ValidBoolValuesMask;

    public static uint JustLayerBooleanValuesMask(this LayerBooleanValues flags) => ((uint)flags & (uint)LayerBooleanValues.ValidBoolValuesMask);

    public static bool HasAllOf(this LayerBooleanValues flags, LayerBooleanValues checkAllFound)   => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this LayerBooleanValues flags, LayerBooleanValues checkNonAreSet) => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this LayerBooleanValues flags, LayerBooleanValues checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this LayerBooleanValues flags, LayerBooleanValues checkAllFound)   => flags == checkAllFound;
}