#region

using FortitudeBusRules.Config;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeTests.FortitudeBusRules.Config;

[TestClass]
public class BusRulesConfigTests
{
    [TestMethod]
    public void LoadJsonFile_BusRulesConfig_ContainsValuesInFile()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("FortitudeBusRules/Config/TestBusRulesConfigLoads.json")
            .Build();
        var busRulesConfig
            = new BusRulesConfig(config, BusRulesConfig.DefaultBusRulesConfigPath);

        Assert.AreEqual(2, busRulesConfig.QueuesConfig.MinEventQueues);
        Assert.AreEqual(10, busRulesConfig.QueuesConfig.MaxEventQueues);
        Assert.AreEqual(5, busRulesConfig.QueuesConfig.RequiredIOInboundQueues);
        Assert.AreEqual(1, busRulesConfig.QueuesConfig.RequiredIOOutboundQueues);
        Assert.AreEqual(3, busRulesConfig.QueuesConfig.MinWorkerQueues);
        Assert.AreEqual(7, busRulesConfig.QueuesConfig.MaxWorkerQueues);
        Assert.AreEqual(3000, busRulesConfig.QueuesConfig.DefaultQueueSize);
        Assert.AreEqual(9000, busRulesConfig.QueuesConfig.EventQueueSize);
        Assert.AreEqual(9, busRulesConfig.QueuesConfig.MessagePumpMaxWaitMs);
        var clusterConfig = busRulesConfig.ClusterConfig;
        var clusterConnectEndpoint = clusterConfig!.ClusterConnectivityEndpoint;
        Assert.AreEqual("TestClusterConnectionManager", clusterConnectEndpoint.Name);
        Assert.AreEqual("ClusterComms", clusterConnectEndpoint.StreamType);
        Assert.AreEqual("localhost", clusterConnectEndpoint.ClusterAccessibleHostname);
        Assert.AreEqual((ushort)7777, clusterConnectEndpoint.ClusterAccessiblePort);
        var clusterConnectServiceStart = clusterConnectEndpoint.ServiceStartConnectionConfig;
        Assert.AreEqual("TestClusterConnectionManager", clusterConnectServiceStart.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor, clusterConnectServiceStart.ConversationProtocol);
        var clusterConnectStartFirstEndpoint = clusterConnectServiceStart.AvailableConnections.First();
        Assert.AreEqual("0.0.0.0", clusterConnectStartFirstEndpoint.Hostname);
        Assert.AreEqual((ushort)7777, clusterConnectStartFirstEndpoint.Port);
        Assert.AreEqual("TestClusterConnectionManager", clusterConnectStartFirstEndpoint.InstanceName);
        Assert.AreEqual("Allows other Bus Rules instances to connect and communicate and send and receive remote topics"
            , clusterConnectServiceStart.TopicDescription);
        var firstRemoteServiceConfig = clusterConfig!.RemoteServiceConfigs.First();
        Assert.AreEqual("FirstRemoteHostService", firstRemoteServiceConfig.Name);
        Assert.AreEqual("ClusterComms", firstRemoteServiceConfig.StreamType);
        Assert.AreEqual("First Service Provides some service to this bus rules instance", firstRemoteServiceConfig.Description);
        var firstRemoteServiceConnConfig = firstRemoteServiceConfig.RemoteServiceConnectionConfig;
        Assert.AreEqual("FirstRemoteHostTopic", firstRemoteServiceConnConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpClient, firstRemoteServiceConnConfig.ConversationProtocol);
        var firstRemoteServiceConnFirstEndpointConfig = firstRemoteServiceConnConfig.AvailableConnections.First();
        Assert.AreEqual("FirstRemoteHostName", firstRemoteServiceConnFirstEndpointConfig.Hostname);
        Assert.AreEqual((ushort)1111, firstRemoteServiceConnFirstEndpointConfig.Port);
        Assert.AreEqual("TestClusterConnectionManager", firstRemoteServiceConnFirstEndpointConfig.InstanceName);
        Assert.AreEqual("Connects this Bus Rules instance to a remote cluster"
            , firstRemoteServiceConnConfig.TopicDescription);
        var lastRemoteServiceConfig = clusterConfig!.RemoteServiceConfigs.Last();
        Assert.AreEqual("SecondRemoteHostService", lastRemoteServiceConfig.Name);
        Assert.AreEqual("OrdersExecution", lastRemoteServiceConfig.StreamType);
        Assert.AreEqual("Second Service Provides pricing", lastRemoteServiceConfig.Description);
        var lastRemoteServiceConnConfig = lastRemoteServiceConfig.RemoteServiceConnectionConfig;
        Assert.AreEqual("SecondRemoteHostTopic", lastRemoteServiceConnConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpClient, lastRemoteServiceConnConfig.ConversationProtocol);
        var lastRemoteServiceConnFirstEndpointConfig = lastRemoteServiceConnConfig.AvailableConnections.First();
        Assert.AreEqual("SecondRemoteHostName", lastRemoteServiceConnFirstEndpointConfig.Hostname);
        Assert.AreEqual((ushort)2222, lastRemoteServiceConnFirstEndpointConfig.Port);
        Assert.AreEqual("SomeOtherInstance", lastRemoteServiceConnFirstEndpointConfig.InstanceName);
        Assert.AreEqual("provides order execution services"
            , lastRemoteServiceConnConfig.TopicDescription);
        var firstAddLocalServiceConfig = clusterConfig!.AdditionalLocalServices.First();
        Assert.AreEqual("FirstAdditionalServiceProvidedToOtherClients", firstAddLocalServiceConfig.Name);
        Assert.AreEqual("SomeKnownType", firstAddLocalServiceConfig.StreamType);
        Assert.AreEqual("localhost", firstAddLocalServiceConfig.ClusterAccessibleHostname);
        Assert.AreEqual((ushort)5555, firstAddLocalServiceConfig.ClusterAccessiblePort);
        var firstAddLocalServiceConnConfig = firstAddLocalServiceConfig.ServiceStartConnectionConfig;
        Assert.AreEqual("FirstAdditionalServiceProvidedToOtherClients", firstAddLocalServiceConnConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor, firstAddLocalServiceConnConfig.ConversationProtocol);
        var firstAddLocalServiceConnFirstEndpointConfig = firstAddLocalServiceConnConfig.AvailableConnections.First();
        Assert.AreEqual("0.0.0.0", firstAddLocalServiceConnFirstEndpointConfig.Hostname);
        Assert.AreEqual((ushort)5555, firstAddLocalServiceConnFirstEndpointConfig.Port);
        Assert.AreEqual("NameOfServiceInstance", firstAddLocalServiceConnFirstEndpointConfig.InstanceName);
        Assert.AreEqual("Provides well known service to clients connecting on this endpoint"
            , firstAddLocalServiceConnConfig.TopicDescription);
        var lastAddLocalServiceConfig = clusterConfig!.AdditionalLocalServices.Last();
        Assert.AreEqual("SecondAdditionalServiceProvidedToOtherClients", lastAddLocalServiceConfig.Name);
        Assert.AreEqual("SomeOtherKnownType", lastAddLocalServiceConfig.StreamType);
        Assert.AreEqual("localhost", lastAddLocalServiceConfig.ClusterAccessibleHostname);
        Assert.AreEqual((ushort)6666, lastAddLocalServiceConfig.ClusterAccessiblePort);
        var lastAddLocalServiceConnConfig = lastAddLocalServiceConfig.ServiceStartConnectionConfig;
        Assert.AreEqual("SecondAdditionalServiceProvidedToOtherClients", lastAddLocalServiceConnConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor, lastAddLocalServiceConnConfig.ConversationProtocol);
        var lastAddLocalServiceConnFirstEndpointConfig = lastAddLocalServiceConnConfig.AvailableConnections.First();
        Assert.AreEqual("0.0.0.0", lastAddLocalServiceConnFirstEndpointConfig.Hostname);
        Assert.AreEqual((ushort)6666, lastAddLocalServiceConnFirstEndpointConfig.Port);
        Assert.AreEqual("OtherNameOfServiceInstance", lastAddLocalServiceConnFirstEndpointConfig.InstanceName);
        Assert.AreEqual("Provides well known service", lastAddLocalServiceConnConfig.TopicDescription);
    }
}
