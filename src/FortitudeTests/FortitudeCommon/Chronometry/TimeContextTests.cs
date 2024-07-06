// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeTests.FortitudeCommon.Chronometry;

public class StubTimeContext : ITimeContext, IDisposable, IAsyncDisposable
{
    public StubTimeContext(DateTime? setUtcTimeTo = null) => UtcNow = setUtcTimeTo ?? DateTime.UtcNow;

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        TimeContext.Provider = new HighPrecisionTimeContext();
    }

    public DateTime UtcNow       { get; set; }
    public DateTime LocalTimeNow => UtcNow.ToLocalTime();
}

public static class StubTimeContextExtensions
{
    public static ITimeContext Add(this StubTimeContext context, TimeSpan addSpan)
    {
        context.UtcNow += addSpan;
        return context;
    }

    public static ITimeContext AddMilliseconds(this StubTimeContext context, int addMs) => context.Add(TimeSpan.FromMilliseconds(addMs));
    public static ITimeContext AddSeconds(this StubTimeContext context, int addS)       => context.Add(TimeSpan.FromSeconds(addS));
    public static ITimeContext AddMinutes(this StubTimeContext context, int addMinutes) => context.Add(TimeSpan.FromMinutes(addMinutes));
    public static ITimeContext AddHours(this StubTimeContext context, int addHours)     => context.Add(TimeSpan.FromHours(addHours));
    public static ITimeContext AddDays(this StubTimeContext context, int addDays)       => context.Add(TimeSpan.FromDays(addDays));
}
