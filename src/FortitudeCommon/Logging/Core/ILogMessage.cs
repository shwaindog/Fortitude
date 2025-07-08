using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.DataStructures.Lists;

namespace FortitudeCommon.Logging.Core;

public interface ILogMessage : IReadOnlyList<char>
{
}


public interface IMutableLogMessage : ILogMessage, IMutableCapacityList<char>
{
}