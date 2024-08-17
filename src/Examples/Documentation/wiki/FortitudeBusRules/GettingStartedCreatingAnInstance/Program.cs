// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Rules;
using FortitudeBusRules;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Config;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted;

public enum TestToPerform
{
    BatchRequestResponse
  , SingleRequestResponse
}

public class Program
{
    public static int EventQueueSize   = 20_100;
    public static int DefaultQueueSize = 20_100;

    public static int NumberOfRuns            = 100;
    public static int BatchNumMessagesToSend  = 100_000;
    public static int SingleNumMessagesToSend = 100_000;
    public static int BatchSendSize           = 10_000;

    // If memory profiling ensure no console logging creates garbage
    public static bool LogStartOfEachRun = false;


    private static TestToPerform testToPerform = TestToPerform.BatchRequestResponse;

    public static void Main(string[] args)
    {
        Console.Write($"{DateTime.Now:hh:mm:ss.ffffff} - Started application in ");
        #if DEBUG
        Console.WriteLine("Debug build.");
        #else
        Console.WriteLine("Release build.");
        #endif

        NoOpLoggerFactory.StartWithNoOpLoggerFactory = true;

        var busRulesConfig
            = new BusRulesConfig(new QueuesConfig(EventQueueSize, DefaultQueueSize, maxEventQueues: 1, emptyEventQueueSleepMs: 0
                                                , defaultEmptyQueueSleepMs: 0, maxWorkerQueues: 1, requiredCustomQueues: 1));
        var busRules   = new BusRules();
        var messageBus = busRules.CreateMessageBus(busRulesConfig);
        Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Finished creating message bus");

        messageBus.Start();
        Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffffff} - Finished starting message bus");
        messageBus.DeployDaemonRule(new StartingBootstrapRule(testToPerform)
                                  , new DeploymentOptions(messageGroupType: MessageQueueType.Custom));

        Console.ReadKey();
    }
}
