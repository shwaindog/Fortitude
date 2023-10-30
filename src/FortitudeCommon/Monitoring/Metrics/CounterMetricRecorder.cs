using System.Text;
using System.Threading;

namespace FortitudeCommon.Monitoring.Metrics
{
    public class CounterMetricRecorder<T> : IMetricRecorder<T> where T : struct 
    {
        private long counter;

        public T LookupId { get; set; }

        public CounterMetricRecorder(T lookupId)
        {
            LookupId = lookupId;
        }

        public void Recorder(long value)
        {
            counter += value;
        }

        public void Increment()
        {
            Interlocked.Increment(ref counter);
        }

        public long AppendAndReset(StringBuilder sb)
        {
            var result = Interlocked.Exchange(ref counter, 0);
            sb.Append(LookupId).Append("=").Append(result);
            return result;
        }
    }
}