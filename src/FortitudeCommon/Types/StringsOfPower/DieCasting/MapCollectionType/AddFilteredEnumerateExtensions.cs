// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public static class KeyedCollectionAddFilteredEnumerateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly ConcurrentDictionary<Type, Type?>       EnumerableToEnumeratorTypeCache       = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo?> EnumerableToEnumeratorMethodInfoCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> NoRevealersCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate>      NoRevealersInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNoNullableStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNoNullableStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNullableValueStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNullableValueStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>       BothRevealersNoNullableStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>       BothRevealersNullableKeyStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>       BothRevealersNullableValueStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type), Delegate> BothNullableRevealersCallStructEnumtrInvokerCache = new();

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

    private delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, out TKRevealBase, out TVRevealBase>(
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

    private delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumbl, TKFilterBase, TVFilterBase, out TKRevealBase, out TVRevealBase>(
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
    private delegate KeyedCollectionMold BothNullRevealersInvoker <in TEnumbl, TKey, TValue>(
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
        var callAsFactory = true;
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var invoker =
            (NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>)
            NoRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblType, tkFilterType, tvFilterType, enumtrType)
                        , static ((Type enumblType, Type tkFilterType, Type tvFilterType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredNoRevealersCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase> BuildAddFilteredNoRevealersCallStructEnumtr
        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(this Type enumblType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredNoRevealersInvokerMethodInfo<TKFilterBase, TVFilterBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast)
        {
            castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType));
        }
        
        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
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
        (Type? enumblType = null)
        where TEnumbl : IEnumerable?
    {
        enumblType ??= typeof(TEnumbl);
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var callAsFactory = true;
        var invoker =
            (NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>)
            NoRevealersInvokerCache
                .GetOrAdd
                    ((enumblType, tkFilterType, tvFilterType)
                     , static ((Type enumerableType, Type tkFilterType, Type tvFilterType) key, bool _) =>
                    {   
                        var kvpTypes = key.enumerableType.GetKeyedCollectionTypes();
                        if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                        var keyType   = kvpTypes.Value.Key;
                        var valueType = kvpTypes.Value.Value;
                        var toInvokeOn = 
                            GetStaticMethodInfo(nameof(AddFilteredEnumerate)
                                                , [key.enumerableType, keyType, valueType, key.tkFilterType, key.tvFilterType],
                        [ typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                          , typeof(string), typeof(string), typeof(FormatFlags) ]);
                        
                        var genGenMethod
                            = myMethodInfosCached!
                                .First(mi => mi.Name.Contains(nameof(BuildAddFilteredNoRevealersInvoker)));
                        var concreteGenMethod
                            = genGenMethod.MakeGenericMethod([typeof(TEnumbl), keyType, valueType, key.tkFilterType, key.tvFilterType]);
                        return (NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>)
                            concreteGenMethod.Invoke(null, [toInvokeOn, key.enumerableType])!;
                    }
                   , callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase> BuildAddFilteredNoRevealersInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(MethodInfo methodInfo, Type enumblType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                  , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        
        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);
        
        // call AddFilteredEnumerate(KeyedCollectionMold, TEnumbl, filterPredicate,  valueFmtStr, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>));
        var createInvoker = (NoRevealersInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, KeyValuePredicate<TKFilterBase, TVFilterBase> filetPredicate
          , string? valueFmtStr, string? keyFmtStr, FormatFlags flags) =>
            createInvoker(kcm, enumbl, filetPredicate, valueFmtStr, keyFmtStr, flags);

        return Wrapped;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>
        GetAddFilteredValueRevealerNoNullableStructICallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var callAsFactory = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>)
            ValueRevealerNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblType, tkFilterType, tvFilterType, enumtrType)
                        , static ((Type enumblType, Type tkFilterType, Type tvFilterType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredValueRevealerNoNullableStructICallStructEnumtrInvoker
                                  <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>(key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>
        BuildAddFilteredValueRevealerNoNullableStructICallStructEnumtrInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>
        (this Type enumblType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredValueRevealerNoNullableStructIInvokerMethodInfo<TKFilterBase, TVFilterBase, TVRevealerBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicValueRevealerNoNullableStructInvoke_{enumeratorType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                  , typeof(PalantírReveal<TVRevealerBase>), typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
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
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>));
        return (ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>)methodInvoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase> 
        GetAddFilteredValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>(
        Type? enumtrType = null)
        where TEnumbl : IEnumerable?
        where TVRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumbl);
        var tkFilterType = typeof(TKFilterBase);
        var tvFilterType = typeof(TVFilterBase);
        var tvRevealBase = typeof(TVRevealerBase);
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumtrType, tkFilterType, tvFilterType, tvRevealBase),
                     static ((Type enumblType, Type tkFilterType, Type tvFilterType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null)
                             throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;
                         var toInvokeOn =
                             GetStaticMethodInfo
                                 (nameof(AddFilteredEnumerateValueRevealer)
                                 , [key.enumblType, keyType, valueType, key.tkFilterType, key.tvFilterType, key.tvRevealType],
                                 [
                                     typeof(KeyedCollectionMold), typeof(TEnumbl)
                                     , typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                                     , typeof(PalantírReveal<TVRevealerBase>)
                                     , typeof(string), typeof(string), typeof(FormatFlags)
                                 ]);

                         var genGenMethod
                             = myMethodInfosCached!
                                 .First(mi => mi.Name.Contains(nameof(BuildAddFilteredValueRevealerNoNullableStructInvoker)));
                         var concreteGenMethod
                             = genGenMethod.MakeGenericMethod([typeof(TEnumbl), keyType, valueType, key.tkFilterType, key.tvFilterType, key.tvRevealType ]);
                         return (ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>)
                             concreteGenMethod.Invoke(null, [toInvokeOn, key.enumblType, typeof(PalantírReveal<TVRevealerBase>)])!;
                     }, true);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase> 
        BuildAddFilteredValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>
        (MethodInfo methodInfo, Type? enumblType = null, Type? tvRevealBase = null)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        enumblType   ??= typeof(TEnumbl);
        tvRevealBase ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateValueRevealerInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>), tvRevealBase
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
        // Make space for enumblType local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        
        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);

        // call AddFilteredEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate,
            PalantírReveal<TVRevealerBase> valueRevealer, string? keyFmtStr, string? valueFmtStr, FormatFlags flags) => 
            createInvoker(kcm, enumbl, filterPredicate, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>
        GetAddFilteredValueRevealerNullableValueStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKFilterBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var callAsFactory = true;
        var tkFilterType  = typeof(TKFilterBase);
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>)
            ValueRevealerNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblType, tkFilterType, enumtrType)
                        , static ((Type enumblType, Type tkFilterType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredValueRevealerNullableValueStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKFilterBase>(key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>
        BuildAddFilteredValueRevealerNullableValueStructCallStructEnumtr<TEnumbl, TKey, TValue, TKFilterBase>
        (this Type enumblType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredValueRevealerNullableValueStructMethodInfo<TValue, TKFilterBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TValue?>)
                   , typeof(PalantírReveal<TValue>), typeof(string), typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
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
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
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
        GetAddFilteredValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>(Type? enumblType = null)
        where TEnumbl : IEnumerable?
        where TValue : struct
    {
        enumblType ??= typeof(TEnumbl);
        var tValueType    = typeof(TValue);
        var tkFilterType  = typeof(TKFilterBase);
        var callAsFactory = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>)
            ValueRevealerNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumblType, tkFilterType, tValueType)
                    , static ((Type enumblType, Type tkFilterType, Type tValue) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType = kvpTypes.Value.Key;
                         var toInvokeOn =
                             GetStaticMethodInfo
                                 (nameof(AddFilteredEnumerateNullValueRevealer)
                                , [typeof(TEnumbl), keyType, key.tValue, key.tkFilterType],
                                  [
                                      typeof(KeyedCollectionMold), key.enumblType,  typeof(KeyValuePredicate<TKFilterBase, TValue?>)
                                    , typeof(PalantírReveal<TValue>), typeof(string), typeof(string), typeof(FormatFlags)
                                  ]);

                         var genGenMethod
                             = myMethodInfosCached!
                                 .First(mi => mi.Name.Contains(nameof(BuildAddFilteredValueRevealerNullableValueStructInvoker)));
                         var concreteGenMethod
                             = genGenMethod.MakeGenericMethod([typeof(TEnumbl), keyType, key.tValue, key.tkFilterType]);
                         return (ValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase>)
                             concreteGenMethod.Invoke(null, [toInvokeOn, key.enumblType, typeof(PalantírReveal<TValue>)])!;
                     }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase> BuildAddFilteredValueRevealerNullableValueStructInvoker
        <TEnumbl, TKey, TValue, TKFilterBase>(MethodInfo methodInfo, Type? enumblType = null, Type? valueRevealerType = null)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        enumblType        ??= typeof(TEnumbl);
        valueRevealerType ??= typeof(PalantírReveal<TValue>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateNullValueRevealerInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TValue?>), valueRevealerType
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        
        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);
        
        // call AddFilteredEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>));
        var createInvoker = (ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
          , PalantírReveal<TValue> valueRevealer, string? keyFmtStr
          , string? valueFmtStr, FormatFlags flags) => 
            createInvoker(kcm, enumbl, filterPredicate, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
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
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var callAsFactory = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>)
            BothRevealersNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblType, tkFilterType, tvFilterType, enumtrType)
                        , static ((Type enumblType, Type tkFilterType, Type tvFilterType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredEnumerateBothRevealersCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(key.enumeratorType), callAsFactory);
        return invoker;
    }


    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
        BuildAddFilteredEnumerateBothRevealersCallStructEnumtr<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
        (this Type enumblType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredBothRevealersMethodInfo<TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                  , typeof(PalantírReveal<TVRevealBase>), typeof(PalantírReveal<TKRevealBase>), typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
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
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
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
            = helperMethod
                .CreateDelegate
                    ( typeof(BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>));
        return (BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase> 
        GetAddFilteredBothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        (Type? enumblType = null)
        where TEnumbl : IEnumerable?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        enumblType ??= typeof(TEnumbl);
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var tKRevealBase  = typeof(TKRevealerBase);
        var tvRevealBase  = typeof(TVRevealerBase);
        var callAsFactory = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumblType, tkFilterType , tvFilterType, tKRevealBase, tvRevealBase),
                      static ((Type enumblType, Type tkFilterType, Type tvFilterType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                      {
                          var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                          if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                          var keyType   = kvpTypes.Value.Key;
                          var valueType = kvpTypes.Value.Value;
                          var toInvokeOn =
                              GetStaticMethodInfo
                                  (nameof(AddFilteredEnumerateBothRevealers)
                                 , [key.enumblType, keyType, valueType, key.tkFilterType, key.tvFilterType, key.tkRevealType, key.tvRevealType]
                                 , [
                                       typeof(KeyedCollectionMold), key.enumblType
                                     , typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                                     , typeof(PalantírReveal<TVRevealerBase>)
                                     , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                     , typeof(FormatFlags)
                                   ]);

                          var genGenMethod =
                              myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddFilteredBothRevealersNoNullableStructInvoker)));
                          var concreteGenMethod =
                              genGenMethod.MakeGenericMethod([typeof(TEnumbl), keyType, valueType, key.tkFilterType, key.tvFilterType, key.tkRevealType, key.tvRevealType]);
                          return (BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)
                              concreteGenMethod.Invoke(null,
                              [
                                  toInvokeOn, key.enumblType , typeof(PalantírReveal<TKRevealerBase>), typeof(PalantírReveal<TVRevealerBase>)
                              ])!;
                      }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase> 
        BuildAddFilteredBothRevealersNoNullableStructInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        (MethodInfo methodInfo, Type? enumblType = null, Type? tkRevealer = null, Type? tvRevealer = null)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealerBase?
        where TValue : TVRevealerBase?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        enumblType ??= typeof(TEnumbl);
        tkRevealer ??= typeof(PalantírReveal<TKRevealerBase>);
        tvRevealer ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                   , tvRevealer, tkRevealer, typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
        // Make space for enumblType local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        
        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);

        // call AddFilteredEnumerateBothRevealers(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate
                (typeof(BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>));
        var createInvoker = (BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)methodInvoker;


        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
          , PalantírReveal<TVRevealerBase> vRevealer, PalantírReveal<TKRevealerBase> keyRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, enumbl, filterPredicate, vRevealer, keyRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
        GetAddFilteredBothRevealersNullableKeyStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var tvFilterType  = typeof(TVFilterBase);
        var tvRevealerType  = typeof(TVRevealBase);
        var callAsFactory = true;
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>)
            BothRevealersNullableKeyStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblType, tvFilterType, tvRevealerType,  enumtrType)
                        , static ((Type enumblType, Type tvFilterType, Type tvRevealerType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredBothRevealersNullableKeyStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
        BuildAddFilteredBothRevealersNullableKeyStructCallStructEnumtr<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
        (this Type enumblType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredBothRevealersNullableKeyStructMethodInfo<TKey, TVFilterBase, TVRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKey?, TVFilterBase>)
                  , typeof(PalantírReveal<TVRevealBase>), typeof(PalantírReveal<TKey>)
                  , typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
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
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>));
        return (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase> GetAddFilteredBothRevealersNullableKeyStructInvoker
        <TEnumbl, TKey, TVFilterBase, TVRevealerBase>(Type? enumblType = null)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        enumblType ??= typeof(TEnumbl);
        var tKeyType         = typeof(TKey);
        var tvFilterType     = typeof(TVFilterBase);
        var tvRevealBaseType = typeof(TVRevealerBase);
        var callAsFactory    = true;
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>)
            BothRevealersNullableKeyStructInvokerCache
                .GetOrAdd
                    ((enumblType, tKeyType, tvFilterType, tvRevealBaseType),
                     static ((Type enumblType, Type tKey, Type tvFilterType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null)
                             throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var valueType = kvpTypes.Value.Value;
                         var toInvokeOn =
                             GetStaticMethodInfo
                                 (nameof(AddFilteredEnumerateBothWithNullKeyRevealers)
                                , [key.enumblType, key.tKey, valueType, key.tvFilterType, key.tvRevealType],
                                  [
                                      typeof(KeyedCollectionMold), typeof(IEnumerator), typeof(KeyValuePredicate<TKey?, TVFilterBase>)
                                    , typeof(PalantírReveal<TVRevealerBase>), typeof(PalantírReveal<TKey>)
                                    , typeof(string), typeof(FormatFlags)
                                  ]);

                         var genGenMethod
                             = myMethodInfosCached!
                                 .First(mi => mi.Name.Contains(nameof(BuildAddFilteredBothRevealersNullableKeyStructInvoker)));
                         var concreteGenMethod
                             = genGenMethod.MakeGenericMethod([
                                 typeof(TEnumbl), key.tKey, valueType, key.tvFilterType, key.tvRevealType
                             ]);
                         return (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase>)
                             concreteGenMethod.Invoke(null, [
                                 toInvokeOn, key.enumblType, typeof(PalantírReveal<TKey>) , typeof(PalantírReveal<TVRevealerBase>)
                             ])!;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVFilterBase, TVRevealerBase> BuildAddFilteredBothRevealersNullableKeyStructInvoker
        <TEnumbl, TKey, TValue, TVFilterBase, TVRevealerBase>(MethodInfo methodInfo, Type enumblType, Type? tKeyRevealerType = null, Type? tvRevealBaseType = null)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        tKeyRevealerType ??= typeof(PalantírReveal<TKey>);
        tvRevealBaseType ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKey?, TVFilterBase>)
                   , tvRevealBaseType, tKeyRevealerType, typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
        // Make space for enumblType local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        
        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);

        // call AddFilteredEnumerateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealerBase>));
        var createInvoker = (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVFilterBase, TVRevealerBase>)methodInvoker;


        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumtr, KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
          , PalantírReveal<TVRevealerBase> vRevealer, PalantírReveal<TKey> kRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, enumtr, filterPredicate, vRevealer, kRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
        GetAddFilteredBothRevealersNullableValueStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var tkFilterType  = typeof(TKFilterBase);
        var tkRevealerType  = typeof(TKRevealBase);
        var callAsFactory = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>)
            BothRevealersNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblType, tkFilterType, tkRevealerType, enumtrType)
                        , static ((Type enumblType, Type tvFilterType, Type tvRevealerType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredBothRevealersNullableValueStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
        BuildAddFilteredBothRevealersNullableValueStructCallStructEnumtr<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
        (this Type enumblType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredBothWithNullableValueStructRevealersMethodInfo<TValue, TKFilterBase, TKRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TValue?>)
                   , typeof(PalantírReveal<TValue>), typeof(PalantírReveal<TKRevealBase>), typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
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
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>));
        return (BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase> GetAddFilteredBothRevealersNullableValueStructInvoker
        <TEnumbl, TValue, TKFilterBase, TKRevealerBase>(Type? enumblType = null)
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        enumblType ??= typeof(TEnumbl);
        var tValue        = typeof(TValue);
        var tkFilterBase  = typeof(TKFilterBase);
        var tkRevealBase  = typeof(TKRevealerBase);
        var callAsFactory = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumblType, tValue, tkFilterBase, tkRevealBase),
                      static ((Type enumblType, Type tValue, Type tkFilterBase, Type tkRevealBase) key, bool _) =>
                      {
                          var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                          if (kvpTypes == null)
                              throw new ArgumentException("Expected to receive a KeyValue enumerator");
                          var keyType = kvpTypes.Value.Key;
                          var toInvokeOn =
                              GetStaticMethodInfo
                                  (nameof( AddFilteredEnumerateBothWithNullValueRevealers)
                                , [key.enumblType, keyType, key.tValue, key.tkFilterBase, key.tkRevealBase],
                                  [
                                      typeof(KeyedCollectionMold), key.enumblType
                                    , typeof(KeyValuePredicate<TKFilterBase, TValue?>)
                                    , typeof(PalantírReveal<TValue>)
                                    , typeof(PalantírReveal<TKRevealerBase>)
                                    , typeof(string), typeof(FormatFlags)
                                  ]);

                          var genGenMethod
                              = myMethodInfosCached!
                                  .First(mi =>
                                             mi.Name.Contains(nameof(
                                                 BuildAddFilteredBothRevealersNullableValueStructInvoker)));
                          var concreteGenMethod
                              = genGenMethod.MakeGenericMethod([
                                  typeof(TEnumbl), keyType, key.tValue, key.tkFilterBase, key.tkRevealBase
                              ]);
                          return (BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase>)
                              concreteGenMethod.Invoke(null, [
                                  toInvokeOn, key.enumblType
                                , typeof(PalantírReveal<TValue>)
                                , typeof(PalantírReveal<TKRevealerBase>)
                              ])!;
                      }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKFilterBase, TKRevealerBase> BuildAddFilteredBothRevealersNullableValueStructInvoker
        <TEnumbl, TKey, TValue, TKFilterBase, TKRevealerBase>
        (MethodInfo methodInfo, Type? enumblType = null, Type? tValueRevealer = null, Type? tkRevealBase = null)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealerBase?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        enumblType     ??= typeof(TEnumbl);
        tValueRevealer ??= typeof(PalantírReveal<TValue>);
        tkRevealBase   ??= typeof(PalantírReveal<TKRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKFilterBase, TValue?>)
                   , tValueRevealer, tkRevealBase, typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        // Make space for enumblType local variables
        var ilGenerator     = helperMethod.GetILGenerator();
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        
        // cast TEnumbl value => (enumblType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);

        // call AddFilteredEnumerateBothWithNullValueRevealers(KeyedCollectionMold, TEnumbl, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealerBase>));

        var createInvoker = (BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKFilterBase, TKRevealerBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
          , PalantírReveal<TValue> vRevealer, PalantírReveal<TKRevealerBase> keyRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, enumbl, filterPredicate, vRevealer, keyRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothNullRevealersInvoker<TEnumbl, TKey, TValue> GetAddFilteredBothNullableRevealersCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var callAsFactory = true;
        var invoker =
            (BothNullRevealersInvoker<TEnumbl, TKey, TValue>)
            BothNullableRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblType, enumtrType)
                        , static ((Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredBothNullableRevealersStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue>(key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothNullRevealersInvoker<TEnumbl, TKey, TValue> BuildAddFilteredBothNullableRevealersStructCallStructEnumtr<TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var callEnumtrInvokeMethInf  = enumeratorType.GetAddFilteredBothNullRevealersMethodInfo<TKey, TValue>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), typeof(TEnumbl), typeof(KeyValuePredicate<TKey?, TValue?>)
                  , typeof(PalantírReveal<TValue>), typeof(PalantírReveal<TKey>), typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
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
        ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType);
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

    private static Type? GetEnumeratorType(Type enumerableType) =>
        EnumerableToEnumeratorTypeCache
            .GetOrAdd(enumerableType, static (enumblType) => enumblType.GetEnumeratorType());


    private static MethodInfo? GetEnumeratorMethodInfo(this Type enumerableType) =>
        EnumerableToEnumeratorMethodInfoCache
            .GetOrAdd(enumerableType, static (enumblType) => enumblType.GetEnumeratorMethod());

    public static KeyedCollectionMold AddFilteredEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value == null) return callOn;
        return callOn.AddFilteredEnumerate(value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
    }
    
    public static KeyedCollectionMold AddFilteredEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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
        where TValue : TVFilterBase?
    {
        if (value == null) return callOn;
        return callOn.AddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>
            (value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase? where TValue : TVFilterBase?
    {
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            foreach (var kvp in value)
            {
                var enumeratorType = GetEnumeratorType(actualType);
                if (enumeratorType?.IsValueType ?? false)
                {
                    var structEnumtrInvoker = actualType.GetAddFilteredNoRevealersCallStructEnumtrInvoker<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(enumeratorType);
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
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredEnumerateValueRevealer(value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }
    
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
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
            (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = GetEnumeratorType(actualType);
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
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
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
        where TValue : struct
    {
        if (value == null) return callOn;
        return callOn
            .AddFilteredEnumerateNullValueRevealer(value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }
    
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
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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
        where TValue : struct
    {
        if (value == null) return callOn;
        return callOn.AddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>
            (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = GetEnumeratorType(actualType);
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
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn
            .AddFilteredEnumerateBothRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }
    
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
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
            (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = GetEnumeratorType(actualType);
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
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn
            .AddFilteredEnumerateBothRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }
    
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
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
            (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = GetEnumeratorType(actualType);
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
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
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
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn
            .AddFilteredEnumerateBothWithNullValueRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }
    
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
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
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
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn
            .AddFilteredEnumerateBothWithNullValueRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = GetEnumeratorType(actualType);
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
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
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
        where TValue : struct
    {
        if (value == null) return callOn;
        return callOn
            .AddFilteredEnumerateBothNullRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue?>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = GetEnumeratorType(actualType);
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
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return callOn;
    }

}
