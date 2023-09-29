using System.Collections.Generic;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Serialization
{
    internal sealed class PQQuoteSerializer : IBinarySerializer<IPQLevel0Quote>
    {
        private readonly UpdateStyle updateStyle;
        // ReSharper disable once UnusedMember.Local
        private IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteSerializer));
        public PQQuoteSerializer(UpdateStyle updateStyle)
        {
            this.updateStyle = updateStyle;
        }

        private const int HeaderSize = 2 * sizeof(byte) + sizeof(ushort)
            + sizeof(ushort) + sizeof(uint) + sizeof(uint);
        private const int FieldSize = 2 * sizeof(byte) + sizeof(uint);

        public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage message)
        {
            var publishAll = (updateStyle & UpdateStyle.FullSnapshot) > 0;
            if ((publishAll ? sizeof(uint) : 0) + HeaderSize > buffer.Length - writeOffset) return -1;
            if (!(message is IPQLevel0Quote level0Quote)) return -1;
            fixed (byte* fptr = buffer)
            {
                if (!publishAll && !level0Quote.HasUpdates)
                {
                    return 0;
                }
                byte* currentPtr = fptr + writeOffset;
                byte* end = fptr + buffer.Length;
                *(currentPtr++) = message.Version; //protocol version
                byte* messageFlags = currentPtr++;
                var flags = publishAll
                    ? (byte)PQBinaryMessageFlags.PublishAll 
                    : (byte)PQBinaryMessageFlags.None;
                byte* messageSizePtr = currentPtr; 
                currentPtr += 4;
                StreamByteOps.ToBytes(ref currentPtr, level0Quote.SourceTickerQuoteInfo.Id);
                byte* sequenceIdptr = currentPtr;                   
                currentPtr += 4;

                IEnumerable<PQFieldUpdate> fields =level0Quote.GetDeltaUpdateFields(TimeContext.UtcNow, 
                    updateStyle);
                level0Quote.Lock.Acquire();
                try
                {
                    foreach (PQFieldUpdate field in fields)
                    {
                        if (currentPtr + FieldSize > end)
                        {
                            return -1;
                        }
                        *(currentPtr++) = field.Flag;
                        if ((field.Flag & PQFieldFlags.IsExtendedFieldId) == 0)
                        {
                            * (currentPtr++) = (byte) field.Id;
                        }
                        else
                        {
                            flags |= (byte)PQBinaryMessageFlags.ExtendedFieldId;
                            StreamByteOps.ToBytes(ref currentPtr, field.Id);
                        }
                        StreamByteOps.ToBytes(ref currentPtr, field.Value);
                    }
                    IEnumerable<PQFieldStringUpdate> stringUpdates = level0Quote.GetStringUpdates
                        (TimeContext.UtcNow, updateStyle);
                    foreach (PQFieldStringUpdate fieldStringUpdate in stringUpdates)
                    {
                        if (currentPtr + 100 > end)
                        {
                            return -1;
                        }

                        flags |= (byte)PQBinaryMessageFlags.ContainsStringUpdate;
                        *(currentPtr++) = fieldStringUpdate.Field.Flag;
                        if ((fieldStringUpdate.Field.Flag & PQFieldFlags.IsExtendedFieldId) == 0)
                        {
                            *(currentPtr++) = (byte)fieldStringUpdate.Field.Id;
                        }
                        else
                        {
                            StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.Field.Id);
                        }
                        byte* stringSizePtr = currentPtr;
                        currentPtr += 4;
                        StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.StringUpdate.DictionaryId);
                        var bytesUsed = StreamByteOps.ToBytes(ref currentPtr, 
                            fieldStringUpdate.StringUpdate.Value, (int) (end - currentPtr));
                        StreamByteOps.ToBytes(ref stringSizePtr, (uint) bytesUsed);
                    }
                    if (publishAll)
                    {
                        StreamByteOps.ToBytes(ref sequenceIdptr, level0Quote.PQSequenceId == uint.MaxValue 
                            ? 0 
                            : level0Quote.PQSequenceId);
                    } 
                    else
                    {
                        StreamByteOps.ToBytes(ref sequenceIdptr, unchecked(++level0Quote.PQSequenceId));
                    }
                    level0Quote.HasUpdates = false;
                }
                finally
                {
                    level0Quote.Lock.Release();
                }

                *messageFlags = flags;
                int written = (int)(currentPtr - fptr - writeOffset);
                /*logger.Debug($"{TimeContext.LocalTimeNow:O} {level0Quote.SourceTickerQuoteInfo.Source}-" +
                                     $"{level0Quote.SourceTickerQuoteInfo.Ticker}:" +
                                     $"{level0Quote.PQSequenceId}-> wrote {written} bytes for " +
                                     $"{updateStyle}.  ThreadName {Thread.CurrentThread.Name}");*/
                StreamByteOps.ToBytes(ref messageSizePtr, (uint)written);
                return written;
            }
        }
    }
}
