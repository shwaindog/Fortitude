﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class RecentlyTraded : LastTradedList, IMutableRecentlyTraded
{
    public RecentlyTraded() { }
    public RecentlyTraded(IEnumerable<ILastTrade> lastTrades) : base(lastTrades) { }
    public RecentlyTraded(IRecentlyTraded toClone) : base(toClone) { }
    public RecentlyTraded(ISourceTickerInfo sourceTickerInfo) : base(sourceTickerInfo) { }

    public TimeBoundaryPeriod DuringPeriod { get; set; }

    IMutableRecentlyTraded ITrackableReset<IMutableRecentlyTraded>.ResetWithTracking() => ResetWithTracking();

    IMutableRecentlyTraded IMutableRecentlyTraded.ResetWithTracking() => ResetWithTracking();

    public override RecentlyTraded ResetWithTracking()
    {
        DuringPeriod = TimeBoundaryPeriod.Tick;
        base.ResetWithTracking();
        return this;
    }

    bool IInterfacesComparable<IRecentlyTraded>.AreEquivalent(IRecentlyTraded? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    IRecentlyTraded IRecentlyTraded.Clone() => Clone();

    IMutableRecentlyTraded IMutableRecentlyTraded.Clone() => Clone();


    public override RecentlyTraded CopyFrom
        (ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IRecentlyTraded recentlyTraded)
        {
            DuringPeriod = recentlyTraded.DuringPeriod;
        }
        return this;
    }

    IRecentlyTraded ICloneable<IRecentlyTraded>.Clone() => Clone();

    public override RecentlyTraded Clone() => Recycler?.Borrow<RecentlyTraded>().CopyFrom(this) ?? new RecentlyTraded(this);


    public override bool AreEquivalent(ILastTradedList? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);

        return baseSame;
    }

    public override string ToString() => $"{nameof(RecentlyTraded)}{{{LastTradedListToStringMembers}, {nameof(DuringPeriod)}: {DuringPeriod}}}";
}
