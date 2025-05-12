using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;

public interface IAdapterExecutionStatistics: IReusableObject<IAdapterExecutionStatistics>, IInterfacesComparable<IAdapterExecutionStatistics>
{
}
public interface IMutableAdapterExecutionStatistics : IAdapterExecutionStatistics
{
}