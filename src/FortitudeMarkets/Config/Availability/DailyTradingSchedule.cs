using FortitudeCommon.Configuration;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Config.Availability;

public interface IDailyTradingScheduleConfig : IInterfacesComparable<IDailyTradingScheduleConfig>
{
    //  if negative then accepting orders before open.
    //  if positive until reached is non-preffered, low liquidity, then normal
    public TimeSpan? TradingStartsFromOpenTimeSpan { get; set; } 

    //  optional usually set to main trading timezone until reached normal liquidity
    //  and neither non/preferred
    public ITimeZoneStartStopTimeConfig? OverridePreferredTradingTimes { get; set; } 
    public ITimeZoneStartStopTimeConfig? HighActivityTimes { get; set; } //  until reached neither just normal
    public TimeSpan? AnnounceClosingSoonFromCloseTimeSpan { get; set; } // positive before close 
}

public class DailyTradingScheduleConfig : ConfigSection, IDailyTradingScheduleConfig
{
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
                var pricingServerConfig = new TimeZoneStartStopTimeConfig(ConfigRoot, Path + ":" + nameof(OverridePreferredTradingTimes))
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
                ? new TimeZoneStartStopTimeConfig(value, ConfigRoot, Path + ":" + nameof(OverridePreferredTradingTimes))
                : null;
        }
    }

    public ITimeZoneStartStopTimeConfig? HighActivityTimes
    {
        get
        {
            if (GetSection(nameof(HighActivityTimes)).GetChildren().Any(cs => StringExtensions.IsNotNullOrEmpty(cs.Value)))
            {
                var pricingServerConfig = new TimeZoneStartStopTimeConfig(ConfigRoot, Path + ":" + nameof(HighActivityTimes))
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
                ? new TimeZoneStartStopTimeConfig(value, ConfigRoot, Path + ":" + nameof(HighActivityTimes))
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

    public bool AreEquivalent(IDailyTradingScheduleConfig? other, bool exactTypes = false)
    {
        if(other == null) return false;
        var startsFromSame = TradingStartsFromOpenTimeSpan == other.TradingStartsFromOpenTimeSpan;
        var overridePreferredSame = OverridePreferredTradingTimes?.AreEquivalent(other.OverridePreferredTradingTimes, exactTypes) ?? other.OverridePreferredTradingTimes == null;
        var highActivitySame = HighActivityTimes?.AreEquivalent(other.HighActivityTimes, exactTypes) ?? other.HighActivityTimes == null;
        var closingSoonSame = AnnounceClosingSoonFromCloseTimeSpan == other.AnnounceClosingSoonFromCloseTimeSpan;

        var allAreSame = startsFromSame && overridePreferredSame && highActivitySame && closingSoonSame;

        return allAreSame;
    }

    public override string ToString() => 
        $"{nameof(DailyTradingScheduleConfig)}{{{nameof(TradingStartsFromOpenTimeSpan)}: {TradingStartsFromOpenTimeSpan}, " +
        $"{nameof(OverridePreferredTradingTimes)}: {OverridePreferredTradingTimes}, {nameof(HighActivityTimes)}: {HighActivityTimes}, " +
        $"{nameof(AnnounceClosingSoonFromCloseTimeSpan)}: {AnnounceClosingSoonFromCloseTimeSpan}";
}
