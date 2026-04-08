// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public static class KeyedCollectionAddFilteredEnumerateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> NoRevealersCallStructEnumtrInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> NoRevealersInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate>
        ValueRevealerNoNullableStructCallStructEnumtrInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> ValueRevealerNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNullableValueStructCallStructEnumtrInvokerCache
        = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNullableValueStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate>
        BothRevealersNoNullableStructCallStructEnumtrInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructCallStructEnumtrInvokerCache
        = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate>
        BothRevealersNullableValueStructCallStructEnumtrInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> BothNullableRevealersCallStructEnumtrInvokerCache = new();

    // ReSharper disable twice TypeParameterCanBeVariant
    private delegate KeyedCollectionMold NoRevealersInvoker<in TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?;

    private delegate KeyedCollectionMold NoRevealersInvoker<in TEnumbl, TKFilterBase, TVFilterBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?;

    private delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<in TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<in TEnumbl, TKFilterBase, TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<in TEnumbl, TKey, TValue, TKFilterBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<in TEnumbl, TValue, TKFilterBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct;

    private delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, out TKRevealBase
                                                                            , out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumbl, TKFilterBase, TVFilterBase, out TKRevealBase
                                                                            , out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableKeyStructInvoker<in TEnumbl, TKey, TValue, TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableKeyStructInvoker<in TEnumbl, TKey, TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<in TEnumbl, TKey, TValue, TKFilterBase, out TKRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<in TEnumbl, TValue, TKFilterBase, out TKRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealBase : notnull;

    // ReSharper disable twice TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothNullRevealersInvoker<in TEnumbl, TKey, TValue>(
        KeyedCollectionMold callOn
      , TEnumbl value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct;

    private static NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase> GetAddFilteredNoRevealersCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var invoker =
            (NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>)
            NoRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tkFilterType, tvFilterType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredNoRevealersCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase> BuildAddFilteredNoRevealersCallStructEnumtr
        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredNoRevealersInvokerMethodInfo<TKFilterBase, TVFilterBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
        else if (requiresCast) { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
        ilGenerator.Emit(OpCodes.Stloc_0);

        var getEnumtrMethInf = enumblLocalType.LocalType.GetEnumeratorMethodInfo() ??
                               throw new ArgumentException("Enumerable does not have a public instance GetEnumerator!");

        // enumblType value => value.GetEnumerator()
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Callvirt, getEnumtrMethInf);
        ilGenerator.Emit(OpCodes.Stloc_1);

        if (castEnumtrToNullable != null)
        {
            // enumeratorType valueEnumtr => Nullable<enumeratorType>
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            var createNullableConstructor = castEnumtrToNullable.LocalType.GetConstructor([enumeratorType])!;
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Call, createNullableConstructor);
        }

        // call AddFilteredIterateValueRevealer(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>));
        return (NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>)methodInvoker;
    }

    private static NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase> GetAddFilteredNoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>
        (Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>)
            NoRevealersInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tkFilterType, tvFilterType)
                   , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tkFilterType;
                         genericParamTypes[4] = key.tvFilterType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddFilteredEnumerate), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddFilteredNoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }
                   , callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase> BuildAddFilteredNoRevealersInvoker
        <TEnumbl, TKFilterBase, TVFilterBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddFilteredEnumerate(KeyedCollectionMold, TEnumbl, filterPredicate,  valueFmtStr, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>));
        var createInvoker = (NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>)methodInvoker;

        return createInvoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>
        GetAddFilteredValueRevealerNoNullableStructICallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>)
            ValueRevealerNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tkFilterType, tvFilterType, enumtrType)
                   , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType, Type enumeratorType) key, bool _) =>
                         key.enumblType.BuildAddFilteredValueRevealerNoNullableStructICallStructEnumtrInvoker
                             <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>(key.enumblParamType, key.enumeratorType)
                   , callAsFactory);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>
        BuildAddFilteredValueRevealerNoNullableStructICallStructEnumtrInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf
            = enumeratorType.GetAddFilteredValueRevealerNoNullableStructIInvokerMethodInfo<TKFilterBase, TVFilterBase, TVRevealerBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
        methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(string);
        methodParamTypes[6] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicValueRevealerNoNullableStructInvoke_{enumeratorType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast)
        {
            var callENumeratorType = typeof(Nullable<>).MakeGenericType(enumeratorType);
            castEnumtrToNullable = ilGenerator.DeclareLocal(callENumeratorType);
        }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
        else if (requiresCast) { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
        ilGenerator.Emit(OpCodes.Stloc_0);

        var getEnumtrMethInf = enumblLocalType.LocalType.GetEnumeratorMethodInfo() ??
                               throw new ArgumentException("Enumerable does not have a public instance GetEnumerator!");

        // enumblType value => value.GetEnumerator()
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Callvirt, getEnumtrMethInf);
        ilGenerator.Emit(OpCodes.Stloc_1);


        if (castEnumtrToNullable != null)
        {
            // enumeratorType valueEnumtr => Nullable<enumeratorType>
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            var createNullableConstructor = castEnumtrToNullable.LocalType.GetConstructor([enumeratorType])!;
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Call, createNullableConstructor);
        }

        // call AddFilteredIterateValueRevealer(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase,
                                              TVRevealerBase>));
        return (ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>)methodInvoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>
        GetAddFilteredValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>(Type enumtrType)
        where TEnumbl : IEnumerable?
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumtrType, tkFilterType, tvFilterType, tvRevealBase),
                     static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tkFilterType;
                         genericParamTypes[4] = key.tvFilterType;
                         genericParamTypes[5] = key.tvRevealType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddFilteredEnumerateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddFilteredValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, true);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>
        BuildAddFilteredValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateValueRevealerInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddFilteredEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>
        GetAddFilteredValueRevealerNullableValueStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKFilterBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var tkFilterType    = typeof(TKFilterBase);
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>)
            ValueRevealerNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tkFilterType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredValueRevealerNullableValueStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKFilterBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>
        BuildAddFilteredValueRevealerNullableValueStructCallStructEnumtr<TEnumbl, TKey, TValue, TKFilterBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredValueRevealerNullableValueStructMethodInfo<TValue, TKFilterBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TValue?>);
        methodParamTypes[3] = typeof(PalantírReveal<TValue>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(string);
        methodParamTypes[6] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast)
        {
            var callEnumeratorType = typeof(Nullable<>).MakeGenericType(enumeratorType);
            castEnumtrToNullable = ilGenerator.DeclareLocal(callEnumeratorType);
        }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
        else if (requiresCast) { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
        ilGenerator.Emit(OpCodes.Stloc_0);

        var getEnumtrMethInf = enumblLocalType.LocalType.GetEnumeratorMethodInfo() ??
                               throw new ArgumentException("Enumerable does not have a public instance GetEnumerator!");

        // enumblType value => value.GetEnumerator()
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Callvirt, getEnumtrMethInf);
        ilGenerator.Emit(OpCodes.Stloc_1);

        if (castEnumtrToNullable != null)
        {
            // enumeratorType valueEnumtr => Nullable<enumeratorType>
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            var createNullableConstructor = castEnumtrToNullable.LocalType.GetConstructor([enumeratorType])!;
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Call, createNullableConstructor);
        }

        // call AddFilteredIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>));
        return (ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>)methodInvoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>
        GetAddFilteredValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>(Type enumblType)
        where TEnumbl : IEnumerable?
        where TValue : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var tValueType      = typeof(TValue);
        var tkFilterType    = typeof(TKFilterBase);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>)
            ValueRevealerNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tkFilterType, tValueType)
                   , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tValue) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType = kvpTypes.Value.Key;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = key.tValue;
                         genericParamTypes[3] = key.tkFilterType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TValue?>);
                         methodParamTypes[3] = typeof(PalantírReveal<TValue>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddFilteredEnumerateNullValueRevealer)
                                               , genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumblParamType;

                         var fullGenericInvoke =
                             BuildAddFilteredValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase> BuildAddFilteredValueRevealerNullableValueStructInvoker
        <TEnumbl, TValue, TKFilterBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateNullValueRevealerInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddFilteredEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>));
        var createInvoker = (ValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>)methodInvoker;

        return createInvoker;
    }

    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
        GetAddFilteredValueRevealerNoNullableStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>)
            BothRevealersNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tkFilterType, tvFilterType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredEnumerateBothRevealersCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                                  (key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }


    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
        BuildAddFilteredEnumerateBothRevealersCallStructEnumtr<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredBothRevealersMethodInfo<TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TValue>);
        methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
        methodParamTypes[4] = typeof(PalantírReveal<TKRevealBase>);
        methodParamTypes[5] = typeof(string);
        methodParamTypes[6] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast)
        {
            var callEnumeratorType = typeof(Nullable<>).MakeGenericType(enumeratorType);
            castEnumtrToNullable = ilGenerator.DeclareLocal(callEnumeratorType);
        }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
        else if (requiresCast) { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
        ilGenerator.Emit(OpCodes.Stloc_0);

        var getEnumtrMethInf = enumblLocalType.LocalType.GetEnumeratorMethodInfo() ??
                               throw new ArgumentException("Enumerable does not have a public instance GetEnumerator!");

        // enumblType value => value.GetEnumerator()
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Callvirt, getEnumtrMethInf);
        ilGenerator.Emit(OpCodes.Stloc_1);

        if (castEnumtrToNullable != null)
        {
            // enumeratorType valueEnumtr => Nullable<enumeratorType>
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            var createNullableConstructor = castEnumtrToNullable.LocalType.GetConstructor([enumeratorType])!;
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Call, createNullableConstructor);
        }

        // call AddFilteredIterateBothRevealers(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate
                (typeof(BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>));
        return (BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        GetAddFilteredBothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        (Type enumblType)
        where TEnumbl : IEnumerable?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var tKRevealBase    = typeof(TKRevealerBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tkFilterType, tvFilterType, tKRevealBase, tvRevealBase)
                   , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType, Type tkRevealType, Type tvRevealType) key
                       , bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tkFilterType;
                         genericParamTypes[4] = key.tvFilterType;
                         genericParamTypes[5] = key.tkRevealType;
                         genericParamTypes[6] = key.tvRevealType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[4] = typeof(PalantírReveal<TKRevealerBase>);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddFilteredEnumerateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumblParamType;

                         var fullGenericInvoke =
                             BuildAddFilteredBothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        BuildAddFilteredBothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddFilteredEnumerateBothRevealers(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate
                (typeof(BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>));
        var createInvoker =
            (BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
        GetAddFilteredBothRevealersNullableKeyStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tvFilterType    = typeof(TVFilterBase);
        var tvRevealerType  = typeof(TVRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>)
            BothRevealersNullableKeyStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tvFilterType, tvRevealerType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type tvFilterType, Type tvRevealerType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredBothRevealersNullableKeyStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
        BuildAddFilteredBothRevealersNullableKeyStructCallStructEnumtr<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredBothRevealersNullableKeyStructMethodInfo<TKey, TVFilterBase, TVRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKey?, TValue>);
        methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
        methodParamTypes[4] = typeof(PalantírReveal<TKey>);
        methodParamTypes[5] = typeof(string);
        methodParamTypes[6] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);

        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast)
        {
            var callEnumeratorType = typeof(Nullable<>).MakeGenericType(enumeratorType);
            castEnumtrToNullable = ilGenerator.DeclareLocal(callEnumeratorType);
        }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
        else if (requiresCast) { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
        ilGenerator.Emit(OpCodes.Stloc_0);

        var getEnumtrMethInf = enumblLocalType.LocalType.GetEnumeratorMethodInfo() ??
                               throw new ArgumentException("Enumerable does not have a public instance GetEnumerator!");

        // enumblType value => value.GetEnumerator()
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Callvirt, getEnumtrMethInf);
        ilGenerator.Emit(OpCodes.Stloc_1);

        if (castEnumtrToNullable != null)
        {
            // enumeratorType valueEnumtr => Nullable<enumeratorType>
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            var createNullableConstructor = castEnumtrToNullable.LocalType.GetConstructor([enumeratorType])!;
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Call, createNullableConstructor);
        }

        // call AddFilteredIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>));
        return (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>
        GetAddFilteredBothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>(Type enumblType)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var enumblParamType  = typeof(TEnumbl);
        var tKeyType         = typeof(TKey);
        var tvFilterType     = typeof(TVFilterBase);
        var tvRevealBaseType = typeof(TVRevealerBase);
        var callAsFactory    = true;
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>)
            BothRevealersNullableKeyStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tKeyType, tvFilterType, tvRevealBaseType),
                     static ((Type enumblParamType, Type enumblType, Type tKey, Type tvFilterType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var valueType = kvpTypes.Value.Value;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = key.tKey;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tvFilterType;
                         genericParamTypes[4] = key.tvFilterType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKey?, TVFilterBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[4] = typeof(PalantírReveal<TKey>);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddFilteredEnumerateBothWithNullKeyRevealers)
                                               , genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumblParamType;

                         var fullGenericInvoke =
                             BuildAddFilteredBothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>
        BuildAddFilteredBothRevealersNullableKeyStructInvoker
        <TEnumbl, TKey, TVFilterBase, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);

        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddFilteredEnumerateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>));
        var createInvoker = (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
        GetAddFilteredBothRevealersNullableValueStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tkFilterType    = typeof(TKFilterBase);
        var tkRevealerType  = typeof(TKRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>)
            BothRevealersNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tkFilterType, tkRevealerType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type tvFilterType, Type tvRevealerType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredBothRevealersNullableValueStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
        BuildAddFilteredBothRevealersNullableValueStructCallStructEnumtr<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf
            = enumeratorType.GetAddFilteredBothWithNullableValueStructRevealersMethodInfo<TValue, TKFilterBase, TKRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TValue?>);
        methodParamTypes[3] = typeof(PalantírReveal<TValue>);
        methodParamTypes[4] = typeof(PalantírReveal<TKRevealBase>);
        methodParamTypes[5] = typeof(string);
        methodParamTypes[6] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast)
        {
            var callEnumeratorType = typeof(Nullable<>).MakeGenericType(enumeratorType);
            castEnumtrToNullable = ilGenerator.DeclareLocal(callEnumeratorType);
        }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
        else if (requiresCast) { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
        ilGenerator.Emit(OpCodes.Stloc_0);

        var getEnumtrMethInf = enumblLocalType.LocalType.GetEnumeratorMethodInfo() ??
                               throw new ArgumentException("Enumerable does not have a public instance GetEnumerator!");

        // enumblType value => value.GetEnumerator()
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Callvirt, getEnumtrMethInf);
        ilGenerator.Emit(OpCodes.Stloc_1);


        if (castEnumtrToNullable != null)
        {
            // enumeratorType valueEnumtr => Nullable<enumeratorType>
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            var createNullableConstructor = castEnumtrToNullable.LocalType.GetConstructor([enumeratorType])!;
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Call, createNullableConstructor);
        }

        // call AddFilteredIterateBothWithNullValueRevealers(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>));
        return (BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase>
        GetAddFilteredBothRevealersNullableValueStructInvoker
        <TEnumbl, TValue, TKFilterBase, TKRevealerBase>(Type enumblType)
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tValue          = typeof(TValue);
        var tkFilterBase    = typeof(TKFilterBase);
        var tkRevealBase    = typeof(TKRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tValue, tkFilterBase, tkRevealBase),
                     static ((Type enumblParamType, Type enumblType, Type tValue, Type tkFilterBase, Type tkRevealBase) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType = kvpTypes.Value.Key;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = key.tValue;
                         genericParamTypes[3] = key.tkFilterBase;
                         genericParamTypes[4] = key.tkRevealBase;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TValue?>);
                         methodParamTypes[3] = typeof(PalantírReveal<TValue>);
                         methodParamTypes[4] = typeof(PalantírReveal<TKRevealerBase>);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn = GetStaticMethodInfo(nameof(AddFilteredEnumerateBothWithNullValueRevealers)
                                                            , genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumblParamType;

                         var fullGenericInvoke =
                             BuildAddFilteredBothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase>
        BuildAddFilteredBothRevealersNullableValueStructInvoker
        <TEnumbl, TValue, TKFilterBase, TKRevealerBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        // Make space for enumblType local variables
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_1);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddFilteredEnumerateBothWithNullValueRevealers(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase>));

        var createInvoker = (BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase>)methodInvoker;

        return createInvoker;
    }

    private static BothNullRevealersInvoker<TEnumbl, TKey, TValue> GetAddFilteredBothNullableRevealersCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (BothNullRevealersInvoker<TEnumbl, TKey, TValue>)
            BothNullableRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredBothNullableRevealersStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothNullRevealersInvoker<TEnumbl, TKey, TValue> BuildAddFilteredBothNullableRevealersStructCallStructEnumtr<TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredBothNullRevealersMethodInfo<TKey, TValue>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKey?, TValue?>);
        methodParamTypes[3] = typeof(PalantírReveal<TValue>);
        methodParamTypes[4] = typeof(PalantírReveal<TKey>);
        methodParamTypes[5] = typeof(string);
        methodParamTypes[6] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast)
        {
            var callEnumeratorType = typeof(Nullable<>).MakeGenericType(enumeratorType);
            castEnumtrToNullable = ilGenerator.DeclareLocal(callEnumeratorType);
        }
        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
        else if (requiresCast) { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
        ilGenerator.Emit(OpCodes.Stloc_0);

        var getEnumtrMethInf = enumblLocalType.LocalType.GetEnumeratorMethodInfo()
                            ?? throw new ArgumentException("Enumerable does not have a public instance GetEnumerator!");

        // enumblType value => value.GetEnumerator()
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Callvirt, getEnumtrMethInf);
        ilGenerator.Emit(OpCodes.Stloc_1);

        if (castEnumtrToNullable != null)
        {
            // enumeratorType valueEnumtr => Nullable<enumeratorType>
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            var createNullableConstructor = castEnumtrToNullable.LocalType.GetConstructor([enumeratorType])!;
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Call, createNullableConstructor);
        }

        // call AddFilteredIterateBothNullRevealers(KeyedCollectionMold, TEnumtr, filterPredicate, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothNullRevealersInvoker<TEnumbl, TKey, TValue>));
        return (BothNullRevealersInvoker<TEnumbl, TKey, TValue>)methodInvoker;
    }

    private static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(KeyedCollectionAddFilteredEnumerateExtensions).GetMethods(NonPublic | Public | Static);

        MethodInfo? genTypeDefMeth = null;

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
            var enumeratorType = checkParameterInfos[1].ParameterType;
            if (enumeratorType.IsNullable()) continue;
            var isParameterMatch = true;
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

    public static KeyedCollectionMold AddFilteredEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        value == null ? callOn : callOn.AddFilteredEnumerate(value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredNoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>
                (value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        valueFormatString ??= "";
        keyFormatString   ??= "";
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        var count     = 0;
        var skipCount = 0;
        foreach (var kvp in value)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker
                    = actualType
                        .GetAddFilteredNoRevealersCallStructEnumtrInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            }
            count++;
            if (skipCount-- > 0) continue;
            var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
            if (filterResult is { IncludeItem: false })
            {
                if (filterResult is { KeepProcessing: true })
                {
                    skipCount = filterResult.SkipNextCount;
                    continue;
                }
                break;
            }
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredEnumerateValueRevealer<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateValueRevealer
                (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateValueRevealer<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();

        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker =
                actualType
                    .GetAddFilteredValueRevealerNoNullableStructICallStructEnumtrInvoker
                        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
        }
        keyFormatString ??= "";
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        var skipCount = 0;
        int count     = 0;
        callOn.ItemCount = 0;
        foreach (var kvp in value)
        {
            count++;
            if (skipCount-- > 0) continue;
            var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
            if (filterResult is { IncludeItem: false })
            {
                if (filterResult is { KeepProcessing: true })
                {
                    skipCount = filterResult.SkipNextCount;
                    continue;
                }
                break;
            }
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredEnumerateNullValueRevealer<TEnumbl, TValue, TKFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateNullValueRevealer
                (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateNullValueRevealer<TEnumbl, TValue, TKFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>
                (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?
        where TValue : struct
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker =
                actualType
                    .GetAddFilteredValueRevealerNullableValueStructCallStructEnumtrInvoker<TEnumbl, TKey, TValue, TKFilterBase>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
        }
        keyFormatString ??= "";
        var kvpType   = typeof(KeyValuePair<TKey, TValue?>);
        var skipCount = 0;
        var count     = 0;
        callOn.ItemCount = 0;
        foreach (var kvp in value)
        {
            count++;
            if (skipCount-- > 0) continue;
            var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
            if (filterResult is { IncludeItem: false })
            {
                if (filterResult is { KeepProcessing: true })
                {
                    skipCount = filterResult.SkipNextCount;
                    continue;
                }
                break;
            }
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredEnumerateBothRevealers<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler,
        PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateBothRevealers
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateBothRevealers<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler,
        PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();

        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredBothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold
        AddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
            this KeyedCollectionMold callOn
          , TEnumbl? value
          , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
          , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold
        AddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
            this KeyedCollectionMold callOn
          , TEnumbl? value
          , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
          , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;

        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker =
                actualType
                    .GetAddFilteredValueRevealerNoNullableStructCallStructEnumtrInvoker
                        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
        }
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        var skipCount = 0;
        var count     = 0;
        callOn.ItemCount = 0;
        foreach (var kvp in value)
        {
            count++;
            if (skipCount-- > 0) continue;
            var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
            if (filterResult is { IncludeItem: false })
            {
                if (filterResult is { KeepProcessing: true })
                {
                    skipCount = filterResult.SkipNextCount;
                    continue;
                }
                break;
            }
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKey : struct
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateBothWithNullKeyRevealers
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler,
        PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredBothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker =
                actualType
                    .GetAddFilteredBothRevealersNullableKeyStructCallStructEnumtrInvoker
                        <TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
        }
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        var skipCount = 0;
        var count     = 0;
        callOn.ItemCount = 0;
        foreach (var kvp in value)
        {
            count++;
            if (skipCount-- > 0) continue;
            var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
            if (filterResult is { IncludeItem: false })
            {
                if (filterResult is { KeepProcessing: true })
                {
                    skipCount = filterResult.SkipNextCount;
                    continue;
                }
                break;
            }
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKFilterBase, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler,
        PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TValue : struct
        where TKRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateBothWithNullValueRevealers
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKFilterBase, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler,
        PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();

        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredBothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker =
                actualType
                    .GetAddFilteredBothRevealersNullableValueStructCallStructEnumtrInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
                        (enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
        }
        var kvpType   = typeof(KeyValuePair<TKey, TValue?>);
        var skipCount = 0;
        var count     = 0;
        callOn.ItemCount = 0;
        foreach (var kvp in value)
        {
            count++;
            if (skipCount-- > 0) continue;
            var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
            if (filterResult is { IncludeItem: false })
            {
                if (filterResult is { KeepProcessing: true })
                {
                    skipCount = filterResult.SkipNextCount;
                    continue;
                }
                break;
            }
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddFilteredEnumerateBothNullRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker =
                actualType
                    .GetAddFilteredBothNullableRevealersCallStructEnumtrInvoker<TEnumbl, TKey, TValue>
                        (enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
        }
        var kvpType   = typeof(KeyValuePair<TKey?, TValue?>);
        var skipCount = 0;
        int count     = 0;
        callOn.ItemCount = 0;
        foreach (var kvp in value)
        {
            count++;
            if (skipCount-- > 0) continue;
            var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
            if (filterResult is { IncludeItem: false })
            {
                if (filterResult is { KeepProcessing: true })
                {
                    skipCount = filterResult.SkipNextCount;
                    continue;
                }
                break;
            }
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
        }
        return callOn;
    }
}
