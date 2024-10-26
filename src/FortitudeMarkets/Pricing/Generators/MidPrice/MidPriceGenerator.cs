// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeMarkets.Pricing.Generators.MidPrice;

public struct PreviousCurrentMidPriceTime
{
    public PreviousCurrentMidPriceTime(MidPriceTime previousMid, MidPriceTime currentMid)
    {
        PreviousMid = previousMid;
        CurrentMid  = currentMid;
    }

    public MidPriceTime PreviousMid { get; }

    public MidPriceTime CurrentMid { get; }
}

public struct MidPriceTime
{
    public MidPriceTime(decimal mid, decimal delta, DateTime time, int sequenceNumber)
    {
        SequenceNumber = sequenceNumber;
        Mid            = mid;
        Delta          = delta;
        Time           = time;
    }

    public int      SequenceNumber { get; }
    public decimal  Mid            { get; }
    public decimal  Delta          { get; }
    public DateTime Time           { get; }
}

public interface IMidPriceGenerator
{
    DateTime     StartTime            { get; set; }
    decimal      StartPrice           { get; set; }
    int          RoundAtDecimalPlaces { get; set; }
    MidPriceTime PriceAt(DateTime atTime, int sequenceNumber = 0);

    IEnumerable<MidPriceTime> Prices(DateTime startFromTime, TimeSpan incrementTime, int numToGenerate, int startSequenceNumber = 0);

    IEnumerable<MidPriceTime> Prices(DateTime startFromTime, TimeBoundaryPeriod incrementTime, int numToGenerate, int startSequenceNumber = 0);

    IEnumerable<PreviousCurrentMidPriceTime> PreviousCurrentPrices
        (DateTime startFromTime, TimeSpan incrementTime, int numToGenerate, int startSequenceNumber = 0);

    IEnumerable<PreviousCurrentMidPriceTime> PreviousCurrentPrices
        (DateTime startFromTime, TimeBoundaryPeriod incrementTime, int numToGenerate, int startSequenceNumber = 0);
}

public abstract class MidPriceGenerator : IMidPriceGenerator
{
    public virtual int      RoundAtDecimalPlaces { get; set; }
    public         DateTime StartTime            { get; set; }

    public decimal StartPrice { get; set; }

    public abstract MidPriceTime PriceAt(DateTime atTime, int sequenceNumber = 0);

    public IEnumerable<MidPriceTime> Prices(DateTime startFromTime, TimeSpan incrementTime, int numToGenerate, int startSequenceNumber = 0)
    {
        var totalTimeSpanTicks = incrementTime.Ticks * numToGenerate;
        var endTime            = startFromTime.Add(TimeSpan.FromTicks(totalTimeSpanTicks));
        var sequenceNumber     = startSequenceNumber;
        for (var currentTime = startFromTime; currentTime < endTime; currentTime = currentTime.Add(incrementTime))
            yield return PriceAt(currentTime, sequenceNumber++);
    }

    public IEnumerable<MidPriceTime> Prices(DateTime startFromTime, TimeBoundaryPeriod incrementTime, int numToGenerate, int startSequenceNumber = 0)
    {
        var endTime = startFromTime;

        var count = numToGenerate;

        while (count-- > 0) endTime = incrementTime.PeriodEnd(endTime);

        var sequenceNumber = startSequenceNumber;
        for (var currentTime = startFromTime; currentTime < endTime; currentTime = incrementTime.PeriodEnd(currentTime))
            yield return PriceAt(currentTime, sequenceNumber++);
    }

    public IEnumerable<PreviousCurrentMidPriceTime> PreviousCurrentPrices
        (DateTime startFromTime, TimeSpan incrementTime, int numToGenerate, int startSequenceNumber = 0)
    {
        var previousStream = Prices(startFromTime, incrementTime, numToGenerate + 1, startSequenceNumber);
        var currentStream  = Prices(startFromTime + incrementTime, incrementTime, numToGenerate, startSequenceNumber + 1);
        return previousStream.Zip(currentStream, (previous, current) => new PreviousCurrentMidPriceTime(previous, current));
    }

    public IEnumerable<PreviousCurrentMidPriceTime> PreviousCurrentPrices
        (DateTime startFromTime, TimeBoundaryPeriod incrementTime, int numToGenerate, int startSequenceNumber = 0)
    {
        var previousStream = Prices(startFromTime, incrementTime, numToGenerate + 1, startSequenceNumber);
        var currentStream  = Prices(incrementTime.PeriodEnd(startFromTime), incrementTime, numToGenerate, startSequenceNumber + 1);
        return previousStream.Zip(currentStream, (previous, current) => new PreviousCurrentMidPriceTime(previous, current));
    }
}
