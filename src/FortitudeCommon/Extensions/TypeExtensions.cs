using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Extensions;

public static class TypeExtensions
{
    public static bool IsCollection(this Type type) =>
        type.GetInterfaces().Any(i => i == typeof(ICollection) || i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));

    public static bool IsNotCollection(this Type type) => !type.IsCollection();

    public static bool IsArray(this Type type)                  => typeof(Array).IsAssignableFrom(type);
    
    public static bool IsNotArray(this Type type)               => !type.IsArray();

    public static bool IsKeyValuePair(this Type type) => type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
    public static bool IsNotKeyValuePair(this Type type) => !type.IsKeyValuePair();
    
    public static bool IsArrayOf(this Type type, Type itemType) => type.IsArray() && type.GetElementType() == itemType;

    public static bool IsReadOnlyList(this Type type) =>
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyList<>));
    
    public static bool IsNotReadOnlyList(this Type type) => !type.IsReadOnlyList();

    public static bool IsIndexableOrderedCollection(this Type type) => type.IsArray() || type.IsReadOnlyList();
    
    public static Type? GetArrayElementType(this Type type) => type.IsArray() ? type.GetElementType() : null;

    public static Type? GetListElementType(this Type type) =>
        type.IsReadOnlyList() ? type.GetGenericArguments().FirstOrDefault() : null;

    public static Type? GetIndexedCollectionElementType(this Type type) => type.GetArrayElementType() ?? type.GetListElementType();
    
    public static bool IsReadOnlyDictionaryType(this Type type) =>
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>));
    
    public static bool IsNotReadOnlyDictionaryType(this Type type) => !type.IsReadOnlyDictionaryType();
    
    public static bool IsKeyValueOrderedCollection(this Type type) =>
        type.IsIndexableOrderedCollection() && (type.GetIndexedCollectionElementType()?.IsKeyValuePair() ?? false) ;
    
    public static bool IsNotKeyValueOrderedCollection(this Type type) => !type.IsKeyValueOrderedCollection();
    
    public static bool IsKeyedCollection(this Type type) =>
        type.IsReadOnlyDictionaryType() || type.IsKeyValueOrderedCollection();
    
    public static bool IsNotKeyedCollection(this Type type) => !type.IsKeyedCollection();
    
    public static KeyValuePair<Type, Type>? GetKeyedCollectionTypes(this Type type)
    {
        if (type.IsReadOnlyDictionaryType())
        {
            var dictionaryInterface = 
                type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>))!;
            return new KeyValuePair<Type, Type>(dictionaryInterface.GenericTypeArguments[0], dictionaryInterface.GenericTypeArguments[1]);
        } 
        if (type.IsKeyValueOrderedCollection())
        {
            var keyValueType = type.GetIndexedCollectionElementType()!;
            return new KeyValuePair<Type, Type>(keyValueType.GenericTypeArguments[0], keyValueType.GenericTypeArguments[1]);
        }
        if (type.IsIterable())
        {
            var iterableType = type.GetIterableElementType();
            if (iterableType?.IsKeyValuePair() ?? false)
            {
                return new KeyValuePair<Type, Type>(iterableType.GenericTypeArguments[0], iterableType.GenericTypeArguments[1]);
            }
        }
        return null;
    }

    public static bool IsReadOnlyListOf(this Type type, Type itemType) =>
        type.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyList<>)
                                      && i.GetGenericArguments()[0] == itemType);

    public static bool IsReadOnlyListSupporting(this Type type, Type itemType) =>
        type.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyList<>)
                                      && i.GetGenericArguments()[0].IsAssignableFrom(itemType));

    public static bool Supports(this Type type, Type maybeDerivedType) => type.IsAssignableFrom(maybeDerivedType);

    public static bool Derives(this Type type, Type maybeBaseType) => maybeBaseType.IsAssignableFrom(type);

    public static bool IsValueTypeArray(this Type type)  => typeof(Array).IsAssignableFrom(type) && (type.GetElementType()?.IsValueType ?? false);
    public static bool IsObjectTypeArray(this Type type) => typeof(Array).IsAssignableFrom(type) && !(type.GetElementType()?.IsValueType ?? false);

    public static bool IsCollectionOf(this Type type, Type itemType) =>
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)
                                                      && i.GetGenericArguments()[0] == itemType);

    public static bool IsEnumerable(this Type type) =>
        type.GetInterfaces().Any(i => i == typeof(IEnumerable) || i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

    public static bool  IsNotEnumerable(this Type type)   => !type.IsEnumerable();
    public static Type? GetEnumerableElementType(this Type type) => type.IsEnumerator() ? type.GetElementType() : null;
    
    public static bool IsEnumerator(this Type type) =>
        type.GetInterfaces().Any(i => i == typeof(IEnumerator) || i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerator<>));
    
    public static bool  IsNotEnumerator(this Type type)     => !type.IsEnumerator();
    public static Type? GetEnumeratorElementType(this Type type) => type.IsEnumerator() ? type.GetElementType() : null;

    public static bool IsIterable(this Type type) => type.IsEnumerable() || type.IsEnumerator();
    
    public static Type? GetIterableElementType(this Type type) => type.GetEnumerableElementType() ?? type.GetEnumeratorElementType();

    public static bool IsSpanFormattable(this Type type) => type.GetInterfaces().Any(i => i == typeof(ISpanFormattable));

    public static bool IsEnumerableOf(this Type type, Type itemType) =>
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                                                      && i.GetGenericArguments()[0] == itemType);

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

    public static bool IsByte(this Type check)              => check == typeof(byte) || check.IsByteEnum();
    public static bool IsNullableByte(this Type check)      => check == typeof(byte?);
    public static bool IsByteArray(this Type check)         => check == typeof(byte[]);
    public static bool IsNullableByteArray(this Type check) => check == typeof(byte?[]);

    public static bool IsByteList(this Type check) =>
        check == typeof(List<byte>)
     || check == typeof(IList<byte>)
     || check == typeof(IReadOnlyList<byte>);

    public static bool IsNullableByteList(this Type check) =>
        check == typeof(List<byte?>)
     || check == typeof(IList<byte?>)
     || check == typeof(IReadOnlyList<byte?>);

    public static bool IsShort(this Type check)              => check == typeof(short) || check.IsShortEnum();
    public static bool IsNullableShort(this Type check)      => check == typeof(short?);
    public static bool IsShortArray(this Type check)         => check == typeof(short[]);
    public static bool IsNullableShortArray(this Type check) => check == typeof(short?[]);

    public static bool IsShortList(this Type check) =>
        check == typeof(List<short>)
     || check == typeof(IList<short>)
     || check == typeof(IReadOnlyList<short>);

    public static bool IsNullableShortList(this Type check) =>
        check == typeof(List<short?>)
     || check == typeof(IList<short?>)
     || check == typeof(IReadOnlyList<short?>);

    public static bool IsUShort(this Type check)              => check == typeof(ushort) || check.IsUShortEnum();
    public static bool IsNullableUShort(this Type check)      => check == typeof(ushort?);
    public static bool IsUShortArray(this Type check)         => check == typeof(ushort[]);
    public static bool IsNullableUShortArray(this Type check) => check == typeof(ushort?[]);

    public static bool IsUShortList(this Type check) =>
        check == typeof(List<ushort>)
     || check == typeof(IList<ushort>)
     || check == typeof(IReadOnlyList<ushort>);

    public static bool IsNullableUShortList(this Type check) =>
        check == typeof(List<ushort?>)
     || check == typeof(IList<ushort?>)
     || check == typeof(IReadOnlyList<ushort?>);

    public static bool IsInt(this Type check)              => check == typeof(int) || check.IsIntEnum();
    public static bool IsNullableInt(this Type check)      => check == typeof(int?);
    public static bool IsIntArray(this Type check)         => check == typeof(int[]);
    public static bool IsNullableIntArray(this Type check) => check == typeof(int?[]);

    public static bool IsIntList(this Type check) =>
        check == typeof(List<int>)
     || check == typeof(IList<int>)
     || check == typeof(IReadOnlyList<int>);

    public static bool IsNullableIntList(this Type check) =>
        check == typeof(List<int?>)
     || check == typeof(IList<int?>)
     || check == typeof(IReadOnlyList<int?>);


    public static bool IsUInt(this Type check)              => check == typeof(uint) || check.IsUIntEnum();
    public static bool IsNullableUInt(this Type check)      => check == typeof(uint?);
    public static bool IsUIntArray(this Type check)         => check == typeof(uint[]);
    public static bool IsNullableUIntArray(this Type check) => check == typeof(uint?[]);

    public static bool IsUIntList(this Type check) =>
        check == typeof(List<uint>)
     || check == typeof(IList<uint>)
     || check == typeof(IReadOnlyList<uint>);

    public static bool IsNullableUIntList(this Type check) =>
        check == typeof(List<uint?>)
     || check == typeof(IList<uint?>)
     || check == typeof(IReadOnlyList<uint?>);

    public static bool IsLong(this Type check)              => check == typeof(long) || check.IsLongEnum();
    public static bool IsNullableLong(this Type check)      => check == typeof(long?);
    public static bool IsLongArray(this Type check)         => check == typeof(long[]);
    public static bool IsNullableLongArray(this Type check) => check == typeof(long?[]);

    public static bool IsLongList(this Type check) =>
        check == typeof(List<long>)
     || check == typeof(IList<long>)
     || check == typeof(IReadOnlyList<long>);

    public static bool IsNullableLongList(this Type check) =>
        check == typeof(List<long?>)
     || check == typeof(IList<long?>)
     || check == typeof(IReadOnlyList<long?>);

    public static bool IsULong(this Type check)              => check == typeof(long) || check.IsULongEnum();
    public static bool IsNullableULong(this Type check)      => check == typeof(ulong?);
    public static bool IsULongArray(this Type check)         => check == typeof(ulong[]);
    public static bool IsNullableULongArray(this Type check) => check == typeof(ulong?[]);

    public static bool IsULongList(this Type check) =>
        check == typeof(List<ulong>)
     || check == typeof(IList<ulong>)
     || check == typeof(IReadOnlyList<ulong>);

    public static bool IsNullableULongList(this Type check) =>
        check == typeof(List<ulong?>)
     || check == typeof(IList<ulong?>)
     || check == typeof(IReadOnlyList<ulong?>);

    public static bool IsDecimal(this Type check)              => check == typeof(decimal);
    public static bool IsNullableDecimal(this Type check)      => check == typeof(decimal?);
    public static bool IsDecimalArray(this Type check)         => check == typeof(decimal[]);
    public static bool IsNullableDecimalArray(this Type check) => check == typeof(decimal?[]);

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

    public static bool IsString(this Type check)      => check == typeof(string);
    public static bool IsNotString(this Type check)      => !check.IsString();
    public static bool IsStringArray(this Type check) => check == typeof(string[]);

    public static bool IsStringList(this Type check) =>
        check == typeof(List<string>)
     || check == typeof(IList<string>)
     || check == typeof(IReadOnlyList<string>);

    public static bool IsMutableString(this Type check)      => check == typeof(MutableString);
    public static bool IsMutableStringArray(this Type check) => check == typeof(MutableString[]);

    public static bool IsMutableStringList(this Type check) =>
        check == typeof(List<MutableString>)
     || check == typeof(IList<MutableString>)
     || check == typeof(IReadOnlyList<MutableString>);


    public static bool IsStringToStringMap(this Type check) =>
        check == typeof(Dictionary<string, string>)
     || check == typeof(IDictionary<string, string>)
     || check == typeof(IImmutableDictionary<string, string>);
}
