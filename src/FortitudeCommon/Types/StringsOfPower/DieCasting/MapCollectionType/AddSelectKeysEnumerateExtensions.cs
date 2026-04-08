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

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public static class AddSelectKeysEnumerateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> NoRevealersCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> NoRevealersInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate> ValueRevealerCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> ValueRevealerInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate> BothRevealersInvokerCache = new();
    
    private delegate KeyedCollectionMold NoRevealersInvoker<TKey, TValue, in TKSelectEnumbl>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?;

    private delegate KeyedCollectionMold ValueRevealerInvoker<TKey, TValue, in TKSelectEnumbl, out TVRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold BothRevealersInvoker<TKey, TValue, in TKSelectEnumbl, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;


    private static NoRevealersInvoker<TKey, TValue, TKSelectEnumbl> GetAddWithSelectKeysNoRevealersInvoker
        <TKey, TValue, TKSelectEnumbl>(Type enumblType)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TKey, TValue, TKSelectEnumbl>)
            NoRevealersInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumblParamType, enumblType)
                   , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType) key, bool _) =>
                     {
                         var selectKeyDerivedType
                             = key.enumblType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);

                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType))
                             throw new ArgumentException("Expected to selectKeys element to be equatable to Key");

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.keyType;
                         genericParamTypes[1] = key.valueType;
                         genericParamTypes[2] = key.enumblType;
                         genericParamTypes[3] = selectKeyDerivedType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                         methodParamTypes[2] = key.enumblType;
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerate), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[2] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddWithSelectKeysNoRevealersInvoker<TKey, TValue, TKSelectEnumbl>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TKey, TValue, TKSelectEnumbl> BuildAddWithSelectKeysNoRevealersInvoker
        <TKey, TValue, TKSelectEnumbl>(
            MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(AddSelectKeysEnumerateExtensions).Module, false);
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

        // call AddWithSelectKeysEnumerate(KeyedCollectionMold, TEnumbl, valueFmtStr, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TKey, TValue, TKSelectEnumbl>));
        var createInvoker = (NoRevealersInvoker<TKey, TValue, TKSelectEnumbl>)methodInvoker;

        return createInvoker;
    }

    private static ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        GetAddWithSelectKeysValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>(Type enumblType)
        where TKSelectEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var tvRevealBase    = typeof(TVRevealBase);
        var invoker =
            (ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>)
            ValueRevealerInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumblParamType, enumblType, tvRevealBase),
                     static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type tvRevealType) key, bool _) =>
                     {
                         var selectKeyDerivedType = key.enumblType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);

                         var keyItemType   = key.keyType.IfNullableGetUnderlyingTypeOrThis();
                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType))
                             throw new ArgumentException($"Expected selectKeys- {selectKeyDerivedType.ShortNameInCSharpFormat()} to be assignable to " 
                                                       + keyItemType.ShortNameInCSharpFormat());
                         var valueItemType = key.valueType.IfNullableGetUnderlyingTypeOrThis();
                         if(!valueItemType.IsAssignableTo(key.tvRevealType)) 
                             throw new ArgumentException($"Expected valueRevealer- {key.tvRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                       + valueItemType.ShortNameInCSharpFormat());
                              
                         var valueNullable = key.valueType.IsNullable();
                         
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                         methodParamTypes[2] = key.enumblType;
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);
                         
                         MethodInfo toInvokeOn;
                         if (valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                             genericParamTypes[0] = key.keyType;
                             genericParamTypes[1] = valueItemType;
                             genericParamTypes[2] = key.enumblType;
                             genericParamTypes[3] = selectKeyDerivedType;
                             toInvokeOn = GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerateNullValueRevealer)
                                                            , genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }
                         else
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                             genericParamTypes[0] = key.keyType;
                             genericParamTypes[1] = key.valueType;
                             genericParamTypes[2] = key.enumblType;
                             genericParamTypes[3] = selectKeyDerivedType;
                             genericParamTypes[4] = key.tvRevealType;
                             toInvokeOn = GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }

                         methodParamTypes[2] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddWithSelectKeysValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, true);
        return invoker;
    }

    private static ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        BuildAddWithSelectKeysValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TKSelectEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddWithSelectKeysIterateValueRevealerInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(AddSelectKeysEnumerateExtensions).Module, false);
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

        // call AddWithSelectKeysEnumerateNullValueRevealer(KeyedCollectionMold, TEnumbl, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
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
            helperMethod.CreateDelegate(typeof(ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>));
        var createInvoker = (ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>)methodInvoker;

        return createInvoker;
    }

    private static BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        GetAddWithSelectKeysBothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(Type enumblType)
        where TKSelectEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var tKRevealBase    = typeof(TKRevealBase);
        var tvRevealBase    = typeof(TVRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>)
            BothRevealersInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumblParamType, enumblType, tKRevealBase, tvRevealBase),
                     static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type tkRevealType, Type tvRevealType) key
                       , bool _) =>
                     {
                         var selectKeyDerivedType = key.enumblType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);

                         var keyItemType   = key.keyType.IfNullableGetUnderlyingTypeOrThis();
                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType))
                             throw new ArgumentException($"Expected selectKeys- {selectKeyDerivedType.ShortNameInCSharpFormat()} to be assignable to " 
                                                       + keyItemType.ShortNameInCSharpFormat());
                         var valueItemType = key.valueType.IfNullableGetUnderlyingTypeOrThis();
                         var valueNullable = key.valueType.IsNullable();
                         if(!keyItemType.IsAssignableTo(key.tkRevealType)) 
                             throw new ArgumentException($"Expected keyRevealer- {key.tkRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                       + keyItemType.ShortNameInCSharpFormat());
                         if(!valueItemType.IsAssignableTo(key.tvRevealType)) 
                             throw new ArgumentException($"Expected valueRevealer- {key.tvRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                       + valueItemType.ShortNameInCSharpFormat());


                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                         methodParamTypes[2] = key.enumblType;
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
                         methodParamTypes[4] = typeof(PalantírReveal<TKRevealBase>);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);
                         
                         MethodInfo toInvokeOn;
                         if (valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                             genericParamTypes[0] = key.tkRevealType;
                             genericParamTypes[1] = valueItemType;
                             genericParamTypes[2] = key.enumblType;
                             genericParamTypes[3] = selectKeyDerivedType;
                             genericParamTypes[4] = key.tkRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         } 
                         else
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                             genericParamTypes[0] = key.keyType;
                             genericParamTypes[1] = key.valueType;
                             genericParamTypes[2] = key.enumblType;
                             genericParamTypes[3] = selectKeyDerivedType;
                             genericParamTypes[4] = key.tkRevealType;
                             genericParamTypes[5] = key.tvRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }

                         methodParamTypes[2] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddWithSelectKeysBothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        BuildAddWithSelectKeysBothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TKSelectEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddWithSelectKeysIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(AddSelectKeysEnumerateExtensions).Module, false);
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

        // call AddWithSelectKeysEnumerateBothRevealers(KeyedCollectionMold, TEnumbl, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
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
                (typeof(BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>));
        var createInvoker
            = (BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>)methodInvoker;

        return createInvoker;
    }
    
    private static NoRevealersInvoker<TKey, TValue, TKSelectEnumbl> GetAddWithSelectKeysNoRevealersCallStructEnumtrInvoker
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived>(this Type enumblType, Type enumtrType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TKey, TValue, TKSelectEnumbl>)
            NoRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((keyType, valueType, enumblParamType, enumblType, enumtrType)
                        , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type enumtrType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf  = key.enumtrType.GetAddWithSelectKeysNoRevealersInvokerMethodInfo<TKey, TValue>();
                              return key.enumblType.BuildAddWithSelectKeysNoRevealersCallStructEnumtr
                                  <TKey, TValue, TKSelectEnumbl>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumtrType);
                          }, callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TKey, TValue, TKSelectEnumbl> BuildAddWithSelectKeysNoRevealersCallStructEnumtr
        <TKey, TValue, TKSelectEnumbl>(this Type enumblType, MethodInfo callEnumtrInvokeMethInf, Type enumblParamType, Type enumeratorType)
        where TKSelectEnumbl : IEnumerable?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[2].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
        methodParamTypes[2] = enumblParamType;
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray
               , typeof(AddSelectKeysEnumerateExtensions).Module, false);
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

        // call AddWithSelectKeysIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TKey, TValue, TKSelectEnumbl>));
        return (NoRevealersInvoker<TKey, TValue, TKSelectEnumbl>)methodInvoker;
    }

    private static ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        GetAddWithSelectKeysValueRevealerCallStructEnumtrInvoker
        <TKey, TValue, TKSelectEnumbl, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TKSelectEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var tvRevealType    = typeof(TVRevealBase);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>)
            ValueRevealerCallStructEnumtrInvokerCache
                .GetOrAdd((keyType, valueType, enumblParamType, enumblType, enumtrType, tvRevealType)
                        , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type enumtrType, Type tvRevealType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf =
                                  key.enumtrType.GetAddWithSelectKeysValueRevealerInvokerMethodInfo<TKey, TValue, TVRevealBase>();
                              return key.enumblType.BuildAddWithSelectKeysValueRevealerCallStructEnumtrInvoker
                                  <TKey, TValue, TKSelectEnumbl, TVRevealBase>(callEnumtrInvokeMethInf, key.enumblParamType, key.enumtrType);
                          }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        BuildAddWithSelectKeysValueRevealerCallStructEnumtrInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        (this Type enumblType, MethodInfo callEnumtrInvokeMethInf, Type enumblParamType, Type enumeratorType)
        where TKSelectEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
        methodParamTypes[2] = enumblParamType;
        methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(string);
        methodParamTypes[6] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicValueRevealerNoNullableStructInvoke_{enumeratorType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(AddSelectKeysEnumerateExtensions).Module, false);
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

        // call AddWithSelectKeysIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate(typeof(ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>));
        return (ValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        GetAddWithSelectKeysBothRevealersCallStructEnumtrInvoker
        <TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TKSelectEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>)
            BothRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((keyType, valueType, enumblParamType, enumblType, enumtrType)
                        , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type enumtrType) key, bool _) =>
                          {
                              var callEnumtrInvokeMethInf =
                                  key.enumtrType.GetAddWithSelectKeysBothRevealersInvokerMethodInfo<TKey, TValue, TKRevealBase, TVRevealBase>();
                              return key.enumblType.BuildAddWithSelectKeysEnumerateBothRevealersCallStructEnumtr
                                  <TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
                                  (callEnumtrInvokeMethInf, key.enumblParamType, key.enumtrType);
                          }, callAsFactory);
        return invoker;
    }

    private static BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        BuildAddWithSelectKeysEnumerateBothRevealersCallStructEnumtr<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        (this Type enumblType, MethodInfo callEnumtrInvokeMethInf, Type enumblParamType, Type enumeratorType)
        where TKSelectEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
        methodParamTypes[2] = enumblParamType;
        methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
        methodParamTypes[4] = typeof(PalantírReveal<TKRevealBase>);
        methodParamTypes[5] = typeof(string);
        methodParamTypes[6] = typeof(FormatFlags);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes.AsArray, typeof(AddSelectKeysEnumerateExtensions).Module, false);
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

        // call AddWithSelectKeysIterateBothRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod
                .CreateDelegate
                    (typeof(BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>));
        return (BothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>)methodInvoker;
    }

    private static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(AddSelectKeysEnumerateExtensions).GetMethods(NonPublic | Public | Static);

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

    public static KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerate(value, selectKeys.Value, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();
        var invoker              = GetAddWithSelectKeysNoRevealersInvoker<TKey, TValue, TKSelectEnumbl>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull
        where TKSelectDerived : TKey =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
                (value, selectKeys.Value, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();
        var enumeratorType       = selectKeysActualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysNoRevealersCallStructEnumtrInvoker
                <TKey, TValue, TKSelectEnumbl, TKSelectDerived>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, selectKeys, valueFormatString, keyFormatString, formatFlags);
        }
        valueFormatString ??= "";
        keyFormatString   ??= "";
        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
        }
        return mws.Mold;
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerateValueRevealer
                (value, selectKeys.Value, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();

        var invoker = GetAddWithSelectKeysValueRevealerInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TVRevealBase : notnull =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>
                (value, selectKeys.Value, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TVRevealBase : notnull
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();
        var enumeratorType       = selectKeysActualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysValueRevealerCallStructEnumtrInvoker
                <TKey, TValue, TKSelectEnumbl, TVRevealBase>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        keyFormatString ??= "";
        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(keyValue, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
        }
        return mws.Mold;
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull
        where TValue : struct =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerateNullValueRevealer
                (value, selectKeys.Value, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
        where TValue : struct
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);

        var selectKeysActualType = selectKeys.GetType();
        var invoker              = GetAddWithSelectKeysValueRevealerInvoker<TKey, TValue?, TKSelectEnumbl, TValue>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull
        where TKSelectDerived : TKey
        where TValue : struct =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
                (value, selectKeys.Value, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();
        var enumeratorType       = selectKeysActualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysValueRevealerCallStructEnumtrInvoker
                <TKey, TValue?, TKSelectEnumbl, TValue>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        keyFormatString ??= "";
        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(keyValue, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
        }
        return mws.Mold;
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerateBothRevealers(value, selectKeys.Value, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null || selectKeys == null) return callOn;

        var valueActualType = value.GetType();
        var mws             = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(valueActualType, "", formatFlags)) return mws.WasSkipped(valueActualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();
        var invoker
            = GetAddWithSelectKeysBothRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase,
                TVRevealBase>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerateBothRevealers
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(
            this KeyedCollectionMold callOn
          , IReadOnlyDictionary<TKey, TValue>? value
          , TKSelectEnumbl? selectKeys
          , PalantírReveal<TVRevealBase> valueStyler
          , PalantírReveal<TKRevealBase> keyStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
                (value, selectKeys.Value, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerateBothRevealers
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(
            this KeyedCollectionMold callOn
          , IReadOnlyDictionary<TKey, TValue>? value
          , TKSelectEnumbl? selectKeys
          , PalantírReveal<TVRevealBase> valueRevealer
          , PalantírReveal<TKRevealBase> keyRevealer
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();
        var enumeratorType       = selectKeysActualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var        structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysBothRevealersCallStructEnumtrInvoker
                <TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealCloakedBearerOrNull(key, keyRevealer, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(keyValue, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
        }
        return mws.Mold;
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerateBothWithNullValueRevealers
                (value, selectKeys.Value, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();
        var invoker = GetAddWithSelectKeysBothRevealersInvoker<TKey, TValue?, TKSelectEnumbl, TKRevealBase, TValue>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysEnumerateBothWithNullValueRevealers
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(
            this KeyedCollectionMold callOn
          , IReadOnlyDictionary<TKey, TValue?>? value
          , TKSelectEnumbl? selectKeys
          , PalantírReveal<TValue> valueStyler
          , PalantírReveal<TKRevealBase> keyStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull =>
        selectKeys == null || value == null
            ? callOn
            : callOn.AddWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
                (value, selectKeys.Value, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddWithSelectKeysEnumerateBothWithNullValueRevealers<
        TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        if (value == null || selectKeys == null) return callOn;

        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var selectKeysActualType = selectKeys.GetType();
        var enumeratorType       = selectKeysActualType.GetEnumeratorType();
        if (enumeratorType?.IsValueType ?? false)
        {
            var structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysBothRevealersCallStructEnumtrInvoker
                <TKey, TValue?, TKSelectEnumbl, TKRevealBase, TValue>(enumeratorType);
            // ReSharper disable once GenericEnumeratorNotDisposed
            return structEnumtrInvoker(callOn, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }

        var kvpType = typeof(KeyValuePair<TKey, TValue>);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
            mws.RevealCloakedBearerOrNull(key, keyRevealer, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(keyValue, valueRevealer, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
        }
        return mws.Mold;
    }
}
