﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class OnTickLastTraded : LastTradedList, IMutableOnTickLastTraded
{
    public OnTickLastTraded() { }
    public OnTickLastTraded(IEnumerable<ILastTrade> lastTrades) : base(lastTrades) { }
    public OnTickLastTraded(IOnTickLastTraded toClone) : base(toClone) { }
    public OnTickLastTraded(ISourceTickerInfo sourceTickerInfo) : base(sourceTickerInfo) { }

    IMutableOnTickLastTraded ITrackableReset<IMutableOnTickLastTraded>.ResetWithTracking() => ResetWithTracking();

    IMutableOnTickLastTraded IMutableOnTickLastTraded.ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IMutableLastTrade> ITrackableReset<ITracksResetCappedCapacityList<IMutableLastTrade>>.ResetWithTracking() => ResetWithTracking();

    public override OnTickLastTraded ResetWithTracking()
    {
        base.ResetWithTracking();
        return this;
    }

    IOnTickLastTraded IOnTickLastTraded.Clone() => Clone();

    IMutableOnTickLastTraded IMutableOnTickLastTraded.Clone() => Clone();
    

    public override OnTickLastTraded CopyFrom
        (ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        return this;
    }

    IOnTickLastTraded ICloneable<IOnTickLastTraded>.Clone() => Clone();

    IMutableOnTickLastTraded ICloneable<IMutableOnTickLastTraded>.Clone() => Clone();

    public override OnTickLastTraded Clone() =>
        Recycler?.Borrow<OnTickLastTraded>().CopyFrom(this) ?? new OnTickLastTraded(this);
    

    bool IInterfacesComparable<IOnTickLastTraded>.AreEquivalent(IOnTickLastTraded? other, bool exactTypes) => AreEquivalent(other, exactTypes);
    
    public override bool AreEquivalent(ILastTradedList? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);

        return baseSame;
    }

    public override string ToString() => $"{nameof(OnTickLastTraded)}{{{NonLastTradedListToStringMembers}, {LastTradedListToString}}}";
}
