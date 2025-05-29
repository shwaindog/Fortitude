using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.DataStructures.Lists;

public interface ICapacityList<out T> : IReadOnlyList<T>
{
    int Capacity { get; }
}

public interface IMutableCapacityList<T> : ICapacityList<T>, IList<T>
{
    new T this [int i] { get; set; }
    new int Count { get; set; }
    new int Capacity { get; set; }
}