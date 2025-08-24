// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Config;
using FortitudeBusRules.Rules;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LoggerViews;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_3;

public class Step3Program
{
    private const int EventQueueSize   = 20_100;
    private const int DefaultQueueSize = 20_100;

    public const int NumberOfRuns            = 100;
    public const int SingleNumMessagesToSend = 100_000;

    // If memory profiling ensure no console logging creates garbage
    public static bool LogStartOfEachRun = true;

    public static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "MainThread";
        FLogConfigExamples.SyncColoredConsoleExample.LoadExampleAsCurrentContext();

        var logger = FLog.FLoggerForType.As<IVersatileFLogger>();
        
        var appender = logger.InfApnd("Started Step3 application in ");
        #if DEBUG
        appender?.Args("Debug build.");
        #else
        appender?.Args("Release build.");
        #endif
        var busRulesConfig
            = new BusRulesConfig
                ("Step3SingleRequestResponse", new QueuesConfig
                    (EventQueueSize, DefaultQueueSize, maxEventQueues: 1, maxWorkerQueues: 1
                   , requiredCustomQueues: 1, emptyEventQueueSleepMs: 0, defaultEmptyQueueSleepMs: 0));
        var busRules   = new BusRules();
        var messageBus = busRules.GetOrCreateMessageBus(busRulesConfig);
        logger.Inf("Finished creating message bus");

        messageBus.Start();
        logger.Inf("Finished starting message bus");
        messageBus.DeployDaemonRule(new StartingBootstrapRule()
                                  , new DeploymentOptions(messageGroupType: MessageQueueType.Custom));

        Console.ReadKey();
        messageBus.Stop();
    }
}
