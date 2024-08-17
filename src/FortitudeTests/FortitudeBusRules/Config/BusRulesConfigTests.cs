// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

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
            = new BusRulesConfig(config, "BusRulesConfig");

        Assert.AreEqual(2, busRulesConfig.QueuesConfig.MinEventQueues);
        Assert.AreEqual(10, busRulesConfig.QueuesConfig.MaxEventQueues);
        Assert.AreEqual(5, busRulesConfig.QueuesConfig.RequiredNetworkInboundQueues);
        Assert.AreEqual(1, busRulesConfig.QueuesConfig.RequiredNetworkOutboundQueues);
        Assert.AreEqual(3, busRulesConfig.QueuesConfig.MinWorkerQueues);
        Assert.AreEqual(7, busRulesConfig.QueuesConfig.MaxWorkerQueues);
        Assert.AreEqual(3000, busRulesConfig.QueuesConfig.DefaultQueueSize);
        Assert.AreEqual(9000, busRulesConfig.QueuesConfig.EventQueueSize);
        Assert.AreEqual(30u, busRulesConfig.QueuesConfig.EmptyEventQueueSleepMs);
        Assert.AreEqual(9U, busRulesConfig.QueuesConfig.DefaultEmptyQueueSleepMs);
        var clusterConfig          = busRulesConfig.ClusterConfig;
        var clusterConnectEndpoint = clusterConfig!.ClusterConnectivityEndpoint!;
        Assert.AreEqual(ActivationState.OnStartup, clusterConnectEndpoint.ActivationState);
        var clusterConnectServiceStartConConfig = clusterConnectEndpoint.ClusterServiceEndpoint!.ServiceStartConnectionConfig;
        Assert.AreEqual("TestClusterConnectionManager", clusterConnectServiceStartConConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor, clusterConnectServiceStartConConfig.ConversationProtocol);
        var clusterConnectStartConConfigFirstEndpoint = clusterConnectServiceStartConConfig.AvailableConnections.First();
        Assert.AreEqual("0.0.0.0", clusterConnectStartConConfigFirstEndpoint.Hostname);
        Assert.AreEqual((ushort)7777, clusterConnectStartConConfigFirstEndpoint.Port);
        Assert.AreEqual("TestClusterConnectionManager", clusterConnectStartConConfigFirstEndpoint.InstanceName);
        Assert.AreEqual("Allows other Bus Rules instances to connect and communicate and send and receive remote topics"
                      , clusterConnectServiceStartConConfig.TopicDescription);
        var clusterConnectClientConConfig = clusterConnectEndpoint.ClusterServiceEndpoint.ClusterAccessibleClientConnectionConfig;
        Assert.AreEqual("TestClusterConnectionManager", clusterConnectClientConConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpClient, clusterConnectClientConConfig.ConversationProtocol);
        var clusterConnectClientConConfigFirstEndpoint = clusterConnectClientConConfig.AvailableConnections.First();
        Assert.AreEqual("ServiceHostname", clusterConnectClientConConfigFirstEndpoint.Hostname);
        Assert.AreEqual((ushort)7777, clusterConnectClientConConfigFirstEndpoint.Port);
        Assert.AreEqual("TestClusterConnectionManager", clusterConnectClientConConfigFirstEndpoint.InstanceName);
        Assert.AreEqual("Allows other Bus Rules instances to connect and communicate and send and receive remote topics"
                      , clusterConnectClientConConfig.TopicDescription);
        var firstRemoteServiceConfig = clusterConfig!.RemoteServiceConfigs.First();
        Assert.AreEqual("FirstRemoteHostService", firstRemoteServiceConfig.Name);
        Assert.AreEqual("ClusterComms", firstRemoteServiceConfig.StreamType);
        Assert.AreEqual(ActivationState.OnStartup, firstRemoteServiceConfig.ActivationState);
        Assert.AreEqual("First Service Provides some service to this bus rules instance", firstRemoteServiceConfig.Description);
        Assert.AreEqual("MyCompany.MyProject.MyInitiatorServiceClassName", firstRemoteServiceConfig.ClientInitiatorFullClassName);
        var firstRemoteServiceConnConfig = firstRemoteServiceConfig.RemoteServiceConnectionConfigs.First();
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
        Assert.AreEqual(ActivationState.OnEachRequest, lastRemoteServiceConfig.ActivationState);
        Assert.AreEqual("Second Service Provides pricing", lastRemoteServiceConfig.Description);
        Assert.AreEqual("MyCompany.MyDifferentProject.MyInitiatorServiceClassName", lastRemoteServiceConfig.ClientInitiatorFullClassName);
        var lastRemoteServiceConnConfig = lastRemoteServiceConfig.RemoteServiceConnectionConfigs.First();
        Assert.AreEqual("SecondRemoteHostTopic", lastRemoteServiceConnConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpClient, lastRemoteServiceConnConfig.ConversationProtocol);
        var lastRemoteServiceConnFirstEndpointConfig = lastRemoteServiceConnConfig.AvailableConnections.First();
        Assert.AreEqual("SecondRemoteHostName", lastRemoteServiceConnFirstEndpointConfig.Hostname);
        Assert.AreEqual((ushort)2222, lastRemoteServiceConnFirstEndpointConfig.Port);
        Assert.AreEqual("SomeOtherInstance", lastRemoteServiceConnFirstEndpointConfig.InstanceName);
        Assert.AreEqual("provides order execution services"
                      , lastRemoteServiceConnConfig.TopicDescription);
        Assert.AreEqual(CustomConfigType.ConfigSectionPath, lastRemoteServiceConfig.ServiceCustomConfig!.CustomConfigType);
        Assert.AreEqual("FirstChild:ThirdGrandChild:CustomConfig", lastRemoteServiceConfig.ServiceCustomConfig.Content);
        var firstAddLocalServiceConfig = clusterConfig!.LocalServiceConfigs.First();
        Assert.AreEqual("FirstAdditionalServiceProvidedToOtherClients", firstAddLocalServiceConfig.Name);
        Assert.AreEqual("SomeKnownType", firstAddLocalServiceConfig.StreamType);
        Assert.AreEqual(ActivationState.OnStartup, firstAddLocalServiceConfig.ActivationState);
        Assert.AreEqual("MyCompany.MyProject.MyServiceInitatorClassName", firstAddLocalServiceConfig.ServiceInitiatorFullClassName);
        var firstAddLocalServiceConfigFirstEndpoint = firstAddLocalServiceConfig.Endpoints.First();
        Assert.AreEqual("FirstAdditionalServiceProvidedToOtherClients"
                      , firstAddLocalServiceConfigFirstEndpoint.ServiceStartConnectionConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor
                      , firstAddLocalServiceConfigFirstEndpoint.ServiceStartConnectionConfig.ConversationProtocol);
        var firstAddLocalServiceConnFirstEndpointConfig
            = firstAddLocalServiceConfigFirstEndpoint.ServiceStartConnectionConfig.AvailableConnections.First();
        Assert.AreEqual("0.0.0.0", firstAddLocalServiceConnFirstEndpointConfig.Hostname);
        Assert.AreEqual((ushort)5555, firstAddLocalServiceConnFirstEndpointConfig.Port);
        Assert.AreEqual("NameOfServiceInstance", firstAddLocalServiceConnFirstEndpointConfig.InstanceName);
        Assert.AreEqual("Provides well known service to clients connecting on this endpoint"
                      , firstAddLocalServiceConfigFirstEndpoint.ServiceStartConnectionConfig.TopicDescription);
        Assert.AreEqual("FirstAdditionalServiceProvidedToOtherClients"
                      , firstAddLocalServiceConfigFirstEndpoint.ClusterAccessibleClientConnectionConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpClient
                      , firstAddLocalServiceConfigFirstEndpoint.ClusterAccessibleClientConnectionConfig.ConversationProtocol);
        var firstAddLocalClientConnFirstEndpointConfig
            = firstAddLocalServiceConfigFirstEndpoint.ClusterAccessibleClientConnectionConfig.AvailableConnections.First();
        Assert.AreEqual("169.254.224.238", firstAddLocalClientConnFirstEndpointConfig.Hostname);
        Assert.AreEqual((ushort)5555, firstAddLocalClientConnFirstEndpointConfig.Port);
        Assert.AreEqual("NameOfServiceInstance", firstAddLocalClientConnFirstEndpointConfig.InstanceName);
        Assert.AreEqual("Provides well known service to clients connecting on this endpoint"
                      , firstAddLocalServiceConfigFirstEndpoint.ServiceStartConnectionConfig.TopicDescription);
        Assert.AreEqual(CustomConfigType.Json, firstAddLocalServiceConfig.ServiceCustomConfig!.CustomConfigType);
        Assert.AreEqual("{\"Key1\": \"Value1\", \"Key2\": \"Value2\" }", firstAddLocalServiceConfig.ServiceCustomConfig.Content);
        var lastAddLocalServiceConfig = clusterConfig!.LocalServiceConfigs.Last();
        Assert.AreEqual("SecondAdditionalServiceProvidedToOtherClients", lastAddLocalServiceConfig.Name);
        Assert.AreEqual("SomeOtherKnownType", lastAddLocalServiceConfig.StreamType);
        Assert.AreEqual(ActivationState.Disabled, lastAddLocalServiceConfig.ActivationState);
        Assert.AreEqual("MyCompany.MyOtherProject.MyServiceInitatorClassName", lastAddLocalServiceConfig.ServiceInitiatorFullClassName);
        var lastAddLocalServiceConnConfigFirstEndpoint = lastAddLocalServiceConfig.Endpoints.First();
        Assert.AreEqual("SecondAdditionalServiceProvidedToOtherClients"
                      , lastAddLocalServiceConnConfigFirstEndpoint.ServiceStartConnectionConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.UdpPublisher
                      , lastAddLocalServiceConnConfigFirstEndpoint.ServiceStartConnectionConfig.ConversationProtocol);
        var lastAddLocalServiceConnFirstEndpointConfig
            = lastAddLocalServiceConnConfigFirstEndpoint.ServiceStartConnectionConfig.AvailableConnections.First();
        Assert.AreEqual("169.254.224.238", lastAddLocalServiceConnFirstEndpointConfig.Hostname);
        Assert.AreEqual((ushort)6666, lastAddLocalServiceConnFirstEndpointConfig.Port);
        Assert.AreEqual("224.1.0.222", lastAddLocalServiceConnFirstEndpointConfig.SubnetMask);
        Assert.AreEqual("OtherNameOfServiceInstance", lastAddLocalServiceConnFirstEndpointConfig.InstanceName);
        Assert.AreEqual("Provides well known service", lastAddLocalServiceConnConfigFirstEndpoint.ServiceStartConnectionConfig.TopicDescription);
        Assert.AreEqual("LastAdditionalServiceProvidedToOtherClients"
                      , lastAddLocalServiceConnConfigFirstEndpoint.ClusterAccessibleClientConnectionConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.UdpSubscriber
                      , lastAddLocalServiceConnConfigFirstEndpoint.ClusterAccessibleClientConnectionConfig.ConversationProtocol);
        var lastAddLocalServiceConnLastEndpointConfig
            = lastAddLocalServiceConnConfigFirstEndpoint.ClusterAccessibleClientConnectionConfig.AvailableConnections.First();
        Assert.AreEqual("169.254.224.238", lastAddLocalServiceConnLastEndpointConfig.Hostname);
        Assert.AreEqual((ushort)6666, lastAddLocalServiceConnLastEndpointConfig.Port);
        Assert.AreEqual("224.1.0.222", lastAddLocalServiceConnLastEndpointConfig.SubnetMask);
        Assert.AreEqual("OtherNameOfServiceInstance", lastAddLocalServiceConnLastEndpointConfig.InstanceName);
        Assert.AreEqual("Provides well known service"
                      , lastAddLocalServiceConnConfigFirstEndpoint.ClusterAccessibleClientConnectionConfig.TopicDescription);
        Assert.AreEqual(CustomConfigType.StringContent, lastAddLocalServiceConfig.ServiceCustomConfig!.CustomConfigType);
        Assert.AreEqual("SingleStringValue", lastAddLocalServiceConfig.ServiceCustomConfig.Content);
    }
}
