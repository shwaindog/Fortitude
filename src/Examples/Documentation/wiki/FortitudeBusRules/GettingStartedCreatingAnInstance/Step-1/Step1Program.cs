// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Config;
using FortitudeBusRules.Rules;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_1;

public class Step1Program
{
    private const int EventQueueSize   = 20_100;
    private const int DefaultQueueSize = 20_100;

    public static void Main(string[] args)
    {
        var busRulesConfig
            = new BusRulesConfig(new QueuesConfig(EventQueueSize, DefaultQueueSize, maxEventQueues: 1, maxWorkerQueues: 1));
        var busRules   = new BusRules();
        var messageBus = busRules.CreateMessageBus(busRulesConfig);
        Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Finished creating message bus");

        messageBus.Start();
        Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Finished starting message bus");
        messageBus.DeployDaemonRule(new HelloHelloRule()
                                  , new DeploymentOptions(messageGroupType: MessageQueueType.Event));

        Console.ReadKey();
        messageBus.Stop();
    }
}
