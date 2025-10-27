using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static System.Reflection.GenericParameterAttributes;

namespace FortitudeCommon.Extensions;

public static class TypeExtensions
{
    public static readonly Type ArrayType                 = typeof(Array);
    public static readonly Type ReadOnlyListTypeDef       = typeof(IReadOnlyList<>);
    public static readonly Type EnumerableType            = typeof(IEnumerable);
    public static readonly Type EnumerableTypeDef         = typeof(IEnumerable<>);
    public static readonly Type EnumeratorType            = typeof(IEnumerator);
    public static readonly Type EnumeratorTypeDef         = typeof(IEnumerator<>);
    public static readonly Type KeyValuePairTypeDef       = typeof(KeyValuePair<,>);
    public static readonly Type ReadOnlyDictionaryTypeDef = typeof(IReadOnlyDictionary<,>);
    public static readonly Type CollectionTypeDef         = typeof(ICollection<>);
    public static readonly Type CollectionType            = typeof(ICollection);
    public static readonly Type SpanFormattableType       = typeof(ISpanFormattable);
    public static readonly Type NullableTypeDef           = typeof(Nullable<>);


    public static bool ImplementsInterface<TInterface>(this Type type) => type.GetInterfaces().Contains(typeof(TInterface));
    public static bool ImplementsInterface(this Type type, Type checkImplementsThis) => type.GetInterfaces().Contains(checkImplementsThis);

    public static bool ImplementsGenericTypeInterface(this Type type, Type genericTypeDef) =>
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDef);

    public static bool ExtendsGenericBaseType(this Type type, Type genericTypeDef) =>
        ((type.BaseType?.IsGenericType ?? false) && type.BaseType.GetGenericTypeDefinition() == genericTypeDef) || 
        type.BaseType != null && type.BaseType != typeof(object) && type.BaseType.ExtendsGenericBaseType(genericTypeDef);

    public static bool IsCollection(this Type type) =>
        type.GetInterfaces().Any(i => i == CollectionType || i.IsGenericType && i.GetGenericTypeDefinition() == CollectionTypeDef);

    public static bool IsNotCollection(this Type type) => !type.IsCollection();

    public static bool IsArray(this Type type) => ArrayType.IsAssignableFrom(type);

    public static bool IsNotArray(this Type type) => !type.IsArray();

    public static bool IsOrImplements(this Type toCheck, Type checkImplements) =>
        toCheck == checkImplements || toCheck.GetInterfaces().Any(i => i == checkImplements);

    public static bool IsKeyValuePair(this Type type)    => type.GetGenericTypeDefinition() == KeyValuePairTypeDef;
    public static bool IsNotKeyValuePair(this Type type) => !type.IsKeyValuePair();

    public static bool IsArrayOf(this Type type, Type itemType) => type.IsArray() && type.GetElementType() == itemType;

    public static bool IsArraySupporting(this Type type, Type itemType) => type.IsArray() && type.GetElementType()!.Supports(itemType);

    public static bool IsNullableArrayOf(this Type type, Type valueType) => type.IsArray() && type.GetElementType()!.IsNullableOf(valueType);

    public static bool IsReadOnlyList(this Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == ReadOnlyListTypeDef ||
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ReadOnlyListTypeDef);

    public static Type? IfReadOnlyListGetElementType(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == ReadOnlyListTypeDef) { return type.GenericTypeArguments.FirstOrDefault(); }
        return type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == ReadOnlyListTypeDef)?.GenericTypeArguments
                   .FirstOrDefault();
    }

    public static bool IsReadOnlyListAndNotArray(this Type type) =>
        !type.IsArray() && type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ReadOnlyListTypeDef);

    public static bool IsReadOnlyListOf(this Type type, Type itemType) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == ReadOnlyListTypeDef && type.GetGenericArguments()[0] == itemType
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ReadOnlyListTypeDef
                                                      && i.GenericTypeArguments[0] == itemType);

    public static bool IsReadOnlyListSupporting(this Type type, Type itemType) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == ReadOnlyListTypeDef && type.GetGenericArguments()[0].Derives(itemType)
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ReadOnlyListTypeDef
                                                      && i.GenericTypeArguments[0].Derives(itemType));

    public static bool IsReadOnlyListOfNullable(this Type type, Type valueType) =>
        type.IsReadOnlyList() && type.IsGenericType && type.GenericTypeArguments[0].IsNullableOf(valueType)
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ReadOnlyListTypeDef
                                                      && i.GenericTypeArguments[0].IsNullableOf(valueType));

    public static bool IsNotReadOnlyList(this Type type) => !type.IsReadOnlyList();

    public static bool IsIndexableOrderedCollection(this Type type) => type.IsArray() || type.IsReadOnlyList();

    public static bool IsIndexableOrderedCollectionOfValueType(this Type type) =>
        type.IsArray() || type.IsReadOnlyList() && (type.GetIndexedCollectionElementType()!.IsValueType);

    public static bool IsNotIndexableOrderedCollectionOfValueType(this Type type) => !type.IsIndexableOrderedCollectionOfValueType();

    public static Type? IfArrayGetElementType(this Type type) => type.IsArray() ? type.GetElementType() : null;

    public static Type? GetListElementType(this Type type) => type.IsReadOnlyList() ? type.GenericTypeArguments.FirstOrDefault() : null;

    public static Type? GetIndexedCollectionElementType(this Type type) => type.IfArrayGetElementType() ?? type.IfReadOnlyListGetElementType();

    public static bool IsReadOnlyDictionaryType(this Type type) =>
        (type.IsGenericType && type.GetGenericTypeDefinition() == ReadOnlyDictionaryTypeDef)
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ReadOnlyDictionaryTypeDef);

    public static bool IsNotReadOnlyDictionaryType(this Type type) => !type.IsReadOnlyDictionaryType();

    public static bool IsKeyValueOrderedCollection(this Type type) =>
        type.IsIndexableOrderedCollection() && (type.GetIndexedCollectionElementType()?.IsKeyValuePair() ?? false);

    public static bool IsNotKeyValueOrderedCollection(this Type type) => !type.IsKeyValueOrderedCollection();

    public static bool IsKeyedCollection(this Type type) => type.IsReadOnlyDictionaryType() || type.IsKeyValueOrderedCollection();

    public static bool IsNotKeyedCollection(this Type type) => !type.IsKeyedCollection();

    public static KeyValuePair<Type, Type>? GetKeyedCollectionTypes(this Type type)
    {
        if (type.IsReadOnlyDictionaryType())
        {
            var dictionaryInterface =
                type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == ReadOnlyDictionaryTypeDef)!;
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

    public static bool Supports(this Type type, Type maybeDerivedType) => type.IsAssignableFrom(maybeDerivedType);

    public static bool Derives(this Type type, Type maybeBaseType) => maybeBaseType.IsAssignableFrom(type);

    public static bool IsValueTypeArray(this Type type)  => ArrayType.IsAssignableFrom(type) && (type.GetElementType()?.IsValueType ?? false);
    public static bool IsObjectTypeArray(this Type type) => ArrayType.IsAssignableFrom(type) && !(type.GetElementType()?.IsValueType ?? false);

    public static bool IsCollectionOf(this Type type, Type itemType) =>
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == CollectionTypeDef
                                                      && i.GenericTypeArguments[0] == itemType);

    public static bool IsEnumerable(this Type type) =>
        (type == EnumerableType || type.IsGenericType && type.GetGenericTypeDefinition() == EnumerableTypeDef) ||
        type.GetInterfaces().Any(i => i == EnumerableType || i.IsGenericType && i.GetGenericTypeDefinition() == EnumerableTypeDef);


    public static bool IsEnumerableOf(this Type type, Type itemType) =>
        (type.IsGenericType && type.GetGenericTypeDefinition() == EnumerableTypeDef && type.GenericTypeArguments[0] == itemType)
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == EnumerableTypeDef
                                                      && i.GenericTypeArguments[0] == itemType);


    public static bool IsEnumerableOfNullable(this Type type, Type itemType) =>
        (type.IsGenericType && type.GetGenericTypeDefinition() == EnumerableTypeDef && type.GenericTypeArguments[0].IsNullableOf(itemType))
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == EnumerableTypeDef
                                                      && i.GenericTypeArguments[0].IsNullableOf(itemType));


    public static bool IsEnumerableSupporting(this Type type, Type itemType) =>
        (type.IsGenericType && type.GetGenericTypeDefinition() == EnumerableTypeDef && type.GenericTypeArguments[0].Supports(itemType))
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == EnumerableTypeDef
                                                      && i.GenericTypeArguments[0].Supports(itemType));

    public static bool IsNotEnumerable(this Type type) => !type.IsEnumerable();

    public static Type? IfEnumerableGetElementType(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == EnumerableTypeDef) { return type.GenericTypeArguments.FirstOrDefault(); }
        return type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == EnumerableTypeDef)?.GenericTypeArguments
                   .FirstOrDefault();
    }

    public static bool IsEnumerator(this Type type) =>
        type.GetInterfaces().Any(i => i == EnumeratorType || i.IsGenericType && i.GetGenericTypeDefinition() == EnumeratorTypeDef);


    public static bool IsEnumeratorOf(this Type type, Type itemType) =>
        (type.IsGenericType && type.GetGenericTypeDefinition() == EnumeratorTypeDef && type.GenericTypeArguments[0] == itemType)
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == EnumeratorTypeDef
                                                      && i.GenericTypeArguments[0] == itemType);


    public static bool IsEnumeratorOfNullable(this Type type, Type itemType) =>
        (type.IsGenericType && type.GetGenericTypeDefinition() == EnumeratorTypeDef && type.GenericTypeArguments[0].IsNullableOf(itemType))
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == EnumeratorTypeDef
                                                      && i.GenericTypeArguments[0].IsNullableOf(itemType));


    public static bool IsEnumeratorSupporting(this Type type, Type itemType) =>
        (type.IsGenericType && type.GetGenericTypeDefinition() == EnumeratorTypeDef && type.GenericTypeArguments[0].Supports(itemType))
     || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == EnumeratorTypeDef
                                                      && i.GenericTypeArguments[0].Supports(itemType));

    public static bool IsNotEnumerator(this Type type) => !type.IsEnumerator();

    public static Type? IfEnumeratorGetElementType(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == EnumeratorTypeDef) { return type.GenericTypeArguments.FirstOrDefault(); }
        return type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == EnumeratorTypeDef)?.GenericTypeArguments
                   .FirstOrDefault();
    }

    public static bool IsIterable(this Type type) => type.IsEnumerable() || type.IsEnumerator();

    public static Type? GetIterableElementType(this Type type) =>
        type.GetIndexedCollectionElementType() ??
        type.IfEnumerableGetElementType() ?? type.IfEnumeratorGetElementType();

    public static bool IsSpanFormattable(this Type type) => 
        type == SpanFormattableType || type.GetInterfaces().Any(i => i == SpanFormattableType);

    public static bool IsNullable(this Type type) =>
        type is { IsValueType: true, IsGenericType: true } && type.GetGenericTypeDefinition() == NullableTypeDef;

    public static bool IsNotNullable(this Type type) => !type.IsNullable();

    public static Type IfNullableGetUnderlyingTypeOrThis(this Type type) =>
        type.IsNullable()
            ? type.GenericTypeArguments[0]
            : type;

    public static object? GetDefaultForUnderlyingNullableOrThis(this Type type) =>
        type.IsNullable()
            ? type.GenericTypeArguments[0].GetDefaultConstructorInstance()
            : type.GetDefaultConstructorInstance();

    public static T IfNullableGetNonNullableUnderlyingDefault<T>(this T? maybeNullable) =>
        maybeNullable ?? (typeof(T).IsNullable()
            ? (T)typeof(T).GenericTypeArguments[0].GetDefaultConstructorInstance()!
            : maybeNullable)!;

    public static T IfNullableGetNonNullableUnderlyingDefault<T>(this T? maybeNullable) where T : struct =>
        maybeNullable ?? default(T);

    public static object? GetDefaultConstructorInstance(this Type type) => Activator.CreateInstance(type);

    public static bool IsNullableOf(this Type type, Type valueType) =>
        valueType.IsValueType && type is { IsValueType: true, IsGenericType: true }
                              && type.GetGenericTypeDefinition() == NullableTypeDef
                              && type.GenericTypeArguments[0] == valueType;

    public static bool IsNullableSpanFormattable(this Type type) =>
        type is { IsValueType: true, IsGenericType: true } && type.GetGenericTypeDefinition() == NullableTypeDef
                                                           && type.GenericTypeArguments[0].IsSpanFormattable();

    public static bool IsSpanFormattableArray(this Type check) => check.IsArray() && check.IfArrayGetElementType()!.IsSpanFormattable();

    public static bool IsNullableSpanFormattableArray(this Type check) =>
        check.IsArray() && check.IfArrayGetElementType()!.IsNullableSpanFormattable();

    public static string AsCSharpKeywordOrName(this Type type)
    {
        var foundName =  type.Name switch
               {
                   "Boolean" => "bool"
                 , "Byte"    => "byte"
                 , "SByte"   => "sbyte"
                 , "Char"    => "char"
                 , "Int16"   => "short"
                 , "UInt16"  => "ushort"
                 , "Int32"   => "int"
                 , "UInt32"  => "uint"
                 , "Int64"   => "long"
                 , "UInt64"  => "ulong"
                 , "Single"  => "float"
                 , "Double"  => "double"
                 , "Decimal" => "decimal"
                 , "String"  => "string"
                 , _         => null
               };
        if(foundName != null) return foundName;
        if (type.IsArray)
        {
            type = type.GetElementType()!;
            var elementType = type.AsCSharpKeywordOrName();
            return elementType + "[]";
        }
        return type.Name;
    }


    public static string ShortNameInCSharpFormat(this Type typeNameToFriendlify, bool includeParamConstraints = true) =>
        typeNameToFriendlify.AppendShortNameInCSharpFormat(new MutableString(), includeParamConstraints).ToString();

    public static IStringBuilder AppendShortNameInCSharpFormat(this Type nameToPrint, IStringBuilder sb, bool includeParamConstraints = true)
    {
        var typeName = nameToPrint.AsCSharpKeywordOrName();
        if (nameToPrint.IsGenericType)
        {
            var indexOfTick = typeName.IndexOf('`');
            if (indexOfTick > 0)
            {
                var isNullable = nameToPrint.IsNullable();
                if (!isNullable)
                {
                    sb.Append(typeName[..indexOfTick]);
                    sb.Append('<');
                }
                if (nameToPrint.ContainsGenericParameters)
                {
                    var genericTypeParams = nameToPrint.GetTypeInfo().GenericTypeParameters;
                    for (var i = 0; i < genericTypeParams.Length; i++)
                    {
                        var genericParam = genericTypeParams[i];
                        if (i > 0) sb.Append(", ");

                        var constraint = genericParam.GenericParameterAttributes;
                        if ((constraint & Contravariant) > 0)
                        {
                            sb.Append("in ");
                            break;
                        }
                        if ((constraint & Covariant) > 0)
                        {
                            sb.Append("out ");
                            break;
                        }
                        genericParam.AppendShortNameInCSharpFormat(sb, false);
                    }
                }
                for (var i = 0; i < nameToPrint.GenericTypeArguments.Length; i++)
                {
                    var genericTypeArg = nameToPrint.GenericTypeArguments[i];

                    if (i > 0) sb.Append(", ");
                    genericTypeArg.AppendShortNameInCSharpFormat(sb, false);
                    if (isNullable) sb.Append('?');
                }
                if (!isNullable) sb.Append('>');

                if (includeParamConstraints && nameToPrint.ContainsGenericParameters)
                {
                    var genericTypeParams = nameToPrint.GetTypeInfo().GenericTypeParameters;

                    for (var i = 0; i < genericTypeParams.Length; i++)
                    {
                        var genericParam         = genericTypeParams[i];
                        var appendCount          = 0;
                        var constraint           = genericParam.GenericParameterAttributes;
                        var paramInterfaces      = genericParam.GetInterfaces();
                        var genericParamBaseType = genericParam.BaseType;
                        if ((constraint == None ||
                             ((constraint & VarianceMask) == constraint)) &&
                            (genericParamBaseType == null || genericParamBaseType == typeof(object))
                         && paramInterfaces.Length == 0) { continue; }
                        sb.Append(" where ");
                        genericParam.AppendShortNameInCSharpFormat(sb);
                        sb.Append(" : ");
                        if ((constraint & SpecialConstraintMask) == ReferenceTypeConstraint)
                        {
                            sb.Append("class");
                            appendCount++;
                        }
                        else if ((constraint & SpecialConstraintMask) == DefaultConstructorConstraint)
                        {
                            sb.Append("new()");
                            appendCount++;
                        }
                        else if ((constraint & SpecialConstraintMask) == NotNullableValueTypeConstraint)
                        {
                            sb.Append("notnull");
                            appendCount++;
                            
                        }
                        if (genericParamBaseType != null)
                        {
                            if (genericParamBaseType == typeof(ValueType))
                            {
                                sb.Append("struct");
                                appendCount++;
                            } else if (genericParamBaseType != typeof(object))
                            {
                                if (appendCount++ > 0) { sb.Append(", "); }
                                genericParamBaseType.AppendShortNameInCSharpFormat(sb, false);
                            }
                        }
                        for (var k = 0; k < paramInterfaces.Length; k++)
                        {
                            var paramInterface = paramInterfaces[k];
                            var isImplicit = false;
                            for (var l = 0; l < paramInterfaces.Length; l++)
                            {
                                var checkInheritsFrom = paramInterfaces[l];
                                if (checkInheritsFrom != paramInterface && checkInheritsFrom.ImplementsInterface(paramInterface))
                                {
                                    isImplicit = true;
                                    break;
                                }
                            }
                            if (!isImplicit)
                            {
                                if (appendCount++ > 0) { sb.Append(", "); }
                                paramInterface.AppendShortNameInCSharpFormat(sb, false);
                            }
                        }
                    }
                }
            }
            else
                sb.Append(typeName);
            return sb;
        }
        return sb.Append(typeName);
    }


    public static bool IsBool(this Type check)              => check == typeof(bool);
    public static bool IsNullableBool(this Type check)      => check == typeof(bool?);
    public static bool IsBoolArray(this Type check)         => check == typeof(bool[]);
    public static bool IsNullableBoolArray(this Type check) => check == typeof(bool?[]);

    public static bool IsBoolList(this Type check) => check.IsReadOnlyListOf(typeof(bool));

    public static bool IsNullableBoolList(this Type check) => check.IsReadOnlyListOfNullable(typeof(bool));

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

    public static bool IsChar(this Type check)                 => check == typeof(char);
    public static bool IsNotChar(this Type check)              => !check.IsChar();
    public static bool IsNullableChar(this Type check)         => check == typeof(char?);
    public static bool IsNotNullableChar(this Type check)      => !check.IsNullableChar();
    public static bool IsCharArray(this Type check)            => check == typeof(char[]);
    public static bool IsNotCharArray(this Type check)         => !check.IsCharArray();
    public static bool IsNullableCharArray(this Type check)    => check == typeof(char?[]);
    public static bool IsNotNullableCharArray(this Type check) => !check.IsNullableCharArray();

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

    public static bool IsString(this Type check)         => check == typeof(string);
    public static bool IsNotString(this Type check)      => !check.IsString();
    public static bool IsStringArray(this Type check)    => check == typeof(string[]);
    public static bool IsNotStringArray(this Type check) => !check.IsStringArray();

    public static bool IsCharSequence(this Type check)                => check.IsOrImplements(typeof(ICharSequence));
    public static bool IsNotCharSequence(this Type check)             => !check.IsCharSequence();
    public static bool IsCharSequenceSupportArray(this Type check)    => check.IsArraySupporting(typeof(ICharSequence));
    public static bool IsNotCharSequenceSupportArray(this Type check) => !check.IsCharSequenceSupportArray();

    public static bool IsCharSequenceSupportingList(this Type check)    => check.IsReadOnlyListSupporting(typeof(ICharSequence));
    public static bool IsNotCharSequenceSupportingList(this Type check) => !check.IsCharSequenceSupportingList();

    public static bool IsStringList(this Type check)    => check.IsReadOnlyListSupporting(typeof(string));
    public static bool IsNotStringList(this Type check) => !check.IsStringList();

    public static bool IsMutableString(this Type check)         => check == typeof(MutableString);
    public static bool IsMutableStringArray(this Type check)    => check == typeof(MutableString[]);
    public static bool IsNotMutableStringArray(this Type check) => !check.IsMutableStringArray();

    public static bool IsMutableStringList(this Type check)    => check.IsReadOnlyListSupporting(typeof(MutableString));
    public static bool IsNotMutableStringList(this Type check) => !check.IsMutableStringList();

    public static bool IsStringBuilder(this Type check)         => check == typeof(StringBuilder);
    public static bool IsNotStringBuilder(this Type check)      => !check.IsStringBuilder();
    public static bool IsStringBuilderArray(this Type check)    => check == typeof(StringBuilder[]);
    public static bool IsNotStringBuilderArray(this Type check) => !check.IsStringBuilderArray();

    public static bool IsStringBuilderList(this Type check)    => check.IsReadOnlyListSupporting(typeof(StringBuilder));
    public static bool IsNotStringBuilderList(this Type check) => !check.IsStringBuilderList();

    public static bool IsAnyTypeHoldingChars(this Type check) =>
        check.IsString() || check.IsStringArray() || check.IsStringList() || check.IsCharArray()
     || check.IsCharSequence() || check.IsCharSequenceSupportArray() || check.IsCharSequenceSupportingList()
     || check.IsStringBuilder() || check.IsStringBuilderArray() || check.IsStringBuilderList();


    public static bool IsStringToStringMap(this Type check) =>
        check == typeof(Dictionary<string, string>)
     || check == typeof(IDictionary<string, string>)
     || check == typeof(IImmutableDictionary<string, string>);

    public static IEnumerable<Type> GetAllTopLevelClassTypes(this Assembly assembly) =>
        assembly
            .GetTypes()
            .Where(t => t.IsClass && (!t.FullName?.Contains('<') ?? false)
                                  && (!t.FullName?.Contains('+') ?? false))
            .OrderBy(t => t.FullName);

    public static IEnumerable<Type> GetAllTopLevelStructTypes(this Assembly assembly) =>
        assembly
            .GetTypes()
            .Where(t => t.IsValueType && (!t.FullName?.Contains('<') ?? false)
                                      && (!t.FullName?.Contains('+') ?? false))
            .OrderBy(t => t.FullName);
}
