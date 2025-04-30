// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeIO.Transports.Network.Config;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter<SocketConnectionAttributes>))]
public enum SocketConnectionAttributes
{
    None               = 0x00
  , KeepAlive          = 0x01
  , Unicast            = 0x02
  , Multicast          = 0x04
  , Reliable           = 0x08
  , Fast               = 0x10
  , Replayable         = 0x20
  , TransportHeartBeat = 0x40
}
