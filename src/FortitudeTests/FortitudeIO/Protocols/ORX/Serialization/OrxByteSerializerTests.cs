#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeTests.FortitudeIO.Protocols.ORX.Serialization;

[TestClass]
public class OrxByteSerializerTests
{
    private const int BufferSize = 2048;
    private byte[] byteBuffer = Array.Empty<byte>();
    private SocketBufferReadContext socketBufferReadContext = new();

    [TestInitialize]
    public void SetUp()
    {
        byteBuffer = new byte[BufferSize];

        socketBufferReadContext = new SocketBufferReadContext
        {
            EncodedBuffer = new ReadWriteBuffer(byteBuffer)
            , DispatchLatencyLogger = new PerfLogger("", TimeSpan.MaxValue, ""), MessageVersion = 1
        };
    }

    [TestMethod]
    public void DoubleStringArrayType_Serializes_DeserializesIsSame()
    {
        var orxDoubleStringArraySerializer = new OrxByteSerializer<DoubleStringArray>();
        var originalDoubleStringArray = new DoubleStringArray
        {
            FirstStringArray = new[] { "First1", "First2", "First3" }
            , SecondStringArray = new[] { "Second1", "Second2", "Second3" }
        };

        socketBufferReadContext.MessageSize = orxDoubleStringArraySerializer.Serialize(originalDoubleStringArray,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orxDoubleStringArrayDeserializer = new OrxByteDeserializer<DoubleStringArray>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedDblStrArry = orxDoubleStringArrayDeserializer.Deserialize(socketBufferReadContext);

        Assert.IsTrue(originalDoubleStringArray.FirstStringArray
            .SequenceEqual(deserializedDblStrArry.FirstStringArray!));
        Assert.IsTrue(originalDoubleStringArray.SecondStringArray
            .SequenceEqual(deserializedDblStrArry.SecondStringArray!));
    }

    [TestMethod]
    public void LongsType_Serializes_DeserializesIsSame()
    {
        var orxLongsSerializer = new OrxByteSerializer<Longs>();
        var originalDoubleStringArray = new Longs
        {
            FirstLong = long.MaxValue, SecondLong = long.MinValue, ThirdLong = long.MaxValue / 2
            , FourthLong = long.MinValue / 2
        };

        socketBufferReadContext.MessageSize = orxLongsSerializer.Serialize(originalDoubleStringArray,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orxLongsDeserializer = new OrxByteDeserializer<Longs>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedLongs = orxLongsDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(long.MaxValue, deserializedLongs.FirstLong);
        Assert.AreEqual(long.MinValue, deserializedLongs.SecondLong);
        Assert.AreEqual(long.MaxValue / 2, deserializedLongs.ThirdLong);
        Assert.AreEqual(long.MinValue / 2, deserializedLongs.FourthLong);
    }

    [TestMethod]
    public void StringsType_Serializes_DeserializesIsSame()
    {
        var orxStringsSerializer = new OrxByteSerializer<Strings>();
        var originalDoubleStringArray = new Strings
        {
            FirstString = "FirstString", SecondString = "SecondString", ThirdString = "ThirdString"
            , FourthString = "FourthString"
        };

        socketBufferReadContext.MessageSize = orxStringsSerializer.Serialize(originalDoubleStringArray,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orxStringsDeserializer = new OrxByteDeserializer<Strings>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedStrings = orxStringsDeserializer
            .Deserialize(socketBufferReadContext);

        Assert.AreEqual("FirstString", deserializedStrings.FirstString);
        Assert.AreEqual("SecondString", deserializedStrings.SecondString);
        Assert.AreEqual("ThirdString", deserializedStrings.ThirdString);
        Assert.AreEqual("FourthString", deserializedStrings.FourthString);
    }

    [TestMethod]
    public void MutableStringsType_Serializes_DeserializesIsSame()
    {
        var orxStringsSerializer = new OrxByteSerializer<MutableStrings>();
        var originalDoubleStringArray = new MutableStrings
        {
            FirstString = "FirstString", SecondString = "SecondString", ThirdString = "ThirdString"
            , FourthString = "FourthString"
        };

        socketBufferReadContext.MessageSize = orxStringsSerializer.Serialize(originalDoubleStringArray,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orxStringsDeserializer = new OrxByteDeserializer<MutableStrings>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedStrings = orxStringsDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual("FirstString", deserializedStrings.FirstString);
        Assert.AreEqual("SecondString", deserializedStrings.SecondString);
        Assert.AreEqual("ThirdString", deserializedStrings.ThirdString);
        Assert.AreEqual("FourthString", deserializedStrings.FourthString);
    }

    public class DoubleStringArray
    {
        [OrxMandatoryField(0)] public string[]? FirstStringArray { get; set; }

        [OrxOptionalField(1)] public string[]? SecondStringArray { get; set; }
    }

    public class Longs
    {
        [OrxMandatoryField(0)] public long FirstLong { get; set; }

        [OrxMandatoryField(1)] public long SecondLong { get; set; }

        [OrxOptionalField(1)] public long ThirdLong { get; set; }

        [OrxOptionalField(2)] public long FourthLong { get; set; }
    }

    public class Strings
    {
        [OrxMandatoryField(0)] public string? FirstString { get; set; }

        [OrxMandatoryField(1)] public string? SecondString { get; set; }

        [OrxOptionalField(1)] public string? ThirdString { get; set; }

        [OrxOptionalField(2)] public string? FourthString { get; set; }
    }

    public class MutableStrings
    {
        [OrxMandatoryField(0)] public MutableString? FirstString { get; set; }

        [OrxMandatoryField(1)] public MutableString? SecondString { get; set; }

        [OrxOptionalField(1)] public MutableString? ThirdString { get; set; }

        [OrxOptionalField(2)] public MutableString? FourthString { get; set; }
    }
}
