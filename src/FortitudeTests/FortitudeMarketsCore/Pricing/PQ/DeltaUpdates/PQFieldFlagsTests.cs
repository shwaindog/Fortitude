using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DeltaUpdates
{
    [TestClass]
    public class PQFieldFlagsTests
    {
        [TestMethod]
        public void AskFieldUpdate_IsAsk_ReturnsIsAskTrueAndIsBidFalse()
        {
            var fieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, 0, PQFieldFlags.IsAskSideFlag);

            Assert.IsTrue(fieldUpdate.IsAsk());
            Assert.IsFalse(fieldUpdate.IsBid());
        }

        [TestMethod]
        public void BidFieldUpdate_IsBid_ReturnsIsAskFalseAndIsBidTrue()
        {
            var fieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, 0);

            Assert.IsFalse(fieldUpdate.IsAsk());
            Assert.IsTrue(fieldUpdate.IsBid());
        }
    }
}