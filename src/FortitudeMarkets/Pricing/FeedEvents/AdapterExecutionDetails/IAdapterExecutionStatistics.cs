using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;

public interface IAdapterExecutionStatistics: IReusableObject<IAdapterExecutionStatistics>, IInterfacesComparable<IAdapterExecutionStatistics>
{
}
public interface IMutableAdapterExecutionStatistics : IAdapterExecutionStatistics, ITrackableReset<IMutableAdapterExecutionStatistics>, IEmptyable 
{
}