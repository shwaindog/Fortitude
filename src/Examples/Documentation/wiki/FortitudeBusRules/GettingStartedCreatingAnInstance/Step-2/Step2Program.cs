// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Config;
using FortitudeBusRules.Rules;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_2;

public class Step2Program
{
    public static int EventQueueSize   = 20_100;
    public static int DefaultQueueSize = 20_100;

    public static void Main(string[] args)
    {
        Console.Write($"{DateTime.Now:hh:mm:ss.ffffff} - Started Step2 application in ");
        #if DEBUG
        Console.WriteLine("Debug build.");
        #else
        Console.WriteLine("Release build.");
        #endif
        var busRulesConfig
            = new BusRulesConfig(new QueuesConfig(EventQueueSize, DefaultQueueSize, maxEventQueues: 1, maxWorkerQueues: 1, requiredCustomQueues: 1));
        var busRules   = new BusRules();
        var messageBus = busRules.CreateMessageBus(busRulesConfig);
        Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Finished creating message bus");

        messageBus.Start();
        Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Finished starting message bus");
        messageBus.DeployDaemonRule(new StartingBootstrapRule()
                                  , new DeploymentOptions(messageGroupType: MessageQueueType.Custom));

        Console.ReadKey();
        messageBus.Stop();
    }
}
