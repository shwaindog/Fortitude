#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

internal class PQSourceTickerInfoResponseSerializer : IMessageSerializer<PQSourceTickerInfoResponse>
{
    private const int FixedSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(uint);

    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQSourceTickerInfoResponse)message, (ISerdeContext)writeContext);
    }

    public void Serialize(PQSourceTickerInfoResponse obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer!.Buffer, bufferContext.EncodedBuffer.WriteCursor
                , obj);
            if (writeLength > 0) bufferContext.EncodedBuffer.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage message)
    {
        var quoteInfos = ((PQSourceTickerInfoResponse)message).SourceTickerQuoteInfos;
        var remainingBytes = buffer.Length - writeOffset;
        if (quoteInfos != null && FixedSize + quoteInfos.Count * sizeof(uint) <= buffer.Length - writeOffset)
            fixed (byte* bufStrt = buffer)
            {
                var writeStart = bufStrt + writeOffset;
                var currPtr = writeStart;
                *currPtr++ = message.Version;
                *currPtr++ = (byte)PQMessageFlags.None; // header flags
                StreamByteOps.ToBytes(ref currPtr, message.MessageId);
                var messageSize = currPtr;
                currPtr += OrxConstants.UInt32Sz;
                remainingBytes -= FixedSize;
                StreamByteOps.ToBytes(ref currPtr, (ushort)quoteInfos.Count);
                remainingBytes -= 2;
                for (var i = 0; i < quoteInfos.Count; i++)
                {
                    var toSerialize = quoteInfos[i];
                    var bytesWritten = Serialize(currPtr, toSerialize, remainingBytes);
                    currPtr += bytesWritten;
                    remainingBytes -= bytesWritten;
                }

                var amtWritten = currPtr - writeStart;
                StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);
                message.DecrementRefCount();
                return (int)amtWritten;
            }

        return -1;
    }

    private unsafe int Serialize(byte* currPtr, ISourceTickerQuoteInfo sourceTickerQuoteInfo, int remainingBytes)
    {
        var writeStart = currPtr;
        StreamByteOps.ToBytes(ref currPtr, sourceTickerQuoteInfo.SourceId);
        StreamByteOps.ToBytes(ref currPtr, sourceTickerQuoteInfo.TickerId);
        StreamByteOps.ToBytes(ref currPtr, sourceTickerQuoteInfo.RoundingPrecision);
        StreamByteOps.ToBytes(ref currPtr, sourceTickerQuoteInfo.MinSubmitSize);
        StreamByteOps.ToBytes(ref currPtr, sourceTickerQuoteInfo.MaxSubmitSize);
        StreamByteOps.ToBytes(ref currPtr, sourceTickerQuoteInfo.IncrementSize);
        StreamByteOps.ToBytes(ref currPtr, sourceTickerQuoteInfo.MinimumQuoteLife);
        StreamByteOps.ToBytes(ref currPtr, (uint)sourceTickerQuoteInfo.LayerFlags);
        *currPtr++ = sourceTickerQuoteInfo.MaximumPublishedLayers;
        StreamByteOps.ToBytes(ref currPtr, (ushort)sourceTickerQuoteInfo.LastTradedFlags);
        StreamByteOps.ToBytesWithSizeHeader(ref currPtr, sourceTickerQuoteInfo.Source, remainingBytes);
        StreamByteOps.ToBytesWithSizeHeader(ref currPtr, sourceTickerQuoteInfo.Ticker, remainingBytes);
        return (int)(currPtr - writeStart);
    }
}
