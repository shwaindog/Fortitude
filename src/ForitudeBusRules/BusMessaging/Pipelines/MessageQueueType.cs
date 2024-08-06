// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeBusRules.BusMessaging.Pipelines;

[Flags]
public enum MessageQueueType
{
    None            = 0
  , NetworkOutbound = 1  // socket sending
  , NetworkInbound  = 2  // socket listening
  , WebServer       = 4  // reserved for future integration with ASP.NET core
  , AllNetwork      = 7  // NetworkOutbound, NetworkInbound and future WebServer
  , Event           = 8  // fast executing non I/O rules and events, actions (eventual to be monitored by watch dog to report long running operations)
  , Worker          = 16 // un-monitored (won't have watchdog) for long running I/O rules e.g. web requests, gpu operations
  , DataAccess      = 32 // any data access requests to either a database or file
  , Custom          = 64 // application custom pool separate to allow applications to target Rules separate to worker or data access queues
  , AllNonNetwork   = 120
  , All             = 127
}

public static class MessageQueueTypeExtensions
{
    public static bool IsEvent(this MessageQueueType messageQueueType)  => (messageQueueType & MessageQueueType.Event) != 0;
    public static bool IsWorker(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.Worker) != 0;

    public static bool IsNetworkOutbound(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.NetworkOutbound) != 0;

    public static bool IsNetworkInbound(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.NetworkInbound) != 0;

    public static bool IsCustom(this MessageQueueType messageQueueType)     => (messageQueueType & MessageQueueType.Custom) != 0;
    public static bool IsDataAccess(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.DataAccess) != 0;
    public static bool IsAll(this MessageQueueType messageQueueType)        => messageQueueType == MessageQueueType.All;
    public static bool IsNone(this MessageQueueType messageQueueType)       => messageQueueType == MessageQueueType.None;
}
