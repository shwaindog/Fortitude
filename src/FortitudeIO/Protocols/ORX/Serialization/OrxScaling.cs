namespace FortitudeIO.Protocols.ORX.Serialization;

public static class OrxScaling
{
    private static readonly decimal[] Factors =
    {
        0m, 0.0000001m, 0.000001m, 0.00001m, 0.0001m, 0.001m, 0.01m, 0.1m, 1m, 10m, 100m, 1000m, 10000m, 100000m
        , 1000000m, 10000000m
    };

    public static decimal Unscale(uint value, byte factor) => (int)value * Factors[16 - factor];

    public static uint Scale(decimal value, byte factor) => (uint)(int)(value * Factors[factor]);

    public static byte GetScalingFactor(decimal price)
    {
        price = Math.Abs(price);
        byte factor = 15;
        while (price * Factors[factor] > int.MaxValue && factor > 8) factor--;
        return factor;
    }
}
