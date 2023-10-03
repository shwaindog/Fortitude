namespace FortitudeCommon.Monitoring.Logging;

public interface IFLogger : IHierarchicalLogger
{
    bool IsDebugEnabled { get; }
    bool IsInfoEnabled { get; }
    bool IsWarnEnabled { get; }
    bool IsErrorEnabled { get; }
    string Name { get; }
    string HierarchicalSetting { get; set; }
    bool DefaultEnabled { get; set; }
    void Debug(string fmt, params object[] args);
    void Debug(string msg);
    void Debug(object obj);
    void Info(string fmt, params object[] args);
    void Info(string msg);
    void Info(object obj);
    void Warn(string fmt, params object[] args);
    void Warn(string msg);
    void Warn(object obj);
    void Error(string fmt, params object[] args);
    void Error(string msg);
    void Error(object obj);
    void OnLogEvent(FLogEvent evt);
}
