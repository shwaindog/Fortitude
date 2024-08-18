// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Rules;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_2;

public class StartingBootstrapRule : Rule
{
    public override async ValueTask StartAsync()
    {
        var receivedRequestLatencyTimeRule = new ReceivedRequestLatencyTimeRule();
        await this.DeployChildRuleAsync(receivedRequestLatencyTimeRule, new DeploymentOptions(messageGroupType: MessageQueueType.Event));

        await this.DeployChildRuleAsync(new SingleRequestResponseRule()
                                      , new DeploymentOptions(messageGroupType: MessageQueueType.Worker));
    }
}
