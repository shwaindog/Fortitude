// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Rules;

public class ReceivedRequestLatencyTimeRule : Rule
{
    public const string TimeSentRequestListenAddress = "Time.Sent.Request.Listen.Address";

    public override async ValueTask StartAsync()
    {
        await this.RegisterRequestListenerAsync<DateTime, TimeSpan>(TimeSentRequestListenAddress, ReceiveSentTimeAndReturnLatencyHandler);
    }

    private TimeSpan ReceiveSentTimeAndReturnLatencyHandler(DateTime sentTime) => TimeContext.UtcNow - sentTime;
}
