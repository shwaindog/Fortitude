namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

public interface ITraceLogger : IHierarchicalLogger
{
    bool WriteTrace { get; set; }
    bool DefaultWriteTrace { get; set; }
    void Start();
    void Add<TU>(string identifier, TU subject);
    void Add(string identifier);
    void Indent();
    void Dedent();
    List<TraceEntry> TraceFinished();
}
