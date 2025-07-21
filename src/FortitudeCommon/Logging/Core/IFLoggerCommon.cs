// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.LoggerVisitors;
using FortitudeCommon.Logging.Core.Pooling;

namespace FortitudeCommon.Logging.Core;

public enum LoggerTreeType
{
    Unknown
  , Root
  , Descendant
}

public interface IFLoggerCommon
{
    FLogLevel LogLevel { get; }

    LoggerTreeType TreeType { get; }

    string Name { get; }

    string FullName { get; }

    FLogEntryPool LogEntryPool { get; }

    T Visit<T>(T visitor) where T : IFLoggerVisitor<T>;

    IReadOnlyList<IFLogger> ImmediateEmbodiedChildren { get; }

    IReadOnlyList<Appending.IFLoggerAppender> Appenders { get; }
}

public abstract class FLoggerBase(IFLoggerTreeCommonConfig loggerConfig) : IFLoggerCommon
{
    protected ConcurrentDictionary<string, IFLogger> ImmediateChildrenDict = new();

    protected List<IFLoggerAppender> LoggerAppenders = new();

    public string Name { get; protected set; } = loggerConfig.Name;

    public string FullName { get; protected set; } = loggerConfig.Name; // ToDo change to Full name

    public FLogLevel LogLevel { get; internal set; } = loggerConfig.LogLevel;

    public FLogEntryPool LogEntryPool { get; protected set; } = null!;

    public IReadOnlyList<IFLoggerAppender> Appenders
    {
        get => LoggerAppenders.AsReadOnly();
        internal set
        {
            LoggerAppenders.Clear();
            LoggerAppenders.AddRange(value);
        }
    }

    public IReadOnlyList<IFLogger> ImmediateEmbodiedChildren => ImmediateChildrenDict.Values.ToList().AsReadOnly();

    public abstract LoggerTreeType TreeType { get; }

    internal IFLogger AddDirectChild(IFLogger newChild)
    {
        if (!ImmediateChildrenDict.TryAdd(newChild.Name, newChild))
        {
            return ImmediateChildrenDict[newChild.Name];
        }
        return newChild;
    }

    public virtual T Visit<T>(T visitor) where T : IFLoggerVisitor<T>
    {
        return visitor.Accept(this);
    }
}
