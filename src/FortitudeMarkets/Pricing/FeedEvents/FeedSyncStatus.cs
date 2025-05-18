// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents;

[JsonConverter(typeof(JsonStringEnumConverter<FeedSyncStatus>))]
public enum FeedSyncStatus : byte
{
    Good      = 0
  , OutOfSync = 1
  , Stale     = 2
  , FeedDown  = 4
  , NotStarted = 5
}
