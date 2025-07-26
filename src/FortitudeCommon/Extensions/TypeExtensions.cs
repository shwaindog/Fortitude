using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Extensions;

public static class TypeExtensions
{
    public static bool IsBool(this Type check)              => check == typeof(bool);
    public static bool IsNullableBool(this Type check)      => check == typeof(bool?);
    public static bool IsBoolArray(this Type check)         => check == typeof(bool[]);
    public static bool IsNullableBoolArray(this Type check) => check == typeof(bool?[]);

    public static bool IsBoolList(this Type check) =>
        check == typeof(List<bool>)
     || check == typeof(IList<bool>)
     || check == typeof(IReadOnlyList<bool>);

    public static bool IsNullableBoolList(this Type check) =>
        check == typeof(List<bool?>)
     || check == typeof(IList<bool?>)
     || check == typeof(IReadOnlyList<bool?>);

    public static bool IsByte(this Type check)                  => check == typeof(byte) || check.IsByteEnum();
    public static bool IsNullableByte(this Type check)          => check == typeof(byte?);
    public static bool IsByteArray(this Type check)             => check == typeof(byte[]);
    public static bool IsNullableByteArray(this Type check)     => check == typeof(byte?[]);

    public static bool IsByteList(this Type check) =>
        check == typeof(List<byte>)
     || check == typeof(IList<byte>)
     || check == typeof(IReadOnlyList<byte>);

    public static bool IsNullableByteList(this Type check) =>
        check == typeof(List<byte?>)
     || check == typeof(IList<byte?>)
     || check == typeof(IReadOnlyList<byte?>);

    public static bool IsShort(this Type check)                 => check == typeof(short) || check.IsShortEnum();
    public static bool IsNullableShort(this Type check)         => check == typeof(short?);
    public static bool IsShortArray(this Type check)            => check == typeof(short[]);
    public static bool IsNullableShortArray(this Type check)    => check == typeof(short?[]);

    public static bool IsShortList(this Type check) =>
        check == typeof(List<short>)
     || check == typeof(IList<short>)
     || check == typeof(IReadOnlyList<short>);

    public static bool IsNullableShortList(this Type check) =>
        check == typeof(List<short?>)
     || check == typeof(IList<short?>)
     || check == typeof(IReadOnlyList<short?>);

    public static bool IsUShort(this Type check)                => check == typeof(ushort) || check.IsUShortEnum();
    public static bool IsNullableUShort(this Type check)        => check == typeof(ushort?);
    public static bool IsUShortArray(this Type check)           => check == typeof(ushort[]);
    public static bool IsNullableUShortArray(this Type check)   => check == typeof(ushort?[]);

    public static bool IsUShortList(this Type check) =>
        check == typeof(List<ushort>)
     || check == typeof(IList<ushort>)
     || check == typeof(IReadOnlyList<ushort>);

    public static bool IsNullableUShortList(this Type check) =>
        check == typeof(List<ushort?>)
     || check == typeof(IList<ushort?>)
     || check == typeof(IReadOnlyList<ushort?>);

    public static bool IsInt(this Type check)                   => check == typeof(int) || check.IsIntEnum();
    public static bool IsNullableInt(this Type check)           => check == typeof(int?);
    public static bool IsIntArray(this Type check)              => check == typeof(int[]);
    public static bool IsNullableIntArray(this Type check)      => check == typeof(int?[]);

    public static bool IsIntList(this Type check) =>
        check == typeof(List<int>)
     || check == typeof(IList<int>)
     || check == typeof(IReadOnlyList<int>);

    public static bool IsNullableIntList(this Type check) =>
        check == typeof(List<int?>)
     || check == typeof(IList<int?>)
     || check == typeof(IReadOnlyList<int?>);


    public static bool IsUInt(this Type check)                  => check == typeof(uint) || check.IsUIntEnum();
    public static bool IsNullableUInt(this Type check)          => check == typeof(uint?);
    public static bool IsUIntArray(this Type check)             => check == typeof(uint[]);
    public static bool IsNullableUIntArray(this Type check)     => check == typeof(uint?[]);

    public static bool IsUIntList(this Type check) =>
        check == typeof(List<uint>)
     || check == typeof(IList<uint>)
     || check == typeof(IReadOnlyList<uint>);

    public static bool IsNullableUIntList(this Type check) =>
        check == typeof(List<uint?>)
     || check == typeof(IList<uint?>)
     || check == typeof(IReadOnlyList<uint?>);

    public static bool IsLong(this Type check)                  => check == typeof(long) || check.IsLongEnum();
    public static bool IsNullableLong(this Type check)          => check == typeof(long?);
    public static bool IsLongArray(this Type check)             => check == typeof(long[]);
    public static bool IsNullableLongArray(this Type check)     => check == typeof(long?[]);

    public static bool IsLongList(this Type check) =>
        check == typeof(List<long>)
     || check == typeof(IList<long>)
     || check == typeof(IReadOnlyList<long>);

    public static bool IsNullableLongList(this Type check) =>
        check == typeof(List<long?>)
     || check == typeof(IList<long?>)
     || check == typeof(IReadOnlyList<long?>);

    public static bool IsULong(this Type check)                 => check == typeof(long) || check.IsULongEnum();
    public static bool IsNullableULong(this Type check)         => check == typeof(ulong?);
    public static bool IsULongArray(this Type check)            => check == typeof(ulong[]);
    public static bool IsNullableULongArray(this Type check)    => check == typeof(ulong?[]);

    public static bool IsULongList(this Type check) =>
        check == typeof(List<ulong>)
     || check == typeof(IList<ulong>)
     || check == typeof(IReadOnlyList<ulong>);

    public static bool IsNullableULongList(this Type check) =>
        check == typeof(List<ulong?>)
     || check == typeof(IList<ulong?>)
     || check == typeof(IReadOnlyList<ulong?>);

    public static bool IsDecimal(this Type check)               => check == typeof(decimal);
    public static bool IsNullableDecimal(this Type check)       => check == typeof(decimal?);
    public static bool IsDecimalArray(this Type check)          => check == typeof(decimal[]);
    public static bool IsNullableDecimalArray(this Type check)  => check == typeof(decimal?[]);

    public static bool IsDecimalList(this Type check) =>
        check == typeof(List<decimal>)
     || check == typeof(IList<decimal>)
     || check == typeof(IReadOnlyList<decimal>);

    public static bool IsNullableDecimalList(this Type check) =>
        check == typeof(List<decimal?>)
     || check == typeof(IList<decimal?>)
     || check == typeof(IReadOnlyList<decimal?>);

    public static bool IsDateTime(this Type check)              => check == typeof(DateTime);
    public static bool IsNullableDateTime(this Type check)      => check == typeof(DateTime?);
    public static bool IsDateTimeArray(this Type check)         => check == typeof(DateTime[]);
    public static bool IsNullableDateTimeArray(this Type check) => check == typeof(DateTime?[]);

    public static bool IsDateTimeList(this Type check) =>
        check == typeof(List<DateTime>)
     || check == typeof(IList<DateTime>)
     || check == typeof(IReadOnlyList<DateTime>);

    public static bool IsNullableDateTimeList(this Type check) =>
        check == typeof(List<DateTime?>)
     || check == typeof(IList<DateTime?>)
     || check == typeof(IReadOnlyList<DateTime?>);

    public static bool IsString(this Type check)                => check == typeof(string);
    public static bool IsStringArray(this Type check)           => check == typeof(string[]);

    public static bool IsStringList(this Type check) =>
        check == typeof(List<string>)
     || check == typeof(IList<string>)
     || check == typeof(IReadOnlyList<string>);

    public static bool IsMutableString(this Type check)         => check == typeof(MutableString);
    public static bool IsMutableStringArray(this Type check)    => check == typeof(MutableString[]);

    public static bool IsMutableStringList(this Type check) =>
        check == typeof(List<MutableString>)
     || check == typeof(IList<MutableString>)
     || check == typeof(IReadOnlyList<MutableString>);


    public static bool IsStringToStringMap(this Type check) =>
        check == typeof(Dictionary<string, string>)
     || check == typeof(IDictionary<string, string>)
     || check == typeof(IImmutableDictionary<string, string>);
}
