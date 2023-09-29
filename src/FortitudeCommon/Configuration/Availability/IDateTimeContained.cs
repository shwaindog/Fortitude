using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IDateTimeContained
    {
        bool DateTimeContained(DateTimeOffset checkDateTime);
    }
}