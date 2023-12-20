#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Client;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Auditing;

public interface IStrategyTraceability
{
    long SignalStrategyId { get; }
    long ExecutionStrategyId { get; }
    long PositionMonitoringId { get; }
    IMutableString StrategySummaryComment { get; }
    PositionChangeType PositionChangeType { get; }
}
