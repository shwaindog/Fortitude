// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface IMessageSerializer
{
    bool AddMessageHeader { get; set; }
    void Serialize(IVersionedMessage message, IBufferContext writeContext);
}

public interface IMessageSerializer<T> : IMessageSerializer, ISerializer<T>
    where T : class, IVersionedMessage { }
