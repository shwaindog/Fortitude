#region

using System.Reactive.Subjects;
using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeTests.FortitudeCommon.Configuration.Availability;
using FortitudeTests.FortitudeIO.Transports.Network.Config;

#endregion

namespace FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig;

[TestClass]
public class MarketServerConfigTests
{
    public static MarketServerConfig<IDummyMarketServerConfig> DummyServerConfig =>
        new DummyMarketServerConfigClass("TestServerName", MarketServerType.MarketData,
            new[] { NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig },
            TimeTableTests.DummyTimeTable);

    public static IList<IDummyMarketServerConfig> ListOfSampleServerConfigs =>
        new List<IDummyMarketServerConfig>
        {
            new DummyMarketServerConfigClass("TestServerName1", MarketServerType.MarketData,
                new[] { NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig },
                TimeTableTests.DummyTimeTable)
            , new DummyMarketServerConfigClass("TestServerName2", MarketServerType.Trading,
                new[] { NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig },
                TimeTableTests.DummyTimeTable)
            , new DummyMarketServerConfigClass("TestServerName3", MarketServerType.ConfigServer,
                new[] { NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig },
                TimeTableTests.DummyTimeTable)
        };

    public static void UpdateServerConfigWithValues<T>(IMarketServerConfig<T> updateConfig, string name
        , MarketServerType marketServerType,
        IEnumerable<INetworkTopicConnectionConfig> serverConnectionConfigs) where T : class, IMarketServerConfig<T>
    {
        NonPublicInvocator.SetInstanceProperty(updateConfig,
            ReflectionHelper.GetPropertyName((MarketServerConfig<T> x) => x.Name),
            name, true);
        NonPublicInvocator.SetInstanceProperty(updateConfig,
            ReflectionHelper.GetPropertyName((MarketServerConfig<T> x) => x.MarketServerType),
            marketServerType, true);
        NonPublicInvocator.SetInstanceProperty(updateConfig,
            ReflectionHelper.GetPropertyName((MarketServerConfig<T> x) =>
                x.ServerConnections),
            serverConnectionConfigs!, true);
    }

    [TestMethod]
    public void InitializedServerConfig_NewConfigSameIdPushedThroughUpdateStream_UpdatesAllValues()
    {
        var originalServerConnectionConfig = NetworkTopicConnectionConfigTests
            .ServerConnectionConfigWithValues("OriginalConnectionName", "OriginalHostName", 5678,
                "127.0.0.1", 125U);

        ISubject<IMarketServerConfigUpdate<IDummyMarketServerConfig>> updatePump
            = new Subject<IMarketServerConfigUpdate<IDummyMarketServerConfig>>();
        var updateAbleServerConfig = new DummyMarketServerConfigClass("OriginalServerName"
            , MarketServerType.ConfigServer,
            new[] { originalServerConnectionConfig }, TimeTableTests.DummyTimeTable, updatePump);

        Assert.AreEqual("OriginalServerName", updateAbleServerConfig.Name);
        Assert.AreEqual(MarketServerType.ConfigServer, updateAbleServerConfig.MarketServerType);
        Assert.IsTrue(
            new[] { originalServerConnectionConfig }.SequenceEqual(updateAbleServerConfig.ServerConnections!));
        NetworkTopicConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.First(),
            "OriginalConnectionName", "OriginalConnectionName", "OriginalHostName", 5678, "127.0.0.1");

        var firstNewServerConnection = NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig;
        var secondNewServerConnection = NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig;
        NonPublicInvocator.SetInstanceProperty(secondNewServerConnection,
            ReflectionHelper.GetPropertyName((NetworkTopicConnectionConfig x) => x.TopicName),
            "New", true);
        NonPublicInvocator.SetInstanceProperty(secondNewServerConnection,
            ReflectionHelper.GetPropertyName((NetworkTopicConnectionConfig x) => x.TopicDescription),
            "New", true);

        IDummyMarketServerConfig updatedConfig = new DummyMarketServerConfigClass(updateAbleServerConfig, false);
        Assert.AreNotSame(updatedConfig, updateAbleServerConfig);
        UpdateServerConfigWithValues(updatedConfig, "NewServerName", MarketServerType.Trading,
            new[] { firstNewServerConnection, secondNewServerConnection });
        Assert.AreEqual(updateAbleServerConfig.Id, updatedConfig.Id);

        updatePump.OnNext(new MarketServerConfigUpdate<IDummyMarketServerConfig>(updatedConfig, EventType.Updated));

        Assert.AreEqual("NewServerName", updateAbleServerConfig.Name);
        Assert.AreEqual(MarketServerType.Trading, updateAbleServerConfig.MarketServerType);
        Assert.IsTrue(
            new[] { firstNewServerConnection, secondNewServerConnection }.SequenceEqual(updateAbleServerConfig
                .ServerConnections!));
        NetworkTopicConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.First(),
            firstNewServerConnection.TopicName!, firstNewServerConnection.TopicDescription!
            , firstNewServerConnection.Current.Hostname, firstNewServerConnection.Current.Port
            , firstNewServerConnection.Current.SubnetMask);
        NetworkTopicConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.Last(),
            secondNewServerConnection.TopicName!, secondNewServerConnection.TopicDescription!
            , secondNewServerConnection.Current.Hostname, secondNewServerConnection.Current.Port
            , secondNewServerConnection.Current.SubnetMask);
    }

    public interface IDummyMarketServerConfig : IMarketServerConfig<IDummyMarketServerConfig>
    {
        new IDummyMarketServerConfig Clone();
    }

    private class DummyMarketServerConfigClass : MarketServerConfig<IDummyMarketServerConfig>, IDummyMarketServerConfig
    {
        public DummyMarketServerConfigClass(string name, MarketServerType marketServerType
            , IEnumerable<INetworkTopicConnectionConfig> serverConnections,
            ITimeTable availabilityTimeTable
            , IObservable<IMarketServerConfigUpdate<IDummyMarketServerConfig>>? repoUpdateStream = null)
            : base(name, marketServerType, serverConnections, availabilityTimeTable, repoUpdateStream) { }

        public DummyMarketServerConfigClass(DummyMarketServerConfigClass toClone, bool toggleProtocolDirection) :
            base(toClone, toggleProtocolDirection) { }

        public new IDummyMarketServerConfig Clone() => new DummyMarketServerConfigClass(this, false);

        public override IDummyMarketServerConfig ToggleProtocolDirection() => new DummyMarketServerConfigClass(this, true);

        object ICloneable.Clone() => Clone();
    }
}
