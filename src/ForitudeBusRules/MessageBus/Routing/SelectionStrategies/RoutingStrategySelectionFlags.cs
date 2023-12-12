#region

using static FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingStrategySelectionFlags;

#endregion

namespace FortitudeBusRules.MessageBus.Routing.SelectionStrategies;

[Flags]
public enum RoutingStrategySelectionFlags
{
    None = 0
    , RecalculateCache = 1
    , DestinationCacheLast = 2
    , SenderCacheLast = 4
    , SendToAll = 8 // Unset send to One
    , RotateEvenly = 16
    , DefaultRequestResponse = 18
    , SelectAllMatching = 32 //Unset only first queue found
    , DefaultPublish = 40
    , LeastBusyQueue = 64
    , TargetSpecific = 128
    , DifferentToSenderQueue = 256
    , DefaultDeploy = 352
    , SameAsSenderQueue = 512
    , CanCreateNewQueue = 1024
}

public static class RoutingStrategySelectionExtensions
{
    public static RoutingStrategySelectionFlags XorToggleEnableDisabled(this RoutingStrategySelectionFlags original
        , RoutingStrategySelectionFlags enable = None
        , RoutingStrategySelectionFlags disable = None)
    {
        var limitedEnabled = enable & ~disable;
        var newlyEnabled = limitedEnabled & ~original;
        var originalDisabled = disable & original;
        return newlyEnabled | originalDisabled;
    }

    public static bool IsNone(this RoutingStrategySelectionFlags flags) => flags == None;
    public static bool IsRecalculateCache(this RoutingStrategySelectionFlags flags) => (flags & RecalculateCache) != 0;

    public static bool IsDestinationCacheLast(this RoutingStrategySelectionFlags flags) =>
        (flags & DestinationCacheLast) != 0;

    public static bool IsSendToAll(this RoutingStrategySelectionFlags flags) => (flags & SendToAll) != 0;

    public static bool IsRotateEvenly(this RoutingStrategySelectionFlags flags) => (flags & RotateEvenly) != 0;

    public static bool IsSenderCacheLast(this RoutingStrategySelectionFlags flags) => (flags & SenderCacheLast) != 0;


    public static bool IsDefaultRequestResponse(this RoutingStrategySelectionFlags flags) =>
        (flags & DefaultRequestResponse) == DefaultRequestResponse;

    public static bool IsSelectAllMatching(this RoutingStrategySelectionFlags flags) =>
        (flags & SelectAllMatching) != 0;

    public static bool IsDefaultPublish(this RoutingStrategySelectionFlags flags) =>
        (flags & DefaultPublish) == DefaultPublish;

    public static bool IsLeastBusyQueue(this RoutingStrategySelectionFlags flags) => (flags & LeastBusyQueue) != 0;
    public static bool IsTargetSpecific(this RoutingStrategySelectionFlags flags) => (flags & TargetSpecific) != 0;

    public static bool IsDifferentToSenderQueue(this RoutingStrategySelectionFlags flags) =>
        (flags & DifferentToSenderQueue) != 0;

    public static bool IsDefaultDeploy(this RoutingStrategySelectionFlags flags) =>
        (flags & DefaultDeploy) == DefaultDeploy;

    public static bool IsSameAsSenderQueue(this RoutingStrategySelectionFlags flags) =>
        (flags & SameAsSenderQueue) != 0;

    public static bool IsCanCreateNewQueue(this RoutingStrategySelectionFlags flags) =>
        (flags & CanCreateNewQueue) != 0;
}
