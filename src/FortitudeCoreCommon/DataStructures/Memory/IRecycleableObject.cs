using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.DataStructures.Memory
{
    public interface IRecycleableObject
    {
        bool ShouldAutoRecycle { get; set; }
        IRecycler Recycler { get; set; }
    }
}
