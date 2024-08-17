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

public class BatchTimeSendLoadTestRule : Rule
{
    private readonly Stopwatch stopWatch = new();
    private readonly IRule     targetRequestRule;

    private readonly ValueTask<TimeSpan>[] timeSpanValueTaskResultList = new ValueTask<TimeSpan>[Program.BatchSendSize];

    public BatchTimeSendLoadTestRule(IRule targetRequestRule) => this.targetRequestRule = targetRequestRule;

    public override async ValueTask StartAsync()
    {
        // prime runtime
        await Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - Starting priming BatchTimeSendLoadTestRule for performance testing");
        for (var i = 0; i < Program.BatchSendSize; i++)
        {
            var listenerReceiveLatency = this.RequestAsync<DateTime, TimeSpan>
                (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                 new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule));
            timeSpanValueTaskResultList[i] = listenerReceiveLatency;
        }

        for (var i = 0; i < Program.BatchSendSize; i++)
        {
            var listenerReceiveLatency = await timeSpanValueTaskResultList[i];
            timeSpanValueTaskResultList[i] = new ValueTask<TimeSpan>(TimeSpan.Zero);
        }
        var averageLatency = TimeSpan.Zero;
        await Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - Priming Finished" +
                                         (!Program.LogStartOfEachRun ? " starting performance test.  ETA 40-120 s" : ""));
        stopWatch.Start();
        var messageCountBase = 0;
        for (var runNum = 0; runNum < Program.NumberOfRuns; runNum++)
        {
            messageCountBase = runNum * Program.BatchNumMessagesToSend;
            // comment out console write if doing memory profiling

            if (Program.LogStartOfEachRun)
                await Console.Out.WriteLineAsync
                    ($"{DateTime.Now:hh:mm:ss.ffffff} - Started BatchTimeSendLoadTestRule performance testing " +
                     $"for run number {runNum + 1} of {Program.NumberOfRuns} runs");
            for (var i = 0; i <= Program.BatchNumMessagesToSend; i++)
            {
                if (i > 0 && i % Program.BatchSendSize == 0)
                {
                    stopWatch.Stop();
                    var iStart = i - Program.BatchSendSize;
                    // await Console.Out.WriteAsync($".");
                    for (var j = 0; j < Program.BatchSendSize; j++)
                    {
                        var getResult = await timeSpanValueTaskResultList[j];
                        // if (j == 9_999) Console.Out.WriteLine($"Ticks: {getResult.Ticks}");
                        averageLatency = (averageLatency * (messageCountBase + iStart + j) + getResult) / (messageCountBase + iStart + j + 1);
                        timeSpanValueTaskResultList[j] = new ValueTask<TimeSpan>(TimeSpan.Zero);
                    }
                    stopWatch.Start();
                    if (i == Program.BatchNumMessagesToSend) break;
                }
                var listenerReceiveLatency = this.RequestAsync<DateTime, TimeSpan>
                    (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                     new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule));
                timeSpanValueTaskResultList[i % Program.BatchSendSize] = listenerReceiveLatency;
            }
        }
        stopWatch.Stop();
        var totalMessagesSent = Program.BatchNumMessagesToSend * Program.NumberOfRuns;
        var messagesPerSecond = 1_000L * totalMessagesSent / stopWatch.ElapsedMilliseconds;
        await
            Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - When sending {totalMessagesSent:###,###,##0} messages total execution time took " +
                                       $"{stopWatch.ElapsedMilliseconds:###,###,##0} ms or about {messagesPerSecond:###,##0} msgs/s.");
        await Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - The average time for a message to be received by a listening rule " +
                                         $"was {averageLatency.TotalMicroseconds} us");
    }

    public override ValueTask StopAsync()
    {
        Console.Out.WriteLine("Closing BatchTimeSendLoadTestRule");
        return base.StopAsync();
    }
}
