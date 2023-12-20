#region

using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public class SwitchingBroadcastSenderDestinationCache : IDeployDispatchDestinationCache
{
    private readonly IDeployDispatchDestinationCache sendAllCache = new DeployDispatchDestinationCache();
    private readonly IDeployDispatchDestinationCache singleDestinationCache = new DeployDispatchDestinationCache();

    public void Save(IRule senderRule, IRule deployRule, DeploymentOptions deploymentOptions
        , RouteSelectionResult routeSelectionResult)
    {
        singleDestinationCache.Save(senderRule, deployRule, deploymentOptions, routeSelectionResult);
    }

    public void Save(IRule senderRule, DispatchOptions dispatchOptions, string destinationAddress
        , IDispatchSelectionResultSet selectionResult)
    {
        var flags = dispatchOptions.RoutingFlags;
        if (flags.IsSendToAll())
            sendAllCache.Save(senderRule, dispatchOptions, destinationAddress, selectionResult);
        else
            singleDestinationCache.Save(senderRule, dispatchOptions, destinationAddress, selectionResult);
    }

    public IDispatchSelectionResultSet? LastDestinationSelectionResultSet(string destinationAddress
        , DispatchOptions requestOptions)
    {
        var flags = requestOptions.RoutingFlags;
        return flags.IsSendToAll() ?
            sendAllCache.LastDestinationSelectionResultSet(destinationAddress, requestOptions) :
            singleDestinationCache.LastDestinationSelectionResultSet(destinationAddress, requestOptions);
    }

    public IDispatchSelectionResultSet? LastSenderDestinationSelectionResultSet(IRule sender, string destinationAddress
        , DispatchOptions requestOptions)
    {
        var flags = requestOptions.RoutingFlags;
        return flags.IsSendToAll() ?
            sendAllCache.LastSenderDestinationSelectionResultSet(sender, destinationAddress, requestOptions) :
            singleDestinationCache.LastSenderDestinationSelectionResultSet(sender, destinationAddress, requestOptions);
    }

    public RouteSelectionResult? LastDeploySelectionResult(EventQueueType eventQueueType
        , DeploymentOptions requestOptions) =>
        singleDestinationCache.LastDeploySelectionResult(eventQueueType, requestOptions);
}
