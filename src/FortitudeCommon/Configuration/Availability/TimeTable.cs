using System;
using System.Collections.Generic;
using System.Linq;

namespace FortitudeCommon.Configuration.Availability
{
    public class TimeTable : ITimeTable
    {
        public TimeTable(TimeZone hostTimeZone = null, IWeeklyTimeTable regularTimeTable = null, 
            IList<IFixedCalendarDate> annualHolidays = null, 
            IList<IDateTimePeriod> shiftingHolidays = null, 
            IList<IDateTimePeriod> extraAvailability = null)
        {
            HostTimeZone = hostTimeZone;
            RegularTimeTable = regularTimeTable;
            AnnualHolidays = annualHolidays;
            ShiftingHolidays = shiftingHolidays;
            ExtraAvailability = extraAvailability;
        }

        public TimeTable(ITimeTable toClone)
        {
            HostTimeZone = toClone.HostTimeZone;
            RegularTimeTable = toClone.RegularTimeTable;
            AnnualHolidays = toClone.AnnualHolidays;
            ShiftingHolidays = toClone.ShiftingHolidays;
            ExtraAvailability = toClone.ExtraAvailability;
        }

        public bool ShouldBeUp(DateTimeOffset atThisDateTime)
        {
            if (!RegularTimeTable.ShouldBeUp(atThisDateTime))
            {
                return ExtraAvailability.Any(tp => tp.DateTimeContained(atThisDateTime));
            }
            return !(AnnualHolidays.Any(fcd => fcd.DateTimeContained(atThisDateTime))
                     || ShiftingHolidays.Any(dtp => dtp.DateTimeContained(atThisDateTime)))
                   || ExtraAvailability.Any(tp => tp.DateTimeContained(atThisDateTime));
        }

        public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
        {
            if (!ShouldBeUp(fromNow)) return TimeSpan.Zero;
            var allRemainingUpTimes =
                (from remainingUpTimes in new[] { RegularTimeTable.ExpectedRemainingUpTime(fromNow) }
                     .Concat(from fcd in AnnualHolidays
                             select fcd.TimeToNextPeriod(fromNow)
                    ).Concat(from dtp in ShiftingHolidays
                             where dtp.TimeToPeriod(fromNow) > TimeSpan.Zero
                             select dtp.TimeToPeriod(fromNow))
                 select remainingUpTimes
                ).ToArray();
            if (allRemainingUpTimes.Any())
            {
                return allRemainingUpTimes.Min();
            }
            return TimeSpan.Zero;
        }

        public DateTimeOffset NextScheduledRestartTime(DateTimeOffset fromNow)
        {
            DateTimeOffset weeklyTimeTableRestart = RegularTimeTable.NextScheduledRestartTime(fromNow);
            var allPossibleRestartTime = new[] { weeklyTimeTableRestart }.Concat(
                from dtp in ExtraAvailability
                where dtp.TimeToPeriod(fromNow) > TimeSpan.Zero
                select fromNow + dtp.TimeToPeriod(fromNow)
                ).ToArray();

            if (allPossibleRestartTime.Any())
            {
                return allPossibleRestartTime.Min();
            }
            return DateTimeOffset.MaxValue;
        }

        public TimeZone HostTimeZone { get; private set; }
        public IWeeklyTimeTable RegularTimeTable { get; private set; }
        public IList<IFixedCalendarDate> AnnualHolidays { get; private set; }
        public IList<IDateTimePeriod> ShiftingHolidays { get; private set; }
        public IList<IDateTimePeriod> ExtraAvailability { get; private set; }

        public ITimeTable Clone()
        {
            return new TimeTable(this);
        }

        protected bool Equals(TimeTable other)
        {
            return Equals(HostTimeZone, other.HostTimeZone) && Equals(RegularTimeTable, other.RegularTimeTable) && Equals(AnnualHolidays, other.AnnualHolidays) && Equals(ShiftingHolidays, other.ShiftingHolidays) && Equals(ExtraAvailability, other.ExtraAvailability); 
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimeTable) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (HostTimeZone != null ? HostTimeZone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RegularTimeTable != null ? RegularTimeTable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AnnualHolidays != null ? AnnualHolidays.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ShiftingHolidays != null ? ShiftingHolidays.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExtraAvailability != null ? ExtraAvailability.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
