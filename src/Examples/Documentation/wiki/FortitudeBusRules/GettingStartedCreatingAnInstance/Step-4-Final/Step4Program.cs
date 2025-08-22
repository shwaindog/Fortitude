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
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_4_Final;

public enum TestToPerform
{
    BatchRequestResponse
  , SingleRequestResponse
}

public class Step4Program
{
    private const int EventQueueSize   = 20_100;
    private const int DefaultQueueSize = 20_100;

    public const int NumberOfRuns            = 100;
    public const int BatchNumMessagesToSend  = 100_000;
    public const int SingleNumMessagesToSend = 100_000;
    public const int BatchSendSize           = 10_000;

    // If memory profiling ensure no console logging creates garbage
    public static bool LogStartOfEachRun = false;


    private static TestToPerform testToPerform = TestToPerform.BatchRequestResponse;

    public static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "MainThread";
        FLogConfigExtractor.SyncFileAndColoredConsoleExample.ExtractExampleTo();
        var context =
            FLogContext
                .NewUninitializedContext
                .InitializeContextFromWorkingDirFilePath(Environment.CurrentDirectory, FLogConfigFile.DefaultConfigFullFilePath)
                .StartFlogSetAsCurrentContext();

        var logger = FLog.FLoggerForType.As<IVersatileFLogger>();
        
        var appender = logger.InfApnd("Started Step4 application in ");
        #if DEBUG
        appender?.Args("Debug build.");
        #else
        appender?.Args("Release build.");
        #endif
    
        NoOpLoggerFactory.StartWithNoOpLoggerFactory = true;
    
        var busRulesConfig
            = new BusRulesConfig
                ("Step4BulkRequestResponses", new QueuesConfig
                    (EventQueueSize, DefaultQueueSize, maxEventQueues: 1, emptyEventQueueSleepMs: 0
                   , requiredCustomQueues: 1, defaultEmptyQueueSleepMs: 0, maxWorkerQueues: 1));
        var busRules   = new BusRules();
        var messageBus = busRules.GetOrCreateMessageBus(busRulesConfig);
        logger.Inf("Finished creating message bus");
    
        messageBus.Start();
        logger.Inf("Finished starting message bus");
        messageBus.DeployDaemonRule(new StartingBootstrapRule(testToPerform)
                                  , new DeploymentOptions(messageGroupType: MessageQueueType.Custom));
    
        Console.ReadKey();
        messageBus.Stop();
    }
}
