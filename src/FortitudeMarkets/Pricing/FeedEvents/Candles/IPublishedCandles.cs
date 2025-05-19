using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Candles;

public interface IPublishedCandles: IReusableObject<IPublishedCandles>, IInterfacesComparable<IPublishedCandles>
{
}

public interface IMutablePublishedCandles : IPublishedCandles, ITrackableReset<IMutablePublishedCandles> 
{
}