// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Config;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules;

public class BusRules
{
    public IConfigureMessageBus? MessageBus;

    public IConfigureMessageBus CreateMessageBus(BusRulesConfig busRulesConfig)
    {
        MessageBus = new MessageBus(busRulesConfig);
        return MessageBus;
    }

    public IMessageBus CreateAndStartMessageBus
        (BusRulesConfig busRulesConfig, IRule bootstrapRule, MessageQueueType launchOnQueueType = MessageQueueType.Worker)
    {
        var messageBus = CreateMessageBus(busRulesConfig);
        messageBus.Start();
        messageBus.DeployDaemonRule(bootstrapRule, new DeploymentOptions(messageGroupType: launchOnQueueType));
        return messageBus;
    }
}
