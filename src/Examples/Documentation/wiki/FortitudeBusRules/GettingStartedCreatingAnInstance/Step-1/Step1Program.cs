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

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_1;

public class Step1Program
{
    private const int EventQueueSize   = 20_100;
    private const int DefaultQueueSize = 20_100;

    public static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "MainThread";
        FLogConfigExamples.AsyncDblBufferedFileAndColoredConsoleExample.LoadExampleAsCurrentContext();

        var logger = FLog.FLoggerForType.As<IVersatileFLogger>();
        
        var busRulesConfig
            = new BusRulesConfig("Step1HelloGoodbye", new QueuesConfig(EventQueueSize, DefaultQueueSize, maxEventQueues: 1, maxWorkerQueues: 1));
        var busRules   = new BusRules();
        var messageBus = busRules.GetOrCreateMessageBus(busRulesConfig);
        logger.Inf("Finished creating message bus");

        messageBus.Start();
        logger.Inf("Finished starting message bus");
        messageBus.DeployDaemonRule(new HelloHelloRule(), new DeploymentOptions(messageGroupType: MessageQueueType.Event));

        Console.ReadKey();
        messageBus.Stop();
    }
}
