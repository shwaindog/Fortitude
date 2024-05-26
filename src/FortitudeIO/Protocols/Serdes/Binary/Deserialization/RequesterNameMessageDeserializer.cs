// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Deserialization;

public class RequesterNameMessageDeserializer : MessageDeserializer<RequesterNameMessage>
{
    private readonly IRecycler recycler;

    public RequesterNameMessageDeserializer(IRecycler recycler) => this.recycler = recycler;

    public RequesterNameMessageDeserializer(RequesterNameMessageDeserializer toClone) => recycler = toClone.recycler;

    public override unsafe RequesterNameMessage Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var deserializedRequesterNameMessage = recycler.Borrow<RequesterNameMessage>();
            var fixedBuffer                      = messageBufferContext.EncodedBuffer!;
            var ptr                              = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            deserializedRequesterNameMessage.RequesterConnectionName = StreamByteOps.ToStringWithSizeHeader(ref ptr)!;

            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(deserializedRequesterNameMessage, messageBufferContext);
            return deserializedRequesterNameMessage;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public override IMessageDeserializer Clone() => new RequesterNameMessageDeserializer(this);
}
