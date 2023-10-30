namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

public struct TraceEntry
{
    public readonly int Depth;
    public readonly string Identifier;
    public readonly object? Subject;
    public readonly DateTime Time;

    public TraceEntry(DateTime time, string identifier, object? subject, int depth)
    {
        Time = time;
        Identifier = identifier;
        Subject = subject;
        Depth = depth;
    }

    public override string ToString() => "Boxed " + this.ToStringNoBoxing();
}
