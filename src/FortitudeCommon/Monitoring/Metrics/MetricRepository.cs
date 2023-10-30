using System.Collections.Generic;

namespace FortitudeCommon.Monitoring.Metrics
{
    public class MetricRepository<T> : IMetricRepository<T> where T : struct 
    {
        private readonly IDictionary<T, IMetricRecorder<T>> repositoryByName = new Dictionary<T, IMetricRecorder<T>>();

        public MetricRepository(params IMetricRecorder<T>[] allMetricRecorders)
        {
            foreach (var metricRecorder in allMetricRecorders)
            {
                repositoryByName.Add(metricRecorder.LookupId, metricRecorder);
            }
        }

        public IMetricRecorder<T> this[T lookupId] => repositoryByName[lookupId];

        public IEnumerable<IMetricRecorder<T>> AllMetrics()
        {
            return repositoryByName.Values;
        }
    }
}