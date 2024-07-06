// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.Rules;

public interface IRuleDeploymentLifeTime : IDispatchResult, IAsyncValueTaskDisposable
{
    IRule     DeployedRule { get; }
    void      Undeploy();
    ValueTask UndeployAsync();
}

public class RuleDeploymentLifeTime : DispatchResult, IRuleDeploymentLifeTime
{
    private IRule deployedRule;
    private IRule senderRule;

    public RuleDeploymentLifeTime()
    {
        senderRule   = null!;
        deployedRule = null!;
    }

    public RuleDeploymentLifeTime(IRule senderRule, IRule deployedRule)
    {
        this.senderRule   = senderRule;
        this.deployedRule = deployedRule;
    }

    public RuleDeploymentLifeTime(RuleDeploymentLifeTime toClone) : base(toClone)
    {
        senderRule   = toClone.senderRule;
        deployedRule = toClone.deployedRule;
    }

    public IRule SenderRule
    {
        get => senderRule;
        set => senderRule = value ?? throw new ArgumentNullException(nameof(SenderRule));
    }

    public IRule DeployedRule
    {
        get => deployedRule;
        set => deployedRule = value ?? throw new ArgumentNullException(nameof(DeployedRule));
    }

    public ValueTask DisposeAsync() => UndeployAsync();
    public ValueTask Dispose()      => UndeployAsync();

    public async ValueTask UndeployAsync() => await deployedRule.Context.RegisteredOn.StopRuleAsync(senderRule, deployedRule);
    public       void      Undeploy()      => deployedRule.Context.RegisteredOn.StopRule(senderRule, deployedRule);

    public ValueTask DisposeAwaitValueTask { get; set; }


    public override IDispatchResult CopyFrom(IDispatchResult source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is RuleDeploymentLifeTime lifeSupport)
        {
            senderRule   = lifeSupport.senderRule;
            deployedRule = lifeSupport.deployedRule;
        }
        return this;
    }

    public override IDispatchResult Clone() => Recycler?.Borrow<RuleDeploymentLifeTime>().CopyFrom(this) ?? new RuleDeploymentLifeTime(this);
}

public static class RuleDeploymentLifeSupportExtensions
{
    public static async ValueTask NullSafeUndeploy(this IRuleDeploymentLifeTime? subscription) =>
        await (subscription?.UndeployAsync() ?? ValueTask.CompletedTask);
}
