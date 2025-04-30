// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Net;
using FortitudeIO.Transports.Network.Config;

#endregion

namespace PublishingRecordingAndReplaying.Step_1;

public class VenueConfigRepository
{
    private IPAddress LoopbackAdapterBindingIpAddress;
    private ushort    StartTcpSnapshotPort          = 46080;
    private IPAddress StartUdpMulticastAddressRange = IPAddress.Parse("224.0.0.0"); // udp multicast range - 224.0.0.0 to 239.255.255.255


    public VenueConfigRepository()
    {
        LoopbackAdapterBindingIpAddress
            = IPAddress.Loopback; // on windows you can install "Microsoft KM-TEST Loopback Adapter" as by default loopback adapter is disabled
        Console.WriteLine("Loopback IP address : " + LoopbackAdapterBindingIpAddress);
    }


    public IPAddress GetTcpSnapshotIpAddress(Venues forVenue) => LoopbackAdapterBindingIpAddress;
    public ushort    GetTcpSnapshotPort(Venues forVenue)      => (ushort)(StartTcpSnapshotPort + (ushort)forVenue);


    public NetworkTopicConnectionConfig GetSnapshotTcpConnectionConfig(Venues forVenue) =>
        forVenue switch
        {
            Venues.ExampleFuturesExchange => new NetworkTopicConnectionConfig
                ($"TcpSnapshotServerFor_{forVenue}", SocketConversationProtocol.TcpAcceptor
               , new List<IEndpointConfig>
                 {
                     new EndpointConfig(LoopbackAdapterBindingIpAddress.ToString()
                                      , GetTcpSnapshotPort(forVenue))
                 }, $"TcpSnapshotServerFor_{forVenue}")
          , _ => throw new ArgumentException($"Unexpected venue {forVenue} received")
        };
}
