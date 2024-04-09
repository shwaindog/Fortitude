#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

internal sealed class PQHeartbeatSerializer : IMessageSerializer<PQHeartBeatQuotesMessage>
{
    private const int HeaderSize = 2 * sizeof(byte) + sizeof(uint);
    private const int HeartbeatSize = 2 * sizeof(uint);
    public MarshalType MarshalType => MarshalType.Binary;

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
            var writeLength = Serialize(bufferContext.EncodedBuffer!.Buffer, bufferContext.EncodedBuffer.WriteCursor
                , obj);
            bufferContext.EncodedBuffer.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage message)
    {
        if (HeaderSize <= buffer.Length - writeOffset)
            fixed (byte* fptr = buffer)
            {
                var ptr = fptr + writeOffset;
                var messageStart = ptr;
                var end = fptr + buffer.Length;
                var quotes = message as IEnumerable<IPQLevel0Quote>;
                if (quotes != null)
                    foreach (var quote in quotes)
                    {
                        *ptr++ = message.Version;
                        *ptr++ = (byte)PQMessageFlags.None;
                        StreamByteOps.ToBytes(ref ptr, quote.SourceTickerQuoteInfo!.Id);
                        var messageSize = ptr;
                        ptr += sizeof(uint);
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

                        StreamByteOps.ToBytes(ref messageSize
                            , PQQuoteMessageHeader.HeaderSize + sizeof(uint)); // just a heartbeat header
                    }

                message.DecrementRefCount();
                return (int)(ptr - messageStart);
            }

        return -1;
    }
}
