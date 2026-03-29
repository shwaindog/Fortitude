// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public static class WhenPopulatedAddAllEnumerateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;
    
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> NoRevealersCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> NoRevealersInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNoNullableStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNoNullableStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNullableValueStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNullableValueStructInvokerCache                 = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructCallStructEnumtrInvokerCache
        = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructCallStructEnumtrInvokerCache
        = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothNullableRevealersCallStructEnumtrInvokerCache = new();
    
    private delegate TMold NoRevealersInvoker<TMold, in TEnumbl, TKey, TValue>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?;
    
    private delegate TMold NoRevealersInvoker<TMold, in TEnumbl>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?;
    
    private delegate TMold ValueRevealerNoNullableStructInvoker<TMold, in TEnumbl, TKey, TValue, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    private delegate TMold ValueRevealerNoNullableStructInvoker<TMold, in TEnumbl, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate TMold ValueRevealerNullableValueStructInvoker<TMold, in TEnumbl, TKey, TValue>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate TMold ValueRevealerNullableValueStructInvoker<TMold, in TEnumbl, TValue>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TValue : struct;

    private delegate TMold BothRevealersNoNullableStructInvoker<TMold, in TEnumbl, TKey, TValue, out TKRevealBase, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    private delegate TMold BothRevealersNoNullableStructInvoker<TMold, in TEnumbl, out TKRevealBase, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate TMold BothRevealersNullableKeyStructInvoker<TMold, in TEnumbl, TKey, TValue, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate TMold BothRevealersNullableKeyStructInvoker<TMold, in TEnumbl, TKey, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate TMold BothRevealersNullableValueStructInvoker<TMold, in TEnumbl, TKey, TValue, out TKRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate TMold BothRevealersNullableValueStructInvoker<TMold, in TEnumbl, TValue, out TKRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealBase : notnull;
    
    // ReSharper disable twice TypeParameterCanBeVariant
    private delegate TMold BothNullRevealersInvoker<TMold, in TEnumbl, TKey, TValue>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct;

    private static NoRevealersInvoker<TMold, TEnumbl, TKey, TValue> GetWhenPopulatedAddAllNoRevealersCallStructEnumtrInvoker<TMold, TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumtrType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TMold, TEnumbl, TKey, TValue>)
            NoRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((moldType, enumblParamType, enumblType, enumtrType)
                        , static ((Type _, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildWhenPopulatedAddAllNoRevealersCallStructEnumtr
                                  <TMold, TEnumbl, TKey, TValue>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TMold, TEnumbl, TKey, TValue> BuildWhenPopulatedAddAllNoRevealersCallStructEnumtr<TMold, TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetWhenPopulatedAddAllNoRevealersInvokerMethodInfo<TMold>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[2].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        ilGenerator.Emit(OpCodes.Ldarg_2);
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

        // call WhenPopulatedAddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TMold, TEnumbl, TKey, TValue>));
        return (NoRevealersInvoker<TMold, TEnumbl, TKey, TValue>)methodInvoker;
    }

    private static NoRevealersInvoker<TMold, TEnumbl> GetWhenPopulatedAddAllNoRevealersInvoker<TMold, TEnumbl>(Type enumblType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
    {
        var moldType = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TMold, TEnumbl>)
            NoRevealersInvokerCache
                .GetOrAdd
                    ((moldType, enumblParamType, enumblType)
                    , static ((Type moldType, Type enumblParamType, Type enumblType) key, bool _) =>
                    {
                        var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                        if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                        var keyType   = kvpTypes.Value.Key;
                        var valueType = kvpTypes.Value.Value;
                        var toInvokeOn = 
                            GetStaticMethodInfo(nameof(WhenPopulatedAddAllEnumerate)
                                              , [key.moldType, key.enumblType, keyType, valueType],
                        [ typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumblType, typeof(string)
                          , typeof(string), typeof(FormatFlags)]);

                        var genGenMethod = myMethodInfosCached!
                                .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllNoRevealersInvoker)));
                        var concreteGenMethod
                            = genGenMethod.MakeGenericMethod([key.moldType, key.enumblParamType, keyType, valueType]);
                        return (NoRevealersInvoker<TMold, TEnumbl>)
                            concreteGenMethod.Invoke(null, [toInvokeOn, key.enumblParamType, key.enumblType])!;
                    }, callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TMold, TEnumbl> BuildWhenPopulatedAddAllNoRevealersInvoker<TMold, TEnumbl, TKey, TValue>(MethodInfo methodInfo
      , Type enumblParamType , Type enumblType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", typeof(TMold),
                [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType
                  , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call WhenPopulatedAddAllEnumerate(KeyedCollectionMold, TEnumbl, valueFmtStr, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TMold, TEnumbl, TKey, TValue>));
        var createInvoker = (NoRevealersInvoker<TMold, TEnumbl, TKey, TValue>)methodInvoker;

        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcf, string fieldName, TEnumbl? enumbl, string? valueFmtStr
          , string? keyFmtStr, FormatFlags flags) => createInvoker(kcf, fieldName, enumbl, valueFmtStr, keyFmtStr, flags);

        return Wrapped;
    }

    private static ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>
        GetWhenPopulatedAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker
        <TMold, TEnumbl, TKey, TValue, TVRevealerBase>(this Type enumblType, Type enumtrType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>)
            ValueRevealerNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((moldType, enumblParamType, enumblType, enumtrType)
                        , static ((Type _, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildWhenPopulatedAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker
                                  <TMold, TEnumbl, TKey, TValue, TVRevealerBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>
        BuildWhenPopulatedAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType
            .GetWhenPopulatedAddAllValueRevealerNoNullableStructIInvokerMethodInfo<TMold, TVRevealerBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[2].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicValueRevealerNoNullableStructInvoke_{enumeratorType.Name}", typeof(TMold),
                [
                    typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, typeof(PalantírReveal<TVRevealerBase>)
                  , typeof(string), typeof(string), typeof(FormatFlags)
                ], typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
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
        ilGenerator.Emit(OpCodes.Ldarg_2);
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

        // call WhenPopulatedAddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>));
        return (ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>)methodInvoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TVRevealerBase>
        GetWhenPopulatedAddAllValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TVRevealerBase>(
            Type enumblType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TVRevealerBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var tvRevealBase    = typeof(TVRevealerBase);
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((moldType, enumblParamType, enumblType, tvRevealBase),
                     static ((Type moldType, Type enumblParamType, Type enumblType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;
                         var toInvokeOn =
                             GetStaticMethodInfo
                                 (nameof(WhenPopulatedAddAllEnumerateValueRevealer)
                                , [key.moldType, key.enumblType, keyType, valueType, key.tvRevealType],
                                  [ typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumblType
                                    , typeof(PalantírReveal<TVRevealerBase>), typeof(string), typeof(string), typeof(FormatFlags) ]);

                         var genGenMethod
                             = myMethodInfosCached!
                                 .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllValueRevealerNoNullableStructInvoker)));
                         var concreteGenMethod
                             = genGenMethod.MakeGenericMethod([key.moldType, key.enumblParamType, keyType, valueType, key.tvRevealType]);
                         return (ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TVRevealerBase>)
                             concreteGenMethod.Invoke(null, [
                                 toInvokeOn, key.enumblParamType, key.enumblType, typeof(PalantírReveal<TVRevealerBase>)
                             ])!;
                     }, true);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TVRevealerBase> BuildWhenPopulatedAddAllValueRevealerNoNullableStructInvoker
        <TMold, TEnumbl, TKey, TValue, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type? tvRevealBase = null)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        tvRevealBase ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateValueRevealerInvoke_{enumblType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, tvRevealBase
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call WhenPopulatedAddAllEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>)methodInvoker;

        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumbl? enumbl, PalantírReveal<TVRevealerBase> valueRevealer
          , string? keyFmtStr, string? valueFmtStr, FormatFlags flags) => 
            createInvoker(kcm, fieldName, enumbl, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }

    private static ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue>
        GetWhenPopulatedAddAllValueRevealerNullableValueStructCallStructEnumtrInvoker
        <TMold, TEnumbl, TKey, TValue>(this Type enumblType, Type enumtrType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue>)
            ValueRevealerNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((moldType, enumblParamType, enumblType, enumtrType)
                        , static ((Type _, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildWhenPopulatedAddAllValueRevealerNullableValueStructCallStructEnumtr
                                  <TMold, TEnumbl, TKey, TValue>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue>
        BuildWhenPopulatedAddAllValueRevealerNullableValueStructCallStructEnumtr<TMold, TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = 
            enumeratorType.GetWhenPopulatedAddAllValueRevealerNullableValueStructMethodInfo<TMold, TValue>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[2].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(TMold),
                [
                    typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, typeof(PalantírReveal<TValue>)
                  , typeof(string) , typeof(string), typeof(FormatFlags)
                ], typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
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
        ilGenerator.Emit(OpCodes.Ldarg_2);
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

        // call WhenPopulatedAddAllIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue>));
        return (ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue>)methodInvoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TValue> 
        GetWhenPopulatedAddAllValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TValue>(Type enumblType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TValue : struct
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var tValueType      = typeof(TValue);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TValue>)
            ValueRevealerNullableValueStructInvokerCache
                .GetOrAdd
                    ((moldType, enumblParamType, enumblType, tValueType),
                      static ((Type moldType, Type enumblParamType, Type enumblType, Type tValue) key, bool _) =>
                      {
                          var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                          if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                          var keyType = kvpTypes.Value.Key;
                          var toInvokeOn =
                              GetStaticMethodInfo
                                  (nameof(WhenPopulatedAddAllEnumerateNullValueRevealer)
                                 , [key.moldType, key.enumblParamType, keyType, key.tValue]
                                  , [ typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumblParamType
                                     , typeof(PalantírReveal<TValue>), typeof(string), typeof(string), typeof(FormatFlags) ]);

                          var genGenMethod
                              = myMethodInfosCached!
                                  .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllValueRevealerNullableValueStructInvoker)));
                          var concreteGenMethod
                              = genGenMethod.MakeGenericMethod([key.moldType, key.enumblParamType , keyType, key.tValue]);
                          return (ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TValue>)
                              concreteGenMethod.Invoke(null, [toInvokeOn, key.enumblParamType, key.enumblType, typeof(PalantírReveal<TValue>)])!;
                      }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TValue> 
        BuildWhenPopulatedAddAllValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type? valueRevealerType = null)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        valueRevealerType ??= typeof(PalantírReveal<TValue>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateNullValueRevealerInvoke_{enumblType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, valueRevealerType
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }
        // call WhenPopulatedAddAllEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue>));
        var createInvoker = (ValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue>)methodInvoker;

        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumbl? enumbl, PalantírReveal<TValue> valueRevealer, string? keyFmtStr
          , string? valueFmtStr, FormatFlags flags) => createInvoker(kcm, fieldName, enumbl, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }

    private static BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        GetWhenPopulatedAddAllValueRevealerNoNullableStructCallStructEnumtrInvoker
        <TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>)
            BothRevealersNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((moldType, enumblParamType, enumblType, enumtrType)
                        , static ((Type _, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildWhenPopulatedAddAllEnumerateBothRevealersCallStructEnumtr
                                  <TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }


    private static BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        BuildWhenPopulatedAddAllEnumerateBothRevealersCallStructEnumtr<TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetWhenPopulatedAddAllBothRevealersNoNullableStructMethodInfo<TMold, TKRevealBase, TVRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[2].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(TMold),
                [
                    typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, typeof(PalantírReveal<TVRevealBase>)
                  , typeof(PalantírReveal<TKRevealBase>), typeof(string), typeof(FormatFlags)
                ], typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
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
        ilGenerator.Emit(OpCodes.Ldarg_2);
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

        // call WhenPopulatedAddAllIterateBothRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>));
        return (BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKRevealerBase, TVRevealerBase> GetWhenPopulatedAddAllBothRevealersNoNullableStructInvoker
        <TMold, TEnumbl, TKRevealerBase, TVRevealerBase>(Type enumblType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var tKRevealBase    = typeof(TKRevealerBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((moldType, enumblParamType, enumblType, tKRevealBase, tvRevealBase),
                     static ((Type moldType, Type enumblParamType, Type enumblType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;
                         var toInvokeOn =
                             GetStaticMethodInfo
                                 (nameof(WhenPopulatedAddAllEnumerateBothRevealers)
                                , [key.moldType, key.enumblType, keyType, valueType, key.tkRevealType, key.tvRevealType]
                                , [
                                      typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumblType
                                    , typeof(PalantírReveal<TVRevealerBase>)
                                    , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                    , typeof(FormatFlags)
                                  ]);

                         var genGenMethod =
                             myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllBothRevealersNoNullableStructInvoker)));
                         var concreteGenMethod =
                             genGenMethod.MakeGenericMethod([key.moldType, key.enumblParamType, keyType, valueType, key.tkRevealType, key.tvRevealType]);
                         return (BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKRevealerBase, TVRevealerBase>)
                             concreteGenMethod.Invoke(null,
                             [
                                 toInvokeOn, key.enumblParamType, key.enumblType, typeof(PalantírReveal<TKRevealerBase>)
                               , typeof(PalantírReveal<TVRevealerBase>)
                             ])!;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKRevealerBase, TVRevealerBase> BuildWhenPopulatedAddAllBothRevealersNoNullableStructInvoker
        <TMold, TEnumbl, TKey, TValue, TKRevealerBase, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type? tkRevealer = null
          , Type? tvRevealer = null)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealerBase?
        where TValue : TVRevealerBase?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        tkRevealer ??= typeof(PalantírReveal<TKRevealerBase>);
        tvRevealer ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateBothRevealers_{enumblType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, tvRevealer
                   , tkRevealer, typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call WhenPopulatedAddAllEnumerateBothRevealers(KeyedCollectionMold, TEnumbl, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate
                (typeof(BothRevealersNoNullableStructInvoker<TMold,TEnumbl, TKey, TValue, TKRevealerBase, TVRevealerBase>));
        var createInvoker = (BothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealerBase, TVRevealerBase>)methodInvoker;

        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumbl? enumbl, PalantírReveal<TVRevealerBase> vRevealer
          , PalantírReveal<TKRevealerBase> keyRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, fieldName, enumbl, vRevealer, keyRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealBase>
        GetWhenPopulatedAddAllBothRevealersNullableKeyStructCallStructEnumtrInvoker
        <TMold, TEnumbl, TKey, TValue, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var tvRevealerType  = typeof(TVRevealBase);
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealBase>)
            BothRevealersNullableKeyStructCallStructEnumtrInvokerCache
                .GetOrAdd((moldType, enumblParamType, enumblType, tvRevealerType, enumtrType)
                        , static ((Type _, Type enumblParamType, Type enumblType, Type tvRevealerType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildWhenPopulatedAddAllBothRevealersNullableKeyStructCallStructEnumtr
                                  <TMold, TEnumbl, TKey, TValue, TVRevealBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealBase>
        BuildWhenPopulatedAddAllBothRevealersNullableKeyStructCallStructEnumtr<TMold, TEnumbl, TKey, TValue, TVRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetWhenPopulatedAddAllBothRevealersNullableKeyStructMethodInfo<TMold, TKey, TVRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[2].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(TMold),
                [ typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, typeof(PalantírReveal<TVRevealBase>)
                  , typeof(PalantírReveal<TKey>), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
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
        ilGenerator.Emit(OpCodes.Ldarg_2);
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

        // call WhenPopulatedAddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealBase>));
        return (BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TVRevealerBase> GetWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker
        <TMold, TEnumbl, TKey, TVRevealerBase>(Type enumblType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var moldType         = typeof(TMold);
        var enumblParamType  = typeof(TEnumbl);
        var tKeyType         = typeof(TKey);
        var tvRevealBaseType = typeof(TVRevealerBase);
        var callAsFactory    = true;
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TVRevealerBase>)
            BothRevealersNullableKeyStructInvokerCache
                .GetOrAdd
                    ((moldType, enumblParamType, enumblType, tKeyType, tvRevealBaseType),
                     static ((Type moldType, Type enumblParamType, Type enumblType, Type tKey, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var valueType = kvpTypes.Value.Value;
                         var toInvokeOn =
                             GetStaticMethodInfo
                                 (nameof(WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers)
                                , [key.moldType, key.enumblType, key.tKey, valueType, key.tvRevealType],
                                  [
                                      typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumblType
                                    , typeof(PalantírReveal<TVRevealerBase>), typeof(PalantírReveal<TKey>)
                                    , typeof(string), typeof(FormatFlags)
                                  ]);

                         var genGenMethod
                             = myMethodInfosCached!
                                 .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker)));
                         var concreteGenMethod
                             = genGenMethod.MakeGenericMethod([ key.moldType, key.enumblParamType, key.tKey, valueType, key.tvRevealType ]);
                         return (BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TVRevealerBase>)
                             concreteGenMethod.Invoke(null, [
                                 toInvokeOn, key.enumblParamType, key.enumblType
                               , typeof(PalantírReveal<TKey>)
                               , typeof(PalantírReveal<TVRevealerBase>)
                             ])!;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TVRevealerBase> BuildWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker
        <TMold, TEnumbl, TKey, TValue, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type? tKeyRevealerType = null
          , Type? tvRevealBaseType = null)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        tKeyRevealerType ??= typeof(PalantírReveal<TKey>);
        tvRevealBaseType ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateBothRevealers_{enumblType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, tvRevealBaseType
                   , tKeyRevealerType, typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumbl, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>));
        var createInvoker = (BothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TValue, TVRevealerBase>)methodInvoker;


        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumbl? enumtr, PalantírReveal<TVRevealerBase> vRevealer
          , PalantírReveal<TKey> kRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, fieldName, enumtr, vRevealer, kRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase>
        GetWhenPopulatedAddAllBothRevealersNullableValueStructCallStructEnumtrInvoker
        <TMold, TEnumbl, TKey, TValue, TKRevealBase>(this Type enumblType, Type enumtrType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var tvRevealerType  = typeof(TKRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase>)
            BothRevealersNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((moldType, enumblParamType, enumblType, tvRevealerType, enumtrType)
                        , static ((Type _, Type enumblParamType, Type enumblType, Type tvRevealerType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildWhenPopulatedAddAllBothRevealersNullableValueStructCallStructEnumtr
                                  <TMold, TEnumbl, TKey, TValue, TKRevealBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase>
        BuildWhenPopulatedAddAllBothRevealersNullableValueStructCallStructEnumtr<TMold, TEnumbl, TKey, TValue, TKRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetWhenPopulatedAddAllBothRevealersNullableValueStructMethodInfo<TMold, TValue, TKRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[2].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(TMold),
                [ typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, typeof(PalantírReveal<TValue>)
                  , typeof(PalantírReveal<TKRevealBase>), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
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
        ilGenerator.Emit(OpCodes.Ldarg_2);
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

        // call WhenPopulatedAddAllIterateBothWithNullValueRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase>));
        return (BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TValue, TKRevealerBase> GetWhenPopulatedAddAllBothRevealersNullableValueStructInvoker
        <TMold, TEnumbl, TValue, TKRevealerBase>(Type enumblType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var tValue          = typeof(TValue);
        var tkRevealBase    = typeof(TKRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TValue, TKRevealerBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((moldType, enumblParamType, enumblType, tValue, tkRevealBase),
                     static ((Type moldType, Type enumblParamType, Type enumblType, Type tValue, Type tkRevealBase) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType = kvpTypes.Value.Key;
                         var toInvokeOn =
                             GetStaticMethodInfo
                                 (nameof(WhenPopulatedAddAllEnumerateBothWithNullValueRevealers)
                                , [key.moldType, key.enumblType, keyType, key.tValue, key.tkRevealBase],
                                  [
                                      typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumblType
                                    , typeof(PalantírReveal<TValue>)
                                    , typeof(PalantírReveal<TKRevealerBase>)
                                    , typeof(string), typeof(FormatFlags)
                                  ]);

                         var genGenMethod
                             = myMethodInfosCached!
                                 .First(mi =>
                                            mi.Name.Contains(nameof(
                                                                 BuildWhenPopulatedAddAllBothRevealersNullableValueStructInvoker)));
                         var concreteGenMethod
                             = genGenMethod.MakeGenericMethod([
                                 key.moldType, key.enumblParamType, keyType, key.tValue, key.tkRevealBase
                             ]);
                         return (BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TValue, TKRevealerBase>)
                             concreteGenMethod.Invoke(null, [
                                 toInvokeOn, key.enumblParamType, key.enumblType
                               , typeof(PalantírReveal<TValue>)
                               , typeof(PalantírReveal<TKRevealerBase>)
                             ])!;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TValue, TKRevealerBase> BuildWhenPopulatedAddAllBothRevealersNullableValueStructInvoker
        <TMold, TEnumbl, TKey, TValue, TKRevealerBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type? tValueRevealer = null, Type? tkRevealBase = null)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealerBase?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;
        
        tValueRevealer ??= typeof(PalantírReveal<TValue>);
        tkRevealBase   ??= typeof(PalantírReveal<TKRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateBothRevealers_{enumblType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, tValueRevealer
                   , tkRevealBase, typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
        // Make space for enumblType local variables
        var ilGenerator     = helperMethod.GetILGenerator();
        if (requiresCast || requiresUnboxing)
        {
            // Make space for enumblType local variables
            var enumblLocalType = ilGenerator.DeclareLocal(enumblType);

            // cast TEnumbl value => (enumblType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            if (requiresUnboxing) { ilGenerator.Emit(OpCodes.Unbox_Any, enumblLocalType.LocalType); }
            else { ilGenerator.Emit(OpCodes.Castclass, enumblLocalType.LocalType); }
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call WhenPopulatedAddAllEnumerateBothWithNullValueRevealers(KeyedCollectionMold, TEnumbl, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = 
            helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealerBase>));

        var createInvoker = (BothRevealersNullableValueStructInvoker<TMold, TEnumbl, TKey, TValue, TKRevealerBase>)methodInvoker;

        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumbl? enumbl, PalantírReveal<TValue> vRevealer
          , PalantírReveal<TKRevealerBase> keyRevealer, string? valueFmtString, FormatFlags flags) =>
            createInvoker(kcm, fieldName, enumbl, vRevealer, keyRevealer, valueFmtString, flags);

        return Wrapped;
    }

    private static BothNullRevealersInvoker<TMold, TEnumbl, TKey, TValue> GetWhenPopulatedAddAllBothNullableRevealersCallStructEnumtrInvoker
        <TMold, TEnumbl, TKey, TValue>(this Type enumblType, Type enumtrType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var moldType        = typeof(TMold);
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (BothNullRevealersInvoker<TMold, TEnumbl, TKey, TValue>)
            BothNullableRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((moldType, enumblParamType, enumblType, enumtrType)
                        , static ((Type _, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildWhenPopulatedAddAllBothNullableRevealersStructCallStructEnumtr
                                  <TMold, TEnumbl, TKey, TValue>(key.enumblParamType,  key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothNullRevealersInvoker<TMold, TEnumbl, TKey, TValue> BuildWhenPopulatedAddAllBothNullableRevealersStructCallStructEnumtr<TMold, TEnumbl, TKey, TValue>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;
        
        var callEnumtrInvokeMethInf  = enumeratorType.GetWhenPopulatedAddAllBothNullRevealersMethodInfo<TMold, TKey, TValue>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[2].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(TMold),
                [
                    typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), enumblParamType, typeof(PalantírReveal<TValue>)
                  , typeof(PalantírReveal<TKey>), typeof(string), typeof(FormatFlags)
                ], typeof(WhenPopulatedAddAllEnumerateExtensions).Module, false);
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
        ilGenerator.Emit(OpCodes.Ldarg_2);
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

        // call WhenPopulatedAddAllIterateBothNullRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothNullRevealersInvoker<TMold, TEnumbl, TKey, TValue>));
        return (BothNullRevealersInvoker<TMold, TEnumbl, TKey, TValue>)methodInvoker;
    }

    private static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(WhenPopulatedAddAllEnumerateExtensions).GetMethods(NonPublic | Public | Static);

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
            var enumeratorType = checkParameterInfos[2].ParameterType;
            if (enumeratorType.IsNullable()) continue;
            var isParameterMatch = true;
            for (var i = 3; i < findParamTypes.Length; i++)
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

    public static TMold WhenPopulatedAddAllEnumerate<TMold, TEnumbl>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetWhenPopulatedAddAllNoRevealersInvoker<TMold, TEnumbl>(actualType);
        return invoker(callOn, fieldName, value, valueFormatString, keyFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerate<TMold, TEnumbl>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllNoRevealersInvoker<TMold, TEnumbl>(actualType);
        return invoker(callOn, fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerate<TMold, TEnumbl, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            callOn.CollectionType ??= actualType;
            var enumeratorType = actualType.GetEnumeratorType()!;
            if (enumeratorType.IsValueType)
            {
                var structEnumtrInvoker = actualType.GetWhenPopulatedAddAllNoRevealersCallStructEnumtrInvoker<TMold, TEnumbl, TKey, TValue>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, fieldName, value, valueFormatString, keyFormatString, formatFlags);
            }
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
            foreach (var kvp in value)
            {
                if (ekcm == null)
                {
                    mws.FieldNameJoin(fieldName);
                    ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
            }
            if(ekcm != null) ekcm.AppendCollectionComplete();
        }
        return mws.Mold;
    }

    public static TMold WhenPopulatedAddAllEnumerateValueRevealer<TMold, TEnumbl, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetWhenPopulatedAddAllValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateValueRevealer<TMold, TEnumbl, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : struct, IEnumerable
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllValueRevealerNoNullableStructInvoker<TMold, TEnumbl, TVRevealBase>(actualType);
        return invoker(callOn, fieldName,  value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateValueRevealer<TMold, TEnumbl, TKey, TValue, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            callOn.CollectionType ??= actualType;
            var enumeratorType = actualType.GetEnumeratorType()!;
            if (enumeratorType.IsValueType)
            {
                var structEnumtrInvoker
                    = actualType.GetWhenPopulatedAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker<TMold, TEnumbl, TKey, TValue, TVRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }

            ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
            foreach (var kvp in value)
            {
                if (ekcm == null)
                {
                    mws.FieldNameJoin(fieldName);
                    ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if(ekcm != null) ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllEnumerateNullValueRevealer<TMold, TEnumbl, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetWhenPopulatedAddAllValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TValue>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateNullValueRevealer<TMold, TEnumbl, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : struct, IEnumerable
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllValueRevealerNullableValueStructInvoker<TMold, TEnumbl, TValue>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateNullValueRevealer<TMold, TEnumbl, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            callOn.CollectionType ??= actualType;
            var enumeratorType = actualType.GetEnumeratorType()!;
            if (enumeratorType.IsValueType)
            {
                var structEnumtrInvoker = actualType.GetWhenPopulatedAddAllValueRevealerNullableValueStructCallStructEnumtrInvoker
                    <TMold, TEnumbl, TKey, TValue>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
            foreach (var kvp in value)
            {
                if (ekcm == null)
                {
                    mws.FieldNameJoin(fieldName);
                    ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if(ekcm != null) ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllEnumerateBothRevealers<TMold, TEnumbl, TKRevealBase, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetWhenPopulatedAddAllBothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateBothRevealers<TMold, TEnumbl, TKRevealBase, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : struct, IEnumerable
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllBothRevealersNoNullableStructInvoker<TMold, TEnumbl, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateBothRevealers<TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            callOn.CollectionType ??= actualType;
            var enumeratorType = actualType.GetEnumeratorType()!;
            if (enumeratorType.IsValueType)
            {
                var structEnumtrInvoker = actualType.GetWhenPopulatedAddAllValueRevealerNoNullableStructCallStructEnumtrInvoker
                    <TMold, TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
            foreach (var kvp in value)
            {
                if (ekcm == null)
                {
                    mws.FieldNameJoin(fieldName);
                    ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            if(ekcm != null) ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TMold, TEnumbl, TKey, TVRevealerBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealerBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TVRevealerBase>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TMold, TEnumbl, TKey, TVRevealerBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealerBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : struct, IEnumerable
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker<TMold, TEnumbl, TKey, TVRevealerBase>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TMold, TEnumbl, TKey, TValue, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            callOn.CollectionType ??= actualType;
            var enumeratorType = actualType.GetEnumeratorType()!;
            if (enumeratorType.IsValueType)
            {
                var structEnumtrInvoker = actualType.GetWhenPopulatedAddAllBothRevealersNullableKeyStructCallStructEnumtrInvoker
                    <TMold, TEnumbl, TKey, TValue, TVRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
            foreach (var kvp in value)
            {
                if (ekcm == null)
                {
                    mws.FieldNameJoin(fieldName);
                    ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            if(ekcm != null) ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TMold, TEnumbl, TValue, TKRevealerBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealerBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable?
        where TKRevealerBase : notnull
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetWhenPopulatedAddAllBothRevealersNullableValueStructInvoker<TMold, TEnumbl, TValue, TKRevealerBase>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TMold, TEnumbl, TValue, TKRevealerBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealerBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : struct, IEnumerable
        where TKRevealerBase : notnull
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllBothRevealersNullableValueStructInvoker<TMold, TEnumbl, TValue, TKRevealerBase>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TMold, TEnumbl, TKey, TValue, TKRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            callOn.CollectionType ??= actualType;
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                var structEnumtrInvoker = actualType.GetWhenPopulatedAddAllBothRevealersNullableValueStructCallStructEnumtrInvoker
                    <TMold, TEnumbl, TKey, TValue, TKRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
            foreach (var kvp in value)
            {
                if (ekcm == null)
                {
                    mws.FieldNameJoin(fieldName);
                    ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            if(ekcm != null) ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllEnumerateBothNullRevealers<TMold, TEnumbl, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct
    {
        if (value == null) return callOn.Mold;
        return callOn.WhenPopulatedAddAllEnumerateBothNullRevealers(fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllEnumerateBothNullRevealers<TMold, TEnumbl, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            callOn.CollectionType ??= actualType;
            var enumeratorType = actualType.GetEnumeratorType()!;
            if (enumeratorType.IsValueType)
            {
                var structEnumtrInvoker = actualType.GetWhenPopulatedAddAllBothNullableRevealersCallStructEnumtrInvoker
                    <TMold, TEnumbl, TKey, TValue>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                return structEnumtrInvoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
            foreach (var kvp in value)
            {
                if (ekcm == null)
                {
                    mws.FieldNameJoin(fieldName);
                    ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            }
            if(ekcm != null) ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }
}
