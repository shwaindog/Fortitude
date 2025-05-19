// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

[Flags]
public enum PQQuoteBooleanValues : uint
{
    None = 0

  , IsExecutableSetFlag         = 0x01      
  , IsBidPriceTopChangedSetFlag = 0x02      
  , IsAskPriceTopChangedSetFlag = 0x04      

  , BooleanValuesMask = 0xFF_FF // ChangedSet Flags are reset too

  , IsExecutableUpdatedFlag         = 0x01_00_00         
  , IsBidPriceTopChangedUpdatedFlag = 0x02_00_00         
  , IsAskPriceTopChangedUpdatedFlag = 0x04_00_00         
                                                         
  , BooleanUpdatesMask     = 0xFF_FF_00_00               
  , DefaultEmptyQuoteFlags = 0x00_00_00_01               

  ,  QuoteValuesAndFlagsOnlySet = 0x00_07_00_07
}

public static class PQQuoteBooleanValuesExtensions
{
    public const PQQuoteBooleanValues AllFields =
      PQQuoteBooleanValues.IsExecutableSetFlag
      | PQQuoteBooleanValues.IsBidPriceTopChangedSetFlag
      | PQQuoteBooleanValues.IsAskPriceTopChangedSetFlag;

    public const PQQuoteBooleanValues LivePricingFieldsSetNoReplayOrSnapshots =
        PQQuoteBooleanValues.IsExecutableUpdatedFlag | PQQuoteBooleanValues.IsExecutableSetFlag
                                                     | PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag |
                                                       PQQuoteBooleanValues.IsBidPriceTopChangedSetFlag
                                                     | PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag |
                                                       PQQuoteBooleanValues.IsAskPriceTopChangedSetFlag;

    public const PQQuoteBooleanValues ExpectedL1QuoteSnapshotBooleanValues = 
      PQQuoteBooleanValues.IsExecutableUpdatedFlag | PQQuoteBooleanValues.IsExecutableSetFlag
      | PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag | PQQuoteBooleanValues.IsBidPriceTopChangedSetFlag
      | PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag | PQQuoteBooleanValues.IsAskPriceTopChangedSetFlag;

    public const PQQuoteBooleanValues AllExceptTopChanged =
      PQQuoteBooleanValues.IsExecutableUpdatedFlag | PQQuoteBooleanValues.IsExecutableSetFlag
      | PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag
      | PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag;

    public const PQQuoteBooleanValues AllSet =  
       PQQuoteBooleanValues.IsExecutableUpdatedFlag | PQQuoteBooleanValues.IsExecutableSetFlag
      | PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag | PQQuoteBooleanValues.IsBidPriceTopChangedSetFlag
      | PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag | PQQuoteBooleanValues.IsAskPriceTopChangedSetFlag;

    public const PQQuoteBooleanValues AllUpdated =
      PQQuoteBooleanValues.IsExecutableUpdatedFlag
      | PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag
      | PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag;


    public static bool HasBidTopPriceChangedSet(this PQQuoteBooleanValues fields) => (fields & PQQuoteBooleanValues.IsBidPriceTopChangedSetFlag) > 0;
    public static bool HasAskTopPriceChangedSet(this PQQuoteBooleanValues fields) => (fields & PQQuoteBooleanValues.IsAskPriceTopChangedSetFlag) > 0;

    public static bool HasBidTopPriceChangedUpdated
        (this PQQuoteBooleanValues fields) =>
        (fields & PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag) > 0;

    public static bool HasAskTopPriceChangedUpdated
        (this PQQuoteBooleanValues fields) =>
        (fields & PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag) > 0;

    public static PQQuoteBooleanValues Set(this PQQuoteBooleanValues flags, PQQuoteBooleanValues toSet)     => flags | toSet;
    public static PQQuoteBooleanValues Unset(this PQQuoteBooleanValues flags, PQQuoteBooleanValues toUnset) => flags & ~toUnset;
}
