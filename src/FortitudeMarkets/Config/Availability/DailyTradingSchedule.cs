using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;
using static FortitudeMarkets.Config.Availability.TradingPeriodTypeFlags;

namespace FortitudeMarkets.Config.Availability;

public interface IDailyTradingScheduleConfig : IInterfacesComparable<IDailyTradingScheduleConfig>, IAlterWeeklyAvailability, IStringBearer
{
    //  if negative then accepting orders before open.
    //  if positive until reached is non-preffered, low liquidity, then normal
    public TimeSpan? TradingStartsFromOpenTimeSpan { get; set; }

    //  optional usually set to main trading timezone until reached normal liquidity
    //  and neither non/preferred
    public ITimeZoneStartStopTimeConfig? OverridePreferredTradingTimes        { get; set; }
    public ITimeZoneStartStopTimeConfig? HighActivityTimes                    { get; set; } //  until reached neither just normal
    public TimeSpan?                     AnnounceClosingSoonFromCloseTimeSpan { get; set; } // positive before close 
}

public class DailyTradingScheduleConfig : ConfigSection, IDailyTradingScheduleConfig
{
    private TradingPeriodTypeFlags allOpenFlags = IsTrading | IsAcceptingOrders;
    public DailyTradingScheduleConfig() { }

    public DailyTradingScheduleConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public DailyTradingScheduleConfig
    (TimeSpan? tradingStartsFromOpen, ITimeZoneStartStopTimeConfig? overridePreferredTradingTimes
      , ITimeZoneStartStopTimeConfig? highActivityTimes, TimeSpan? announceClosingSoonFromClose)
        : this(InMemoryConfigRoot, InMemoryPath, tradingStartsFromOpen, overridePreferredTradingTimes, highActivityTimes
             , announceClosingSoonFromClose) { }

    public DailyTradingScheduleConfig
    (IConfigurationRoot root, string path, TimeSpan? tradingStartsFromOpen, ITimeZoneStartStopTimeConfig? overridePreferredTradingTimes
      , ITimeZoneStartStopTimeConfig? highActivityTimes, TimeSpan? announceClosingSoonFromClose) : base(root, path)
    {
        TradingStartsFromOpenTimeSpan        = tradingStartsFromOpen;
        OverridePreferredTradingTimes        = overridePreferredTradingTimes;
        HighActivityTimes                    = highActivityTimes;
        AnnounceClosingSoonFromCloseTimeSpan = announceClosingSoonFromClose;
    }

    public DailyTradingScheduleConfig(IDailyTradingScheduleConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        TradingStartsFromOpenTimeSpan        = toClone.TradingStartsFromOpenTimeSpan;
        OverridePreferredTradingTimes        = toClone.OverridePreferredTradingTimes;
        HighActivityTimes                    = toClone.HighActivityTimes;
        AnnounceClosingSoonFromCloseTimeSpan = toClone.AnnounceClosingSoonFromCloseTimeSpan;
    }

    public DailyTradingScheduleConfig(IDailyTradingScheduleConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeSpan? TradingStartsFromOpenTimeSpan
    {
        get
        {
            var checkValue = this[nameof(TradingStartsFromOpenTimeSpan)];
            return StringExtensions.IsNotNullOrEmpty(checkValue) ? TimeSpan.Parse(checkValue!) : null;
        }
        set => this[nameof(TradingStartsFromOpenTimeSpan)] = value?.ToString() ?? "";
    }

    public ITimeZoneStartStopTimeConfig? OverridePreferredTradingTimes
    {
        get
        {
            if (GetSection(nameof(OverridePreferredTradingTimes)).GetChildren().Any(cs => StringExtensions.IsNotNullOrEmpty(cs.Value)))
            {
                var pricingServerConfig = new TimeZoneStartStopTimeConfig(ConfigRoot, $"{Path}{Split}{nameof(OverridePreferredTradingTimes)}")
                {
                    ParentTimeZone = ParentTimeZone
                };
                return pricingServerConfig;
            }
            return null;
        }
        set
        {
            if (value is TimeZoneStartStopTimeConfig valueTrStartStopTimeConfig)
            {
                valueTrStartStopTimeConfig.ParentTimeZone = ParentTimeZone;
            }
            _ = value != null
                ? new TimeZoneStartStopTimeConfig(value, ConfigRoot, $"{Path}{Split}{nameof(OverridePreferredTradingTimes)}")
                : null;
        }
    }

    public ITimeZoneStartStopTimeConfig? HighActivityTimes
    {
        get
        {
            if (GetSection(nameof(HighActivityTimes)).GetChildren().Any(cs => StringExtensions.IsNotNullOrEmpty(cs.Value)))
            {
                var pricingServerConfig = new TimeZoneStartStopTimeConfig(ConfigRoot, $"{Path}{Split}{nameof(HighActivityTimes)}")
                {
                    ParentTimeZone = ParentTimeZone
                };
                return pricingServerConfig;
            }
            return null;
        }
        set
        {
            if (value is TimeZoneStartStopTimeConfig valueTrStartStopTimeConfig)
            {
                valueTrStartStopTimeConfig.ParentTimeZone = ParentTimeZone;
            }
            _ = value != null
                ? new TimeZoneStartStopTimeConfig(value, ConfigRoot, $"{Path}{Split}{nameof(HighActivityTimes)}")
                : null;
        }
    }

    public TimeSpan? AnnounceClosingSoonFromCloseTimeSpan
    {
        get
        {
            var checkValue = this[nameof(AnnounceClosingSoonFromCloseTimeSpan)];
            return StringExtensions.IsNotNullOrEmpty(checkValue) ? TimeSpan.Parse(checkValue!) : null;
        }
        set => this[nameof(AnnounceClosingSoonFromCloseTimeSpan)] = value?.ToString() ?? "";
    }

    public TimeZoneInfo? ParentTimeZone { get; set; }

    public TradingPeriodTypeFlags PreOpenFlags { get; set; } = IsPreOpen | IsAcceptingOrders;

    public TradingPeriodTypeFlags OpenFlags { get; set; } = IsPricing | IsOpen;

    public TradingPeriodTypeFlags AllOpenFlags
    {
        get => OpenFlags | allOpenFlags;
        set => allOpenFlags = value;
    }

    public WeeklyTradingSchedule WeeklySchedule(DateTimeOffset forTimeInWeek, WeeklyTradingSchedule original)
    {
        var tradingSchedule = Recycler.Borrow<WeeklyTradingSchedule>().Initialise(forTimeInWeek);

        AddOpenCloseTransitions(original, tradingSchedule);
        AddOnOffPeriodRange(tradingSchedule, HighActivityTimes, IsHighActivityPeriod, IsLowActivityPeriod);
        AddOnOffPeriodRange(tradingSchedule, OverridePreferredTradingTimes, IsPreferredTradingPeriod, IsNonPreferredTradingPeriod);

        return tradingSchedule;
    }

    private void AddOnOffPeriodRange(WeeklyTradingSchedule tradingSchedule, ITimeZoneStartStopTimeConfig? periodRange, TradingPeriodTypeFlags onState, TradingPeriodTypeFlags offState)
    {
        if (periodRange == null) return;
        var prevTransition = tradingSchedule[0];
        for (var i = 0; i < tradingSchedule.Count; i++)
        {
            var currentTransition = tradingSchedule[i];
            if (prevTransition.IsOpenTransition(currentTransition))
            {
                var openingTransition = currentTransition;

                var startTime = periodRange.NextStartTimeFromDate(openingTransition.AtTime);
                var stopTime  = periodRange.NextStopTimeFromDate(openingTransition.AtTime);
                if (startTime < openingTransition.AtTime)
                {
                    startTime = openingTransition.AtTime;
                }

                var foundIndex = tradingSchedule.FindTimeMatchAt(startTime);
                if (foundIndex > 0)
                {
                    tradingSchedule[foundIndex] = tradingSchedule[foundIndex].AddTradingState(onState);
                }
                else
                {
                    var entryBeforeNew = tradingSchedule.FindEntryActiveAt(startTime);
                    var highActivityOpenTransition = new AvailabilityTransitionTime
                        (startTime, (entryBeforeNew.MarketState & ~(offState)) | onState);
                    tradingSchedule.Add(highActivityOpenTransition);
                }
                foundIndex = tradingSchedule.FindTimeMatchAt(openingTransition.AtTime);
                AvailabilityTransitionTime closeTransition = openingTransition;
                if (foundIndex > 0)
                {
                    var untilClosePrev = openingTransition;
                    for (int j = foundIndex; j < tradingSchedule.Count; j++)
                    {
                        var untilCloseCurrent = tradingSchedule[j];
                        if (untilCloseCurrent.AtTime < startTime)
                        {
                            tradingSchedule[j] = untilCloseCurrent.AddTradingState(offState);
                        }
                        else if (untilCloseCurrent.AtTime > startTime && untilCloseCurrent.AtTime < stopTime &&
                                 !untilClosePrev.IsClosedTransition(untilCloseCurrent))
                        {
                            tradingSchedule[j] = untilCloseCurrent.WithNewState
                                ((untilCloseCurrent.MarketState & ~(offState)) | onState);
                        }
                        if (untilClosePrev.IsClosedTransition(untilCloseCurrent))
                        {
                            closeTransition = untilCloseCurrent;
                            break;
                        }
                        untilClosePrev = untilCloseCurrent;
                    }
                }
                if (stopTime > closeTransition.AtTime)
                {
                    stopTime = closeTransition.AtTime;
                }

                foundIndex = tradingSchedule.FindTimeMatchAt(stopTime);
                if (foundIndex < 0)
                {
                    var entryBeforeNew = tradingSchedule.FindEntryActiveAt(stopTime);
                    var highActivityOpenTransition = new AvailabilityTransitionTime
                        (stopTime, (entryBeforeNew.MarketState & ~(onState)) | offState);
                    tradingSchedule.Add(highActivityOpenTransition);
                }
                foundIndex = tradingSchedule.FindTimeMatchAt(openingTransition.AtTime);
                if (foundIndex > 0)
                {
                    var untilClosePrev = openingTransition;
                    for (int j = foundIndex; j < tradingSchedule.Count; j++)
                    {
                        var untilCloseCurrent = tradingSchedule[j];
                        if (untilCloseCurrent.AtTime > stopTime && !untilClosePrev.IsClosedTransition(untilCloseCurrent))
                        {
                            tradingSchedule[j] = untilCloseCurrent.WithNewState
                                ((untilCloseCurrent.MarketState & ~(onState)) | offState);
                        }
                        if (untilClosePrev.IsClosedTransition(untilCloseCurrent))
                        {
                            break;
                        }
                        untilClosePrev = untilCloseCurrent;
                    }
                }
            }
            prevTransition = currentTransition;
        }
    }

    private void AddOpenCloseTransitions(WeeklyTradingSchedule original, WeeklyTradingSchedule tradingSchedule)
    {
        var prevTransition = original[0];
        for (var i = 0; i < original.Count; i++)
        {
            var currentTransition = original[i];
            if (TradingStartsFromOpenTimeSpan != null)
            {
                if (prevTransition.IsOpenTransition(currentTransition))
                {
                    var timeSpan = TradingStartsFromOpenTimeSpan.Value;
                    if (timeSpan < TimeSpan.Zero)
                    {
                        var preOpenTransition = new AvailabilityTransitionTime
                            (currentTransition.AtTime + timeSpan, PreOpenFlags);
                        tradingSchedule.Add(preOpenTransition);
                        var openTransition = currentTransition.AddTradingState(AllOpenFlags);
                        tradingSchedule.Add(openTransition);
                    }
                    else if (timeSpan > TimeSpan.Zero)
                    {
                        var openTransition = currentTransition.AddTradingState(OpenFlags);
                        tradingSchedule.Add(openTransition);
                        var entryBeforeNew = tradingSchedule.FindEntryActiveAt(currentTransition.AtTime + timeSpan);
                        var postOpenTransition = new AvailabilityTransitionTime
                            (currentTransition.AtTime + timeSpan, entryBeforeNew.MarketState | AllOpenFlags);
                        tradingSchedule.Add(postOpenTransition);
                    }
                    else
                    {
                        var openTransition = currentTransition.AddTradingState(AllOpenFlags);
                        tradingSchedule.Add(openTransition);
                    }
                }
            }
            else if (prevTransition.IsOpenTransition(currentTransition))
            {
                var openTransition = currentTransition.AddTradingState(AllOpenFlags);
                tradingSchedule.Add(openTransition);
            }

            if (AnnounceClosingSoonFromCloseTimeSpan != null)
            {
                if (prevTransition.IsClosedTransition(currentTransition))
                {
                    var timeSpan = AnnounceClosingSoonFromCloseTimeSpan.Value;
                    if (timeSpan > TimeSpan.Zero)
                    {
                        var entryBeforeNew = tradingSchedule.FindEntryActiveAt(currentTransition.AtTime - timeSpan);
                        var preOpenTransition = new AvailabilityTransitionTime
                            (currentTransition.AtTime - timeSpan, IsClosingSoon | entryBeforeNew.MarketState);
                        tradingSchedule.Add(preOpenTransition);
                        var openTransition = currentTransition;
                        tradingSchedule.Add(openTransition);
                    }
                }
            }
            else if (prevTransition.IsClosedTransition(currentTransition))
            {
                tradingSchedule.Add(currentTransition);
            }

            if (!prevTransition.IsOpenTransition(currentTransition) && !prevTransition.IsClosedTransition(currentTransition))
            {
                tradingSchedule.Add(currentTransition);
            }
            prevTransition = tradingSchedule.Any() ? tradingSchedule[^1] : prevTransition;
        }
    }

    public bool AreEquivalent(IDailyTradingScheduleConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var startsFromSame = TradingStartsFromOpenTimeSpan == other.TradingStartsFromOpenTimeSpan;
        var overridePreferredSame = OverridePreferredTradingTimes?.AreEquivalent(other.OverridePreferredTradingTimes, exactTypes) ??
                                    other.OverridePreferredTradingTimes == null;
        var highActivitySame = HighActivityTimes?.AreEquivalent(other.HighActivityTimes, exactTypes) ?? other.HighActivityTimes == null;
        var closingSoonSame  = AnnounceClosingSoonFromCloseTimeSpan == other.AnnounceClosingSoonFromCloseTimeSpan;

        var allAreSame = startsFromSame && overridePreferredSame && highActivitySame && closingSoonSame;

        return allAreSame;
    }
    
    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this, nameof(DailyTradingScheduleConfig))
           .Field.AlwaysAdd(nameof(TradingStartsFromOpenTimeSpan), TradingStartsFromOpenTimeSpan)
           .Field.AlwaysReveal(nameof(OverridePreferredTradingTimes), OverridePreferredTradingTimes)
           .Field.AlwaysReveal(nameof(HighActivityTimes), HighActivityTimes)
           .Field.WhenNonDefaultAdd(nameof(AnnounceClosingSoonFromCloseTimeSpan), AnnounceClosingSoonFromCloseTimeSpan)
           .Complete();

    public override string ToString() =>
        $"{nameof(DailyTradingScheduleConfig)}{{{nameof(TradingStartsFromOpenTimeSpan)}: {TradingStartsFromOpenTimeSpan}, " +
        $"{nameof(OverridePreferredTradingTimes)}: {OverridePreferredTradingTimes}, {nameof(HighActivityTimes)}: {HighActivityTimes}, " +
        $"{nameof(AnnounceClosingSoonFromCloseTimeSpan)}: {AnnounceClosingSoonFromCloseTimeSpan}";
}
