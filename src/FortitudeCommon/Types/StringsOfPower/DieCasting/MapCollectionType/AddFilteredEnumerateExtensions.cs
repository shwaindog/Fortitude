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
    
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>       NoRevealersInvokerCache                 = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> NoRevealersCallStructEnumtrInvokerCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> ValueRevealerInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate> ValueRevealerCallStructEnumtrInvokerCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type, Type), Delegate> BothRevealersCallStructEnumtrInvokerCache
        = new();

    private delegate KeyedCollectionMold NoRevealersInvoker<in TEnumbl, out TKFilterBase, out TVFilterBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?;

    private delegate KeyedCollectionMold ValueRevealerInvoker<in TEnumbl, out TKFilterBase, out TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold BothRevealersInvoker<in TEnumbl, out TKFilterBase, out TVFilterBase, out TKRevealBase
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

    private static NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase> GetAddFilteredNoRevealersCallStructEnumtrInvoker
        <TEnumbl, TKFilterBase, TVFilterBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var invoker =
            (NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>)
            NoRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, tkFilterType, tvFilterType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddFilteredNoRevealersCallStructEnumtr
                                  <TEnumbl, TKFilterBase, TVFilterBase>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase> BuildAddFilteredNoRevealersCallStructEnumtr
        <TEnumbl, TKFilterBase, TVFilterBase>(this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable?
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>));
        return (NoRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase>)methodInvoker;
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


    private static ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>
        GetAddFilteredValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>(Type enumtrType)
        where TEnumbl : IEnumerable?
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var invoker =
            (ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>)
            ValueRevealerInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumtrType, tkFilterType, tvFilterType, tvRevealBase),
                     static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType       = kvpTypes.Value.Key;
                         var valueType     = kvpTypes.Value.Value;
                         var valueItemType = valueType.IfNullableGetUnderlyingTypeOrThis();
                         if (!valueItemType.IsAssignableTo(key.tvRevealType)) 
                             throw new ArgumentException("Expected valueRevealer to be assignable from " + valueItemType.Name);
                              
                         var valueNullable = valueType.IsNullable();
                         
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         MethodInfo toInvokeOn;
                         if (valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = key.tvRevealType;
                             genericParamTypes[3] = key.tkFilterType;
                             toInvokeOn = GetStaticMethodInfo(nameof(AddFilteredEnumerateNullValueRevealer)
                                                            , genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }
                         else
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = valueType;
                             genericParamTypes[3] = key.tkFilterType;
                             genericParamTypes[4] = key.tvFilterType;
                             genericParamTypes[5] = key.tvRevealType;
                             toInvokeOn = GetStaticMethodInfo(nameof(AddFilteredEnumerateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }

                         methodParamTypes[1] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddFilteredValueRevealerNoNullableStructInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, true);
        return invoker;
    }

    private static ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>
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
            = helperMethod.CreateDelegate(typeof(ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>));
        var createInvoker = (ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    private static ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>
        GetAddFilteredValueRevealerCallStructEnumtrInvoker
        <TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var tvRevealType    = typeof(TVRevealBase);
        var invoker =
            (ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>)
            ValueRevealerCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, tkFilterType, tvFilterType, tvRevealType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type tkFilterType, Type tVFilterType, Type tvRevealType) key
                            , bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = key.enumeratorType.GetAddFilteredValueRevealerInvokerMethodInfo<TKFilterBase, TVFilterBase, TVRevealBase>();
                              return key.enumblType.BuildAddFilteredValueRevealerCallStructEnumtr
                                  <TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>
        BuildAddFilteredValueRevealerCallStructEnumtr<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>
        (this Type enumblType, MethodInfo callEnumtrInvokeMethInf, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase?>);
        methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>));
        return (ValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        GetAddFilteredBothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
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
            (BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tkFilterType, tvFilterType, tKRevealBase, tvRevealBase)
                   , static ((Type enumblParamType, Type enumblType, Type tkFilterType, Type tvFilterType, Type tkRevealType, Type tvRevealType) key
                       , bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType       = kvpTypes.Value.Key;
                         var valueType     = kvpTypes.Value.Value;
                         var keyNullable   = keyType.IsNullable();
                         var valueNullable = valueType.IsNullable();
                         var keyItemType   = keyType.IfNullableGetUnderlyingTypeOrThis();
                         var valueItemType = valueType.IfNullableGetUnderlyingTypeOrThis();
                         if(!keyItemType.IsAssignableTo(key.tkRevealType)) 
                             throw new ArgumentException($"Expected keyRevealer- {key.tkRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                       + keyItemType.ShortNameInCSharpFormat());
                         if(!valueItemType.IsAssignableTo(key.tvRevealType)) 
                             throw new ArgumentException($"Expected valueRevealer- {key.tvRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                       + valueItemType.ShortNameInCSharpFormat());


                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[4] = typeof(PalantírReveal<TKRevealerBase>);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);
                         
                         MethodInfo toInvokeOn;
                         if (keyNullable && valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = key.tkRevealType;
                             genericParamTypes[2] = key.tvRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddFilteredEnumerateBothNullRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         } else if (keyNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = key.tkRevealType;
                             genericParamTypes[2] = valueType;
                             genericParamTypes[3] = key.tvFilterType;
                             genericParamTypes[4] = key.tvFilterType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddFilteredEnumerateBothWithNullKeyRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         } else if (valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = key.tvRevealType;
                             genericParamTypes[3] = key.tkFilterType;
                             genericParamTypes[4] = key.tkRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddFilteredEnumerateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }
                         else
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = valueType;
                             genericParamTypes[3] = key.tkFilterType;
                             genericParamTypes[4] = key.tvFilterType;
                             genericParamTypes[5] = key.tkRevealType;
                             genericParamTypes[6] = key.tvRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddFilteredEnumerateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }

                         methodParamTypes[1] = key.enumblParamType;

                         var fullGenericInvoke =
                             BuildAddFilteredBothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        BuildAddFilteredBothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
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
                (typeof(BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>));
        var createInvoker =
            (BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
        GetAddFilteredBothRevealersCallStructEnumtrInvoker
        <TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var tkRevealerType  = typeof(TKRevealBase);
        var tvRevealerType  = typeof(TVRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>)
            BothRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, tkFilterType, tvFilterType, tkRevealerType, tvRevealerType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type tkFilterType, Type tvFilterType, Type tkRevealerType, Type tvRevealerType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = 
                                  key.enumeratorType.GetAddFilteredBothRevealersInvokerMethodInfo
                                      <TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>();
                              return key.enumblType.BuildAddFilteredBothRevealersCallStructEnumtr
                                  <TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                                  (callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
        BuildAddFilteredBothRevealersCallStructEnumtr<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
        (this Type enumblType, MethodInfo callEnumtrInvokeMethInf, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
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
            = helperMethod.CreateDelegate(typeof(BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>));
        return (BothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>)methodInvoker;
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
                        .GetAddFilteredNoRevealersCallStructEnumtrInvoker<TEnumbl, TKFilterBase, TVFilterBase>(enumeratorType);
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
        var invoker = GetAddFilteredValueRevealerInvoker<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(actualType);
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
                    .GetAddFilteredValueRevealerCallStructEnumtrInvoker
                        <TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(enumeratorType);
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
        var invoker = GetAddFilteredValueRevealerInvoker<TEnumbl, TKFilterBase, TValue?, TValue>(actualType);
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
                    .GetAddFilteredValueRevealerCallStructEnumtrInvoker<TEnumbl, TKFilterBase, TValue?, TValue>(enumeratorType);
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
        var invoker = GetAddFilteredBothRevealersInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(actualType);
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
                    .GetAddFilteredBothRevealersCallStructEnumtrInvoker<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                        (enumeratorType);
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
        var invoker = GetAddFilteredBothRevealersInvoker<TEnumbl, TKey?, TVFilterBase, TKey, TVRevealBase>(actualType);
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
                    .GetAddFilteredBothRevealersCallStructEnumtrInvoker<TEnumbl, TKey?, TVFilterBase, TKey, TVRevealBase>(enumeratorType);
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
        var invoker = GetAddFilteredBothRevealersInvoker<TEnumbl, TKFilterBase, TValue?, TKRevealBase, TValue>(actualType);
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
                    .GetAddFilteredBothRevealersCallStructEnumtrInvoker<TEnumbl, TKFilterBase, TValue?, TKRevealBase, TValue>
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
                    .GetAddFilteredBothRevealersCallStructEnumtrInvoker<TEnumbl, TKey?, TValue?, TKey, TValue>
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
