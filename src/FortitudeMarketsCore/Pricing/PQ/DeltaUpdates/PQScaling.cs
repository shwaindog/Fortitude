namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

public static class PQScaling
{
    private const byte FactorMask = 0x0F;
    internal const byte NegativeMask = 0x10;

    private static readonly decimal[] Factors =
    {
        0m, 0.0000001m, 0.000001m, 0.00001m, 0.0001m, 0.001m, 0.01m, 0.1m, 1m, 10m, 100m, 1000m, 10000m, 100000m
        , 1000000m, 10000000m
    };

    public static byte FindFlagForDecimalPlacesShift(int numberOfDecimalPlaces)
    {
        if (numberOfDecimalPlaces > 7) numberOfDecimalPlaces = 7;
        if (numberOfDecimalPlaces < -7) numberOfDecimalPlaces = -7;
        return (byte)(8 - numberOfDecimalPlaces);
    }

    public static decimal Unscale(uint value, byte flag) =>
        value * Factors[flag & FactorMask] * ((flag & NegativeMask) > 0 ? -1 : 1);

    public static uint Scale(decimal value, byte flag) => (uint)(Math.Abs(value) * Factors[16 - (flag & FactorMask)]);

    public static uint AutoScale(decimal value, int maxNumberOfSignificantDigits, out byte flagSelected)
    {
        int afterDecimalPoint = BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
        var abs = Math.Abs(value);
        var beforeDecimalPoint = abs < 1 ? 0 : (int)(Math.Log10(decimal.ToDouble(abs)) + 1);
        var totalSignificantDigits = afterDecimalPoint + beforeDecimalPoint;
        var rounded = totalSignificantDigits > maxNumberOfSignificantDigits ?
            decimal.Round(value, maxNumberOfSignificantDigits) :
            value;
        var decimalPointDelta = totalSignificantDigits < maxNumberOfSignificantDigits ?
            0 :
            totalSignificantDigits - maxNumberOfSignificantDigits;
        afterDecimalPoint -= decimalPointDelta;

        flagSelected = beforeDecimalPoint < maxNumberOfSignificantDigits ?
            FindFlagForDecimalPlacesShift(afterDecimalPoint) :
            FindFlagForDecimalPlacesShift(maxNumberOfSignificantDigits - beforeDecimalPoint);

        flagSelected |= (byte)(value < 0 ? NegativeMask : 0);
        return Scale(rounded, (byte)(flagSelected & 0x1F));
    }

    public static byte GetScalingFactor(decimal precision)
    {
        byte factor = 8;
        while (precision * Factors[factor] != Math.Round(precision * Factors[factor]) && factor <= 15) factor++;
        return (byte)(16 - factor);
    }
}
