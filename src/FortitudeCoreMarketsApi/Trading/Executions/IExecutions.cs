using System.Collections.Generic;

namespace FortitudeMarketsApi.Trading.Executions
{
    public interface IExecutions : IEnumerable<IExecution>
    {
        int Count { get; }
        IExecution this[int index] { get; set; }
        void Add(IExecution execution);
        IExecutions Clone();
    }
}
