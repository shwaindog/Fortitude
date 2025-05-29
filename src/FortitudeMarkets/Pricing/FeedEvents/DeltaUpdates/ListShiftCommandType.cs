// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;

[Flags]
public enum ListShiftCommandType : byte // sent and stored in PQFieldFlags Scaling
{
    ShiftAllElementsAwayFromPinnedIndex = 0    // positive Shift is from pinned (FromIndex) + 1 to end, negative Shift is from pinned -1 to 0
  , ShiftAllElementsTowardPinnedIndex   = 0x01 // positive Shift is from 0 to pinned (FromIndex) -1, negative Shift is from end to pinned + 1

  , MoveSingleElement   = 0x02
  , RemoveElementsRange = 0x04
  , InsertElementsRange = 0x08

  , ListShiftCommandMask = 0x1F

  , PQFieldFlagsReserved  = 0x80
  , PQFieldFlagsReserved1 = 0x40
  , PQFieldFlagsReserved2 = 0x20
}

public static class ListShiftCommandTypeExtensions
{
    public static bool HasMoveSingleElementFlag(this ListShiftCommandType flags)   => (flags & ListShiftCommandType.MoveSingleElement) > 0;
    public static bool HasInsertElementsRangeFlag(this ListShiftCommandType flags) => (flags & ListShiftCommandType.InsertElementsRange) > 0;
}
