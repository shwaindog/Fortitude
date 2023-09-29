using System.Collections.Generic;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IWeeklyTimeTable : IAvailability
    {
        IInterDayWeeklyPeriod ExpectedUpTime { get; }
        IList<IDayOfWeekAvailability> DaysOfWeek { get; }
    }
}