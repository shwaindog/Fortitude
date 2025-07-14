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
    private readonly Dictionary<string, IConfigureMessageBus> messageBusRegistry = new();

    private readonly object syncLock = new ();

    public IConfigureMessageBus? MessageBus;

    public IConfigureMessageBus GetOrCreateMessageBus(BusRulesConfig busRulesConfig)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        if (!messageBusRegistry.TryGetValue(busRulesConfig.Name, out var messageBus))
        {
            lock (syncLock)
            {
                if (!messageBusRegistry.TryGetValue(busRulesConfig.Name, out messageBus))
                {
                    messageBus = new MessageBus(busRulesConfig);
                    messageBusRegistry.Add(messageBus.Name, messageBus);
                }
            }
        }
        return messageBus;
    }

    public IMessageBus GetOrCreateStartedMessageBus
        (BusRulesConfig busRulesConfig, IRule? bootstrapRule = null, MessageQueueType launchOnQueueType = MessageQueueType.Worker)
    {
        var messageBus = GetOrCreateMessageBus(busRulesConfig);
        if (!messageBus.IsRunning)
        {
            messageBus.Start();
            if (bootstrapRule != null)
            {
                messageBus.DeployDaemonRule(bootstrapRule, new DeploymentOptions(messageGroupType: launchOnQueueType));
            }
        }
        return messageBus;
    }
}
