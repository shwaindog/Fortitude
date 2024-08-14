// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Rules;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Rules;

public class StartingBootstrapRule(TestToPerform testToPerform) : Rule
{
    public override async ValueTask StartAsync()
    {
        var receivedRequestLatencyTimeRule = new ReceivedRequestLatencyTimeRule();
        await this.DeployChildRuleAsync(receivedRequestLatencyTimeRule, new DeploymentOptions(messageGroupType: MessageQueueType.Event));

        switch (testToPerform)
        {
            case TestToPerform.BatchRequestResponse:
                this.DeployDaemonRule(new BatchTimeSendLoadTestRule(receivedRequestLatencyTimeRule)
                                    , new DeploymentOptions(messageGroupType: MessageQueueType.Worker));
                break;
            case TestToPerform.SingleRequestResponse:
                this.DeployDaemonRule(new SingleRequestResponseTimeSendLoadTestRule(receivedRequestLatencyTimeRule)
                                    , new DeploymentOptions(messageGroupType: MessageQueueType.Worker));
                break;
        }
    }
}
