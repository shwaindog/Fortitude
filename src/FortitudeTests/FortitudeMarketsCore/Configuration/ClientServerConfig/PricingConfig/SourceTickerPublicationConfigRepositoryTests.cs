#region

using FortitudeCommon.Types;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;

[TestClass]
public class SourceTickerPublicationConfigRepositoryTests
{
    private SourceTickerPublicationConfig firstSourceTickerPublicationConfig = null!;
    private SourceTickerPublicationConfig secondSourceTickerPublicationConfig = null!;
    private SourceTickerPublicationConfigRepository sourceTickerPublicationConfig = null!;
    private SourceTickerPublicationConfig thirdSourceTickerPublicationConfig = null!;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerPublicationConfig = SampleSourceTickerPublicationConfigRepo();
        firstSourceTickerPublicationConfig = (SourceTickerPublicationConfig)sourceTickerPublicationConfig.ElementAt(0);
        secondSourceTickerPublicationConfig = (SourceTickerPublicationConfig)sourceTickerPublicationConfig.ElementAt(1);
        thirdSourceTickerPublicationConfig = (SourceTickerPublicationConfig)sourceTickerPublicationConfig.ElementAt(2);
    }

    public static SourceTickerPublicationConfigRepository SampleSourceTickerPublicationConfigRepo()
    {
        var firstSourceTickerPublicationConfig = SourceTickerPublicationConfigTests.DummySourceTickerPublicationConfig;
        var secondSourceTickerPublicationConfig = SourceTickerPublicationConfigTests.DummySourceTickerPublicationConfig;

        NonPublicInvocator.SetAutoPropertyInstanceField(secondSourceTickerPublicationConfig,
            (SourceTickerPublicationConfig x) => x.Ticker, "AnotherTestTicker");

        var thirdSourceTickerPublicationConfig = SourceTickerPublicationConfigTests.DummySourceTickerPublicationConfig;
        NonPublicInvocator.SetAutoPropertyInstanceField(thirdSourceTickerPublicationConfig,
            (SourceTickerPublicationConfig x) => x.Ticker, "YetAnotherTestTicker");

        var sourceTickerPublicationConfig = new SourceTickerPublicationConfigRepository(new[]
        {
            firstSourceTickerPublicationConfig, secondSourceTickerPublicationConfig, thirdSourceTickerPublicationConfig
        });
        return sourceTickerPublicationConfig;
    }

    [TestMethod]
    public void InitializingRepoFromEnumerable_Constructor_ReturnsPassedInValues()
    {
        var srcTkrEnumerator = sourceTickerPublicationConfig.GetEnumerator();
        Assert.IsTrue(srcTkrEnumerator.MoveNext());
        Assert.AreEqual(firstSourceTickerPublicationConfig, srcTkrEnumerator.Current);
        Assert.IsTrue(srcTkrEnumerator.MoveNext());
        Assert.AreEqual(secondSourceTickerPublicationConfig, srcTkrEnumerator.Current);
        Assert.IsTrue(srcTkrEnumerator.MoveNext());
        Assert.AreEqual(thirdSourceTickerPublicationConfig, srcTkrEnumerator.Current);
        Assert.IsFalse(srcTkrEnumerator.MoveNext());
        srcTkrEnumerator.Dispose();
    }
}
