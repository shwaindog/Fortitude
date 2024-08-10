// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Rules;
using FortitudeBusRules;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Config;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted;

internal class Program
{
    private const int EventQueueSize   = 1_001_000;
    private const int DefaultQueueSize = 1_001_000;

    public static void Main(string[] args)
    {
        var busRulesConfig
            = new BusRulesConfig(new QueuesConfig(EventQueueSize, DefaultQueueSize, maxEventQueues: 1, maxWorkerQueues: 1, requiredCustomQueues: 1));
        var busRules = new BusRules();
        busRules.Start(busRulesConfig, new StartingBootstrapRule(), MessageQueueType.Custom);

        Console.ReadKey();
    }
}
