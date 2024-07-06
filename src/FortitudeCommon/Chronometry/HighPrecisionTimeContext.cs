// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Chronometry;

public sealed class StandardTimeContext : ITimeContext
{
    public DateTime UtcNow       => DateTime.UtcNow;
    public DateTime LocalTimeNow => DateTime.Now;
}

public sealed class HighPrecisionTimeContext(int syncPeriodSeconds = 10) : ITimeContext
{
    private readonly DateTimeEx dtx = new(syncPeriodSeconds);

    public DateTime UtcNow => dtx.UtcNow;

    public DateTime LocalTimeNow => dtx.UtcNow.ToLocalTime();
}
