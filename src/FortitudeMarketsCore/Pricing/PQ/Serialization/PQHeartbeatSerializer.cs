using System;
using System.Collections.Generic;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Serialization
{
    internal sealed class PQHeartbeatSerializer : IBinarySerializer<PQLevel0Quote>
    {
        private const int HeaderSize = 2 * sizeof(byte) + sizeof(uint);
        private const int HeartbeatSize = 2 * sizeof(uint);

        public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage message)
        {
            if (HeaderSize <= buffer.Length - writeOffset)
            {
                fixed (byte* fptr = buffer)
                {
                    byte* ptr = fptr + writeOffset;
                    byte* messageStart = ptr;
                    byte* end = fptr + buffer.Length;

                    *ptr++ = message.Version;                                                   
                    *ptr++ = (byte)PQBinaryMessageFlags.IsHeartBeat;                                                          

                    IEnumerable<IPQLevel0Quote> quotes = message as IEnumerable<IPQLevel0Quote>;
                    byte* messageSize = ptr;
                    ptr += sizeof(uint);

                    if (quotes != null)
                    {
                        foreach (IPQLevel0Quote quote in quotes)
                        {
                            if (ptr + HeartbeatSize > end)
                            {
                                return -1;
                            }
                            quote.Lock.Acquire();
                            try
                            {
                                StreamByteOps.ToBytes(ref ptr, quote.SourceTickerQuoteInfo.Id);
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
                    }

                    StreamByteOps.ToBytes(ref messageSize, (uint)(ptr - messageStart));

                    return (int)(ptr - messageStart);
                }
            }
            return -1;
        }
    }
}
