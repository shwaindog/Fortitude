// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_1;

public class HelloHelloRule : Rule
{
    private static IVersatileFLogger logger = FLog.FLoggerForType.As<IVersatileFLogger>();
    
    public override void Start()
    {
        logger.Inf("I say Hello");
    }

    public override void Stop()
    {
        logger.Inf("And you Goodbye");
    }
}
