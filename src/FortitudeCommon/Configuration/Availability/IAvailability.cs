using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IAvailability
    {
        bool ShouldBeUp(DateTimeOffset atThisDateTime);
        TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow);
        DateTimeOffset NextScheduledRestartTime(DateTimeOffset fromNow);
    }
}