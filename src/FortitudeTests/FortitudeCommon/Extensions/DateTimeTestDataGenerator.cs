namespace FortitudeTests.FortitudeCommon.Extensions;

public static class DateTimeTestDataGenerator
{
    private static readonly DateTime DefaultFromDateTime   = new DateTime(1900, 1, 1); 
    private static readonly DateTime DefaultToDateTime     = new DateTime(2100, 1, 1); 
    private static readonly TimeSpan DefaultIncrementTime  = TimeSpan.FromMilliseconds(1);
    private static readonly TimeSpan DefaultFromTimeSpan   = TimeSpan.FromHours(-7); 
    private static readonly TimeSpan DefaultToTimeTimeSpan = TimeSpan.FromDays(7); 

    private static Random random = new Random();

    public static void ReseedRandom(int seed)
    {
        random = new Random(seed);
    }

    public static IEnumerable<DateTime> GenRandomDateTimeRange(int numberToGenerate
      , DateTime? fromDate = null, DateTime? toDate = null, TimeSpan? incrementBy = null, double chanceOfMinDateTime = 0.1d)
    {
        foreach (var nullableDt in GenerateNextSequence(numberToGenerate, fromDate, toDate, incrementBy,  0.0d, chanceOfMinDateTime))
        {
            yield return nullableDt!.Value;
        }
    }

    public static IEnumerable<DateTime?> GenRandomNullableDateTimeRange(int numberToGenerate
      , DateTime? fromDate = null, DateTime? toDate = null, TimeSpan? incrementBy = null, double chanceOfNull = 0.5d 
      , double chanceOfMinDateTime = 0.10)
    {
        foreach (var nullableDt in GenerateNextSequence(numberToGenerate, fromDate, toDate, incrementBy,  chanceOfNull , chanceOfMinDateTime))
        {
            yield return nullableDt;
        }
    }

    public static IEnumerable<DateOnly> GenRandomDateOnlyRange(int numberToGenerate
      , DateOnly? fromDate = null, DateOnly? toDate = null, uint incrementDays = 1, double chanceOfMinDateTime = 0.1d)
    {
        incrementDays = Math.Max(1, incrementDays);
        var from = fromDate ?? DateOnly.FromDateTime(DefaultFromDateTime); 
        var to = fromDate ?? DateOnly.FromDateTime(DefaultToDateTime); 
        foreach (var nullableDo in GenerateNextSequence(numberToGenerate, from.ToDateTime(TimeOnly.MinValue)
                                                       , to.ToDateTime(TimeOnly.MinValue)
                                                       , TimeSpan.FromDays(incrementDays),  0.0d, chanceOfMinDateTime))
        {
            yield return DateOnly.FromDateTime(nullableDo!.Value);
        }
    }

    public static IEnumerable<DateOnly?> GenRandomNullableDateOnlyRange(int numberToGenerate
      , DateOnly? fromDate = null, DateOnly? toDate = null, uint incrementDays = 1, double chanceOfNull = 0.5d 
      , double chanceOfMinDateTime = 0.1d)
    {
        incrementDays = Math.Max(1, incrementDays);
        var from = fromDate ?? DateOnly.FromDateTime(DefaultFromDateTime); 
        var to   = toDate ?? DateOnly.FromDateTime(DefaultToDateTime); 
        foreach (var nullableDo in GenerateNextSequence(numberToGenerate, from.ToDateTime(TimeOnly.MinValue)
                                                       , to.ToDateTime(TimeOnly.MinValue)
                                                       , TimeSpan.FromDays(incrementDays),  chanceOfNull, chanceOfMinDateTime))
        {
            if(nullableDo == null)
            {
                yield return null;
            }
            else
            {
                yield return DateOnly.FromDateTime(nullableDo!.Value);
            }
        }
    }

    public static IEnumerable<TimeSpan> GenRandomTimeSpanRange(int numberToGenerate
      , TimeSpan? fromTime = null, TimeSpan? toTime = null, TimeSpan? incrementBy = null, double chanceOfMinDateTime = 0.1d)
    {
        foreach (var nullableTo in GenerateNextNullableTimeSpanSequence(numberToGenerate, fromTime
                                                                       , toTime
                                                                       , incrementBy,  0.0d, chanceOfMinDateTime))
        {
            yield return nullableTo!.Value;
        }
    }

    public static IEnumerable<TimeSpan?> GenRandomNullableTimeSpanRange(int numberToGenerate
      , TimeSpan? fromTime = null, TimeSpan? toTime = null, TimeSpan? incrementBy = null,  double chanceOfNull = 0.5d 
      , double chanceOfMinDateTime = 0.1d)
    {
        foreach (var nullableTo in GenerateNextNullableTimeSpanSequence(numberToGenerate, fromTime
                                                                       , toTime
                                                                       , incrementBy,  0.0d, chanceOfMinDateTime))
        {
            yield return nullableTo;
        }
    }

    public static IEnumerable<TimeOnly> GenRandomTimeOnlyRange(int numberToGenerate
      , TimeOnly? fromTime = null, TimeOnly? toTime = null, TimeSpan? incrementBy = null, double chanceOfMinDateTime = 0.1d)
    {
        var from = TimeSpan.FromTicks((fromTime ?? TimeOnly.MinValue).Ticks); 
        var to   = TimeSpan.FromTicks((toTime ?? TimeOnly.MaxValue).Ticks);
        var increment = incrementBy ?? DefaultIncrementTime;
        if (increment.Ticks > TimeSpan.FromDays(1).Ticks)
        {
            increment = DefaultIncrementTime;
        }
        foreach (var nullableTo in GenerateNextNullableTimeSpanSequence(numberToGenerate, from
                                                                       , to
                                                                       , increment,  0.0d, chanceOfMinDateTime))
        {
            yield return TimeOnly.FromTimeSpan(nullableTo!.Value);
        }
    }

    public static IEnumerable<TimeOnly?> GenRandomNullableTimeOnlyRange(int numberToGenerate
      , TimeOnly? fromTime = null, TimeOnly? toTime = null, TimeSpan? incrementBy = null,  double chanceOfNull = 0.5 
      , double chanceOfMinDateTime = 0.1d)
    {
        var from      = TimeSpan.FromTicks((fromTime ?? TimeOnly.MinValue).Ticks); 
        var to        = TimeSpan.FromTicks((toTime ?? TimeOnly.MaxValue).Ticks);
        var increment = incrementBy ?? DefaultIncrementTime;
        if (increment.Ticks > TimeSpan.FromDays(1).Ticks)
        {
            increment = DefaultIncrementTime;
        }
        foreach (var nullableTo in GenerateNextNullableTimeSpanSequence(numberToGenerate, from
                                                                       , to
                                                                       , increment,  chanceOfNull, chanceOfMinDateTime))
        {
            if(nullableTo == null)
            {
                yield return null;
            }
            else
            {
                yield return TimeOnly.FromTimeSpan(nullableTo!.Value);
            }
        }
    }

    public static IEnumerable<DateTime?> GenerateNextSequence(int seqLength, DateTime? fromDate = null, DateTime? toDate = null,
        TimeSpan? incrementBy = null, double chanceOfNull = 0.0d , double chanceOfMinTime = 0.0d)
    {
        var from    = fromDate ?? DefaultFromDateTime;
        var to      = toDate ?? DefaultToDateTime;
        var increments = incrementBy ?? DefaultIncrementTime;
        
        var dtMinValue = from.Ticks;
        var incrementTicks = Math.Abs(increments.Ticks);
        var dtMaxValue = to.Ticks;
        if (dtMaxValue <= dtMinValue)
        {
            dtMinValue = DefaultFromDateTime.Ticks;
            dtMaxValue = DefaultToDateTime.Ticks;
        }
        var tickRangeLength          = dtMaxValue - dtMinValue;
        var numberOfIntervals        = tickRangeLength / incrementTicks;
        var numberOfNullIntervals        = (long)(numberOfIntervals * chanceOfNull);
        var numberOfMinDateIntervals = (long)(numberOfIntervals * chanceOfMinTime);
        for (int i = 0; i < seqLength; i++)
        {
            var tickInterval = (long)(random.NextDouble() * (dtMaxValue - dtMinValue)/incrementTicks);
            
            if(numberOfNullIntervals > 1 && tickInterval % numberOfNullIntervals == 1) yield return null; 
            if(numberOfMinDateIntervals > 2 && tickInterval % numberOfMinDateIntervals == 2) yield return DateTime.MinValue;
            var nextValue = DateTime.FromBinary(dtMinValue + tickInterval * incrementTicks);
            yield return nextValue;
        }
    }

    public static IEnumerable<TimeSpan?> GenerateNextNullableTimeSpanSequence(int seqLength, TimeSpan? fromTime = null, TimeSpan? toTime = null,
        TimeSpan? incrementBy = null, double chanceOfNull = 0.0d , double chanceOfMinTime = 0.0d)
    {
        var from       = fromTime ?? DefaultFromTimeSpan;
        var to         = toTime ?? DefaultToTimeTimeSpan;
        var increments = incrementBy ?? DefaultIncrementTime;
        
        var toMinValue = from.Ticks;
        var incrementTicks = Math.Abs(increments.Ticks);
        var toMaxValue = to.Ticks;
        if (toMaxValue <= toMinValue)
        {
            toMinValue = DefaultFromDateTime.Ticks;
            toMaxValue = DefaultToDateTime.Ticks;
        }
        
        var tickRangeLength          = toMaxValue - toMinValue;
        var numberOfIntervals        = tickRangeLength / incrementTicks;
        var numberOfNullIntervals        = (long)(numberOfIntervals * chanceOfNull);
        var numberOfMinDateIntervals = (long)(numberOfIntervals * chanceOfMinTime);
        for (int i = 0; i < seqLength; i++)
        {
            var tickInterval = (long)(random.NextDouble() * (toMaxValue - toMinValue)/incrementTicks);
            
            if(numberOfNullIntervals > 1 && tickInterval % numberOfNullIntervals == 1) yield return null; 
            if(numberOfMinDateIntervals > 2 && tickInterval % numberOfMinDateIntervals == 2) yield return TimeSpan.MinValue;
            var nextValue = new  TimeSpan(toMinValue + tickInterval * incrementTicks);
            yield return nextValue;
        }
    }
}
