using System;

namespace FortitudeCommon.Chronometry
{
    public interface ITimeContext
    {
        DateTime UtcNow { get; }
        DateTime LocalTimeNow { get; }
    }
}