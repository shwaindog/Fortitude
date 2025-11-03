// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public interface IMarketCalendarPanel : IReusableObject<IMarketCalendarPanel>, IInterfacesComparable<IMarketCalendarPanel>
{
}


public interface IMutableMarketCalendarPanel : IMarketCalendarPanel, ITrackableReset<IMutableMarketCalendarPanel>, IEmptyable
{
}