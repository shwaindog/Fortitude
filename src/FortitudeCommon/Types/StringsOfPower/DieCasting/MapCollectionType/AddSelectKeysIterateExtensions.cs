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

public static class AddSelectKeysIterateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>       NoRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> NoRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo>     ValueRevealerNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> ValueRevealerNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>     ValueRevealerNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), MethodInfo>     BothRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo>     BothRevealersNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructInvokerCache    = new();

    internal delegate KeyedCollectionMold NoRevealersNoNullableStructInvoker<TKey, TValue, in TKSelectTEnumtr>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator?
        where TKey : notnull;

    internal delegate KeyedCollectionMold NoRevealersNoNullableStructInvoker<TKey, TValue, in TKSelectEnumtr, TKSelectDerived>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey;

    internal delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<TKey, TValue, in TKSelectEnumtr, out TVRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<TKey, TValue, in TKSelectEnumtr, TKSelectDerived, out TVRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<TKey, TValue, in TKSelectEnumtr>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull
        where TValue : struct;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<TKey, TValue, in TKSelectEnumtr, TKSelectDerived>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull
        where TValue : struct
        where TKSelectDerived : TKey;


    internal delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<TKey, TValue, in TKSelectEnumtr, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<TKey, TValue, in TKSelectEnumtr, in TKSelectDerived, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr, out TKRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, out TKRevealBase>(
        KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull;

    internal static MethodInfo GetAddWithSelectKeysNoRevealersInvokerMethodInfo<TKey, TValue>(this Type enumtrType)
    {
        var keyType = typeof(TKey);
        var valueType = typeof(TValue);
        var methInf =
            NoRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((keyType, valueType, enumtrType)
               , static ((Type keyType, Type valueType, Type enumeratorType) key) =>
                {
                    var selectKeyDerivedType = key.enumeratorType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                    if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");
                    
                    using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                    genericParamTypes[0] = key.keyType;
                    genericParamTypes[1] = key.valueType;
                    genericParamTypes[2] = key.enumeratorType;
                    genericParamTypes[3] = selectKeyDerivedType;

                    using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                    methodParamTypes[0] = typeof(KeyedCollectionMold);
                    methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                    methodParamTypes[2] = key.enumeratorType;
                    methodParamTypes[3] = typeof(string);
                    methodParamTypes[4] = typeof(string);
                    methodParamTypes[5] = typeof(FormatFlags);

                    return GetStaticMethodInfo(nameof(AddWithSelectKeysIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
                });
        return methInf;
    }

    internal static NoRevealersNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr> 
        GetAddWithSelectKeysNoRevealersInvoker<TKey, TValue, TKSelectTEnumtr>(Type enumtrType)
        where TKey : notnull
        where TKSelectTEnumtr : IEnumerator?
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumtrParamType = typeof(TKSelectTEnumtr);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr>)
            NoRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumtrParamType, enumtrType)
                   , static ((Type keyType, Type valueType, Type enumtrParamType, Type enumtrType) key, bool _) =>
                     {
                    
                         var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");
                    
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.keyType;
                         genericParamTypes[1] = key.valueType;
                         genericParamTypes[2] = key.enumtrType;
                         genericParamTypes[3] = selectKeyDerivedType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                         methodParamTypes[2] = key.enumtrType;
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);
                         
                         var toInvokeOn = 
                             GetStaticMethodInfo(nameof(AddWithSelectKeysIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         var genGenMethod
                             = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddWithSelectKeysNoRevealersNoNullableStructInvoker)));
                         
                         genericParamTypes[2] = key.enumtrParamType;
                         var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                         
                         methodParamTypes[2] = key.enumtrParamType;
                    
                         using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                         invokeReflectedArgs[0] = toInvokeOn;
                         invokeReflectedArgs[1] = key.enumtrParamType;
                         invokeReflectedArgs[2] = key.enumtrType;
                         invokeReflectedArgs[3] = methodParamTypes.AsArray;
                         
                         return (NoRevealersNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr>
                                 )concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                     }, callAsFactory);
        return invoker;
    }

    internal static NoRevealersNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr> 
        BuildAddWithSelectKeysNoRevealersNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TKSelectTEnumtr : IEnumerator<TKSelectDerived>
        where TKey : notnull
        where TKSelectDerived : TKey
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;
        
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes , typeof(AddSelectKeysIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddWithSelectKeysIterate(KeyedCollectionMold, TEnumtr, valueFmtStr, keyFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>));
        var createInvoker = (NoRevealersNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, IReadOnlyDictionary<TKey, TValue>? value, TKSelectTEnumtr? selectKeysEnumtr
          , string? valueFmtStr, string? keyFmtString, FormatFlags flags) =>
            createInvoker(kcm, value, selectKeysEnumtr, valueFmtStr, keyFmtString, flags);

        return Wrapped;
    }

    internal static MethodInfo GetAddWithSelectKeysValueRevealerNoNullableStructIInvokerMethodInfo<TKey, TValue, TVRevealerBase>(this Type enumtrType)
        where TKey : notnull
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var keyType            = typeof(TKey);
        var valueType          = typeof(TValue);
        var tvRevealerBaseType = typeof(TVRevealerBase);
        var methInf =
            ValueRevealerNoNullableStructMethodInfoCache.GetOrAdd
                ((keyType, valueType, enumtrType, tvRevealerBaseType)
               , static ((Type keyType, Type valueType, Type enumtrType, Type tvRevealerType) key, bool _) =>
                 {
                     var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                     if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     genericParamTypes[0] = key.keyType;
                     genericParamTypes[1] = key.valueType;
                     genericParamTypes[2] = key.enumtrType;
                     genericParamTypes[3] = selectKeyDerivedType;
                     genericParamTypes[4] = key.tvRevealerType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                     methodParamTypes[2] = key.enumtrType;
                     methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(string);
                     methodParamTypes[6] = typeof(FormatFlags);

                     return GetStaticMethodInfo(nameof(AddWithSelectKeysIterateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 }, true);
        return methInf;
    }

    internal static ValueRevealerNoNullableStructInvoker<TKey, TValue, TEnumtr, TVRevealerBase> GetAddWithSelectKeysValueRevealerNoNullableStructInvoker
        <TKey, TValue, TEnumtr, TVRevealerBase>
        (Type enumtrType)
        where TKey : notnull
        where TValue : TVRevealerBase?
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumtrParamType = typeof(TEnumtr);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TKey, TValue, TEnumtr, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumtrParamType, enumtrType, tvRevealBase),
                     static ((Type keyType, Type valueType, Type enumtrParamType, Type enumtrType, Type tvRevealType) key, bool _) =>
                     {
                         var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.keyType;
                         genericParamTypes[1] = key.valueType;
                         genericParamTypes[2] = key.enumtrType;
                         genericParamTypes[3] = selectKeyDerivedType;
                         genericParamTypes[4] = key.tvRevealType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                         methodParamTypes[2] = key.enumtrType;
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);
                         
                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddWithSelectKeysIterateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         var genGenMethod
                             = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddWithSelectKeysValueRevealerNoNullableStructInvoker)));
                         
                         genericParamTypes[2] = key.enumtrParamType;
                         var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                         
                         methodParamTypes[2] = key.enumtrParamType;
                         
                         using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                         invokeReflectedArgs[0] = toInvokeOn;
                         invokeReflectedArgs[1] = key.enumtrParamType;
                         invokeReflectedArgs[2] = key.enumtrType;
                         invokeReflectedArgs[3] = methodParamTypes.AsArray;
                         
                         return (ValueRevealerNoNullableStructInvoker<TKey, TValue, TEnumtr, TVRevealerBase>)
                             concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                     }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TVRevealerBase> BuildAddWithSelectKeysValueRevealerNoNullableStructInvoker
        <TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealerBase>(
            MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TKey : notnull
        where TValue : TVRevealerBase?
        where TKSelectDerived : TKey
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;
        
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddWithSelectKeysIterateValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(AddSelectKeysIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddWithSelectKeysIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealerBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, IReadOnlyDictionary<TKey, TValue>? value, TKSelectEnumtr? selectKeysEnumtr
          , PalantírReveal<TVRevealerBase> valueRevealer, string? keyFmtStr, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(kcm, value, selectKeysEnumtr, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }

    internal static MethodInfo GetAddWithSelectKeysValueRevealerNullableValueStructMethodInfo<TKey, TValue>(this Type enumtrType)
        where TValue : struct
    {
        var keyType   = typeof(TKey);
        var valueType = typeof(TValue);
        var methInf =
            ValueRevealerNullableValueStructMethodInfoCache.GetOrAdd
                ((keyType, valueType, enumtrType),
                 static ((Type keyType, Type valueType, Type enumtrType) key) =>
                 {
                     var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                     if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");
                         
                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                     genericParamTypes[0] = key.keyType;
                     genericParamTypes[1] = key.valueType;
                     genericParamTypes[2] = key.enumtrType;
                     genericParamTypes[3] = selectKeyDerivedType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue?>);
                     methodParamTypes[2] = key.enumtrType;
                     methodParamTypes[3] = typeof(PalantírReveal<TValue>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(string);
                     methodParamTypes[6] = typeof(FormatFlags);

                     return GetStaticMethodInfo(nameof(AddWithSelectKeysIterateNullValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectTEnumtr> GetAddWithSelectKeysValueRevealerNullableValueStructInvoker
        <TKey, TValue, TKSelectTEnumtr>( Type enumtrType)
        where TKSelectTEnumtr : IEnumerator?
        where TKey : notnull
        where TValue : struct
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumtrParamType = typeof(TKSelectTEnumtr);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectTEnumtr>)
            ValueRevealerNullableValueStructInvokerCache
                .GetOrAdd((keyType, valueType, enumtrParamType, enumtrType)
                         ,
                          static ((Type keyType, Type valueType, Type enumtrParamType, Type enumtrType) key, bool _) =>
                          {
                              var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                              if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");
                         
                              using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                              genericParamTypes[0] = key.keyType;
                              genericParamTypes[1] = key.valueType;
                              genericParamTypes[2] = key.enumtrType;
                              genericParamTypes[3] = selectKeyDerivedType;

                              using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                              methodParamTypes[0] = typeof(KeyedCollectionMold);
                              methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue?>);
                              methodParamTypes[2] = key.enumtrType;
                              methodParamTypes[3] = typeof(PalantírReveal<TValue>);
                              methodParamTypes[4] = typeof(string);
                              methodParamTypes[5] = typeof(string);
                              methodParamTypes[6] = typeof(FormatFlags);
                              
                              var toInvokeOn =
                                  GetStaticMethodInfo(nameof(AddWithSelectKeysIterateNullValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);

                              var genGenMethod
                                  = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddWithSelectKeysValueRevealerNullableValueStructInvoker)));
                              
                              genericParamTypes[2] = key.enumtrParamType;
                              var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                              
                              methodParamTypes[2] = key.enumtrParamType;
                         
                              using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                              invokeReflectedArgs[0] = toInvokeOn;
                              invokeReflectedArgs[1] = key.enumtrParamType;
                              invokeReflectedArgs[2] = key.enumtrType;
                              invokeReflectedArgs[3] = methodParamTypes.AsArray;
                              
                              return (ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectTEnumtr>)
                                  concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                          }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr> BuildAddWithSelectKeysValueRevealerNullableValueStructInvoker
        <TKey, TValue, TKSelectEnumtr, TKSelectDerived>(MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull
        where TValue : struct
        where TKSelectDerived : TKey
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;
        
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddWithSelectKeysIterateNullValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(AddSelectKeysIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddWithSelectKeysIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived>));
        var createInvoker   = (ValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, IReadOnlyDictionary<TKey, TValue?>? value, TKSelectEnumtr? selectKeysEnumtr
          , PalantírReveal<TValue> valueRevealer, string? keyFmtStr, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(kcm, value, selectKeysEnumtr, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetAddWithSelectKeysBothRevealersNoNullableStructMethodInfo<TKey, TValue, TKRevealerBase, TVRevealerBase>(this Type enumtrType)
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var keyType      = typeof(TKey);
        var valueType    = typeof(TValue);
        var tKRevealBase = typeof(TKRevealerBase);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((keyType, valueType, enumtrType, tKRevealBase, tvRevealBase),
                 static ((Type keyType, Type valueType, Type enumtrType, Type tkRevealType, Type tvRevealType) key) =>
                 {
                     var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                     if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");
                         
                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     genericParamTypes[0] = key.keyType;
                     genericParamTypes[1] = key.valueType;
                     genericParamTypes[2] = key.enumtrType;
                     genericParamTypes[3] = selectKeyDerivedType;
                     genericParamTypes[4] = key.tkRevealType;
                     genericParamTypes[5] = key.tvRevealType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                     methodParamTypes[2] = key.enumtrType;
                     methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[4] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[5] = typeof(string);
                     methodParamTypes[6] = typeof(FormatFlags);

                     return GetStaticMethodInfo(nameof(AddWithSelectKeysIterateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase> GetAddWithSelectKeysBothRevealersNoNullableStructInvoker
        <TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>(Type enumtrType)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumtrParamType = typeof(TKSelectEnumtr);
        var tKRevealBase    = typeof(TKRevealBase);
        var tvRevealBase    = typeof(TVRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>)
            BothRevealersNoNullableStructInvokerCache.GetOrAdd
                ((keyType, valueType, enumtrParamType, enumtrType, tKRevealBase, tvRevealBase),
                 static ((Type keyType, Type valueType, Type enumtrParamType, Type enumtrType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                 {
                     var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                     if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");
                         
                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     genericParamTypes[0] = key.keyType;
                     genericParamTypes[1] = key.valueType;
                     genericParamTypes[2] = key.enumtrType;
                     genericParamTypes[3] = selectKeyDerivedType;
                     genericParamTypes[4] = key.tkRevealType;
                     genericParamTypes[5] = key.tvRevealType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                     methodParamTypes[2] = key.enumtrType;
                     methodParamTypes[3] = typeof(PalantírReveal<TVRevealBase>);
                     methodParamTypes[4] = typeof(PalantírReveal<TKRevealBase>);
                     methodParamTypes[5] = typeof(string);
                     methodParamTypes[6] = typeof(FormatFlags);
                     
                     var toInvokeOn =
                         GetStaticMethodInfo(nameof(AddWithSelectKeysIterateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     
                     var genGenMethod
                         = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddWithSelectKeysBothRevealersNoNullableStructInvoker)));
                     
                     genericParamTypes[2] = key.enumtrParamType;
                     var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);

                     methodParamTypes[2] = key.enumtrParamType;
                         
                     using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                     invokeReflectedArgs[0] = toInvokeOn;
                     invokeReflectedArgs[1] = key.enumtrParamType;
                     invokeReflectedArgs[2] = key.enumtrType;
                     invokeReflectedArgs[3] = methodParamTypes.AsArray;
                     
                     return (BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>)
                         concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                 }, callAsFactory);
        return invoker;
    }


    internal static BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>
        BuildAddWithSelectKeysBothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] parameterArgTypes)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var signatureName = $"{parameterArgTypes[2].Name}_{parameterArgTypes[3].Name}";
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddWithSelectKeysIterateBothRevealers_{parameterArgTypes[1].Name}_{signatureName}"
               , typeof(KeyedCollectionMold), parameterArgTypes, typeof(AddSelectKeysIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddWithSelectKeysIterateBothRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate(typeof(BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>));
        var createInvoker = (BothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, IReadOnlyDictionary<TKey, TValue>? value, TKSelectEnumtr? selectKeysEnumtr
          , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(kcm, value, selectKeysEnumtr, valueRevealer, keyRevealer, valueFmtStr, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetAddWithSelectKeysBothRevealersNullableValueStructMethodInfo<TKey, TValue, TKRevealerBase>(this Type enumtrType)
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var keyType      = typeof(TKey);
        var valueType    = typeof(TValue);
        var tkRevealBase = typeof(TKRevealerBase);
        var methInf =
            BothRevealersNullableValueStructMethodInfoCache.GetOrAdd
                ((keyType, valueType, enumtrType, tkRevealBase),
                 static (( Type keyType,  Type valueType,  Type enumtrType, Type tkRevealBase) key) =>
                 {
                     var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                     if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");
                         
                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     genericParamTypes[0] = key.keyType;
                     genericParamTypes[1] = key.valueType;
                     genericParamTypes[2] = key.enumtrType;
                     genericParamTypes[3] = selectKeyDerivedType;
                     genericParamTypes[4] = key.tkRevealBase;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue>);
                     methodParamTypes[2] = key.enumtrType;
                     methodParamTypes[3] = typeof(PalantírReveal<TValue>);
                     methodParamTypes[4] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[5] = typeof(string);
                     methodParamTypes[6] = typeof(FormatFlags);

                     return
                         GetStaticMethodInfo (nameof(AddWithSelectKeysIterateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectTEnumtr, TKRevealBase> GetAddWithSelectKeysBothRevealersNullableValueStructInvoker
        <TKey, TValue, TKSelectTEnumtr, TKRevealBase>(Type enumtrType)
        where TKSelectTEnumtr : IEnumerator?
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        var keyType         = typeof(TKey);
        var valueType       = typeof(TValue);
        var enumtrParamType = typeof(TKSelectTEnumtr);
        var tkRevealBase    = typeof(TKRevealBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectTEnumtr, TKRevealBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((keyType, valueType, enumtrParamType, enumtrType, tkRevealBase),
                     static ((Type keyType, Type valueType, Type enumtrParamType, Type enumtrType, Type tkRevealBase) key, bool _) =>
                     {
                         var selectKeyDerivedType = key.enumtrType.GetIterableElementType()?.IfNullableGetUnderlyingTypeOrThis() ?? typeof(object);
                    
                         if (!selectKeyDerivedType.IsAssignableTo(key.keyType)) throw new ArgumentException("Expected to selectKeys element to be equatable to Key");
                         
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.keyType;
                         genericParamTypes[1] = key.valueType;
                         genericParamTypes[2] = key.enumtrType;
                         genericParamTypes[3] = selectKeyDerivedType;
                         genericParamTypes[4] = key.tkRevealBase;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = typeof(IReadOnlyDictionary<TKey, TValue?>);
                         methodParamTypes[2] = key.enumtrType;
                         methodParamTypes[3] = typeof(PalantírReveal<TValue>);
                         methodParamTypes[4] = typeof(PalantírReveal<TKRevealBase>);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn =
                             GetStaticMethodInfo(nameof(AddWithSelectKeysIterateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         var genGenMethod
                             = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddWithSelectKeysBothRevealersNullableValueStructInvoker)));
                         
                         genericParamTypes[2] = key.enumtrParamType;
                         var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);
                         
                         methodParamTypes[2] = key.enumtrParamType;
                         
                         using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
                         invokeReflectedArgs[0] = toInvokeOn;
                         invokeReflectedArgs[1] = key.enumtrParamType;
                         invokeReflectedArgs[2] = key.enumtrType;
                         invokeReflectedArgs[3] = methodParamTypes.AsArray;
                         
                         return (BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectTEnumtr, TKRevealBase>)
                             concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
                     }, callAsFactory);
        return invoker;
    }

    internal static BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr, TKRevealBase> 
        BuildAddWithSelectKeysBothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>(
            MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddWithSelectKeysIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(AddSelectKeysIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call AddWithSelectKeysIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker  = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>));
        var createInvoker = (BothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, IReadOnlyDictionary<TKey, TValue?>? value, TKSelectEnumtr? selectKeysEnumtr
          , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(kcm, value, selectKeysEnumtr, valueRevealer, keyRevealer, valueFmtStr, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(AddSelectKeysIterateExtensions).GetMethods( NonPublic | Public | Static);

        MethodInfo? genTypeDefMeth = null;
        var         findEnumtrType = findParamTypes[2];

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
            var checkEnumtrType  = checkParameterInfos[2].ParameterType;
            if ((!findEnumtrType.IsNullable() && checkEnumtrType.IsNullable())
             || (findEnumtrType.IsNullable() && !checkEnumtrType.IsNullable()))
                continue;
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
    
    public static KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator
        where TKey: notnull
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterate(value, selectKeys.Value, valueFormatString, keyFormatString, formatFlags);
    }
    
    public static KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator?
        where TKey: notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var selectKeysActualType = selectKeys?.GetType() ?? typeof(TKSelectTEnumtr);
        var invoker              = GetAddWithSelectKeysNoRevealersInvoker<TKey, TValue, TKSelectTEnumtr>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueFormatString, keyFormatString, formatFlags);
    }
    
    public static KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey: notnull
        where TKSelectDerived: TKey
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>
            (value, selectKeys.Value, valueFormatString, keyFormatString, formatFlags);
    }
    
    public static KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKSelectDerived>?
        where TKey: notnull
        where TKSelectDerived: TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (callOn.ItemCount == 0)
            {
                callOn.BeforeFirstElement(mws);
            }
            mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        selectKeys?.Dispose();
        return mws.Mold;
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectTEnumtr, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator
        where TKey: notnull
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterateValueRevealer(value, selectKeys.Value, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectTEnumtr, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator?
        where TKey: notnull
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var selectKeysActualType = selectKeys?.GetType() ?? typeof(TKSelectTEnumtr);
        var invoker              = 
            GetAddWithSelectKeysValueRevealerNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr, TVRevealBase>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey: notnull
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TVRevealBase>
            (value, selectKeys.Value, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKSelectDerived>?
        where TKey: notnull
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            keyFormatString ??= "";
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (callOn.ItemCount == 0)
            {
                callOn.BeforeFirstElement(mws);
            }
            mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        selectKeys?.Dispose();
        return mws.Mold;
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectTEnumtr>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator
        where TKey : notnull
        where TValue : struct
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterateNullValueRevealer(value, selectKeys.Value, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectTEnumtr>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator?
        where TKey : notnull
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var selectKeysActualType = selectKeys?.GetType() ?? typeof(TKSelectTEnumtr);
        var invoker              = GetAddWithSelectKeysValueRevealerNullableValueStructInvoker<TKey, TValue, TKSelectTEnumtr>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator<TKSelectDerived>
        where TValue : struct
        where TKSelectDerived : TKey
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>
            (value, selectKeys.Value, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectTEnumtr, TKSelectDerived>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKSelectDerived>?
        where TValue : struct
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            keyFormatString ??= "";
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (callOn.ItemCount == 0)
            {
                callOn.BeforeFirstElement(mws);
            }
            mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        selectKeys?.Dispose();
        return mws.Mold;
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectTEnumtr, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterateBothRevealers(value, selectKeys.Value, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectTEnumtr, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var selectKeysActualType = selectKeys?.GetType() ?? typeof(TKSelectTEnumtr);
        var invoker = 
            GetAddWithSelectKeysBothRevealersNoNullableStructInvoker<TKey, TValue, TKSelectTEnumtr, TKRevealBase, TVRevealBase>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>
            (value, selectKeys.Value, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (callOn.ItemCount == 0)
            {
                callOn.BeforeFirstElement(mws);
            }
            mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        selectKeys?.Dispose();
        return mws.Mold;
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectTEnumtr, TKRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterateBothWithNullValueRevealers(value, selectKeys.Value, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectTEnumtr, TKRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator?
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var selectKeysActualType = selectKeys?.GetType() ?? typeof(TKSelectTEnumtr);
        var invoker = GetAddWithSelectKeysBothRevealersNullableValueStructInvoker<TKey, TValue, TKSelectTEnumtr, TKRevealBase>(selectKeysActualType);
        return invoker(callOn, value, selectKeys, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TKRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        if (selectKeys == null) return callOn;
        return callOn.AddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TKRevealBase>
            (value, selectKeys.Value, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectTEnumtr, TKSelectDerived, TKRevealBase>(
        this KeyedCollectionMold callOn
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (callOn.ItemCount == 0)
            {
                callOn.BeforeFirstElement(mws);
            }
            mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
            mws.FieldEnd();
            mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        selectKeys?.Dispose();
        return mws.Mold;
    }
}
