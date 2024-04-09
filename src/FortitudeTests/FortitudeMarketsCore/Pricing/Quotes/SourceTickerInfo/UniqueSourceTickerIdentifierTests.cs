#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;

[TestClass]
public class UniqueSourceTickerIdentifierTests
{
    public static void UpdateUniqueSourceTickerIdentifier(IUniqueSourceTickerIdentifier updateIdentifier,
        string sourceName, string ticker)
    {
        NonPublicInvocator.SetAutoPropertyInstanceField(updateIdentifier,
            (SourceTickerPublicationConfig x) => x.Source, sourceName);
        NonPublicInvocator.SetAutoPropertyInstanceField(updateIdentifier,
            (SourceTickerPublicationConfig x) => x.Ticker, ticker);
    }

    [TestMethod]
    public void NewUniqueSourceTickerIdentifer_New_IdGeneratedIsExpected()
    {
        ushort srcId = 123;
        ushort tkrId = 234;

        var firstUniSrcTkrId = new UniqueSourceTickerIdentifier("TestSource", "TestTicker", srcId, tkrId);

        var expectedId = ((uint)srcId << 16) + tkrId;

        Assert.AreEqual(expectedId, firstUniSrcTkrId.Id);
        Assert.AreEqual("TestTicker", firstUniSrcTkrId.Ticker);
        Assert.AreEqual("TestSource", firstUniSrcTkrId.Source);

        var secondUniSrcTkrId = new UniqueSourceTickerIdentifier(expectedId, "TestSource", "TestTicker");
        Assert.AreEqual(firstUniSrcTkrId, secondUniSrcTkrId);
    }

    [TestMethod]
    public void GivenASourceIdTickerId_UniqueSourceTickerIdentifier_GeneratesUniqueAndRepeatableKey()
    {
        var firstUniSrcTkrId = new UniqueSourceTickerIdentifier("TestSource", "TestTicker", 123, 234);
        var secondUniSrcTkrId = new UniqueSourceTickerIdentifier("TestSource1", "TestTicker1", 345, 456);
        var thirdUniSrcTkrId = new UniqueSourceTickerIdentifier("TestSource", "TestTicker", 123, 234);

        Assert.AreNotEqual(firstUniSrcTkrId.Id, secondUniSrcTkrId.Id);
        Assert.AreNotEqual(thirdUniSrcTkrId.Id, secondUniSrcTkrId.Id);
        Assert.AreEqual(firstUniSrcTkrId.Id, thirdUniSrcTkrId.Id);
    }

    [TestMethod]
    public void PopulatedUniqueSourceTickerId_Clone_CreatesNewUniqSourceTickerId()
    {
        var firstUniSrcTkrId = new UniqueSourceTickerIdentifier(uint.MaxValue, "TestSource", "TestTicker");

        var clone = firstUniSrcTkrId.Clone();
        var clone2 = ((ICloneable<IUniqueSourceTickerIdentifier>)firstUniSrcTkrId).Clone();

        Assert.AreNotSame(firstUniSrcTkrId, clone);
        Assert.AreEqual(firstUniSrcTkrId, clone);
        Assert.AreNotSame(firstUniSrcTkrId, clone2);
        Assert.AreEqual(firstUniSrcTkrId, clone2);
    }

    [TestMethod]
    public void NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame()
    {
        var commonSti = new UniqueSourceTickerIdentifier(12345, "TestSource", "TestTicker");

        var nonSti = new SourceTickerQuoteInfo(12345, "TestSource", "TestTicker");

        Assert.IsTrue(commonSti.AreEquivalent(nonSti));
        Assert.IsFalse(commonSti.AreEquivalent(nonSti, true));
    }

    [TestMethod]
    public void OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent()
    {
        var commonSti = new UniqueSourceTickerIdentifier("TestSource", "TestTicker", 123, 234);

        var srcDiffSti = new UniqueSourceTickerIdentifier("DiffSource", "TestTicker", 123, 234);
        Assert.AreNotEqual(commonSti, srcDiffSti);

        var tkrDiffSti = new UniqueSourceTickerIdentifier("TestSource", "DiffTicker", 123, 234);
        Assert.AreNotEqual(commonSti, tkrDiffSti);

        var idDiffSti = new UniqueSourceTickerIdentifier("TestSource", "TestTicker", 123, 235);
        Assert.AreNotEqual(commonSti, idDiffSti);

        var sameSti = new UniqueSourceTickerIdentifier("TestSource", "TestTicker", 123, 234);
        Assert.AreEqual(commonSti.Id, sameSti.Id);
    }

    [TestMethod]
    public void PopulatedSti_GetHashCode_NotEqualTo0()
    {
        var firstUniSrcTkrId = new UniqueSourceTickerIdentifier("TestSource", "TestTicker", 123, 234);

        Assert.AreNotEqual(0, firstUniSrcTkrId.GetHashCode());
    }

    [TestMethod]
    public void FullyPopulatedSti_ToString_ReturnsNameAndValues()
    {
        var firstUniSrcTkrId = new UniqueSourceTickerIdentifier("TestSource", "TestTicker", 123, 234);
        var toString = firstUniSrcTkrId.ToString();

        Assert.IsTrue(toString.Contains(firstUniSrcTkrId.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(firstUniSrcTkrId.Id)}: {firstUniSrcTkrId.Id}"));
        Assert.IsTrue(toString.Contains($"{nameof(firstUniSrcTkrId.Ticker)}: {firstUniSrcTkrId.Ticker}"));
        Assert.IsTrue(toString.Contains($"{nameof(firstUniSrcTkrId.Source)}: {firstUniSrcTkrId.Source}"));
    }
}
