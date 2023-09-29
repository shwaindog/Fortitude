using FortitudeCommon.DataStructures.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeCommon.DataStructures.Memory
{
    [TestClass]
    public class StreamByteOpsTests
    {
        private byte[] testBuffer;
        private int byteBufferSize;

        [TestInitialize]
        public void SetUp()
        {
            byteBufferSize = 1000;
            testBuffer = new byte[byteBufferSize];
        }

        [TestMethod]
        public unsafe void EmptyBuffer_MultipleStringEncodesThenDecodes_ReturnsMultipleStrings()
        {
            fixed (byte* bufferStrtPtr = testBuffer)
            {
                byte* ptr = bufferStrtPtr;
                string firstString = "firstString";
                string secondString = "secondString";
                string thirdString = "thirdString";

                var firstStringBytes = StreamByteOps.ToBytes(ref ptr, firstString, byteBufferSize);
                var secondStringBytes = StreamByteOps.ToBytes(ref ptr, secondString, byteBufferSize - firstStringBytes);
                var thirdStringBytes = StreamByteOps.ToBytes(ref ptr, thirdString, 
                    byteBufferSize - (firstStringBytes + secondStringBytes));
                
                ptr = bufferStrtPtr;
                var deserializedFirstString = StreamByteOps.ToString(ref ptr, firstStringBytes);
                var deserializedSecondString = StreamByteOps.ToString(ref ptr, secondStringBytes);
                var deserializedThirdString = StreamByteOps.ToString(ref ptr, thirdStringBytes);
                
                Assert.AreEqual(firstString, deserializedFirstString);
                Assert.AreEqual(secondString, deserializedSecondString);
                Assert.AreEqual(thirdString, deserializedThirdString);
            }
        }
    }
}