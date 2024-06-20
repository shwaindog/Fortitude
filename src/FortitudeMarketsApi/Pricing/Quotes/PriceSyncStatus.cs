// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Pricing.Quotes;

public enum PriceSyncStatus : byte
{
    OutOfSync = 0
  , Good      = 1
  , Stale     = 2
  , FeedDown  = 4
}
