// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeTests.FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;

[TestClass]
public class PricingServerConfigTests
{
    private PricingServerConfig pricingServerConfig = null!;

    public static PricingServerConfig DummyPricingServerConfig =>
        new(
            new NetworkTopicConnectionConfig("SnapshotTopicName", SocketConversationProtocol.TcpAcceptor
                                           , new[] { new EndpointConfig("SnapshotHostName", 2222) }),
            new NetworkTopicConnectionConfig("UpdateTopicName", SocketConversationProtocol.UdpPublisher
                                           , new[] { new EndpointConfig("UpdateHostName", 2244) }));


    [TestInitialize]
    public void SetUp()
    {
        pricingServerConfig = DummyPricingServerConfig;
    }


    [TestMethod]
    public void When_Cloned_NewButEqualConfig()
    {
        var cloneIdLookup = pricingServerConfig.Clone();
        Assert.AreNotSame(cloneIdLookup, pricingServerConfig);
        Assert.AreEqual(pricingServerConfig, cloneIdLookup);
    }
}
