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

public static class KeyedCollectionAddAllIterateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;
    
    private static readonly ConcurrentDictionary<Type, MethodInfo>       NoRevealersMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> NoRevealersInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo>     ValueRevealerMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>     BothRevealersMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> BothRevealersInvokerCache    = new();
    
    internal delegate KeyedCollectionMold NoRevealersInvoker<in TEnumtr>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , string? valueFormatString = null
      , string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator;


    internal delegate KeyedCollectionMold ValueRevealerInvoker<in TEnumtr, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull;
    

    internal delegate KeyedCollectionMold BothRevealersInvoker<in TEnumtr, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;


    internal static MethodInfo GetAddAllNoRevealersInvokerMethodInfo(this Type enumtrType)
    {
        var methInf =
            NoRevealersMethodInfoCache.GetOrAdd
                (enumtrType, static enumeratorType =>
                {
                    var kvpTypes = enumeratorType.GetKeyedCollectionTypes();
                    if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");

                    var keyType   = kvpTypes.Value.Key;
                    var valueType = kvpTypes.Value.Value;

                    using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                    genericParamTypes[0] = enumeratorType;
                    genericParamTypes[1] = keyType;
                    genericParamTypes[2] = valueType;

                    using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                    methodParamTypes[0] = typeof(KeyedCollectionMold);
                    methodParamTypes[1] = enumeratorType;
                    methodParamTypes[2] = typeof(string);
                    methodParamTypes[3] = typeof(string);
                    methodParamTypes[4] = typeof(FormatFlags);

                    return GetStaticMethodInfo(nameof(AddAllIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
                });
        return methInf;
    }

    internal static MethodInfo GetAddAllValueRevealerInvokerMethodInfo<TValue>(this Type enumtrType)
        where TValue : notnull
    {
        var tValue = typeof(TValue);
        var methInf =
            ValueRevealerMethodInfoCache.GetOrAdd
                ((enumtrType, tValue),
                 static ((Type enumtrType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType       = kvpTypes.Value.Key;
                     var valueType     = kvpTypes.Value.Value;
                     var valueNullable = valueType.IsNullable();
                     var valueItemType = valueType.IfNullableGetUnderlyingTypeOrThis();
                     if(!valueItemType.IsAssignableTo(key.tvRevealType)) 
                         throw new ArgumentException($"Expected valueRevealer- {key.tvRevealType.ShortNameInCSharpFormat()} to be assignable from " 
                                                   + valueItemType.ShortNameInCSharpFormat());

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TValue>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);
                     
                     MethodInfo toInvokeOn;
                     if (valueNullable)
                     {
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = key.tvRevealType;
                         toInvokeOn = GetStaticMethodInfo(nameof(AddAllIterateNullValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     }
                     else
                     {
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tvRevealType;
                         toInvokeOn = GetStaticMethodInfo(nameof(AddAllIterateValueRevealer), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     }

                     return toInvokeOn;
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllBothRevealersInvokerMethodInfo<TKRevealerBase, TVRevealerBase>(this Type enumtrType)
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var tKRevealBase = typeof(TKRevealerBase);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersMethodInfoCache.GetOrAdd
                ((enumtrType, tKRevealBase, tvRevealBase),
                 static ((Type enumtrType, Type tkRevealType, Type tvRevealType) key) =>
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

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);
                     
                     MethodInfo toInvokeOn;
                     if (keyNullable && valueNullable)
                     {
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = key.tkRevealType;
                         genericParamTypes[2] = key.tvRevealType;
                         
                         toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterateBothNullRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     } else if (keyNullable)
                     {
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = key.tkRevealType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tvRevealType;
                         
                         toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterateBothWithNullKeyRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     } else if (valueNullable)
                     {
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = key.tvRevealType;
                         genericParamTypes[3] = key.tkRevealType;
                         
                         toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterateBothWithNullValueRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     }
                     else
                     {
                         using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         genericParamTypes[0] = key.enumtrType;
                         genericParamTypes[1] = keyType;
                         genericParamTypes[2] = valueType;
                         genericParamTypes[3] = key.tkRevealType;
                         genericParamTypes[4] = key.tvRevealType;
                         
                         toInvokeOn =
                             GetStaticMethodInfo(nameof(AddAllIterateBothRevealers), genericParamTypes.AsArray, methodParamTypes.AsArray);
                     }

                     return toInvokeOn;
                 });
        return methInf;
    }

    internal static NoRevealersInvoker<TEnumtr> GetAddAllNoRevealersInvoker<TEnumtr>(Type enumtrType)
        where TEnumtr : IEnumerator?
    {
        var enumtrParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersInvoker<TEnumtr>)
            NoRevealersInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType)
                   , static ((Type enumtrParamType, Type enumtrType) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(string);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(FormatFlags);

                         var toInvokeOn = key.enumtrType.GetAddAllNoRevealersInvokerMethodInfo();

                         methodParamTypes[1] = key.enumtrParamType;
                         var fullGenericInvoke =
                             BuildAddAllNoRevealersInvoker<TEnumtr>
                                 (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static NoRevealersInvoker<TEnumtr> BuildAddAllNoRevealersInvoker<TEnumtr>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterate(KeyedCollectionMold, TEnumtr, valueFmtStr, keyFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersInvoker<TEnumtr>));
        var createInvoker = (NoRevealersInvoker<TEnumtr>)methodInvoker;

        return createInvoker;
    }

    internal static ValueRevealerInvoker<TEnumtr, TVRevealBase> GetAddAllValueRevealerInvoker<TEnumtr, TVRevealBase>(
        Type enumtrType)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull
    {
        var enumtrParamType = typeof(TEnumtr);
        var tvRevealBase    = typeof(TVRevealBase);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerInvoker<TEnumtr, TVRevealBase>)
            ValueRevealerInvokerCache
                .GetOrAdd
                    ((enumtrParamType, enumtrType, tvRevealBase)
                   , static ((Type enumtrParamType, Type enumtrType, Type tvRevealType) key, bool _) =>
                     {
                         var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                         if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                         
                         using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                         methodParamTypes[0] = typeof(KeyedCollectionMold);
                         methodParamTypes[1] = key.enumtrType;
                         methodParamTypes[2] = typeof(PalantírReveal<TVRevealBase>);
                         methodParamTypes[3] = typeof(string);
                         methodParamTypes[4] = typeof(string);
                         methodParamTypes[5] = typeof(FormatFlags);

                         var toInvokeOn = key.enumtrType.GetAddAllValueRevealerInvokerMethodInfo<TVRevealBase>();

                         methodParamTypes[1] = key.enumtrParamType;
                         var fullGenericInvoke =
                             BuildAddAllValueRevealerInvoker<TEnumtr, TVRevealBase>(toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                         return fullGenericInvoke;
                     }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerInvoker<TEnumtr, TValue> BuildAddAllValueRevealerInvoker
        <TEnumtr, TValue>(MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator?
        where TValue : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateNullValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 methodParamTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerInvoker<TEnumtr, TValue>));
        var createInvoker = (ValueRevealerInvoker<TEnumtr, TValue>)methodInvoker;

        return createInvoker;
    }

    internal static BothRevealersInvoker<TEnumtr, TKRevealerBase, TVRevealerBase> GetAddAllBothRevealersInvoker
        <TEnumtr, TKRevealerBase, TVRevealerBase>(Type enumtrType)
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var enumtrParamType = typeof(TEnumtr);
        var tKRevealBase    = typeof(TKRevealerBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>)
            BothRevealersInvokerCache.GetOrAdd
                ((enumtrParamType, enumtrType, tKRevealBase, tvRevealBase),
                 static ((Type enumtrParamType, Type enumtrType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var valueType     = kvpTypes.Value.Value;
                     var valueItemType = valueType.IfNullableGetUnderlyingTypeOrThis();
                     if (!valueItemType.IsAssignableTo(key.tvRevealType)) 
                         throw new ArgumentException("Expected valueRevealer to be assignable from " + valueItemType.Name);

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(KeyedCollectionMold);
                     methodParamTypes[1] = key.enumtrType;
                     methodParamTypes[2] = typeof(PalantírReveal<TVRevealerBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TKRevealerBase>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);

                     MethodInfo toInvokeOn = key.enumtrType.GetAddAllBothRevealersInvokerMethodInfo<TKRevealerBase, TVRevealerBase>();

                     methodParamTypes[1] = key.enumtrParamType;
                     var fullGenericInvoke =
                         BuildAddAllBothRevealersInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>
                             (toInvokeOn, key.enumtrParamType, key.enumtrType, methodParamTypes.AsArray);
                     return fullGenericInvoke;
                 }, callAsFactory);
        return invoker;
    }

    internal static BothRevealersInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>
        BuildAddAllBothRevealersInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type[] parameterArgTypes)
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !enumtrParamType.IsValueType && enumtrType.IsValueType;

        var signatureName = $"{parameterArgTypes[2].Name}_{parameterArgTypes[3].Name}";
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{parameterArgTypes[1].Name}_{signatureName}"
               , typeof(KeyedCollectionMold), parameterArgTypes, typeof(KeyedCollectionAddAllIterateExtensions).Module, false);
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

        // call AddAllIterateBothRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate(typeof(BothRevealersInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>));
        var createdInvoker = (BothRevealersInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>)methodInvoker;

        return createdInvoker;
    }

    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(KeyedCollectionAddAllIterateExtensions).GetMethods(NonPublic | Public | Static);

        MethodInfo? genTypeDefMeth = null;
        var         findEnumtrType = findParamTypes[1];

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

    public static KeyedCollectionMold AddAllIterate<TEnumtr>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? callOn : callOn.AddAllIterate(value.Value, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterate<TEnumtr>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddAllNoRevealersInvoker<TEnumtr?>(actualType);
        return invoker(callOn, value, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterate<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>> =>
        value == null ? callOn : callOn.AddAllIterate<TEnumtr, TKey, TValue>(value.Value, valueFormatString, keyFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterate<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealBase : notnull =>
        value == null ? callOn : callOn.AddAllIterateValueRevealer(value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllValueRevealerInvoker<TEnumtr, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
                (value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct =>
        value == null ? callOn : callOn.AddAllIterateNullValueRevealer(value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
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
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllValueRevealerInvoker<TEnumtr, TValue>(actualType);
        return invoker(callOn, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
                (value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothRevealers(value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllBothRevealersInvoker<TEnumtr, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKey : struct
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothWithNullKeyRevealers
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
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
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllBothRevealersInvoker<TEnumtr, TKey, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey?, TValue>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothWithNullValueRevealers
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TValue : struct
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetAddAllBothRevealersInvoker<TEnumtr, TKRevealBase, TValue>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        var actualType = value.GetType();
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value.MoveNext();
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value.Current;
                mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }

    public static KeyedCollectionMold AddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct =>
        value == null
            ? callOn
            : callOn.AddAllIterateBothNullRevealers
                (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public static KeyedCollectionMold AddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue?>>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey?, TValue?>);
            while (hasValue)
            {
                if (callOn.ItemCount == 0) { callOn.BeforeFirstElement(mws); }
                var kvp = value!.Current;
                mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                mws.FieldEnd();
                mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(kvpType, callOn.ItemCount++);
            }
        }
        return callOn;
    }
}
