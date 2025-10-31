// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

#endregion

namespace FortitudeCommon.Extensions;

public class ByteComparer : IComparer<byte>
{
    public int Compare(byte x, byte y)
    {
        return unchecked((x - y));
    }
}

public class UShortComparer : IComparer<ushort>
{
    public int Compare(ushort x, ushort y)
    {
        return unchecked((x - y));
    }
}

public class ShortComparer : IComparer<short>
{
    public int Compare(short x, short y)
    {
        return unchecked((x - y));
    }
}

public class IntComparer : IComparer<int>
{
    public int Compare(int x, int y)
    {
        return unchecked((x - y));
    }
}

public class UIntComparer : IComparer<uint>
{
    public int Compare(uint x, uint y)
    {
        return (int)unchecked((x - y));
    }
}

public class LongComparer : IComparer<long>
{
    public int Compare(long x, long y)
    {
        var totalCompare = unchecked((x - y));
        var topHalf      = (int)(totalCompare / int.MaxValue);
        if (topHalf != 0) return topHalf;
        return (int)totalCompare;
    }
}

public class ULongComparer : IComparer<ulong>
{
    public int Compare(ulong x, ulong y)
    {
        var totalCompare = unchecked(((long)x - (long)y));
        var topHalf      = (int)(totalCompare / int.MaxValue);
        if (topHalf != 0) return topHalf;
        return (int)totalCompare;
    }
}

public static class NumberExtensions
{
    public static ByteComparer   ByteComparer   = new();
    public static UShortComparer UShortComparer = new();
    public static ShortComparer  ShortComparer  = new();
    public static IntComparer    IntComparer    = new();
    public static UIntComparer   UIntComparer   = new();
    public static LongComparer   LongComparer   = new();
    public static ULongComparer  ULongComparer  = new();

    public static Type[] AllNumberTypes =
    [
        typeof(byte)
      , typeof(sbyte)
      , typeof(char)
      , typeof(short)
      , typeof(ushort)
      , typeof(int)
      , typeof(uint)
      , typeof(nint)
      , typeof(float)
      , typeof(long)
      , typeof(ulong)
      , typeof(double)
      , typeof(decimal)
      , typeof(nint)
      , typeof(nuint)
      , typeof(Half)
      , typeof(Int128)
      , typeof(UInt128)
      , typeof(BigInteger)
      , typeof(Complex)
    ];

    public static Type[] AllNullableNumberTypes =
    [
        typeof(byte?)
      , typeof(sbyte?)
      , typeof(char?)
      , typeof(short?)
      , typeof(ushort?)
      , typeof(int?)
      , typeof(uint?)
      , typeof(nint?)
      , typeof(float?)
      , typeof(long?)
      , typeof(ulong?)
      , typeof(double?)
      , typeof(decimal?)
      , typeof(nint?)
      , typeof(nuint?)
      , typeof(Half?)
      , typeof(Int128?)
      , typeof(UInt128?)
      , typeof(BigInteger?)
      , typeof(Complex?)
    ];

    public static Type[] UniversalNumberTypes =
    [
        typeof(byte)
      , typeof(char)
      , typeof(int)
      , typeof(float)
      , typeof(long)
      , typeof(double)
    ];

    public static Type[] UniversalNullableNumberTypes =
    [
        typeof(byte?)
      , typeof(char?)
      , typeof(int?)
      , typeof(float?)
      , typeof(long?)
      , typeof(double?)
    ];

    public static Type[] UniversalStringExemptNumberTypes =
    [
        typeof(byte)
      , typeof(sbyte)
      , typeof(short)
      , typeof(ushort)
      , typeof(int)
      , typeof(uint)
      , typeof(nint)
      , typeof(float)
      , typeof(long)
      , typeof(ulong)
      , typeof(double)
      , typeof(decimal)
      , typeof(nint)
      , typeof(nuint)
      , typeof(Half)
    ];

    public static Type[] UniversalStringExemptNullableNumberTypes =
    [
        typeof(byte?)
      , typeof(sbyte?)
      , typeof(short?)
      , typeof(ushort?)
      , typeof(int?)
      , typeof(uint?)
      , typeof(nint?)
      , typeof(float?)
      , typeof(long?)
      , typeof(ulong?)
      , typeof(double?)
      , typeof(decimal?)
      , typeof(nint?)
      , typeof(nuint?)
      , typeof(Half?)
    ];
    
    public static bool IsUniversalStringExemptNumberType(this Type checkType) => 
        UniversalStringExemptNumberTypes.Contains(checkType) || UniversalStringExemptNullableNumberTypes.Contains(checkType);

    public static bool IsNumericType(this Type toCheck)
    {
        for (int i = 0; i < AllNumberTypes.Length; i++)
        {
            if (toCheck == AllNumberTypes[i]) return true;
        }
        return false;
    }

    public static bool IsNullableNumericType(this Type toCheck)
    {
        for (int i = 0; i < AllNullableNumberTypes.Length; i++)
        {
            if (toCheck == AllNullableNumberTypes[i]) return true;
        }
        return false;
    }

    public static bool IsNumericOrNullableType(this Type toCheck)
    {
        return toCheck.IsNumericType() || toCheck.IsNullableNumericType();
    }

    public static int NextPowerOfTwo(this int value)
    {
        var ceiling = 2;

        while (ceiling < value) ceiling *= 2;

        return ceiling;
    }

    public static string TorHexFormat_2(this ulong toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[23].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_2(toConvert); }
        else { buildSpan.ToLowerHexFormat_2(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_2(this Span<char> buffer, ulong toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 24) return -1;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        return 23;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_2(this Span<char> buffer, ulong toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 24) return -1;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        return 23;
    }

    public static string TorHexFormat_4(this ulong toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[19].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_2(toConvert); }
        else { buildSpan.ToLowerHexFormat_2(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_4(this Span<char> buffer, ulong toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 20) return -1;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        return 19;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_4(this Span<char> buffer, ulong toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 24) return -1;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        return 19;
    }

    public static string ToHexFormat_2(this long toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[23].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_2(toConvert); }
        else { buildSpan.ToLowerHexFormat_2(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_2(this Span<char> buffer, long toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 24) return -1;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        return 23;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_2(this Span<char> buffer, long toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 24) return -1;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        return 23;
    }

    public static string ToHexFormat_4(this long toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[19].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_4(toConvert); }
        else { buildSpan.ToLowerHexFormat_4(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_4(this Span<char> buffer, long toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 20) return -1;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        return 19;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_4(this Span<char> buffer, long toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 20) return -1;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        return 19;
    }

    public static string ToHex(this int toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[8].ResetMemory();
        if (isUpperCaseHex) { buildSpan.AppendAsUpperHex(toConvert); }
        else { buildSpan.AppendAsLowerHex(toConvert); }
        return new string(buildSpan);
    }

    public static string ToHexFormat_2(this int toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[11].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_4(toConvert); }
        else { buildSpan.ToLowerHexFormat_4(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_2(this Span<char> buffer, int toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 12) return -1;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        return 11;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_2(this Span<char> buffer, int toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 12) return -1;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        return 11;
    }

    public static string ToHexFormat_4(this int toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[9].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_4(toConvert); }
        else { buildSpan.ToLowerHexFormat_4(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_4(this Span<char> buffer, int toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 10) return -1;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        return 9;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_4(this Span<char> buffer, int toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 10) return -1;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        return 9;
    }

    public static string ToHex(this uint toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[8].ResetMemory();
        if (isUpperCaseHex) { buildSpan.AppendAsUpperHex(toConvert); }
        else { buildSpan.AppendAsLowerHex(toConvert); }
        return new string(buildSpan);
    }

    public static string ToHexFormat_2(this uint toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[11].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_4(toConvert); }
        else { buildSpan.ToLowerHexFormat_4(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_2(this Span<char> buffer, uint toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 12) return -1;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        return 11;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_2(this Span<char> buffer, uint toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 12) return -1;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        return 11;
    }

    public static string ToHexFormat_4(this uint toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[9].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_4(toConvert); }
        else { buildSpan.ToLowerHexFormat_4(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_4(this Span<char> buffer, uint toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 10) return -1;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsUpperHexUnchecked(toConvert, offset);
        return 9;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_4(this Span<char> buffer, uint toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 10) return -1;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        offset           +=  4;
        buffer[offset++] =   '_';
        toConvert        >>= 16;

        buffer.AppendLowestShortAsLowerHexUnchecked(toConvert, offset);
        return 9;
    }

    public static string ToHexFormat_2(this short toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[5].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_4(toConvert); }
        else { buildSpan.ToLowerHexFormat_4(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_2(this Span<char> buffer, short toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 6) return -1;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        return 5;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_2(this Span<char> buffer, short toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 6) return -1;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        return 5;
    }

    public static string ToHexFormat_2(this ushort toConvert, bool isUpperCaseHex = true)
    {
        var buildSpan = stackalloc char[5].ResetMemory();
        if (isUpperCaseHex) { buildSpan.ToUpperHexFormat_4(toConvert); }
        else { buildSpan.ToLowerHexFormat_4(toConvert); }
        return new string(buildSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperHexFormat_2(this Span<char> buffer, ushort toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 6) return -1;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsUpperHexUnchecked(toConvert, offset);
        return 5;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLowerHexFormat_2(this Span<char> buffer, ushort toConvert, int bufferOffset = 0)
    {
        var offset = bufferOffset;
        if (buffer.Length - bufferOffset < 6) return -1;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        offset           +=  2;
        buffer[offset++] =   '_';
        toConvert        >>= 8;

        buffer.AppendLowestByteAsLowerHexUnchecked(toConvert, offset);
        return 5;
    }

    public static int NumOfDigits(this int findDigits, bool includeMinusSign = true)
    {
        if (findDigits is >= 0 and < 10) return 1;
        if (findDigits is < 0 and > -10) return includeMinusSign ? 2 : 1;
        return 1 + (findDigits / 10).NumOfDigits();
    }

    public static int NumOfDigits(this uint findDigits)
    {
        if (findDigits < 10) return 1;
        return 1 + (findDigits / 10).NumOfDigits();
    }

    public static int NumOfDigits(this long findDigits, bool includeMinusSign = true)
    {
        if (findDigits < 10) return 1;
        if (findDigits is < 0 and > -10) return includeMinusSign ? 2 : 1;
        return 1 + (findDigits / 10).NumOfDigits();
    }

    public static int NumOfDigits(this ulong findDigits)
    {
        if (findDigits < 10) return 1;
        return 1 + (findDigits / 10).NumOfDigits();
    }


    public const ulong KiloByte = 1024;
    public const ulong MegaByte = KiloByte * KiloByte;
    public const ulong GigaByte = KiloByte * MegaByte;
    public const ulong TeraByte = KiloByte * GigaByte;
    public const ulong PetaByte = KiloByte * TeraByte;
    public const ulong ExaByte  = KiloByte * PetaByte;

    public static ulong AsKiloBytes(this ulong change) => change * KiloByte;
    public static ulong AsMegaBytes(this ulong change) => change * MegaByte;
    public static ulong AsGigaBytes(this ulong change) => change * GigaByte;
    public static ulong AsTeraBytes(this ulong change) => change * TeraByte;
    public static ulong AsPetaBytes(this ulong change) => change * PetaByte;
    public static ulong AsExaBytes(this ulong change)  => change * ExaByte;


    private static readonly (string, ulong)[] ByteSuffixesAndSizes =
    [
        ("b|byte", 1)
      , ("kb,kilobyte", KiloByte)
      , ("mb,megabyte", MegaByte)
      , ("gb,gigabyte", GigaByte)
      , ("tb,terabyte", TeraByte)
      , ("pb,petabyte", PetaByte)
      , ("eb,exabyte", ExaByte)
    ];

    private enum NumberFormatCheckStage
    {
        IgnorePrefix
      , SignOrDigit
      , FirstDigit
      , DigitSeparatorOrDecimal
      , DecimalOrDigit
      , DigitOrExponent
      , ExponentSignOrDigit
      , ExponentDigit
      , IgnoreSuffix
    }

    private static readonly char DecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
    private static readonly char GroupSeparator   = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator[0];

    public static bool CharsAreAllowedInNumbers(this ReadOnlySpan<char> checkIsAllowedNumberSequence
      , bool allowGroupSeparators = false, string allowedIgnorePrefixSuffixChars = "")
    {
        var isValidNumber = checkIsAllowedNumberSequence.Length > 0;
        var foundExpectedDigit   = false;

        bool IsAllowedIgnore(char checkIsAllowed, string allowedChars)
        {
            var isAllowedIgnore = false;
            for (int j = 0; j < allowedChars.Length; j++)
            {
                var ignore = allowedChars[j];
                if (checkIsAllowed == ignore)
                {
                    isAllowedIgnore = true;
                    break;
                }
            }
            return isAllowedIgnore;
        }

        NumberFormatCheckStage stage = allowedIgnorePrefixSuffixChars.Length > 0 
            ? NumberFormatCheckStage.IgnorePrefix 
            : NumberFormatCheckStage.SignOrDigit;
        for (var i = 0; i < checkIsAllowedNumberSequence.Length && isValidNumber; i++)
        {
            var check = checkIsAllowedNumberSequence[i];
            switch (stage)
            {
                case NumberFormatCheckStage.IgnorePrefix:
                    if (check.IsNumberSign() || check.IsDigit())
                    {
                        stage = NumberFormatCheckStage.SignOrDigit;
                        i--;
                    }
                    isValidNumber = IsAllowedIgnore(check, allowedIgnorePrefixSuffixChars);
                    break;
                case NumberFormatCheckStage.SignOrDigit:
                    if (check.IsNumberSign())
                    {
                        stage = NumberFormatCheckStage.FirstDigit;
                        break;
                    }
                    if (check.IsDigit())
                    {
                        stage       = allowGroupSeparators 
                            ? NumberFormatCheckStage.DigitSeparatorOrDecimal
                            : NumberFormatCheckStage.DecimalOrDigit;
                        foundExpectedDigit = true;
                        break;
                    }
                    isValidNumber = false;
                    break;
                case NumberFormatCheckStage.FirstDigit:
                    stage       = allowGroupSeparators 
                        ? NumberFormatCheckStage.DigitSeparatorOrDecimal
                        : NumberFormatCheckStage.DecimalOrDigit;
                    isValidNumber = foundExpectedDigit = check.IsDigit();
                    break;
                case NumberFormatCheckStage.DigitSeparatorOrDecimal:
                    stage = NumberFormatCheckStage.DigitSeparatorOrDecimal;
                    if (check == DecimalSeparator)
                    {
                        stage = NumberFormatCheckStage.DigitOrExponent;
                        break;
                    }
                    if (check == GroupSeparator)
                    {
                        break;
                    }
                    if (check.IsDigit())
                    {
                        foundExpectedDigit = true;
                        break;
                    }
                    if (IsAllowedIgnore(check, allowedIgnorePrefixSuffixChars))
                    {
                        stage = NumberFormatCheckStage.IgnoreSuffix;
                        break;
                    }
                    isValidNumber = false;
                    break;
                case NumberFormatCheckStage.DecimalOrDigit:
                    stage = NumberFormatCheckStage.DigitSeparatorOrDecimal;
                    if (check == DecimalSeparator)
                    {
                        stage = NumberFormatCheckStage.DigitOrExponent;
                        break;
                    }
                    if (check.IsDigit())
                    {
                        foundExpectedDigit = true;
                        break;
                    }
                    if (IsAllowedIgnore(check, allowedIgnorePrefixSuffixChars))
                    {
                        stage = NumberFormatCheckStage.IgnoreSuffix;
                        break;
                    }
                    isValidNumber = false;
                    break;
                case NumberFormatCheckStage.DigitOrExponent:
                    stage = NumberFormatCheckStage.DigitSeparatorOrDecimal;
                    if (check.IsExponentSymbol())
                    {
                        stage = NumberFormatCheckStage.ExponentSignOrDigit;
                        break;
                    }
                    if (check.IsDigit())
                    {
                        isValidNumber = foundExpectedDigit = true;
                        break;
                    }
                    if (IsAllowedIgnore(check, allowedIgnorePrefixSuffixChars))
                    {
                        stage = NumberFormatCheckStage.IgnoreSuffix;
                        break;
                    }
                    isValidNumber = false;
                    break;
                case NumberFormatCheckStage.ExponentSignOrDigit:
                    stage = NumberFormatCheckStage.DigitSeparatorOrDecimal;
                    if (check.IsNumberSign())
                    {
                        stage = NumberFormatCheckStage.ExponentDigit;
                        foundExpectedDigit = false;
                        break;
                    }
                    if (check.IsDigit())
                    {
                        isValidNumber = foundExpectedDigit = true;
                        break;
                    }
                    if (IsAllowedIgnore(check, allowedIgnorePrefixSuffixChars))
                    {
                        stage = NumberFormatCheckStage.IgnoreSuffix;
                        break;
                    }
                    isValidNumber = false;
                    break;
                case NumberFormatCheckStage.ExponentDigit:
                    if (check.IsDigit())
                    {
                        isValidNumber = foundExpectedDigit = true;
                        break;
                    }
                    if (IsAllowedIgnore(check, allowedIgnorePrefixSuffixChars))
                    {
                        stage = NumberFormatCheckStage.IgnoreSuffix;
                        break;
                    }
                    isValidNumber = false;
                    break;
                case NumberFormatCheckStage.IgnoreSuffix:
                    if (IsAllowedIgnore(check, allowedIgnorePrefixSuffixChars))
                    {
                        break;
                    }
                    isValidNumber = false;
                    break;
            }
        }
        return isValidNumber && foundExpectedDigit;
    }

    public static ulong ToSizeInBytes(this ulong value, string suffixLowerCase)
    {
        if (suffixLowerCase[^1] == 's') { suffixLowerCase = suffixLowerCase.Substring(0, suffixLowerCase.Length - 1); }
        foreach (var (suffix, multiplier) in ByteSuffixesAndSizes)
        {
            var split = suffix.Split(',');
            foreach (var suffixType in split)
            {
                if (suffixType == suffixLowerCase) { return value * multiplier; }
            }
        }
        throw new ArgumentException($"Expected suffixLowerCase to be one of {ByteSuffixesAndSizes.Select((suffix, _) => suffix)}");
    }
}
