// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

internal sealed class PQHeartbeatSerializer : IMessageSerializer<PQHeartBeatQuotesMessage>
{
    private const int HeaderSize    = 2 * sizeof(byte) + sizeof(uint);
    private const int HeartbeatSize = 2 * sizeof(uint);

    public MarshalType MarshalType => MarshalType.Binary;

    public bool AddMessageHeader { get; set; } = true;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQHeartBeatQuotesMessage)message, (ISerdeContext)writeContext);
    }

    public void Serialize(PQHeartBeatQuotesMessage obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer!, obj);
            bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength            =  writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, IVersionedMessage message)
    {
        using var fixedBuffer = buffer;
        if (HeaderSize > buffer.RemainingStorage) return -1;

        var ptr          = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var messageStart = ptr;
        var end          = ptr + buffer.RemainingStorage;
        if (message is IEnumerable<IPQLevel0Quote> quotes)
            foreach (var quote in quotes)
            {
                byte* messageSize = null;
                if (AddMessageHeader)
                {
                    *ptr++ = message.Version;
                    *ptr++ = (byte)PQMessageFlags.None;
                    StreamByteOps.ToBytes(ref ptr, quote.SourceTickerQuoteInfo!.Id);
                    messageSize = ptr;

                    ptr += sizeof(uint);
                }
                quote.Lock.Acquire();
                try
                {
                    StreamByteOps.ToBytes(ref ptr, unchecked(++quote.PQSequenceId));
                }
                finally
                {
                    quote.Lock.Release();
                }

                if (ptr + HeartbeatSize > end) return -1;

                if (AddMessageHeader)
                    StreamByteOps.ToBytes(ref messageSize
                                        , PQQuoteMessageHeader.HeaderSize + sizeof(uint)); // just a heartbeat header
            }

        message.DecrementRefCount();
        return (int)(ptr - messageStart);
    }
}
