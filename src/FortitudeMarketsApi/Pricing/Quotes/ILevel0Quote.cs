// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ILevel0Quote : IReusableObject<ILevel0Quote>,
    IInterfacesComparable<ILevel0Quote>,
    ITimeSeriesEntry<ILevel0Quote>
{
    QuoteLevel              QuoteLevel            { get; }
    bool                    IsReplay              { get; }
    DateTime                SourceTime            { get; }
    DateTime                ClientReceivedTime    { get; }
    ISourceTickerQuoteInfo? SourceTickerQuoteInfo { get; }
    decimal                 SinglePrice           { get; }
    new ILevel0Quote        CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public struct Level0QuoteStruct : ITimeSeriesEntry<Level0QuoteStruct>
{
    public Level0QuoteStruct(DateTime sourceTime, decimal singlePrice, DateTime clientReceivedTime, bool isReplay = false)
    {
        IsReplay           = isReplay;
        SourceTime         = sourceTime;
        ClientReceivedTime = clientReceivedTime;
        SinglePrice        = singlePrice;
    }

    public bool     IsReplay;
    public DateTime SourceTime;
    public DateTime ClientReceivedTime;
    public decimal  SinglePrice;
    public DateTime StorageTime(IStorageTimeResolver<Level0QuoteStruct>? resolver = null) => SourceTime;
}
