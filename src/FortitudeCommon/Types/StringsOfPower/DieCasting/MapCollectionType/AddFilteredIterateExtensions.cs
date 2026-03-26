// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public static class KeyedCollectionAddFilteredIterateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly Type GenericEnumerator = typeof(IEnumerator<>);


    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> NoRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate>   NoRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo> ValueRevealerNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>   ValueRevealerNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo>           ValueRevealerNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ValueRevealerNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), MethodInfo> BothRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo> BothRevealersNullableKeyStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>   BothRevealersNullableKeyStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo> BothRevealersNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate>   BothRevealersNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> BothRevealersBothNullableStructsMethodInfoCache = new();


    // ReSharper disable twice TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold NoRevealersNoNullableStructInvoker<in TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?;
    

    // ReSharper disable twice TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold NoRevealersNoNullableStructInvoker<in TEnumtr, TKFilterBase, TVFilterBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?;
    
    // ReSharper disable twice TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold CreateValueRevealerNoNullableStructInvoker<in TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVRevealBase?, TVFilterBase?
        where TVRevealBase : notnull;

    // ReSharper disable twice TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<in TEnumtr, TKFilterBase, TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull;
    
    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold CreateValueRevealerNullableValueStructInvoker<in TEnumtr, TKey, TValue, TKFilterBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<in TEnumtr, TValue, TKFilterBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct;

    internal delegate KeyedCollectionMold CreateBothRevealersNoNullableStructInvoker<in TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase,
                                                                                     out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold CallBothRevealersNoNullableStructInvoker<in TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, 
                                                                                   out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumtr, TKFilterBase, TVFilterBase, out TKRevealBase, out TVRevealBase>(
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

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableKeyStructInvoker<in TEnumtr, TKey, TVFilterBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<in TEnumtr, TValue, TKFilterBase, out TKRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealBase : notnull;
    
    internal static MethodInfo GetAddFilteredNoRevealersInvokerMethodInfo<TKFilterBase, TVFilterBase>(this Type enumtrType)
    {
        var tkFilterType = typeof(TKFilterBase);
        var tvFilterType = typeof(TVFilterBase);
        var methInf =
            NoRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tkFilterType, tvFilterType)
               , static ((Type enumeratorType, Type tkFilterType, Type tvFilterType) key )=>
                 {
                     var kvpTypes = key.enumeratorType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(AddFilteredIterate)
                            , [ key.enumeratorType, keyType, valueType, key.tkFilterType, key.tvFilterType],
                              [
                                  typeof(KeyedCollectionMold), key.enumeratorType
                                , typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                                , typeof(string), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static NoRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase> GetAddFilteredNoRevealersInvoker
    <TEnumtr, TKFilterBase, TVFilterBase>(Type? enumtrType = null)
        where TEnumtr : IEnumerator?
    {
        enumtrType ??= typeof(TEnumtr);
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var callAsFactory = true;
        var invoker =
            (NoRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase>)NoRevealersNoNullableStructInvokerCache
                .GetOrAdd((enumtrType, tkFilterType, tvFilterType), static ((Type enumtrType, Type tkFilterType, Type tvFilterType) key, bool _) =>
            {
                var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                var keyType   = kvpTypes.Value.Key;
                var valueType = kvpTypes.Value.Value;
                var toInvokeOn = 
                    GetStaticMethodInfo(nameof(AddFilteredIterate)
                                      , [key.enumtrType, keyType, key.tvFilterType, key.tvFilterType, valueType],
                [ typeof(KeyedCollectionMold), key.enumtrType, typeof(string), typeof(string), typeof(FormatFlags) ]);

                var genGenMethod
                    = myMethodInfosCached!
                        .First(mi => mi.Name.Contains(nameof(BuildAddFilteredNoRevealersNoNullableStructInvoker)));
                var concreteGenMethod
                    = genGenMethod.MakeGenericMethod([typeof(TEnumtr), keyType, valueType, key.tvFilterType, key.tvFilterType ]);
                return (NoRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase>)concreteGenMethod.Invoke(null, [ toInvokeOn, key.enumtrType ])!;
            }, callAsFactory);
        return invoker;
    }

    internal static NoRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase> BuildAddFilteredNoRevealersNoNullableStructInvoker
        <TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(MethodInfo methodInfo, Type? enumtrType = null)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?
    {
        enumtrType ??= typeof(TEnumtr);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumtr), typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>), typeof(string)
                   , typeof(string), typeof(FormatFlags)], typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
        // Make space for enumtrType local variables
        var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);
        
        // cast TEnumtr value => (enumtrType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumtrLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);
        
        // call AddFilteredIterate(KeyedCollectionMold, TEnumtr, filterPredicate,  valueFmtStr, keyFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>));
        var createInvoker = (NoRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>)methodInvoker;
        
        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumtr? enumtr, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
          , string? valueFmtStr, string? keyFmtString, FormatFlags flags) => createInvoker(kcm, enumtr, filterPredicate, valueFmtStr, keyFmtString, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetAddFilteredValueRevealerNoNullableStructIInvokerMethodInfo<TKFilterBase, TVFilterBase, TVRevealerBase>(this Type enumtrType)
    where TVRevealerBase : notnull
    {
        var tvRevealerBaseType = typeof(TVRevealerBase);
        var tkFilterType       = typeof(TKFilterBase);
        var tvFilterType       = typeof(TVFilterBase);
        var methInf =
            ValueRevealerNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tkFilterType, tvFilterType, tvRevealerBaseType), 
                 static ((Type enumeratorType, Type tkFilterType, Type tvFilterType, Type tvRevealerType) key, bool _) =>
                {
                    var kvpTypes = key.enumeratorType.GetKeyedCollectionTypes();
                    if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                    var keyType   = kvpTypes.Value.Key;
                    var valueType = kvpTypes.Value.Value;
                    return
                        GetStaticMethodInfo
                            (nameof(AddFilteredIterateValueRevealer)
                           , [ key.enumeratorType, keyType, valueType, key.tkFilterType, key.tvFilterType, key.tvRevealerType],
                             [
                                 typeof(KeyedCollectionMold), key.enumeratorType
                               , typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)  
                               , typeof(PalantírReveal<TVRevealerBase>), typeof(string), typeof(string)
                               , typeof(FormatFlags)
                             ]);
                }, true);
        return methInf;
    }

    internal static ValueRevealerNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase> 
        GetAddFilteredValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(
        Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumtr);
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var tvRevealBase  = typeof(TVRevealerBase);
        var callAsFactory = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumtrType, tkFilterType, tvFilterType, tvRevealBase),
                       static ((Type enumtrType, Type tkFilterType, Type tvFilterType, Type tvRevealType) key, bool _) =>
                       {
                           var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                           if (kvpTypes == null)
                               throw new ArgumentException("Expected to receive a KeyValue enumerator");
                           var keyType   = kvpTypes.Value.Key;
                           var valueType = kvpTypes.Value.Value;
                           var toInvokeOn =
                               GetStaticMethodInfo
                                   (nameof(AddFilteredIterateValueRevealer)
                                 , [key.enumtrType, keyType, valueType, key.tkFilterType, key.tvFilterType, key.tvRevealType],
                                   [
                                       typeof(KeyedCollectionMold), typeof(TEnumtr)
                                     , typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)  
                                     , typeof(PalantírReveal<TVRevealerBase>)
                                     , typeof(string), typeof(string), typeof(FormatFlags)
                                   ]);

                           var genGenMethod = myMethodInfosCached!
                               .First(mi => mi.Name.Contains(nameof(BuildAddFilteredValueRevealerNoNullableStructInvoker)));
                           var concreteGenMethod = 
                               genGenMethod
                                   .MakeGenericMethod( [typeof(TEnumtr), keyType, valueType, key.tvFilterType, key.tvFilterType, key.tvRevealType ]);
                           return (ValueRevealerNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>)
                               concreteGenMethod.Invoke(null, [ toInvokeOn, key.enumtrType, key.tvRevealType ])!;
                       }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase> BuildAddFilteredValueRevealerNoNullableStructInvoker
        <TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>(MethodInfo methodInfo, Type? enumtrType = null, Type? tvRevealBase = null)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>? 
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealerBase?
        where TVRevealerBase : notnull
    {
        enumtrType   ??= typeof(TEnumtr);
        tvRevealBase ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumtr), tvRevealBase, typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumtrType local variables
        var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);
        
        // cast TEnumtr value => (enumtrType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumtrLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);
        
        // call AddFilteredIterateValueRevealer(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CreateValueRevealerNoNullableStructInvoker<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>)methodInvoker;
        
        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumtr? enumtr, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
          , PalantírReveal<TVRevealerBase> valueRevealer, string? keyFmtStr, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(kcm, enumtr, filterPredicate, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetAddFilteredValueRevealerNullableValueStructMethodInfo<TValue, TKFilterBase>(this Type enumtrType)
        where TValue : struct
    {
        var tValue       = typeof(TValue);
        var tkFilterType = typeof(TKFilterBase);
        var methInf =
            ValueRevealerNullableValueStructMethodInfoCache.GetOrAdd
                ((enumtrType,  tValue, tkFilterType),
                 static ((Type enumtrType, Type tvalueType, Type tkFilterType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     return
                         GetStaticMethodInfo
                             (nameof(AddFilteredIterateNullValueRevealer)
                            , [ key.enumtrType, keyType, key.tvalueType, key.tkFilterType],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(KeyValuePredicate<TKFilterBase, TValue?>)  
                                , typeof(PalantírReveal<TValue>)
                                , typeof(string), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static ValueRevealerNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase> 
        GetAddFilteredValueRevealerNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase>(
        Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        enumtrType ??= typeof(TEnumtr);
        var tkFilterType = typeof(TKFilterBase);
        var tvRevealBase = typeof(TValue);
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase>)
            ValueRevealerNullableValueStructInvokerCache.GetOrAdd
                ((enumtrType, tkFilterType, tvRevealBase)
                , static (Type enumtrType, Type tkFilterType, Type tvRevealType) =>
                 {
                     var kvpTypes = enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     var toInvokeOn =
                         GetStaticMethodInfo(nameof(AddFilteredIterateNullValueRevealer), [enumtrType, keyType, valueType, tkFilterType, tvRevealType],
                         [
                             typeof(KeyedCollectionMold), typeof(TEnumtr), typeof(PalantírReveal<TValue>)
                           , typeof(string), typeof(string), typeof(FormatFlags)
                         ]);

                     var genGenMethod
                         = myMethodInfosCached!
                             .First(mi => mi.Name.Contains(nameof(BuildAddFilteredValueRevealerNullableValueStructInvoker)));
                     var concreteGenMethod
                         = genGenMethod.MakeGenericMethod([
                             enumtrType, keyType, tkFilterType, valueType
                         ]);
                     return (ValueRevealerNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase>)concreteGenMethod.Invoke(null, [
                         toInvokeOn
                       , new[]
                         {
                             typeof(KeyedCollectionMold), typeof(TEnumtr)
                           , typeof(PalantírReveal<TValue>)
                           , typeof(string), typeof(string)
                           , typeof(FormatFlags)
                         }])!;
                   });
        return invoker;
    }

    internal static ValueRevealerNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase> BuildAddFilteredValueRevealerNullableValueStructInvoker
        <TEnumtr, TKey, TValue, TKFilterBase>
        (MethodInfo methodInfo, Type? enumtrType = null, Type? valueType = null)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        enumtrType ??= typeof(TEnumtr);
        valueType  ??= typeof(PalantírReveal<TValue>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateNullValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumtr), typeof(KeyValuePredicate<TKFilterBase, TValue?>),  valueType
                   , typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumtrType local variables
        var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);
        
        // cast TEnumtr value => (enumtrType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumtrLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);
        
        // call AddFilteredIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CreateValueRevealerNullableValueStructInvoker<TEnumtr, TKey, TValue, TKFilterBase>));
        var fullInvoker   = (CreateValueRevealerNullableValueStructInvoker<TEnumtr, TKey, TValue, TKFilterBase>)methodInvoker;
        
        KeyedCollectionMold Wrapped( KeyedCollectionMold keyCollMold, TEnumtr? enumtr, KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
          , PalantírReveal<TValue> valueRevealer, string? keyFmtStr, string? valueFmtStr, FormatFlags flags ) => 
            fullInvoker(keyCollMold, enumtr, filterPredicate, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetAddFilteredBothRevealersMethodInfo<TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>(this Type enumtrType)
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var tkFilterType = typeof(TKFilterBase);
        var tvFilterType = typeof(TVFilterBase);
        var tKRevealBase = typeof(TKRevealerBase);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tkFilterType, tvFilterType, tKRevealBase, tvRevealBase),
                 static ((Type enumtrType, Type tkFilterType, Type tvFilterType, Type tkRevealType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(AddFilteredIterateBothRevealers)
                            , [ key.enumtrType, keyType, valueType, key.tkFilterType, key.tvFilterType, key.tkRevealType, key.tvRevealType ],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                                , typeof(PalantírReveal<TVRevealerBase>)
                                , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase> GetAddFilteredBothRevealersInvoker
        <TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>(Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumtr);
        var tkFilterType  = typeof(TKFilterBase);
        var tvFilterType  = typeof(TVFilterBase);
        var tKRevealBase  = typeof(TKRevealerBase);
        var tvRevealBase  = typeof(TVRevealerBase);
        var callAsFactory = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache.GetOrAdd
                ( (enumtrType, tkFilterType, tvFilterType, tKRevealBase, tvRevealBase),
                   static ((Type enumtrType, Type tkFilterType, Type tvFilterType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                   {
                       var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                       if (kvpTypes == null)
                           throw new ArgumentException("Expected to receive a KeyValue enumerator");
                       var keyType    = kvpTypes.Value.Key;
                       var valueType  = kvpTypes.Value.Value;
                       var toInvokeOn = 
                           GetStaticMethodInfo
                               (nameof(AddFilteredIterateBothRevealers)
                              , [ key.enumtrType, keyType, valueType, key.tkFilterType, key.tvRevealType, key.tkRevealType, key.tvRevealType ],
                                [
                                    typeof(KeyedCollectionMold), typeof(TEnumtr)
                                  , typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)  
                                  , typeof(PalantírReveal<TVRevealerBase>)
                                  , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                  , typeof(FormatFlags)
                                ]);


                       var genGenMethod
                           = myMethodInfosCached!
                               .First(mi => mi.Name.Contains(nameof(BuildAddFilteredBothRevealersInvoker)));
                       var concreteGenMethod
                           = genGenMethod.MakeGenericMethod([
                               typeof(TEnumtr), keyType, valueType, key.tkFilterType, key.tvFilterType, key.tkRevealType, key.tvRevealType
                           ]);
                       return (BothRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase,
                           TVRevealerBase>)concreteGenMethod.Invoke(null, [
                           toInvokeOn
                         , new[]
                           {
                               typeof(KeyedCollectionMold), key.enumtrType
                             , typeof(KeyValuePredicate<TKFilterBase, TVFilterBase>)
                             , typeof(PalantírReveal<TVRevealerBase>)
                             , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                             , typeof(FormatFlags)
                           }
                       ])!;
                   }, callAsFactory);
        return invoker;
    }


    internal static BothRevealersNoNullableStructInvoker<TEnumtr, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase> 
        BuildAddFilteredBothRevealersInvoker<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>
        (MethodInfo methodInfo, Type[] parameterArgTypes)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealerBase?
        where TValue : TVRevealerBase?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var signatureName = $"{parameterArgTypes[2].Name}_{parameterArgTypes[3].Name}";
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{parameterArgTypes[1].Name}_{signatureName}"
               , typeof(KeyedCollectionMold), parameterArgTypes, typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        
        // call AddFilteredIterateBothRevealers(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate(typeof(CreateBothRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>));
        var createdInvoker = (CreateBothRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumtr enumtr, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
          , PalantírReveal<TVRevealerBase> vRevealer, PalantírReveal<TKRevealerBase> kRevealer, string? valueFmtString, FormatFlags flags) =>
            createdInvoker(kcm, enumtr, filterPredicate, vRevealer, kRevealer, valueFmtString, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetAddFilteredBothRevealersNullableKeyStructMethodInfo<TKey, TVFilterBase, TVRevealerBase>(this Type enumtrType)
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var tKey         = typeof(TKey);
        var tvFilterType = typeof(TVFilterBase);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNullableKeyStructMethodInfoCache.GetOrAdd
                ((enumtrType, tKey, tvFilterType, tvRevealBase),
                 static ((Type enumtrType, Type tkeyType, Type tvFilterType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(AddFilteredIterateBothWithNullKeyRevealers)
                            , [ key.enumtrType, keyType.IfNullableGetUnderlyingTypeOrThisCached()
                                , valueType, key.tvFilterType, key.tvRevealType ],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(KeyValuePredicate<TKey?, TVFilterBase>)
                                , typeof(PalantírReveal<TVRevealerBase>)
                                , typeof(PalantírReveal<TKey>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVFilterBase, TVRevealerBase> GetAddFilteredBothRevealersNullableKeyStructInvoker
        <TEnumtr, TKey, TVFilterBase, TVRevealerBase>(Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumtr);
        var tKeyType         = typeof(TKey);
        var tvFilterType     = typeof(TVFilterBase);
        var tvRevealBaseType = typeof(TVRevealerBase);
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVFilterBase, TVRevealerBase>)
            BothRevealersNullableKeyStructInvokerCache
                .GetOrAdd
                    ((enumtrType, tKeyType, tvFilterType, tvRevealBaseType),
                        static (Type enumtrType, Type tKey, Type _, Type _, Type tvRevealType) =>
                        {
                            var kvpTypes = enumtrType.GetKeyedCollectionTypes();
                            if (kvpTypes == null)
                                throw new ArgumentException("Expected to receive a KeyValue enumerator");
                            var valueType = kvpTypes.Value.Value;
                            var toInvokeOn =
                                GetStaticMethodInfo
                                    (nameof(AddFilteredIterateBothWithNullKeyRevealers)
                                      , [enumtrType, tKey, valueType, tvRevealType],
                                        [
                                            typeof(KeyedCollectionMold), typeof(IEnumerator)
                                          , typeof(KeyValuePredicate<TKey?, TVFilterBase>)
                                          , typeof(PalantírReveal<TVRevealerBase>)
                                          , typeof(PalantírReveal<TKey>), typeof(string)
                                          , typeof(FormatFlags)
                                      ]);

                            return
                                BuildAddFilteredBothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVFilterBase, TVRevealerBase>
                                    (toInvokeOn, enumtrType, typeof(PalantírReveal<TKey>), typeof(PalantírReveal<TVRevealerBase>));
                        });
        return invoker;
    }

    internal static BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVFilterBase, TVRevealerBase> BuildAddFilteredBothRevealersNullableKeyStructInvoker
        <TEnumtr, TKey, TVFilterBase, TVRevealerBase>(MethodInfo methodInfo, Type? enumtrType = null, Type? tKeyRevealType = null, Type? tvRevealBaseType = null)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        enumtrType       ??= typeof(TEnumtr);
        var filterPredicate  = typeof(KeyValuePredicate<TKey?, TVFilterBase>);
        tKeyRevealType   ??= typeof(PalantírReveal<TKey>);
        tvRevealBaseType ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), enumtrType, filterPredicate, tvRevealBaseType, tKeyRevealType
                   , typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        
        // call AddFilteredIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod
            .CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVFilterBase, TVRevealerBase>));
        return (BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVFilterBase, TVRevealerBase>)methodInvoker;
    }
    
    internal static MethodInfo GetAddFilteredBothWithNullableValueStructRevealersMethodInfo<TValue, TKFilterBase, TKRevealBase>
        (this Type enumtrType)
        where TValue : struct
        where TKRevealBase : notnull
    {
        var tValue         = typeof(TValue);
        var tkFilterType   = typeof(TKFilterBase);
        var tkRevealerType = typeof(TKRevealBase);
        var methInf =
            BothRevealersNullableValueStructMethodInfoCache.GetOrAdd
                ((enumtrType, tValue, tkFilterType, tkRevealerType),
                 static ((Type enumtrType, Type tvalueType, Type tkFilterType, Type tkRevealBase) key) =>
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
                             (nameof(AddFilteredIterateBothWithNullValueRevealers)
                            , [ key.enumtrType, keyType, key.tvalueType, key.tkFilterType, key.tkRevealBase ],
                              [
                                  typeof(KeyedCollectionMold), acceptEnumtrType
                                , typeof(KeyValuePredicate<TKFilterBase, TValue?>)  
                                , typeof(PalantírReveal<TValue>)
                                , typeof(PalantírReveal<TKRevealBase>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase> 
        GetAddFilteredBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase>
        (Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumtr);
        var tValueType   = typeof(TValue);
        var tkFilterType = typeof(TKFilterBase);
        var tkRevealBase = typeof(TKRevealerBase);
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumtrType, tValueType, tkFilterType, tkRevealBase),
                      static (Type enumtrType, Type valueType, Type tkFilterType, Type tkRevealBase) =>
                      {
                          var kvpTypes = enumtrType.GetKeyedCollectionTypes();
                          if (kvpTypes == null)
                              throw new ArgumentException("Expected to receive a KeyValue enumerator");
                          var keyType = kvpTypes.Value.Key;
                          var toInvokeOn =
                              GetStaticMethodInfo
                                  (nameof(AddFilteredIterateBothWithNullValueRevealers)
                                    , [enumtrType, keyType, valueType, tkFilterType, tkRevealBase],
                                      [
                                          typeof(KeyedCollectionMold), enumtrType
                                        , typeof(KeyValuePredicate<TKFilterBase, TValue?>)  
                                        , typeof(PalantírReveal<TValue>)
                                        , typeof(PalantírReveal<TKRevealerBase>)
                                        , typeof(string), typeof(FormatFlags)
                                      ]);

                          return BuildAddFilteredBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase>
                              (toInvokeOn, enumtrType, typeof(PalantírReveal<TKRevealerBase>), typeof(PalantírReveal<TValue>));
                      });
        return invoker;
    }

    internal static BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase> 
        BuildAddFilteredBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase>
        (MethodInfo methodInfo, Type? enumtrType = null, Type? tkRevealBase = null, Type? tvRevealType = null)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        enumtrType   ??= typeof(TEnumtr);
        var filterPredicate = typeof(KeyValuePredicate<TKFilterBase, TValue?>);  
        tkRevealBase ??= typeof(PalantírReveal<TKRevealerBase>);
        tvRevealType ??= typeof(PalantírReveal<TValue>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), enumtrType, filterPredicate, tvRevealType, tkRevealBase, typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        
        // call AddFilteredIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase>));
        return (BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase>)methodInvoker;
    }
    
    internal static MethodInfo GetAddFilteredBothRevealersNullableStructValueStruct<TKey, TValue, TKFilterBas, TKRevealerBase>(this Type enumtrType)
        where TKey : struct
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var tKey   = typeof(TKey);
        var tValue = typeof(TValue);
        var methInf =
            BothRevealersBothNullableStructsMethodInfoCache.GetOrAdd
                ((enumtrType, tValue, tKey),
                 static ((Type enumtrType, Type tvalueType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(AddFilteredIterateBothNullRevealers)
                            , [ key.enumtrType, keyType, valueType],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(KeyValuePredicate<TKFilterBas, TValue?>)  
                                , typeof(PalantírReveal<TValue>)
                                , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase> BuildAddFilteredBothNullableRevealersCallStructEnumtrInvoker
        <TEnumtr, TValue, TKFilterBase, TKRevealerBase>
        (MethodInfo methodInfo, Type? enumtrType = null, Type? tkRevealBase = null, Type? tvRevealType = null)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        enumtrType   ??= typeof(TEnumtr);
        tkRevealBase ??= typeof(PalantírReveal<TKRevealerBase>);
        tvRevealType ??= typeof(PalantírReveal<TValue>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddFilteredIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), enumtrType, tvRevealType, tkRevealBase, typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionAddFilteredIterateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        
        // call AddFilteredIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, filterPredicate,  valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase>));
        return (BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealerBase>)methodInvoker;
    }
    
    internal static MethodInfo GetAddFilteredBothNullRevealersMethodInfo<TKey, TValue>(this Type enumtrType)
        where TKey : struct
        where TValue : struct
    {
        var tKey   = typeof(TKey);
        var tValue = typeof(TValue);
        var methInf =
            BothRevealersBothNullableStructsMethodInfoCache.GetOrAdd
                ((enumtrType, tValue, tKey),
                 static ((Type enumtrType, Type tvalueType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(AddFilteredIterateBothNullRevealers)
                            , [ key.enumtrType, keyType.IfNullableGetUnderlyingTypeOrThisCached()
                                , valueType.IfNullableGetUnderlyingTypeOrThisCached() ],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(KeyValuePredicate<TKey?, TValue?>)  
                                , typeof(PalantírReveal<TValue>)
                                , typeof(PalantírReveal<TKey>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(KeyedCollectionAddFilteredIterateExtensions).GetMethods(NonPublic | Public | Static);

        MethodInfo? genTypeDefMeth   = null;
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
            var checkEnumtrType    = checkParameterInfos[1].ParameterType;
            if ((!findEnumtrType.IsNullable() && checkEnumtrType.IsNullable())
                || (findEnumtrType.IsNullable() && !checkEnumtrType.IsNullable())) continue;
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
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetAddFilteredNoRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddFilteredNoRevealersInvoker<TEnumtr, TKFilterBase, TVFilterBase>(actualType);
        return invoker(callOn, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
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
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
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
        where TVRevealerBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateValueRevealer(value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

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
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
            (value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
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
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
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

    public static KeyedCollectionMold AddFilteredIterateNullValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealerBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealerBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateNullValueRevealer(value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateNullValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(
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
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddFilteredValueRevealerInvoker<TEnumtr, TKFilterBase, TVFilterBase, TVRevealerBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateNullValueRevealer<TEnumtr, TKFilterBase, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateNullValueRevealer(value.Value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateNullValueRevealer<TEnumtr, TKFilterBase, TValue>(
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
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddFilteredValueRevealerNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase>(actualType);
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
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?
        where TValue : struct
    {
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
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
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
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
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
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

    public static KeyedCollectionMold AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothWithNullKeyRevealers
            (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothWithNullKeyRevealers
            (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddFilteredBothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVFilterBase, TVRevealBase>(actualType);
        return invoker(callOn, value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>
            (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
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
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
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

    public static KeyedCollectionMold AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothWithNullValueRevealers
            (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothWithNullValueRevealers
            (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetAddFilteredBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKFilterBase, TKRevealBase>(actualType);
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
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>
            (value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
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
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
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
        where TValue : struct
    {
        if (value == null) return callOn;
        return callOn.AddFilteredIterateBothNullRevealers(value.Value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);
    }

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
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
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
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
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
