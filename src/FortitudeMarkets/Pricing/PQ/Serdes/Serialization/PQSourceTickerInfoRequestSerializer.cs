// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

public class PQSourceTickerInfoRequestSerializer : IMessageSerializer<PQSourceTickerInfoRequest>
{
    private const int FixedSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(uint);

    public MarshalType MarshalType => MarshalType.Binary;

    public bool AddMessageHeader { get; set; } = true;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQSourceTickerInfoRequest)message, (ISerdeContext)writeContext);
    }

    public void Serialize(PQSourceTickerInfoRequest obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected readContext to support writing");
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

    public unsafe int Serialize(IBuffer buffer, PQSourceTickerInfoRequest sourceTickerInfoRequest)
    {
        using var fixedBuffer = buffer;

        var   writeStart  = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var   currPtr     = writeStart;
        byte* messageSize = null;

        if (AddMessageHeader)
        {
            *currPtr++ = sourceTickerInfoRequest.Version;
            *currPtr++ = (byte)Messages.FeedEvents.Quotes.PQMessageFlags.None;
            StreamByteOps.ToBytes(ref currPtr, sourceTickerInfoRequest.MessageId);
            messageSize =  currPtr;
            currPtr     += sizeof(uint);
        }
        StreamByteOps.ToBytes(ref currPtr, sourceTickerInfoRequest.RequestId);
        var amtWritten = currPtr - writeStart;
        if (AddMessageHeader) StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);
        sourceTickerInfoRequest.DecrementRefCount();
        return (int)amtWritten;
    }
}
