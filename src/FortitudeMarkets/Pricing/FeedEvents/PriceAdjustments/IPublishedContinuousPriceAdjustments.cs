﻿using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.PriceAdjustments;

public interface IPublishedContinuousPriceAdjustments : IReusableObject<IPublishedContinuousPriceAdjustments>, IInterfacesComparable<IPublishedContinuousPriceAdjustments>
{
}
public interface IMutablePublishedContinuousPriceAdjustments : IPublishedContinuousPriceAdjustments, ITrackableReset<IMutablePublishedContinuousPriceAdjustments>
{
}