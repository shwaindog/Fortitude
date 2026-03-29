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

public static class WhenPopulatedAddAllIterateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;


    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo>       NoRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> NoRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> ValueRevealerNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>   ValueRevealerNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>     ValueRevealerNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> ValueRevealerNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo>     BothRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo>     BothRevealersNullableKeyStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableKeyStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo>     BothRevealersNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo> BothRevealersBothNullableStructsMethodInfoCache = new();

    internal delegate TMold NoRevealersNoNullableStructInvoker<TMold, in TEnumtr>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?;
    
    internal delegate TMold NoRevealersNoNullableStructInvoker<TMold, in TEnumtr, TKey, TValue>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?;

    internal delegate TMold ValueRevealerNoNullableStructInvoker<TMold, in TEnumtr, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull;
    
    internal delegate TMold ValueRevealerNoNullableStructInvoker<TMold, in TEnumtr, TKey, TValue, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate TMold ValueRevealerNullableValueStructInvoker<TMold, in TEnumtr, TValue>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TValue : struct;
    
    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate TMold ValueRevealerNullableValueStructInvoker<TMold, in TEnumtr, TKey, TValue>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct;


    internal delegate TMold BothRevealersNoNullableStructInvoker<TMold, in TEnumtr, out TKRevealBase, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    internal delegate TMold BothRevealersNoNullableStructInvoker<TMold, in TEnumtr, TKey, TValue, out TKRevealBase, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate TMold BothRevealersNullableKeyStructInvoker<TMold, in TEnumtr, TKey, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate TMold BothRevealersNullableKeyStructInvoker<TMold, in TEnumtr, TKey, TValue, out TVRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate TMold BothRevealersNullableValueStructInvoker<TMold, in TEnumtr, TValue, out TKRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate TMold BothRevealersNullableValueStructInvoker<TMold, in TEnumtr, TKey, TValue, out TKRevealBase>(
        SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
        where TKRevealBase : notnull;
    
    internal static MethodInfo GetWhenPopulatedAddAllNoRevealersInvokerMethodInfo<TMold>(this Type enumtrType)
        where TMold : TypeMolder
    {
        var moldType        = typeof(TMold);
        var callAsFactory   = true;
        var methInf =
            NoRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((moldType, enumtrType), static ((Type moldType, Type enumeratorType) key, bool _ )=>
                 {
                     var kvpTypes = key.enumeratorType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return GetStaticMethodInfo(nameof(WhenPopulatedAddAllIterate)
                            , [ key.moldType, key.enumeratorType, keyType, valueType],
                              [
                                  typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumeratorType
                                , typeof(string), typeof(string), typeof(FormatFlags)
                              ]);
                 }, callAsFactory);
        return methInf;
    }

    internal static NoRevealersNoNullableStructInvoker<TMold, TEnumtr> GetWhenPopulatedAddAllNoRevealersInvoker<TMold, TEnumtr>(Type enumtrType)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
    {
        var moldType        = typeof(TMold);
        var enumtrParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (NoRevealersNoNullableStructInvoker<TMold, TEnumtr>)
            NoRevealersNoNullableStructInvokerCache
                .GetOrAdd
                    ((moldType, enumtrParamType, enumtrType)
                    , static ((Type moldType, Type enumtrParamType, Type enumtrType) key, bool _) =>
            {
                var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                var keyType   = kvpTypes.Value.Key;
                var valueType = kvpTypes.Value.Value;
                var toInvokeOn = GetStaticMethodInfo(nameof(WhenPopulatedAddAllIterate)
                                                   , [key.moldType, key.enumtrType, keyType, valueType],
                [ typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                  , typeof(string), typeof(string), typeof(FormatFlags) ]);

                var genGenMethod
                    = myMethodInfosCached!
                        .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllNoRevealersNoNullableStructInvoker)));
                var concreteGenMethod
                    = genGenMethod.MakeGenericMethod([key.moldType, key.enumtrParamType, keyType, valueType ]);
                return (NoRevealersNoNullableStructInvoker<TMold, TEnumtr>)concreteGenMethod.Invoke(null, [ toInvokeOn, key.enumtrType ])!;
            }, callAsFactory);
        return invoker;
    }

    internal static NoRevealersNoNullableStructInvoker<TMold, TEnumtr> BuildWhenPopulatedAddAllNoRevealersNoNullableStructInvoker<TMold, TEnumtr, TKey, TValue>
        (MethodInfo methodInfo, Type enumtrType)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        var shouldUnbox = !typeof(TEnumtr).IsValueType && enumtrType.IsValueType;
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumtrType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), typeof(TEnumtr)
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }
        
        // call WhenPopulatedAddAllIterate(KeyedCollectionMold, TEnumtr, valueFmtStr, keyFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit( shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersNoNullableStructInvoker<TMold, TEnumtr, TKey, TValue>));
        var createInvoker = (NoRevealersNoNullableStructInvoker<TMold, TEnumtr, TKey, TValue>)methodInvoker;
        
        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumtr? enumtr
          , string? valueFmtStr, string? keyFmtString, FormatFlags flags) =>
            createInvoker(kcm, fieldName, enumtr, valueFmtStr, keyFmtString, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetWhenPopulatedAddAllValueRevealerNoNullableStructIInvokerMethodInfo<TMold, TVRevealerBase>(this Type enumtrType)
    where TVRevealerBase : notnull
    where TMold : TypeMolder
    {
        var moldType           = typeof(TMold);
        var tvRevealerBaseType = typeof(TVRevealerBase);
        var callAsFactory      = true;
        var methInf =
            ValueRevealerNoNullableStructMethodInfoCache.GetOrAdd
                ((moldType, enumtrType, tvRevealerBaseType)
               , static ((Type moldType, Type enumeratorType, Type tvRevealerType) key, bool _) =>
                {
                    var kvpTypes = key.enumeratorType.GetKeyedCollectionTypes();
                    if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                    var keyType   = kvpTypes.Value.Key;
                    var valueType = kvpTypes.Value.Value;
                    return
                        GetStaticMethodInfo
                            (nameof(WhenPopulatedAddAllIterateValueRevealer)
                           , [ key.moldType, key.enumeratorType, keyType, valueType, key.tvRevealerType],
                             [
                                 typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumeratorType
                               , typeof(PalantírReveal<TVRevealerBase>), typeof(string), typeof(string), typeof(FormatFlags)
                             ]);
                }, callAsFactory);
        return methInf;
    }

    internal static ValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TVRevealerBase> 
        GetWhenPopulatedAddAllValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TVRevealerBase>( Type enumtrType)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumtrParamType = typeof(TEnumtr);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((moldType, enumtrParamType, enumtrType, tvRevealBase),
                       static ((Type moldType, Type enumtrParamType, Type enumtrType, Type tvRevealType) key, bool _) =>
                       {
                           var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                           if (kvpTypes == null)
                               throw new ArgumentException("Expected to receive a KeyValue enumerator");
                           var keyType   = kvpTypes.Value.Key;
                           var valueType = kvpTypes.Value.Value;
                           var toInvokeOn =
                               GetStaticMethodInfo
                                   (nameof(WhenPopulatedAddAllIterateValueRevealer)
                                 , [key.moldType, key.enumtrType, keyType, valueType, key.tvRevealType],
                                   [
                                       typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                                     , typeof(PalantírReveal<TVRevealerBase>), typeof(string), typeof(string), typeof(FormatFlags)
                                   ]);

                           var genGenMethod
                               = myMethodInfosCached!
                                   .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllValueRevealerNoNullableStructInvoker)));
                           var concreteGenMethod
                               = genGenMethod.MakeGenericMethod([key.moldType, key.enumtrParamType, keyType, valueType, key.tvRevealType ]);
                           return (ValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TVRevealerBase>)
                               concreteGenMethod.Invoke(null, [ toInvokeOn, key.enumtrType, typeof(PalantírReveal<TVRevealerBase>) ])!;
                           
                       }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TVRevealerBase> BuildWhenPopulatedAddAllValueRevealerNoNullableStructInvoker
        <TMold, TEnumtr, TKey, TValue, TVRevealerBase>(MethodInfo methodInfo, Type enumtrType, Type? tvRevealBase = null)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !typeof(TEnumtr).IsValueType && enumtrType.IsValueType;
        
        tvRevealBase ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateValueRevealerInvoke_{enumtrType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), typeof(TEnumtr), tvRevealBase
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllIterateExtensions).Module, false);
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
        
        // call WhenPopulatedAddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit( shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TKey, TValue, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TKey, TValue, TVRevealerBase>)methodInvoker;
        
        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumtr? enumtr
          , PalantírReveal<TVRevealerBase> valueRevealer, string? keyFmtStr, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(kcm, fieldName, enumtr, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetWhenPopulatedAddAllValueRevealerNullableValueStructMethodInfo<TMold, TValue>(this Type enumtrType)
        where TMold : TypeMolder
        where TValue : struct
    {
        var moldType = typeof(TMold);
        var tValue   = typeof(TValue);
        var methInf =
            ValueRevealerNullableValueStructMethodInfoCache.GetOrAdd
                ((moldType, enumtrType, tValue),
                 static ((Type moldType, Type enumtrType, Type tvalueType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     return
                         GetStaticMethodInfo
                             (nameof(WhenPopulatedAddAllIterateNullValueRevealer)
                            , [ key.moldType, key.enumtrType, keyType, key.tvalueType],
                              [
                                  typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                                , typeof(PalantírReveal<TValue>), typeof(string), typeof(string), typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static ValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TValue> 
        GetWhenPopulatedAddAllValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TValue>(Type enumtrType)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        var moldType        = typeof(TMold);
        var enumtrParamType = typeof(TEnumtr);
        var tValueType    = typeof(TValue);
        var callAsFactory   = true;
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TValue>)
            ValueRevealerNullableValueStructInvokerCache
                .GetOrAdd((moldType, enumtrParamType, enumtrType, tValueType)
            ,
             static ((Type moldType, Type enumtrParamType, Type enumtrType, Type tValueType) key, bool _) =>
             {
                 var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                 if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                 var keyType   = kvpTypes.Value.Key;
                 var toInvokeOn =
                     GetStaticMethodInfo(nameof(WhenPopulatedAddAllIterateNullValueRevealer)
                                       , [key.moldType, key.enumtrType, keyType, key.tValueType],
                     [
                         typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType, typeof(PalantírReveal<TValue>)
                       , typeof(string), typeof(string), typeof(FormatFlags)
                     ]);

                 var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllValueRevealerNullableValueStructInvoker)));
                 var concreteGenMethod = genGenMethod.MakeGenericMethod([key.moldType, key.enumtrParamType, keyType, key.tValueType]);
                 return (ValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TValue>)
                     concreteGenMethod.Invoke(null, [ toInvokeOn, key.enumtrParamType, key.enumtrType, typeof(PalantírReveal<TValue>)])!;
               }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TValue> 
        BuildWhenPopulatedAddAllValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TKey, TValue>
        (MethodInfo methodInfo, Type enumtrParamType, Type enumtrType, Type? valueRevealType = null)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var shouldUnbox = !typeof(TEnumtr).IsValueType && enumtrType.IsValueType;
        
        valueRevealType  ??= typeof(PalantírReveal<TValue>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateNullValueRevealerInvoke_{enumtrType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), typeof(TEnumtr), valueRevealType
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllIterateExtensions).Module, false);
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

        // call WhenPopulatedAddAllIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit( shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(ValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TKey, TValue>));
        var fullInvoker   = (ValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TKey, TValue>)methodInvoker;
        
        TMold Wrapped( SelectTypeKeyedCollectionField<TMold> keyCollMold, string fieldName, TEnumtr? enumtr, PalantírReveal<TValue> valueRevealer
          , string? keyFmtStr, string? valueFmtStr, FormatFlags flags ) => 
            fullInvoker(keyCollMold, fieldName,  enumtr, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetWhenPopulatedAddAllBothRevealersNoNullableStructMethodInfo<TMold, TKRevealerBase, TVRevealerBase>
        (this Type enumtrType)
        where TMold : TypeMolder
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var moldType     = typeof(TMold);
        var tKRevealBase = typeof(TKRevealerBase);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((moldType, enumtrType, tKRevealBase, tvRevealBase),
                 static ((Type moldType, Type enumtrType, Type tkRevealType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(WhenPopulatedAddAllIterateBothRevealers)
                            , [ key.moldType, key.enumtrType, keyType, valueType, key.tkRevealType, key.tvRevealType ],
                              [
                                  typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                                , typeof(PalantírReveal<TVRevealerBase>)
                                , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKRevealerBase, TVRevealerBase> 
        GetWhenPopulatedAddAllBothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKRevealerBase, TVRevealerBase>
        (Type enumtrType)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumtrParamType = typeof(TEnumtr);
        var tKRevealBase    = typeof(TKRevealerBase);
        var tvRevealBase    = typeof(TVRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache.GetOrAdd
                ( (moldType, enumtrParamType, enumtrType, tKRevealBase, tvRevealBase),
                   static ((Type moldType, Type enumtrParamType, Type enumtrType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                   {
                       var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                       if (kvpTypes == null)
                           throw new ArgumentException("Expected to receive a KeyValue enumerator");
                       var keyType    = kvpTypes.Value.Key;
                       var valueType  = kvpTypes.Value.Value;
                       var toInvokeOn = 
                           GetStaticMethodInfo
                               (nameof(WhenPopulatedAddAllIterateBothRevealers)
                              , [ key.moldType, key.enumtrType, keyType, valueType, key.tkRevealType, key.tvRevealType ],
                                [
                                    typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                                  , typeof(PalantírReveal<TVRevealerBase>)
                                  , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                  , typeof(FormatFlags)
                                ]);


                       var genGenMethod
                           = myMethodInfosCached!
                               .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllBothRevealersNoNullableStructInvoker)));
                       var concreteGenMethod
                           = genGenMethod.MakeGenericMethod([
                               key.moldType, key.enumtrParamType, keyType, valueType, key.tkRevealType, key.tvRevealType
                           ]);
                       return 
                           (BothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKRevealerBase, TVRevealerBase>)
                           concreteGenMethod.Invoke(null, 
                           [ toInvokeOn, key.enumtrType
                         , new[]
                           {
                               typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrParamType
                             , typeof(PalantírReveal<TVRevealerBase>)
                             , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                             , typeof(FormatFlags)
                           }
                       ])!;
                   }, callAsFactory);
        return invoker;
    }


    internal static BothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKRevealerBase, TVRevealerBase> 
        BuildWhenPopulatedAddAllBothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKey, TValue, TKRevealerBase, TVRevealerBase>
        (MethodInfo methodInfo, Type enumtrType, Type[] parameterArgTypes)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealerBase?
        where TValue : TVRevealerBase?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !typeof(TEnumtr).IsValueType && enumtrType.IsValueType;
        
        var signatureName = $"{parameterArgTypes[2].Name}_{parameterArgTypes[3].Name}";
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateBothRevealers_{parameterArgTypes[1].Name}_{signatureName}"
               , typeof(TMold), parameterArgTypes, typeof(WhenPopulatedAddAllIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
        if (shouldUnbox)
        {
            // Make space for enumtrType unboxed local variable
            var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);

            // unbox TEnumtr value => (enumtrType)value
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Unbox_Any, enumtrLocalType.LocalType);
            ilGenerator.Emit(OpCodes.Stloc_0);
        }

        // call WhenPopulatedAddAllIterateBothRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
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
            helperMethod.CreateDelegate(typeof(BothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKey, TValue, TKRevealerBase, TVRevealerBase>));
        var createdInvoker = (BothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKey, TValue, TKRevealerBase, TVRevealerBase>)methodInvoker;

        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumtr enumtr, PalantírReveal<TVRevealerBase> vRevealer
          , PalantírReveal<TKRevealerBase> kRevealer, string? valueFmtString, FormatFlags flags) =>
            createdInvoker(kcm, fieldName, enumtr, vRevealer, kRevealer, valueFmtString, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetWhenPopulatedAddAllBothRevealersNullableKeyStructMethodInfo<TMold, TKey, TVRevealerBase>(this Type enumtrType)
        where TMold : TypeMolder
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var moldType     = typeof(TMold);
        var tKey         = typeof(TKey);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNullableKeyStructMethodInfoCache.GetOrAdd
                ((moldType, enumtrType, tKey, tvRevealBase),
                 static ((Type moldType, Type enumtrType, Type tkeyType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(WhenPopulatedAddAllIterateBothWithNullKeyRevealers)
                            , [ key.moldType, key.enumtrType, keyType.IfNullableGetUnderlyingTypeOrThisCached()
                                , valueType, key.tvRevealType ],
                              [
                                  typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                                , typeof(PalantírReveal<TVRevealerBase>), typeof(PalantírReveal<TKey>), typeof(string), typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TVRevealerBase> 
        GetWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TVRevealerBase>(Type enumtrType)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var moldType         = typeof(TMold);
        var enumtrParamType  = typeof(TEnumtr);
        var tKeyType         = typeof(TKey);
        var tvRevealBaseType = typeof(TVRevealerBase);
        var callAsFactory    = true;
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TVRevealerBase>)
            BothRevealersNullableKeyStructInvokerCache
                .GetOrAdd
                    ((moldType, enumtrParamType, enumtrType, tKeyType, tvRevealBaseType),
                        static ((Type moldType, Type enumtrParamType, Type enumtrType, Type tKey, Type tvRevealType) key, bool _) =>
                        {
                            var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                            if (kvpTypes == null)
                                throw new ArgumentException("Expected to receive a KeyValue enumerator");
                            var valueType = kvpTypes.Value.Value;
                            var toInvokeOn =
                                GetStaticMethodInfo
                                    (nameof(WhenPopulatedAddAllIterateBothWithNullKeyRevealers)
                                      , [key.moldType, key.enumtrType, key.tKey, valueType, key.tvRevealType],
                                        [
                                            typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                                          , typeof(PalantírReveal<TVRevealerBase>)
                                          , typeof(PalantírReveal<TKey>), typeof(string)
                                          , typeof(FormatFlags)
                                      ]);

                            var genGenMethod
                                = myMethodInfosCached!
                                    .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker)));
                            var concreteGenMethod
                                = genGenMethod.MakeGenericMethod([key.moldType, key.enumtrParamType, key.tKey, valueType, key.tvRevealType ]);
                            return (BothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TVRevealerBase>)
                                concreteGenMethod.Invoke(null, [toInvokeOn, key.enumtrType
                                                        , typeof(PalantírReveal<TKey>), typeof(PalantírReveal<TVRevealerBase>)
                            ])!;
                        }, callAsFactory);
        return invoker;
    }

    internal static BothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TVRevealerBase> 
        BuildWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TValue, TVRevealerBase>
        (MethodInfo methodInfo, Type enumtrType, Type? tKeyRevealType = null, Type? tvRevealBaseType = null)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        var shouldUnbox = !typeof(TEnumtr).IsValueType && enumtrType.IsValueType;
        
        tKeyRevealType   ??= typeof(PalantírReveal<TKey>);
        tvRevealBaseType ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateBothRevealers_{enumtrType.Name}", typeof(TMold),
                 [typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), typeof(TEnumtr), tvRevealBaseType
                   , tKeyRevealType, typeof(string), typeof(FormatFlags)]
               , typeof(WhenPopulatedAddAllIterateExtensions).Module, false);
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
        
        // call WhenPopulatedAddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker  = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TValue, TVRevealerBase>));
        var createdInvoker = (BothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TValue, TVRevealerBase>)methodInvoker;
        
        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumtr? enumtr
          , PalantírReveal<TVRevealerBase> vRevealer, PalantírReveal<TKey> kRevealer, string? valueFmtString, FormatFlags flags) =>
            createdInvoker(kcm, fieldName, enumtr, vRevealer, kRevealer, valueFmtString, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetWhenPopulatedAddAllBothRevealersNullableValueStructMethodInfo<TMold, TValue, TKRevealerBase>(this Type enumtrType)
        where TMold : TypeMolder
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var moldType     = typeof(TMold);
        var tValue       = typeof(TValue);
        var tkRevealBase = typeof(TKRevealerBase);
        var methInf =
            BothRevealersNullableValueStructMethodInfoCache.GetOrAdd
                ((moldType, enumtrType, tValue, tkRevealBase),
                 static ((Type moldType, Type enumtrType, Type tvalueType, Type tkRevealBase) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;

                     var acceptEnumtrType = key.enumtrType;
                     if (acceptEnumtrType.IsValueType && !acceptEnumtrType.IsNullable())
                     {
                         acceptEnumtrType = typeof(Nullable<>).MakeGenericType(acceptEnumtrType);
                     } 
                     return
                         GetStaticMethodInfo
                             (nameof(WhenPopulatedAddAllIterateBothWithNullValueRevealers)
                            , [ key.moldType, key.enumtrType, keyType, key.tvalueType, key.tkRevealBase ]
                             , [ typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), acceptEnumtrType
                                , typeof(PalantírReveal<TValue>), typeof(PalantírReveal<TKRevealerBase>), typeof(string), typeof(FormatFlags)]);
                 });
        return methInf;
    }

    internal static BothRevealersNullableValueStructInvoker<TMold, TEnumtr, TValue, TKRevealerBase> 
        GetWhenPopulatedAddAllBothRevealersNullableValueStructInvoker<TMold, TEnumtr, TValue, TKRevealerBase>(Type enumtrType)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var moldType        = typeof(TMold);
        var enumtrParamType = typeof(TEnumtr);
        var tValueType      = typeof(TValue);
        var tkRevealBase    = typeof(TKRevealerBase);
        var callAsFactory   = true;
        var invoker =
            (BothRevealersNullableValueStructInvoker<TMold, TEnumtr, TValue, TKRevealerBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((moldType, enumtrParamType, enumtrType, tValueType, tkRevealBase),
                      static ((Type moldType, Type enumtrParamType, Type enumtrType, Type valueType, Type tkRevealBase) key, bool _) =>
                      {
                          var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                          if (kvpTypes == null)
                              throw new ArgumentException("Expected to receive a KeyValue enumerator");
                          var keyType = kvpTypes.Value.Key;
                          var toInvokeOn =
                              GetStaticMethodInfo
                                  (nameof(WhenPopulatedAddAllIterateBothWithNullValueRevealers)
                                    , [key.moldType, key.enumtrType, keyType, key.valueType, key.tkRevealBase],
                                      [
                                          typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                                        , typeof(PalantírReveal<TValue>), typeof(PalantírReveal<TKRevealerBase>)
                                        , typeof(string), typeof(FormatFlags)
                                      ]);

                          var genGenMethod
                              = myMethodInfosCached!
                                  .First(mi => mi.Name.Contains(nameof(BuildWhenPopulatedAddAllBothRevealersNullableValueStructInvoker)));
                          var concreteGenMethod
                              = genGenMethod.MakeGenericMethod([key.moldType, key.enumtrParamType, keyType, key.valueType, key.tkRevealBase ]);
                          return (BothRevealersNullableValueStructInvoker<TMold, TEnumtr, TValue, TKRevealerBase>)
                              concreteGenMethod.Invoke(null
                                        , [toInvokeOn, key.enumtrType, typeof(PalantírReveal<TKRevealerBase>), typeof(PalantírReveal<TValue>)])!;
                      }, callAsFactory);
        return invoker;
    }

    internal static BothRevealersNullableValueStructInvoker<TMold, TEnumtr, TValue, TKRevealerBase> 
        BuildWhenPopulatedAddAllBothRevealersNullableValueStructInvoker<TMold, TEnumtr, TKey, TValue, TKRevealerBase>
        (MethodInfo methodInfo, Type enumtrType, Type? tkRevealBase = null, Type? tvRevealType = null)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var shouldUnbox = !typeof(TEnumtr).IsValueType && enumtrType.IsValueType;
        
        tkRevealBase ??= typeof(PalantírReveal<TKRevealerBase>);
        tvRevealType ??= typeof(PalantírReveal<TValue>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicWhenPopulatedAddAllIterateBothRevealers_{enumtrType.Name}", typeof(TMold),
                [
                    typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), typeof(TEnumtr), tvRevealType
                  , tkRevealBase, typeof(string), typeof(FormatFlags)
                ], typeof(WhenPopulatedAddAllIterateExtensions).Module, false);
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
        
        // call WhenPopulatedAddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(shouldUnbox ? OpCodes.Ldloc_0 : OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker  = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TMold, TEnumtr, TKey, TValue, TKRevealerBase>));
        var createdInvoker = (BothRevealersNullableValueStructInvoker<TMold, TEnumtr, TKey, TValue, TKRevealerBase>)methodInvoker;
        
        TMold Wrapped(SelectTypeKeyedCollectionField<TMold> kcm, string fieldName, TEnumtr? enumtr, PalantírReveal<TValue> vRevealer
          , PalantírReveal<TKRevealerBase> kRevealer, string? valueFmtString, FormatFlags flags) =>
            createdInvoker(kcm, fieldName, enumtr, vRevealer, kRevealer, valueFmtString, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetWhenPopulatedAddAllBothNullRevealersMethodInfo<TMold, TKey, TValue>(this Type enumtrType)
        where TMold : TypeMolder
        where TKey : struct
        where TValue : struct
    {
        var moldType = typeof(TMold);
        var tKey     = typeof(TKey);
        var tValue   = typeof(TValue);
        var methInf =
            BothRevealersBothNullableStructsMethodInfoCache.GetOrAdd
                ((moldType, enumtrType, tValue, tKey),
                 static ((Type moldType, Type enumtrType, Type tvalueType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(WhenPopulatedAddAllIterateBothNullRevealers)
                            , [ key.moldType,  key.enumtrType, keyType.IfNullableGetUnderlyingTypeOrThisCached()
                                , valueType.IfNullableGetUnderlyingTypeOrThisCached() ],
                              [
                                  typeof(SelectTypeKeyedCollectionField<TMold>), typeof(string), key.enumtrType
                                , typeof(PalantírReveal<TValue>), typeof(PalantírReveal<TKey>), typeof(string), typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(WhenPopulatedAddAllIterateExtensions).GetMethods( NonPublic | Public | Static);

        MethodInfo? genTypeDefMeth   = null;
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
            var checkEnumtrType    = checkParameterInfos[2].ParameterType;
            if ((!findEnumtrType.IsNullable() && checkEnumtrType.IsNullable())
                || (findEnumtrType.IsNullable() && !checkEnumtrType.IsNullable())) continue;
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

    public static TMold WhenPopulatedAddAllIterate<TMold, TEnumtr>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetWhenPopulatedAddAllNoRevealersInvoker<TMold, TEnumtr?>(actualType);
        return invoker(callOn, fieldName, value, valueFormatString, keyFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterate<TMold, TEnumtr>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllNoRevealersInvoker<TMold, TEnumtr>(actualType);
        return invoker(callOn, fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterate<TMold, TEnumtr, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
    {
        if (value == null) return callOn.Mold;
        return callOn.WhenPopulatedAddAllIterate<TMold, TEnumtr, TKey, TValue>(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterate<TMold, TEnumtr, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            mws.FieldNameJoin(fieldName);
            var ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>
                (value! , new CreateContext(formatFlags: formatFlags, displayAsType: callOn.CollectionType));
            var kvp  = value!.Current;
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
            ekcm.AddAllIterate<TEnumtr, TKey, TValue>(value, valueFormatString, keyFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        value?.Dispose();
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllIterateValueRevealer<TMold, TEnumtr, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetWhenPopulatedAddAllValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateValueRevealer<TMold, TEnumtr, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllValueRevealerNoNullableStructInvoker<TMold, TEnumtr, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateValueRevealer<TMold, TEnumtr, TKey, TValue, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn.Mold;
        return callOn.WhenPopulatedAddAllIterateValueRevealer<TMold, TEnumtr, TKey, TValue, TVRevealBase>
            (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateValueRevealer<TMold, TEnumtr, TKey, TValue, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            mws.FieldNameJoin(fieldName);
            var ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>
                (value! , new CreateContext(formatFlags: formatFlags, displayAsType: callOn.CollectionType));
            var kvp  = value!.Current;
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        value?.Dispose();
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllIterateNullValueRevealer<TMold, TEnumtr, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetWhenPopulatedAddAllValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TValue>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateNullValueRevealer<TMold, TEnumtr, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllValueRevealerNullableValueStructInvoker<TMold, TEnumtr, TValue>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateNullValueRevealer<TMold, TEnumtr, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TValue : struct
    {
        if (value == null) return callOn.Mold;
        return callOn.WhenPopulatedAddAllIterateNullValueRevealer<TMold, TEnumtr, TKey, TValue>
            (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateNullValueRevealer<TMold, TEnumtr, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            mws.FieldNameJoin(fieldName);
            var ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>
                (value! , new CreateContext(formatFlags: formatFlags, displayAsType: callOn.CollectionType));
            var kvp  = value!.Current;
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        value?.Dispose();
        return callOn.Mold;
    }
    
    public static TMold WhenPopulatedAddAllIterateBothRevealers<TMold, TEnumtr, TKRevealBase, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetWhenPopulatedAddAllBothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value!, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothRevealers<TMold, TEnumtr, TKRevealBase, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllBothRevealersNoNullableStructInvoker<TMold, TEnumtr, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothRevealers<TMold, TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn.Mold;
        return callOn.WhenPopulatedAddAllIterateBothRevealers(fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothRevealers<TMold, TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            mws.FieldNameJoin(fieldName);
            var ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>
                (value! , new CreateContext(formatFlags: formatFlags, displayAsType: callOn.CollectionType));
            var kvp  = value!.Current;
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            ekcm.AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        value?.Dispose();
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TMold, TEnumtr, TKey, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TMold, TEnumtr, TKey, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator
        where TKey : struct
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllBothRevealersNullableKeyStructInvoker<TMold, TEnumtr, TKey, TVRevealBase>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TMold, TEnumtr, TKey, TValue, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value == null) return callOn.Mold;
        return callOn.WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TMold, TEnumtr, TKey, TValue, TVRevealBase>
            (fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TMold, TEnumtr, TKey, TValue, TVRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            mws.FieldNameJoin(fieldName);
            var ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>
                (value! , new CreateContext(formatFlags: formatFlags, displayAsType: callOn.CollectionType));
            var kvp  = value!.Current;
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            ekcm.AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllIterateBothWithNullValueRevealers<TMold, TEnumtr, TValue, TKRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetWhenPopulatedAddAllBothRevealersNullableValueStructInvoker<TMold, TEnumtr, TValue, TKRevealBase>(actualType);
        return invoker(callOn, fieldName, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothWithNullValueRevealers<TMold, TEnumtr, TValue, TKRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn.Mold;
        var invoker = GetWhenPopulatedAddAllBothRevealersNullableValueStructInvoker<TMold, TEnumtr, TValue, TKRevealBase>(actualType);
        return invoker(callOn, fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothWithNullValueRevealers<TMold, TEnumtr, TKey, TValue, TKRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value == null) return callOn.Mold;
        return callOn.WhenPopulatedAddAllIterateBothWithNullValueRevealers<TMold, TEnumtr, TKey, TValue, TKRevealBase>
            (fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothWithNullValueRevealers<TMold, TEnumtr, TKey, TValue, TKRevealBase>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            mws.FieldNameJoin(fieldName);
            var ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>
                (value! , new CreateContext(formatFlags: formatFlags, displayAsType: callOn.CollectionType));
            var kvp  = value!.Current;
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            ekcm.AddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }

    public static TMold WhenPopulatedAddAllIterateBothNullRevealers<TMold, TEnumtr, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct
    {
        if (value == null) return callOn.Mold;
        return callOn.WhenPopulatedAddAllIterateBothNullRevealers(fieldName, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static TMold WhenPopulatedAddAllIterateBothNullRevealers<TMold, TEnumtr, TKey, TValue>(
        this SelectTypeKeyedCollectionField<TMold> callOn
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TMold : TypeMolder
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = callOn.Mws;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            mws.FieldNameJoin(fieldName);
            var ekcm = mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>
                (value! , new CreateContext(formatFlags: formatFlags, displayAsType: callOn.CollectionType));
            var kvp  = value!.Current;
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            ekcm.AddAllIterateBothNullRevealers(value, valueRevealer, keyStyler, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        return callOn.Mold;
    }
}
