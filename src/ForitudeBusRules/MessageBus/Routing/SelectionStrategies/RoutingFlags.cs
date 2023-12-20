#region

using static FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingFlags;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[Flags]
public enum RoutingFlags
{
    None = 0
    , RecalculateCache = 1 // force cache reset if required
    , ExpireCacheAfterAMinute = 2
    , ExpireCacheAfter100Reads = 4
    , UseLastCacheEntry = 8 //Unset only first matching queue found
    , DestinationCacheLast = 16 // support fast routing and rotate requests evenly
    , SenderCacheLast = 32 // so sender can have a stateful conversation with another rule
    , SendToAll
        = 64 // publish can send to all or one, requests and stateful conversations can only send to one.  Unset is send to one.

    , DefaultPublish
        = 64 + 16 + 8 + 4 +
          2 // SendToAll | DestinationCacheLast | UseLastCacheEntry | ExpireCacheAfter100Reads | ExpireCacheAfterAMinute

    , RotateEvenly = 128 // Will set SenderCacheLast to know which is next
    , DefaultRequestResponse = 128 + 32 // RotateEvenly | SenderCacheLast
    , LeastBusyQueue = 256 // If neither LeastBusy or RotateEvenly then First Queue in order
    , DefaultStatefulConversation = 256 + 32 + 8 // LeastBusy | SenderCacheLast | UseLastCacheEntry
    , TargetSpecific = 512 // Either specific Queue or Rule
    , PreferNotSenderQueue = 1024 // Prefer deploying to a queue other than the sender queue
    , DefaultDeploy
        = 1024 + 256 + 128 + 16 // PreferNotSenderQueue | LeastBusyQueue | RotateEvenly | DestinationCacheLast

    , SameAsSenderQueue = 2048 // prefer deploying or dispatching to the same queue as sender.
    // If both or none Of *SenderQueue then Rotate Evenly or LeastBusy or First In Order
    , CanCreateNewQueue
        = 4096 // if no matching criteria can be found allow creation of a new queue to support deployment
}

public static class RoutingFlagsExtensions
{
    public static RoutingFlags XorToggleEnableDisabled(this RoutingFlags original
        , RoutingFlags enable = None
        , RoutingFlags disable = None)
    {
        var limitedEnabled = enable & ~disable;
        var newlyEnabled = limitedEnabled & ~original;
        var originalDisabled = disable & original;
        return newlyEnabled | originalDisabled;
    }

    public static bool IsNone(this RoutingFlags flags) => flags == None;
    public static bool IsRecalculateCache(this RoutingFlags flags) => (flags & RecalculateCache) != 0;
    public static bool IsExpireCacheAfterAMinute(this RoutingFlags flags) => (flags & ExpireCacheAfterAMinute) != 0;
    public static bool IsExpireCacheAfter100Reads(this RoutingFlags flags) => (flags & ExpireCacheAfter100Reads) != 0;

    public static bool IsUseLastCacheEntry(this RoutingFlags flags) => (flags & UseLastCacheEntry) != 0;

    public static bool IsDestinationCacheLast(this RoutingFlags flags) => (flags & DestinationCacheLast) != 0;

    public static bool IsSendToAll(this RoutingFlags flags) => (flags & SendToAll) != 0;
    public static bool IsSendToOne(this RoutingFlags flags) => (flags & SendToAll) == 0;

    public static bool IsRotateEvenly(this RoutingFlags flags) => (flags & RotateEvenly) != 0;

    public static bool IsSenderCacheLast(this RoutingFlags flags) => (flags & SenderCacheLast) != 0;


    public static bool IsDefaultRequestResponse(this RoutingFlags flags) =>
        (flags & DefaultRequestResponse) == DefaultRequestResponse;


    public static bool IsDefaultPublish(this RoutingFlags flags) => (flags & DefaultPublish) == DefaultPublish;

    public static bool IsLeastBusyQueue(this RoutingFlags flags) => (flags & LeastBusyQueue) != 0;

    public static bool IsDefaultStatefulConversation(this RoutingFlags flags) =>
        (flags & DefaultStatefulConversation) == DefaultStatefulConversation;

    public static bool IsTargetSpecific(this RoutingFlags flags) => (flags & TargetSpecific) != 0;

    public static bool IsPreferNotSenderQueue(this RoutingFlags flags) => (flags & PreferNotSenderQueue) != 0;

    public static bool IsDefaultDeploy(this RoutingFlags flags) => (flags & DefaultDeploy) == DefaultDeploy;

    public static bool IsSameAsSenderQueue(this RoutingFlags flags) => (flags & SameAsSenderQueue) != 0;

    public static bool IsCanCreateNewQueue(this RoutingFlags flags) => (flags & CanCreateNewQueue) != 0;
}
