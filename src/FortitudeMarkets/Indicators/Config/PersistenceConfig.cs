// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Indicators.Config;

public interface IPersistenceConfig : IStyledToStringObject
{
    bool PersistPrices           { get; set; }
    bool PersistPriceSummaries   { get; set; }
    bool PersistIndicatorState   { get; set; }
    bool PersistIndicatorSignals { get; set; }

    TimeSpan DefaultAutoCloseAfter { get; set; }
}

public class PersistenceConfig : ConfigSection, IPersistenceConfig
{
    public PersistenceConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public PersistenceConfig() { }

    public PersistenceConfig(bool enableAll, int autoCloseAfterSeconds = 5)
    {
        PersistPrices           = enableAll;
        PersistPriceSummaries   = enableAll;
        PersistIndicatorState   = enableAll;
        PersistIndicatorSignals = enableAll;
        DefaultAutoCloseAfter   = TimeSpan.FromSeconds(autoCloseAfterSeconds);
    }

    public PersistenceConfig(IPersistenceConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        PersistPrices           = toClone.PersistPrices;
        PersistPriceSummaries   = toClone.PersistPriceSummaries;
        PersistIndicatorState   = toClone.PersistIndicatorState;
        PersistIndicatorSignals = toClone.PersistIndicatorSignals;
    }

    public bool PersistPrices
    {
        get
        {
            var checkValue = this[nameof(PersistPrices)];
            return checkValue != null && bool.Parse(checkValue);
        }
        set => this[nameof(PersistPrices)] = value.ToString();
    }

    public bool PersistPriceSummaries
    {
        get
        {
            var checkValue = this[nameof(PersistPriceSummaries)];
            return checkValue != null && bool.Parse(checkValue);
        }
        set => this[nameof(PersistPriceSummaries)] = value.ToString();
    }

    public bool PersistIndicatorState
    {
        get
        {
            var checkValue = this[nameof(PersistIndicatorState)];
            return checkValue != null && bool.Parse(checkValue);
        }
        set => this[nameof(PersistIndicatorState)] = value.ToString();
    }

    public bool PersistIndicatorSignals
    {
        get
        {
            var checkValue = this[nameof(PersistIndicatorSignals)];
            return checkValue != null && bool.Parse(checkValue);
        }
        set => this[nameof(PersistIndicatorSignals)] = value.ToString();
    }

    public TimeSpan DefaultAutoCloseAfter
    {
        get
        {
            var checkValue = this[nameof(DefaultAutoCloseAfter)];
            return checkValue != null ? TimeSpan.Parse(checkValue) : TimeSpan.FromSeconds(5);
        }
        set => this[nameof(DefaultAutoCloseAfter)] = value.ToString();
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) => 
        stsa.StartComplexType(this)
            .Field.AlwaysAdd(nameof(PersistPrices), PersistPrices)
            .Field.AlwaysAdd(nameof(PersistPriceSummaries), PersistPriceSummaries)
            .Field.AlwaysAdd(nameof(PersistIndicatorState), PersistIndicatorState)
            .Field.AlwaysAdd(nameof(PersistIndicatorSignals), PersistIndicatorSignals)
            .Field.AlwaysAdd(nameof(DefaultAutoCloseAfter), DefaultAutoCloseAfter)
            .Complete();
}
