namespace FortitudeCommon.Monitoring.Logging;

public sealed class FLogEvent
{
    public Exception? Exception;
    public FLogLevel Level = FLogLevel.Debug;
    public IFLogger Logger = NoopFactory.SingletonNoopLogger;
    public DateTime LogTime = default;
    public string MsgFormat = string.Empty;
    public object MsgObject = string.Empty;
    public object[]? MsgParams;
}
