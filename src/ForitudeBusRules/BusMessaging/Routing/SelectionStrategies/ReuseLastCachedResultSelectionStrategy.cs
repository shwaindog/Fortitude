#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

public class ReuseLastCachedResultSelectionStrategy : ISelectionStrategy
{
    public const string StrategyName = "ReuseLastCachedResultSelectionStrategy";

    private readonly IPreviouslyCachedSelections previouslyCachedSelections;

    public ReuseLastCachedResultSelectionStrategy(IPreviouslyCachedSelections previouslyCachedSelections) =>
        this.previouslyCachedSelections = previouslyCachedSelections;

    public string Name => StrategyName;

    public RouteSelectionResult? Select(IMessageQueueGroupContainer availableMessageQueues, IRule senderRule
        , IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        var flags = deploymentOptions.RoutingFlags;
        if (flags.IsRecalculateCache()) return null;
        var previousSelect
            = previouslyCachedSelections.LastDeploySelectionResult(deploymentOptions.MessageGroupType, deploymentOptions);
        if (previousSelect == null) return null;
        var cacheResult = previousSelect.Value;
        if (cacheResult.RoutingFlags != deploymentOptions.RoutingFlags) return null;
        return new RouteSelectionResult(cacheResult.MessageQueue, StrategyName, deploymentOptions.RoutingFlags
            , deployRule);
    }

    public IDispatchSelectionResultSet? Select(IMessageQueueGroupContainer availableMessageQueues, IRule senderRule
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
