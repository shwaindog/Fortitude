﻿#region

using System.Reactive.Subjects;
using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeTests.FortitudeCommon.Configuration.Availability;
using FortitudeTests.FortitudeIO.Transports.Sockets;

#endregion

namespace FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig;

[TestClass]
public class MarketServerConfigTests
{
    public static MarketServerConfig<IDummyMarketServerConfig> DummyServerConfig =>
        new DummyMarketServerConfigClass("TestServerName", MarketServerType.MarketData,
            new[] { ConnectionConfigTests.DummyConnectionConfig },
            TimeTableTests.DummyTimeTable);

    public static IList<IDummyMarketServerConfig> ListOfSampleServerConfigs =>
        new List<IDummyMarketServerConfig>
        {
            new DummyMarketServerConfigClass("TestServerName1", MarketServerType.MarketData,
                new[] { ConnectionConfigTests.DummyConnectionConfig },
                TimeTableTests.DummyTimeTable)
            , new DummyMarketServerConfigClass("TestServerName2", MarketServerType.Trading,
                new[] { ConnectionConfigTests.DummyConnectionConfig },
                TimeTableTests.DummyTimeTable)
            , new DummyMarketServerConfigClass("TestServerName3", MarketServerType.ConfigServer,
                new[] { ConnectionConfigTests.DummyConnectionConfig },
                TimeTableTests.DummyTimeTable)
        };

    public static void UpdateServerConfigWithValues<T>(IMarketServerConfig<T> updateConfig, string name
        , MarketServerType marketServerType,
        IEnumerable<IConnectionConfig> serverConnectionConfigs) where T : class, IMarketServerConfig<T>
    {
        NonPublicInvocator.SetInstanceProperty(updateConfig,
            ReflectionHelper.GetPropertyName((MarketServerConfig<T> x) => x.Name),
            name, true);
        NonPublicInvocator.SetInstanceProperty(updateConfig,
            ReflectionHelper.GetPropertyName((MarketServerConfig<T> x) => x.MarketServerType),
            marketServerType, true);
        NonPublicInvocator.SetInstanceProperty(updateConfig,
            ReflectionHelper.GetPropertyName((MarketServerConfig<T> x) => x.ServerConnections),
            serverConnectionConfigs, true);
    }

    [TestMethod]
    public void InitializedServerConfig_NewConfigSameIdPushedThroughUpdateStream_UpdatesAllValues()
    {
        var originalServerConnectionConfig = ConnectionConfigTests
            .ServerConnectionConfigWithValues("OriginalConnectionName", "OriginalHostName", 5678,
                ConnectionDirectionType.Both, "OriginalNetworkSubAddress", 125U);

        ISubject<IMarketServerConfigUpdate<IDummyMarketServerConfig>> updatePump
            = new Subject<IMarketServerConfigUpdate<IDummyMarketServerConfig>>();
        var updateAbleServerConfig = new DummyMarketServerConfigClass("OriginalServerName"
            , MarketServerType.ConfigServer,
            new[] { originalServerConnectionConfig }, TimeTableTests.DummyTimeTable, updatePump);

        Assert.AreEqual("OriginalServerName", updateAbleServerConfig.Name);
        Assert.AreEqual(MarketServerType.ConfigServer, updateAbleServerConfig.MarketServerType);
        Assert.IsTrue(
            new[] { originalServerConnectionConfig }.SequenceEqual(updateAbleServerConfig.ServerConnections!));
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.First(),
            "OriginalConnectionName", "OriginalHostName", 5678, ConnectionDirectionType.Both,
            "OriginalNetworkSubAddress", 125U);

        var firstNewServerConnection = ConnectionConfigTests.DummyConnectionConfig!;
        var secondNewServerConnection = ConnectionConfigTests.DummyConnectionConfig;
        NonPublicInvocator.SetInstanceProperty(secondNewServerConnection,
            ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.Id),
            3344L, true);
        NonPublicInvocator.SetInstanceProperty(secondNewServerConnection,
            ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.ConnectionName),
            "SecondNewConnectionName", true);

        IDummyMarketServerConfig updatedConfig = new DummyMarketServerConfigClass(updateAbleServerConfig);
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
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.First(),
            firstNewServerConnection.ConnectionName, firstNewServerConnection.Hostname, firstNewServerConnection.Port
            , firstNewServerConnection.ConnectionDirectionType,
            firstNewServerConnection?.NetworkSubAddress, firstNewServerConnection!.ReconnectIntervalMs);
        ConnectionConfigTests.AssertIsExpected(updateAbleServerConfig.ServerConnections!.Last(),
            secondNewServerConnection.ConnectionName, secondNewServerConnection.Hostname, secondNewServerConnection.Port
            , secondNewServerConnection.ConnectionDirectionType,
            secondNewServerConnection?.NetworkSubAddress, secondNewServerConnection!.ReconnectIntervalMs);
    }

    public interface IDummyMarketServerConfig : IMarketServerConfig<IDummyMarketServerConfig>
    {
        new IDummyMarketServerConfig Clone();
    }

    private class DummyMarketServerConfigClass : MarketServerConfig<IDummyMarketServerConfig>, IDummyMarketServerConfig
    {
        public DummyMarketServerConfigClass(string name, MarketServerType marketServerType
            , IEnumerable<IConnectionConfig> serverConnections,
            ITimeTable availabilityTimeTable
            , IObservable<IMarketServerConfigUpdate<IDummyMarketServerConfig>>? repoUpdateStream = null)
            : base(name, marketServerType, serverConnections, availabilityTimeTable, repoUpdateStream) { }

        public DummyMarketServerConfigClass(DummyMarketServerConfigClass toClone) : base(toClone) { }

        public new IDummyMarketServerConfig Clone() => new DummyMarketServerConfigClass(this);

        object ICloneable.Clone() => Clone();
    }
}
