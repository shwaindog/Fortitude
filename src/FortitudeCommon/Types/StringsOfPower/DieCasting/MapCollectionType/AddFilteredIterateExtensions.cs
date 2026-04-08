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

public static class KeyedCollectionAddFilteredIterateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly Type GenericEnumerator = typeof(IEnumerator<>);
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>     NoRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> NoRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo>     ValueRevealerNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> ValueRevealerNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), MethodInfo>     BothRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache    = new();


    // ReSharper disable twice TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold NoRevealersInvoker<in TEnumtr, TKFilterBase, TVFilterBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?;

    // ReSharper disable twice TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold ValueRevealerInvoker<in TEnumtr, TKFilterBase, TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold BothRevealersInvoker<in TEnumtr, TKFilterBase, TVFilterBase, out TKRevealBase
                                                                             , out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    internal static MethodInfo GetAddFilteredNoRevealersInvokerMethodInfo<TKFilterBase, TVFilterBase>(this Type enumtrType)
    {
        var tkFilterType = typeof(TKFilterBase);
        var tvFilterType = typeof(TVFilterBase);
        var methInf =
            NoRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tkFilterType, tvFilterType)
               , static ((Type enumeratorType, Type tkFilterType, Type tvFilterType) key) =>
                 {
                     var kvpTypes = key.enumeratorType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = keyType;
                     genericParamTypes[2] = valueType;
                     genericParamTypes[3] = key.tkFilterType;
                     genericParamTypes[4] = key.tvFilterType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     return GetStaticMethodInfo(nameof(AddFilteredIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredValueRevealerInvokerMethodInfo<TKFilterBase, TVFilterBase, TVRevealerBase>(
        this Type enumtrType)
        where TVRevealerBase : notnull
    {
        var tvRevealerBaseType = typeof(TVRevealerBase);
        var tkFilterType       = typeof(TKFilterBase);
        var tvFilterType       = typeof(TVFilterBase);
        var methInf =
            ValueRevealerNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tkFilterType, tvFilterType, tvRevealerBaseType),
                 static ((Type enumtrType, Type tkFilterType, Type tvFilterType, Type tvRevealerType) key, bool _) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType       = kvpTypes.Value.Key;
                     var valueType     = kvpTypes.Value.Value;
                     var valueItemType = valueType.IfNullableGetUnderlyingTypeOrThis();
                     var valueNullable = valueType.IsNullable();
                     if(!valueItemType.IsAssignableTo(key.tvRevealerType)) 
                         throw new ArgumentException($"Expected valueRevealer- {key.tvRevealerType.ShortNameInCSharpFormat()} to be assignable from " 
                                                   + valueItemType.ShortNameInCSharpFormat());


                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(string);
                     methodParamTypes[6] = typeof(FormatFlags);
                     
                     MethodInfo toInvokeOn;
                     if (valueNullable)
                     {
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = key.tvRevealerType;
                         genericParamTypes[3] = key.tkFilterType;
                         toInvokeOn = GetStaticMethodInfo(nameof(AddFilteredIterateNullValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     }
                     else
                     {
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tkFilterType;
                         genericParamTypes[4] = key.tvFilterType;
                         genericParamTypes[5] = key.tvRevealerType;
                         toInvokeOn = GetStaticMethodInfo(nameof(AddFilteredIterateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     }

                     return toInvokeOn;
                 }, true);
        return methInf;
    }

    internal static MethodInfo GetAddFilteredBothRevealersInvokerMethodInfo<TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>(this Type enumtrType)
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var tKRevealBase  = typeof(TKRevealerBase);
        var tvRevealBase  = typeof(TVRevealerBase);
        var callAsFactory = true;
        var methInf =
            BothRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tkFilterType, tvFilterType, tKRevealBase, tvRevealBase),
                 static ((Type enumtrType, Type tkFilterType, Type tvFilterType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
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
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[4] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[5] = typeof(string);
                     methodParamTypes[6] = typeof(FormatFlags);
                     
                         MethodInfo toInvokeOn;
                         if (keyNullable && valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                             genericParamTypes[0] = key.enumtrType;
                             genericParamTypes[1] = key.tkRevealType;
                             genericParamTypes[2] = key.tvRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddFilteredIterateBothNullRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         } else if (keyNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                             genericParamTypes[0] = key.enumtrType;
                             genericParamTypes[1] = key.tkRevealType;
                             genericParamTypes[2] = valueType;
                             genericParamTypes[3] = key.tvFilterType;
                             genericParamTypes[4] = key.tvFilterType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddFilteredIterateBothWithNullKeyRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         } else if (valueNullable)
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                             genericParamTypes[0] = key.enumtrType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = key.tvRevealType;
                             genericParamTypes[3] = key.tkFilterType;
                             genericParamTypes[4] = key.tkRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddFilteredIterateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }
                         else
                         {
                             using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                             genericParamTypes[0] = key.enumtrType;
                             genericParamTypes[1] = keyType;
                             genericParamTypes[2] = valueType;
                             genericParamTypes[3] = key.tkFilterType;
                             genericParamTypes[4] = key.tvFilterType;
                             genericParamTypes[5] = key.tkRevealType;
                             genericParamTypes[6] = key.tvRevealType;
                             
                             toInvokeOn =
                                 GetStaticMethodInfo(nameof(AddFilteredIterateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                         }

                     return toInvokeOn;
                 }, callAsFactory);
        return methInf;
    }

    internal static NoRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase> GetAddFilteredNoRevealersInvoker
        <TEnumtr, TKFilterBase, TVFilterBase>(Type enumtrType)
        where TEnumtr : IEnumerator?
    {
        var enumtrParamType = typeof(TEnumtr);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase>)
            NoRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType, tkFilterType, tvFilterType)
                   , static ((Type enumtrParamType, Type enumtrType, Type tkFilterType, Type tvFilterType) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         var keyType   = kvpTypes.Value.Key;
                         var valueType = kvpTypes.Value.Value;

                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tkFilterType;
                         genericParamTypes[4] = key.tvFilterType;

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn = GetStaticMethodInfo(nameof(AddFilteredIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);

                         methodParamTypes[1] = key.enumtrParamType;

                         var fullGenericInvoke =
                             BuildAddFilteredNoRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase>
                                 (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static NoRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase> BuildAddFilteredNoRevealersNoNullableStructInvoker
        <TEnumtr, TKFilterBase, TVFilterBase>(
            MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
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

        // call AddFilteredIterate(KeyedCollectionMold, TEnumtr, filterPredicate,  valueFmtStr, keyFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase>));
        var createInvoker = (NoRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase>)methodInvoker;

        return createInvoker;
    }

    internal static ValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>
        GetAddFilteredValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(Type enumtrType)
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        var enumtrParamType = typeof(TEnumtr);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType, tkFilterType, tvFilterType, tvRevealBase),
                     static ((Type enumtrParamType, Type enumtrType, Type tkFilterType, Type tvFilterType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");

                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                         methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(string);
                         methodParamTypes[6] = typeof(FormatFlags);

                         var toInvokeOn = key.enumtrType.GetAddFilteredValueRevealerInvokerMethodInfo<TKFilterBase, TVFilterBase, TVRevealerBase>();

                         methodParamTypes[1] = key.enumtrParamType;

                         var fullGenericInvoke =
                             BuildAddFilteredValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>
                                 (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>
        BuildAddFilteredValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(
            MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
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

        // call AddFilteredIterateValueRevealer(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker
            = helperMethod.CreateDelegate(typeof(ValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase,
                                              TVRevealerBase>));
        var createInvoker = (ValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>)methodInvoker;

        return createInvoker;
    }

    internal static BothRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        GetAddFilteredBothRevealersInvoker
        <TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>(Type enumtrType)
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var enumtrParamType = typeof(TEnumtr);
        var tkFilterType    = typeof(TKFilterBase);
        var tvFilterType    = typeof(TVFilterBase);
        var tKRevealBase    = typeof(TKRevealerBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache.GetOrAdd
                ((enumtrParamType, enumtrType, tkFilterType, tvFilterType, tKRevealBase, tvRevealBase),
                 static ((Type enumtrParamType, Type enumtrType, Type tkFilterType, Type tvFilterType, Type tkRevealType, Type tvRevealType) key
                   , bool _) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[4] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[5] = typeof(string);
                     methodParamTypes[6] = typeof(FormatFlags);

                     var toInvokeOn = key.enumtrType.GetAddFilteredBothRevealersInvokerMethodInfo<TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>();

                     methodParamTypes[1] = key.enumtrParamType;

                     var fullGenericInvoke =
                         BuildAddFilteredBothRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
                             (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                     return fullGenericInvoke;
                 }, callAsFactory);
        return invoker;
    }


    internal static BothRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        BuildAddFilteredBothRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var signatureName = $"{methodParamTypes[2].Name}_{methodParamTypes[3].Name}";
        var helperMethod =
            new DynamicMethod
                ($"{enumtrParamType.Name}_DynamicAddFilteredIterateBothRevealers_{enumtrType.Name}_{signatureName}"
               , typeof(KeyedCollectionMold), methodParamTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
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

        // call AddFilteredIterateBothRevealers(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate(typeof(BothRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase,
                                            TVRevealerBase>));
        var createdInvoker
            = (BothRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)methodInvoker;

        return createdInvoker;
    }

    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(KeyedCollectionAddFilteredIterateExtensions).GetMethods(NonPublic | Public | Static);

        MethodInfo? genTypeDefMeth = null;
        var         findEnumtrType = findParamTypes[1];

        foreach (var checkMethodInfo in myMethodInfosCached)
        {
            if (!checkMethodInfo.Name.Contains(findMethodName)) continue;
            var checkParameterInfos = checkMethodInfo.GetParameters();
            if (checkParameterInfos.Length != findParamTypes.Length) continue;
            var enumtrType = checkParameterInfos[1].ParameterType.IfNullableGetUnderlyingTypeOrThis();
            if (!enumtrType.ImplementsGenericTypeInterface(GenericEnumerator))
            {
                Console.Out.WriteLine("enumtrType: {0}", enumtrType);
                continue;
            }
            if (findGenericParams.Length > 0)
            {
                if (!checkMethodInfo.IsGenericMethod) continue;
                var checkGenParams = checkMethodInfo.GetGenericArguments();
                if (checkGenParams.Length != findGenericParams.Length) continue;
            }
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

    public static KeyedCollectionMold AddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? callOn : callOn.AddFilteredIterate(value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredNoRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        value == null
            ? callOn
            : callOn.AddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>
                (value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                var kvp          = value.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealerBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealerBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateValueRevealer
                (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealerBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                var kvp          = value.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateNullValueRevealer
                (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
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
        var invoker = GetAddFilteredValueRevealerInvoker<TEnumtr, TKFilterBase, TValue?, TValue>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>
                (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?
        where TValue : struct
    {
        if (value == null) return callOn;
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                var kvp          = value.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateBothRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddFilteredBothRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                var kvp          = value.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKey : struct
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateBothWithNullKeyRevealers
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
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
        var invoker = GetAddFilteredBothRevealersInvoker<TEnumtr, TKey?, TVFilterBase, TKey, TVRevealBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                var kvp          = value.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct
        where TKRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateBothWithNullValueRevealers
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredBothRevealersInvoker<TEnumtr, TKFilterBase, TValue?, TKRevealBase, TValue>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        var mws = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;

        var actualType = value.GetType();

        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                var kvp          = value.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddFilteredIterateBothNullRevealers
                (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        if (value == null) return callOn;
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value.GetType();
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                var kvp          = value.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return callOn;
    }
}
