namespace Fortitude.EventProcessing.BusRules.Rules;

public interface IDeploymentOptions
{
    uint Instances { get; }
    bool IsWorker { get; }
    bool IsIOOutbound { get; }
    bool IsIOInbound { get; }
}

internal class DeploymentOptions : IDeploymentOptions
{
    public DeploymentOptions(uint instances = 1, bool isWorker = false, bool isIoOutbound = false
        , bool isIoInbound = false)
    {
        Instances = instances;
        IsWorker = isWorker;
        IsIOOutbound = isIoOutbound;
        IsIOInbound = isIoInbound;
    }

    public uint Instances { get; }
    public bool IsWorker { get; }
    public bool IsIOOutbound { get; }
    public bool IsIOInbound { get; }
}
