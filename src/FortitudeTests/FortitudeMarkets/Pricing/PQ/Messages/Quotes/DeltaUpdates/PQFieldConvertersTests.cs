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

        var hoursFromEpoch = dateNearEndOfHour.Get2MinIntervalsFromUnixEpoch();
        var nanosInHour    = dateNearEndOfHour.GetSub2MinComponent();

        var reconstituteDateTime = new DateTime();

        PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref reconstituteDateTime, hoursFromEpoch);
        PQFieldConverters.UpdateSub2MinComponent(ref reconstituteDateTime, nanosInHour);

        Assert.AreEqual(dateNearEndOfHour, reconstituteDateTime);
    }


    [TestMethod]
    public void DateNearEndOf2MinInterval_WhenConvertedToNumbersAndNumbersSplitAndReconstituted_NumberIsStillTheSame()
    {
        var dateNearEndOfHour = new DateTime(2015, 11, 01, 23, 59, 58).AddMilliseconds(987);

        var nanosIn2Mins = dateNearEndOfHour.GetSub2MinComponent();

        uint uintComponent;
        var  byteComponent = nanosIn2Mins.BreakLongToUShortAndScaleFlags(out uintComponent);

        var reconstitutedLong = byteComponent.AppendScaleFlagsToUintToMakeLong(uintComponent);

        Assert.AreEqual(nanosIn2Mins, reconstitutedLong);
    }
}
