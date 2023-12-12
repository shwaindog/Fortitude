#region

using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

public interface IDeployDispatchStrategySelector
{
    ISelectionStrategy SelectDeployStrategy(IRule senderRule, IRule deployRule, DeploymentOptions deploymentOptions);

    ISelectionStrategy SelectDispatchStrategy(IRule senderRule, DispatchOptions deploymentOptions
        , string? destinationAddress = null);
}

public class DeployDispatchStrategySelector : IDeployDispatchStrategySelector
{
    private readonly EventQueueTypeOrderSelectionStrategy eventQueueTypeOrderSelectionStrategy = new();

    private IMap<string, SelectionResult> destinationStrategies
        = new ConcurrentMap<string, SelectionResult>();

    private IRecycler recycler;

    private IMap<IRule, IMap<string, ISelectionStrategy>> stickySourceDestinationStrategy
        = new ConcurrentMap<IRule, IMap<string, ISelectionStrategy>>();

    public DeployDispatchStrategySelector(IRecycler recycler) => this.recycler = recycler;

    public ISelectionStrategy SelectDeployStrategy(IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        var selectionAggregator = recycler.Borrow<SelectionStrategiesAggregator>();
        selectionAggregator.Add(eventQueueTypeOrderSelectionStrategy);


        return selectionAggregator;
    }

    public ISelectionStrategy SelectDispatchStrategy(IRule senderRule, DispatchOptions deploymentOptions
        , string? destinationAddress = null)
    {
        var selectionAggregator = recycler.Borrow<SelectionStrategiesAggregator>();
        selectionAggregator.Add(eventQueueTypeOrderSelectionStrategy);


        return selectionAggregator;
    }
}
