// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

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

    public static int NextPowerOfTwo(this int value)
    {
        var ceiling = 2;

        while (ceiling < value) ceiling *= 2;

        return ceiling;
    }

    public static string ToHex2(this ulong toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 8;
        var currentShiftAmount = 7;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X2}" : "{0:x2}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static string ToHex4(this ulong toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 16;
        var currentShiftAmount = 3;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFFFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X4}" : "{0:x4}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static string ToHex2(this long toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 8;
        var currentShiftAmount = 7;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X2}" : "{0:x2}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static string ToHex4(this long toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 16;
        var currentShiftAmount = 3;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFFFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X4}" : "{0:x4}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static string ToHex2(this int toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 8;
        var currentShiftAmount = 3;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X2}" : "{0:x2}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static string ToHex4(this int toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 16;
        var currentShiftAmount = 1;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFFFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X4}" : "{0:x4}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static string ToHex2(this uint toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 8;
        var currentShiftAmount = 3;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X2}" : "{0:x2}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static string ToHex2(this ushort toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 8;
        var currentShiftAmount = 3;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X2}" : "{0:x2}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static string ToHex4(this uint toConvert, bool isUpperCaseHex = true)
    {
        var sb                 = new StringBuilder();
        var groupShift         = 16;
        var currentShiftAmount = 1;
        while (currentShiftAmount >= 0)
        {
            var currentByteGroup = (byte)((toConvert >> (currentShiftAmount * groupShift)) & 0xFFFF);
            sb.AppendFormat(isUpperCaseHex ? "{0:X4}" : "{0:x4}", currentByteGroup);
            if (currentShiftAmount > 0) sb.Append("_");
            --currentShiftAmount;
        }
        return sb.ToString();
    }

    public static int NumOfDigits(this int findDigits, bool includeMinusSign = true)
    {
        if (findDigits is >= 0 and < 10) return 1;
        if (findDigits is < 0 and > -10) return includeMinusSign ? 2 : 1;
        return 1 + (findDigits / 10).NumOfDigits();
    }

    public static int NumOfDigits(this uint findDigits)
    {
        if (findDigits is >= 0 and < 10) return 1;
        return 1 + (findDigits / 10).NumOfDigits();
    }

    public static int NumOfDigits(this long findDigits, bool includeMinusSign = true)
    {
        if (findDigits is >= 0 and < 10) return 1;
        if (findDigits is < 0 and > -10) return includeMinusSign ? 2 : 1;
        return 1 + (findDigits / 10).NumOfDigits();
    }

    public static int NumOfDigits(this ulong findDigits)
    {
        if (findDigits is >= 0 and < 10) return 1;
        return 1 + (findDigits / 10).NumOfDigits();
    }


    public const ulong KiloByte  = 1024;
    public const ulong MegaByte  = KiloByte * KiloByte;
    public const ulong GigaByte  = KiloByte * MegaByte;
    public const ulong TeraByte  = KiloByte * GigaByte;
    public const ulong PetaByte  = KiloByte * TeraByte;
    public const ulong ExaByte   = KiloByte * PetaByte;

    public static ulong AsKiloBytes(this ulong change) => change * KiloByte;
    public static ulong AsMegaBytes(this ulong change) => change * MegaByte;
    public static ulong AsGigaBytes(this ulong change) => change * GigaByte;
    public static ulong AsTeraBytes(this ulong change) => change * TeraByte;
    public static ulong AsPetaBytes(this ulong change) => change * PetaByte;
    public static ulong AsExaBytes(this ulong change) => change * ExaByte;


    private static readonly (string, ulong)[] byteSuffixesAndSizes =
    [
        ("b|byte", 1)
      , ("kb,kilobyte", KiloByte)
      , ("mb,megabyte", MegaByte)
      , ("gb,gigabyte", GigaByte)
      , ("tb,terabyte", TeraByte)
      , ("pb,petabyte", PetaByte)
      , ("eb,exabyte", ExaByte)
    ];

    public static ulong ToSizeInBytes(this ulong value, string suffixLowerCase)
    {
        if (suffixLowerCase[^1] == 's')
        {
            suffixLowerCase = suffixLowerCase.Substring(0, suffixLowerCase.Length - 1);
        }
        foreach (var (suffix, multiplier) in byteSuffixesAndSizes)
        {
            var split = suffix.Split(',');
            foreach (var suffixType in split)
            {
                if (suffixType == suffixLowerCase)
                {
                    return value * multiplier;
                }
            }
        }
        throw new ArgumentException($"Expected suffixLowerCase to be one of {byteSuffixesAndSizes.Select((suffix, _) => suffix)}");
    }
}
