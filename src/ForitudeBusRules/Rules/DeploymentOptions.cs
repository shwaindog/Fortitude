// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.Rules;

public readonly struct DeploymentOptions
{
    public readonly MessageQueueType MessageGroupType;
    public readonly RoutingFlags     RoutingFlags;

    public readonly uint Instances;

    public readonly string? SpecificEventQueueName = null;

    public readonly int  TimeoutMs   = 60_000;
    public readonly bool RunAsDaemon = false;

    public DeploymentOptions()
    {
        RoutingFlags     = RoutingFlags.DefaultDeploy;
        MessageGroupType = MessageQueueType.Event;
        Instances        = 1;
    }

    public DeploymentOptions
    (RoutingFlags routingFlags = RoutingFlags.DefaultDeploy
      , MessageQueueType messageGroupType = MessageQueueType.Event
      , uint instances = 1, string? specificEventQueueName = null, int timeoutMs = 60_000, bool runAsDaemon = false)
    {
        Instances        = instances;
        MessageGroupType = messageGroupType;

        SpecificEventQueueName = specificEventQueueName;

        TimeoutMs    = timeoutMs;
        RoutingFlags = routingFlags;
        RunAsDaemon  = runAsDaemon;
    }
}

public static class DeploymentOptionsExtensions
{
    public static DeploymentOptions Copy
    (this DeploymentOptions original
      , RoutingFlags xorRoutingFlags = RoutingFlags.None
      , MessageQueueType xorMessageGroupType = MessageQueueType.None, uint? instances = null
      , string? specificEventQueueName = null, int? timeoutMs = null, bool? runAsDaemon = null) =>
        new(xorRoutingFlags ^ original.RoutingFlags, xorMessageGroupType ^ original.MessageGroupType
          , instances ?? original.Instances, specificEventQueueName ?? original.SpecificEventQueueName
          , timeoutMs ?? original.TimeoutMs, runAsDaemon ?? original.RunAsDaemon);

    public static DeploymentOptions SetRunAsDaemon(this DeploymentOptions original, bool runAsDaemon) =>
        new(original.RoutingFlags, original.MessageGroupType
          , original.Instances, original.SpecificEventQueueName
          , original.TimeoutMs, runAsDaemon);

    public static bool RequiresSpecificEventQueue(this DeploymentOptions check) => check.SpecificEventQueueName.IsNotNullOrEmpty();
}
