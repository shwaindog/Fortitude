using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Indicators.MovingAverages;

public interface IMovingAverage : IReusableObject<IMovingAverage>, IInterfacesComparable<IMovingAverage>
{
}