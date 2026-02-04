// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Serialization;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Config.Availability;

public interface ITradingTimeTableConfig : IInterfacesComparable<ITradingTimeTableConfig>, IWeeklyAvailability, IStringBearer
{
    IDailyTradingScheduleConfig? TradingScheduleConfig { get; set; }

    ITimeTableConfig? HighLiquidityTimeTable { get; set; }
}

public class TradingTimeTableConfig : ConfigSection, ITradingTimeTableConfig
{
    public TradingTimeTableConfig()
    {
    }

    public TradingTimeTableConfig(IConfigurationRoot root, string path) : base(root, path)
    {
    }

    public TradingTimeTableConfig
        (IDailyTradingScheduleConfig tradingScheduleConfig, ITimeTableConfig? highLiquidityTimeTable = null)
        : this(tradingScheduleConfig, InMemoryConfigRoot, InMemoryPath, highLiquidityTimeTable) { }

    public TradingTimeTableConfig
    (IDailyTradingScheduleConfig tradingScheduleConfig, IConfigurationRoot root, string path
      , ITimeTableConfig? highLiquidityTimeTable = null) : base(root, path)
    {
        TradingScheduleConfig  = tradingScheduleConfig;
        if (highLiquidityTimeTable != null)
        {
            HighLiquidityTimeTable = highLiquidityTimeTable;
        }
    }

    public TradingTimeTableConfig(ITradingTimeTableConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        TradingScheduleConfig  = toClone.TradingScheduleConfig;
        HighLiquidityTimeTable = toClone.HighLiquidityTimeTable;
    }

    public TradingTimeTableConfig(ITradingTimeTableConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IDailyTradingScheduleConfig? TradingScheduleConfig
    {
        get
        {
            if (GetSection(nameof(TradingScheduleConfig)).GetChildren().Any(cs => StringExtensions.IsNotNullOrEmpty(cs.Value)))
            {
                return new DailyTradingScheduleConfig(ConfigRoot, $"{Path}{Split}{nameof(TradingScheduleConfig)}")
                {
                    ParentTimeZone = VenueOperatingTimeTable?.OperatingTimeZone
                };
            }
            return ParentTradingTimeTableConfig?.TradingScheduleConfig;
        }
        set => _ = value != null ? new DailyTradingScheduleConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TradingScheduleConfig)}") : null;
    }

    public ITimeTableConfig? HighLiquidityTimeTable
    {
        get
        {
            if (GetSection(nameof(HighLiquidityTimeTable)).GetChildren().Any(cs => StringExtensions.IsNotNullOrEmpty(cs.Value)))
            {
                var timeTableConfig = new TimeTableConfig(ConfigRoot, $"{Path}{Split}{nameof(HighLiquidityTimeTable)}");

                return timeTableConfig;
            }
            return ParentTradingTimeTableConfig?.HighLiquidityTimeTable;
        }
        set => _ = value != null ? new TimeTableConfig(value, ConfigRoot, $"{Path}{Split}{nameof(HighLiquidityTimeTable)}") : null;
    }

    public ITradingTimeTableConfig? ParentTradingTimeTableConfig { get; set; }

    [JsonIgnore]
    public ITimeTableConfig? VenueOperatingTimeTable { get; set; }

    public WeeklyTradingSchedule WeeklySchedule(DateTimeOffset forTimeInWeek)
    {
        var schedule = VenueOperatingTimeTable!.WeeklySchedule(forTimeInWeek);
        if (TradingScheduleConfig != null)
        {
            schedule = TradingScheduleConfig.WeeklySchedule(forTimeInWeek, schedule);
        }
        if (HighLiquidityTimeTable != null)
        {
            HighLiquidityTimeTable.AddWeeklyOnOffTradingState
                (schedule, TradingPeriodTypeFlags.IsMainTradingPeriod, TradingPeriodTypeFlags.IsGreyMarketTradingPeriod);
        }

        return schedule;
    }

    public bool AreEquivalent(ITradingTimeTableConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var tradingScheduleSame = TradingScheduleConfig?.AreEquivalent(other.TradingScheduleConfig, exactTypes) ?? other.TradingScheduleConfig == null;
        var highLiquidSame = HighLiquidityTimeTable?.AreEquivalent(other.HighLiquidityTimeTable, exactTypes) ?? other.HighLiquidityTimeTable == null;

        var allAreSame = tradingScheduleSame && highLiquidSame;

        return allAreSame;
    }
    
    public virtual AppendSummary RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
            .Field.AlwaysReveal(nameof(TradingScheduleConfig), TradingScheduleConfig)
            .Field.AlwaysReveal(nameof(HighLiquidityTimeTable), HighLiquidityTimeTable)
            .Field.AlwaysReveal(nameof(ParentTradingTimeTableConfig), ParentTradingTimeTableConfig)
            .Field.AlwaysReveal(nameof(VenueOperatingTimeTable), VenueOperatingTimeTable)
            .Complete();

    public override string ToString() => 
        $"{nameof(TradingTimeTableConfig)}{{{nameof(TradingScheduleConfig)}: {TradingScheduleConfig}, {nameof(HighLiquidityTimeTable)}: {HighLiquidityTimeTable}, " +
        $"{nameof(ParentTradingTimeTableConfig)}: {ParentTradingTimeTableConfig}, {nameof(VenueOperatingTimeTable)}: {VenueOperatingTimeTable}}}";
}
