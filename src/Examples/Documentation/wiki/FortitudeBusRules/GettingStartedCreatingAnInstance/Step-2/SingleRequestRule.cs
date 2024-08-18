// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_2;

public class SingleRequestResponseRule : Rule
{
    public override async ValueTask StartAsync()
    {
        await Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - Started running SingleRequestResponseRule");
        var listenerReceiveLatency = await this.RequestAsync<DateTime, TimeSpan>
            (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow);
        await Console.Out.WriteLineAsync($"{DateTime.Now:hh:mm:ss.ffffff} - Received the response from the listener.rule.  " +
                                         $"It took {listenerReceiveLatency.TotalMicroseconds} us for the listener to receive the request from the time it was sent. ");
    }

    public override ValueTask StopAsync()
    {
        Console.Out.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Closing SingleRequestResponseRule");
        return base.StopAsync();
    }
}
