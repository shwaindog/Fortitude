// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_3;

public class ReceivedRequestLatencyTimeRule : Rule
{
    public const string TimeSentRequestListenAddress = "Time.Sent.Request.Listen.Address";

    public override async ValueTask StartAsync()
    {
        await this.RegisterRequestListenerAsync<DateTime, TimeSpan>(TimeSentRequestListenAddress, ReceiveSentTimeAndReturnLatencyHandler);
    }

    private TimeSpan ReceiveSentTimeAndReturnLatencyHandler(DateTime sentTime) => TimeContext.UtcNow - sentTime;

    public override ValueTask StopAsync()
    {
        Console.Out.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Closing ReceivedRequestLatencyTimeRule");
        return base.StopAsync();
    }
}
