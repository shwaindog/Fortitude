﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Storage.TimeSeries.FileSystem.Config;
using FortitudeMarkets.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Indicators.Config;

public interface IIndicatorServicesConfig : IStringBearer
{
    IMarketsConfig?        MarketsConfig                  { get; set; }
    IFileRepositoryConfig? TimeSeriesFileRepositoryConfig { get; set; }


    TimeSpan DefaultCachePricesTimeSpan               { get; set; }
    TimeSpan DefaultCacheCandlesTimeSpan { get; set; }

    IPersistenceConfig PersistenceConfig { get; set; }
}

public class IndicatorServicesConfig : ConfigSection, IIndicatorServicesConfig
{
    private object? ignoreSuppressWarnings;
    public IndicatorServicesConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public IndicatorServicesConfig() { }

    public IndicatorServicesConfig(IMarketsConfig marketsConfig) => MarketsConfig = marketsConfig;

    public IndicatorServicesConfig(IMarketsConfig marketsConfig, IFileRepositoryConfig repositoryConfig, IPersistenceConfig persistenceConfig)
    {
        MarketsConfig     = marketsConfig;
        PersistenceConfig = persistenceConfig;

        TimeSeriesFileRepositoryConfig = repositoryConfig;
    }

    public IMarketsConfig? MarketsConfig
    {
        get
        {
            if (GetSection(nameof(MarketsConfig)).GetChildren().Any()) return new MarketsConfig(ConfigRoot, $"{Path}{Split}{nameof(MarketsConfig)}");
            return null;
        }
        set => ignoreSuppressWarnings = value != null ? new MarketsConfig(value, ConfigRoot, $"{Path}{Split}{nameof(MarketsConfig)}") : null;
    }

    public IFileRepositoryConfig? TimeSeriesFileRepositoryConfig
    {
        get
        {
            if (GetSection(nameof(TimeSeriesFileRepositoryConfig)).GetChildren().Any())
                return new FileRepositoryConfig(ConfigRoot, $"{Path}{Split}{nameof(TimeSeriesFileRepositoryConfig)}");
            return null;
        }
        set =>
            ignoreSuppressWarnings
                = value != null ? new FileRepositoryConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TimeSeriesFileRepositoryConfig)}") : null;
    }

    public TimeSpan DefaultCachePricesTimeSpan
    {
        get
        {
            var checkValue = this[nameof(DefaultCachePricesTimeSpan)];
            return checkValue != null ? TimeSpan.Parse(checkValue) : TimeSpan.FromHours(1);
        }
        set => this[nameof(DefaultCachePricesTimeSpan)] = value.ToString();
    }

    public TimeSpan DefaultCacheCandlesTimeSpan
    {
        get
        {
            var checkValue = this[nameof(DefaultCacheCandlesTimeSpan)];
            return checkValue != null ? TimeSpan.Parse(checkValue) : TimeSpan.FromHours(1);
        }
        set => this[nameof(DefaultCacheCandlesTimeSpan)] = value.ToString();
    }

    public IPersistenceConfig PersistenceConfig
    {
        get => new PersistenceConfig(ConfigRoot, $"{Path}{Split}{nameof(PersistenceConfig)}");
        set => ignoreSuppressWarnings = new PersistenceConfig(value, ConfigRoot, $"{Path}{Split}{nameof(PersistenceConfig)}");
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
            .Field.AlwaysReveal(nameof(MarketsConfig), MarketsConfig)
            .Field.AlwaysReveal(nameof(TimeSeriesFileRepositoryConfig), TimeSeriesFileRepositoryConfig)
            .Field.AlwaysAdd(nameof(DefaultCachePricesTimeSpan), DefaultCachePricesTimeSpan)
            .Field.AlwaysAdd(nameof(DefaultCacheCandlesTimeSpan), DefaultCacheCandlesTimeSpan)
            .Field.AlwaysReveal(nameof(PersistenceConfig), PersistenceConfig)
            .Complete();
}
