// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

public class PricingInstrumentSerializer : ISerializer<IPricingInstrumentId>
{
    private readonly List<KeyValuePair<string, string>> instrumentAttributes = new();

    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IPricingInstrumentId obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0) throw new ArgumentException("Expected readContext to support writing");
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

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((ISourceTickerInfo)message, (ISerdeContext)writeContext);
    }

    public unsafe int Serialize(IBuffer buffer, IPricingInstrumentId message)
    {
        buffer.LimitNextSerialize = byte.MaxValue;

        using var fixedBuffer = buffer;

        var remainingBytes = buffer.RemainingStorage;
        if (MessageHeader.SerializationSize > remainingBytes) return -1;

        var writeStart  = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var ptr         = writeStart;
        var messageSize = writeStart;
        ptr += 2;
        SerializePricingInstrument(message, ref ptr, fixedBuffer, instrumentAttributes);
        var amtWritten = ptr - writeStart;
        StreamByteOps.ToBytes(ref messageSize, (ushort)amtWritten);

        message.DecrementRefCount();
        return (int)amtWritten;
    }

    public static unsafe void SerializePricingInstrument
        (IPricingInstrumentId message, ref byte* ptr, IBuffer fixedBuffer, List<KeyValuePair<string, string>> instrumentAttributes)
    {
        instrumentAttributes.Clear();
        instrumentAttributes.AddRange(message.AllAttributes.Where(kvp => !kvp.Key.Contains("Market")));
        var remainingBytes = fixedBuffer.RemainingStorage;
        StreamByteOps.ToBytes(ref ptr, message.SourceId);
        StreamByteOps.ToBytes(ref ptr, message.InstrumentId);
        StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, message.SourceName, Math.Min(remainingBytes, 255));
        StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, message.InstrumentName, Math.Min(remainingBytes, 255));
        StreamByteOps.ToBytes(ref ptr, message.MarketClassification.CompoundedClassification);
        *ptr++ = (byte)message.InstrumentType;
        StreamByteOps.ToBytes(ref ptr, (uint)message.CoveringPeriod.Period);
        *ptr++ = (byte)instrumentAttributes.Count;
        foreach (var instrumentAttribute in instrumentAttributes)
        {
            StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, instrumentAttribute.Key, Math.Min(remainingBytes, 255));
            StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, instrumentAttribute.Value, Math.Min(remainingBytes, 255));
        }
    }
}
