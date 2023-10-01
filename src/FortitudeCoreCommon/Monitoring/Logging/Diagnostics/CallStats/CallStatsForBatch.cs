using System;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats
{
    public class CallStatsForBatch
    {
        public readonly CallStatsForPercentile AllCalls;
        public readonly DateTime EarliestCallCompletion;
        public readonly DateTime LatestCallCompletion;
        public readonly CallStatsForPercentile Quickest50ThPercentile;
        public readonly CallStatsForPercentile Quickest90ThPercentile;
        public readonly CallStatsForPercentile Quickest99ThPercentile;
        public readonly TimeSpan QuickestCall;
        public readonly CallStatsForPercentile Slowest5ThPercentile;

        public CallStatsForBatch(DateTime earliestCallCompletion, DateTime latestCallCompletion,
            TimeSpan quickestCall, CallStatsForPercentile quickest50ThPercentile, CallStatsForPercentile
                quickest90ThPercentile, CallStatsForPercentile quickest99ThPercentile,
            CallStatsForPercentile slowest5ThPercentile,
            CallStatsForPercentile allCalls)
        {
            EarliestCallCompletion = earliestCallCompletion;
            LatestCallCompletion = latestCallCompletion;
            QuickestCall = quickestCall;
            Quickest50ThPercentile = quickest50ThPercentile;
            Quickest90ThPercentile = quickest90ThPercentile;
            Quickest99ThPercentile = quickest99ThPercentile;
            Slowest5ThPercentile = slowest5ThPercentile;
            AllCalls = allCalls;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "ErlstCmpltn:{0:HH\\:mm\\:ss\\.ffffff}, LtstCmpltn:{1:HH\\:mm\\:ss\\.ffffff}, QkstCall:{2:N3} ms, " +
                    "Qkst50Th%tile:{3}, Qkst90Th%tile:{4}, Qkst99Th%ile:{5}, Slwst5Th%tile:{6}, AllCalls:{7}"
                    , EarliestCallCompletion, LatestCallCompletion, QuickestCall.TotalMilliseconds,
                    Quickest50ThPercentile.ToStringNoBoxing(), Quickest90ThPercentile.ToStringNoBoxing(),
                    Quickest99ThPercentile.ToStringNoBoxing(), Slowest5ThPercentile.ToStringNoBoxing(),
                    AllCalls.ToStringNoBoxing());
        }
    }
}