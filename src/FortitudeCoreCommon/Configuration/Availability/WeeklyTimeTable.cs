using System;
using System.Collections.Generic;
using System.Linq;

namespace FortitudeCommon.Configuration.Availability
{
    public class WeeklyTimeTable : IWeeklyTimeTable
    {
        public WeeklyTimeTable(IInterDayWeeklyPeriod expectedUpTime, IList<IDayOfWeekAvailability> expectedDailyOutages)
        {
            ExpectedUpTime = expectedUpTime;
            ExpectedDailyOutages = expectedDailyOutages;
        }

        public IInterDayWeeklyPeriod ExpectedUpTime { get; }
        public IList<IDayOfWeekAvailability> ExpectedDailyOutages { get; }

        public bool ShouldBeUp(DateTimeOffset atThisDateTime)
        {
            return ExpectedUpTime.ShouldBeUp(atThisDateTime) && !ExpectedDailyOutages.Any(dow => dow.ShouldBeUp(atThisDateTime));
        }

        public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
        {
            return new [] {ExpectedUpTime.ExpectedRemainingUpTime(fromNow)}.Concat(
                ExpectedDailyOutages.Select(dow => dow.ExpectedRemainingUpTime(fromNow))).Min();
        }

        public DateTimeOffset NextScheduledRestartTime(DateTimeOffset fromNow)
        {
            return new[] { ExpectedUpTime.NextScheduledRestartTime(fromNow) }.Concat(
                ExpectedDailyOutages.Select(dow => dow.NextScheduledRestartTime(fromNow))).Min();
        }
    }
}