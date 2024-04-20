#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

public readonly struct RouteSelectionResult
{
    public RouteSelectionResult(IMessageQueue messageQueue, string strategyName
        , RoutingFlags routingFlags
        , IRule? rule = null)
    {
        MessageQueue = messageQueue;
        StrategyName = strategyName;
        RoutingFlags = routingFlags;
        Rule = rule;
    }

    public readonly IMessageQueue MessageQueue;
    public readonly IRule? Rule;
    public readonly string StrategyName;
    public readonly RoutingFlags RoutingFlags;
}

public interface ISelectionStrategy
{
    string Name { get; }

    RouteSelectionResult? Select(IMessageQueueGroupContainer availableMessageQueues, IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions);

    IDispatchSelectionResultSet? Select(IMessageQueueGroupContainer availableMessageQueues, IRule senderRule
        , DispatchOptions dispatchOptions
        , string destinationAddress);
}

public interface IPreviouslyCachedSelections
{
    IDispatchSelectionResultSet? LastDestinationSelectionResultSet(string destinationAddress
        , DispatchOptions requestOptions);

    IDispatchSelectionResultSet? LastSenderDestinationSelectionResultSet(IRule sender, string destinationAddress
        , DispatchOptions requestOptions);

    RouteSelectionResult? LastDeploySelectionResult(MessageQueueType messageQueueType, DeploymentOptions requestOptions);
}

public interface ISaveSelectionResult
{
    void Save(IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions, RouteSelectionResult routeSelectionResult);

    void Save(IRule senderRule
        , DispatchOptions dispatchOptions, string destinationAddress
        , IDispatchSelectionResultSet selectionResult);
}
