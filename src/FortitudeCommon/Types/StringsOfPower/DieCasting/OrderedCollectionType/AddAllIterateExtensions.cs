// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public static class OrderedCollectionAddAllIterateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo> BuiltInTypeInvoke = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> CloakedTypeInvoke = new();
    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo> NullableCloakedTypeInvoke = new();
    
    
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> InputTypeInvokeCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> CloakedInvokeCache = new();

    internal delegate void InputTypeInvoke<in TEnumtr>(
        ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?;

    internal delegate void CloakedRevealerInvoker<in TEnumtr, out TRevealBase>(
        ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TRevealBase> cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull;

    internal delegate void CloakedRevealerInvoker<in TEnumtr, TElement, out TRevealBase>(
        ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TRevealBase>? cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TElement>?
        where TElement : TRevealBase?
        where TRevealBase : notnull;

    internal static MethodInfo GetAddAllBoolMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     bool isNullable = key.elementType.IsNullable();
                     var  itemType   = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (!itemType.IsInputConstructionType()) 
                         throw new ArgumentException("Expected to receive a a built in enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
                     genericParamTypes[0] = key.enumeratorType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(string);
                     methodParamTypes[3] = typeof(FormatFlags);
                     methodParamTypes[4] = typeof(bool?);
                     
                     var methodName = isNullable ? nameof(AddAllIterateNullableBool) : nameof(AddAllIterateBool);

                     return GetStaticMethodInfo(methodName, genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllSpanFormattableMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     
                     bool isNullable = key.elementType.IsNullable();
                     var  itemType = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (!itemType.IsInputConstructionType()) 
                         throw new ArgumentException("Expected to receive a a built in enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(string);
                     methodParamTypes[3] = typeof(FormatFlags);
                     methodParamTypes[4] = typeof(bool?);

                     string methodName;
                     if (isNullable)
                     {
                         methodName           = nameof(AddAllIterateNullable);
                         genericParamTypes[1] = itemType;
                     }
                     else
                     {
                         methodName = nameof(AddAllIterate);
                     }

                     return GetStaticMethodInfo(methodName, genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllStringBearerMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     
                     bool isNullable = key.elementType.IsNullable();
                     var  itemType   = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (!itemType.IsStringBearerOrNullableCached()) 
                         throw new ArgumentException("Expected to receive a built-in enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(string);
                     methodParamTypes[3] = typeof(FormatFlags);
                     methodParamTypes[4] = typeof(bool?);

                     string methodName;
                     if (isNullable)
                     {
                         methodName           = nameof(RevealAllIterateNullableStringBearer);
                         genericParamTypes[1] = itemType;
                     }
                     else
                     {
                         methodName = nameof(RevealAllIterate);
                     }

                     return GetStaticMethodInfo(methodName, genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllStringMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     
                     if (!key.elementType.IsString()) 
                         throw new ArgumentException("Expected to receive a string enumerator type.  Got " + key.elementType.FullName);

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
                     genericParamTypes[0] = key.enumeratorType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(string);
                     methodParamTypes[3] = typeof(FormatFlags);
                     methodParamTypes[4] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddAllIterateString), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllCharSequenceMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     if (!key.elementType.IsCharSequence()) 
                         throw new ArgumentException("Expected to receive a ICharSequence enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(string);
                     methodParamTypes[3] = typeof(FormatFlags);
                     methodParamTypes[4] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddAllIterateCharSeq), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllStringBuilderMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     
                     if (!key.elementType.IsStringBuilder()) 
                         throw new ArgumentException("Expected to receive a StringBuilder enumerator type.  Got " + key.elementType.FullName);

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
                     genericParamTypes[0] = key.enumeratorType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(string);
                     methodParamTypes[3] = typeof(FormatFlags);
                     methodParamTypes[4] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddAllIterateStringBuilder), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllMatchMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(string);
                     methodParamTypes[3] = typeof(FormatFlags);
                     methodParamTypes[4] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddAllIterateMatch), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllObjectMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
                     genericParamTypes[0] = key.enumeratorType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(string);
                     methodParamTypes[3] = typeof(FormatFlags);
                     methodParamTypes[4] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddAllIterateObject), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllCloakedRevealerMethodInfo<TRevealBase>(this Type enumtrType, Type elementType)
        where TRevealBase : notnull
    {
        var cloakedRevealerType = typeof(TRevealBase);
        var methInf =
            CloakedTypeInvoke.GetOrAdd
                ((enumtrType, elementType, cloakedRevealerType)
               , static ((Type enumeratorType, Type elementType, Type cloakedRevealType) key) =>
                 {
                     bool isNullable = key.elementType.IsNullable();
                     var  itemType = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (isNullable || !itemType.IsAssignableTo(key.cloakedRevealType)) 
                         throw new ArgumentException
                             ( $"Expected to receive a non nullable Type compatible with {key.cloakedRevealType} enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;
                     genericParamTypes[2] = key.cloakedRevealType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(PalantírReveal<TRevealBase>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(RevealAllIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddAllNullableCloakedRevealerMethodInfo<TCloakedStruct>(this Type enumtrType, Type elementType)
        where TCloakedStruct : struct
    {
        var methInf =
            NullableCloakedTypeInvoke.GetOrAdd
                ((enumtrType, elementType)
               , static ((Type enumeratorType, Type elementType) key) =>
                 {
                     bool isNullable = key.elementType.IsNullable();
                     
                     var itemType = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (!isNullable) 
                         throw new ArgumentException
                             ( $"Expected to receive a nullable Type struct type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = itemType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(PalantírReveal<TCloakedStruct>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);
                     
                     return GetStaticMethodInfo(nameof(RevealAllIterateNullable), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    private static InputTypeInvoke<TEnumtr> CreateInputTypeInvokerDelegate<TEnumtr>(Type enumblParamType, Type enumtrType, Type elementType
      , string toInvokeMethodName)
        where TEnumtr : IEnumerator?
    {
        var itemType = elementType.IfNullableGetUnderlyingTypeOrThis();
        
        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
        genericParamTypes[0] = enumtrType;
        genericParamTypes[1] = itemType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumtrType;
        methodParamTypes[2] = typeof(string);
        methodParamTypes[3] = typeof(FormatFlags);
        methodParamTypes[4] = typeof(bool?);

        var toInvokeOn = GetStaticMethodInfo(toInvokeMethodName, genericParamTypes.AsArray, methodParamTypes.AsArray);

        var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllSingleGenericEnumerableInvoker)));
        genericParamTypes[0] = enumblParamType;
        genericParamTypes[1] = elementType;
        var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;

        using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
        invokeReflectedArgs[0] = toInvokeOn;
        invokeReflectedArgs[1] = enumblParamType;
        invokeReflectedArgs[2] = enumtrType;
        invokeReflectedArgs[3] = methodParamTypes.AsArray;

        return (InputTypeInvoke<TEnumtr>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static InputTypeInvoke<TEnumtr> BuildAddAllSingleGenericEnumerableInvoker<TEnumtr, TElement>(MethodInfo methodInfo, Type enumblParamType
      , Type enumblType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator<TElement>?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", null,
                 methodParamTypes, typeof(OrderedCollectionAddAllEnumerateExtensions).Module, false);
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumtr>));
        var createInvoker = (InputTypeInvoke<TEnumtr>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumtr? enumbl, string? valueFmtStr = null
          , FormatFlags flags = DefaultCallerTypeFlags, bool? hasValue = null) =>
            createInvoker(mws, enumbl, valueFmtStr, flags);
    }

    private static InputTypeInvoke<TEnumtr> CreateSingleInputTypeInvokerDelegate<TEnumtr>(Type enumblParamType, Type enumblType, Type itemType
      , string toInvokeMethodName)
        where TEnumtr : IEnumerator?
    {
        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
        genericParamTypes[0] = enumblType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(string);
        methodParamTypes[3] = typeof(FormatFlags);
        methodParamTypes[4] = typeof(bool?);

        var toInvokeOn              = GetStaticMethodInfo(toInvokeMethodName, genericParamTypes.AsArray, methodParamTypes.AsArray);
        
        var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllSingleToSingleGenericEnumerableInvoker)));
        using var invokeGenericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
        invokeGenericParamTypes[0] = enumblParamType;
        invokeGenericParamTypes[1] = itemType;
        var concreteGenMethod = genGenMethod.MakeGenericMethod(invokeGenericParamTypes.AsArray);

        methodParamTypes[1]  = enumblParamType;

        using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
        invokeReflectedArgs[0] = toInvokeOn;
        invokeReflectedArgs[1] = enumblParamType;
        invokeReflectedArgs[2] = enumblType;
        invokeReflectedArgs[3] = methodParamTypes.AsArray;

        return (InputTypeInvoke<TEnumtr>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static InputTypeInvoke<TEnumtr> BuildAddAllSingleToSingleGenericEnumerableInvoker<TEnumtr, TElement>(
        MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator<TElement>?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", null,
                 methodParamTypes, typeof(OrderedCollectionAddAllEnumerateExtensions).Module, false);
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumtr>));
        var createInvoker = (InputTypeInvoke<TEnumtr>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumtr? enumbl, string? valueFmtStr = null
          , FormatFlags flags = DefaultCallerTypeFlags, bool? hasValues = null) =>
            createInvoker(mws, enumbl, valueFmtStr, flags, hasValues);
    }

    private static InputTypeInvoke<TEnumtr> CreateAddAllBoolDelegate<TEnumtr>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        var isNullable  = elementType.IsNullable();
        var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
        if (!itemType.IsBool()) throw new ArgumentException("Expected to receive a Boolean(?) collection");

        var toInvokeMethodName = isNullable ? nameof(AddAllIterateNullableBool) : nameof(AddAllIterateBool);

        return CreateSingleInputTypeInvokerDelegate<TEnumtr>(enumblParamType, enumblType, elementType, toInvokeMethodName);
    }
    
    internal static InputTypeInvoke<TEnumtr> GetAddAllSpanFormattable<TEnumtr>(Type enumblType)
        where TEnumtr : IEnumerator?
    {
        var enumblParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumtr>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllSpanFormattableDelegate<TEnumtr>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumtr> CreateAddAllSpanFormattableDelegate<TEnumtr>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        var isNullable  = elementType.IsNullable();
        var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
        if (!itemType.IsSpanFormattableCached()) throw new ArgumentException("Expected to receive a ISpanFormattable collection");

        var toInvokeMethodName = isNullable ? nameof(AddAllIterateNullable) : nameof(AddAllIterate);

        return CreateInputTypeInvokerDelegate<TEnumtr>(enumblParamType, enumblType, elementType, toInvokeMethodName);
    }

    private static CloakedRevealerInvoker<TEnumtr, TRevealBase> GetAddAllCloakedRevealer<TEnumtr, TRevealBase>(Type enumblType)
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumtr);
        var revealType      = typeof(TRevealBase);
        var callAsFactory   = true;
        var invoker =
            (CloakedRevealerInvoker<TEnumtr, TRevealBase>)
            CloakedInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType, revealType)
                   , static ((Type enumblParamType, Type enumblType, Type revealType) key, bool _) => 
                         CreateAddAllCloakedRevealerDelegate<TEnumtr, TRevealBase>(key.enumblParamType, key.enumblType, key.revealType)
                   , callAsFactory);
        return invoker;
    }

    private static CloakedRevealerInvoker<TEnumtr, TRevealBase> CreateAddAllCloakedRevealerDelegate<TEnumtr, TRevealBase>
        (Type enumblParamType, Type enumblType, Type revealType) 
        where TEnumtr : IEnumerator? 
        where TRevealBase : notnull
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
                     
        if (!elementType.IsAssignableTo(revealType)) 
            throw new ArgumentException($"Expected to receive a enumerable element " +
                                        $"{elementType.Name} to be equatable to {revealType.Name}");

        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
        genericParamTypes[0] = enumblType;
        genericParamTypes[1] = elementType;
        genericParamTypes[2] = revealType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(PalantírReveal<TRevealBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);
        methodParamTypes[5] = typeof(bool?);

        var toInvokeOn = GetStaticMethodInfo(nameof(RevealAllIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);

        var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllCloakedRevealerInvoker)));
        genericParamTypes[0] = enumblParamType;
        var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;

        using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
        invokeReflectedArgs[0] = toInvokeOn;
        invokeReflectedArgs[1] = enumblParamType;
        invokeReflectedArgs[2] = enumblType;
        invokeReflectedArgs[3] = methodParamTypes.AsArray;

        return (CloakedRevealerInvoker<TEnumtr, TRevealBase>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static CloakedRevealerInvoker<TEnumtr, TRevealBase> BuildAddAllCloakedRevealerInvoker<TEnumtr, TElement, TRevealBase>(
        MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator<TElement>?
        where TElement : TRevealBase?
        where TRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", null,
                 methodParamTypes, typeof(OrderedCollectionAddAllEnumerateExtensions).Module, false);
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
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CloakedRevealerInvoker<TEnumtr, TElement, TRevealBase>));
        var createInvoker = (CloakedRevealerInvoker<TEnumtr, TElement, TRevealBase>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumtr? enumbl, PalantírReveal<TRevealBase> revealer, string? valueFmtStr = null
          , FormatFlags flags = DefaultCallerTypeFlags, bool? hasValue = null) =>
            createInvoker(mws, enumbl, revealer, valueFmtStr, flags, hasValue);
    }

    internal static InputTypeInvoke<TEnumtr> GetAddAllStringBearer<TEnumtr>(Type enumblType)
        where TEnumtr : IEnumerator?
    {
        var enumblParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumtr>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllStringBearerDelegate<TEnumtr>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumtr> CreateAddAllStringBearerDelegate<TEnumtr>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        bool isNullable  = elementType.IsNullable();
        var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
        if (!itemType.IsStringBearer()) throw new ArgumentException("Expected to receive a IStringBearer collection");

        string toInvokeMethodName = isNullable ? nameof(RevealAllIterateNullable) : nameof(RevealAllIterate);

        return CreateInputTypeInvokerDelegate<TEnumtr>(enumblParamType, enumblType, elementType, toInvokeMethodName);
    }

    private static InputTypeInvoke<TEnumtr> CreateAddAllStringDelegate<TEnumtr>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         
        if (!elementType.IsString()) throw new ArgumentException("Expected to receive a string collection");

        return CreateSingleInputTypeInvokerDelegate<TEnumtr>(enumblParamType, enumblType, elementType, nameof(AddAllIterateString));
    }

    internal static InputTypeInvoke<TEnumtr> GetAddAllCharSequence<TEnumtr>(Type enumblType)
        where TEnumtr : IEnumerator?
    {
        var enumblParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumtr>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllCharSequenceDelegate<TEnumtr>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumtr> CreateAddAllCharSequenceDelegate<TEnumtr>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
                         
        if (!elementType.IsCharSequence()) throw new ArgumentException("Expected to receive a ICharSequence collection");

        return CreateInputTypeInvokerDelegate<TEnumtr>(enumblParamType, enumblType, elementType, nameof(AddAllIterateCharSeq));
    }

    private static InputTypeInvoke<TEnumtr> CreateAddAllStringBuilderDelegate<TEnumtr>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         
        if (!elementType.IsStringBuilder()) throw new ArgumentException("Expected to receive a StringBuilder collection");

        return CreateSingleInputTypeInvokerDelegate<TEnumtr>(enumblParamType, enumblType, elementType, nameof(AddAllIterateStringBuilder));
    }

    internal static InputTypeInvoke<TEnumtr> GetAddAllMatch<TEnumtr>(Type enumblType)
        where TEnumtr : IEnumerator?
    {
        var enumblParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumtr>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllMatchDelegate<TEnumtr>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumtr> CreateAddAllMatchDelegate<TEnumtr>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        var itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();

        if (itemType.IsSpanFormattable())
        {
            return CreateAddAllSpanFormattableDelegate<TEnumtr>(enumblParamType, enumblType);
        }
        if (itemType.IsStringBearer())
        {
            return CreateAddAllStringBearerDelegate<TEnumtr>(enumblParamType, enumblType);
        }
        if (itemType.IsString())
        {
            return CreateAddAllStringDelegate<TEnumtr>(enumblParamType, enumblType);
        }
        if (itemType.IsStringBuilder())
        {
            return CreateAddAllStringBuilderDelegate<TEnumtr>(enumblParamType, enumblType);
        }
        if (itemType.IsCharSequence())
        {
            return CreateAddAllCharSequenceDelegate<TEnumtr>(enumblParamType, enumblType);
        }
        if (itemType.IsBool())
        {
            return CreateAddAllBoolDelegate<TEnumtr>(enumblParamType, enumblType);
        }

        return CreateInputTypeInvokerDelegate<TEnumtr>(enumblParamType, enumblType, elementType, nameof(AddAllIterateMatch));
    }

    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(OrderedCollectionAddAllIterateExtensions).GetMethods( NonPublic | Public | Static);

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

    public static void AddAllIterateBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<bool>
    {
        if (value != null)
        {
            mws.AddAllIterateBool(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(bool);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<bool>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(bool);
        var  any             = false;
        int? collectionItems = value == null ? null : 0;
        hasValue ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateNullableBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<bool?>
    {
        if (value != null)
        {
            mws.AddAllIterateNullableBool(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(bool);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateNullableBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(bool);
        var  any             = false;
        int? collectionItems = value == null ? null : 0;
        hasValue ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterate<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.AddAllIterate(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterate<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllSpanFormattable<TEnumtr>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags, hasValue);
    }

    public static void AddAllIterate<TEnumtr, TFmt>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TFmt?>
        where TFmt : ISpanFormattable?
    {
        if (value != null)
        {
            mws.AddAllIterate<TEnumtr, TFmt>(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterate<TEnumtr, TFmt>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TFmt?>?
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TFmt);
        var  any             = false;
        int? collectionItems = value == null ? null : 0;
        hasValue ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateNullable<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.AddAllIterateNullable(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateNullable<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllSpanFormattable<TEnumtr>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags, hasValue);
    }

    public static void AddAllIterateNullable<TEnumtr, TFmtStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable
    {
        if (value != null)
        {
            mws.AddAllIterateNullable<TEnumtr, TFmtStruct>(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateNullable<TEnumtr, TFmtStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TFmtStruct?);
        var  any             = false;
        int? collectionItems = value == null ? null : 0;
        hasValue ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllIterate<TEnumtr, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
        where TRevealBase : notnull
    {
        if (value != null)
        {
            mws.RevealAllIterate(value.Value, palantírReveal, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllIterate<TEnumtr, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllCloakedRevealer<TEnumtr, TRevealBase>(actualType);
        callGenericInvoker(mws, value, palantírReveal, formatString, formatFlags, hasValue);
    }

    public static void RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCloaked>
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (value != null)
        {
            mws.RevealAllIterate(value.Value, palantírReveal, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloaked?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TCloaked);
        var  any             = false;
        int? collectionItems = value == null ? null : 0;
        hasValue ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = 
                mws.ConditionalCollectionPrefix(value, elementType, false
                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllIterateNullable<TEnumtr, TCloakedStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCloakedStruct?>
        where TCloakedStruct : struct
    {
        if (value != null)
        {
            mws.RevealAllIterateNullable(value.Value, palantírReveal, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }
    
    public static void RevealAllIterateNullable<TEnumtr, TCloakedStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TCloakedStruct?>?
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TCloakedStruct);
        var  any             = false;
        int? collectionItems = value == null ? null : 0;
        hasValue ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }
    
    public static void RevealAllIterate<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.RevealAllIterate(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(IStringBearer);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }
    
    public static void RevealAllIterate<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllStringBearer<TEnumtr>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags, hasValue);
    }

    public static void RevealAllIterate<TEnumtr, TBearer>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TBearer>
        where TBearer : IStringBearer?
    {
        if (value != null)
        {
            mws.RevealAllIterate(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(IStringBearer);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllIterate<TEnumtr, TBearer>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TBearer>?
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var elementType = typeof(TBearer);
        var any         = false;
        hasValue ??= value?.MoveNext() ?? false;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllIterateNullableStringBearer<TEnumtr, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer
    {
        if (value != null)
        {
            mws.RevealAllIterateNullableStringBearer<TEnumtr, TBearerStruct>(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(IStringBearer);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllIterateNullableStringBearer<TEnumtr, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var elementType = typeof(TBearerStruct);
        var any         = false;
        hasValue ??= value?.MoveNext() ?? false;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateString<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<string?>
    {
        if (value != null)
        {
            mws.AddAllIterateString(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateString<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<string?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(string);
        var  any             = false;
        int? collectionItems = value == null ? null : 0;
        hasValue ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateCharSeq<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.AddAllIterateCharSeq(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(ICharSequence);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateCharSeq<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllCharSequence<TEnumtr>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags, hasValue);
    }

    public static void AddAllIterateCharSeq<TEnumtr, TCharSeq>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCharSeq?>
        where TCharSeq : ICharSequence?
    {
        if (value != null)
        {
            mws.AddAllIterateCharSeq<TEnumtr, TCharSeq>(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TCharSeq);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateCharSeq<TEnumtr, TCharSeq>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TCharSeq?>?
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var elementType = typeof(TCharSeq);
        var any         = false;
        hasValue ??= value?.MoveNext() ?? false;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                any = true;
                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateStringBuilder<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<StringBuilder?>
    {
        if (value != null)
        {
            mws.AddAllIterateStringBuilder(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(StringBuilder);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateStringBuilder<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<StringBuilder?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var elementType = typeof(StringBuilder);
        var any         = false;
        hasValue ??= value?.MoveNext() ?? false;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateMatch<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.AddAllIterateMatch(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateMatch<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllMatch<TEnumtr>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags, hasValue);
    }

    public static void AddAllIterateMatch<TEnumtr, TAny>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TAny?>
    {
        if (value != null)
        {
            mws.AddAllIterateMatch<TEnumtr, TAny>(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TAny);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllIterateMatch<TEnumtr, TAny>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TAny?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var elementType = typeof(TAny);
        var any         = false;
        hasValue ??= value?.MoveNext() ?? false;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    [CallsObjectToString]
    public static void AddAllIterateObject<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<object?>
    {
        if (value != null)
        {
            mws.AddAllIterateObject(value.Value, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    [CallsObjectToString]
    public static void AddAllIterateObject<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<object?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(object);
        var  any             = false;
        int? collectionItems = value == null ? null : 0;
        hasValue ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix
                        (value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any         = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }
}
