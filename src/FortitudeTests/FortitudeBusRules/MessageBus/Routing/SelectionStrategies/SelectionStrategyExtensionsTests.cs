#region

using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using static FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingFlags;

#endregion

namespace FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[TestClass]
public class RoutingFlagsExtensionsTests
{
    [TestMethod]
    public void XorToggleFlagsCorrectlyTogglesFlagsWhenXorWithOriginal()
    {
        var original = RecalculateCache | ExpireCacheAfterAMinute | ExpireCacheAfter100Reads | UseLastCacheEntry |
                       DestinationCacheLast | SenderCacheLast | SendToAll;
        var enableFlags = RotateEvenly | UseLastCacheEntry | TargetSpecific | CanCreateNewQueue;
        var disableFlags = DestinationCacheLast | SendToAll | UseLastCacheEntry | PreferNotSenderQueue |
                           SameAsSenderQueue;

        var xorSwitch = original.XorToggleEnableDisabled(enableFlags, disableFlags);
        var result = original ^ xorSwitch;

        Assert.IsTrue((result & RecalculateCache) != 0);
        Assert.IsTrue((result & ExpireCacheAfterAMinute) != 0);
        Assert.IsTrue((result & ExpireCacheAfter100Reads) != 0);
        Assert.IsFalse((result & UseLastCacheEntry) != 0);
        Assert.IsFalse((result & DestinationCacheLast) != 0);
        Assert.IsTrue((result & SenderCacheLast) != 0);
        Assert.IsFalse((result & SendToAll) != 0);
        Assert.IsFalse((result & DefaultPublish) == DefaultPublish);
        Assert.IsTrue((result & RotateEvenly) != 0);
        Assert.IsFalse((result & DefaultRequestResponse) != DefaultRequestResponse);
        Assert.IsFalse((result & LeastBusyQueue) != 0);
        Assert.IsTrue((result & TargetSpecific) != 0);
        Assert.IsFalse((result & PreferNotSenderQueue) != 0);
        Assert.IsFalse((result & DefaultDeploy) == DefaultDeploy);
        Assert.IsFalse((result & SameAsSenderQueue) != 0);
        Assert.IsTrue((result & CanCreateNewQueue) != 0);
    }

    [TestMethod]
    public void XorToggleFlagsWithNoEnabledCorrectlyTogglesFlagsWhenXorWithOriginal()
    {
        var original = RecalculateCache | ExpireCacheAfterAMinute | ExpireCacheAfter100Reads | UseLastCacheEntry |
                       DestinationCacheLast | SenderCacheLast | SendToAll;
        var disableFlags = DestinationCacheLast | SendToAll | UseLastCacheEntry | PreferNotSenderQueue |
                           SameAsSenderQueue;

        var xorSwitch = original.XorToggleEnableDisabled(disable: disableFlags);
        var result = original ^ xorSwitch;

        Assert.IsTrue((result & RecalculateCache) != 0);
        Assert.IsTrue((result & ExpireCacheAfterAMinute) != 0);
        Assert.IsTrue((result & ExpireCacheAfter100Reads) != 0);
        Assert.IsFalse((result & UseLastCacheEntry) != 0);
        Assert.IsFalse((result & DestinationCacheLast) != 0);
        Assert.IsTrue((result & SenderCacheLast) != 0);
        Assert.IsFalse((result & SendToAll) != 0);
        Assert.IsFalse((result & DefaultPublish) == DefaultPublish);
        Assert.IsFalse((result & RotateEvenly) != 0);
        Assert.IsFalse((result & DefaultRequestResponse) == DefaultRequestResponse);
        Assert.IsFalse((result & LeastBusyQueue) != 0);
        Assert.IsFalse((result & TargetSpecific) != 0);
        Assert.IsFalse((result & PreferNotSenderQueue) != 0);
        Assert.IsFalse((result & DefaultDeploy) == DefaultDeploy);
        Assert.IsFalse((result & SameAsSenderQueue) != 0);
        Assert.IsFalse((result & CanCreateNewQueue) != 0);
    }

    [TestMethod]
    public void XorToggleFlagsWithNoDisabledCorrectlyTogglesFlagsWhenXorWithOriginal()
    {
        var original = RecalculateCache | ExpireCacheAfterAMinute | ExpireCacheAfter100Reads | UseLastCacheEntry |
                       DestinationCacheLast | SenderCacheLast | SendToAll;
        var enableFlags = RotateEvenly | UseLastCacheEntry | TargetSpecific | CanCreateNewQueue;

        var xorSwitch = original.XorToggleEnableDisabled(enableFlags);
        var result = original ^ xorSwitch;

        Assert.IsTrue((result & RecalculateCache) != 0);
        Assert.IsTrue((result & ExpireCacheAfterAMinute) != 0);
        Assert.IsTrue((result & ExpireCacheAfter100Reads) != 0);
        Assert.IsTrue((result & UseLastCacheEntry) != 0);
        Assert.IsTrue((result & DestinationCacheLast) != 0);
        Assert.IsTrue((result & SenderCacheLast) != 0);
        Assert.IsTrue((result & SendToAll) != 0);
        Assert.IsTrue((result & DefaultPublish) == DefaultPublish);
        Assert.IsTrue((result & RotateEvenly) != 0);
        Assert.IsTrue((result & DefaultRequestResponse) == DefaultRequestResponse);
        Assert.IsFalse((result & LeastBusyQueue) != 0);
        Assert.IsTrue((result & TargetSpecific) != 0);
        Assert.IsFalse((result & DefaultDeploy) == DefaultDeploy);
        Assert.IsFalse((result & PreferNotSenderQueue) != 0);
        Assert.IsFalse((result & SameAsSenderQueue) != 0);
        Assert.IsTrue((result & CanCreateNewQueue) != 0);
    }

    [TestMethod]
    public void XorToggleFlagsNoAlterationsReturnsOriginal()
    {
        var original = RecalculateCache | ExpireCacheAfterAMinute | ExpireCacheAfter100Reads | UseLastCacheEntry |
                       DestinationCacheLast | SenderCacheLast | SendToAll;

        var xorSwitch = original.XorToggleEnableDisabled();
        var result = original ^ xorSwitch;

        Assert.IsTrue((result & RecalculateCache) != 0);
        Assert.IsTrue((result & ExpireCacheAfterAMinute) != 0);
        Assert.IsTrue((result & ExpireCacheAfter100Reads) != 0);
        Assert.IsTrue((result & UseLastCacheEntry) != 0);
        Assert.IsTrue((result & DestinationCacheLast) != 0);
        Assert.IsTrue((result & SenderCacheLast) != 0);
        Assert.IsTrue((result & SendToAll) != 0);
        Assert.IsTrue((result & DefaultPublish) == DefaultPublish);
        Assert.IsFalse((result & RotateEvenly) != 0);
        Assert.IsFalse((result & DefaultRequestResponse) == DefaultRequestResponse);
        Assert.IsFalse((result & LeastBusyQueue) != 0);
        Assert.IsFalse((result & TargetSpecific) != 0);
        Assert.IsFalse((result & PreferNotSenderQueue) != 0);
        Assert.IsFalse((result & DefaultDeploy) == DefaultDeploy);
        Assert.IsFalse((result & SameAsSenderQueue) != 0);
        Assert.IsFalse((result & CanCreateNewQueue) != 0);
    }
}
