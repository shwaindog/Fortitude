using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;

public interface IAdapterExecutionStatistics: IReusableObject<IAdapterExecutionStatistics>, IInterfacesComparable<IAdapterExecutionStatistics>
{
}
public interface IMutableAdapterExecutionStatistics : IAdapterExecutionStatistics, ITrackableReset<IMutableAdapterExecutionStatistics>, IEmptyable 
{
}