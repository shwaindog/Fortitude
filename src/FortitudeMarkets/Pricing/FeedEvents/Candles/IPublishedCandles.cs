using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Candles;

public interface IPublishedCandles: IReusableObject<IPublishedCandles>, IInterfacesComparable<IPublishedCandles>
{
}

public interface IMutablePublishedCandles : IPublishedCandles
{
}