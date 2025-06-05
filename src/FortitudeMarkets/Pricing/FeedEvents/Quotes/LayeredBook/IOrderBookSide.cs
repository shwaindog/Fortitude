// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public enum BookSide
{
    Unknown
  , BidBook
  , AskBook
}

public interface IOrderBookSide : IReusableQuoteElement<IOrderBookSide>, ITracksShiftsList<IPriceVolumeLayer, IPriceVolumeLayer>
  , IParentQuoteElement, IChildQuoteElement
{
    new IPriceVolumeLayer this[int index] { get; }

    IReadOnlyList<IPriceVolumeLayer> AllLayers { get; }

    LayerType LayerSupportedType { get; }

    LayerFlags LayerSupportedFlags { get; }

    bool IsLadder { get; }

    uint DailyTickUpdateCount { get; }

    bool HasNonEmptyOpenInterest { get; }

    IMarketAggregate OpenInterestSide { get; }

    BookSide BookSide { get; }

    string EachLayerByIndexOnNewLines();
}

public interface IMutableOrderBookSide : IOrderBookSide, IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>,
    ICloneable<IMutableOrderBookSide>, ITrackableReset<IMutableOrderBookSide>, IMutableChildQuoteElement
{
    new IMutablePriceVolumeLayer this[int index] { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new ushort MaxAllowedSize { get; }

    new bool HasUnreliableListTracking { get; set; }

    new int Count { get; set; }

    new int Capacity { get; set; }

    new LayerFlags LayerSupportedFlags { get; set; }

    new bool HasNonEmptyOpenInterest { get; set; }

    new uint DailyTickUpdateCount { get; set; }

    new IMutableMarketAggregate? OpenInterestSide { get; set; }

    new bool CalculateShift(DateTime asAtTime, IReadOnlyList<IPriceVolumeLayer> updatedCollection);

    new ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd);

    new void ClearShiftCommands();

    new IEnumerator<IMutablePriceVolumeLayer> GetEnumerator();

    new IMutableOrderBookSide ResetWithTracking();

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
            var pvl = orderBookSide[i];
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
            lastIndex = i;
        }

        var publishedVwap = volAccum > 0 ? volPriceAccum / volAccum : 0m;
        return new VwapResult(lastIndex, volume, volAccum, publishedVwap);
    }
}
