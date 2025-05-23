﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;


public interface IRecentlyTraded : ILastTradedList, IInterfacesComparable<IRecentlyTraded>, ICloneable<IRecentlyTraded>
{
    TimeBoundaryPeriod         DuringPeriod { get; }

    new IRecentlyTraded Clone();
}

public interface IMutableRecentlyTraded : IRecentlyTraded, IMutableLastTradedList, ITrackableReset<IMutableRecentlyTraded>
{
    new TimeBoundaryPeriod     DuringPeriod { get; set; }
    new IMutableRecentlyTraded Clone();

    new IMutableRecentlyTraded ResetWithTracking();
}
