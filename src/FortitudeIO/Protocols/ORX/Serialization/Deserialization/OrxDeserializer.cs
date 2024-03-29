﻿#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public sealed class OrxDeserializer<Tm> : MessageDeserializer<Tm> where Tm : class, IVersionedMessage, new()
{
    private readonly OrxByteDeserializer<Tm> orxByteDeserializer;

    public OrxDeserializer(IRecycler recyclingFactory) =>
        orxByteDeserializer = new OrxByteDeserializer<Tm>(new OrxDeserializerLookup(recyclingFactory));

    public override Tm Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        var tradingMessage = orxByteDeserializer.Deserialize(readContext);
        if (readContext is ReadSocketBufferContext sockBuffContext)
        {
            if (sockBuffContext.Session != null)
                Dispatch(tradingMessage, sockBuffContext.MessageHeader!,
                    sockBuffContext.Session, sockBuffContext.DispatchLatencyLogger);
            else if (sockBuffContext.LegacySession != null)
                Dispatch(tradingMessage, sockBuffContext.MessageHeader!,
                    sockBuffContext.LegacySession, sockBuffContext.DispatchLatencyLogger);
            else if (sockBuffContext.SessionContext != null)
                Dispatch(tradingMessage, sockBuffContext.MessageHeader!,
                    sockBuffContext.Conversation, sockBuffContext.DispatchLatencyLogger);
        }
        else if (readContext is IBufferContext bufferContext)
        {
            var messageHeader = bufferContext.ReadBasicMessageHeader();
            Dispatch(tradingMessage, messageHeader);
        }
        else
        {
            throw new ArgumentException("Expected readContext to be of type IBufferContext");
        }

        return tradingMessage;
    }
}
