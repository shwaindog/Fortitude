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

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_2;

public class Step2Program
{
    private const int EventQueueSize   = 20_100;
    private const int DefaultQueueSize = 20_100;

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
        
        var appender = logger.InfApnd("Started Step2 application in ");
        #if DEBUG
        appender?.Args("Debug build.");
        #else
        appender?.Args("Release build.");
        #endif
        var busRulesConfig
            = new BusRulesConfig
                ("Step2ARequestAResponse"
               , new QueuesConfig(EventQueueSize, DefaultQueueSize, maxEventQueues: 1, maxWorkerQueues: 1, requiredCustomQueues: 1));
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
