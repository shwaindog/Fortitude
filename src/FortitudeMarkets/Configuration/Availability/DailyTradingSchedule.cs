namespace FortitudeMarkets.Configuration.Availability;

public interface IDailyTradingSchedule
{                                                                        //
    public int? TradingStartsFromOpenMinutes { get; set; }               //  if negative then accepting orders before open.
                                                                         //  if positive until reached is non-preffered, low liquidity, then normal
    public ITimeZoneStartEndTime? PreferredTradingTimes { get; set; }    //  optionall usually set to main trading timezone until reached normal liquidity
                                                                         //  and neither non/preferred
    public ITimeZoneStartEndTime? HighActivityTimes { get; set; }        //  until reached neither just normal
                                                                         //
    public TimeSpan? AnnounceClosingSoonFromCloseMinutes { get; set; }   // offset from 
}

// public class DailyTradingSchedule : IDailyTradingSchedule
// {
//
//
// }