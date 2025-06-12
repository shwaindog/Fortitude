using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Configuration.Availability;

public interface ITimeZoneStartEndTime
{
    public TimeZoneInfo? OverrideTimeZone { get; set; }
    public TimeSpan      StartTime        { get; set; }
    public TimeSpan      EndTime          { get; set; }
}

// public class TimeZoneStartEndTime : ITimeZoneStartEndTime
// {
//
// }