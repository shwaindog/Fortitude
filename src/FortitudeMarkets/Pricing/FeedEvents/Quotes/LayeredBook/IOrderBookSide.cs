// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public enum BookSide
{
    Unknown
  , BidBook
  , AskBook
}

public interface IOrderBookSide : ICapacityList<IPriceVolumeLayer>, IReusableObject<IOrderBookSide>,
    IInterfacesComparable<IOrderBookSide>
{
    LayerType  LayerSupportedType             { get; }
    LayerFlags LayerSupportedFlags { get; }

    bool IsLadder { get; }
    ushort MaxPublishDepth { get; }

    uint DailyTickUpdateCount { get; }
    
    bool      HasNonEmptyOpenInterest { get; }
    IMarketAggregate OpenInterestSide               { get; }

    BookSide BookSide { get; }
}

public interface IMutableOrderBookSide : IOrderBookSide, IMutableCapacityList<IMutablePriceVolumeLayer>,
    ICloneable<IMutableOrderBookSide>, ITrackableReset<IMutableOrderBookSide>
{
    new int Count { get; set; }

    new int Capacity { get; set; }
    
    new LayerFlags            LayerSupportedFlags { get; set; }
    
    new bool HasNonEmptyOpenInterest { get; set; }

    new uint  DailyTickUpdateCount    { get; set; }

    new IMutableMarketAggregate? OpenInterestSide        { get; set; }

    new IEnumerator<IMutablePriceVolumeLayer> GetEnumerator(); 

    new IMutablePriceVolumeLayer this[int level] { get; set; }

    new IMutableOrderBookSide Clone();

    int AppendEntryAtEnd();
}

public static class OrderBookSideExtensions
{
    public static VwapResult CalculateVwap(this IOrderBookSide orderBookSide, decimal volume = decimal.MaxValue, decimal ignoreBestVolumeAmount = 0)
    {
        var remainingSkipVol = ignoreBestVolumeAmount;
        var remainingVol     = volume;
        var volAccum         = 0m;
        var volPriceAccum    = 0m;
        var lastIndex        = 0;
        
        var layerDeductVolume = 0m;
        for (int i = 0; i < orderBookSide.Count && remainingVol > 0m; i++)
        {
            var pvl         = orderBookSide[i];
            if (pvl == null) continue;
            decimal cappedVolume;
            if (remainingSkipVol > 0)
            {
                cappedVolume     =  Math.Min(remainingSkipVol, pvl.Volume);
                remainingSkipVol -= cappedVolume;
                if (cappedVolume < pvl.Volume)
                {
                    layerDeductVolume = pvl.Volume - cappedVolume;
                    i--;
                    continue;
                }
            }
            else
            {
                cappedVolume  =  Math.Min(remainingVol, pvl.Volume - layerDeductVolume);
                remainingVol  -= pvl.Volume;
                volAccum      += cappedVolume;
                volPriceAccum += cappedVolume * pvl.Price;
            }
            lastIndex     =  i;
        }

        var publishedVwap    = volAccum > 0 ? volPriceAccum / volAccum : 0m;
        return new VwapResult(lastIndex, volume,  volAccum, publishedVwap);
    }
}
