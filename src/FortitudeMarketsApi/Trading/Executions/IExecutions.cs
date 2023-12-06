#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarketsApi.Trading.Executions;

public interface IExecutions : IEnumerable<IExecution>, IReusableObject<IExecutions>
{
    int Count { get; }
    IExecution this[int index] { get; set; }
    void Add(IExecution execution);
}
