// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Configuration.Availability;

public interface IAvailability
{
    bool           ShouldBeUp(DateTimeOffset atThisDateTime);
    TimeSpan       ExpectedRemainingUpTime(DateTimeOffset fromNow);
    DateTimeOffset NextScheduledOpeningTime(DateTimeOffset fromNow);
}
