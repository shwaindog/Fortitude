// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public static class JsonFormatterExtensions
{
    public const char DblQtChar     = '"';
    public const char SqBrktOpnChar = '[';
    public const char BrcOpnChar    = '{';
    public const char SqBrktClsChar = ']';
    public const char BrcClsChar    = '}';
    
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

    public static bool IsValidJsonTypeOpening(this char check) => check is DblQtChar or SqBrktOpnChar or BrcOpnChar; 
    public static bool IsValidJsonTypeClosing(this char check) => check is DblQtChar or SqBrktClsChar or BrcClsChar; 

    private static ConcurrentDictionary<Type, bool> typeIsDoubleQtDelimited = new();
    private static ConcurrentDictionary<Type, bool> typeIsDoubleQtExemptCache = new();

    public static bool IsDoubleQuoteDelimitedSpanFormattable<T>(this T check)
    {
        return check.IsDoubleQuoteDelimitedSpanFormattable("", "");
    }

    public static bool IsDoubleQuoteDelimitedSpanFormattable<T>(this T check, ReadOnlySpan<char> fallbackValue, ReadOnlySpan<char> formatString)
    {
        var typeOfT = typeof(T) == typeof(Type) ? (Type)(object)check! : typeof(T);

        var formatStringDelmited = formatString.IsDblQtBounded() || formatString.IsSqBrktBounded() || formatString.IsBrcBounded();
        var nullableSpanFormattable = (typeOfT.IsSpanFormattableOrNullableCached());

        var doubleQtDelimited = 
            !formatStringDelmited 
         && typeIsDoubleQtDelimited.GetOrAdd(typeOfT, t => 
                                                 !t.IsEnumOrNullable() &&  nullableSpanFormattable && !t.IsJsonStringExemptType());

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

    public static bool IsJsonStringExemptTypeCached(this Type checkType) => 
        typeIsDoubleQtExemptCache.GetOrAdd(checkType, t => t.IsJsonStringExemptType());
}
