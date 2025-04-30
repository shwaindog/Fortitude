// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;

#endregion

namespace FortitudeCommon.Extensions;

public static class NumberFormattingExtensions
{
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
}
