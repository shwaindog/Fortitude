#region

using Fortitude.EventProcessing.BusRules.Config;
using Fortitude.EventProcessing.BusRules.MessageBus;
using Fortitude.EventProcessing.BusRules.Rules;

#endregion

namespace Fortitude.EventProcessing.BusRules;

public class BusRulesLauncher
{
    private IEventBus? eventBus;

    public void /*IEventBus */ Start(BusRulesConfig busRulesConfig, IRule bootstrapRule)
    {
        eventBus = new EventBus(busRulesConfig);
        eventBus.DeployRuleAsync(bootstrapRule, bootstrapRule, new DeploymentOptions());
    }
}
