// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

internal class PQSourceTickerInfoResponseSerializer : IMessageSerializer<PQSourceTickerInfoResponse>
{
    private readonly List<KeyValuePair<string, string>> instrumentAttributes = new();

    public MarshalType MarshalType => MarshalType.Binary;

    public bool AddMessageHeader { get; set; } = true;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQSourceTickerInfoResponse)message, (ISerdeContext)writeContext);
    }

    public void Serialize(PQSourceTickerInfoResponse obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0) throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength                                               = Serialize(bufferContext.EncodedBuffer!, obj);
            if (writeLength > 0) bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, PQSourceTickerInfoResponse message)
    {
        var quoteInfos = message.SourceTickerInfos;

        using var fixedBuffer = buffer;

        var remainingBytes = buffer.RemainingStorage;

        if (MessageHeader.SerializationSize + quoteInfos.Count * sizeof(uint) > remainingBytes) return -1;

        var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var currPtr    = writeStart;

        byte* messageSize = null;
        if (AddMessageHeader)
        {
            *currPtr++ = message.Version;
            *currPtr++ = (byte)PQMessageFlags.None; // header flags
            StreamByteOps.ToBytes(ref currPtr, message.MessageId);
            messageSize =  currPtr;
            currPtr     += sizeof(uint);
        }
        remainingBytes -= MessageHeader.SerializationSize;
        StreamByteOps.ToBytes(ref currPtr, message.RequestId);
        StreamByteOps.ToBytes(ref currPtr, message.ResponseId);
        StreamByteOps.ToBytes(ref currPtr, (ushort)quoteInfos.Count);
        remainingBytes -= 10;
        for (var i = 0; i < quoteInfos.Count; i++)
        {
            var toSerialize  = quoteInfos[i];
            var bytesWritten = Serialize(currPtr, toSerialize, fixedBuffer);
            currPtr        += bytesWritten;
            remainingBytes -= bytesWritten;
        }

        var amtWritten = currPtr - writeStart;
        if (AddMessageHeader) StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);
        message.DecrementRefCount();
        return (int)amtWritten;
    }

    private unsafe int Serialize(byte* currPtr, ISourceTickerInfo sourceTickerInfo, IBuffer fixedBuffer)
    {
        var writeStart = currPtr;
        SourceTickerInfoSerializer.SerializeSourceTickerInfo(sourceTickerInfo, ref currPtr, fixedBuffer, instrumentAttributes);
        return (int)(currPtr - writeStart);
    }
}
