using System;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DeltaUpdates
{
    [TestClass]
    public class PQFieldConvertersTests
    {
        [TestMethod]
        public void DateNearEndOfHour_WhenConvertedToNumbersAndConvertedBackToDate_MaintainsNanosecondsNearEndOfHour()
        {
            DateTime dateNearEndOfHour = new DateTime(2015, 11, 01, 23, 59, 58).AddMilliseconds(987);

            uint hoursFromEpoch = dateNearEndOfHour.GetHoursFromUnixEpoch();
            long nanosInHour = dateNearEndOfHour.GetSubHourComponent();

            DateTime reconstituteDateTime = new DateTime();

            PQFieldConverters.UpdateHoursFromUnixEpoch(ref reconstituteDateTime, hoursFromEpoch);
            PQFieldConverters.UpdateSubHourComponent(ref reconstituteDateTime, nanosInHour);

            Assert.AreEqual(dateNearEndOfHour, reconstituteDateTime);
        }


        [TestMethod]
        public void DateNearEndOfHour_WhenConvertedToNumbersAndNumbersSplitAndReconstituted_NumberIsStillTheSame()
        {
            DateTime dateNearEndOfHour = new DateTime(2015, 11, 01, 23, 59, 58).AddMilliseconds(987);
            
            long nanosInHour = dateNearEndOfHour.GetSubHourComponent();

            uint uintComponent;
            byte byteComponent = nanosInHour.BreakLongToByteAndUint(out uintComponent);

            long reconstitutedLong = byteComponent.AppendUintToMakeLong(uintComponent);

            Assert.AreEqual(nanosInHour, reconstitutedLong);
        }
    }
}
