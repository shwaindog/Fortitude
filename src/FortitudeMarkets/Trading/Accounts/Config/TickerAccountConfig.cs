// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface ITickerAccountConfig : IRelativeTickerTradingLimitsConfig, ICloneable<ITickerAccountConfig>
{
    ushort InstrumentId { get; set; }

    string? InstrumentName { get; set; }

    PositionsDirectionFlags AllowedTradingDirection { get; set; }

    decimal? DefaultAutoStopLossPips { get; set; }

    bool AlwaysSubmitWithStopLoss { get; set; }

    decimal? AutoTrailingAdjustStopLossEveryPips { get; set; }

    ITickerThroughputLimitsConfig? TickerThroughputLimits { get; set; }

    new ITickerAccountConfig Clone();
}

public class TickerAccountConfig : RelativeTickerTradingLimitsConfig, ITickerAccountConfig
{
    public TickerAccountConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public TickerAccountConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public TickerAccountConfig
    (ushort instrumentId, PositionsDirectionFlags allowedTradingDirection, string? instrumentName = null, decimal? defaultAutoStopLoss = null
      , bool alwaysSubmitWithStopLoss = false, decimal? autoTrailingAdjustStopLossEveryPips = null
      , ITickerThroughputLimitsConfig? overrideThroughputLimits = null
      , decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : base(overrideTradingAccountSize, adjustTradingAccountSizePercentage
             , maxTickerOpenPositionPercentage, maxFixedTickerOpenPosition, maxTickerOpeningOrderSizePercentage, maxFixedTickerOpeningOrderSize
             , maxTickerReducingOrderSizePercentage, maxFixedTickerReducingOrderSize)
    {
        InstrumentId                        = instrumentId;
        InstrumentName                      = instrumentName;
        AllowedTradingDirection             = allowedTradingDirection;
        DefaultAutoStopLossPips             = defaultAutoStopLoss;
        AlwaysSubmitWithStopLoss            = alwaysSubmitWithStopLoss;
        AutoTrailingAdjustStopLossEveryPips = autoTrailingAdjustStopLossEveryPips;
        TickerThroughputLimits            = overrideThroughputLimits;
    }

    public TickerAccountConfig(ITickerAccountConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        InstrumentId                        = toClone.InstrumentId;
        InstrumentName                      = toClone.InstrumentName;
        AllowedTradingDirection             = toClone.AllowedTradingDirection;
        DefaultAutoStopLossPips             = toClone.DefaultAutoStopLossPips;
        AlwaysSubmitWithStopLoss            = toClone.AlwaysSubmitWithStopLoss;
        AutoTrailingAdjustStopLossEveryPips = toClone.AutoTrailingAdjustStopLossEveryPips;
        TickerThroughputLimits            = toClone.TickerThroughputLimits;
    }

    public TickerAccountConfig(ITickerAccountConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort InstrumentId
    {
        get => ushort.TryParse(this[nameof(InstrumentId)], out var id) ? id : (ushort)0;
        set => this[nameof(InstrumentId)] = value.ToString();
    }

    public string? InstrumentName
    {
        get => this[nameof(AdjustTradingAccountSizePercentage)];
        set => this[nameof(AdjustTradingAccountSizePercentage)] = value;
    }

    public PositionsDirectionFlags AllowedTradingDirection
    {
        get =>
            Enum.TryParse<PositionsDirectionFlags>(this[nameof(AllowedTradingDirection)], out var allowedTradingDirection)
                ? allowedTradingDirection
                : PositionsDirectionFlags.Default;
        set => this[nameof(AllowedTradingDirection)] = value.ToString();
    }

    public decimal? DefaultAutoStopLossPips
    {
        get => decimal.TryParse(this[nameof(DefaultAutoStopLossPips)], out var defaultAutoStopLossPips) ? defaultAutoStopLossPips : 0m;
        set => this[nameof(DefaultAutoStopLossPips)] = value.ToString();
    }

    public bool AlwaysSubmitWithStopLoss
    {
        get => bool.TryParse(this[nameof(AlwaysSubmitWithStopLoss)], out var alwaysSubmitWithStopLoss) && alwaysSubmitWithStopLoss;
        set => this[nameof(AlwaysSubmitWithStopLoss)] = value.ToString();
    }

    public decimal? AutoTrailingAdjustStopLossEveryPips
    {
        get => decimal.TryParse(this[nameof(AutoTrailingAdjustStopLossEveryPips)], out var accountSize) ? accountSize : null;
        set => this[nameof(AutoTrailingAdjustStopLossEveryPips)] = value.ToString();
    }

    public ITickerThroughputLimitsConfig? TickerThroughputLimits
    {
        get
        {
            if (GetSection(nameof(TickerThroughputLimits)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new TickerThroughputLimitsConfig(ConfigRoot, $"{Path}{Split}{nameof(TickerThroughputLimits)}");
            }
            return null;
        }
        set => _ = value != null ? new TickerThroughputLimitsConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TickerThroughputLimits)}") : null;
    }

    ITickerAccountConfig ICloneable<ITickerAccountConfig>.Clone() => Clone();

    ITickerAccountConfig ITickerAccountConfig.Clone() => Clone();

    public override TickerAccountConfig Clone() => new(this);

    public override bool AreEquivalent(IFixedTickerTradingLimitsConfig? other, bool exactTypes = false)
    {
        if (other is not ITickerAccountConfig tickerAccountConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var instrumentIdSame             = InstrumentId == tickerAccountConfig.InstrumentId;
        var instrumentNameSame           = InstrumentName == tickerAccountConfig.InstrumentName;
        var tradingDirSame               = AllowedTradingDirection == tickerAccountConfig.AllowedTradingDirection;
        var defaultAutoSLPipsSame        = DefaultAutoStopLossPips == tickerAccountConfig.DefaultAutoStopLossPips;
        var alwaysSubmitWithSLSame       = AlwaysSubmitWithStopLoss == tickerAccountConfig.AlwaysSubmitWithStopLoss;
        var autoTrailingSLEveryPipsSame  = AutoTrailingAdjustStopLossEveryPips == tickerAccountConfig.AutoTrailingAdjustStopLossEveryPips;
        var overrideThroughputLimitsSame = TickerThroughputLimits?.AreEquivalent(tickerAccountConfig.TickerThroughputLimits) 
                                        ?? tickerAccountConfig.TickerThroughputLimits == null;

        var allAreSame = baseSame && instrumentIdSame && instrumentNameSame && tradingDirSame && defaultAutoSLPipsSame
                      && alwaysSubmitWithSLSame && autoTrailingSLEveryPipsSame && overrideThroughputLimitsSame;

        return allAreSame;
    }


    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(InstrumentId)}"]                        = null;
        root[$"{path}{Split}{nameof(InstrumentName)}"]                      = null;
        root[$"{path}{Split}{nameof(AllowedTradingDirection)}"]             = null;
        root[$"{path}{Split}{nameof(DefaultAutoStopLossPips)}"]             = null;
        root[$"{path}{Split}{nameof(AlwaysSubmitWithStopLoss)}"]            = null;
        root[$"{path}{Split}{nameof(AutoTrailingAdjustStopLossEveryPips)}"] = null;
        root[$"{path}{Split}{nameof(TickerThroughputLimits)}"]            = null;
        AccountTradingLimitsConfig.ClearValues(root, path);
    }

    protected bool Equals(ITickerAccountConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickerAccountConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ InstrumentId.GetHashCode();
            hashCode = (hashCode * 397) ^ (InstrumentName?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ AllowedTradingDirection.GetHashCode();
            hashCode = (hashCode * 397) ^ (DefaultAutoStopLossPips?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ AlwaysSubmitWithStopLoss.GetHashCode();
            hashCode = (hashCode * 397) ^ (AutoTrailingAdjustStopLossEveryPips?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (TickerThroughputLimits?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public string RelativeAccountSizingToStringMembers =>
        $"{nameof(InstrumentId)}: {InstrumentId}, {nameof(InstrumentName)}: {InstrumentName}, " +
        $"{nameof(AllowedTradingDirection)}: {AllowedTradingDirection}, {nameof(DefaultAutoStopLossPips)}: {DefaultAutoStopLossPips}, " +
        $"{nameof(AlwaysSubmitWithStopLoss)}: {AlwaysSubmitWithStopLoss}, {nameof(AutoTrailingAdjustStopLossEveryPips)}: {AutoTrailingAdjustStopLossEveryPips}, " +
        $"{nameof(TickerThroughputLimits)}: {TickerThroughputLimits}, {RelativeTickerLimitsToStringMembers}";

    public override string ToString() => $"{nameof(TickerAccountConfig)}{{{RelativeAccountSizingToStringMembers}}}";
}
