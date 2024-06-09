// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[Flags]
public enum PQBooleanValues : uint
{
    None                            = 0
  , IsReplayUpdatedFlag             = 0x0001
  , IsReplaySetFlag                 = 0x0002
  , IsExecutableUpdatedFlag         = 0x0004
  , IsExecutableSetFlag             = 0x0008
  , IsBidPriceTopUpdatedChangedFlag = 0x0010
  , IsBidPriceTopUpdatedSetFlag     = 0x0020
  , IsAskPriceTopUpdatedChangedFlag = 0x0040
  , IsAskPriceTopUpdatedSetFlag     = 0x0080
}

public static class PQBooleanValuesExtensions
{
    public const PQBooleanValues AllFields = PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsReplaySetFlag |
                                             PQBooleanValues.IsExecutableUpdatedFlag
                                           | PQBooleanValues.IsExecutableSetFlag | PQBooleanValues.IsBidPriceTopUpdatedChangedFlag
                                           | PQBooleanValues.IsBidPriceTopUpdatedSetFlag | PQBooleanValues.IsAskPriceTopUpdatedChangedFlag
                                           | PQBooleanValues.IsAskPriceTopUpdatedSetFlag;

    public const PQBooleanValues AllExceptExecutableUpdated = PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsReplaySetFlag
      | PQBooleanValues.IsExecutableSetFlag | PQBooleanValues.IsBidPriceTopUpdatedChangedFlag
      | PQBooleanValues.IsBidPriceTopUpdatedSetFlag | PQBooleanValues.IsAskPriceTopUpdatedChangedFlag
      | PQBooleanValues.IsAskPriceTopUpdatedSetFlag;

    public const PQBooleanValues AllSet =
        PQBooleanValues.IsReplaySetFlag | PQBooleanValues.IsExecutableSetFlag
                                        | PQBooleanValues.IsBidPriceTopUpdatedSetFlag | PQBooleanValues.IsAskPriceTopUpdatedSetFlag;

    public const PQBooleanValues AllUpdated =
        PQBooleanValues.IsReplayUpdatedFlag | PQBooleanValues.IsExecutableUpdatedFlag
                                            | PQBooleanValues.IsBidPriceTopUpdatedChangedFlag | PQBooleanValues.IsAskPriceTopUpdatedChangedFlag;

    public static PQBooleanValues Set(this PQBooleanValues flags, PQBooleanValues toSet)     => flags | toSet;
    public static PQBooleanValues Unset(this PQBooleanValues flags, PQBooleanValues toUnset) => flags & ~toUnset;
}
