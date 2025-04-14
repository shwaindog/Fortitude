// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeIO.Transports.Network.Config;

[JsonConverter(typeof(JsonStringEnumConverter<SocketConversationProtocol>))]
public enum SocketConversationProtocol
{
    Unknown
  , TcpClient
  , TcpAcceptor
  , UdpPublisher
  , UdpSubscriber
}
