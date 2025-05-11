// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[Flags]
public enum LayerBooleanFlags : ushort
{
    None             = 0x00_00
  , IsExecutableFlag = 0x00_01

  , ValidFlagsMask = 0x00_01
}

public static class LayerBooleanFlagsExtensions
{
    public static uint              ToUInt(this LayerBooleanFlags flags)     => (uint)flags;
    public static LayerBooleanFlags ToLayerBooleanFlags(this uint uintFlags) => (LayerBooleanFlags)uintFlags & LayerBooleanFlags.ValidFlagsMask;
}
