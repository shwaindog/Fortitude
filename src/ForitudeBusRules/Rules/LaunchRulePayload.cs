// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeBusRules.Rules;

public struct LaunchRulePayload
{
    public LaunchRulePayload(IRule rule, DeploymentOptions? deploymentOptions = null)
    {
        DeploymentOptions = deploymentOptions ?? new DeploymentOptions();
        Rule              = (IListeningRule)rule;
    }

    public IListeningRule Rule { get; set; }

    public DeploymentOptions DeploymentOptions { get; set; }
}
