using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.TrackingTimes;

public interface IAdditionalTrackingFields : IReusableObject<IAdditionalTrackingFields>, IInterfacesComparable<IAdditionalTrackingFields>
{
}
public interface IMutableAdditionalTrackingFields : IAdditionalTrackingFields
{
}