#region

using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using static FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingStrategySelectionFlags;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class RoutingStrategySelectionExtensionsTests
{
    [TestMethod]
    public void XorToggleFlagsCorrectlyTogglesFlagsWhenXorWithOriginal()
    {
        var original = RecalculateCache | DestinationCacheLast | SenderCacheLast | SendToAll;
        var enableFlags = RotateEvenly | SelectAllMatching | TargetSpecific | CanCreateNewQueue;
        var disableFlags = DestinationCacheLast | SendToAll | SelectAllMatching | DifferentToSenderQueue |
                           SameAsSenderQueue;

        var xorSwitch = original.XorToggleEnableDisabled(enableFlags, disableFlags);
        var result = original ^ xorSwitch;

        Assert.IsTrue((result & RecalculateCache) != 0);
        Assert.IsFalse((result & DestinationCacheLast) != 0);
        Assert.IsTrue((result & SenderCacheLast) != 0);
        Assert.IsFalse((result & SendToAll) != 0);
        Assert.IsTrue((result & RotateEvenly) != 0);
        Assert.IsFalse((result & DefaultRequestResponse) == DefaultRequestResponse);
        Assert.IsFalse((result & SelectAllMatching) != 0);
        Assert.IsFalse((result & DefaultPublish) == DefaultPublish);
        Assert.IsFalse((result & LeastBusyQueue) != 0);
        Assert.IsTrue((result & TargetSpecific) != 0);
        Assert.IsFalse((result & DifferentToSenderQueue) != 0);
        Assert.IsFalse((result & DefaultDeploy) == DefaultDeploy);
        Assert.IsFalse((result & SameAsSenderQueue) != 0);
        Assert.IsTrue((result & CanCreateNewQueue) != 0);
    }

    [TestMethod]
    public void XorToggleFlagsWithNoEnabledCorrectlyTogglesFlagsWhenXorWithOriginal()
    {
        var original = RecalculateCache | DestinationCacheLast | SenderCacheLast | SendToAll;
        var disableFlags = DestinationCacheLast | SendToAll | SelectAllMatching | DifferentToSenderQueue |
                           SameAsSenderQueue;

        var xorSwitch = original.XorToggleEnableDisabled(disable: disableFlags);
        var result = original ^ xorSwitch;

        Assert.IsTrue((result & RecalculateCache) != 0);
        Assert.IsFalse((result & DestinationCacheLast) != 0);
        Assert.IsTrue((result & SenderCacheLast) != 0);
        Assert.IsFalse((result & SendToAll) != 0);
        Assert.IsFalse((result & RotateEvenly) != 0);
        Assert.IsFalse((result & DefaultRequestResponse) == DefaultRequestResponse);
        Assert.IsFalse((result & SelectAllMatching) != 0);
        Assert.IsFalse((result & DefaultPublish) == DefaultPublish);
        Assert.IsFalse((result & LeastBusyQueue) != 0);
        Assert.IsFalse((result & TargetSpecific) != 0);
        Assert.IsFalse((result & DifferentToSenderQueue) != 0);
        Assert.IsFalse((result & DefaultDeploy) == DefaultDeploy);
        Assert.IsFalse((result & SameAsSenderQueue) != 0);
        Assert.IsFalse((result & CanCreateNewQueue) != 0);
    }

    [TestMethod]
    public void XorToggleFlagsWithNoDisabledCorrectlyTogglesFlagsWhenXorWithOriginal()
    {
        var original = RecalculateCache | DestinationCacheLast | SenderCacheLast | SendToAll;
        var enableFlags = RotateEvenly | SelectAllMatching | TargetSpecific | CanCreateNewQueue;

        var xorSwitch = original.XorToggleEnableDisabled(enableFlags);
        var result = original ^ xorSwitch;

        Assert.IsTrue((result & RecalculateCache) != 0);
        Assert.IsTrue((result & DestinationCacheLast) != 0);
        Assert.IsTrue((result & SenderCacheLast) != 0);
        Assert.IsTrue((result & SendToAll) != 0);
        Assert.IsTrue((result & RotateEvenly) != 0);
        Assert.IsTrue((result & DefaultRequestResponse) == DefaultRequestResponse);
        Assert.IsTrue((result & SelectAllMatching) != 0);
        Assert.IsTrue((result & DefaultPublish) == DefaultPublish);
        Assert.IsFalse((result & LeastBusyQueue) != 0);
        Assert.IsTrue((result & TargetSpecific) != 0);
        Assert.IsFalse((result & DefaultDeploy) == DefaultDeploy);
        Assert.IsFalse((result & DifferentToSenderQueue) != 0);
        Assert.IsFalse((result & SameAsSenderQueue) != 0);
        Assert.IsTrue((result & CanCreateNewQueue) != 0);
    }

    [TestMethod]
    public void XorToggleFlagsNoAlterationsReturnsOriginal()
    {
        var original = RecalculateCache | DestinationCacheLast | SenderCacheLast | SendToAll;

        var xorSwitch = original.XorToggleEnableDisabled();
        var result = original ^ xorSwitch;

        Assert.IsTrue((result & RecalculateCache) != 0);
        Assert.IsTrue((result & DestinationCacheLast) != 0);
        Assert.IsTrue((result & SenderCacheLast) != 0);
        Assert.IsTrue((result & SendToAll) != 0);
        Assert.IsFalse((result & RotateEvenly) != 0);
        Assert.IsFalse((result & DefaultRequestResponse) == DefaultRequestResponse);
        Assert.IsFalse((result & SelectAllMatching) != 0);
        Assert.IsFalse((result & DefaultPublish) == DefaultPublish);
        Assert.IsFalse((result & LeastBusyQueue) != 0);
        Assert.IsFalse((result & TargetSpecific) != 0);
        Assert.IsFalse((result & DifferentToSenderQueue) != 0);
        Assert.IsFalse((result & DefaultDeploy) == DefaultDeploy);
        Assert.IsFalse((result & SameAsSenderQueue) != 0);
        Assert.IsFalse((result & CanCreateNewQueue) != 0);
    }
}
