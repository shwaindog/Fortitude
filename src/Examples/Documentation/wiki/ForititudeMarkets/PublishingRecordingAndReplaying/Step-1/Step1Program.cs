// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Config;
using FortitudeBusRules.Rules;

#endregion

namespace PublishingRecordingAndReplaying.Step_1;

public class Step4Program
{
    private const int EventQueueSize   = 20_100;
    private const int DefaultQueueSize = 20_100;

    // If memory profiling ensure no console logging creates garbage
    public static bool LogStartOfEachRun = false;

    public static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "MainThread";
        
        var busRulesConfig
            = new BusRulesConfig
                ("Step1PublishingPrices", new QueuesConfig
                    (EventQueueSize, DefaultQueueSize, maxEventQueues: 1, emptyEventQueueSleepMs: 0
                   , requiredCustomQueues: 1, defaultEmptyQueueSleepMs: 0, maxWorkerQueues: 1));
        var busRules   = new BusRules();
        var messageBus = busRules.GetOrCreateMessageBus(busRulesConfig);

        var commandLineOptions = CommandLineOptions.ParseCommandLine(args);
        if (commandLineOptions.ShouldExit) return;
        messageBus.Start();
        messageBus.DeployDaemonRule(new StartingBootstrapRule(commandLineOptions)
                                  , new DeploymentOptions(messageGroupType: MessageQueueType.Custom));

        Console.ReadKey();
        messageBus.Stop();
    }
}
