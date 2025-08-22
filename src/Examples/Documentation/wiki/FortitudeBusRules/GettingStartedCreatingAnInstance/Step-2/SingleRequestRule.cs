// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_2;

public class SingleRequestResponseRule : Rule
{
    private static IVersatileFLogger logger = FLog.FLoggerForType.As<IVersatileFLogger>();
    
    public override async ValueTask StartAsync()
    {
        logger.Inf("Started running SingleRequestResponseRule");
        var listenerReceiveLatency = await this.RequestAsync<DateTime, TimeSpan>
            (ReceivedRequestLatencyTimeRule.TimeSentRequestListenAddress, TimeContext.UtcNow);
        logger.Inf("Received the response from the listener.rule.  " +
                   $"It took {listenerReceiveLatency.TotalMicroseconds} us for the listener to receive the request from the time it was sent. ");
    }

    public override ValueTask StopAsync()
    {
        logger.Inf("Closing SingleRequestResponseRule");
        return base.StopAsync();
    }
}
