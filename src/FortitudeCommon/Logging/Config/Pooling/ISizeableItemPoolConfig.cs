using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Logging.Config.Pooling;

public enum PoolScope
{
    Default
  , Private
  , Global
  , Named
  , LoggersGlobal
  , LoggerDescendants
  , AppendersGlobal
  , AppendersAllFile
}

public interface ISizeableItemPoolConfig
{
    const string Global        = "Global";
    const string LoggersGlobal = "LoggersGlobal";

    const int LogEntryDefaultStringCapacity = 512;

    const int LogEntryBatchSize             = 32;

    string PoolName { get; }

    PoolScope PoolScope { get; }

    int NewItemCapacity { get; }

    int ItemBatchSize { get; }
}
