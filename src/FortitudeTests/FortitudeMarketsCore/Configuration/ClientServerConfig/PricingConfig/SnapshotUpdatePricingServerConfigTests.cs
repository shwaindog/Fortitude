#region

using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeTests.FortitudeIO.Transports.Sockets;
using FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;

[TestClass]
public class SnapshotUpdatePricingServerConfigTests
{
    private SnapshotUpdatePricingServerConfig snapshotUpdatePricingServerConfig = null!;

    public static SnapshotUpdatePricingServerConfig DummySnapshotUpdatePricingServerConfig =>
        new("TestSnapshotName", MarketServerType.MarketData,
            new[] { ConnectionConfigTests.DummyConnectionConfig }, null, 1234,
            Enumerable.Empty<ISourceTickerPublicationConfig>(), true, false);

    public static void UpdateServerConfigWithValues(ISnapshotUpdatePricingServerConfig updatedConfig,
        string serverName, MarketServerType marketServerType, IEnumerable<IConnectionConfig> serverConnectionConfigs,
        ushort publicationId, IList<ISourceTickerPublicationConfig> sourceTicker, bool isLastLook,
        bool supportsIceBerges)
    {
        MarketServerConfigTests.UpdateServerConfigWithValues(
            updatedConfig, "NewServerName",
            MarketServerType.Trading, serverConnectionConfigs);

        NonPublicInvocator.SetInstanceProperty(updatedConfig,
            ReflectionHelper.GetPropertyName((SnapshotUpdatePricingServerConfig x) => x.PublicationId),
            publicationId, true);
        NonPublicInvocator.SetInstanceProperty(updatedConfig,
            ReflectionHelper.GetPropertyName((SnapshotUpdatePricingServerConfig x) => x.SourceTickerPublicationConfigs),
            sourceTicker, true);
        NonPublicInvocator.SetInstanceProperty(updatedConfig,
            ReflectionHelper.GetPropertyName((SnapshotUpdatePricingServerConfig x) => x.IsLastLook),
            isLastLook, true);
        NonPublicInvocator.SetInstanceProperty(updatedConfig,
            ReflectionHelper.GetPropertyName((SnapshotUpdatePricingServerConfig x) => x.SupportsIceBergs),
            supportsIceBerges, true);
    }

    [TestInitialize]
    public void SetUp()
    {
        snapshotUpdatePricingServerConfig = DummySnapshotUpdatePricingServerConfig;
    }


    [TestMethod]
    public void When_Cloned_NewButEqualConfig()
    {
        var cloneIdLookup = snapshotUpdatePricingServerConfig.Clone();
        Assert.AreNotSame(cloneIdLookup, snapshotUpdatePricingServerConfig);
        Assert.AreEqual(snapshotUpdatePricingServerConfig, cloneIdLookup);
    }
}
