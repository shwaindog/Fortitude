#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.Config;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules;

public class BusRules
{
    public IConfigureMessageBus? EventBus;

    public IMessageBus Start(BusRulesConfig busRulesConfig, IRule bootstrapRule)
    {
        EventBus = new MessageBus(busRulesConfig);
        EventBus.Start();
        EventBus.DeployRuleAsync(bootstrapRule, bootstrapRule, new DeploymentOptions());
        return EventBus;
    }
}
