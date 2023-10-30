namespace FortitudeCommon.Types;

public static class StringConverts
{
    public static int? ToInt(this string? parse) => parse.IsNotNullOrEmpty() ? int.Parse(parse!) : null;
    public static uint? ToUInt(this string? parse) => parse.IsNotNullOrEmpty() ? uint.Parse(parse!) : null;
    public static float? ToFloat(this string? parse) => parse.IsNotNullOrEmpty() ? float.Parse(parse!) : null;
    public static decimal? ToDecimal(this string? parse) => parse.IsNotNullOrEmpty() ? decimal.Parse(parse!) : null;
    public static double? ToDouble(this string? parse) => parse.IsNotNullOrEmpty() ? double.Parse(parse!) : null;

    // ReSharper disable ReplaceWithStringIsNullOrEmpty
    public static bool IsNullOrEmpty(this string? value) => value is null or "";

    public static bool IsNotNullOrEmpty(this string? value) => value != null && value != "";
    // ReSharper restore ReplaceWithStringIsNullOrEmpty
}
