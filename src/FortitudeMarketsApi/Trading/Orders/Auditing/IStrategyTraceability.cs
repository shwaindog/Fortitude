using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Client;

namespace FortitudeMarketsApi.Trading.Orders.Auditing
{
    public interface IStrategyTraceability
    {
        long SignalStrategyId { get; }
        long ExecutionStrategyId { get; }
        long PositionMonitoringId { get; }
        IMutableString StrategySummaryComment { get; }
        PositionChangeType PositionChangeType { get; }
    }
}
