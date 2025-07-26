using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;

namespace FortitudeCommon.Extensions;

public static class Utf8StringExtensions
{
    public static string CharIndexPosListedSizeString(this int size)
    {
        var sb = new StringBuilder();
        sb.Append("0");
        int i;
        if (size < 1000)
        {
            i = 4;
            for (; i < size + 4; i += 4) sb.AppendFormat("_{0:000}", i);
        }
        else if (size < 10_000)
        {
            i = 5;
            for (; i < size + 5; i += 5) sb.AppendFormat("_{0:0000}", i);
        }
        else if (size < 100_000)
        {
            i = 6;
            for (; i < size + 6; i += 6) sb.AppendFormat("_{0:00000}", i);
        }
        else
        {
            i = 7;
            for (; i < size + 7; i += 7) sb.AppendFormat("_{0:000000}", i);
        }

        return sb.ToString(0, size);
    }

    public static int?     ToInt(this string? parse)     => parse.IsNotNullOrEmpty() ? int.Parse(parse!) : null;
    public static uint?    ToUInt(this string? parse)    => parse.IsNotNullOrEmpty() ? uint.Parse(parse!) : null;
    public static float?   ToFloat(this string? parse)   => parse.IsNotNullOrEmpty() ? float.Parse(parse!) : null;
    public static decimal? ToDecimal(this string? parse) => parse.IsNotNullOrEmpty() ? decimal.Parse(parse!) : null;
    public static double?  ToDouble(this string? parse)  => parse.IsNotNullOrEmpty() ? double.Parse(parse!) : null;

    //Taken from Microsoft - System.String
    internal static unsafe int strlen(byte* ptr) => IndexOfNullByte(ptr);

    // IndexOfNullByte processes memory in aligned chunks, and thus it won't crash even if it accesses memory beyond the null terminator.
    // This behavior is an implementation detail of the runtime and callers outside System.Private.CoreLib must not depend on it.
    internal static unsafe int IndexOfNullByte(byte* searchSpace)
    {
        const int  Length = int.MaxValue;
        const uint uValue = 0; // Use uint for comparisons to avoid unnecessary 8->32 extensions

        nuint offset = 0; // Use nuint for arithmetic to avoid unnecessary 64->32->64 truncations

        nuint lengthToExamine = (nuint)(uint)Length;

        if (Vector128.IsHardwareAccelerated)
        {
            // Avx2 branch also operates on Sse2 sizes, so check is combined.
            lengthToExamine = UnalignedCountVector128(searchSpace);
        }

    SequentialScan:
        while (lengthToExamine >= 8)
        {
            lengthToExamine -= 8;

            if (uValue == searchSpace[offset]) goto Found;
            if (uValue == searchSpace[offset + 1]) goto Found1;
            if (uValue == searchSpace[offset + 2]) goto Found2;
            if (uValue == searchSpace[offset + 3]) goto Found3;
            if (uValue == searchSpace[offset + 4]) goto Found4;
            if (uValue == searchSpace[offset + 5]) goto Found5;
            if (uValue == searchSpace[offset + 6]) goto Found6;
            if (uValue == searchSpace[offset + 7]) goto Found7;

            offset += 8;
        }

        if (lengthToExamine >= 4)
        {
            lengthToExamine -= 4;

            if (uValue == searchSpace[offset]) goto Found;
            if (uValue == searchSpace[offset + 1]) goto Found1;
            if (uValue == searchSpace[offset + 2]) goto Found2;
            if (uValue == searchSpace[offset + 3]) goto Found3;

            offset += 4;
        }

        while (lengthToExamine > 0)
        {
            lengthToExamine -= 1;

            if (uValue == searchSpace[offset]) goto Found;

            offset += 1;
        }

        // We get past SequentialScan only if IsHardwareAccelerated is true; and remain length is greater than Vector length.
        // However, we still have the redundant check to allow the JIT to see that the code is unreachable and eliminate it when the platform does not
        // have hardware accelerated. After processing Vector lengths we return to SequentialScan to finish any remaining.
        if (Vector512.IsHardwareAccelerated)
        {
            if (offset < (nuint)(uint)Length)
            {
                if ((((nuint)(uint)searchSpace + offset) & (nuint)(Vector256<byte>.Count - 1)) != 0)
                {
                    // Not currently aligned to Vector256 (is aligned to Vector128); this can cause a problem for searches
                    // with no upper bound e.g. String.strlen.
                    // Start with a check on Vector128 to align to Vector256, before moving to processing Vector256.
                    // This ensures we do not fault across memory pages while searching for an end of string.
                    Vector128<byte> search = Vector128.Load(searchSpace + offset);

                    // Same method as below
                    uint matches = Vector128.Equals(Vector128<byte>.Zero, search).ExtractMostSignificantBits();
                    if (matches == 0)
                    {
                        // Zero flags set so no matches
                        offset += (nuint)Vector128<byte>.Count;
                    }
                    else
                    {
                        // Find bitflag offset of first match and add to current offset
                        return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                    }
                }

                if ((((nuint)(uint)searchSpace + offset) & (nuint)(Vector512<byte>.Count - 1)) != 0)
                {
                    // Not currently aligned to Vector512 (is aligned to Vector256); this can cause a problem for searches
                    // with no upper bound e.g. String.strlen.
                    // Start with a check on Vector256 to align to Vector512, before moving to processing Vector256.
                    // This ensures we do not fault across memory pages while searching for an end of string.
                    Vector256<byte> search = Vector256.Load(searchSpace + offset);

                    // Same method as below
                    uint matches = Vector256.Equals(Vector256<byte>.Zero, search).ExtractMostSignificantBits();
                    if (matches == 0)
                    {
                        // Zero flags set so no matches
                        offset += (nuint)Vector256<byte>.Count;
                    }
                    else
                    {
                        // Find bitflag offset of first match and add to current offset
                        return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                    }
                }
                lengthToExamine = GetByteVector512SpanLength(offset, Length);
                if (lengthToExamine > offset)
                {
                    do
                    {
                        Vector512<byte> search  = Vector512.Load(searchSpace + offset);
                        ulong           matches = Vector512.Equals(Vector512<byte>.Zero, search).ExtractMostSignificantBits();
                        // Note that MoveMask has converted the equal vector elements into a set of bit flags,
                        // So the bit position in 'matches' corresponds to the element offset.
                        if (matches == 0)
                        {
                            // Zero flags set so no matches
                            offset += (nuint)Vector512<byte>.Count;
                            continue;
                        }

                        // Find bitflag offset of first match and add to current offset
                        return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                    } while (lengthToExamine > offset);
                }

                lengthToExamine = GetByteVector256SpanLength(offset, Length);
                if (lengthToExamine > offset)
                {
                    Vector256<byte> search = Vector256.Load(searchSpace + offset);

                    // Same method as above
                    uint matches = Vector256.Equals(Vector256<byte>.Zero, search).ExtractMostSignificantBits();
                    if (matches == 0)
                    {
                        // Zero flags set so no matches
                        offset += (nuint)Vector256<byte>.Count;
                    }
                    else
                    {
                        // Find bitflag offset of first match and add to current offset
                        return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                    }
                }

                lengthToExamine = GetByteVector128SpanLength(offset, Length);
                if (lengthToExamine > offset)
                {
                    Vector128<byte> search = Vector128.Load(searchSpace + offset);

                    // Same method as above
                    uint matches = Vector128.Equals(Vector128<byte>.Zero, search).ExtractMostSignificantBits();
                    if (matches == 0)
                    {
                        // Zero flags set so no matches
                        offset += (nuint)Vector128<byte>.Count;
                    }
                    else
                    {
                        // Find bitflag offset of first match and add to current offset
                        return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                    }
                }

                if (offset < (nuint)(uint)Length)
                {
                    lengthToExamine = ((nuint)(uint)Length - offset);
                    goto SequentialScan;
                }
            }
        }
        else if (Vector256.IsHardwareAccelerated)
        {
            if (offset < (nuint)(uint)Length)
            {
                if ((((nuint)(uint)searchSpace + offset) & (nuint)(Vector256<byte>.Count - 1)) != 0)
                {
                    // Not currently aligned to Vector256 (is aligned to Vector128); this can cause a problem for searches
                    // with no upper bound e.g. String.strlen.
                    // Start with a check on Vector128 to align to Vector256, before moving to processing Vector256.
                    // This ensures we do not fault across memory pages while searching for an end of string.
                    Vector128<byte> search = Vector128.Load(searchSpace + offset);

                    // Same method as below
                    uint matches = Vector128.Equals(Vector128<byte>.Zero, search).ExtractMostSignificantBits();
                    if (matches == 0)
                    {
                        // Zero flags set so no matches
                        offset += (nuint)Vector128<byte>.Count;
                    }
                    else
                    {
                        // Find bitflag offset of first match and add to current offset
                        return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                    }
                }

                lengthToExamine = GetByteVector256SpanLength(offset, Length);
                if (lengthToExamine > offset)
                {
                    do
                    {
                        Vector256<byte> search  = Vector256.Load(searchSpace + offset);
                        uint            matches = Vector256.Equals(Vector256<byte>.Zero, search).ExtractMostSignificantBits();
                        // Note that MoveMask has converted the equal vector elements into a set of bit flags,
                        // So the bit position in 'matches' corresponds to the element offset.
                        if (matches == 0)
                        {
                            // Zero flags set so no matches
                            offset += (nuint)Vector256<byte>.Count;
                            continue;
                        }

                        // Find bitflag offset of first match and add to current offset
                        return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                    } while (lengthToExamine > offset);
                }

                lengthToExamine = GetByteVector128SpanLength(offset, Length);
                if (lengthToExamine > offset)
                {
                    Vector128<byte> search = Vector128.Load(searchSpace + offset);

                    // Same method as above
                    uint matches = Vector128.Equals(Vector128<byte>.Zero, search).ExtractMostSignificantBits();
                    if (matches == 0)
                    {
                        // Zero flags set so no matches
                        offset += (nuint)Vector128<byte>.Count;
                    }
                    else
                    {
                        // Find bitflag offset of first match and add to current offset
                        return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                    }
                }

                if (offset < (nuint)(uint)Length)
                {
                    lengthToExamine = ((nuint)(uint)Length - offset);
                    goto SequentialScan;
                }
            }
        }
        else if (Vector128.IsHardwareAccelerated)
        {
            if (offset < (nuint)(uint)Length)
            {
                lengthToExamine = GetByteVector128SpanLength(offset, Length);

                while (lengthToExamine > offset)
                {
                    Vector128<byte> search = Vector128.Load(searchSpace + offset);

                    // Same method as above
                    Vector128<byte> compareResult = Vector128.Equals(Vector128<byte>.Zero, search);
                    if (compareResult == Vector128<byte>.Zero)
                    {
                        // Zero flags set so no matches
                        offset += (nuint)Vector128<byte>.Count;
                        continue;
                    }

                    // Find bitflag offset of first match and add to current offset
                    uint matches = compareResult.ExtractMostSignificantBits();
                    return (int)(offset + (uint)BitOperations.TrailingZeroCount(matches));
                }

                if (offset < (nuint)(uint)Length)
                {
                    lengthToExamine = ((nuint)(uint)Length - offset);
                    goto SequentialScan;
                }
            }
        }

        throw new ArgumentException("String  Must Be Null Terminated");
    Found: // Workaround for https://github.com/dotnet/runtime/issues/8795
        return (int)offset;
    Found1:
        return (int)(offset + 1);
    Found2:
        return (int)(offset + 2);
    Found3:
        return (int)(offset + 3);
    Found4:
        return (int)(offset + 4);
    Found5:
        return (int)(offset + 5);
    Found6:
        return (int)(offset + 6);
    Found7:
        return (int)(offset + 7);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint GetByteVector128SpanLength(nuint offset, int length) => (nuint)(uint)((length - (int)offset) & ~(Vector128<byte>.Count - 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint GetByteVector256SpanLength(nuint offset, int length) => (nuint)(uint)((length - (int)offset) & ~(Vector256<byte>.Count - 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint GetByteVector512SpanLength(nuint offset, int length) => (nuint)(uint)((length - (int)offset) & ~(Vector512<byte>.Count - 1));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe nuint UnalignedCountVector128(byte* searchSpace)
    {
        nint unaligned = (nint)searchSpace & (Vector128<byte>.Count - 1);
        return (nuint)(uint)((Vector128<byte>.Count - unaligned) & (Vector128<byte>.Count - 1));
    }

    // Optimized byte-based SequenceEquals. The "length" parameter for this one is declared a nuint rather than int as we also use it for types other than byte
    // where the length can exceed 2Gb once scaled by sizeof(T).
    public static unsafe bool SequenceEqual(ref byte first, ref byte second, nuint length)
    {
        bool result;
        // Use nint for arithmetic to avoid unnecessary 64->32->64 truncations
        if (length >= (nuint)sizeof(nuint))
        {
            // Conditional jmp forward to favor shorter lengths. (See comment at "Equal:" label)
            // The longer lengths can make back the time due to branch misprediction
            // better than shorter lengths.
            goto Longer;
        }

        #if TARGET_64BIT
            // On 32-bit, this will always be true since sizeof(nuint) == 4
            if (length < sizeof(uint))
        #endif
        {
            uint  differentBits = 0;
            nuint offset        = (length & 2);
            if (offset != 0)
            {
                differentBits =  LoadUShort(ref first);
                differentBits -= LoadUShort(ref second);
            }
            if ((length & 1) != 0)
            {
                differentBits |= (uint)Unsafe.AddByteOffset(ref first, offset) - (uint)Unsafe.AddByteOffset(ref second, offset);
            }
            result = (differentBits == 0);
            goto Result;
        }
        #if TARGET_64BIT
            else
            {
                nuint offset = length - sizeof(uint);
                uint differentBits = LoadUInt(ref first) - LoadUInt(ref second);
                differentBits |= LoadUInt(ref first, offset) - LoadUInt(ref second, offset);
                result = (differentBits == 0);
                goto Result;
            }
        #endif
    Longer:
        // Only check that the ref is the same if buffers are large,
        // and hence its worth avoiding doing unnecessary comparisons
        if (!Unsafe.AreSame(ref first, ref second))
        {
            // C# compiler inverts this test, making the outer goto the conditional jmp.
            goto Vector;
        }

        // This becomes a conditional jmp forward to not favor it.
        goto Equal;

    Result:
        return result;
        // When the sequence is equal; which is the longest execution, we want it to determine that
        // as fast as possible so we do not want the early outs to be "predicted not taken" branches.
    Equal:
        return true;

    Vector:
        if (Vector128.IsHardwareAccelerated)
        {
            if (Vector512.IsHardwareAccelerated && length >= (nuint)Vector512<byte>.Count)
            {
                nuint offset          = 0;
                nuint lengthToExamine = length - (nuint)Vector512<byte>.Count;
                // Unsigned, so it shouldn't have overflowed larger than length (rather than negative)
                Debug.Assert(lengthToExamine < length);
                if (lengthToExamine != 0)
                {
                    do
                    {
                        if (Vector512.LoadUnsafe(ref first, offset) !=
                            Vector512.LoadUnsafe(ref second, offset))
                        {
                            goto NotEqual;
                        }
                        offset += (nuint)Vector512<byte>.Count;
                    } while (lengthToExamine > offset);
                }

                // Do final compare as Vector512<byte>.Count from end rather than start
                if (Vector512.LoadUnsafe(ref first, lengthToExamine) ==
                    Vector512.LoadUnsafe(ref second, lengthToExamine))
                {
                    // C# compiler inverts this test, making the outer goto the conditional jmp.
                    goto Equal;
                }

                // This becomes a conditional jmp forward to not favor it.
                goto NotEqual;
            }
            else if (Vector256.IsHardwareAccelerated && length >= (nuint)Vector256<byte>.Count)
            {
                nuint offset          = 0;
                nuint lengthToExamine = length - (nuint)Vector256<byte>.Count;
                // Unsigned, so it shouldn't have overflowed larger than length (rather than negative)
                Debug.Assert(lengthToExamine < length);
                if (lengthToExamine != 0)
                {
                    do
                    {
                        if (Vector256.LoadUnsafe(ref first, offset) !=
                            Vector256.LoadUnsafe(ref second, offset))
                        {
                            goto NotEqual;
                        }
                        offset += (nuint)Vector256<byte>.Count;
                    } while (lengthToExamine > offset);
                }

                // Do final compare as Vector256<byte>.Count from end rather than start
                if (Vector256.LoadUnsafe(ref first, lengthToExamine) ==
                    Vector256.LoadUnsafe(ref second, lengthToExamine))
                {
                    // C# compiler inverts this test, making the outer goto the conditional jmp.
                    goto Equal;
                }

                // This becomes a conditional jmp forward to not favor it.
                goto NotEqual;
            }
            else if (length >= (nuint)Vector128<byte>.Count)
            {
                nuint offset          = 0;
                nuint lengthToExamine = length - (nuint)Vector128<byte>.Count;
                // Unsigned, so it shouldn't have overflowed larger than length (rather than negative)
                Debug.Assert(lengthToExamine < length);
                if (lengthToExamine != 0)
                {
                    do
                    {
                        if (Vector128.LoadUnsafe(ref first, offset) !=
                            Vector128.LoadUnsafe(ref second, offset))
                        {
                            goto NotEqual;
                        }
                        offset += (nuint)Vector128<byte>.Count;
                    } while (lengthToExamine > offset);
                }

                // Do final compare as Vector128<byte>.Count from end rather than start
                if (Vector128.LoadUnsafe(ref first, lengthToExamine) ==
                    Vector128.LoadUnsafe(ref second, lengthToExamine))
                {
                    // C# compiler inverts this test, making the outer goto the conditional jmp.
                    goto Equal;
                }

                // This becomes a conditional jmp forward to not favor it.
                goto NotEqual;
            }
        }

        #if TARGET_64BIT
            if (Vector128.IsHardwareAccelerated)
            {
                Debug.Assert(length <= (nuint)sizeof(nuint) * 2);

                nuint offset = length - (nuint)sizeof(nuint);
                nuint differentBits = LoadNUInt(ref first) - LoadNUInt(ref second);
                differentBits |= LoadNUInt(ref first, offset) - LoadNUInt(ref second, offset);
                result = (differentBits == 0);
                goto Result;
            }
            else
        #endif
        {
            Debug.Assert(length >= (nuint)sizeof(nuint));
            {
                nuint offset          = 0;
                nuint lengthToExamine = length - (nuint)sizeof(nuint);
                // Unsigned, so it shouldn't have overflowed larger than length (rather than negative)
                Debug.Assert(lengthToExamine < length);
                if (lengthToExamine > 0)
                {
                    do
                    {
                        // Compare unsigned so not do a sign extend mov on 64 bit
                        if (LoadNUInt(ref first, offset) != LoadNUInt(ref second, offset))
                        {
                            goto NotEqual;
                        }
                        offset += (nuint)sizeof(nuint);
                    } while (lengthToExamine > offset);
                }

                // Do final compare as sizeof(nuint) from end rather than start
                result = (LoadNUInt(ref first, lengthToExamine) == LoadNUInt(ref second, lengthToExamine));
                goto Result;
            }
        }

        // As there are so many true/false exit points the Jit will coalesce them to one location.
        // We want them at the end so the conditional early exit jmps are all jmp forwards so the
        // branch predictor in a uninitialized state will not take them e.g.
        // - loops are conditional jmps backwards and predicted
        // - exceptions are conditional forwards jmps and not predicted
    NotEqual:
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint LoadNUInt(ref byte start, nuint offset) => Unsafe.ReadUnaligned<nuint>(ref Unsafe.AddByteOffset(ref start, offset));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ushort LoadUShort(ref byte start) => Unsafe.ReadUnaligned<ushort>(ref start);
}
