// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

public static class PQScaling
{
    private const  byte FactorMask   = (byte)PQFieldFlags.DecimalScaleBits;
    internal const byte NegativeMask = (byte)PQFieldFlags.NegativeBit;

    internal const ulong Max48BitsULong = 0xFFFF_FFFF_FFFF;
    internal const ulong Max32BitsULong = 0xFFFF_FFFF;

    public const byte NegativeAndScaleMask = (byte)PQFieldFlags.NegativeAndScaleMask;

    private static readonly decimal[] Factors =
    {
        0m
      , 0.0000001m
      , 0.000001m
      , 0.00001m
      , 0.0001m
      , 0.001m
      , 0.01m
      , 0.1m
      , 1m
      , 10m
      , 100m
      , 1000m
      , 10000m
      , 100000m
      , 1000000m
      , 10000000m
    };

    public static PQFieldFlags FindFlagForDecimalPlacesShift(int numberOfDecimalPlaces)
    {
        if (numberOfDecimalPlaces > 7) numberOfDecimalPlaces  = 7;
        if (numberOfDecimalPlaces < -7) numberOfDecimalPlaces = -7;
        return (PQFieldFlags)(8 - numberOfDecimalPlaces);
    }

    public static decimal Unscale
        (uint value, PQFieldFlags flag) =>
        value * Factors[(byte)flag & FactorMask] * (((ushort)flag & NegativeMask) > 0 ? -1 : 1);

    public static long UnscaleLong
        (long value, PQFieldFlags flag) =>
        (long)(value * Factors[(byte)flag & FactorMask] * (((ushort)flag & NegativeMask) > 0 ? -1 : 1));

    public static uint Scale(decimal value, PQFieldFlags flag) => (uint)(Math.Abs(value) * Factors[16 - ((byte)flag & FactorMask)]);

    public static ulong ScaleToLong(decimal value, PQFieldFlags flag) => (ulong)(Math.Abs(value) * Factors[16 - ((byte)flag & FactorMask)]);

    public static ulong ScaleDownLongTo48Bits(long value)
    {
        var absValue = (ulong)Math.Abs(value);
        if ((absValue & Max48BitsULong) == 0) return absValue;
        var scaleFactor = FindVolumeScaleFactor48Bits(value);
        return (ulong)(Math.Abs(value) * Factors[16 - ((byte)scaleFactor & FactorMask)]);
    }

    public static ulong ScaleDownLongTo32Bits(long value)
    {
        var absValue = (ulong)Math.Abs(value);
        if ((absValue & Max32BitsULong) == 0) return absValue;
        var scaleFactor = FindVolumeScaleFactor32Bits(value);
        return (ulong)(Math.Abs(value) * Factors[16 - ((byte)scaleFactor & FactorMask)]);
    }

    public static PQFieldFlags FindVolumeScaleFactor48Bits(long value)
    {
        var absValue = (ulong)Math.Abs(value);
        if (absValue == 0m) return (PQFieldFlags)8;
        var currentScale = absValue;
        var count        = 0;
        while ((currentScale & Max48BitsULong) != 0)
        {
            count++;
            currentScale /= 10;
        }
        if (count < 8) return (PQFieldFlags)(8 + count);
        return (PQFieldFlags)15;
    }

    public static PQFieldFlags FindVolumeScaleFactor32Bits(long value)
    {
        var absValue = (ulong)Math.Abs(value);
        if (absValue == 0m) return (PQFieldFlags)8;
        var currentScale = absValue;
        var count        = 0;
        while ((currentScale & Max32BitsULong) != 0)
        {
            count++;
            currentScale /= 10;
        }
        if (count < 8) return (PQFieldFlags)(8 + count);
        return (PQFieldFlags)15;
    }

    public static PQFieldFlags FindPriceScaleFactor(decimal precisionExample)
    {
        if (precisionExample == 0m) return (PQFieldFlags)8;
        if (precisionExample < 1_000_000m && precisionExample.Scale is > 0 and < 8) return (PQFieldFlags)(8 - precisionExample.Scale);
        if (precisionExample is >= 1m and < 10m) return (PQFieldFlags)8;
        var currentScale = precisionExample;
        var count        = 0;
        while (currentScale % 10 == 0 && currentScale > 1_000_000_000)
        {
            count++;
            currentScale /= 10;
        }
        if (count < 8) return (PQFieldFlags)(8 + count);
        return (PQFieldFlags)15;
    }

    public static PQFieldFlags FindVolumeScaleFactor(decimal precisionExample)
    {
        if (precisionExample == 0m) return (PQFieldFlags)8;
        if (precisionExample < 10m && precisionExample.Scale is > 0 and < 8) return (PQFieldFlags)(8 - precisionExample.Scale);
        if (precisionExample is >= 1m and < 10m) return (PQFieldFlags)8;
        var currentScale = precisionExample;
        var count        = 0;
        while (currentScale % 10 == 0 && currentScale > 0)
        {
            count++;
            currentScale /= 10;
        }
        if (count < 8) return (PQFieldFlags)(8 + count);
        return (PQFieldFlags)15;
    }

    public static uint AutoScale(decimal value, int maxNumberOfSignificantDigits, out PQFieldFlags flagSelected)
    {
        int afterDecimalPoint = BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
        var abs = Math.Abs(value);
        var beforeDecimalPoint = abs < 1 ? 0 : (int)(Math.Log10(decimal.ToDouble(abs)) + 1);
        var totalSignificantDigits = afterDecimalPoint + beforeDecimalPoint;
        var rounded = totalSignificantDigits > maxNumberOfSignificantDigits ? decimal.Round(value, maxNumberOfSignificantDigits) : value;
        var decimalPointDelta = totalSignificantDigits < maxNumberOfSignificantDigits ? 0 : totalSignificantDigits - maxNumberOfSignificantDigits;
        afterDecimalPoint -= decimalPointDelta;

        flagSelected = beforeDecimalPoint < maxNumberOfSignificantDigits
            ? FindFlagForDecimalPlacesShift(afterDecimalPoint)
            : FindFlagForDecimalPlacesShift(maxNumberOfSignificantDigits - beforeDecimalPoint);

        flagSelected |= (PQFieldFlags)(value < 0 ? NegativeMask : 0);
        return Scale(rounded, (PQFieldFlags)((byte)flagSelected & 0x1F));
    }

    public static byte GetScalingFactor(decimal precision)
    {
        byte factor = 8;
        while (precision * Factors[factor] != Math.Round(precision * Factors[factor]) && factor <= 15) factor++;
        return (byte)(16 - factor);
    }
}
