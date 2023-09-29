using System;
using System.Text;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.DataStructures.Memory
{
    public static unsafe class StreamByteOps
    {
        private const int writeBufferSize = 8000;
        private static Func<StringBuilder, char[]> getStringBuilderBackingFunc;
        private static GarbageAndLockFreePooledFactory<char[]> pooledCharBuffer = new GarbageAndLockFreePooledFactory<char[]>(()=> new char[writeBufferSize]);

        static StreamByteOps()
        {
            getStringBuilderBackingFunc = NonPublicInvocator.GetInstanceFieldExtractor<StringBuilder, char[]>(
                typeof(StringBuilder), "m_ChunkChars");
        }

        public static void ToBytes(ref byte* ptr, ushort value)
        {
            byte* data = (byte*)&value;
            if (BitConverter.IsLittleEndian)
            {
                *(ptr++) = *(data + 1);
                *(ptr++) = *data;
            }
            else
            {
                *(ptr++) = *(data++);
                *(ptr++) = *data;
            }
        }
        public static void ToBytes(ref byte* ptr, short value)
        {
            byte* data = (byte*)&value;
            if (BitConverter.IsLittleEndian)
            {
                *(ptr++) = *(data + 1);
                *(ptr++) = *data;
            }
            else
            {
                *(ptr++) = *(data++);
                *(ptr++) = *data;
            }
        }

        public static void ToBytes(ref byte* ptr, uint value)
        {
            byte* data = (byte*)&value;
            if (BitConverter.IsLittleEndian)
            {
                *(ptr++) = *(data + 3);
                *(ptr++) = *(data + 2);
                *(ptr++) = *(data + 1);
                *(ptr++) = *data;
            }
            else
            {
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *data;
            }
        }

        public static void ToBytes(ref byte* ptr, int value)
        {
            byte* data = (byte*)&value;
            if (BitConverter.IsLittleEndian)
            {
                *(ptr++) = *(data + 3);
                *(ptr++) = *(data + 2);
                *(ptr++) = *(data + 1);
                *(ptr++) = *data;
            }
            else
            {
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *data;
            }
        }

        public static void ToBytes(ref byte* ptr, long value)
        {
            byte* data = (byte*)&value;
            if (BitConverter.IsLittleEndian)
            {
                *(ptr++) = *(data + 7);
                *(ptr++) = *(data + 6);
                *(ptr++) = *(data + 5);
                *(ptr++) = *(data + 4);
                *(ptr++) = *(data + 3);
                *(ptr++) = *(data + 2);
                *(ptr++) = *(data + 1);
                *(ptr++) = *data;
            }
            else
            {
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *data;
            }
        }
        
        public static void ToBytes(ref byte* ptr, double value)
        {
            byte* data = (byte*)&value;
            if (BitConverter.IsLittleEndian)
            {
                *(ptr++) = *(data + 7);
                *(ptr++) = *(data + 6);
                *(ptr++) = *(data + 5);
                *(ptr++) = *(data + 4);
                *(ptr++) = *(data + 3);
                *(ptr++) = *(data + 2);
                *(ptr++) = *(data + 1);
                *(ptr++) = *data;
            }
            else
            {
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *(data++);
                *(ptr++) = *data;
            }
        }

        public static int ToBytesWithSizeHeader(ref byte* ptr, string value, int availableBytes)
        {
            var stringSize = ptr;
            ptr += 2;
            ushort strSize = (ushort)ToBytes(ref ptr, value, ushort.MaxValue);
            ToBytes(ref stringSize, strSize);
            return strSize + 2;
        }

        public static int ToBytes(ref byte* ptr, string value, int availableBytes)
        {
            if (value != null)
            {

                int bytesUsed;
                fixed (char* toEncode = value)
                {
                    int charsEncoded;
                    bool completed;

                    Encoding.UTF8.GetEncoder().Convert(toEncode,
                        value.Length, ptr, availableBytes, true, out charsEncoded, out bytesUsed,
                        out completed);
                    ptr += bytesUsed;
                    *(ptr++) = 0;
                }
                return bytesUsed + 1;
            }
            return 0;
        }

        public static int ToBytesWithSizeHeader(ref byte* ptr, MutableString value, int availableBytes)
        {
            var stringSize = ptr;
            ptr += 2;
            ushort strSize = (ushort)ToBytes(ref ptr, value, ushort.MaxValue);
            ToBytes(ref stringSize, strSize);
            return strSize + 2;
        }

        public static int ToBytes(ref byte* ptr, MutableString value, int availableBytes)
        {
            if (value != null)
            {
                var temporaryBuffer = pooledCharBuffer.Borrow();

                var sb = value.GetBackingStringBuilder();
                int i = 0;
                int bytesConsumed = 0;
                do
                {
                    int j = 0;
                    for (; i < sb.Length && j < temporaryBuffer.Length; j++, i++)
                    {
                        temporaryBuffer[j] = sb[i];
                    }
                    fixed (char* toEncode = temporaryBuffer)
                    {
                        Encoding.UTF8.GetEncoder().Convert(toEncode,
                            j, ptr, availableBytes, true, out int _, out var bytesUsed,
                            out bool _);
                        bytesConsumed += bytesUsed;
                        ptr += bytesUsed;
                    }
                } while (i < sb.Length);
                *(ptr++) = 0;
                pooledCharBuffer.ReturnBorrowed(temporaryBuffer);
                return bytesConsumed + 1;
            }
            return 0;
        }

        public static ushort ToUShort(ref byte* ptr)
        {
            ushort buffer;
            byte* data = (byte*)&buffer;
            if (BitConverter.IsLittleEndian)
            {
                *(data + 1) = *(ptr++);
                *data = *(ptr++);
            }
            else
            {
                *(data++) = *(ptr++);
                *data = *(ptr++);
            }
            return buffer;
        }

        public static short ToShort(ref byte* ptr)
        {
            short buffer;
            byte* data = (byte*)&buffer;
            if (BitConverter.IsLittleEndian)
            {
                *(data + 1) = *(ptr++);
                *data = *(ptr++);
            }
            else
            {
                *(data++) = *(ptr++);
                *data = *(ptr++);
            }
            return buffer;
        }

        public static uint ToUInt(ref byte* ptr)
        {
            uint buffer;
            byte* data = (byte*)&buffer;
            if (BitConverter.IsLittleEndian)
            {
                *(data + 3) = *(ptr++);
                *(data + 2) = *(ptr++);
                *(data + 1) = *(ptr++);
                *data = *(ptr++);
            }
            else
            {
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *data = *(ptr++);
            }
            return buffer;
        }

        public static int ToInt(ref byte* ptr)
        {
            int buffer;
            byte* data = (byte*)&buffer;
            if (BitConverter.IsLittleEndian)
            {
                *(data + 3) = *(ptr++);
                *(data + 2) = *(ptr++);
                *(data + 1) = *(ptr++);
                *data = *(ptr++);
            }
            else
            {
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *data = *(ptr++);
            }
            return buffer;
        }

        public static long ToLong(ref byte* ptr)
        {
            long buffer;
            byte* data = (byte*)&buffer;
            if (BitConverter.IsLittleEndian)
            {
                *(data + 7) = *(ptr++);
                *(data + 6) = *(ptr++);
                *(data + 5) = *(ptr++);
                *(data + 4) = *(ptr++);
                *(data + 3) = *(ptr++);
                *(data + 2) = *(ptr++);
                *(data + 1) = *(ptr++);
                *data = *(ptr++);
            }
            else
            {
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *data = *(ptr++);
            }
            return buffer;
        }

        public static double ToDouble(ref byte* ptr)
        {
            double buffer;
            byte* data = (byte*)&buffer;
            if (BitConverter.IsLittleEndian)
            {
                *(data + 7) = *(ptr++);
                *(data + 6) = *(ptr++);
                *(data + 5) = *(ptr++);
                *(data + 4) = *(ptr++);
                *(data + 3) = *(ptr++);
                *(data + 2) = *(ptr++);
                *(data + 1) = *(ptr++);
                *data = *(ptr++);
            }
            else
            {
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *(data++) = *(ptr++);
                *data = *(ptr++);
            }
            return buffer;
        }

        public static string ToString(ref byte* ptr, int readBytes)
        {
            var charBuffer = pooledCharBuffer.Borrow();

            var sb = new StringBuilder();
            int bytesReadSoFar = 0;
            while (bytesReadSoFar < readBytes - 1)
            {
                int maxReadAmount = Math.Min(readBytes - bytesReadSoFar - 1, charBuffer.Length);
                int charsUsed;
                fixed (char* chrPtr = charBuffer)
                {
                    Encoding.UTF8.GetDecoder().Convert(ptr, maxReadAmount, chrPtr, maxReadAmount, false, out var bytesUsed,
                        out charsUsed, out bool _);
                    bytesReadSoFar += bytesUsed;
                    ptr += bytesUsed;
                }
                sb.Append(charBuffer, 0, charsUsed);
            }
            ptr++; //null terminator
            pooledCharBuffer.ReturnBorrowed(charBuffer);
            return sb.ToString();
        }

        public static string ToStringWithSizeHeader(ref byte* ptr)
        {
            var stringSize = ToUShort(ref ptr);
            return ToString(ref ptr, stringSize);
        }

        public static int ToMutableString(ref byte* ptr, int readBytes, MutableString destination)
        {
            var charBuffer = pooledCharBuffer.Borrow();

            var backingStringBuilder = destination.GetBackingStringBuilder();
            backingStringBuilder.Clear();
            int bytesReadSoFar = 0;
            while (bytesReadSoFar < readBytes - 1)
            {
                int maxReadAmount = Math.Min(readBytes - bytesReadSoFar - 1, charBuffer.Length);
                int charsUsed;
                fixed (char* chrPtr = charBuffer)
                {
                    Encoding.UTF8.GetDecoder().Convert(ptr, maxReadAmount, chrPtr, maxReadAmount, false, out var bytesUsed,
                        out charsUsed, out bool _);
                    bytesReadSoFar += bytesUsed;
                    ptr += bytesUsed;
                }
                backingStringBuilder.Append(charBuffer, 0, charsUsed);
            }
            ptr++; //null terminator
            pooledCharBuffer.ReturnBorrowed(charBuffer);
            return bytesReadSoFar + 1;
        }

        public static MutableString ToMutableStringWithSizeHeader(ref byte* ptr, MutableString destination)
        {
            var stringSize = ToUShort(ref ptr);
            ToMutableString(ref ptr, stringSize, destination);
            return destination;
        }
    }
}
