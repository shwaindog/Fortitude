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

public static class KeyedCollectionAddAllEnumerateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> NoRevealersCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type), Delegate>       NoRevealersInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNoNullableStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNoNullableStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNullableValueStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNullableValueStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> BothRevealersNoNullableStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructCallStructEnumtrInvokerCache
        = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructCallStructEnumtrInvokerCache
        = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> BothNullableRevealersCallStructEnumtrInvokerCache = new();

    private delegate KeyedCollectionMold NoRevealersInvoker<in TEnumbl, TKey, TValue>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?;

    private delegate KeyedCollectionMold NoRevealersInvoker<in TEnumbl>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?;

    private delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<in TEnumbl, TKey, TValue, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<in TEnumbl, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<in TEnumbl, TKey, TValue>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<in TEnumbl, TValue>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct;

    private delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumbl, TKey, TValue, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumbl, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableKeyStructInvoker<in TEnumbl, TKey, TValue, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableKeyStructInvoker<in TEnumbl, TKey, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<in TEnumbl, TKey, TValue, out TKRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<in TEnumbl, TValue, out TKRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealBase : notnull;

    private delegate KeyedCollectionMold BothNullRevealersInvoker
        // ReSharper disable twice TypeParameterCanBeVariant
        <in TEnumbl, TKey, TValue>(
            KeyedCollectionMold callOn
          , TEnumbl value
          , PalantírReveal<TValue> valueRevealer
          , PalantírReveal<TKey> keyStyler
          , string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct;

    private static NoRevealersInvoker<TEnumbl, TKey, TValue> GetAddAllNoRevealersCallStructEnumtrInvoker<TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TEnumbl, TKey, TValue>)
            NoRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddAllNoRevealersCallStructEnumtr
                                  <TEnumbl, TKey, TValue>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl, TKey, TValue> BuildAddAllNoRevealersCallStructEnumtr<TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddAllNoRevealersInvokerMethodInfo();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(string);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray
               , typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumbl, TKey, TValue>));
        return (NoRevealersInvoker<TEnumbl, TKey, TValue>)methodInvoker;
    }

    private static NoRevealersInvoker<TEnumbl> GetAddAllNoRevealersInvoker<TEnumbl>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TEnumbl>)
            NoRevealersInvokerCache
                .GetOrAdd((enumblParamType, enumblType), static ((Type enumblParamType, Type enumblType) key, bool _) =>
                {
                    var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                    if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                    var keyType   = kvpTypes.Value.Key;
                    var valueType = kvpTypes.Value.Value;
                    
                    using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                    genericParamTypes[0] = key.enumblType;
                    genericParamTypes[1] = keyType;
                    genericParamTypes[2] = valueType;
                    
                    using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                    methodParamTypes[0] = typeof(KeyedCollectionMold);
                    methodParamTypes[1] = key.enumblType;
                    methodParamTypes[2] = typeof(string);
                    methodParamTypes[3] = typeof(string);
                    methodParamTypes[4] = typeof(FormatFlags);
                    
                    var toInvokeOn = GetStaticMethodInfo(nameof(AddAllEnumerate), genericParamTypes.AsArray, methodParamTypes.AsArray);
                    
                    var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllNoRevealersInvoker)));
                    genericParamTypes[0] = key.enumblParamType;
                    var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                    
                    methodParamTypes[1] = key.enumblParamType;
                    
                    using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                    invokeReflectedArgs[0] = toInvokeOn;
                    invokeReflectedArgs[1] = key.enumblParamType;
                    invokeReflectedArgs[2] = key.enumblType;
                    invokeReflectedArgs[3] = methodParamTypes.AsArray;
                    
                    return (NoRevealersInvoker<TEnumbl>) concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                }, callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl> BuildAddAllNoRevealersInvoker<TEnumbl, TKey, TValue>(MethodInfo methodInfo, Type enumblParamType
      , Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllEnumerate(KeyedCollectionMold, TEnumbl, valueFmtStr, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumbl, TKey, TValue>));
        var createInvoker = (NoRevealersInvoker<TEnumbl, TKey, TValue>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, string? valueFmtStr, string? keyFmtStr, FormatFlags flags) =>
            createInvoker(kcm, enumbl, valueFmtStr, keyFmtStr, flags);

        return Wrapped;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>
        GetAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TVRevealerBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>)
            ValueRevealerNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker
                                  <TEnumbl, TKey, TValue, TVRevealerBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>
        BuildAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker<TEnumbl, TKey, TValue, TVRevealerBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddAllValueRevealerNoNullableStructIInvokerMethodInfo<TVRevealerBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);
        
        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicValueRevealerNoNullableStructInvoke_{enumeratorType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>));
        return (ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>)methodInvoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TVRevealerBase>
        GetAddAllValueRevealerNoNullableStructInvoker<TEnumbl, TVRevealerBase>(
            Type enumblType)
        where TEnumbl : IEnumerable?
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tvRevealBase    = typeof(TVRevealerBase);
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumbl, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tvRevealBase),
                     static ((Type enumblParamType, Type enumblType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;
                         
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tvRevealType;
                    
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);
                         
                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllEnumerateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         
                         var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllValueRevealerNoNullableStructInvoker)));
                         
                         genericParamTypes[0] = key.enumblParamType;
                         var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                         
                         methodParamTypes[1] = key.enumblParamType;
                         
                         using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                         invokeReflectedArgs[0] = toInvokeOn;
                         invokeReflectedArgs[1] = key.enumblParamType;
                         invokeReflectedArgs[2] = key.enumblType;
                         invokeReflectedArgs[3] = methodParamTypes.AsArray;
                         
                         return (ValueRevealerNoNullableStructInvoker<TEnumbl, TVRevealerBase>)
                             concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                     }, true);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TEnumbl, TVRevealerBase> BuildAddAllValueRevealerNoNullableStructInvoker
        <TEnumbl, TKey, TValue, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;
        
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateValueRevealerInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, PalantírReveal<TVRevealerBase> valueRevealer, string? keyFmtStr
          , string? valueFmtStr, FormatFlags flags) => createInvoker(kcm, enumbl, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue>
        GetAddAllValueRevealerNullableValueStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>
        where TValue : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue>)
            ValueRevealerNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddAllValueRevealerNullableValueStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue>
        BuildAddAllValueRevealerNullableValueStructCallStructEnumtr<TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddAllValueRevealerNullableValueStructMethodInfo<TValue>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();
        
        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TValue>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue>));
        return (ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue>)methodInvoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TValue> GetAddAllValueRevealerNullableValueStructInvoker<TEnumbl, TValue>
        (Type enumblType)
        where TEnumbl : IEnumerable
        where TValue : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var tValueType      = typeof(TValue);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumbl, TValue>)
            ValueRevealerNullableValueStructInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tValueType),
                          static ((Type enumblParamType, Type enumblType, Type tValue) key, bool _) =>
                          {
                              var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                              if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                              var keyType = kvpTypes.Value.Key;
                         
                              using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                              genericParamTypes[0] = key.enumblType;
                              genericParamTypes[1] = keyType;
                              genericParamTypes[2] = key.tValue;
                    
                              using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                              methodParamTypes[0] = typeof(KeyedCollectionMold);
                              methodParamTypes[1] = key.enumblType;
                              methodParamTypes[2] = typeof(PalantírReveal<TValue>);
                              methodParamTypes[3] = typeof(string);
                              methodParamTypes[4] = typeof(string);
                              methodParamTypes[5] = typeof(FormatFlags);
                              
                              var toInvokeOn = 
                                  GetStaticMethodInfo(nameof(AddAllEnumerateNullValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                              
                              var genGenMethod = 
                                  myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllValueRevealerNullableValueStructInvoker)));
                              
                              genericParamTypes[0] = key.enumblParamType;
                              var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                              
                              methodParamTypes[1] = key.enumblParamType;
                              
                              using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                              invokeReflectedArgs[0] = toInvokeOn;
                              invokeReflectedArgs[1] = key.enumblParamType;
                              invokeReflectedArgs[2] = key.enumblType;
                              invokeReflectedArgs[3] = methodParamTypes.AsArray;
                              
                              return (ValueRevealerNullableValueStructInvoker<TEnumbl, TValue>)
                                  concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                          }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TEnumbl, TValue> BuildAddAllValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;
        
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateNullValueRevealerInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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
        // call AddAllEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue>));
        var createInvoker = (ValueRevealerNullableValueStructInvoker<TEnumbl, TKey, TValue>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, PalantírReveal<TValue> valueRevealer, string? keyFmtStr
          , string? valueFmtStr, FormatFlags flags) => createInvoker(kcm, enumbl, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }

    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        GetAddAllValueRevealerNoNullableStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>)
            BothRevealersNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddAllEnumerateBothRevealersCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }


    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        BuildAddAllEnumerateBothRevealersCallStructEnumtr<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddAllBothRevealersNoNullableStructMethodInfo<TKRevealBase, TVRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();
        
        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TVRevealBase>);
        methodParamTypes[3] = typeof(PalantírReveal<TKRevealBase>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateBothRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>));
        return (BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKRevealerBase, TVRevealerBase> GetAddAllBothRevealersNoNullableStructInvoker
        <TEnumbl, TKRevealerBase, TVRevealerBase>(Type enumblType)
        where TEnumbl : IEnumerable?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tKRevealBase    = typeof(TKRevealerBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumbl, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tKRevealBase, tvRevealBase),
                     static ((Type enumblParamType, Type enumblType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;
                         
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tkRevealType;
                         genericParamTypes[4] = key.tvRevealType;
                    
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);
                         
                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllEnumerateBothRevealers), genericParamTypes.AsArray , methodParamTypes.AsArray);
                         var genGenMethod =
                             myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllBothRevealersNoNullableStructInvoker)));
                         
                         genericParamTypes[0] = key.enumblParamType;
                         var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                         
                         methodParamTypes[1] = key.enumblParamType;
                         
                         using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                         invokeReflectedArgs[0] = toInvokeOn;
                         invokeReflectedArgs[1] = key.enumblParamType;
                         invokeReflectedArgs[2] = key.enumblType;
                         invokeReflectedArgs[3] = methodParamTypes.AsArray;
                         
                         return (BothRevealersNoNullableStructInvoker<TEnumbl, TKRevealerBase, TVRevealerBase>)
                             concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNoNullableStructInvoker<TEnumbl, TKRevealerBase, TVRevealerBase> BuildAddAllBothRevealersNoNullableStructInvoker
        <TEnumbl, TKey, TValue, TKRevealerBase, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealerBase?
        where TValue : TVRevealerBase?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;
        
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllEnumerateBothRevealers(KeyedCollectionMold, TEnumbl, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate
                (typeof(BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKRevealerBase, TVRevealerBase>));
        var createInvoker = (BothRevealersNoNullableStructInvoker<TEnumbl, TKey, TValue, TKRevealerBase, TVRevealerBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, PalantírReveal<TVRevealerBase> vRevealer
          , PalantírReveal<TKRevealerBase> keyRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, enumbl, vRevealer, keyRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVRevealBase>
        GetAddAllBothRevealersNullableKeyStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var tvRevealerType  = typeof(TVRevealBase);
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVRevealBase>)
            BothRevealersNullableKeyStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tvRevealerType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type tvRevealerType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddAllBothRevealersNullableKeyStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TVRevealBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVRevealBase>
        BuildAddAllBothRevealersNullableKeyStructCallStructEnumtr<TEnumbl, TKey, TValue, TVRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddAllBothRevealersNullableKeyStructMethodInfo<TKey, TVRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();
        
        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TVRevealBase>);
        methodParamTypes[3] = typeof(PalantírReveal<TKey>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVRevealBase>));
        return (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVRevealerBase> GetAddAllBothRevealersNullableKeyStructInvoker
        <TEnumbl, TKey, TVRevealerBase>(Type enumblType)
        where TEnumbl : IEnumerable
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var enumblParamType  = typeof(TEnumbl);
        var tKeyType         = typeof(TKey);
        var tvRevealBaseType = typeof(TVRevealerBase);
        var callAsFactory    = true;
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVRevealerBase>)
            BothRevealersNullableKeyStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tKeyType, tvRevealBaseType),
                     static ((Type enumblParamType, Type enumblType, Type tKey, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var valueType = kvpTypes.Value.Value;
                         
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = key.tKey;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tvRevealType;
                    
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TKey>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);
                         
                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllEnumerateBothWithNullKeyRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         var genGenMethod
                             = myMethodInfosCached!
                                 .First(mi => mi.Name.Contains(nameof(BuildAddAllBothRevealersNullableKeyStructInvoker)));
                         
                         genericParamTypes[0] = key.enumblParamType;
                         var concreteGenMethod
                             = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                         
                         methodParamTypes[1] = key.enumblParamType;
                         
                         using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                         invokeReflectedArgs[0] = toInvokeOn;
                         invokeReflectedArgs[1] = key.enumblParamType;
                         invokeReflectedArgs[2] = key.enumblType;
                         invokeReflectedArgs[3] = methodParamTypes.AsArray;
                         
                         return (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVRevealerBase>)
                             concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVRevealerBase> BuildAddAllBothRevealersNullableKeyStructInvoker
        <TEnumbl, TKey, TValue, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes , typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllEnumerateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumbl, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>));
        var createInvoker = (BothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TValue, TVRevealerBase>)methodInvoker;
        
        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumtr, PalantírReveal<TVRevealerBase> vRevealer
          , PalantírReveal<TKey> kRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, enumtr, vRevealer, kRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKRevealBase>
        GetAddAllBothRevealersNullableValueStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tvRevealerType  = typeof(TKRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKRevealBase>)
            BothRevealersNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tvRevealerType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type tvRevealerType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddAllBothRevealersNullableValueStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue, TKRevealBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKRevealBase>
        BuildAddAllBothRevealersNullableValueStructCallStructEnumtr<TEnumbl, TKey, TValue, TKRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddAllBothRevealersNullableValueStructMethodInfo<TValue, TKRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();
        
        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TValue>);
        methodParamTypes[3] = typeof(PalantírReveal<TKRevealBase>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateBothWithNullValueRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKRevealBase>));
        return (BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKRevealerBase> GetAddAllBothRevealersNullableValueStructInvoker
        <TEnumbl, TValue, TKRevealerBase>(Type enumblType)
        where TEnumbl : IEnumerable
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tValue          = typeof(TValue);
        var tkRevealBase    = typeof(TKRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKRevealerBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tValue, tkRevealBase),
                     static ((Type enumblParamType, Type enumblType, Type tValue, Type tkRevealBase) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType = kvpTypes.Value.Key;
                         
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumblType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = key.tValue;
                         genericParamTypes[3] = key.tkRevealBase;
                    
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(PalantírReveal<TValue>);
                         methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllEnumerateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         var genGenMethod
                             = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllBothRevealersNullableValueStructInvoker)));
                         
                         genericParamTypes[0] = key.enumblParamType;
                         var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                         
                         methodParamTypes[1] = key.enumblParamType;
                         
                         using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                         invokeReflectedArgs[0] = toInvokeOn;
                         invokeReflectedArgs[1] = key.enumblParamType;
                         invokeReflectedArgs[2] = key.enumblType;
                         invokeReflectedArgs[3] = methodParamTypes.AsArray;
                         
                         return (BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKRevealerBase>)
                             concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKRevealerBase> BuildAddAllBothRevealersNullableValueStructInvoker
        <TEnumbl, TKey, TValue, TKRevealerBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealerBase?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;
        
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
        // Make space for enumblType local variables
        var ilGenerator     = helperMethod.GetILGenerator();
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

        // call AddAllEnumerateBothWithNullValueRevealers(KeyedCollectionMold, TEnumbl, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKRevealerBase>));

        var createInvoker = (BothRevealersNullableValueStructInvoker<TEnumbl, TKey, TValue, TKRevealerBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumbl? enumbl, PalantírReveal<TValue> vRevealer
          , PalantírReveal<TKRevealerBase> keyRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, enumbl, vRevealer, keyRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothNullRevealersInvoker<TEnumbl, TKey, TValue> GetAddAllBothNullableRevealersCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>
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
                              key.enumblType.BuildAddAllBothNullableRevealersStructCallStructEnumtr
                                  <TEnumbl, TKey, TValue>(key.enumblParamType,  key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothNullRevealersInvoker<TEnumbl, TKey, TValue> BuildAddAllBothNullableRevealersStructCallStructEnumtr<TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;
        
        var callEnumtrInvokeMethInf  = enumeratorType.GetAddAllBothNullRevealersMethodInfo<TKey, TValue>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();
        
        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TValue>);
        methodParamTypes[3] = typeof(PalantírReveal<TKey>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateBothNullRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothNullRevealersInvoker<TEnumbl, TKey, TValue>));
        return (BothNullRevealersInvoker<TEnumbl, TKey, TValue>)methodInvoker;
    }

    private static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(KeyedCollectionAddAllEnumerateExtensions).GetMethods(NonPublic | Public | Static);

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
    

    public static KeyedCollectionMold AddAllEnumerate<TEnumbl>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllNoRevealersInvoker<TEnumbl>(actualType);
        return invoker(callOn, value, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerate<TEnumbl>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddAllNoRevealersInvoker<TEnumbl>(actualType);
        return invoker(callOn, value.Value, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerate<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker = actualType.GetAddAllNoRevealersCallStructEnumtrInvoker<TEnumbl, TKey, TValue>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, value, valueFormatString, keyFormatString, formatFlags);
            }

            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var kvp in value)
            {
                if (callOn.ItemCount == 0)
                {
                    callOn.BeforeFirstElement(mws);
                }
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return mws.Mold;
    }

    public static KeyedCollectionMold AddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllValueRevealerNoNullableStructInvoker<TEnumbl, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddAllValueRevealerNoNullableStructInvoker<TEnumbl, TVRevealBase>(actualType);
        return invoker(callOn, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker
                    = actualType.GetAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker<TEnumbl, TKey, TValue, TVRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var kvp in value)
            {
                if (callOn.ItemCount == 0)
                {
                    callOn.BeforeFirstElement(mws);
                }
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllValueRevealerNullableValueStructInvoker<TEnumbl, TValue>(actualType);
        return invoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddAllValueRevealerNullableValueStructInvoker<TEnumbl, TValue>(actualType);
        return invoker(callOn, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker = actualType.GetAddAllValueRevealerNullableValueStructCallStructEnumtrInvoker
                    <TEnumbl, TKey, TValue>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var kvp in value)
            {
                if (callOn.ItemCount == 0)
                {
                    callOn.BeforeFirstElement(mws);
                }
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllBothRevealersNoNullableStructInvoker<TEnumbl, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddAllBothRevealersNoNullableStructInvoker<TEnumbl, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker = actualType.GetAddAllValueRevealerNoNullableStructCallStructEnumtrInvoker
                    <TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var kvp in value)
            {
                if (callOn.ItemCount == 0)
                {
                    callOn.BeforeFirstElement(mws);
                }
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealerBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealerBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllBothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVRevealerBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealerBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealerBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddAllBothRevealersNullableKeyStructInvoker<TEnumbl, TKey, TVRevealerBase>(actualType);
        return invoker(callOn, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker = actualType.GetAddAllBothRevealersNullableKeyStructCallStructEnumtrInvoker
                    <TEnumbl, TKey, TValue, TVRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            var kvpType = typeof(KeyValuePair<TKey?, TValue>);
            foreach (var kvp in value)
            {
                if (callOn.ItemCount == 0)
                {
                    callOn.BeforeFirstElement(mws);
                }
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealerBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealerBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKRevealerBase : notnull
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllBothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKRevealerBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealerBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealerBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKRevealerBase : notnull
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddAllBothRevealersNullableValueStructInvoker<TEnumbl, TValue, TKRevealerBase>(actualType);
        return invoker(callOn, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker = actualType.GetAddAllBothRevealersNullableValueStructCallStructEnumtrInvoker
                    <TEnumbl, TKey, TValue, TKRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            foreach (var kvp in value)
            {
                if (callOn.ItemCount == 0)
                {
                    callOn.BeforeFirstElement(mws);
                }
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct
    {
        if (value == null) return callOn;
        return callOn.AddAllEnumerateBothNullRevealers(value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker = actualType.GetAddAllBothNullableRevealersCallStructEnumtrInvoker
                    <TEnumbl, TKey, TValue>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            foreach (var kvp in value)
            {
                if (callOn.ItemCount == 0)
                {
                    callOn.BeforeFirstElement(mws);
                }
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }
}
