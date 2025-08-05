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

    IReadOnlyList<IAppenderClient> Appenders { get; }
}

public interface IMutableFLoggerCommon : IFLoggerCommon
{
    new IMutableFLoggerTreeCommonConfig ResolvedConfig { get; set;  }

    new IReadOnlyList<IMutableFLogger> ImmediateEmbodiedChildren { get; }

    void HandleConfigUpdate(IMutableFLoggerTreeCommonConfig newLoggerState, IFLogAppenderRegistry appenderRegistry);

    IMutableFLogger AddDirectChild(IMutableFLogger newChild);
}

public abstract class FLoggerBase : IMutableFLoggerCommon
{
    protected static readonly FLogEntryPoolConfig DefaultLoggerEntryPoolConfig
        = new(IFLogEntryPoolConfig.LoggersGlobal, PoolScope.LoggersGlobal);

    protected ConcurrentDictionary<string, IMutableFLogger> ImmediateChildrenDict = new();

    protected List<IAppenderClient> LoggerAppenders = [];

    protected bool AppendersUpdatedFlag = true;

    protected IMutableFLoggerTreeCommonConfig Config = null!;

    protected FLoggerBase(IMutableFLoggerTreeCommonConfig loggerConfig, IFLogLoggerRegistry loggerRegistry)
    {
        Name     = loggerConfig.Name;
        FullName = loggerConfig.FullName;
        HandleConfigUpdate(loggerConfig, loggerRegistry.AppenderRegistry);

        LogLevel = loggerConfig.LogLevel;


        LogEntryPool = loggerRegistry.SourceFLogEntryPool(loggerConfig.LogEntryPool ?? DefaultLoggerEntryPoolConfig);
    }

    public string Name { get; protected set; }

    public string FullName { get; protected set; }

    public FLogLevel LogLevel { get; internal set; }

    public FLogEntryPool LogEntryPool { get; private set; }

    IMutableFLoggerTreeCommonConfig IMutableFLoggerCommon.ResolvedConfig
    {
        get => Config;
        set => Config = value;
    }

    public virtual IFLoggerTreeCommonConfig ResolvedConfig => Config;

    public void HandleConfigUpdate(IMutableFLoggerTreeCommonConfig newLoggerState, IFLogAppenderRegistry appenderRegistry)
    {
        if (FullName != newLoggerState.FullName)
        {
            #if DEBUG
            Debugger.Break();
            #endif
            return;
        }
        if (newLoggerState.AreEquivalent(ResolvedConfig)) return;
        LogLevel = newLoggerState.LogLevel;

        lock (Appenders)
        {
            foreach (var appenderConfig in newLoggerState.Appenders)
            {
                if (Appenders.All(a => a.BackingAppender.AppenderName != appenderConfig.Key))
                {
                    AppendersUpdatedFlag = true;
                    var appenderClient = appenderRegistry.GetAppenderClient(appenderConfig.Key, this);
                    LoggerAppenders.Add(appenderClient);
                }
            }
            for (var i = 0; i < Appenders.Count; i++)
            {
                var existingAppender = Appenders[i];
                if (!newLoggerState.Appenders.ContainsKey(existingAppender.BackingAppender.AppenderName))
                {
                    AppendersUpdatedFlag = true;
                    LoggerAppenders.RemoveAt(i);
                    i--;
                }
            }
        }
        Config = newLoggerState;
    }

    public IReadOnlyList<IAppenderClient> Appenders
    {
        get => LoggerAppenders;
        internal set
        {
            lock (Appenders)
            {
                AppendersUpdatedFlag = true;
                LoggerAppenders.Clear();
                LoggerAppenders.AddRange(value);
            }
        }
    }

    IReadOnlyList<IMutableFLogger> IMutableFLoggerCommon.ImmediateEmbodiedChildren => ImmediateChildrenDict.Values.ToList().AsReadOnly();

    public IReadOnlyList<IFLogger> ImmediateEmbodiedChildren => ((IMutableFLoggerCommon)this).ImmediateEmbodiedChildren;

    public abstract LoggerTreeType TreeType { get; }

    IMutableFLogger IMutableFLoggerCommon.AddDirectChild(IMutableFLogger newChild)
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
