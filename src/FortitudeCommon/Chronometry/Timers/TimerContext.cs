// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Chronometry.Timers;

public interface ITimerProvider : IAsyncDisposable, IDisposable
{
    IUpdateableTimer CreateUpdateableTimer(string? name = "Unnamed-Timer");
}

public class RealTimerProvider : ITimerProvider
{
    public IUpdateableTimer CreateUpdateableTimer(string? name = "Unnamed-Timer") => new UpdateableTimer(name);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public void Dispose() { }
}

public class TimerContext
{
    private static readonly object SyncLock = new();

    private static IRunContextTimer? instance;
    private static ITimerProvider?   provider = new RealTimerProvider();
    public static IRunContextTimer Instance
    {
        get
        {
            if (instance == null)
                lock (SyncLock)
                {
                    instance ??= CreateUpdateableTimer("SingletonSharedInstance");
                }

            return instance;
        }
        set => instance = value;
    }

    public static ITimerProvider Provider
    {
        get => provider ??= new RealTimerProvider();
        set
        {
            if (instance != null) instance = null!;
            provider = value;
        }
    }

    public static IUpdateableTimer CreateUpdateableTimer(string? name = "Unnamed-Timer") => Provider.CreateUpdateableTimer(name);
}
