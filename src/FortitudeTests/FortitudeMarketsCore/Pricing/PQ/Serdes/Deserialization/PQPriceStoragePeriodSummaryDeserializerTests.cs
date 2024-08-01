// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using FortitudeMarketsCore.Pricing.Summaries;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQPriceStoragePeriodSummaryDeserializerTests
{
    private BufferContext bufferContext = null!;

    private PQPriceStoragePeriodSummaryDeserializer deserializer = new();

    private List<IPricePeriodSummary> originalSummaries = null!;

    private PQPriceStoragePeriodSummary pqPriceStoragePeriodSummary = null!;
    private CircularReadWriteBuffer     readWriteBuffer             = null!;

    private PQPriceStoragePeriodSummarySerializer snapshotSerializer = new(StorageFlags.Snapshot);
    private PQPriceStoragePeriodSummarySerializer updateSerializer   = new(StorageFlags.Update);


    [TestInitialize]
    public void SetUp()
    {
        readWriteBuffer = new CircularReadWriteBuffer(new byte[9000]);
        bufferContext   = new BufferContext(readWriteBuffer);

        pqPriceStoragePeriodSummary = new PQPriceStoragePeriodSummary();

        snapshotSerializer = new PQPriceStoragePeriodSummarySerializer(StorageFlags.Snapshot);
        updateSerializer   = new PQPriceStoragePeriodSummarySerializer(StorageFlags.Update);

        originalSummaries = new List<IPricePeriodSummary>(6);
        var startDateTime = new DateTime(2024, 6, 17, 16, 0, 0);
        originalSummaries.Add
            (CreatePeriodSummary(startDateTime, OneMinute, 1.234m, 0.0002m, 1.2361m, 0.048m, 60, 30_000));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreatePeriodSummary(startDateTime, OneMinute, 1.2361m, 0.0002m, 1.234m, 0.089m, 20, 2_000_000));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreatePeriodSummary(startDateTime, OneMinute, 1.234m, 0.0002m, 1.2311m, 0.022m, 20, 50));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreatePeriodSummary(startDateTime, OneMinute, 1.2320m, 0.0002m, 1.2366m, 0.030m, 2_000_000, 30));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreatePeriodSummary(startDateTime, OneMinute, 1.2366m, 0.0002m, 1.2361m, 0.010m, 60, 30_000));
        startDateTime = startDateTime.AddMinutes(1);
        originalSummaries.Add
            (CreatePeriodSummary(startDateTime, OneMinute, 1.2361m, 0.0002m, 1.234m, 0.048m, 60, 200_000));
    }


    [TestMethod]
    public void SerializeAllSummaries_Deserialize_ReturnsSameValues()
    {
        SerializeSummary(originalSummaries[0], snapshotSerializer);
        foreach (var pricePeriodSummary in originalSummaries.Skip(1)) SerializeSummary(pricePeriodSummary, updateSerializer);
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
        SerializeSummary(originalSummaries[0], snapshotSerializer);
        foreach (var pricePeriodSummary in originalSummaries.Skip(1)) SerializeSummary(pricePeriodSummary, updateSerializer);
        for (var i = 0; i < originalSummaries.Count; i++)
        {
            var original     = originalSummaries[i];
            var deserialized = deserializer.Deserialize(bufferContext);

            Assert.IsTrue(deserialized!.AreEquivalent(original));
        }
    }

    private void SerializeSummary(IPricePeriodSummary priceSummary, ISerializer<IPQPriceStoragePeriodSummary> serializer)
    {
        pqPriceStoragePeriodSummary.HasUpdates = false;
        pqPriceStoragePeriodSummary.CopyFrom(priceSummary);
        Assert.IsTrue(pqPriceStoragePeriodSummary.AreEquivalent(priceSummary));
        serializer.Serialize(pqPriceStoragePeriodSummary, bufferContext);
        var bytesSerialized = bufferContext.LastWriteLength;
        Assert.IsTrue(bytesSerialized > 0);
    }

    private IPricePeriodSummary CreatePeriodSummary
    (
        DateTime startTime, TimeBoundaryPeriod period, decimal startMid,
        decimal bidAskSpread, decimal endMid, decimal highLowSpread, uint tickCount, long volume)
    {
        var halfBidAskSpread   = bidAskSpread / 2;
        var averageStartEndMid = (startMid + endMid) / 2;
        var halfHighLowSpread  = highLowSpread / 2;

        var result =
            new PricePeriodSummary
                (period, startTime, period.PeriodEnd(startTime), startMid - halfBidAskSpread, startMid + halfBidAskSpread
               , averageStartEndMid + halfHighLowSpread - halfBidAskSpread, averageStartEndMid + halfHighLowSpread + halfBidAskSpread
               , averageStartEndMid - halfHighLowSpread - halfBidAskSpread, averageStartEndMid - halfHighLowSpread + halfBidAskSpread,
                 endMid - halfBidAskSpread, endMid + halfBidAskSpread, tickCount, volume, averageStartEndMid - halfBidAskSpread,
                 averageStartEndMid + halfBidAskSpread);
        return result;
    }
}
