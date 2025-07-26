// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Chronometry.Timers;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLogContext
{
    bool HasStarted { get; }

    bool IsInitialized { get; }

    IFLogAppenderRegistry  AppenderRegistry     { get; }
    IFLogLoggerRegistry    LoggerRegistry       { get; }
    IFLoggerAsyncRegistry  AsyncRegistry        { get; }
    IFLogConfigRegistry    ConfigRegistry       { get; }
    IFLogEntryPoolRegistry LogEntryPoolRegistry { get; }
    IUpdateableTimer       LoggerTimers         { get; }
} 


public class FLogContext : IFLogContext
{
    private static IFLogContext? singletonInstance;

    private static readonly object SyncLock = new ();

    public static IFLogContext UninitializedInstance
    {
        get
        {
            if (singletonInstance == null)
            {
                lock (SyncLock)
                {
                    if (singletonInstance == null)
                    {
                        singletonInstance = new FLogContext();
                    }
                }
            }
            return singletonInstance;
        }
        set
        {
            lock (SyncLock)
            {
                if (singletonInstance != null && (singletonInstance.IsInitialized || singletonInstance.HasStarted))
                {
                    Console.Out.WriteLine($"Can not replace {typeof(FLogContext).FullName} once it has been initialized or started");
                    throw new InvalidOperationException($"Can not replace {typeof(FLogContext).FullName} once it has been initialized or started");
                }
                singletonInstance = value;
            }
        }
    }

    public static IFLogContext UnstartedInstance
    {
        get
        {
            var instance = UninitializedInstance;
            if (!instance.IsInitialized)
            {
                return instance.DefaultInitializeContext();
            }
            return instance;
        }
    }

    public static IFLogContext Context
    {
        get
        {
            var instance = UninitializedInstance;
            if (!instance.HasStarted)
            {
                return FLogBootstrap.Instance.StartFlog((FLogContext)instance);
            }
            return instance;
        }
    }

    public FLogContext(string flogTimersName = "FLog Timers")
    {
        LoggerTimers     = new UpdateableTimer(flogTimersName);
    }

    public bool HasStarted { get; internal set; }

    public bool IsInitialized { get; internal set; }

    public IFLogAppenderRegistry AppenderRegistry { get; set; }

    public IFLogLoggerRegistry LoggerRegistry { get; set; }

    public IFLoggerAsyncRegistry AsyncRegistry { get; set; }

    public IFLogConfigRegistry    ConfigRegistry       { get; set; }

    public IFLogEntryPoolRegistry LogEntryPoolRegistry { get; set; }

    public IUpdateableTimer LoggerTimers { get; }
}
