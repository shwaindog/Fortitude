﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface IQueuesConfig
{
    int  MinEventQueues                { get; set; }
    int  MaxEventQueues                { get; set; }
    int  RequiredDataAccessQueues      { get; set; }
    int  RequiredCustomQueues          { get; set; }
    int  RequiredNetworkInboundQueues  { get; set; }
    int  RequiredNetworkOutboundQueues { get; set; }
    int  MinWorkerQueues               { get; set; }
    int  MaxWorkerQueues               { get; set; }
    int  DefaultQueueSize              { get; set; }
    int  EventQueueSize                { get; set; }
    uint MessagePumpMaxWaitMs          { get; set; }
    uint SelectorPollIntervalMs        { get; set; }
}

public class QueuesConfig : ConfigSection, IQueuesConfig
{
    private static readonly Dictionary<string, string?> Defaults = new()
    {
        { nameof(MinEventQueues), "1" }, { nameof(MaxEventQueues), "10" }
      , { nameof(RequiredDataAccessQueues), "0" }, { nameof(RequiredCustomQueues), "0" }
      , { nameof(RequiredNetworkInboundQueues), "0" }, { nameof(RequiredNetworkOutboundQueues), "0" }
      , { nameof(MaxWorkerQueues), "10" }, { nameof(MinWorkerQueues), "1" }
      , { nameof(EventQueueSize), "100_000" }, { nameof(DefaultQueueSize), "10_000" }
      , { nameof(MessagePumpMaxWaitMs), "30" }
    };

    public QueuesConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path)
    {
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
    }

    public QueuesConfig
    (int eventQueueSize = 10_000, int defaultQueueSize = 10_000, int minEventQueues = 1, int maxEventQueues = 10, int minWorkerQueues = 1
      , int maxWorkerQueues = 10, int requiredNetworkInboundQueues = 0, int requiredNetworkOutboundQueues = 0, int requiredDataAccessQueues = 0
      , int requiredCustomQueues = 0, uint messagePumpMaxWaitMs = 30)
    {
        MinEventQueues                = minEventQueues;
        MaxEventQueues                = maxEventQueues;
        RequiredDataAccessQueues      = requiredDataAccessQueues;
        RequiredCustomQueues          = requiredCustomQueues;
        RequiredNetworkInboundQueues  = requiredNetworkInboundQueues;
        RequiredNetworkOutboundQueues = requiredNetworkOutboundQueues;
        MinWorkerQueues               = minWorkerQueues;
        MaxWorkerQueues               = maxWorkerQueues;
        DefaultQueueSize              = defaultQueueSize;
        EventQueueSize                = eventQueueSize;
        MessagePumpMaxWaitMs          = messagePumpMaxWaitMs;
    }

    public QueuesConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public QueuesConfig(IQueuesConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        MinEventQueues                = toClone.MinEventQueues;
        MaxEventQueues                = toClone.MaxEventQueues;
        RequiredDataAccessQueues      = toClone.RequiredDataAccessQueues;
        RequiredCustomQueues          = toClone.RequiredCustomQueues;
        RequiredNetworkInboundQueues  = toClone.RequiredNetworkInboundQueues;
        RequiredNetworkOutboundQueues = toClone.RequiredNetworkOutboundQueues;
        MinWorkerQueues               = toClone.MinWorkerQueues;
        MaxWorkerQueues               = toClone.MaxWorkerQueues;
        DefaultQueueSize              = toClone.DefaultQueueSize;
        EventQueueSize                = toClone.EventQueueSize;
        MessagePumpMaxWaitMs          = toClone.MessagePumpMaxWaitMs;
    }

    public QueuesConfig(IQueuesConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public int MinEventQueues
    {
        get => int.Parse(this[nameof(MinEventQueues)]!);
        set => this[nameof(MinEventQueues)] = value.ToString();
    }

    public int MaxEventQueues
    {
        get => int.Parse(this[nameof(MaxEventQueues)]!);
        set => this[nameof(MaxEventQueues)] = value.ToString();
    }

    public int RequiredDataAccessQueues
    {
        get => int.Parse(this[nameof(RequiredDataAccessQueues)]!);
        set => this[nameof(RequiredDataAccessQueues)] = value.ToString();
    }

    public int RequiredCustomQueues
    {
        get => int.Parse(this[nameof(RequiredCustomQueues)]!);
        set => this[nameof(RequiredCustomQueues)] = value.ToString();
    }

    public int RequiredNetworkInboundQueues
    {
        get => int.Parse(this[nameof(RequiredNetworkInboundQueues)]!);
        set => this[nameof(RequiredNetworkInboundQueues)] = value.ToString();
    }

    public int RequiredNetworkOutboundQueues
    {
        get => int.Parse(this[nameof(RequiredNetworkOutboundQueues)]!);
        set => this[nameof(RequiredNetworkOutboundQueues)] = value.ToString();
    }

    public int MinWorkerQueues
    {
        get => int.Parse(this[nameof(MinWorkerQueues)]!);
        set => this[nameof(MinWorkerQueues)] = value.ToString();
    }

    public int MaxWorkerQueues
    {
        get => int.Parse(this[nameof(MaxWorkerQueues)]!);
        set => this[nameof(MaxWorkerQueues)] = value.ToString();
    }

    public int DefaultQueueSize
    {
        get => int.Parse(this[nameof(DefaultQueueSize)]!);
        set => this[nameof(DefaultQueueSize)] = value.ToString();
    }

    public int EventQueueSize
    {
        get => int.Parse(this[nameof(EventQueueSize)]!);
        set => this[nameof(EventQueueSize)] = value.ToString();
    }

    public uint MessagePumpMaxWaitMs
    {
        get => uint.Parse(this[nameof(MessagePumpMaxWaitMs)]!);
        set => this[nameof(MessagePumpMaxWaitMs)] = value.ToString();
    }

    public uint SelectorPollIntervalMs
    {
        get => uint.Parse(this[nameof(SelectorPollIntervalMs)]!);
        set => this[nameof(SelectorPollIntervalMs)] = value.ToString();
    }
}
