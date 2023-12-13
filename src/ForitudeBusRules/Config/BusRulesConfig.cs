#region

using FortitudeBusRules.Injection;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public class BusRulesConfig
{
    private readonly IConfigurationSection busRulesConfig;

    private readonly Dictionary<string, string?> defaults = new()
    {
        { nameof(MinEventQueues), "1" }, { nameof(MaxEventQueues), "10" }
        , { nameof(RequiredIOInboundQueues), "1" }, { nameof(RequiredIOOutboundQueues), "1" }
        , { nameof(MaxWorkerQueues), "10" }, { nameof(MinWorkerQueues), "1" }
        , { nameof(EventQueueSize), "50_000" }, { nameof(DefaultQueueSize), "10_000" }
        , { nameof(MessagePumpMaxWaitMs), "30" }
    };

    public BusRulesConfig(IConfigurationSection? busRulesConfig)
    {
        if (busRulesConfig != null)
        {
            this.busRulesConfig = busRulesConfig;
            foreach (var defaultKvp in defaults) busRulesConfig[defaultKvp.Key] ??= defaultKvp.Value;
        }
        else
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(defaults);
            this.busRulesConfig = new ConfigurationSection(builder.Build(), "");
        }
    }

    public int MinEventQueues
    {
        get => int.Parse(busRulesConfig[nameof(MinEventQueues)]!);
        set => busRulesConfig[nameof(MinEventQueues)] = value.ToString();
    }

    public int MaxEventQueues
    {
        get => int.Parse(busRulesConfig[nameof(MaxEventQueues)]!);
        set => busRulesConfig[nameof(MaxEventQueues)] = value.ToString();
    }

    public int RequiredIOInboundQueues
    {
        get => int.Parse(busRulesConfig[nameof(RequiredIOInboundQueues)]!);
        set => busRulesConfig[nameof(RequiredIOInboundQueues)] = value.ToString();
    }

    public int RequiredIOOutboundQueues
    {
        get => int.Parse(busRulesConfig[nameof(RequiredIOOutboundQueues)]!);
        set => busRulesConfig[nameof(RequiredIOOutboundQueues)] = value.ToString();
    }

    public int MinWorkerQueues
    {
        get => int.Parse(busRulesConfig[nameof(MinWorkerQueues)]!);
        set => busRulesConfig[nameof(MinWorkerQueues)] = value.ToString();
    }

    public int MaxWorkerQueues
    {
        get => int.Parse(busRulesConfig[nameof(MaxWorkerQueues)]!);
        set => busRulesConfig[nameof(MaxWorkerQueues)] = value.ToString();
    }

    public int DefaultQueueSize
    {
        get => int.Parse(busRulesConfig[nameof(DefaultQueueSize)]!);
        set => busRulesConfig[nameof(DefaultQueueSize)] = value.ToString();
    }

    public int EventQueueSize
    {
        get => int.Parse(busRulesConfig[nameof(EventQueueSize)]!);
        set => busRulesConfig[nameof(EventQueueSize)] = value.ToString();
    }

    public int MessagePumpMaxWaitMs
    {
        get => int.Parse(busRulesConfig[nameof(MessagePumpMaxWaitMs)]!);
        set => busRulesConfig[nameof(MessagePumpMaxWaitMs)] = value.ToString();
    }

    public IDependencyResolver? Resolver { get; set; }

    public IConfigurationSection ToConfigurationSection() => busRulesConfig;
}
