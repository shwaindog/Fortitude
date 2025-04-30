// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[TestClass]
public class PQFieldConvertersTests
{
    [TestMethod]
    public void DateNearEndOfHour_WhenConvertedToNumbersAndConvertedBackToDate_MaintainsNanosecondsNearEndOfHour()
    {
        var dateNearEndOfHour = new DateTime(2015, 11, 01, 23, 59, 58).AddMilliseconds(987);

        var hoursFromEpoch = dateNearEndOfHour.GetHoursFromUnixEpoch();
        var nanosInHour    = dateNearEndOfHour.GetSubHourComponent();

        var reconstituteDateTime = new DateTime();

        PQFieldConverters.UpdateHoursFromUnixEpoch(ref reconstituteDateTime, hoursFromEpoch);
        PQFieldConverters.UpdateSubHourComponent(ref reconstituteDateTime, nanosInHour);

        Assert.AreEqual(dateNearEndOfHour, reconstituteDateTime);
    }


    [TestMethod]
    public void DateNearEndOfHour_WhenConvertedToNumbersAndNumbersSplitAndReconstituted_NumberIsStillTheSame()
    {
        var dateNearEndOfHour = new DateTime(2015, 11, 01, 23, 59, 58).AddMilliseconds(987);

        var nanosInHour = dateNearEndOfHour.GetSubHourComponent();

        uint uintComponent;
        var  byteComponent = nanosInHour.BreakLongToUShortAndUint(out uintComponent);

        var reconstitutedLong = byteComponent.AppendUintToMakeLong(uintComponent);

        Assert.AreEqual(nanosInHour, reconstitutedLong);
    }
}
