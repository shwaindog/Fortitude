// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Rules;

public class SendTimeLoadTestRule : Rule
{
    private readonly IRule targetRequestRule;

    public SendTimeLoadTestRule(IRule targetRequestRule) => this.targetRequestRule = targetRequestRule;

    public override async ValueTask StartAsync()
    {
        // prime runtime
        var timeSpanValueTaskResultList = new ReusableList<ValueTask<TimeSpan>>(Context.PooledRecycler, 10_000);
        for (var i = 0; i < 1_000; i++)
            timeSpanValueTaskResultList.Add(this.RequestAsync<DateTime, TimeSpan>
                                                (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow,
                                                 new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule)));

        foreach (var valueTask in timeSpanValueTaskResultList) await valueTask;
        timeSpanValueTaskResultList.Clear();
        Console.Out.WriteLine($"Started SendTimeLoadTestRule");
        var averageLatency = TimeSpan.Zero;
        for (var i = 0; i < 1_000_000; i++)
        {
            timeSpanValueTaskResultList.Add
                (this.RequestAsync<DateTime, TimeSpan>
                    (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow
                   , new DispatchOptions(RoutingFlags.TargetSpecific, targetRule: targetRequestRule)));
            if (i > 0 && i % 10_000 == 0)
            {
                var iStart = i - 10_000;
                Console.Out.WriteLine($".");
                for (var j = 0; j < 10_000; j++)
                {
                    var getResult = await timeSpanValueTaskResultList[j];
                    // if (j == 9_999) Console.Out.WriteLine($"Ticks: {getResult.Ticks}");
                    averageLatency = (averageLatency * (iStart + j) + getResult) / (iStart + j + 1);
                }
                timeSpanValueTaskResultList.Clear();
            }
        }
        await
            Console.Out.WriteLineAsync($"The average time for a message to be received by a listening rule is {averageLatency.TotalMicroseconds} us");
    }
}
