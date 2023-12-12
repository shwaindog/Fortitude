#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public readonly struct SelectionResult
{
    public SelectionResult(IEventQueue eventQueue, string strategyName
        , RoutingStrategySelectionFlags routingStrategySelectionFlags
        , IRule? rule = null)
    {
        EventQueue = eventQueue;
        StrategyName = strategyName;
        RoutingStrategySelectionFlags = routingStrategySelectionFlags;
        Rule = rule;
    }

    public readonly IEventQueue EventQueue;
    public readonly IRule? Rule;
    public readonly string StrategyName;
    public readonly RoutingStrategySelectionFlags RoutingStrategySelectionFlags;
}

public interface ISelectionStrategy : IRecyclableObject
{
    string Name { get; }

    SelectionResult? Select(IReusableList<IEventQueue> availableEventQueues, IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions);

    IDispatchSelectionResultSet Select(IReusableList<IEventQueue> availableEventQueues, IRule senderRule
        , DispatchOptions dispatchOptions
        , string? destinationAddress = null);
}

public interface ISaveSelectionResult : IRecyclableObject
{
    void Save(IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions, SelectionResult selectionResult);

    void Save(IRule senderRule
        , DispatchOptions deploymentOptions, string? destinationAddress
        , IDispatchSelectionResultSet selectionResult);
}
