// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;

public interface ICachedRecentCountHistory<out TElement, in TCompare>
    : ITracksShiftsList<TElement, TCompare> where TElement : TCompare { }


public interface IMutableCachedRecentCountHistory<TElement, in TCompare> : IMutableTracksReorderingList<TElement, TCompare>
  , ICachedRecentCountHistory<TElement, TCompare> where TElement : ITrackableReset<TElement>, TCompare
{
    new TElement this[int index] { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new bool HasRandomAccessUpdates { get; set; }
}