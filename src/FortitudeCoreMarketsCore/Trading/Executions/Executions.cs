using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeMarketsApi.Trading.Executions;

namespace FortitudeMarketsCore.Trading.Executions
{
    public class Executions : IExecutions
    {
        private readonly IList<IExecution> executions;

        public Executions()
        {
            executions = new List<IExecution>();
        }

        public Executions(IExecutions toClone)
        {
            executions = toClone.Select(e => e.Clone()).ToList();
        }

        public Executions(IList<IExecution> executions)
        {
            this.executions = executions;
        }

        public int Count => executions.Count;

        public IExecution this[int index]
        {
            get => executions[index];
            set => executions[index] = value;
        }

        public void Add(IExecution execution)
        {
            executions.Add(execution);
        }

        public IExecutions Clone()
        {
            return new Executions(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IExecution> GetEnumerator()
        {
            return executions.GetEnumerator();
        }
    }
}