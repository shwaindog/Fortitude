using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Extensions;

public static class EnumExtensions
{
    public static Type EnumBase(this Type enumType) => Enum.GetUnderlyingType(enumType);

    public static string EnumBaseName(this Type enumType) => enumType.EnumBase().Name;
    
    public static bool IsEnum(this Type maybeEnumType) => maybeEnumType.IsEnum || maybeEnumType == typeof(Enum);
    
    public static bool IsEnumOrNullable(this Type maybeEnumType) => maybeEnumType.IfNullableGetUnderlyingTypeOrThis().IsEnum();

    public static bool IsByteEnum(this Type enumType)   => enumType.IsEnum && enumType.EnumBase() == typeof(byte);
    public static bool IsShortEnum(this Type enumType)  => enumType.IsEnum && enumType.EnumBase() == typeof(short);
    public static bool IsUShortEnum(this Type enumType) => enumType.IsEnum && enumType.EnumBase() == typeof(ushort);
    public static bool IsIntEnum(this Type enumType)    => enumType.IsEnum && enumType.EnumBase() == typeof(int);
    public static bool IsUIntEnum(this Type enumType)   => enumType.IsEnum && enumType.EnumBase() == typeof(uint);
    public static bool IsLongEnum(this Type enumType)   => enumType.IsEnum && enumType.EnumBase() == typeof(long);
    public static bool IsULongEnum(this Type enumType)  => enumType.IsEnum && enumType.EnumBase() == typeof(ulong);

    public static bool EnumFormatStringIsNotAsNumberFormatSequence(this string formatString)
    {
        if (formatString.Length == 0) return true;
        var formatSpan = formatString.AsSpan();
        if (!formatSpan.FormatStringHasFormatSequence()) return true;
        var formatSequence = formatSpan.ExtractFormatStringFormatSequence();
        return formatSequence.IsNeitherUpperOrLowerCaseOf("d");
    }

    public static string? IfEnumBaseName(this Type enumType) => enumType.IsEnum ? Enum.GetUnderlyingType(enumType).Name : null;
}