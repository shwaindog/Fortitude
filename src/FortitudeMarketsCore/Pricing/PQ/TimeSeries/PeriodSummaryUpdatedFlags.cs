namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries;

[Flags]
public enum PeriodSummaryUpdatedFlags : ushort
{
    None = 0x0000
    , StartTimeDateFlag = 0x0001
    , StartTimeSubHourFlag = 0x0002
    , EndTimeDateFlag = 0x0004
    , EndTimeSubHourFlag = 0x0008
    , StartBidPriceFlag = 0x0010
    , StartAskPriceFlag = 0x0020
    , HighestBidPriceFlag = 0x0040
    , HighestAskPriceFlag = 0x0080
    , LowestBidPriceFlag = 0x0100
    , LowestAskPriceFlag = 0x0200
    , EndBidPriceFlag = 0x0400
    , EndAskPriceFlag = 0x0800
    , TickCountFlag = 0x1000
    , PeriodVolumeLowerBytesFlag = 0x2000
    , PeriodVolumeUpperBytesFlag = 0x4000
    , AverageMidPriceFlag = 0x8000
}
