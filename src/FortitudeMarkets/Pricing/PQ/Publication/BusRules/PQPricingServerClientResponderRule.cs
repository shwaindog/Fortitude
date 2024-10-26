// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.Rules;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication.BusRules;

public class PQPricingServerClientResponderRule : Rule
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPricingServerClientResponderRule));

    private readonly string feedName;

    private readonly INetworkTopicConnectionConfig snapshotConnectionConfig;

    private PQPricingServerSnapshotResponder? snapshotConversationResponder;

    public PQPricingServerClientResponderRule(string feedName, IPricingServerConfig pricingServerConfig)
    {
        this.feedName            = feedName;
        snapshotConnectionConfig = pricingServerConfig.SnapshotConnectionConfig!;
    }

    public override async ValueTask StartAsync()
    {
        await AttemptStartResponder();
        IncrementLifeTimeCount();
    }

    public async ValueTask AttemptStartResponder()
    {
        var thisQueue = Context.RegisteredOn;
        if (thisQueue is not INetworkInboundMessageQueue networkInboundMessageQueue)
        {
            Logger.Warn("Expected this rule to be deployed on an NetworkInboundQueue so that it can reduce Queue hops.  " +
                        "Was deployed on {0}", thisQueue.Name);
            return;
        }

        var snapshotRespondingServerDispatcher = Context.MessageBus.BusNetworkResolver.GetNetworkDispatcherResolver(
             QueueSelectionStrategy.LeastBusy, MessageQueueType.AllNetwork, networkInboundMessageQueue);
        snapshotConversationResponder = PQPricingServerSnapshotResponder.BuildTcpResponder(
                                                                                           feedName, snapshotConnectionConfig
                                                                                         , snapshotRespondingServerDispatcher, Context.MessageBus);
        var workerQueueConnect = Context.GetEventQueues(MessageQueueType.Worker)
                                        .SelectEventQueue(QueueSelectionStrategy.EarliestCompleted).GetExecutionContextResult<bool, TimeSpan>(this);
        var connected = await snapshotConversationResponder.StartAsync(10_000, workerQueueConnect);
        if (connected)
        {
            Logger.Info("Successfully connected to pricing snapshot responder adapter and port for {0}", feedName);
            snapshotConversationResponder.NewClient += LaunchNewSnapshotClientConversationRule;
            return;
        }

        Logger.Error("Warning did not connect to the configured adapter and port typically this means there is a misconfiguration " +
                     "between your configuration and environment or another process or rule has claimed the port.");
    }

    private void LaunchNewSnapshotClientConversationRule(IConversationRequester newClientSession)
    {
        var newClientSessionRule = new PQPricingServerClientSessionRule(feedName, newClientSession);
        this.DeployChildRuleAsync(newClientSessionRule, new DeploymentOptions());
    }
}
