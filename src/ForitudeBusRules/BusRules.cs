#region

using Fortitude.EventProcessing.BusRules.Config;
using Fortitude.EventProcessing.BusRules.MessageBus;
using Fortitude.EventProcessing.BusRules.Rules;

#endregion

namespace Fortitude.EventProcessing.BusRules;

public class BusRules
{
    public IConfigureEventBus? EventBus;

    public IEventBus Start(BusRulesConfig busRulesConfig, IRule bootstrapRule)
    {
        EventBus = new EventBus(busRulesConfig);
        EventBus.Start();
        EventBus.DeployRuleAsync(bootstrapRule, bootstrapRule, new DeploymentOptions());
        return EventBus;
    }
}
