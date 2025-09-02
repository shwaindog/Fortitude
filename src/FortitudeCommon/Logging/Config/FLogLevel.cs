// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config;

public enum FLogLevel
{
    None = 0
  , Trace
  , Debug
  , Info
  , Warn
  , Error
  , Disabled
}

public enum FLogLevelMatch
{
    None = 0
  , Any
  , ConfigDefined
  , Trace
  , Debug
  , Info
  , Warn
  , Error
}

public static class FLogLevelExtensions
{
    public static int LoggableRange => FLogLevel.Error - FLogLevel.None;

    public static bool IsLevelDisabled(this FLogLevel level, FLogLevel checkEnabled) => checkEnabled < level;

    public static bool IsTraceDisabled(this FLogLevel level) => FLogLevel.Trace < level;

    public static bool IsDebugDisabled(this FLogLevel level) => FLogLevel.Debug < level;

    public static bool IsInfoDisabled(this FLogLevel level) => FLogLevel.Info < level;

    public static bool IsWarnDisabled(this FLogLevel level) => FLogLevel.Warn < level;

    public static bool IsErrorDisabled(this FLogLevel level) => FLogLevel.Error < level;
}
