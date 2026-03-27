// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public static class KeyedCollectionMoldIteratorExtensions
{
    private static MethodInfo[]? myMethodInfosCached;


    private static readonly ConcurrentDictionary<Type, MethodInfo> NoRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<Type, Delegate>   NoRevealersNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo> ValueRevealerNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type), Delegate>   ValueRevealerNoNullableStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo> ValueRevealerNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type), Delegate>   ValueRevealerNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> BothRevealersNoNullableStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> BothRevealersNoNullableStructInvokerCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> BothRevealersNullableKeyStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate>   BothRevealersNullableKeyStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> BothRevealersNullableValueStructMethodInfoCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate>   BothRevealersNullableValueStructInvokerCache    = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> BothRevealersBothNullableStructsMethodInfoCache = new();


    internal delegate KeyedCollectionMold NoRevealersNoNullableStructInvoker<in TEnumtr, TKey, TValue>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?;
    

    internal delegate KeyedCollectionMold NoRevealersNoNullableStructInvoker<in TEnumtr>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator;
    
    internal delegate KeyedCollectionMold CreateValueRevealerNoNullableStructInvoker<in TEnumtr, TKey, TValue, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold ValueRevealerNoNullableStructInvoker<in TEnumtr, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull;
    
    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold CreateValueRevealerNullableValueStructInvoker<in TEnumtr, TKey, TValue>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold ValueRevealerNullableValueStructInvoker<in TEnumtr, TValue>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct;

    internal delegate KeyedCollectionMold CreateBothRevealersNoNullableStructInvoker<in TEnumtr, TKey, TValue, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold CallBothRevealersNoNullableStructInvoker<in TEnumtr, TKey, TValue, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    internal delegate KeyedCollectionMold BothRevealersNoNullableStructInvoker<in TEnumtr, out TKRevealBase, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableKeyStructInvoker<in TEnumtr, TKey, out TVRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull;

    // ReSharper disable once TypeParameterCanBeVariant
    internal delegate KeyedCollectionMold BothRevealersNullableValueStructInvoker<in TEnumtr, TValue, out TKRevealBase>(
        KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealBase : notnull;
    
    internal static MethodInfo GetGetNoRevealersInvokerMethodInfo(this Type enumtrType)
    {
        var methInf =
            NoRevealersNoNullableStructMethodInfoCache.GetOrAdd
                (enumtrType, static enumeratorType =>
                 {
                     var kvpTypes = enumeratorType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(AddAllIterate)
                            , [ enumeratorType, keyType, valueType],
                              [
                                  typeof(KeyedCollectionMold), enumeratorType
                                , typeof(string), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static NoRevealersNoNullableStructInvoker<TEnumtr> GetNoRevealersInvoker<TEnumtr>(Type? enumtrType = null)
        where TEnumtr : IEnumerator?
    {
        enumtrType ??= typeof(TEnumtr);
        var callAsFactory = true;
        var invoker =
            (NoRevealersNoNullableStructInvoker<TEnumtr>)NoRevealersNoNullableStructInvokerCache
                .GetOrAdd(enumtrType, static (enumtrType, _) =>
            {
                var kvpTypes = enumtrType.GetKeyedCollectionTypes();
                if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                var keyType   = kvpTypes.Value.Key;
                var valueType = kvpTypes.Value.Value;
                var toInvokeOn = GetStaticMethodInfo(nameof(AddAllIterate), [enumtrType, keyType, valueType],
                [ typeof(KeyedCollectionMold), enumtrType, typeof(string), typeof(string), typeof(FormatFlags) ]);

                var genGenMethod
                    = myMethodInfosCached!
                        .First(mi => mi.Name.Contains(nameof(BuildNoRevealersNoNullableStructInvoker)));
                var concreteGenMethod
                    = genGenMethod.MakeGenericMethod([typeof(TEnumtr), keyType, valueType ]);
                return (NoRevealersNoNullableStructInvoker<TEnumtr>)concreteGenMethod.Invoke(null, [ toInvokeOn, enumtrType ])!;
            }, callAsFactory);
        return invoker;
    }

    internal static NoRevealersNoNullableStructInvoker<TEnumtr> BuildNoRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue>
        (MethodInfo methodInfo, Type? enumtrType = null)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        enumtrType ??= typeof(TEnumtr);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumtr), typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionMoldIteratorExtensions).Module, false);
        var ilGenerator     = helperMethod.GetILGenerator();
        // Make space for enumtrType local variables
        var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);
        
        // cast TEnumtr value => (enumtrType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumtrLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);
        
        // call AddAllIterate(KeyedCollectionMold, TEnumtr, valueFmtStr, keyFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NoRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue>));
        var createInvoker = (NoRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue>)methodInvoker;
        
        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumtr? enumtr, string? valueFmtStr, string? keyFmtString, FormatFlags flags) =>
            createInvoker(kcm, enumtr, valueFmtStr, keyFmtString, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetValueRevealerNoNullableStructIInvokerMethodInfo<TVRevealerBase>(this Type enumtrType)
    where TVRevealerBase : notnull
    {
        var tvRevealerBaseType = typeof(TVRevealerBase);
        var methInf =
            ValueRevealerNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tvRevealerBaseType), static ((Type enumeratorType, Type tvRevealerType) key, bool _) =>
                {
                    var kvpTypes = key.enumeratorType.GetKeyedCollectionTypes();
                    if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                    var keyType   = kvpTypes.Value.Key;
                    var valueType = kvpTypes.Value.Value;
                    return
                        GetStaticMethodInfo
                            (nameof(AddAllIterateValueRevealer)
                           , [ key.enumeratorType, keyType, valueType, key.tvRevealerType],
                             [
                                 typeof(KeyedCollectionMold), key.enumeratorType
                               , typeof(PalantírReveal<TVRevealerBase>), typeof(string), typeof(string)
                               , typeof(FormatFlags)
                             ]);
                }, true);
        return methInf;
    }

    internal static ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase> GetValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase>(
        Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TVRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumtr);
        var tvRevealBase  = typeof(TVRevealerBase);
        var callAsFactory = true;
        var invoker =
            (ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase>)
            ValueRevealerNoNullableStructInvokerCache
                .GetOrAdd
                    ((enumtrType, tvRevealBase),
                       static ((Type enumtrType, Type tvRevealType) key, bool _) =>
                       {
                           var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                           if (kvpTypes == null)
                               throw new ArgumentException("Expected to receive a KeyValue enumerator");
                           var keyType   = kvpTypes.Value.Key;
                           var valueType = kvpTypes.Value.Value;
                           var toInvokeOn =
                               GetStaticMethodInfo
                                   (nameof(AddAllIterateValueRevealer)
                                 , [key.enumtrType, keyType, valueType, key.tvRevealType],
                                   [
                                       typeof(KeyedCollectionMold), typeof(TEnumtr)
                                     , typeof(PalantírReveal<TVRevealerBase>)
                                     , typeof(string), typeof(string), typeof(FormatFlags)
                                   ]);

                           var genGenMethod
                               = myMethodInfosCached!
                                   .First(mi => mi.Name.Contains(nameof(BuildValueRevealerNoNullableStructInvoker)));
                           var concreteGenMethod
                               = genGenMethod.MakeGenericMethod([typeof(TEnumtr), keyType, valueType, key.tvRevealType ]);
                           return (ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase>)
                               concreteGenMethod.Invoke(null, [ toInvokeOn, key.enumtrType, key.tvRevealType ])!;
                           
                       }, callAsFactory);
        return invoker;
    }

    internal static ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase> BuildValueRevealerNoNullableStructInvoker
        <TEnumtr, TKey, TValue, TVRevealerBase>(MethodInfo methodInfo, Type? enumtrType = null, Type? tvRevealBase = null)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealerBase?
        where TVRevealerBase : notnull
    {
        enumtrType   ??= typeof(TEnumtr);
        tvRevealBase ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumtr), tvRevealBase, typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionMoldIteratorExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumtrType local variables
        var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);
        
        // cast TEnumtr value => (enumtrType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumtrLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);
        
        // call AddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CreateValueRevealerNoNullableStructInvoker<TEnumtr, TKey, TValue, TVRevealerBase>));
        var createInvoker = (ValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealerBase>)methodInvoker;
        
        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumtr? enumtr, PalantírReveal<TVRevealerBase> valueRevealer, string? keyFmtStr
          , string? valueFmtStr, FormatFlags flags) =>
            createInvoker(kcm, enumtr, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GeValueRevealerNullableValueStructMethodInfo<TValue>(this Type enumtrType)
        where TValue : struct
    {
        var tValue = typeof(TValue);
        var methInf =
            ValueRevealerNullableValueStructMethodInfoCache.GetOrAdd
                ((enumtrType, tValue),
                 static ((Type enumtrType, Type tvalueType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     return
                         GetStaticMethodInfo
                             (nameof(AddAllIterateNullValueRevealer)
                            , [ key.enumtrType, keyType, key.tvalueType],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(PalantírReveal<TValue>)
                                , typeof(string), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static ValueRevealerNullableValueStructInvoker<TEnumtr, TValue> GetValueRevealerNullableValueStructInvoker<TEnumtr, TValue>(
        Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        enumtrType ??= typeof(TEnumtr);
        var tvRevealBase = typeof(TValue);
        var invoker =
            (ValueRevealerNullableValueStructInvoker<TEnumtr, TValue>)ValueRevealerNullableValueStructInvokerCache.GetOrAdd((enumtrType, tvRevealBase)
            ,
             static (Type enumtrType, Type tvRevealType) =>
             {
                 var kvpTypes = enumtrType.GetKeyedCollectionTypes();
                 if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                 var keyType   = kvpTypes.Value.Key;
                 var valueType = kvpTypes.Value.Value;
                 var toInvokeOn =
                     GetStaticMethodInfo(nameof(AddAllIterateNullValueRevealer), [enumtrType, keyType, valueType, tvRevealType],
                     [
                         typeof(KeyedCollectionMold), typeof(TEnumtr), typeof(PalantírReveal<TValue>)
                       , typeof(string), typeof(string), typeof(FormatFlags)
                     ]);

                 var genGenMethod
                     = myMethodInfosCached!
                         .First(mi => mi.Name.Contains(nameof(BuildValueRevealerNullableValueStructInvoker)));
                 var concreteGenMethod
                     = genGenMethod.MakeGenericMethod([
                         enumtrType, keyType, valueType
                     ]);
                 return (ValueRevealerNullableValueStructInvoker<TEnumtr, TValue>)concreteGenMethod.Invoke(null, [
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

    internal static ValueRevealerNullableValueStructInvoker<TEnumtr, TValue> BuildValueRevealerNullableValueStructInvoker
        <TEnumtr, TKey, TValue>
        (MethodInfo methodInfo, Type? enumtrType = null, Type? valueType = null)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        enumtrType ??= typeof(TEnumtr);
        valueType  ??= typeof(PalantírReveal<TValue>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateNullValueRevealerInvoke_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), typeof(TEnumtr), valueType, typeof(string), typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionMoldIteratorExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumtrType local variables
        var enumtrLocalType = ilGenerator.DeclareLocal(enumtrType);
        
        // cast TEnumtr value => (enumtrType)value
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Castclass, enumtrLocalType.LocalType);
        ilGenerator.Emit(OpCodes.Stloc_0);
        
        // call AddAllIterateNullValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CreateValueRevealerNullableValueStructInvoker<TEnumtr, TKey, TValue>));
        var fullInvoker   = (CreateValueRevealerNullableValueStructInvoker<TEnumtr, TKey, TValue>)methodInvoker;
        
        KeyedCollectionMold Wrapped( KeyedCollectionMold keyCollMold, TEnumtr? enumtr, PalantírReveal<TValue> valueRevealer, string? keyFmtStr
          , string? valueFmtStr, FormatFlags flags ) => fullInvoker(keyCollMold, enumtr, valueRevealer, keyFmtStr, valueFmtStr, flags);

        return Wrapped;
    }

    internal static BothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase> GetBothRevealersNoNullableStructInvoker
        <TEnumtr, TKRevealerBase, TVRevealerBase>(Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumtr);
        var tKRevealBase  = typeof(TKRevealerBase);
        var tvRevealBase  = typeof(TVRevealerBase);
        var callAsFactory = true;
        var invoker =
            (BothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase, TVRevealerBase>)
            BothRevealersNoNullableStructInvokerCache.GetOrAdd
                ( (enumtrType, tKRevealBase, tvRevealBase),
                   static ((Type enumtrType, Type tkRevealType, Type tvRevealType) key, bool _) =>
                   {
                       var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                       if (kvpTypes == null)
                           throw new ArgumentException("Expected to receive a KeyValue enumerator");
                       var keyType    = kvpTypes.Value.Key;
                       var valueType  = kvpTypes.Value.Value;
                       var toInvokeOn = 
                           GetStaticMethodInfo
                               (nameof(AddAllIterateBothRevealers)
                              , [ key.enumtrType, keyType, valueType, key.tkRevealType, key.tvRevealType ],
                                [
                                    typeof(KeyedCollectionMold), typeof(TEnumtr)
                                  , typeof(PalantírReveal<TVRevealerBase>)
                                  , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                  , typeof(FormatFlags)
                                ]);


                       var genGenMethod
                           = myMethodInfosCached!
                               .First(mi => mi.Name.Contains(nameof(BuildBothRevealersNoNullableStructInvoker)));
                       var concreteGenMethod
                           = genGenMethod.MakeGenericMethod([
                               typeof(TEnumtr), keyType, valueType, key.tkRevealType, key.tvRevealType
                           ]);
                       return (BothRevealersNoNullableStructInvoker<TEnumtr, TKRevealerBase,
                           TVRevealerBase>)concreteGenMethod.Invoke(null, [
                           toInvokeOn
                         , new[]
                           {
                               typeof(KeyedCollectionMold), key.enumtrType
                             , typeof(PalantírReveal<TVRevealerBase>)
                             , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                             , typeof(FormatFlags)
                           }
                       ])!;
                   }, callAsFactory);
        return invoker;
    }
    
    internal static MethodInfo GetBothRevealersNoNullableStructMethodInfo<TKRevealerBase, TVRevealerBase>(this Type enumtrType)
        where TKRevealerBase : notnull
        where TVRevealerBase : notnull
    {
        var tKRevealBase  = typeof(TKRevealerBase);
        var tvRevealBase  = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNoNullableStructMethodInfoCache.GetOrAdd
                ((enumtrType, tKRevealBase, tvRevealBase),
                 static ((Type enumtrType, Type tkRevealType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(AddAllIterateBothRevealers)
                            , [ key.enumtrType, keyType, valueType, key.tkRevealType, key.tvRevealType ],
                         [
                             typeof(KeyedCollectionMold), key.enumtrType
                           , typeof(PalantírReveal<TVRevealerBase>)
                           , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                           , typeof(FormatFlags)
                         ]);
                 });
        return methInf;
    }


    internal static CallBothRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue, TKRevealerBase, TVRevealerBase> 
        BuildBothRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue, TKRevealerBase, TVRevealerBase>
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
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{parameterArgTypes[1].Name}_{signatureName}"
               , typeof(KeyedCollectionMold), parameterArgTypes, typeof(KeyedCollectionMoldIteratorExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        
        // call AddAllIterateBothRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker =
            helperMethod.CreateDelegate(typeof(CreateBothRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue, TKRevealerBase, TVRevealerBase>));
        var createdInvoker = (CreateBothRevealersNoNullableStructInvoker<TEnumtr, TKey, TValue, TKRevealerBase, TVRevealerBase>)methodInvoker;

        KeyedCollectionMold Wrapped(KeyedCollectionMold kcm, TEnumtr enumtr, PalantírReveal<TVRevealerBase> vRevealer
          , PalantírReveal<TKRevealerBase> kRevealer, string? valueFmtString, FormatFlags flags) =>
            createdInvoker(kcm, enumtr, vRevealer, kRevealer, valueFmtString, flags);

        return Wrapped;
    }
    
    internal static MethodInfo GetBothRevealersNullableKeyStructMethodInfo<TKey, TVRevealerBase>(this Type enumtrType)
        where TKey : struct
        where TVRevealerBase : notnull
    {
        var tKey = typeof(TKey);
        var tvRevealBase = typeof(TVRevealerBase);
        var methInf =
            BothRevealersNullableKeyStructMethodInfoCache.GetOrAdd
                ((enumtrType, tKey, tvRevealBase),
                 static ((Type enumtrType, Type tkeyType, Type tvRevealType) key) =>
                 {
                     var kvpTypes = key.enumtrType.GetKeyedCollectionTypes();
                     if (kvpTypes == null) throw new ArgumentException("Expected to receive a KeyValue enumerator");
                     var keyType   = kvpTypes.Value.Key;
                     var valueType = kvpTypes.Value.Value;
                     return
                         GetStaticMethodInfo
                             (nameof(AddAllIterateBothWithNullKeyRevealers)
                            , [ key.enumtrType, keyType.IfNullableGetUnderlyingTypeOrThisCached()
                                , valueType, key.tvRevealType ],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(PalantírReveal<TVRevealerBase>)
                                , typeof(PalantírReveal<TKey>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase> GetBothRevealersNullableKeyStructInvoker
        <TEnumtr, TKey, TVRevealerBase>(Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumtr);
        var tKeyType         = typeof(TKey);
        var tvRevealBaseType = typeof(TVRevealerBase);
        var invoker =
            (BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase>)
            BothRevealersNullableKeyStructInvokerCache
                .GetOrAdd
                    ((enumtrType, tKeyType, tvRevealBaseType),
                        static (Type enumtrType, Type tKey, Type tvRevealType) =>
                        {
                            var kvpTypes = enumtrType.GetKeyedCollectionTypes();
                            if (kvpTypes == null)
                                throw new ArgumentException("Expected to receive a KeyValue enumerator");
                            var valueType = kvpTypes.Value.Value;
                            var toInvokeOn =
                                GetStaticMethodInfo
                                    (nameof(AddAllIterateBothWithNullKeyRevealers)
                                      , [enumtrType, tKey, valueType, tvRevealType],
                                        [
                                            typeof(KeyedCollectionMold), typeof(IEnumerator)
                                          , typeof(PalantírReveal<TVRevealerBase>)
                                          , typeof(PalantírReveal<TKey>), typeof(string)
                                          , typeof(FormatFlags)
                                      ]);

                            return
                                BuildBothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase>
                                    (toInvokeOn, enumtrType, typeof(PalantírReveal<TKey>), typeof(PalantírReveal<TVRevealerBase>));
                        });
        return invoker;
    }

    internal static BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase> BuildBothRevealersNullableKeyStructInvoker
        <TEnumtr, TKey, TVRevealerBase>(MethodInfo methodInfo, Type? enumtrType = null, Type? tKeyRevealType = null, Type? tvRevealBaseType = null)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealerBase : notnull
    {
        enumtrType       ??= typeof(TEnumtr);
        tKeyRevealType   ??= typeof(PalantírReveal<TKey>);
        tvRevealBaseType ??= typeof(PalantírReveal<TVRevealerBase>);
        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                 [typeof(KeyedCollectionMold), enumtrType, tvRevealBaseType, tKeyRevealType, typeof(string), typeof(FormatFlags)]
               , typeof(KeyedCollectionMoldIteratorExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        
        
        // call AddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase>));
        return (BothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealerBase>)methodInvoker;
    }
    
    internal static MethodInfo GetBothRevealersNullableValueStructMethodInfo<TValue, TKRevealerBase>(this Type enumtrType)
        where TValue : struct
        where TKRevealerBase : notnull
    {
        var tValue         = typeof(TValue);
        var tkRevealBase = typeof(TKRevealerBase);
        var methInf =
            BothRevealersNullableValueStructMethodInfoCache.GetOrAdd
                ((enumtrType, tValue, tkRevealBase),
                 static ((Type enumtrType, Type tvalueType, Type tkRevealBase) key) =>
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
                             (nameof(AddAllIterateBothWithNullValueRevealers)
                            , [ key.enumtrType, keyType, key.tvalueType, key.tkRevealBase ],
                              [
                                  typeof(KeyedCollectionMold), acceptEnumtrType
                                , typeof(PalantírReveal<TValue>)
                                , typeof(PalantírReveal<TKRevealerBase>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase> GetBothRevealersNullableValueStructInvoker
        <TEnumtr, TValue, TKRevealerBase>(Type? enumtrType = null)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealerBase : notnull
    {
        enumtrType ??= typeof(TEnumtr);
        var tValueType   = typeof(TValue);
        var tkRevealBase = typeof(TKRevealerBase);
        var invoker =
            (BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>)
            BothRevealersNullableValueStructInvokerCache
                .GetOrAdd
                    ((enumtrType, tValueType, tkRevealBase),
                      static (Type enumtrType, Type valueType, Type tkRevealBase) =>
                      {
                          var kvpTypes = enumtrType.GetKeyedCollectionTypes();
                          if (kvpTypes == null)
                              throw new ArgumentException("Expected to receive a KeyValue enumerator");
                          var keyType = kvpTypes.Value.Key;
                          var toInvokeOn =
                              GetStaticMethodInfo
                                  (nameof(AddAllIterateBothWithNullValueRevealers)
                                    , [enumtrType, keyType, valueType, tkRevealBase],
                                      [
                                          typeof(KeyedCollectionMold), enumtrType
                                        , typeof(PalantírReveal<TValue>)
                                        , typeof(PalantírReveal<TKRevealerBase>)
                                        , typeof(string), typeof(FormatFlags)
                                      ]);

                          return BuildBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>
                              (toInvokeOn, enumtrType, typeof(PalantírReveal<TKRevealerBase>), typeof(PalantírReveal<TValue>));
                      });
        return invoker;
    }

    internal static BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase> BuildBothRevealersNullableValueStructInvoker
        <TEnumtr, TValue, TKRevealerBase>
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
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), enumtrType, tvRevealType, tkRevealBase, typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionMoldIteratorExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        
        // call AddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>));
        return (BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>)methodInvoker;
    }
    
    internal static MethodInfo GetBothRevealersNullableStructValueStruct<TKey, TValue>(this Type enumtrType)
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
                             (nameof(AddAllIterateBothNullRevealers)
                            , [ key.enumtrType, keyType, valueType, key.tvalueType, key.tvRevealType ],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(PalantírReveal<TValue>)
                                , typeof(PalantírReveal<TKey>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase> BuildBothNullableRevealersCallStructEnumtrInvoker
        <TEnumtr, TValue, TKRevealerBase>
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
                ($"{methodInfo.Name}_DynamicAddAllIterateBothRevealers_{enumtrType.Name}", typeof(KeyedCollectionMold),
                [
                    typeof(KeyedCollectionMold), enumtrType, tvRevealType, tkRevealBase, typeof(string), typeof(FormatFlags)
                ], typeof(KeyedCollectionMoldIteratorExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        
        // call AddAllIterateBothWithNullKeyRevealers(KeyedCollectionMold, TEnumtr, valueRevealer, keyRevealer, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>));
        return (BothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealerBase>)methodInvoker;
    }
    
    internal static MethodInfo GeBothNullRevealersMethodInfo<TKey, TValue>(this Type enumtrType)
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
                             (nameof(AddAllIterateBothNullRevealers)
                            , [ key.enumtrType, keyType.IfNullableGetUnderlyingTypeOrThisCached()
                                , valueType.IfNullableGetUnderlyingTypeOrThisCached() ],
                              [
                                  typeof(KeyedCollectionMold), key.enumtrType
                                , typeof(PalantírReveal<TValue>)
                                , typeof(PalantírReveal<TKey>), typeof(string)
                                , typeof(FormatFlags)
                              ]);
                 });
        return methInf;
    }

    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(KeyedCollectionMoldIteratorExtensions).GetMethods(NonPublic | Public | Static);

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

    public static KeyedCollectionMold AddAllIterate<TEnumtr>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var invoker = GetNoRevealersInvoker<TEnumtr?>(actualType);
        return invoker(callOn, value, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterate<TEnumtr>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        if (value == null) return callOn;
        var invoker = GetNoRevealersInvoker<TEnumtr>(actualType);
        return invoker(callOn, value.Value, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterate<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
    {
        if (value == null) return callOn;
        return callOn.AddAllIterate<TEnumtr, TKey, TValue>
            (value.Value, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterate<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            callOn.ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
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
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn;
        var invoker = GetValueRevealerNoNullableStructInvoker<TEnumtr, TVRevealBase>(actualType);
        return invoker(callOn, value.Value, valueRevealer, valueFormatString, keyFormatString, formatFlags);
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
            (value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

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
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            callOn.ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
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
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetValueRevealerNullableValueStructInvoker<TEnumtr, TValue>(actualType);
        return invoker(callOn, value, valueRevealer, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn;
        var invoker = GetValueRevealerNullableValueStructInvoker<TEnumtr, TValue>(actualType);
        return invoker(callOn, value.Value, valueRevealer, valueFormatString, keyFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
        this KeyedCollectionMold callOn
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TValue : struct
    {
        if (value == null) return callOn;
        return callOn.AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
            (value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    }

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
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            callOn.ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
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
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetBothRevealersNoNullableStructInvoker<TEnumtr, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value!, valueRevealer, keyStyler, valueFormatString, formatFlags);
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
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn;
        var invoker = GetBothRevealersNoNullableStructInvoker<TEnumtr, TKRevealBase, TVRevealBase>(actualType);
        return invoker(callOn, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddAllIterateBothRevealers(value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

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
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            callOn.ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
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
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetBothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
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
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn;
        var invoker = GetBothRevealersNullableKeyStructInvoker<TEnumtr, TKey, TVRevealBase>(actualType);
        return invoker(callOn, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
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
        where TVRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
            (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

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
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey?, TValue>);
            callOn.ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
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
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        var invoker = GetBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealBase>(actualType);
        return invoker(callOn, value, valueRevealer, keyStyler, valueFormatString, formatFlags);
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
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var keyValueTypes = actualType.GetKeyedCollectionTypes();
        if (!keyValueTypes.HasValue) throw new ArgumentException("Expected to receive a KeyValue enumerator");
        if (value == null) return callOn;
        var invoker = GetBothRevealersNullableValueStructInvoker<TEnumtr, TValue, TKRevealBase>(actualType);
        return invoker(callOn, value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
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
        where TKRevealBase : notnull
    {
        if (value == null) return callOn;
        return callOn.AddAllIterateBothWithNullValueRevealersExplicit<TEnumtr, TKey, TValue, TKRevealBase>
            (value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

    public static KeyedCollectionMold AddAllIterateBothWithNullValueRevealersExplicit<TEnumtr, TKey, TValue, TKRevealBase>(
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
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);
        var mws        = ((ITypeBuilderComponentSource<KeyedCollectionMold>)callOn).KnownTypeMoldState;
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            callOn.ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
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
        where TValue : struct
    {
        if (value == null) return callOn;
        return callOn.AddAllIterateBothNullRevealers(value.Value, valueRevealer, keyStyler, valueFormatString, formatFlags);
    }

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
            callOn.ItemCount = 0;
            while (hasValue)
            {
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
