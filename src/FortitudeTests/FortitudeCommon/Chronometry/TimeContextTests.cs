// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Extensions;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry;

public interface IStubTimeProgressListener
{
    DateTime? GetNextScheduleEvent(DateTime fromDateTime, DateTime toDateTime);
    void      ProgressTime(DateTime fromDateTime, DateTime toDateTime);
}

public interface IUpdateTime : IDisposable, IAsyncDisposable
{
    DateTime UtcNow { get; set; }

    ValueTask<DateTime> UpdateTime(DateTime toTime);
}

public class StubTimeContext : ITimeContext, IUpdateTime
{
    private readonly List<IStubTimeProgressListener> registeredListeners = new();

    private DateTime utcNow;

    public StubTimeContext()
    {
        ThrowIfTimeGoesBackwards = false;
        UtcNow                   = DateTime.UtcNow;
    }

    public StubTimeContext(DateTime setUtcTimeTo) => UtcNow = setUtcTimeTo;

    public IUpdateTime ProgressTimeWithoutEvents => new UpdateNoEventsTime(this);

    public Func<DateTime, TimeSpan, IStubTimeProgressListener, ValueTask> IncrementedTimeCallback { get; set; }
        = (_, _, _) => ValueTask.CompletedTask;

    public bool ThrowIfTimeGoesBackwards { get; set; } = true;

    public DateTime UtcNow
    {
        get => utcNow;
        set
        {
            var updateEvent = UpdateTime(value);
            Task.WaitAny(updateEvent.AsTask(), Task.Delay(10_000));
        }
    }

    public DateTime LocalTimeNow => UtcNow.ToLocalTime();

    public async ValueTask<DateTime> UpdateTime(DateTime toTime)
    {
        if (toTime < utcNow && ThrowIfTimeGoesBackwards) throw new Exception($"Did not expect time to go from {utcNow} to {toTime}");
        var incrementingTime = utcNow;
        var listenersHaveEvent = registeredListeners
            .Any(stpl => stpl.GetNextScheduleEvent(incrementingTime, toTime) != null);
        while (listenersHaveEvent)
        {
            var nextEvent = registeredListeners
                            .Select(stpl => stpl.GetNextScheduleEvent(incrementingTime, toTime))
                            .Where(nextDateTime => nextDateTime != null)
                            .Min()!.Value;
            if (nextEvent > toTime) break;
            foreach (var listener in registeredListeners
                                     .Where(stpl => stpl.GetNextScheduleEvent(incrementingTime, toTime) == nextEvent).ToList())
            {
                var fromTime = utcNow;
                utcNow = nextEvent;
                listener.ProgressTime(incrementingTime, utcNow);
                await IncrementedTimeCallback(utcNow, utcNow - fromTime, listener);
            }
            incrementingTime = nextEvent.Min(toTime);
            listenersHaveEvent = registeredListeners
                .Any(stpl => stpl.GetNextScheduleEvent(incrementingTime, toTime) != null);
        }
        utcNow = toTime;
        return toTime;
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        registeredListeners.Clear();
        TimeContext.Provider = new HighPrecisionTimeContext();
        if (TimerContext.Provider is not RealTimerProvider) TimerContext.Provider = new RealTimerProvider();
    }

    public void AddStubTimeProgressListener(IStubTimeProgressListener stubTimeProgressListener)
    {
        registeredListeners.Add(stubTimeProgressListener);
    }

    public bool RemoveStubTimeProgressListener
        (IStubTimeProgressListener stubTimeProgressListener) =>
        registeredListeners.Remove(stubTimeProgressListener);

    private class UpdateNoEventsTime(StubTimeContext stubTimeContext) : IUpdateTime
    {
        private readonly DateTime originalTime = stubTimeContext.UtcNow;

        public DateTime UtcNow
        {
            get => stubTimeContext.utcNow;
            set => stubTimeContext.utcNow = value;
        }

        public ValueTask<DateTime> UpdateTime(DateTime toTime)
        {
            UtcNow = toTime;
            return new ValueTask<DateTime>(toTime);
        }

        public void Dispose()
        {
            stubTimeContext.utcNow = originalTime;
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }
    }
}

public static class StubTimeContextExtensions
{
    public static StubTimeContext Add(this StubTimeContext context, TimeSpan addSpan)
    {
        context.UtcNow += addSpan;
        return context;
    }

    public static async ValueTask<StubTimeContext> AddAsync(this StubTimeContext context, TimeSpan addSpan)
    {
        await context.UpdateTime(context.UtcNow + addSpan);
        return context;
    }

    public static StubTimeContext AddMilliseconds(this StubTimeContext context, int addMs) => context.Add(TimeSpan.FromMilliseconds(addMs));
    public static StubTimeContext AddSeconds(this StubTimeContext context, int addS)       => context.Add(TimeSpan.FromSeconds(addS));
    public static StubTimeContext AddMinutes(this StubTimeContext context, int addMinutes) => context.Add(TimeSpan.FromMinutes(addMinutes));
    public static StubTimeContext AddHours(this StubTimeContext context, int addHours)     => context.Add(TimeSpan.FromHours(addHours));
    public static StubTimeContext AddDays(this StubTimeContext context, int addDays)       => context.Add(TimeSpan.FromDays(addDays));

    public static ValueTask<StubTimeContext> AddMillisecondsAsync
        (this StubTimeContext context, int addMs) =>
        context.AddAsync(TimeSpan.FromMilliseconds(addMs));

    public static ValueTask<StubTimeContext> AddSecondsAsync(this StubTimeContext context, int addS) => context.AddAsync(TimeSpan.FromSeconds(addS));

    public static ValueTask<StubTimeContext> AddMinutesAsync
        (this StubTimeContext context, int addMinutes) =>
        context.AddAsync(TimeSpan.FromMinutes(addMinutes));

    public static ValueTask<StubTimeContext> AddHoursAsync
        (this StubTimeContext context, int addHours) =>
        context.AddAsync(TimeSpan.FromHours(addHours));

    public static ValueTask<StubTimeContext> AddDaysAsync(this StubTimeContext context, int addDays) => context.AddAsync(TimeSpan.FromDays(addDays));
}
