using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.TrackingTimes;

public interface IAdditionalTrackingFields : IReusableObject<IAdditionalTrackingFields>, IInterfacesComparable<IAdditionalTrackingFields>
{
}
public interface IMutableAdditionalTrackingFields : IAdditionalTrackingFields, ITrackableReset<IMutableAdditionalTrackingFields>
{
}