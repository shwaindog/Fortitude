#region

using FortitudeBusRules.Config;
using FortitudeBusRules.MessageBus;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules;

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
