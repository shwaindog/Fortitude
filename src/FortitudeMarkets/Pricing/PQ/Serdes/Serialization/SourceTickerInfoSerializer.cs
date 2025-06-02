// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

public class SourceTickerInfoSerializer : IMessageSerializer<ISourceTickerInfo>
{
    private readonly List<KeyValuePair<string, string>> instrumentAttributes = new();

    public MarshalType MarshalType => MarshalType.Binary;

    public bool AddMessageHeader { get; set; } = true;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((ISourceTickerInfo)message, (ISerdeContext)writeContext);
    }

    public void Serialize(ISourceTickerInfo obj, ISerdeContext writeContext)
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

    public unsafe int Serialize(IBuffer buffer, ISourceTickerInfo message)
    {
        buffer.LimitNextSerialize = byte.MaxValue;

        using var fixedBuffer = buffer;

        var remainingBytes = buffer.RemainingStorage;
        if (MessageHeader.SerializationSize > remainingBytes) return -1;

        var   writeStart  = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var   ptr         = writeStart;
        byte* messageSize = null;

        if (AddMessageHeader)
        {
            *ptr++ = message.Version;
            *ptr++ = (byte)Messages.FeedEvents.Quotes.PQMessageFlags.None; // header flags
            StreamByteOps.ToBytes(ref ptr, message.MessageId);
            messageSize =  ptr;
            ptr         += sizeof(uint);
        }

        SerializeSourceTickerInfo(message, ref ptr, fixedBuffer, instrumentAttributes);
        var amtWritten = ptr - writeStart;
        if (AddMessageHeader) StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);
        message.DecrementRefCount();
        return (int)amtWritten;
    }

    public static unsafe void SerializeSourceTickerInfo
        (ISourceTickerInfo message, ref byte* ptr, IBuffer fixedBuffer, List<KeyValuePair<string, string>> instrumentAttributes)
    {
        PricingInstrumentSerializer.SerializePricingInstrument(message, ref ptr, fixedBuffer, instrumentAttributes);
        SerializeQuoteInfo(message, ref ptr);
    }

    private static unsafe void SerializeQuoteInfo(ISourceTickerInfo message, ref byte* ptr)
    {
        *ptr++ = (byte)message.PublishedTickerQuoteDetailLevel;
        StreamByteOps.ToBytes(ref ptr, message.MaximumPublishedLayers);
        var booleanValues = message.GetSourceTickerInfoBooleanFlagsEnum();
        StreamByteOps.ToBytes(ref ptr, message.RoundingPrecision);
        StreamByteOps.ToBytes(ref ptr, message.Pip);
        StreamByteOps.ToBytes(ref ptr, (uint)booleanValues);
        StreamByteOps.ToBytes(ref ptr, message.MinSubmitSize);
        StreamByteOps.ToBytes(ref ptr, message.MaxSubmitSize);
        StreamByteOps.ToBytes(ref ptr, message.IncrementSize);
        StreamByteOps.ToBytes(ref ptr, message.MinimumQuoteLife);
        StreamByteOps.ToBytes(ref ptr, message.DefaultMaxValidMs);
        StreamByteOps.ToBytes(ref ptr, (uint)message.LayerFlags);
        StreamByteOps.ToBytes(ref ptr, (ushort)message.LastTradedFlags);
    }
}
