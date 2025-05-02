// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[Flags]
public enum PQBooleanValues : uint
{
    None                        = 0
  , IsReplaySetFlag             = 0x01
  , IsExecutableSetFlag         = 0x02
  , IsBidPriceTopChangedSetFlag = 0x04
  , IsAskPriceTopChangedSetFlag = 0x08

  , BooleanValuesMask = 0x0F  // ChangedSet Flags are reset too
        
  , IsReplayUpdatedFlag             = 0x01_00_00
  , IsExecutableUpdatedFlag         = 0x02_00_00
  , IsBidPriceTopChangedUpdatedFlag = 0x04_00_00
  , IsAskPriceTopChangedUpdatedFlag = 0x08_00_00

  , BooleanUpdatesMask = 0x0F_00_00
}

public static class PQBooleanValuesExtensions
{
    public const PQBooleanValues AllFields = PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsReplaySetFlag |
                                             PQBooleanValues.IsExecutableUpdatedFlag
                                           | PQBooleanValues.IsExecutableSetFlag | PQBooleanValues.IsBidPriceTopChangedUpdatedFlag
                                           | PQBooleanValues.IsBidPriceTopChangedSetFlag | PQBooleanValues.IsAskPriceTopChangedUpdatedFlag
                                           | PQBooleanValues.IsAskPriceTopChangedSetFlag;

    public const PQBooleanValues AllExceptExecutableUpdated = PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsReplaySetFlag
      | PQBooleanValues.IsExecutableSetFlag | PQBooleanValues.IsBidPriceTopChangedUpdatedFlag
      | PQBooleanValues.IsBidPriceTopChangedSetFlag | PQBooleanValues.IsAskPriceTopChangedUpdatedFlag
      | PQBooleanValues.IsAskPriceTopChangedSetFlag;

    public const PQBooleanValues AllExceptTopChanged =
        PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsReplaySetFlag | PQBooleanValues.IsExecutableUpdatedFlag
      | PQBooleanValues.IsExecutableSetFlag | PQBooleanValues.IsBidPriceTopChangedUpdatedFlag | PQBooleanValues.IsAskPriceTopChangedUpdatedFlag;

    public const PQBooleanValues AllSet =
        PQBooleanValues.IsReplaySetFlag | PQBooleanValues.IsExecutableSetFlag
                                        | PQBooleanValues.IsBidPriceTopChangedSetFlag | PQBooleanValues.IsAskPriceTopChangedSetFlag;

    public const PQBooleanValues AllUpdated =
        PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsExecutableUpdatedFlag
                                            | PQBooleanValues.IsBidPriceTopChangedUpdatedFlag | PQBooleanValues.IsAskPriceTopChangedUpdatedFlag;


    public static bool HasBidTopPriceChangedSet(this PQBooleanValues fields) => (fields & PQBooleanValues.IsBidPriceTopChangedSetFlag) > 0;
    public static bool HasAskTopPriceChangedSet(this PQBooleanValues fields) => (fields & PQBooleanValues.IsAskPriceTopChangedSetFlag) > 0;
    public static bool HasBidTopPriceChangedUpdated(this PQBooleanValues fields) => (fields & PQBooleanValues.IsBidPriceTopChangedUpdatedFlag) > 0;
    public static bool HasAskTopPriceChangedUpdated(this PQBooleanValues fields) => (fields & PQBooleanValues.IsAskPriceTopChangedUpdatedFlag) > 0;

    public static PQBooleanValues Set(this PQBooleanValues flags, PQBooleanValues toSet)     => flags | toSet;
    public static PQBooleanValues Unset(this PQBooleanValues flags, PQBooleanValues toUnset) => flags & ~toUnset;
}
