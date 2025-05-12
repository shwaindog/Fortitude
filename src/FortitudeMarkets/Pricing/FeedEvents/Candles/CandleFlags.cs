// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using static FortitudeMarkets.Pricing.FeedEvents.Candles.CandleFlags;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Candles;

[JsonConverter(typeof(JsonStringEnumConverter<CandleFlags>))]
[Flags]
public enum CandleFlags : uint
{
    None                   = 0x00_00_00_00
  , PeriodLatest           = 0x00_00_00_01
  , Updating               = 0x00_00_00_02
  , CreatedFromPreviousEnd = 0x00_00_00_04
  , IsBestPossible         = 0x00_00_00_10
  , FromStorage            = 0x00_00_00_20
  , CalculatedVolumes      = 0x00_00_00_40
  , AllIncompleteMask      = 0xFF_FF_00_00
  , NoIncompleteMask       = 0x00_00_FF_FF
  , IncompleteStartMask    = 0x00_FF_00_00
  , IncompleteEndMask      = 0xFF_00_00_00
  , MissingTicksRange01    = 0x00_01_00_00
  , MissingTicksRange02    = 0x00_02_00_00
  , MissingTicksRange03    = 0x00_04_00_00
  , MissingTicksRange04    = 0x00_08_00_00
  , MissingTicksRange05    = 0x00_10_00_00
  , MissingTicksRange06    = 0x00_20_00_00
  , MissingTicksRange07    = 0x00_40_00_00
  , MissingTicksRange08    = 0x00_80_00_00
  , MissingTicksRange09    = 0x01_00_00_00
  , MissingTicksRange10    = 0x02_00_00_00
  , MissingTicksRange11    = 0x04_00_00_00
  , MissingTicksRange12    = 0x08_00_00_00
  , MissingTicksRange13    = 0x10_00_00_00
  , MissingTicksRange14    = 0x20_00_00_00
  , MissingTicksRange15    = 0x40_00_00_00
  , MissingTicksRange16    = 0x80_00_00_00
  , AllFlags               = 0xFF_FF_00_7F
}

public static class CandleFlagsExtensions
{
    public static ushort MissingTickFlags(this CandleFlags flags) => (ushort)((uint)flags >> 16);

    public static CandleFlags SetFlags(this CandleFlags flags, CandleFlags toSet) => flags | toSet;

    public static CandleFlags UnsetFlags
        (this CandleFlags flags, CandleFlags toUnset) =>
        flags & AllFlags & ~toUnset;

    public static byte CountMissingSet
        (this CandleFlags flags, CandleFlags maskRange = AllIncompleteMask)
    {
        const ushort checkBitMask     = 0x1;
        var          maskMissingRange = (ushort)(((uint)flags & (uint)maskRange) >> 16);

        byte countBits = 0;
        while (maskMissingRange > 0)
        {
            if ((maskMissingRange & checkBitMask) > 0) countBits++;
            maskMissingRange >>= 1;
        }
        return countBits;
    }
}
