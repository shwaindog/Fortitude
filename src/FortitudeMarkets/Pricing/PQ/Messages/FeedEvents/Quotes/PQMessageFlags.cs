// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

[Flags]
public enum PQMessageFlags : byte
{
    None                 = 0
  , Complete             = 1
  , Snapshot             = 3
  , Update               = 4
  , CompleteUpdate       = 5
  , IncludeReceiverTimes = 8
  , NoChangeOrHeartbeat  = 16
}
