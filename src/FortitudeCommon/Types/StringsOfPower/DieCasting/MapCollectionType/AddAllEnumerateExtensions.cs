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

    private static readonly ConcurrentDictionary<(Type, Type), Delegate>       NoRevealersInvokerCache                 = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> NoRevealersCallStructEnumtrInvokerCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerInvokerCache                 = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerCallStructEnumtrInvokerCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersCallStructEnumtrInvokerCache = new();

    private delegate KeyedCollectionMold NoRevealersInvoker<in TEnumbl>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?;

    private delegate KeyedCollectionMold ValueRevealerInvoker<in TEnumbl, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold BothRevealersInvoker<in TEnumbl, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    private static NoRevealersInvoker<TEnumbl> GetAddAllNoRevealersCallStructEnumtrInvoker<TEnumbl>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TEnumbl>)
            NoRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = key.enumeratorType.GetAddAllNoRevealersInvokerMethodInfo();
                              return key.enumblType.BuildAddAllNoRevealersCallStructEnumtr<TEnumbl>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl> BuildAddAllNoRevealersCallStructEnumtr<TEnumbl>
        (this Type enumblType, MethodInfo  callEnumtrInvokeMethInf, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

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
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumbl>));
        return (NoRevealersInvoker<TEnumbl>)methodInvoker;
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

                    methodParamTypes[1] = key.enumblParamType;
                    var fullGenericInvoke =
                        BuildAddAllNoRevealersInvoker<TEnumbl>(toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                    return fullGenericInvoke;
                }, callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TEnumbl> BuildAddAllNoRevealersInvoker<TEnumbl>(MethodInfo methodInfo, Type enumblParamType
      , Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumbl>));
        var createInvoker = (NoRevealersInvoker<TEnumbl>)methodInvoker;

        return createInvoker;
    }

    private static ValueRevealerInvoker<TEnumbl, TVRevealerBase> GetAddAllValueRevealerInvoker
        <TEnumbl, TVRevealerBase>(Type enumblType)
        where TEnumbl : IEnumerable?
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var tvRevealBase    = typeof(TVRevealerBase);
        var invoker =
            (ValueRevealerInvoker<TEnumbl, TVRevealerBase>)
            ValueRevealerInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tvRevealBase)
                   , static ((Type enumblParamType, Type enumblType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;
                         var valueItemType = valueType.IfNullableGetUnderlyingTypeOrThis();
                         if(!valueItemType.IsAssignableTo(key.tvRevealType)) 
                             throw new ArgumentException($"Expected valueRevealer- {key.tvRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                       + valueItemType.ShortNameInCSharpFormat());
                              
                         var valueNullable = valueType.IsNullable();

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         MethodInfo toInvokeOn;
                         if (valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = key.tvRevealType;
                             toInvokeOn = GetStaticMethodInfo(nameof(AddAllEnumerateNullValueRevealer)
                                                            , genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }
                         else
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = valueType;
                             genericParamTypes[3] = key.tvRevealType;
                             toInvokeOn = GetStaticMethodInfo(nameof(AddAllEnumerateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }

                         methodParamTypes[1] = key.enumblParamType;
                         var fullGenericInvoke = BuildAddAllValueRevealerInvoker<TEnumbl, TVRevealerBase> 
                             (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, true);
        return invoker;
    }

    private static ValueRevealerInvoker<TEnumbl, TVRevealerBase> BuildAddAllValueRevealerInvoker
        <TEnumbl, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerInvoker<TEnumbl, TVRevealerBase>));
        var createInvoker = (ValueRevealerInvoker<TEnumbl, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    private static ValueRevealerInvoker<TEnumbl, TVRevealerBase>
        GetAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker
        <TEnumbl, TVRevealerBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable?
        where TVRevealerBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerInvoker<TEnumbl, TVRevealerBase>)
            ValueRevealerCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = key.enumeratorType.GetAddAllValueRevealerInvokerMethodInfo<TVRevealerBase>();
                              return key.enumblType.BuildAddAllValueRevealerCallStructEnumtr
                                  <TEnumbl, TVRevealerBase>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerInvoker<TEnumbl, TVRevealBase>
        GetAddAllValueRevealerNullableValueStructCallStructEnumtrInvoker
        <TEnumbl, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerInvoker<TEnumbl, TVRevealBase>)
            ValueRevealerCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = key.enumeratorType.GetAddAllValueRevealerInvokerMethodInfo<TVRevealBase>();
                              return key.enumblType.BuildAddAllValueRevealerCallStructEnumtr
                                  <TEnumbl, TVRevealBase>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerInvoker<TEnumbl, TVRevealBase>
        BuildAddAllValueRevealerCallStructEnumtr<TEnumbl, TVRevealBase>
        (this Type enumblType, MethodInfo callEnumtrInvokeMethInf, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull
        
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TVRevealBase>);
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerInvoker<TEnumbl, TVRevealBase>));
        return (ValueRevealerInvoker<TEnumbl, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKRevealerBase, TVRevealerBase> GetAddAllBothRevealersInvoker
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
            (BothRevealersInvoker<TEnumbl, TKRevealerBase, TVRevealerBase>)
            BothRevealersInvokerCache
                .GetOrAdd
                    ((enumblParamType, enumblType, tKRevealBase, tvRevealBase),
                     static ((Type enumblParamType, Type enumblType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumblType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType       = kvpTypes.Value.Key;
                         var valueType     = kvpTypes.Value.Value;
                         var keyNullable   = keyType.IsNullable();
                         var valueNullable = valueType.IsNullable();
                         var keyItemType   = keyType.IfNullableGetUnderlyingTypeOrThis();
                         var valueItemType   = valueType.IfNullableGetUnderlyingTypeOrThis();
                         if(!keyItemType.IsAssignableTo(key.tkRevealType)) 
                             throw new ArgumentException($"Expected keyRevealer- {key.tkRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                       + keyItemType.ShortNameInCSharpFormat());
                         if(!valueItemType.IsAssignableTo(key.tvRevealType)) 
                             throw new ArgumentException($"Expected valueRevealer- {key.tvRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                       + valueItemType.ShortNameInCSharpFormat());
                         
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumblType;
                         methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         MethodInfo toInvokeOn;
                         if (keyNullable && valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = key.tkRevealType;
                             genericParamTypes[2] = key.tvRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddAllEnumerateBothNullRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         } else if (keyNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = key.tkRevealType;
                             genericParamTypes[2] = valueType;
                             genericParamTypes[3] = key.tvRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddAllEnumerateBothWithNullKeyRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         } else if (valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = key.tvRevealType;
                             genericParamTypes[3] = key.tkRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddAllEnumerateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }
                         else
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                             genericParamTypes[0] = key.enumblType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = valueType;
                             genericParamTypes[3] = key.tkRevealType;
                             genericParamTypes[4] = key.tvRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddAllEnumerateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }
                         
                         methodParamTypes[1] = key.enumblParamType;
                         
                         var fullGenericInvoke =
                             BuildAddAllBothRevealersInvoker<TEnumbl, TKRevealerBase, TVRevealerBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKRevealerBase, TVRevealerBase> BuildAddAllBothRevealersInvoker
        <TEnumbl, TKRevealerBase, TVRevealerBase>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
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
                (typeof(BothRevealersInvoker<TEnumbl, TKRevealerBase, TVRevealerBase>));
        var createInvoker = (BothRevealersInvoker<TEnumbl, TKRevealerBase, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKRevealBase, TVRevealBase>
        GetAddAllValueRevealerNoNullableStructCallStructEnumtrInvoker
        <TEnumbl, TKRevealBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var keyRevealType   = typeof(TKRevealBase);
        var valueRevealType = typeof(TVRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersInvoker<TEnumbl, TKRevealBase, TVRevealBase>)
            BothRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, valueRevealType, keyRevealType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type _, Type _1) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = 
                                  key.enumeratorType.GetAddAllBothRevealersInvokerMethodInfo<TKRevealBase, TVRevealBase>();
                              return key.enumblType.BuildAddAllBothRevealersCallStructEnumtr
                                  <TEnumbl, TKRevealBase, TVRevealBase>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKey, TVRevealBase>
        GetAddAllBothRevealersNullableKeyStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable?
        where TKey : notnull
        where TVRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var tKeyType        = typeof(TKey);
        var tvRevealerType  = typeof(TVRevealBase);
        var invoker =
            (BothRevealersInvoker<TEnumbl, TKey, TVRevealBase>)
            BothRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, tvRevealerType, tKeyType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type tvRevealerType, Type tkRevealerType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = key.enumeratorType.GetAddAllBothRevealersInvokerMethodInfo<TKey, TVRevealBase>();
                              return key.enumblType.BuildAddAllBothRevealersCallStructEnumtr
                                  <TEnumbl, TKey, TVRevealBase>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKey, TValue> GetAddAllBothNullableRevealersCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersInvoker<TEnumbl, TKey, TValue>)
            BothRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, valueType, keyType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type valueType, Type keyType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = key.enumeratorType.GetAddAllBothRevealersInvokerMethodInfo<TKey, TValue>();
                              return key.enumblType.BuildAddAllBothRevealersCallStructEnumtr
                                  <TEnumbl, TKey, TValue>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKRevealBase, TValue>
        GetAddAllBothRevealersNullableValueStructCallStructEnumtrInvoker
        <TEnumbl, TKey, TValue, TKRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var keyType         = typeof(TKRevealBase);
        var tvRevealerType  = typeof(TKRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersInvoker<TEnumbl, TKRevealBase, TValue>)
            BothRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, tvRevealerType, keyType)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type _, Type _1) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = key.enumeratorType.GetAddAllBothRevealersInvokerMethodInfo<TKRevealBase, TValue>();
                              return key.enumblType.BuildAddAllBothRevealersCallStructEnumtr
                                  <TEnumbl, TKRevealBase, TValue>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumeratorType);
                          }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TEnumbl, TKey, TVRevealBase>
        BuildAddAllBothRevealersCallStructEnumtr<TEnumbl, TKey, TVRevealBase>
        (this Type enumblType, MethodInfo callEnumtrInvokeMethInf, Type enumblParamType, Type enumeratorType)
        where TEnumbl : IEnumerable?
        where TKey : notnull
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

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
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersInvoker<TEnumbl, TKey, TVRevealBase>));
        return (BothRevealersInvoker<TEnumbl, TKey, TVRevealBase>)methodInvoker;
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
        where TEnumbl : struct, IEnumerable =>
        value == null ? callOn : callOn.AddAllEnumerate(value.Value, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddAllEnumerate<TEnumbl>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllNoRevealersInvoker<TEnumbl>(actualType);
        return invoker(callOn, value, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerate<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>> =>
        value == null ? callOn : callOn.AddAllEnumerate<TEnumbl, TKey, TValue>(value.Value, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddAllEnumerate<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker = actualType.GetAddAllNoRevealersCallStructEnumtrInvoker<TEnumbl>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, valueFormatString, keyFormatString, formatFlags);
        }

        valueFormatString ??= "";
        keyFormatString   ??= "";
        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var kvp in value)
        {
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TVRevealBase : notnull =>
        value == null ? callOn : callOn.AddAllEnumerateValueRevealer(value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllValueRevealerInvoker<TEnumbl, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
                (value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker
                = actualType.GetAddAllValueRevealerNoNullableStructICallStructEnumtrInvoker<TEnumbl, TVRevealBase>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        keyFormatString ??= "";
        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var kvp in value)
        {
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
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
        where TEnumbl : struct, IEnumerable
        where TValue : struct =>
        value == null ? callOn : callOn.AddAllEnumerateNullValueRevealer(value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllValueRevealerInvoker<TEnumbl, TValue>(actualType);
        return invoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
                (value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker = actualType.GetAddAllValueRevealerNullableValueStructCallStructEnumtrInvoker
                <TEnumbl, TValue>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        keyFormatString ??= "";
        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var kvp in value)
        {
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
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
        where TEnumbl : struct, IEnumerable
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null ? callOn : callOn.AddAllEnumerateBothRevealers(value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllBothRevealersInvoker<TEnumbl, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker = actualType.GetAddAllValueRevealerNoNullableStructCallStructEnumtrInvoker<TEnumbl, TKRevealBase, TVRevealBase>
                (enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
        }
        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var kvp in value)
        {
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
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
        where TEnumbl : struct, IEnumerable
        where TKey : struct
        where TVRevealerBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllEnumerateBothWithNullKeyRevealers(value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllBothRevealersInvoker<TEnumbl, TKey, TVRevealerBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var enumeratorType = actualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker = actualType.GetAddAllBothRevealersNullableKeyStructCallStructEnumtrInvoker
                <TEnumbl, TKey, TVRevealBase>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
        }
        var kvpType = typeof(KeyValuePair<TKey?, TValue>);
        foreach (var kvp in value)
        {
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
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
        where TEnumbl : struct, IEnumerable
        where TKRevealerBase : notnull
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddAllEnumerateBothWithNullValueRevealers
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllBothRevealersInvoker<TEnumbl, TKRevealerBase, TValue>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

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
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
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
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
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
        where TValue : struct =>
        value == null ? callOn : callOn.AddAllEnumerateBothNullRevealers(value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

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
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }
}
