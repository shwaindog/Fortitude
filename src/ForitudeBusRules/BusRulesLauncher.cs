using Fortitude.EventProcessing.BusRules.Config;
using Fortitude.EventProcessing.BusRules.EventBus;

namespace Fortitude.EventProcessing.BusRules
{
    public class BusRulesLauncher
    {

        public void /*IEventBus */ Start(BusRulesConfig busRulesConfig)
        {
            // find the number of cores

            // spin up message bus for each core

            // register each msg bus to the event bus

            // return eventBus

        }
    }
}
