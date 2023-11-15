#region

using Microsoft.Extensions.Configuration;

#endregion

namespace Fortitude.EventProcessing.BusRules.Config;

public class BusRulesConfig
{
    private readonly IConfigurationSection busRulesConfig;

    public BusRulesConfig(IConfigurationSection? busRulesConfig)
    {
        if (busRulesConfig != null)
        {
            this.busRulesConfig = busRulesConfig;
        }
        else
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { nameof(MaxEventLoops), "0" }, { nameof(MaxWorkerLoops), "0" }, { nameof(EventQueueSize), "50_000" }
                , { nameof(MessagePumpMaxWaitMs), "30" }
            }!);
            this.busRulesConfig = new ConfigurationSection(builder.Build(), "");
        }
    }

    public int MaxEventLoops
    {
        get => int.Parse(busRulesConfig[nameof(MaxEventLoops)]!);
        set => busRulesConfig[nameof(MaxEventLoops)] = value.ToString();
    }

    public int MaxWorkerLoops
    {
        get => int.Parse(busRulesConfig[nameof(MaxWorkerLoops)]!);
        set => busRulesConfig[nameof(MaxWorkerLoops)] = value.ToString();
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
}
