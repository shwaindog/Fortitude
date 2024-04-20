#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

public struct DispatchCacheEntry
{
    public IDispatchSelectionResultSet ResultSet;
    public DateTime CalculatedTime;
    public int RequestCount;

    public DispatchCacheEntry(IDispatchSelectionResultSet resultSet, DateTime? creationTime = default)
    {
        ResultSet = resultSet;
        CalculatedTime = creationTime ?? TimeContext.UtcNow;
        RequestCount = 0;
    }
}

public struct DeployCacheEntry
{
    public RouteSelectionResult RouteSelectionResult;
    public DateTime CalculatedTime;
    public int RequestCount;

    public DeployCacheEntry(RouteSelectionResult routeSelectionResult, DateTime? lastRequestTime = default)
    {
        RouteSelectionResult = routeSelectionResult;
        CalculatedTime = lastRequestTime ?? TimeContext.UtcNow;
        RequestCount = 0;
    }
}

public interface IDeployDispatchDestinationCache : ISaveSelectionResult, IPreviouslyCachedSelections { }

public class DeployDispatchDestinationCache : IDeployDispatchDestinationCache
{
    private readonly IMap<MessageQueueType, DeployCacheEntry?> deployQueueTypeCache
        = new ConcurrentMap<MessageQueueType, DeployCacheEntry?>();

    private readonly IMap<string, DispatchCacheEntry?> destinationAddressCache
        = new ConcurrentMap<string, DispatchCacheEntry?>();

    private readonly IMap<string, IMap<IRule, DispatchCacheEntry?>> destinationAddressSenderCache
        = new ConcurrentMap<string, IMap<IRule, DispatchCacheEntry?>>();

    public void Save(IRule senderRule, IRule deployRule, DeploymentOptions deploymentOptions
        , RouteSelectionResult routeSelectionResult)
    {
        var flags = deploymentOptions.RoutingFlags;
        if (!(flags.IsDestinationCacheLast() || flags.IsRotateEvenly()) ||
            routeSelectionResult.StrategyName == ReuseLastCachedResultSelectionStrategy.StrategyName) return;
        deployQueueTypeCache.AddOrUpdate(deploymentOptions.MessageGroupType, new DeployCacheEntry(routeSelectionResult));
    }

    public void Save(IRule senderRule, DispatchOptions dispatchOptions, string destinationAddress
        , IDispatchSelectionResultSet selectionResult)
    {
        var flags = dispatchOptions.RoutingFlags;
        if (!selectionResult.HasItems ||
            selectionResult.StrategyName == ReuseLastCachedResultSelectionStrategy.StrategyName) return;
        if (flags.IsDestinationCacheLast())
        {
            selectionResult.IncrementRefCount();
            var dispatchCacheEntry = new DispatchCacheEntry(selectionResult);
            // free replaced results
            if (destinationAddressCache.TryGetValue(destinationAddress, out var existing))
                existing?.ResultSet.DecrementRefCount();

            destinationAddressCache.AddOrUpdate(destinationAddress, dispatchCacheEntry);
        }

        if (flags.IsSenderCacheLast() || (flags.IsRotateEvenly() && !flags.IsDestinationCacheLast()))
        {
            selectionResult.IncrementRefCount(); // two references as a diff sender may update each
            var dispatchCacheEntry = new DispatchCacheEntry(selectionResult);
            var senderCache = destinationAddressSenderCache.GetOrPut(destinationAddress
                , _ => new ConcurrentMap<IRule, DispatchCacheEntry?>());
            // free replaced results
            if (senderCache.TryGetValue(senderRule, out var existing)) existing?.ResultSet.DecrementRefCount();
            senderCache.AddOrUpdate(senderRule, dispatchCacheEntry);
        }
    }

    public IDispatchSelectionResultSet? LastDestinationSelectionResultSet(string destinationAddress
        , DispatchOptions requestOptions)
    {
        var cacheEntry = destinationAddressCache.GetValue(destinationAddress);
        if (cacheEntry == null) return null;
        var updateEntry = cacheEntry.Value;
        return UpdateOrClearCache(destinationAddress, updateEntry) ? updateEntry.ResultSet : null;
    }

    public IDispatchSelectionResultSet?
        LastSenderDestinationSelectionResultSet(IRule sender, string destinationAddress, DispatchOptions requestOptions)
    {
        var senderCacheEntry = destinationAddressSenderCache.GetValue(destinationAddress);
        if (senderCacheEntry != null)
        {
            var cacheEntry = senderCacheEntry.GetValue(sender);
            if (cacheEntry == null) return null;
            var updateEntry = cacheEntry.Value;
            return UpdateOrClearCache(sender, senderCacheEntry, updateEntry) ? updateEntry.ResultSet : null;
        }

        return null;
    }

    public RouteSelectionResult? LastDeploySelectionResult(MessageQueueType messageQueueType
        , DeploymentOptions requestOptions)
    {
        var cacheEntry = deployQueueTypeCache.GetValue(messageQueueType);
        if (cacheEntry == null) return null;
        var updateEntry = cacheEntry.Value;
        return UpdateOrClearCache(messageQueueType, updateEntry) ? updateEntry.RouteSelectionResult : null;
    }

    private bool UpdateOrClearCache(MessageQueueType messageQueueType, DeployCacheEntry checkForExpiry)
    {
        checkForExpiry.RequestCount++;
        var flags = checkForExpiry.RouteSelectionResult.RoutingFlags;
        if ((flags.IsExpireCacheAfter100Reads() && checkForExpiry.RequestCount > 100) ||
            (flags.IsExpireCacheAfterAMinute() &&
             checkForExpiry.CalculatedTime + TimeSpan.FromMinutes(1) < TimeContext.UtcNow))
        {
            deployQueueTypeCache.Remove(messageQueueType);
            return false;
        }

        deployQueueTypeCache.AddOrUpdate(messageQueueType, checkForExpiry);
        return true;
    }

    private bool UpdateOrClearCache(string destinationAddress, DispatchCacheEntry checkForExpiry)
    {
        checkForExpiry.RequestCount++;
        var flags = checkForExpiry.ResultSet.DispatchOptions.RoutingFlags;
        if ((flags.IsExpireCacheAfter100Reads() && checkForExpiry.RequestCount > 100) ||
            (flags.IsExpireCacheAfterAMinute() &&
             checkForExpiry.CalculatedTime + TimeSpan.FromMinutes(1) < TimeContext.UtcNow))
        {
            checkForExpiry.ResultSet.DecrementRefCount();
            destinationAddressCache.Remove(destinationAddress);
            return false;
        }

        destinationAddressCache.AddOrUpdate(destinationAddress, checkForExpiry);
        return true;
    }

    private bool UpdateOrClearCache(IRule senderRule, IMap<IRule, DispatchCacheEntry?> senderCache
        , DispatchCacheEntry checkForExpiry)
    {
        checkForExpiry.RequestCount++;
        var flags = checkForExpiry.ResultSet.DispatchOptions.RoutingFlags;
        if ((flags.IsExpireCacheAfter100Reads() && checkForExpiry.RequestCount > 100) ||
            (flags.IsExpireCacheAfterAMinute() &&
             checkForExpiry.CalculatedTime + TimeSpan.FromMinutes(1) < TimeContext.UtcNow))
        {
            checkForExpiry.ResultSet.DecrementRefCount();
            senderCache.Remove(senderRule);
            return false;
        }

        senderCache.AddOrUpdate(senderRule, checkForExpiry);
        return true;
    }
}
