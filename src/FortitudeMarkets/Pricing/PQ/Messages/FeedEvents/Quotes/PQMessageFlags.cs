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


public static class PQMessageFlagsExtensions
{
    public static bool HasCompleteFlag(this PQMessageFlags? flags) => (flags & PQMessageFlags.Complete) > 0;
    public static bool HasSnapshotFlag(this PQMessageFlags? flags) => (flags & PQMessageFlags.Snapshot) > 0;
    public static bool HasUpdateFlag(this PQMessageFlags? flags) => (flags & PQMessageFlags.Update) > 0;
    public static bool HasCompleteUpdateFlag(this PQMessageFlags? flags) => (flags & PQMessageFlags.CompleteUpdate) == PQMessageFlags.CompleteUpdate;
    public static bool HasIncludeReceiverTimesFlag(this PQMessageFlags? flags) => (flags & PQMessageFlags.IncludeReceiverTimes) > 0;
    public static bool HasNoChangeOrHeartbeatFlag(this PQMessageFlags? flags) => (flags & PQMessageFlags.NoChangeOrHeartbeat) > 0;
}