// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

[Flags]
public enum PQDepthKey : ushort
{
    None      = 0
  , TwoBytes  = 0x80_00 // 32,768
  , AskSide   = 0x40_00 // 16,384
  , DepthMask = 0x3F_FF // 16,383
}

public static class PQDepthKeyExtensions
{
    public const byte   SingleByteIsTwoByteDepth = 0x80;    // 128
    public const byte   SingleByteDepthMask      = 0x3F;    // 63
    public const ushort TwoByteFlagsMask         = 0xC0_00; // 49,152
    public const byte   SingleByteFlagsMask      = 0xC0;    // 192

    public static bool IsSingleByteDepth(this byte firstDepthByte) => (firstDepthByte & SingleByteIsTwoByteDepth) == 0;
    public static bool IsTwoByteDepth(this byte firstDepthByte)    => (firstDepthByte & SingleByteIsTwoByteDepth) == SingleByteIsTwoByteDepth;
    public static bool IsTwoByteDepth(this PQDepthKey depthKey)    => (depthKey & PQDepthKey.TwoBytes) > 0;

    public static byte ToByte(this PQDepthKey depthKey) =>
        (byte)((((ushort)depthKey & TwoByteFlagsMask) >> 8) | ((ushort)depthKey & SingleByteDepthMask));

    public static PQDepthKey ToDepthKey
        (this byte firstDepthByte) =>
        (PQDepthKey)(((firstDepthByte & SingleByteFlagsMask) << 8) | (firstDepthByte & SingleByteDepthMask));

    public static PQDepthKey ToDepthKey(this byte firstDepthByte, byte secondByte) => (PQDepthKey)((firstDepthByte << 8) | secondByte);

    public static bool IsAsk(this PQDepthKey flags) => (flags & PQDepthKey.AskSide) == PQDepthKey.AskSide;

    public static bool IsBid(this PQDepthKey flags) => (flags & PQDepthKey.AskSide) == 0;

    public static PQDepthKey DepthToDepthKey(this ushort depth, bool isAsk = false)
    {
        var isTwoByte = depth > SingleByteDepthMask;
        if (depth > (ushort)PQDepthKey.DepthMask)
            throw new ArgumentException($"Depth exceeded exception can not go above {(ushort)PQDepthKey.DepthMask}.  Got {depth}");
        var additionalFlags = (isTwoByte ? PQDepthKey.TwoBytes : PQDepthKey.None)
                            | (isAsk ? PQDepthKey.AskSide : PQDepthKey.None);
        return (PQDepthKey)depth | additionalFlags;
    }

    public static ushort KeyToDepth(this PQDepthKey flags) => (ushort)(flags & PQDepthKey.DepthMask);
}
