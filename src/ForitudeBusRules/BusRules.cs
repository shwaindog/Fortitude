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

    public IMessageBus Start(BusRulesConfig busRulesConfig, IRule bootstrapRule, MessageQueueType launchOnQueueType = MessageQueueType.Worker)
    {
        MessageBus = new MessageBus(busRulesConfig);
        MessageBus.Start();
        MessageBus.DeployDaemonRule(bootstrapRule, new DeploymentOptions(messageGroupType: launchOnQueueType));
        return MessageBus;
    }
}
