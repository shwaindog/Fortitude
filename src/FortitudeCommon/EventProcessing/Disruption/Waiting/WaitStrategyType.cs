namespace FortitudeCommon.EventProcessing.Disruption.Waiting
{
    public enum WaitStrategyType
    {
        BlockingMultiConsumers,
        BlockingSingleConsumer,
        Yielding,
        Spinning
    }
}