using System;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats
{
    public struct CallStatsForPercentile
    {
        public readonly double AverageMeasurement;
        public readonly TimeSpan AverageTimeSpan;
        public readonly TimeSpan BoundaryTimeSpan;
        public readonly double HighestMeasurement;
        public readonly double LowestMeasurement;
        public readonly int NumberOfItems;
        public bool BoundaryIsLongestTime;

        public CallStatsForPercentile(int numberOfItems, TimeSpan averageTimeSpan, bool boundaryIsLongestTime,
            TimeSpan boundaryTimeSpan, double lowestMeasurement, double highestMeasurement, double averageMeasurement)
        {
            NumberOfItems = numberOfItems;
            AverageTimeSpan = averageTimeSpan;
            BoundaryIsLongestTime = boundaryIsLongestTime;
            BoundaryTimeSpan = boundaryTimeSpan;
            LowestMeasurement = lowestMeasurement;
            HighestMeasurement = highestMeasurement;
            AverageMeasurement = averageMeasurement;
        }
    }
}