using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Executions
{
    public class OrxExecutions : IExecutions
    {
        public OrxExecutions()
        {
        }

        public OrxExecutions(IExecutions toClone)
        {
            ExecutionsList = toClone.Select(e => new OrxExecution(e)).ToList();
        }

        public OrxExecutions(IEnumerable<IExecution> executions)
        {
            ExecutionsList = executions.Select(e => new OrxExecution(e)).ToList();
        }

        [OrxMandatoryField(0)]
        public List<OrxExecution> ExecutionsList { get; set; }

        public void Add(IExecution execution)
        {
            if (execution is OrxExecution orxExecution)
            {
                ExecutionsList.Add(orxExecution);
            }
            else
            {
                ExecutionsList.Add(new OrxExecution(execution));
            }
        }

        public int Count => ExecutionsList.Count;

        public IExecution this[int index]
        {
            get => ExecutionsList[index];
            set => ExecutionsList[index] = (OrxExecution) value;
        }

        public void CopyFrom(IExecutions executions, IRecycler recycler)
        {
            var executionsCount = executions.Count;
            if (executionsCount > 0)
            {
                var orxExecutionsList = recycler.Borrow<List<OrxExecution>>();
                orxExecutionsList.Clear();
                for (int i = 0; i < executionsCount; i++)
                {
                    var orxExecution = recycler.Borrow<OrxExecution>();
                    orxExecution.CopyFrom(executions[i], recycler);
                    orxExecutionsList.Add(orxExecution);
                }
                ExecutionsList = orxExecutionsList;
            }
        }

        public IExecutions Clone()
        {
            return new OrxExecutions(this);
        }

        protected bool Equals(OrxExecutions other)
        {
            return ExecutionsList?.SequenceEqual(other.ExecutionsList) ?? other.ExecutionsList == null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxExecutions) obj);
        }

        public override int GetHashCode()
        {
            return (ExecutionsList != null ? ExecutionsList.GetHashCode() : 0);
        }

        public IEnumerator<IExecution> GetEnumerator()
        {
            return ExecutionsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}