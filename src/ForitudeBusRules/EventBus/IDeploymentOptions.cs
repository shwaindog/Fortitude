namespace Fortitude.EventProcessing.BusRules.EventBus;

public interface IDeploymentOptions
{
    uint Instances { get; }
    bool IsWorker { get; }
}
