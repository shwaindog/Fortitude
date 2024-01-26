#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization;

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
            var writeLength = Serialize(bufferContext.EncodedBuffer!.Buffer, bufferContext.EncodedBuffer.WrittenCursor
                , obj);
            bufferContext.EncodedBuffer.WrittenCursor += writeLength;
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

                *ptr++ = message.Version;
                *ptr++ = (byte)PQBinaryMessageFlags.IsHeartBeat;

                var quotes = message as IEnumerable<IPQLevel0Quote>;
                var messageSize = ptr;
                ptr += sizeof(uint);

                if (quotes != null)
                    foreach (var quote in quotes)
                    {
                        if (ptr + HeartbeatSize > end) return -1;
                        quote.Lock.Acquire();
                        try
                        {
                            StreamByteOps.ToBytes(ref ptr, quote.SourceTickerQuoteInfo!.Id);
                            StreamByteOps.ToBytes(ref ptr, unchecked(++quote.PQSequenceId));
                        }
                        finally
                        {
                            quote.Lock.Release();
                        }
                        /*Console.Out.WriteLine($"{TimeContext.LocalTimeNow:O} {quote.SourceTickerQuoteInfo.Source}-" +
                                              $"{quote.SourceTickerQuoteInfo.Ticker}:" +
                                              $"{quote.PQSequenceId} -> heartbeat");*/
                    }

                StreamByteOps.ToBytes(ref messageSize, (uint)(ptr - messageStart));
                message.DecrementRefCount();

                return (int)(ptr - messageStart);
            }

        return -1;
    }
}
