#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Orders.Client;

#endregion

namespace FortitudeMarkets.Trading.Orders.Auditing;

public interface IStrategyTraceability
{
    long SignalStrategyId { get; }
    long ExecutionStrategyId { get; }
    long PositionMonitoringId { get; }
    IMutableString StrategySummaryComment { get; }
    PositionChangeType PositionChangeType { get; }
}
