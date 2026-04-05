// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public static class KeyedCollectionAddAllIterateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;


    private static readonly ConcurrentDictionary<Type, MethodInfo>       NoRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> NoRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo>     ValueRevealerNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo>     ValueRevealerNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>     BothRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>     BothRevealersNullableKeyStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>     BothRevealersNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> BothRevealersBothNullableStructsMethodInfoCache = new();

    internal delegate KeyedCollectionMold NoRevealersNoNullableStructInvoker<in TEnumtr>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator;

    internal delegate KeyedCollectionMold NoRevealersNoNullableStructInvoker<in TEnumtr, TKey, TValue>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?;

    internal delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<in TEnumtr, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<in TEnumtr, TKey, TValue, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<in TEnumtr, TValue>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<in TEnumtr, TKey, TValue>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct;


    internal delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumtr, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumtr, TKey, TValue, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableKeyStructInvoker<in TEnumtr, TKey, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableKeyStructInvoker<in TEnumtr, TKey, TValue, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<in TEnumtr, TValue, out TKRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<in TEnumtr, TKey, TValue, out TKRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
        where TKRevealBase : notnull;

    internal static MethodInfo GetAddAllNoRevealersInvokerMethodInfo(this Type enumtrType)
    {
        var methInf =
            NoRevealersNoNullableStructMethodInfoCache.GetOrAdd
                (enumtrType, static enumeratorType =>
                {
                    var kvpTypes = enumeratorType.GetKeyedCollectionTypes();
                    if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");

                    var keyType   = kvpTypes.Value.Key;
                    var valueType = kvpTypes.Value.Value;

                    using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                    genericParamTypes[0] = enumeratorType;
                    genericParamTypes[1] = keyType;
                    genericParamTypes[2] = valueType;

                    using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                    methodParamTypes[0] = typeof(KeyedCollectionMold);
                    methodParamTypes[1] = enumeratorType;
                    methodParamTypes[2] = typeof(string);
                    methodParamTypes[3] = typeof(string);
                    methodParamTypes[4] = typeof(FormatFlags);

                    return GetStaticMethodInfo(nameof(AddAllIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
                });
        return methInf;
    }

    internal static NoRevealersNoNullableStructInvoker<TEnumtr> GetAddAllNoRevealersInvoker<TEnumtr>(Type enumtrType)
        where TEnumtr : IEnumerator?
    {
        var enumtrParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersNoNullableStructInvoker<TEnumtr>)
            NoRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType)
                   , static ((Type enumtrParamType, Type enumtrType) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(string);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumtrParamType;
                         var fullGenericInvoke =
                             BuildAddAllNoRevealersNoNullableStructInvoker<TEnumtr>
                                 (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static NoRevealersNoNullableStructInvoker<TEnumtr> BuildAddAllNoRevealersNoNullableStructInvoker<TEnumtr>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddAllIterate(KeyedCollectionMold, TEnumtr, valueFmtStr, keyFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersNoNullableStructInvoker<TEnumtr>));
        var createInvoker = (NoRevealersNoNullableStructInvoker<TEnumtr>)methodInvoker;

        return createInvoker;
    }

    internal static MethodInfo GetAddAllValueRevealerNoNullableStructIInvokerMethodInfo<TVRevealerBase>(this Type enumtrType)
        where TVRevealerBase : notnull
    {
        var tvRevealerBaseType = typeof(TVRevealerBase);
        var methInf =
            ValueRevealerNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tvRevealerBaseType)
               , static ((Type enumeratorType, Type tvRevealerType) key, bool _) =>
                 {
                     var kvpTypes = key.enumeratorType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = keyType;
                     genericParamTypes[2] = valueType;
                     genericParamTypes[3] = key.tvRevealerType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     return GetStaticMethodInfo(nameof(AddAllIterateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 }, true);
        return methInf;
    }

    internal static ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase> GetAddAllValueRevealerNoNullableStructInvoker<
            TEnumtr, TVRevealerBase>
        (Type enumtrType)
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        var enumtrParamType = typeof(TEnumtr);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType, tvRevealBase)
                   , static ((Type enumtrParamType, Type enumtrType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tvRevealType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumtrParamType;
                         var fullGenericInvoke =
                             BuildAddAllValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase>
                                 (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase> BuildAddAllValueRevealerNoNullableStructInvoker
        <TEnumtr, TVRevealerBase>(MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    internal static MethodInfo GetAddAllValueRevealerNullableValueStructMethodInfo<TValue>(this Type enumtrType)
        where TValue : struct
    {
        var tValue = typeof(TValue);
        var methInf =
            ValueRevealerNullableValueStructMethodInfoCache.GetOrAdd
                ((enumtrType, tValue),
                 static ((Type enumtrType, Type tvalueType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType = kvpTypes.Value.Key;

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                     genericParamTypes[0] = key.enumtrType;
                     genericParamTypes[1] = keyType;
                     genericParamTypes[2] = key.tvalueType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TValue>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     return GetStaticMethodInfo(nameof(AddAllIterateNullValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static ValueRevealerNullableValueStructInvoker<TEnumtr, TValue> GetAddAllValueRevealerNullableValueStructInvoker<TEnumtr, TValue>(
        Type enumtrType)
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        var enumtrParamType = typeof(TEnumtr);
        var tvRevealBase    = typeof(TValue);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumtr, TValue>)
            ValueRevealerNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType, tvRevealBase)
                   , static ((Type enumtrParamType, Type enumtrType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType = kvpTypes.Value.Key;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = key.tvRevealType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(PalantírReveal<TValue>);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterateNullValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumtrParamType;
                         var fullGenericInvoke =
                             BuildAddAllValueRevealerNullableValueStructInvoker<TEnumtr, TValue>
                                 (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerNullableValueStructInvoker<TEnumtr, TValue> BuildAddAllValueRevealerNullableValueStructInvoker
        <TEnumtr, TValue>(MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateNullValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddAllIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TEnumtr, TValue>));
        var createInvoker = (ValueRevealerNullableValueStructInvoker<TEnumtr, TValue>)methodInvoker;

        return createInvoker;
    }

    internal static MethodInfo GetAddAllBothRevealersNoNullableStructMethodInfo<TKRevealerBase, TVRevealerBase>(this Type enumtrType)
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var tKRevealBase = typeof(TKRevealerBase);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tKRevealBase, tvRevealBase),
                 static ((Type enumtrType, Type tkRevealType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     genericParamTypes[0] = key.enumtrType;
                     genericParamTypes[1] = keyType;
                     genericParamTypes[2] = valueType;
                     genericParamTypes[3] = key.tkRevealType;
                     genericParamTypes[4] = key.tvRevealType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     return GetStaticMethodInfo(nameof(AddAllIterateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static BothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase> GetAddAllBothRevealersNoNullableStructInvoker
        <TEnumtr, TKRevealerBase, TVRevealerBase>(Type enumtrType)
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var enumtrParamType = typeof(TEnumtr);
        var tKRevealBase    = typeof(TKRevealerBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache.GetOrAdd
                ((enumtrParamType, enumtrType, tKRevealBase, tvRevealBase),
                 static ((Type enumtrParamType, Type enumtrType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     genericParamTypes[0] = key.enumtrType;
                     genericParamTypes[1] = keyType;
                     genericParamTypes[2] = valueType;
                     genericParamTypes[3] = key.tkRevealType;
                     genericParamTypes[4] = key.tvRevealType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     var toInvokeOn =
                         GetStaticMethodInfo(nameof(AddAllIterateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);

                     methodParamTypes[1] = key.enumtrParamType;
                     var fullGenericInvoke =
                         BuildAddAllBothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>
                             (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                     return fullGenericInvoke;
                 }, callAsFactory);
        return invoker;
    }


    internal static BothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>
        BuildAddAllBothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] parameterArgTypes)
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var signatureName = $"{parameterArgTypes[2].Name}_{parameterArgTypes[3].Name}";
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{parameterArgTypes[1].Name}_{signatureName}"
               , typeof(KeyedCollectionMold), parameterArgTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddAllIterateBothRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate(typeof(BothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>));
        var createdInvoker = (BothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>)methodInvoker;

        return createdInvoker;
    }

    internal static MethodInfo GetAddAllBothRevealersNullableKeyStructMethodInfo<TKey, TVRevealerBase>(this Type enumtrType)
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var tKey         = typeof(TKey);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNullableKeyStructMethodInfoCache.GetOrAdd
                ((enumtrType, tKey, tvRevealBase)
               , static ((Type enumtrType, Type tkeyType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var valueType = kvpTypes.Value.Value;

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                     genericParamTypes[0] = key.enumtrType;
                     genericParamTypes[1] = key.tkeyType;
                     genericParamTypes[2] = valueType;
                     genericParamTypes[3] = key.tvRevealType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TKey>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     return
                         GetStaticMethodInfo(nameof(AddAllIterateBothWithNullKeyRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase> GetAddAllBothRevealersNullableKeyStructInvoker
        <TEnumtr, TKey, TVRevealerBase>(Type enumtrType)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var enumtrParamType  = typeof(TEnumtr);
        var tKeyType         = typeof(TKey);
        var tvRevealBaseType = typeof(TVRevealerBase);
        var callAsFactory    = true;
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase>)
            BothRevealersNullableKeyStructInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType, tKeyType, tvRevealBaseType)
                   , static ((Type enumtrParamType, Type enumtrType, Type tKey, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var valueType = kvpTypes.Value.Value;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = key.tKey;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tvRevealType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TKey>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterateBothWithNullKeyRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumtrParamType;
                         var fullGenericInvoke =
                             BuildAddAllBothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase>
                                 (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase> BuildAddAllBothRevealersNullableKeyStructInvoker
        <TEnumtr, TKey, TVRevealerBase>(MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker  = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase>));
        var createdInvoker = (BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase>)methodInvoker;

        return createdInvoker;
    }

    internal static MethodInfo GetAddAllBothRevealersNullableValueStructMethodInfo<TValue, TKRevealerBase>(this Type enumtrType)
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var tValue       = typeof(TValue);
        var tkRevealBase = typeof(TKRevealerBase);
        var methInf =
            BothRevealersNullableValueStructMethodInfoCache.GetOrAdd
                ((enumtrType, tValue, tkRevealBase),
                 static ((Type enumtrType, Type tvalueType, Type tkRevealBase) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType = kvpTypes.Value.Key;

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                     genericParamTypes[0] = key.enumtrType;
                     genericParamTypes[1] = keyType;
                     genericParamTypes[2] = key.tvalueType;
                     genericParamTypes[3] = key.tkRevealBase;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TValue>);
                     methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     return
                         GetStaticMethodInfo(nameof(AddAllIterateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase> GetAddAllBothRevealersNullableValueStructInvoker
        <TEnumtr, TValue, TKRevealerBase>(Type enumtrType)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var enumtrParamType = typeof(TEnumtr);
        var tValueType      = typeof(TValue);
        var tkRevealBase    = typeof(TKRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType, tValueType, tkRevealBase),
                     static ((Type enumtrParamType, Type enumtrType, Type valueType, Type tkRevealBase) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType = kvpTypes.Value.Key;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = key.valueType;
                         genericParamTypes[3] = key.tkRevealBase;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(PalantírReveal<TValue>);
                         methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterateBothWithNullValueRevealers), genericParamTypes.AsArray
                                               , methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumtrParamType;
                         var fullGenericInvoke =
                             BuildAddAllBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>
                                 (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase> BuildAddAllBothRevealersNullableValueStructInvoker
        <TEnumtr, TValue, TKRevealerBase>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker  = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>));
        var createdInvoker = (BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>)methodInvoker;

        return createdInvoker;
    }

    internal static MethodInfo GetAddAllBothNullRevealersMethodInfo<TKey, TValue>(this Type enumtrType)
        where TKey : struct
        where TValue : struct
    {
        var tKey   = typeof(TKey);
        var tValue = typeof(TValue);
        var methInf =
            BothRevealersBothNullableStructsMethodInfoCache.GetOrAdd
                ((enumtrType, tKey, tValue),
                 static ((Type enumtrType, Type keyType, Type valueType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                     genericParamTypes[0] = key.enumtrType;
                     genericParamTypes[1] = key.keyType;
                     genericParamTypes[2] = key.valueType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TValue>);
                     methodParamTypes[3] = typeof(PalantírReveal<TKey>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     return
                         GetStaticMethodInfo(nameof(AddAllIterateBothNullRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(KeyedCollectionAddAllIterateExtensions).GetMethods(NonPublic | Public | Static);

        MethodInfo? genTypeDefMeth = null;
        var         findEnumtrType = findParamTypes[1];

        foreach (var checkMethodInfo in myMethodInfosCached)
        {
            if (!checkMethodInfo.Name.Contains(findMethodName)) continue;
            var checkParameterInfos = checkMethodInfo.GetParameters();
            if (checkParameterInfos.Length != findParamTypes.Length) continue;
            if (findGenericParams.Length > 0)
            {
                if (!checkMethodInfo.IsGenericMethod) continue;
                var checkGenParams = checkMethodInfo.GetGenericArguments();
                if (checkGenParams.Length != findGenericParams.Length) continue;
            }
            if (checkParameterInfos.Length != findParamTypes.Length) continue;
            var isParameterMatch = true;
            var checkEnumtrType  = checkParameterInfos[1].ParameterType;
            if ((!findEnumtrType.IsNullable() && checkEnumtrType.IsNullable())
             || (findEnumtrType.IsNullable() && !checkEnumtrType.IsNullable()))
                continue;
            for (var i = 2; i < findParamTypes.Length; i++)
            {
                var findParamInfo  = findParamTypes[i];
                var checkParamType = checkParameterInfos[i].ParameterType;
                if (!findParamInfo.IsAssignableTo(checkParamType))
                {
                    if (!findParamInfo.IsGenericType || !checkParamType.IsGenericType)
                    {
                        isParameterMatch = false;
                        break;
                    }
                    if (findParamInfo.GetGenericTypeDefinition() != checkParamType.GetGenericTypeDefinition())
                    {
                        isParameterMatch = false;
                        break;
                    }
                }
            }
            if (isParameterMatch)
            {
                genTypeDefMeth = checkMethodInfo;
                break;
            }
        }
        if (genTypeDefMeth == null)
            throw new ArgumentException($"Could not find method \"{findMethodName} with generic arguments " +
                                        $"[{findGenericParams.Select(t => t.ShortNameInCSharpFormat()).JoinToString()}]" +
                                        $" and parameters of type [{findParamTypes.Select(t => t.ShortNameInCSharpFormat()).JoinToString()}]");

        var generified = genTypeDefMeth.MakeGenericMethod(findGenericParams);

        return generified;
    }

    public static KeyedCollectionMold AddAllIterate<TEnumtr>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? callOn : callOn.AddAllIterate(value.Value, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterate<TEnumtr>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllNoRevealersInvoker<TEnumtr?>(actualType);
        return invoker(callOn, value, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterate<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>> =>
        value == null ? callOn : callOn.AddAllIterate<TEnumtr, TKey, TValue>(value.Value, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterate<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealBase : notnull =>
        value == null ? callOn : callOn.AddAllIterateValueRevealer(value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
                (value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct =>
        value == null ? callOn : callOn.AddAllIterateNullValueRevealer(value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllValueRevealerNullableValueStructInvoker<TEnumtr, TValue>(actualType);
        return invoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
                (value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothRevealers(value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllBothRevealersNoNullableStructInvoker<TEnumtr, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKey : struct
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothWithNullKeyRevealers
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllBothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey?, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothWithNullValueRevealers
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TValue : struct
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothNullRevealers
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue?>>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey?, TValue?>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value!.Current;
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }
}
