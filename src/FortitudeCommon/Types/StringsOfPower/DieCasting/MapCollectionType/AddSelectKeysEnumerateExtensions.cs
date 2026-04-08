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

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate>
        ValueRevealerNoNullableStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate>
        ValueRevealerNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate>
        ValueRevealerNullableValueStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>
        ValueRevealerNullableValueStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate>
        BothRevealersNoNullableStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate>
        BothRevealersNullableValueStructCallStructEnumtrInvokerCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructInvokerCache = new();

    private delegate KeyedCollectionMold NoRevealersInvoker<TKey, TValue, in TKSelectEnumbl, TKSelectDerived>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey;

    private delegate KeyedCollectionMold NoRevealersInvoker<TKey, TValue, in TKSelectEnumbl>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull;

    private delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<TKey, TValue, in TKSelectEnumbl, TKSelectDerived, out TVRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<TKey, TValue, in TKSelectEnumbl, out TVRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
        where TValue : struct;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
        where TValue : struct;

    private delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker
        <TKey, TValue, in TKSelectEnumbl, TKSelectDerived, out TKRevealBase, out TVRevealBase>(
            KeyedCollectionMold callOn
          , IReadOnlyDictionary<TKey, TValue>? value
          , TKSelectEnumbl? selectKeys
          , PalantírReveal<TVRevealBase> valueStyler
          , PalantírReveal<TKRevealBase> keyStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    private delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<TKey, TValue, in TKSelectEnumbl, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
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
        where TVRevealBase : notnull;


    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, out TKRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    private delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, out TKRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull;

    private static NoRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived> GetAddWithSelectKeysNoRevealersCallStructEnumtrInvoker
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived>(this Type enumblType, Type enumtrType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>)
            NoRevealersCallStructEnumtrInvokerCache
                .GetOrAdd((keyType, valueType, enumblParamType, enumblType, enumtrType)
                        , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddWithSelectKeysNoRevealersCallStructEnumtr
                                  <TKey, TValue, TKSelectEnumbl, TKSelectDerived>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static NoRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived> BuildAddWithSelectKeysNoRevealersCallStructEnumtr
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived>(this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddWithSelectKeysNoRevealersInvokerMethodInfo<TKey, TValue>();
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>));
        return (NoRevealersInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>)methodInvoker;
    }

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

    private static ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>
        GetAddWithSelectKeysValueRevealerNoNullableStructICallStructEnumtrInvoker
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TVRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>)
            ValueRevealerNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((keyType, valueType, enumblParamType, enumblType, enumtrType)
                        , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddWithSelectKeysValueRevealerNoNullableStructICallStructEnumtrInvoker
                                  <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(key.enumblParamType, key.enumeratorType)
                        , callAsFactory);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>
        BuildAddWithSelectKeysValueRevealerNoNullableStructICallStructEnumtrInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf =
            enumeratorType.GetAddWithSelectKeysValueRevealerNoNullableStructIInvokerMethodInfo<TKey, TValue, TVRevealBase>();
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
            helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>));
        return (ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>)methodInvoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        GetAddWithSelectKeysValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>(Type enumblType)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var tvRevealBase    = typeof(TVRevealBase);
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumblParamType, enumblType, tvRevealBase),
                     static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type tvRevealType) key, bool _) =>
                     {
                         var selectKeyDerivedType = key.enumblType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);

                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType))
                             throw new ArgumentException("Expected to selectKeys element to be equatable to Key");

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.keyType;
                         genericParamTypes[1] = key.valueType;
                         genericParamTypes[2] = key.enumblType;
                         genericParamTypes[3] = selectKeyDerivedType;
                         genericParamTypes[4] = key.tvRevealType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                         methodParamTypes[2] = key.enumblType;
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerateValueRevealer), genericParamTypes.AsArray
                                               , methodParamTypes.AsArray);


                         methodParamTypes[2] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddWithSelectKeysValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, true);
        return invoker;
    }

    private static ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        BuildAddWithSelectKeysValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
        where TValue : TVRevealBase?
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
            helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>)methodInvoker;

        return createInvoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
        GetAddWithSelectKeysValueRevealerNullableValueStructCallStructEnumtrInvoker
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived>(this Type enumblType, Type enumtrType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
        where TValue : struct
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>)
            ValueRevealerNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((keyType, valueType, enumblParamType, enumblType, enumtrType)
                        , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddWithSelectKeysValueRevealerNullableValueStructCallStructEnumtr
                                  <TKey, TValue, TKSelectEnumbl, TKSelectDerived>(key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
        BuildAddWithSelectKeysValueRevealerNullableValueStructCallStructEnumtr<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddWithSelectKeysValueRevealerNullableValueStructMethodInfo<TKey, TValue>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue?>);
        methodParamTypes[2] = enumblParamType;
        methodParamTypes[3] = typeof(PalantírReveal<TValue>);
        methodParamTypes[4] = typeof(string);
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

        // call AddWithSelectKeysIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
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
            helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>));
        return (ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived>)methodInvoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl>
        GetAddWithSelectKeysValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl>
        (Type enumblType)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
        where TValue : struct
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl>)
            ValueRevealerNullableValueStructInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumblParamType, enumblType)
                   , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType) key, bool _) =>
                     {
                         var selectKeyDerivedType = key.enumblType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);

                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType))
                             throw new ArgumentException("Expected to selectKeys element to be equatable to Key");

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.keyType;
                         genericParamTypes[1] = key.valueType;
                         genericParamTypes[2] = key.enumblType;
                         genericParamTypes[3] = selectKeyDerivedType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue?>);
                         methodParamTypes[2] = key.enumblType;
                         methodParamTypes[3] = typeof(PalantírReveal<TValue>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerateNullValueRevealer)
                                               , genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[2] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddWithSelectKeysValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl> BuildAddWithSelectKeysValueRevealerNullableValueStructInvoker
        <TKey, TValue, TKSelectEnumbl>(MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
        where TValue : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddWithSelectKeysIterateNullValueRevealerInvoke_{enumblType.Name}", typeof(KeyedCollectionMold),
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
            helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl>));
        var createInvoker = (ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl>)methodInvoker;

        return createInvoker;
    }

    private static BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
        GetAddWithSelectKeysValueRevealerNoNullableStructCallStructEnumtrInvoker
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(this Type enumblType, Type enumtrType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>)
            BothRevealersNoNullableStructCallStructEnumtrInvokerCache
                .GetOrAdd((keyType, valueType, enumblParamType, enumblType, enumtrType)
                        , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type enumeratorType) key, bool _) =>
                              key.enumblType.BuildAddWithSelectKeysEnumerateBothRevealersCallStructEnumtr
                                  <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
                                  (key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }


    private static BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
        BuildAddWithSelectKeysEnumerateBothRevealersCallStructEnumtr<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf =
            enumeratorType.GetAddWithSelectKeysBothRevealersNoNullableStructMethodInfo<TKey, TValue, TKRevealBase, TVRevealBase>();
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
                    (typeof(BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>));
        return (BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>)methodInvoker;
    }

    private static BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        GetAddWithSelectKeysBothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(Type enumblType)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
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
            (BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>)
            BothRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumblParamType, enumblType, tKRevealBase, tvRevealBase),
                     static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type tkRevealType, Type tvRevealType) key
                       , bool _) =>
                     {
                         var selectKeyDerivedType = key.enumblType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);

                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType))
                             throw new ArgumentException("Expected to selectKeys element to be equatable to Key");

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         genericParamTypes[0] = key.keyType;
                         genericParamTypes[1] = key.valueType;
                         genericParamTypes[2] = key.enumblType;
                         genericParamTypes[3] = selectKeyDerivedType;
                         genericParamTypes[4] = key.tkRevealType;
                         genericParamTypes[5] = key.tvRevealType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                         methodParamTypes[2] = key.enumblType;
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
                         methodParamTypes[4] = typeof(PalantírReveal<TKRevealBase>);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerateBothRevealers), genericParamTypes.AsArray
                                               , methodParamTypes.AsArray);

                         methodParamTypes[2] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddWithSelectKeysBothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        BuildAddWithSelectKeysBothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
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
                (typeof(BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>));
        var createInvoker
            = (BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>)methodInvoker;

        return createInvoker;
    }

    private static BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
        GetAddWithSelectKeysBothRevealersNullableValueStructCallStructEnumtrInvoker
        <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(this Type enumblType, Type enumtrType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var tvRevealerType  = typeof(TKRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>)
            BothRevealersNullableValueStructCallStructEnumtrInvokerCache
                .GetOrAdd((keyType, valueType, enumblParamType, enumblType, tvRevealerType, enumtrType)
                        , static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type tvRevealerType, Type enumeratorType) key
                                , bool _) =>
                              key.enumblType.BuildAddWithSelectKeysBothRevealersNullableValueStructCallStructEnumtr
                                  <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
                                  (key.enumblParamType, key.enumeratorType), callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
        BuildAddWithSelectKeysBothRevealersNullableValueStructCallStructEnumtr<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var callEnumtrInvokeMethInf  = enumeratorType.GetAddWithSelectKeysBothRevealersNullableValueStructMethodInfo<TKey, TValue, TKRevealBase>();
        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(KeyedCollectionMold);
        methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue?>);
        methodParamTypes[2] = enumblParamType;
        methodParamTypes[3] = typeof(PalantírReveal<TValue>);
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

        // call AddWithSelectKeysIterateBothWithNullValueRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
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
            = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived,
                                              TKRevealBase>));
        return (BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>)methodInvoker;
    }

    private static BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase>
        GetAddWithSelectKeysBothRevealersNullableValueStructInvoker
        <TKey, TValue, TKSelectEnumbl, TKRevealBase>(Type enumblType)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumblParamType = typeof(TKSelectEnumbl);
        var tkRevealBase    = typeof(TKRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumblParamType, enumblType, tkRevealBase),
                     static ((Type keyType, Type valueType, Type enumblParamType, Type enumblType, Type tkRevealBase) key, bool _) =>
                     {
                         var selectKeyDerivedType = key.enumblType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);

                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType))
                             throw new ArgumentException("Expected to selectKeys element to be equatable to Key");

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.keyType;
                         genericParamTypes[1] = key.valueType;
                         genericParamTypes[2] = key.enumblType;
                         genericParamTypes[3] = selectKeyDerivedType;
                         genericParamTypes[4] = key.tkRevealBase;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue?>);
                         methodParamTypes[2] = key.enumblType;
                         methodParamTypes[3] = typeof(PalantírReveal<TValue>);
                         methodParamTypes[4] = typeof(PalantírReveal<TKRevealBase>);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddWithSelectKeysEnumerateBothWithNullValueRevealers), genericParamTypes.AsArray
                                               , methodParamTypes.AsArray);

                         methodParamTypes[2] = key.enumblParamType;
                         var fullGenericInvoke =
                             BuildAddWithSelectKeysBothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase>
                                 (toInvokeOn, key.enumblParamType, key.enumblType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    private static BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase>
        BuildAddWithSelectKeysBothRevealersNullableValueStructInvoker
        <TKey, TValue, TKSelectEnumbl, TKRevealBase>
        (MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddWithSelectKeysIterateBothRevealers_{enumblType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(AddSelectKeysEnumerateExtensions).Module, false);
        // Make space for enumblType local variables
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

        // call AddWithSelectKeysEnumerateBothWithNullValueRevealers(KeyedCollectionMold, TEnumbl, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
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
            helperMethod
                .CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase>));

        var createInvoker = (BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase>)methodInvoker;

        return createInvoker;
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

        var invoker = GetAddWithSelectKeysValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TVRevealBase>(selectKeysActualType);
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
            var structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysValueRevealerNoNullableStructICallStructEnumtrInvoker
                <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(enumeratorType);
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
        var invoker              = GetAddWithSelectKeysValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl>(selectKeysActualType);
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
            var structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysValueRevealerNullableValueStructCallStructEnumtrInvoker
                <TKey, TValue, TKSelectEnumbl, TKSelectDerived>(enumeratorType);
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
            = GetAddWithSelectKeysBothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase,
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
        where TKey : TKRevealBase
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
            var structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysValueRevealerNoNullableStructCallStructEnumtrInvoker
                <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(enumeratorType);
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
        var invoker = GetAddWithSelectKeysBothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumbl, TKRevealBase>(selectKeysActualType);
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
            var structEnumtrInvoker = selectKeysActualType.GetAddWithSelectKeysBothRevealersNullableValueStructCallStructEnumtrInvoker
                <TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(enumeratorType);
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
