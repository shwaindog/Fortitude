namespace Fortitude.EventProcessing.BusRules.Rules;

public interface IDeploymentOptions
{
    uint Instances { get; }
    bool IsWorker { get; }
}

internal class DeploymentOptions : IDeploymentOptions
{
    public DeploymentOptions(uint instances = 1, bool isWorker = false)
    {
        Instances = instances;
        IsWorker = isWorker;
    }

    public uint Instances { get; }
    public bool IsWorker { get; }
}
