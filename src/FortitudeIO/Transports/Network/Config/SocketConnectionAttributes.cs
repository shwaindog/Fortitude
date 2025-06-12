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

public static class SocketConnectionAttributesExtensions
{
    public static bool HasKeepAliveFlag(this SocketConnectionAttributes flags) => (flags & SocketConnectionAttributes.KeepAlive) > 0;
    public static bool HasUnicastFlag(this SocketConnectionAttributes flags) => (flags & SocketConnectionAttributes.Unicast) > 0;
    public static bool HasMulticastFlag(this SocketConnectionAttributes flags) => (flags & SocketConnectionAttributes.Multicast) > 0;
    public static bool HasReliableFlag(this SocketConnectionAttributes flags) => (flags & SocketConnectionAttributes.Reliable) > 0;
    public static bool HasFastFlag(this SocketConnectionAttributes flags) => (flags & SocketConnectionAttributes.Fast) > 0;
    public static bool HasReplayableFlag(this SocketConnectionAttributes flags) => (flags & SocketConnectionAttributes.Replayable) > 0;
    public static bool HasTransportHeartBeatFlag(this SocketConnectionAttributes flags) => (flags & SocketConnectionAttributes.TransportHeartBeat) > 0;
}