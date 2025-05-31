// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;

public interface IExpiringCachedPeriodUpdateHistory<out TElement, in TCompare>
    : ICachedRecentCountHistory<TElement, TCompare> where TElement : TCompare
{
    DateTime UpdateTime { get; }

    TimeBoundaryPeriod DuringPeriod { get; }
}

public interface IMutableExpiringCachedPeriodUpdateHistory<TElement, in TCompare> : IExpiringCachedPeriodUpdateHistory<TElement, TCompare>
  , IMutableCachedRecentCountHistory<TElement, TCompare> where TElement : ITrackableReset<TElement>, TCompare
{
    new DateTime UpdateTime { get; set; }

    new TimeBoundaryPeriod DuringPeriod { get; set; }
}
