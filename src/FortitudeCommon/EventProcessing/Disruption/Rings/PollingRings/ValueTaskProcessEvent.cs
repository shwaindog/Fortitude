// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public delegate ValueTask ValueTaskProcessEvent<in T>(T data) where T : class;

public class WrappedExceptionCatchingValueTaskProcessEvent<T>(ValueTaskProcessEvent<T> action, IFLogger logger) where T : class
{
    public async ValueTask SafeCall(T data)
    {
        try
        {
            await action(data);
        }
        catch (Exception ex)
        {
            logger.Warn("Caught exception processing ValueTaskPollSink data {0}. Got {1}", data, ex);
        }
    }
}
