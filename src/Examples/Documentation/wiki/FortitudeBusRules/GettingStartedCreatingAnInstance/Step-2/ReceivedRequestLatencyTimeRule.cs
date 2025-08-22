// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_2;

public class ReceivedRequestLatencyTimeRule : Rule
{
    private static IVersatileFLogger logger = FLog.FLoggerForType.As<IVersatileFLogger>();

    public const string TimeSentRequestListenAddress = "Time.Sent.Request.Listen.Address";

    public override async ValueTask StartAsync()
    {
        await this.RegisterRequestListenerAsync<DateTime, TimeSpan>(TimeSentRequestListenAddress, ReceiveSentTimeAndReturnLatencyHandler);
    }

    private TimeSpan ReceiveSentTimeAndReturnLatencyHandler(DateTime sentTime) => TimeContext.UtcNow - sentTime;

    public override ValueTask StopAsync()
    {
        logger.Inf("Closing ReceivedRequestLatencyTimeRule");
        return base.StopAsync();
    }
}
