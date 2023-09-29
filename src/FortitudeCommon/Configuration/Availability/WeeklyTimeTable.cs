using System;
using System.Collections.Generic;
using System.Linq;

namespace FortitudeCommon.Configuration.Availability
{
    public class WeeklyTimeTable : IWeeklyTimeTable
    {
        public WeeklyTimeTable(IInterDayWeeklyPeriod expectedUpTime, IList<IDayOfWeekAvailability> daysOfWeek)
        {
            ExpectedUpTime = expectedUpTime;
            DaysOfWeek = daysOfWeek;
        }

        public IInterDayWeeklyPeriod ExpectedUpTime { get; private set; }
        public IList<IDayOfWeekAvailability> DaysOfWeek { get; private set; }

        public bool ShouldBeUp(DateTimeOffset atThisDateTime)
        {
            return ExpectedUpTime.ShouldBeUp(atThisDateTime) && DaysOfWeek.All(dow => dow.ShouldBeUp(atThisDateTime));
        }

        public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
        {
            return new [] {ExpectedUpTime.ExpectedRemainingUpTime(fromNow)}.Concat(
                    DaysOfWeek.Select(dow => dow.ExpectedRemainingUpTime(fromNow))).Min();
        }

        public DateTimeOffset NextScheduledRestartTime(DateTimeOffset fromNow)
        {
            return new[] { ExpectedUpTime.NextScheduledRestartTime(fromNow) }.Concat(
                    DaysOfWeek.Select(dow => dow.NextScheduledRestartTime(fromNow))).Min();
        }
    }
}