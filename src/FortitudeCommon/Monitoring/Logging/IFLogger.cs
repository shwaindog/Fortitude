#region

using System.Diagnostics.CodeAnalysis;

#endregion

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
    void Debug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string fmt, params object?[] args);
    void Debug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string msg);
    void Debug(object obj);
    void Info([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string fmt, params object?[] args);
    void Info([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string msg);
    void Info(object obj);
    void Warn([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string fmt, params object?[] args);
    void Warn([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string msg);
    void Warn(object obj);
    void Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string fmt, params object?[] args);
    void Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string msg);
    void Error(object obj);
    void OnLogEvent(FLogEvent evt);
}
