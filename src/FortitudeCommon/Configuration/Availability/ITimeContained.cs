using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface ITimeContained
    {
        bool ActiveTime(DateTime checkDateTime);
    }
}