using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface ITimeContained
    {
        bool TimeContained(TimeSpan checkDateTime);
    }
}