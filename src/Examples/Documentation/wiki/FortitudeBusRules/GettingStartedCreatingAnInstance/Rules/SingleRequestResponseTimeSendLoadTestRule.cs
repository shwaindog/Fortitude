// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Rules;

public class SingleRequestResponseTimeSendLoadTestRule : Rule
{
    private readonly IRule targetRequestRule;

    private readonly TimeSpan[] timeSpanResultList = new TimeSpan[Program.SingleNumMessagesToSend];

    public SingleRequestResponseTimeSendLoadTestRule(IRule targetRequestRule) => this.targetRequestRule = targetRequestRule;

    public override async ValueTask StartAsync()
    {
        IncrementLifeTimeCount();
        // prime runtime
        await Console.Out
                     .WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - Started priming SingleRequestResponseTimeSendLoadTestRule for performance testing");
        for (var i = 0; i < Program.SingleNumMessagesToSend; i++)
        {
            var listenerReceiveLatency = await this.RequestAsync<DateTime, TimeSpan>
                (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                 new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule));
            timeSpanResultList[i] = listenerReceiveLatency;
        }

        for (var i = 0; i < Program.SingleNumMessagesToSend; i++) timeSpanResultList[i] = TimeSpan.Zero;
        var stopWatch                                                                   = new Stopwatch();
        var totalLatency                                                                = TimeSpan.Zero;
        await Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - Priming Finished");
        for (var runNum = 0; runNum < Program.NumberOfRuns; runNum++)
        {
            // comment out console write if doing memory profiling
            // await
            //     Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - Started SingleRequestResponseTimeSendLoadTestRule performance testing " +
            //                                $"run number {runNum + 1} of {Program.NumberOfRuns} runs");
            stopWatch.Start();
            for (var i = 0; i < Program.SingleNumMessagesToSend; i++)
            {
                var listenerReceiveLatency = await this.RequestAsync<DateTime, TimeSpan>
                    (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                     new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule));
                timeSpanResultList[i] = listenerReceiveLatency;
            }
            stopWatch.Stop();
            for (var i = 0; i < Program.SingleNumMessagesToSend; i++)
            {
                var listenerReceiveLatency = timeSpanResultList[i];
                totalLatency += listenerReceiveLatency;
            }
        }

        var totalMessagesSent = Program.SingleNumMessagesToSend * Program.NumberOfRuns;
        var averageLatency    = totalLatency / totalMessagesSent;

        var messagesPerSecond = 1_000 * totalMessagesSent / stopWatch.ElapsedMilliseconds;
        await
            Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - When sending {totalMessagesSent:###,###,##0} messages total execution time took " +
                                       $"{stopWatch.ElapsedMilliseconds:###,###,##0} ms or about {messagesPerSecond:###,##0} msgs/s.");
        await Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - The average time for a message to be received by a listening rule " +
                                         $"was {averageLatency.TotalMicroseconds} us");
        DecrementLifeTimeCount();
    }

    public override ValueTask StopAsync()
    {
        Console.Out.WriteLine("Closing BatchTimeSendLoadTestRule");
        return base.StopAsync();
    }
}
