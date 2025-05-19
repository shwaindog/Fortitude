using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Limits;

public interface ILimitBreaches : IReusableObject<ILimitBreaches>, IInterfacesComparable<ILimitBreaches>
{
}
public interface IMutableLimitBreaches : ILimitBreaches, ITrackableReset<IMutableLimitBreaches>, IEmptyable
{
}