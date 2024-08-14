// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Monitoring.Logging;

public sealed class FLogEvent
{
    public static readonly object?[] UnsetMsgParams = Array.Empty<object?>();
    public static readonly object    UnsetMsgObject = string.Empty;
    public static readonly string    UnsetMsgFormat = string.Empty;

    public Exception? Exception;
    public FLogLevel  Level     = FLogLevel.Debug;
    public IFLogger   Logger    = NoOpLoggerFactory.SingletonNoopLogger;
    public DateTime   LogTime   = default;
    public string     MsgFormat = UnsetMsgFormat;
    public object     MsgObject = UnsetMsgObject;
    public object?[]  MsgParams = UnsetMsgParams;
}
