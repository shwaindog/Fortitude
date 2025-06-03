// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQStorageCandleDeserializerTests
{
    private BufferContext bufferContext = null!;

    private PQStorageCandleDeserializer deserializer = new();

    private List<ICandle> originalSummaries = null!;

    private PQStorageCandle pqStorageCandle = null!;
    private CircularReadWriteBuffer     readWriteBuffer             = null!;

    private PQStorageCandleSerializer snapshotSerializer = new(PQMessageFlags.Snapshot);
    private PQStorageCandleSerializer updateSerializer   = new(PQMessageFlags.Update);


    [TestInitialize]
    public void SetUp()
    {
        readWriteBuffer = new CircularReadWriteBuffer(new byte[9000]);
        bufferContext   = new BufferContext(readWriteBuffer);

        pqStorageCandle = new PQStorageCandle();

        snapshotSerializer = new PQStorageCandleSerializer(PQMessageFlags.Snapshot);
        updateSerializer   = new PQStorageCandleSerializer(PQMessageFlags.Update);

        originalSummaries = new List<ICandle>(6);
        var startDateTime = new DateTime(2024, 6, 17, 16, 0, 0);
        originalSummaries.Add
            (CreateCandle(startDateTime, OneMinute, 1.234m, 0.0002m, 1.2361m, 0.048m, 60, 30_000));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreateCandle(startDateTime, OneMinute, 1.2361m, 0.0002m, 1.234m, 0.089m, 20, 2_000_000));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreateCandle(startDateTime, OneMinute, 1.234m, 0.0002m, 1.2311m, 0.022m, 20, 50));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreateCandle(startDateTime, OneMinute, 1.2320m, 0.0002m, 1.2366m, 0.030m, 2_000_000, 30));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreateCandle(startDateTime, OneMinute, 1.2366m, 0.0002m, 1.2361m, 0.010m, 60, 30_000));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreateCandle(startDateTime, OneMinute, 1.2361m, 0.0002m, 1.234m, 0.048m, 60, 200_000));
    }


    [TestMethod]
    public void SerializeAllCandles_Deserialize_ReturnsSameValues()
    {
        SerializeCandle(originalSummaries[0], snapshotSerializer);
        foreach (var candle in originalSummaries.Skip(1)) SerializeCandle(candle, updateSerializer);
        for (var i = 0; i < originalSummaries.Count; i++)
        {
            var original     = originalSummaries[i];
            var deserialized = deserializer.Deserialize(bufferContext);

            Assert.IsTrue(deserialized!.AreEquivalent(original), $"Expected {deserialized} to be equivalent to {original}");
        }
    }


    [TestMethod]
    public void SerializeWithMissingPeriod_Deserialize_ReturnsSameValues()
    {
        originalSummaries.RemoveAt(1);
        SerializeCandle(originalSummaries[0], snapshotSerializer);
        foreach (var candle in originalSummaries.Skip(1)) SerializeCandle(candle, updateSerializer);
        for (var i = 0; i < originalSummaries.Count; i++)
        {
            var original     = originalSummaries[i];
            var deserialized = deserializer.Deserialize(bufferContext);

            Assert.IsTrue(deserialized!.AreEquivalent(original));
        }
    }

    private void SerializeCandle(ICandle candle, ISerializer<IPQStorageCandle> serializer)
    {
        pqStorageCandle.HasUpdates = false;
        pqStorageCandle.CopyFrom(candle);
        Assert.IsTrue(pqStorageCandle.AreEquivalent(candle));
        serializer.Serialize(pqStorageCandle, bufferContext);
        var bytesSerialized = bufferContext.LastWriteLength;
        Assert.IsTrue(bytesSerialized > 0);
    }

    private ICandle CreateCandle
    (
        DateTime startTime, TimeBoundaryPeriod period, decimal startMid,
        decimal bidAskSpread, decimal endMid, decimal highLowSpread, uint tickCount, long volume)
    {
        var halfBidAskSpread   = bidAskSpread / 2;
        var averageStartEndMid = (startMid + endMid) / 2;
        var halfHighLowSpread  = highLowSpread / 2;

        var result =
            new Candle
                (period, startTime, period.PeriodEnd(startTime), startMid - halfBidAskSpread, startMid + halfBidAskSpread
               , averageStartEndMid + halfHighLowSpread - halfBidAskSpread, averageStartEndMid + halfHighLowSpread + halfBidAskSpread
               , averageStartEndMid - halfHighLowSpread - halfBidAskSpread, averageStartEndMid - halfHighLowSpread + halfBidAskSpread,
                 endMid - halfBidAskSpread, endMid + halfBidAskSpread, tickCount, volume, averageStartEndMid - halfBidAskSpread,
                 averageStartEndMid + halfBidAskSpread);
        return result;
    }
}
