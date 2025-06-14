// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Configuration;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Config.Availability;

public interface ITradingTimeTableConfig : IInterfacesComparable<ITradingTimeTableConfig>
{
    IDailyTradingScheduleConfig TradingScheduleConfig { get; set; }

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

    public IDailyTradingScheduleConfig TradingScheduleConfig 
    {
        get => new DailyTradingScheduleConfig(ConfigRoot, Path + ":" + nameof(TradingScheduleConfig))
        {
            ParentTimeZone = VenueOperatingTimeTable?.OperatingTimeZone
        };
        set => _ = new DailyTradingScheduleConfig(value, ConfigRoot, Path + ":" + nameof(TradingScheduleConfig));
    }

    public ITimeTableConfig? HighLiquidityTimeTable
    {
        get
        {
            if (GetSection(nameof(HighLiquidityTimeTable)).GetChildren().Any(cs => StringExtensions.IsNotNullOrEmpty(cs.Value)))
            {
                var timeTableConfig = new TimeTableConfig(ConfigRoot, Path + ":" + nameof(HighLiquidityTimeTable));

                return timeTableConfig;
            }
            return ParentHighLiquidityTimeTable;
        }
        set => _ = value != null ? new TimeTableConfig(value, ConfigRoot, Path + ":" + nameof(HighLiquidityTimeTable)) : null;
    }

    public ITimeTableConfig? ParentHighLiquidityTimeTable { get; set; }

    public ITimeTableConfig? VenueOperatingTimeTable { get; set; }

    public bool AreEquivalent(ITradingTimeTableConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var tradingScheduleSame = TradingScheduleConfig.AreEquivalent(other.TradingScheduleConfig, exactTypes);
        var highLiquidSame = HighLiquidityTimeTable?.AreEquivalent(other.HighLiquidityTimeTable, exactTypes) ?? other.HighLiquidityTimeTable == null;

        var allAreSame = tradingScheduleSame && highLiquidSame;

        return allAreSame;
    }

    public override string ToString() => 
        $"{nameof(TradingTimeTableConfig)}{{{nameof(TradingScheduleConfig)}: {TradingScheduleConfig}, {nameof(HighLiquidityTimeTable)}: {HighLiquidityTimeTable}, " +
        $"{nameof(ParentHighLiquidityTimeTable)}: {ParentHighLiquidityTimeTable}, {nameof(VenueOperatingTimeTable)}: {VenueOperatingTimeTable}}}";
}
