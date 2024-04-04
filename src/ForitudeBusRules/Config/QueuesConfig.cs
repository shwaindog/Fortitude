#region

using FortitudeCommon.Configuration;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface IQueuesConfig
{
    int MinEventQueues { get; set; }
    int MaxEventQueues { get; set; }
    int RequiredIOInboundQueues { get; set; }
    int RequiredIOOutboundQueues { get; set; }
    int MinWorkerQueues { get; set; }
    int MaxWorkerQueues { get; set; }
    int DefaultQueueSize { get; set; }
    int EventQueueSize { get; set; }
    int MessagePumpMaxWaitMs { get; set; }
}

public class QueuesConfig : ConfigSection, IQueuesConfig
{
    private static readonly Dictionary<string, string?> Defaults = new()
    {
        { nameof(MinEventQueues), "1" }, { nameof(MaxEventQueues), "10" }
        , { nameof(RequiredIOInboundQueues), "1" }, { nameof(RequiredIOOutboundQueues), "1" }
        , { nameof(MaxWorkerQueues), "10" }, { nameof(MinWorkerQueues), "1" }
        , { nameof(EventQueueSize), "50_000" }, { nameof(DefaultQueueSize), "10_000" }
        , { nameof(MessagePumpMaxWaitMs), "30" }
    };

    public QueuesConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path)
    {
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
    }

    public QueuesConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public QueuesConfig(IQueuesConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        MinEventQueues = toClone.MinEventQueues;
        MaxEventQueues = toClone.MaxEventQueues;
        RequiredIOInboundQueues = toClone.RequiredIOInboundQueues;
        RequiredIOOutboundQueues = toClone.RequiredIOOutboundQueues;
        MinWorkerQueues = toClone.MinWorkerQueues;
        MaxWorkerQueues = toClone.MaxWorkerQueues;
        DefaultQueueSize = toClone.DefaultQueueSize;
        EventQueueSize = toClone.EventQueueSize;
        MessagePumpMaxWaitMs = toClone.MessagePumpMaxWaitMs;
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

    public int RequiredIOInboundQueues
    {
        get => int.Parse(this[nameof(RequiredIOInboundQueues)]!);
        set => this[nameof(RequiredIOInboundQueues)] = value.ToString();
    }

    public int RequiredIOOutboundQueues
    {
        get => int.Parse(this[nameof(RequiredIOOutboundQueues)]!);
        set => this[nameof(RequiredIOOutboundQueues)] = value.ToString();
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

    public int MessagePumpMaxWaitMs
    {
        get => int.Parse(this[nameof(MessagePumpMaxWaitMs)]!);
        set => this[nameof(MessagePumpMaxWaitMs)] = value.ToString();
    }
}
