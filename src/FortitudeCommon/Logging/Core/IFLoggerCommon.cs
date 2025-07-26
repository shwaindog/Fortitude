// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Hub;
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

    IFLoggerTreeCommonConfig ResolvedConfig { get; }

    T Visit<T>(T visitor) where T : IFLoggerVisitor<T>;

    IReadOnlyList<IFLogger> ImmediateEmbodiedChildren { get; }

    IReadOnlyList<Appending.IFLogAppender> Appenders { get; }
}

public interface IMutableFLoggerCommon : IFLoggerCommon
{
    void HandleConfigUpdate(IFLoggerTreeCommonConfig newLoggerState, IFLogAppenderRegistry appenderRegistry);

    IFLogger AddDirectChild(IFLogger newChild);
}

public abstract class FLoggerBase : IMutableFLoggerCommon
{
    protected static readonly FLogEntryPoolConfig DefaultLoggerEntryPoolConfig
        = new(IFLogEntryPoolConfig.LoggersGlobal, PoolScope.LoggersGlobal);

    protected ConcurrentDictionary<string, IFLogger> ImmediateChildrenDict = new();

    protected List<IFLogAppender> LoggerAppenders = new();

    protected FLoggerBase(IFLoggerTreeCommonConfig loggerConfig, IFLogLoggerRegistry loggerRegistry)
    {
        ResolvedConfig = loggerConfig;

        Name     = loggerConfig.Name;
        FullName = loggerConfig.FullName;
        LogLevel = loggerConfig.LogLevel;

        LogEntryPool = loggerRegistry.SourceFLogEntryPool(loggerConfig.LogEntryPool ?? DefaultLoggerEntryPoolConfig);
    }

    public string Name { get; protected set; }

    public string FullName { get; protected set; }

    public FLogLevel LogLevel { get; internal set; }

    public FLogEntryPool LogEntryPool { get; private set; }

    public IFLoggerTreeCommonConfig ResolvedConfig { get; protected set; }

    public void HandleConfigUpdate(IFLoggerTreeCommonConfig newLoggerState, IFLogAppenderRegistry appenderRegistry)
    {
        if (FullName != newLoggerState.FullName)
        {
            #if DEBUG
            Debugger.Break();
            #endif
            return;
        }
        if (ResolvedConfig.AreEquivalent(newLoggerState)) return;
        LogLevel = newLoggerState.LogLevel;

        foreach (var appenderConfig in newLoggerState.Appenders)
        {
            if (Appenders.All(a => a.AppenderName != appenderConfig.Key))
            {
                appenderRegistry.RegistryAppenderInterest((toAdd) => LoggerAppenders.Add(toAdd), appenderConfig.Key);
            }
        }
        for (var i = 0; i < Appenders.Count; i++)
        {
            var existingAppender = Appenders[i];
            if (!newLoggerState.Appenders.ContainsKey(existingAppender.AppenderName))
            {
                LoggerAppenders.RemoveAt(i);
                i--;
            }
        }
        ResolvedConfig = newLoggerState;
    }

    public IReadOnlyList<IFLogAppender> Appenders
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

    IFLogger IMutableFLoggerCommon.AddDirectChild(IFLogger newChild)
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
