// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public static class JsonFormatterExtensions
{
    public static readonly Type[] DoubleQuoteWrappedSpanFormattableTypes =
    [
        typeof(char)
      , typeof(char?)
      , typeof(Int128)
      , typeof(Int128?)
      , typeof(UInt128)
      , typeof(UInt128?)
      , typeof(BigInteger)
      , typeof(BigInteger?)
      , typeof(Complex)
      , typeof(Complex?)
      , typeof(DateTime)
      , typeof(DateTime?)
      , typeof(TimeSpan)
      , typeof(TimeSpan?)
      , typeof(DateOnly)
      , typeof(DateOnly?)
      , typeof(TimeOnly)
      , typeof(TimeOnly?)
      , typeof(IPNetwork)
      , typeof(IPNetwork?)
      , typeof(Rune)
      , typeof(Rune?)
      , typeof(Uri)
      , typeof(Guid)
      , typeof(Guid?)
      , typeof(IPAddress)
      , typeof(Version)
    ];

    public static bool IsDoubleQuoteDelimitedSpanFormattable<T>(this T check)
    {
        var typeOfT = typeof(T) == typeof(Type) ? (Type)(object)check! : typeof(T);
        var nullableSpanFormattable = (typeOfT.IsSpanFormattableOrNullable());
        var doubleQtDelimited = (nullableSpanFormattable && !typeOfT.IsJsonStringExemptType());

        if (!doubleQtDelimited)
        {
            switch (check)
            {
                case Half halfSource:     doubleQtDelimited = Half.IsNaN(halfSource); break;
                case float floatSource:   doubleQtDelimited = float.IsNaN(floatSource); break;
                case double doubleSource: doubleQtDelimited = double.IsNaN(doubleSource); break;
            }
        }
        return doubleQtDelimited;
    }
    
    public static bool IsJsonStringExemptType(this Type checkType)
    {
        if (checkType.IsUniversalStringExemptNumberType()) return true;
        if (checkType.IsBool()) return true;
        if (checkType.IsNullableBool()) return true;
        return false;
    }
}
