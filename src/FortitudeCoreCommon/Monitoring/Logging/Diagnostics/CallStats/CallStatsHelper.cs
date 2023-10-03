using System;
using System.Collections.Generic;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats
{
    internal static class CallStatsHelper
    {
        public static string ToStringNoBoxing(this CallStatsForPercentile convertToString)
        {
            var boundaryTimeSpanText = convertToString.BoundaryIsLongestTime ? "LngstTime" : "QkstTime";
            if (Math.Abs(convertToString.AverageMeasurement) < 0.001)
            {
                return
                    $"{{#Itms:{convertToString.NumberOfItems:N0}, " +
                    $"AvgTime:{convertToString.AverageTimeSpan.TotalMilliseconds:N3}ms, " +
                    $"{boundaryTimeSpanText}:{convertToString.BoundaryTimeSpan.TotalMilliseconds:N3}ms}}";
            }
            return
                $"{{#Itms:{convertToString.NumberOfItems:N0}, " +
                $"AvgTime:{convertToString.AverageTimeSpan.TotalMilliseconds:N3}ms, " +
                $"{boundaryTimeSpanText}:{convertToString.BoundaryTimeSpan.TotalMilliseconds:N3}ms, " +
                $"LwstMsrmnt:{convertToString.LowestMeasurement:N3}, " +
                $"HghstMsrmnt:{convertToString.HighestMeasurement:N3}, " +
                $"AvgMsrmnt:{convertToString.AverageMeasurement:N3}}}";
        }

        public static CallStatsForPercentile QuickestPercentile(this List<CallStat> sortedAscendingTimeSpan,
            double percentile)
        {
            var indexOfPercentile =
                (int) Math.Round(sortedAscendingTimeSpan.Count*percentile, 0, MidpointRounding.AwayFromZero);
            long totalTicks = 0;
            double totalMeasurement = 0;
            var lowestMeasurement = double.MaxValue;
            var highestMeasurement = double.MinValue;
            var longestTime = TimeSpan.MinValue;
            for (var i = 0; i < indexOfPercentile; i++)
            {
                var callStat = sortedAscendingTimeSpan[i];
                totalTicks += callStat.ExecutionTime.Ticks;
                if (callStat.ExecutionTime > longestTime)
                {
                    longestTime = callStat.ExecutionTime;
                }
                var measurement = callStat.Measurement;
                totalMeasurement += measurement;
                if (measurement < lowestMeasurement)
                {
                    lowestMeasurement = measurement;
                }
                if (measurement > highestMeasurement)
                {
                    highestMeasurement = measurement;
                }
            }
            return new CallStatsForPercentile(indexOfPercentile,
                TimeSpan.FromTicks(totalTicks/(indexOfPercentile <= 0 ? 1 : indexOfPercentile)),
                true, longestTime, lowestMeasurement, highestMeasurement, totalMeasurement/indexOfPercentile);
        }

        public static CallStatsForPercentile SlowestPercentile(this List<CallStat> sortedAscendingTimeSpan,
            double percentile)
        {
            var indexOfPercentile =
                (int)
                Math.Round(sortedAscendingTimeSpan.Count - (sortedAscendingTimeSpan.Count*percentile), 0,
                    MidpointRounding.AwayFromZero);
            long totalTicks = 0;
            var numberOfCalls = 0;
            double totalMeasurement = 0;
            var lowestMeasurement = double.MaxValue;
            var highestMeasurement = double.MinValue;
            var quickesTimeSpan = TimeSpan.MaxValue;
            for (var i = sortedAscendingTimeSpan.Count - 1; i >= indexOfPercentile; i--)
            {
                numberOfCalls++;
                var callStat = sortedAscendingTimeSpan[i];
                totalTicks += callStat.ExecutionTime.Ticks;
                if (callStat.ExecutionTime < quickesTimeSpan)
                {
                    quickesTimeSpan = callStat.ExecutionTime;
                }
                var measurement = callStat.Measurement;
                totalMeasurement += measurement;
                if (measurement < lowestMeasurement)
                {
                    lowestMeasurement = measurement;
                }
                if (measurement > highestMeasurement)
                {
                    highestMeasurement = measurement;
                }
            }
            numberOfCalls = numberOfCalls <= 0 ? 1 : numberOfCalls;
            return new CallStatsForPercentile(numberOfCalls, TimeSpan.FromTicks(totalTicks/numberOfCalls),
                false, quickesTimeSpan, lowestMeasurement, highestMeasurement, totalMeasurement/numberOfCalls);
        }
    }
}