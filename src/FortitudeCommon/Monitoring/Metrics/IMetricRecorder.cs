using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Monitoring.Metrics
{
    public interface IMetricRecorder<out T> where T : struct
    {
        T LookupId { get; }
        void Recorder(long value);
        void Increment();
        long AppendAndReset(StringBuilder sb);
    }
}
