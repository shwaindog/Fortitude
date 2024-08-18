// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_3;

public class SingleRequestResponseTimeSendLoadTestRule : Rule
{
    private readonly Stopwatch stopWatch = new();
    private readonly IRule     targetRequestRule;

    private readonly TimeSpan[] timeSpanResultList = new TimeSpan[Step3Program.SingleNumMessagesToSend];

    public SingleRequestResponseTimeSendLoadTestRule(IRule targetRequestRule) => this.targetRequestRule = targetRequestRule;

    public override async ValueTask StartAsync()
    {
        // prime runtime
        await Console.Out
                     .WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - Started priming SingleRequestResponseTimeSendLoadTestRule for performance testing");
        for (var i = 0; i < Step3Program.SingleNumMessagesToSend; i++)
        {
            var listenerReceiveLatency = await this.RequestAsync<DateTime, TimeSpan>
                (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                 new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule));
            timeSpanResultList[i] = listenerReceiveLatency;
        }

        for (var i = 0; i < Step3Program.SingleNumMessagesToSend; i++) timeSpanResultList[i] = TimeSpan.Zero;
        var totalLatency                                                                     = TimeSpan.Zero;
        await Console.Out.WriteLineAsync
            ($"{DateTime.Now:hh:mm:ss.ffffff} - Priming Finished" +
             (!Step3Program.LogStartOfEachRun ? " starting performance test.  ETA 40-120 s" : ""));
        for (var runNum = 0; runNum < Step3Program.NumberOfRuns; runNum++)
        {
            if (Step3Program.LogStartOfEachRun)
                await Console.Out.WriteLineAsync
                    ($"{DateTime.Now:hh:mm:ss.ffffff} - Started SingleRequestResponseTimeSendLoadTestRule performance testing " +
                     $"run number {runNum + 1} of {Step3Program.NumberOfRuns} runs");
            stopWatch.Start();
            for (var i = 0; i < Step3Program.SingleNumMessagesToSend; i++)
            {
                var listenerReceiveLatency = await this.RequestAsync<DateTime, TimeSpan>
                    (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                     new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule));
                timeSpanResultList[i] = listenerReceiveLatency;
            }
            stopWatch.Stop();
            for (var i = 0; i < Step3Program.SingleNumMessagesToSend; i++)
            {
                var listenerReceiveLatency = timeSpanResultList[i];
                totalLatency += listenerReceiveLatency;
            }
        }

        var totalMessagesSent = Step3Program.SingleNumMessagesToSend * Step3Program.NumberOfRuns;
        var averageLatency    = totalLatency / totalMessagesSent;

        var messagesPerSecond = 1_000L * totalMessagesSent / stopWatch.ElapsedMilliseconds;
        await
            Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - When sending {totalMessagesSent:###,###,##0} messages total execution time took " +
                                       $"{stopWatch.ElapsedMilliseconds:###,###,##0} ms or about {messagesPerSecond:###,##0} msgs/s.");
        await Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - The average time for a message to be received by a listening rule " +
                                         $"was {averageLatency.TotalMicroseconds} us");
    }

    public override ValueTask StopAsync()
    {
        Console.Out.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Closing SingleRequestResponseTimeSendLoadTestRule");
        return base.StopAsync();
    }
}
