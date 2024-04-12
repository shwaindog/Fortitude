#region

using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public delegate ValueTask<long> ValueTaskProcessEvent<in T>(long currentSequence, T data) where T : class;

public class WrappedExceptionCatchingValueTaskProcessEvent<T>(ValueTaskProcessEvent<T> action, IFLogger logger) where T : class
{
    public async ValueTask<long> SafeCall(long currentSequence, T data)
    {
        try
        {
            await action(currentSequence, data);
        }
        catch (Exception ex)
        {
            logger.Warn("Caught exception processing ValueTaskPollSink data {0}. Got {1}", data, ex);
        }

        return currentSequence;
    }
}
