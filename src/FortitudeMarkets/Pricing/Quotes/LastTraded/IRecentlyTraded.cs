// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LastTraded;


public interface IRecentlyTraded : ILastTradedList, IInterfacesComparable<IRecentlyTraded>, ICloneable<IRecentlyTraded>
{
    TimeBoundaryPeriod         DuringPeriod { get; }

    new IRecentlyTraded Clone();
}

public interface IMutableRecentlyTraded : IRecentlyTraded, IMutableLastTradedList
{
    new TimeBoundaryPeriod     DuringPeriod { get; set; }
    new IMutableRecentlyTraded Clone();
}
