#region

using System.Reactive.Subjects;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeTests.FortitudeCommon.Configuration.Availability;
using FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap;
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
    public void InitializedSnapUpdateServerConfig_NewConfigSameIdThroughUpdateStream_UpdatesSnapUpdateValues()
    {
        var originalSnapshotServerConnectionConfig = ConnectionConfigTests
            .ServerConnectionConfigWithValues("OriginalSnapshotConnectionName", "OriginalSnapshotHostName", 5678,
                ConnectionDirectionType.Both, "OriginalSnapshotNetworkSubAddress", 125U);

        var originalUpdateServerConnectionConfig = ConnectionConfigTests
            .ServerConnectionConfigWithValues("OriginalUpdateConnectionName", "OriginalUpdateHostName", 5679,
                ConnectionDirectionType.Publisher, "OriginalUpdateNetworkSubAddress", 125U);

        ISubject<IMarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>> updatePump =
            new Subject<IMarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>>();
        var updateAbleServerConfig = new SnapshotUpdatePricingServerConfig("OriginalServerName",
            MarketServerType.ConfigServer,
            new[] { originalSnapshotServerConnectionConfig, originalUpdateServerConnectionConfig },
            TimeTableTests.DummyTimeTable, 123,
            SourceTickerPublicationConfigTests.SampleSourceTickerPublicationConfigs, true, false, updatePump);

        Assert.AreEqual("OriginalServerName", updateAbleServerConfig.Name);
        Assert.AreEqual(MarketServerType.ConfigServer, updateAbleServerConfig.MarketServerType);
        Assert.AreEqual(123, updateAbleServerConfig.PublicationId);
        Assert.IsTrue(SourceTickerPublicationConfigTests.SampleSourceTickerPublicationConfigs
            .SequenceEqual(updateAbleServerConfig.SourceTickerPublicationConfigs!));
        Assert.IsTrue(updateAbleServerConfig.IsLastLook);
        Assert.IsFalse(updateAbleServerConfig.SupportsIceBergs);
        Assert.IsTrue(
            new[] { originalSnapshotServerConnectionConfig, originalUpdateServerConnectionConfig }.SequenceEqual(
                updateAbleServerConfig.ServerConnections!));
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.First(),
            "OriginalSnapshotConnectionName", "OriginalSnapshotHostName", 5678, ConnectionDirectionType.Both,
            "OriginalSnapshotNetworkSubAddress", 125U);
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.SnapshotConnectionConfig!,
            "OriginalSnapshotConnectionName", "OriginalSnapshotHostName", 5678, ConnectionDirectionType.Both,
            "OriginalSnapshotNetworkSubAddress", 125U);
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.Last(),
            "OriginalUpdateConnectionName", "OriginalUpdateHostName", 5679, ConnectionDirectionType.Publisher,
            "OriginalUpdateNetworkSubAddress", 125U);
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.UpdateConnectionConfig!,
            "OriginalUpdateConnectionName", "OriginalUpdateHostName", 5679, ConnectionDirectionType.Publisher,
            "OriginalUpdateNetworkSubAddress", 125U);

        var newSnapshotServerConnectionConfig = ConnectionConfigTests
            .ServerConnectionConfigWithValues("NewSnapshotConnectionName", "NewSnapshotHostName", 6789,
                ConnectionDirectionType.Both, "NewSnapshotNetworkSubAddress", 250U);
        NonPublicInvocator.SetInstanceProperty(newSnapshotServerConnectionConfig,
            ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.Id),
            originalSnapshotServerConnectionConfig.Id, true);
        var newUpdateServerConnectionConfig = ConnectionConfigTests
            .ServerConnectionConfigWithValues("NewUpdateConnectionName", "NewUpdateHostName", 8901,
                ConnectionDirectionType.Publisher, "NewUpdateNetworkSubAddress", 250U);
        NonPublicInvocator.SetInstanceProperty(newUpdateServerConnectionConfig,
            ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.Id),
            originalUpdateServerConnectionConfig.Id, true);

        var updatedConfig = updateAbleServerConfig.Clone();
        Assert.AreNotSame(updatedConfig, updateAbleServerConfig);
        var updatedTickerInfo = SourceTickerPublicationConfigTests.SampleSourceTickerPublicationConfigs.Take(2)
            .Concat(new[] { SourceTickerPublicationConfigTests.DummySourceTickerPublicationConfig })
            .Select(stpc => (ISourceTickerPublicationConfig)stpc).ToList();
        SourceTickerPublicationConfigTests.UpdateSourceTickerPublicationConfig(updatedTickerInfo.Last(),
            "UpdatedSourceName", "UpdatedTicker", 0.000005m, 1m, 10000000m, 1, 1000, 10,
            LayerFlags.Price | LayerFlags.Volume, LastTradedFlags.None, updatedConfig,
            NameIdLookupGeneratorTests.Dummy3NameIdLookup);

        UpdateServerConfigWithValues((SnapshotUpdatePricingServerConfig)updatedConfig, "NewServerName",
            MarketServerType.Trading, new[] { newSnapshotServerConnectionConfig, newUpdateServerConnectionConfig }, 234,
            updatedTickerInfo, true, false);

        Assert.AreEqual(updateAbleServerConfig.Id, updatedConfig.Id);

        updatePump.OnNext(new MarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>(updatedConfig,
            EventType.Updated));

        Assert.AreEqual("NewServerName", updateAbleServerConfig.Name);
        Assert.AreEqual(MarketServerType.Trading, updateAbleServerConfig.MarketServerType);
        Assert.AreEqual(234, updateAbleServerConfig.PublicationId);
        Assert.IsTrue(updatedTickerInfo
            .SequenceEqual(updateAbleServerConfig.SourceTickerPublicationConfigs!));
        Assert.IsTrue(updateAbleServerConfig.IsLastLook);
        Assert.IsFalse(updateAbleServerConfig.SupportsIceBergs);
        Assert.IsTrue(
            new[] { newSnapshotServerConnectionConfig, newUpdateServerConnectionConfig }.SequenceEqual(
                updateAbleServerConfig.ServerConnections!));
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.First(),
            "NewSnapshotConnectionName", "NewSnapshotHostName", 6789, ConnectionDirectionType.Both,
            "NewSnapshotNetworkSubAddress", 250U);
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.SnapshotConnectionConfig!,
            "NewSnapshotConnectionName", "NewSnapshotHostName", 6789, ConnectionDirectionType.Both,
            "NewSnapshotNetworkSubAddress", 250U);
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.Last(),
            "NewUpdateConnectionName", "NewUpdateHostName", 8901, ConnectionDirectionType.Publisher,
            "NewUpdateNetworkSubAddress", 250U);
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.UpdateConnectionConfig!,
            "NewUpdateConnectionName", "NewUpdateHostName", 8901, ConnectionDirectionType.Publisher,
            "NewUpdateNetworkSubAddress", 250U);
    }

    [TestMethod]
    public void When_Cloned_NewButEqualConfig()
    {
        var cloneIdLookup = snapshotUpdatePricingServerConfig.Clone();
        Assert.AreNotSame(cloneIdLookup, snapshotUpdatePricingServerConfig);
        Assert.AreEqual(snapshotUpdatePricingServerConfig, cloneIdLookup);
    }
}
