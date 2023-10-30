using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Monitoring.Metrics
{
    public interface IMetricRepository<T> where T : struct
    {
        IMetricRecorder<T> this[T lookupId] { get; }
        IEnumerable<IMetricRecorder<T>> AllMetrics();
    }
}
