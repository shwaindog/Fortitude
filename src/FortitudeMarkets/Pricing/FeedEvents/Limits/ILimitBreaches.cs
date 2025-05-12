using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.Limits;

public interface ILimitBreaches : IReusableObject<ILimitBreaches>, IInterfacesComparable<ILimitBreaches>
{
}
public interface IMutableLimitBreaches : ILimitBreaches
{
}