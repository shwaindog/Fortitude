#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

public class SourceTickerQuoteInfoSerializer : IMessageSerializer<ISourceTickerQuoteInfo>
{
    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((ISourceTickerQuoteInfo)message, (ISerdeContext)writeContext);
    }

    public void Serialize(ISourceTickerQuoteInfo obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer!, obj);
            if (writeLength > 0) bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, ISourceTickerQuoteInfo message)
    {
        using var fixedBuffer = buffer;
        var remainingBytes = buffer.RemainingStorage;
        if (MessageHeader.SerializationSize > remainingBytes) return -1;
        var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var currPtr = writeStart;
        *currPtr++ = message.Version;
        *currPtr++ = (byte)PQMessageFlags.None; // header flags
        StreamByteOps.ToBytes(ref currPtr, message.MessageId);
        var messageSize = currPtr;
        remainingBytes -= MessageHeader.SerializationSize;
        remainingBytes -= 10;

        StreamByteOps.ToBytes(ref currPtr, message.SourceId);
        StreamByteOps.ToBytes(ref currPtr, message.TickerId);
        *currPtr++ = (byte)message.PublishedQuoteLevel;
        StreamByteOps.ToBytes(ref currPtr, message.RoundingPrecision);
        StreamByteOps.ToBytes(ref currPtr, message.MinSubmitSize);
        StreamByteOps.ToBytes(ref currPtr, message.MaxSubmitSize);
        StreamByteOps.ToBytes(ref currPtr, message.IncrementSize);
        StreamByteOps.ToBytes(ref currPtr, message.MinimumQuoteLife);
        StreamByteOps.ToBytes(ref currPtr, (uint)message.LayerFlags);
        *currPtr++ = message.MaximumPublishedLayers;
        StreamByteOps.ToBytes(ref currPtr, (ushort)message.LastTradedFlags);
        StreamByteOps.ToBytesWithAutoSizeHeader(ref currPtr, message.Source, remainingBytes);
        StreamByteOps.ToBytesWithAutoSizeHeader(ref currPtr, message.Ticker, remainingBytes);
        var amtWritten = currPtr - writeStart;
        StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);
        message.DecrementRefCount();
        return (int)amtWritten;
    }
}
