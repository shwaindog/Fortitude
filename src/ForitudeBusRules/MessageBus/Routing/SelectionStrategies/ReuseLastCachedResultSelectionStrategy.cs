#region

using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public class ReuseLastCachedResultSelectionStrategy : ISelectionStrategy
{
    public const string StrategyName = "ReuseLastCachedResultSelectionStrategy";

    private readonly IPreviouslyCachedSelections previouslyCachedSelections;

    public ReuseLastCachedResultSelectionStrategy(IPreviouslyCachedSelections previouslyCachedSelections) =>
        this.previouslyCachedSelections = previouslyCachedSelections;

    public string Name => StrategyName;

    public RouteSelectionResult? Select(IEventQueueGroupContainer availableEventQueues, IRule senderRule
        , IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        var flags = deploymentOptions.RoutingFlags;
        if (flags.IsRecalculateCache()) return null;
        var previousSelect
            = previouslyCachedSelections.LastDeploySelectionResult(deploymentOptions.EventGroupType, deploymentOptions);
        if (previousSelect == null) return null;
        var cacheResult = previousSelect.Value;
        if (cacheResult.RoutingFlags != deploymentOptions.RoutingFlags) return null;
        return new RouteSelectionResult(cacheResult.EventQueue, StrategyName, deploymentOptions.RoutingFlags
            , deployRule);
    }

    public IDispatchSelectionResultSet? Select(IEventQueueGroupContainer availableEventQueues, IRule senderRule
        , DispatchOptions dispatchOptions, string destinationAddress)
    {
        var flags = dispatchOptions.RoutingFlags;
        if (flags.IsRecalculateCache()) return null;
        if (flags.IsSenderCacheLast())
        {
            var cachedResultSet
                = previouslyCachedSelections.LastSenderDestinationSelectionResultSet(senderRule, destinationAddress
                    , dispatchOptions);
            if (cachedResultSet == null ||
                cachedResultSet.DispatchOptions != dispatchOptions) return null;
            var returnResult = cachedResultSet.Clone();
            returnResult.StrategyName = Name;
            return returnResult;
        }

        if (!flags.IsDestinationCacheLast()) return null;
        var cachedResults
            = previouslyCachedSelections.LastDestinationSelectionResultSet(destinationAddress, dispatchOptions);
        if (cachedResults == null ||
            cachedResults.DispatchOptions != dispatchOptions) return null;
        cachedResults.StrategyName = Name;
        return cachedResults.Clone();
    }
}
