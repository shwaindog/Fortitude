// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_4_Final;

public class SingleRequestResponseTimeSendLoadTestRule : Rule
{
    private static IVersatileFLogger logger = FLog.FLoggerForType.As<IVersatileFLogger>();

    private readonly Stopwatch stopWatch = new();
    private readonly IRule     targetRequestRule;

    private readonly TimeSpan[] timeSpanResultList = new TimeSpan[Step4Program.SingleNumMessagesToSend];

    public SingleRequestResponseTimeSendLoadTestRule(IRule targetRequestRule) => this.targetRequestRule = targetRequestRule;

    public override async ValueTask StartAsync()
    {
        // prime runtime
        logger.Inf("Started priming SingleRequestResponseTimeSendLoadTestRule for performance testing");
        for (var i = 0; i < Step4Program.SingleNumMessagesToSend; i++)
        {
            var listenerReceiveLatency = await this.RequestAsync<DateTime, TimeSpan>
                (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                 new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule));
            timeSpanResultList[i] = listenerReceiveLatency;
        }

        for (var i = 0; i < Step4Program.SingleNumMessagesToSend; i++) timeSpanResultList[i] = TimeSpan.Zero;
        var totalLatency                                                                     = TimeSpan.Zero;
        logger.Inf("Priming Finished" +
                   (!Step4Program.LogStartOfEachRun ? " starting performance test.  ETA 40-120 s" : ""));
        for (var runNum = 0; runNum < Step4Program.NumberOfRuns; runNum++)
        {
            if (Step4Program.LogStartOfEachRun)
                logger.Inf("Started SingleRequestResponseTimeSendLoadTestRule performance testing " +
                           $"run number {runNum + 1} of {Step4Program.NumberOfRuns} runs");
            stopWatch.Start();
            for (var i = 0; i < Step4Program.SingleNumMessagesToSend; i++)
            {
                var listenerReceiveLatency = await this.RequestAsync<DateTime, TimeSpan>
                    (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                     new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule));
                timeSpanResultList[i] = listenerReceiveLatency;
            }
            stopWatch.Stop();
            for (var i = 0; i < Step4Program.SingleNumMessagesToSend; i++)
            {
                var listenerReceiveLatency = timeSpanResultList[i];
                totalLatency += listenerReceiveLatency;
            }
        }

        var totalMessagesSent = Step4Program.SingleNumMessagesToSend * Step4Program.NumberOfRuns;
        var averageLatency    = totalLatency / totalMessagesSent;

        var messagesPerSecond = 1_000L * totalMessagesSent / stopWatch.ElapsedMilliseconds;
        logger.Inf("When sending {totalMessagesSent:###,###,##0} messages total execution time took " +
                   $"{stopWatch.ElapsedMilliseconds:###,###,##0} ms or about {messagesPerSecond:###,##0} msgs/s.");
        logger.Inf("The average time for a message to be received by a listening rule " +
                   $"was {averageLatency.TotalMicroseconds} us");
    }

    public override ValueTask StopAsync()
    {
        logger.Inf("Closing SingleRequestResponseTimeSendLoadTestRule");
        return base.StopAsync();
    }
}
