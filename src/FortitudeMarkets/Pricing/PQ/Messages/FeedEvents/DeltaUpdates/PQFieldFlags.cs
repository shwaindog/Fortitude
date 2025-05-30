// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

[Flags]
public enum PQFieldFlags : byte
{
    None                     = 0
  , IncludesDepth            = 0x80 // 128
  , IncludesSubId            = 0x40 // 64
  , IncludesAuxiliaryPayload = 0x20 // 32
  , NegativeAndScaleMask     = 0x1F // 31
  , NegativeBit              = 0x10 // 16 
  , DecimalScaleBits         = 0x0F // 15

    // IncludesDepth | IncludesExtendedPayLoad = 192
    // IncludesDepth | IncludesAuxiliaryPayload = 160
    // IncludesDepth | IncludesExtendedPayLoad | IncludesAuxiliaryPayload = 224
    // IncludesExtendedPayLoad | IncludesAuxiliaryPayload = 96
}

public static class PQFieldFlagsExtensions
{
    public static bool HasDepthKeyFlag(this PQFieldFlags flags) => (flags & PQFieldFlags.IncludesDepth) > 0;

    public static bool HasBothExtendedPayloadBytesFlags
        (this PQFieldFlags flags) =>
        flags.HasSubIdFlag() && flags.HasAuxiliaryPayloadFlag();

    public static bool HasSubIdFlag(this PQFieldFlags flags) => (flags & PQFieldFlags.IncludesSubId) == PQFieldFlags.IncludesSubId;

    public static bool HasAuxiliaryPayloadFlag
        (this PQFieldFlags flags) =>
        (flags & PQFieldFlags.IncludesAuxiliaryPayload) == PQFieldFlags.IncludesAuxiliaryPayload;
}
