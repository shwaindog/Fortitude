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
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemResult;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public static class OrderedCollectionAddFilteredIterateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> BuiltInTypeInvoke = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), MethodInfo> CloakedTypeInvoke = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type), MethodInfo> NullableCloakedTypeInvoke = new();
    
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> InputTypeInvokeCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> CloakedInvokeCache = new();

    internal delegate void InputTypeInvoke<in TEnumtr, out TFilterBase>(
        ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?;

    internal delegate void CloakedRevealerInvoker<in TEnumtr, out TFilterBase, out TRevealBase>(
        ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull;

    internal delegate void CloakedRevealerInvoker<in TEnumtr, TElement, out TFilterBase, out TRevealBase>(
        ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase>? cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TElement>?
        where TElement : TFilterBase?, TRevealBase?
        where TRevealBase : notnull;

    internal static MethodInfo GetAddFilteredBoolMethodInfo<TFilterType>(this Type enumtrType, Type elementType)
    {
        var filterType = typeof(TFilterType);
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType)
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     bool isNullable = key.elementType.IsNullable();
                     var  itemType   = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (!itemType.IsInputConstructionType()) 
                         throw new ArgumentException("Expected to receive a a built in enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
                     genericParamTypes[0] = key.enumeratorType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterType>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);
                     
                     var methodName = isNullable ? nameof(AddFilteredIterateNullableBool) : nameof(AddFilteredIterateBool);

                     return GetStaticMethodInfo(methodName, genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredSpanFormattableMethodInfo<TFilterBase>(this Type enumtrType, Type elementType)
    {
        var filterType = typeof(TFilterBase);
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType)
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     
                     bool isNullable = key.elementType.IsNullable();
                     var  itemType = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (!itemType.IsInputConstructionType()) 
                         throw new ArgumentException("Expected to receive a a built in enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;
                     genericParamTypes[2] = key.elementType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);

                     string methodName;
                     if (isNullable)
                     {
                         methodName           = nameof(AddFilteredIterateNullable);
                         genericParamTypes[1] = itemType;
                     }
                     else
                     {
                         methodName = nameof(AddFilteredIterate);
                     }

                     return GetStaticMethodInfo(methodName, genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredStringBearerMethodInfo<TFilterBase>(this Type enumtrType, Type elementType)
    {
        var filterType = typeof(TFilterBase);
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType)
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     
                     bool isNullable = key.elementType.IsNullable();
                     var  itemType   = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (!itemType.IsStringBearerOrNullableCached()) 
                         throw new ArgumentException("Expected to receive a built-in enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;
                     genericParamTypes[2] = key.filterType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);

                     string methodName;
                     if (isNullable)
                     {
                         methodName           = nameof(RevealFilteredIterateNullable);
                         genericParamTypes[1] = itemType;
                     }
                     else
                     {
                         methodName = nameof(RevealFilteredIterate);
                     }

                     return GetStaticMethodInfo(methodName, genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredStringMethodInfo(this Type enumtrType, Type elementType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType, typeof(string))
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     
                     if (!key.elementType.IsString()) 
                         throw new ArgumentException("Expected to receive a string enumerator type.  Got " + key.elementType.FullName);

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
                     genericParamTypes[0] = key.enumeratorType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<string>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddFilteredIterateString), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredCharSequenceMethodInfo<TFilterBase>(this Type enumtrType, Type elementType)
    {
        var filterType = typeof(TFilterBase);
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType)
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     if (!key.elementType.IsCharSequence()) 
                         throw new ArgumentException("Expected to receive a ICharSequence enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;
                     genericParamTypes[2] = key.elementType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddFilteredIterateCharSeq), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredStringBuilderMethodInfo(this Type enumtrType, Type elementType)
    {
        var filterType = typeof(StringBuilder);
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType)
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     
                     if (!key.elementType.IsStringBuilder()) 
                         throw new ArgumentException("Expected to receive a StringBuilder enumerator type.  Got " + key.elementType.FullName);

                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
                     genericParamTypes[0] = key.enumeratorType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<StringBuilder>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddFilteredIterateStringBuilder), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredMatchMethodInfo<TFilterBase>(this Type enumtrType, Type elementType)
    {
        var filterType = typeof(TFilterBase);
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType)
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;
                     genericParamTypes[2] = key.filterType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddFilteredIterateMatch), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredObjectMethodInfo(this Type enumtrType, Type elementType, Type filterType)
    {
        var methInf =
            BuiltInTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType)
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
                     genericParamTypes[0] = key.enumeratorType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<object>);
                     methodParamTypes[3] = typeof(string);
                     methodParamTypes[4] = typeof(FormatFlags);
                     methodParamTypes[5] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(AddFilteredIterateObject), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredCloakedRevealerMethodInfo<TFilterBase, TRevealBase>(this Type enumtrType, Type elementType)
        where TRevealBase : notnull
    {
        var filterType          = typeof(TFilterBase);
        var cloakedRevealerType = typeof(TRevealBase);
        var methInf =
            CloakedTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType, cloakedRevealerType)
               , static ((Type enumeratorType, Type elementType, Type filterType, Type cloakedRevealType) key) =>
                 {
                     bool isNullable = key.elementType.IsNullable();
                     var  itemType = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (isNullable || !itemType.IsAssignableTo(key.cloakedRevealType)) 
                         throw new ArgumentException
                             ( $"Expected to receive a non nullable Type compatible with {key.cloakedRevealType} enumerator type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = key.elementType;
                     genericParamTypes[2] = key.filterType;
                     genericParamTypes[3] = key.cloakedRevealType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
                     methodParamTypes[3] = typeof(PalantírReveal<TRevealBase>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);
                     methodParamTypes[6] = typeof(bool?);

                     return GetStaticMethodInfo(nameof(RevealFilteredIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    internal static MethodInfo GetAddFilteredNullableCloakedRevealerMethodInfo<TCloakedStruct>(this Type enumtrType, Type elementType)
        where TCloakedStruct : struct
    {
        var filterType = typeof(TCloakedStruct?);
        var methInf =
            NullableCloakedTypeInvoke.GetOrAdd
                ((enumtrType, elementType, filterType)
               , static ((Type enumeratorType, Type elementType, Type filterType) key) =>
                 {
                     bool isNullable = key.elementType.IsNullable();
                     
                     var itemType = key.elementType.IfNullableGetUnderlyingTypeOrThis();
                     
                     if (!isNullable) 
                         throw new ArgumentException
                             ( $"Expected to receive a nullable Type struct type.  Got " + key.elementType.FullName);


                     using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
                     genericParamTypes[0] = key.enumeratorType;
                     genericParamTypes[1] = itemType;

                     using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
                     methodParamTypes[0] = typeof(ICollectionMoldWriteState);
                     methodParamTypes[1] = key.enumeratorType;
                     methodParamTypes[2] = typeof(OrderedCollectionPredicate<TCloakedStruct?>);
                     methodParamTypes[3] = typeof(PalantírReveal<TCloakedStruct>);
                     methodParamTypes[4] = typeof(string);
                     methodParamTypes[5] = typeof(FormatFlags);
                     methodParamTypes[6] = typeof(bool?);
                     
                     return GetStaticMethodInfo(nameof(RevealFilteredIterateNullable), genericParamTypes.AsArray, methodParamTypes.AsArray);
                 });
        return methInf;
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateSingleToDoubleGenericDelegate<TEnumtr, TFilterBase>(
        Type enumblParamType, Type enumtrType, Type elementType, string toInvokeMethodName)
        where TEnumtr : IEnumerator?
    {
        var itemType   = elementType.IfNullableGetUnderlyingTypeOrThis();
        var filterType = typeof(TFilterBase);
        
        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
        genericParamTypes[0] = enumtrType;
        genericParamTypes[1] = itemType;
        genericParamTypes[2] = filterType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumtrType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);
        methodParamTypes[5] = typeof(bool?);

        var toInvokeOn = GetStaticMethodInfo(toInvokeMethodName, genericParamTypes.AsArray, methodParamTypes.AsArray);

        var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllSingleToDoubleGenericEnumerableInvoker)));
        genericParamTypes[0] = enumblParamType;
        genericParamTypes[0] = elementType;
        var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;

        using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
        invokeReflectedArgs[0] = toInvokeOn;
        invokeReflectedArgs[1] = enumblParamType;
        invokeReflectedArgs[2] = enumtrType;
        invokeReflectedArgs[3] = methodParamTypes.AsArray;

        return (InputTypeInvoke<TEnumtr, TFilterBase>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> BuildAddAllSingleToDoubleGenericEnumerableInvoker<TEnumtr, TElement, TFilterBase>(
        MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator<TElement>?
        where TElement : TFilterBase?
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumtr, TFilterBase>));
        var createInvoker = (InputTypeInvoke<TEnumtr, TFilterBase>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumtr? enumbl, OrderedCollectionPredicate<TFilterBase> filterPredicate
          , string? valueFmtStr = null, FormatFlags flags = DefaultCallerTypeFlags, bool? hasValue = null) =>
            createInvoker(mws, enumbl, filterPredicate, valueFmtStr, flags);
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateSingleToSingleGenericInvokerDelegate<TEnumtr, TFilterBase>(
        Type enumblParamType, Type enumblType, Type elementType, Type filterType, string toInvokeMethodName)
        where TEnumtr : IEnumerator?
    {
        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
        genericParamTypes[0] = enumblType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);
        methodParamTypes[5] = typeof(bool?);

        var toInvokeOn              = GetStaticMethodInfo(toInvokeMethodName, genericParamTypes.AsArray, methodParamTypes.AsArray);
        
        var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllSingleToSingleGenericEnumerableInvoker)));
        using var invokeGenericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
        invokeGenericParamTypes[0] = enumblParamType;
        invokeGenericParamTypes[1] = elementType;
        invokeGenericParamTypes[2] = filterType;
        var concreteGenMethod = genGenMethod.MakeGenericMethod(invokeGenericParamTypes.AsArray);

        methodParamTypes[1]  = enumblParamType;

        using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
        invokeReflectedArgs[0] = toInvokeOn;
        invokeReflectedArgs[1] = enumblParamType;
        invokeReflectedArgs[2] = enumblType;
        invokeReflectedArgs[3] = methodParamTypes.AsArray;

        return (InputTypeInvoke<TEnumtr, TFilterBase>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> BuildAddAllSingleToSingleGenericEnumerableInvoker<TEnumtr, TElement, TFilterBase>(
        MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator<TElement>?
        where TElement : TFilterBase?
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
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumtr, TFilterBase>));
        var createInvoker = (InputTypeInvoke<TEnumtr, TFilterBase>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumtr? enumbl, OrderedCollectionPredicate<TFilterBase> filterPredicate
          , string? valueFmtStr = null, FormatFlags flags = DefaultCallerTypeFlags, bool? hasValues = null) =>
            createInvoker(mws, enumbl, filterPredicate, valueFmtStr, flags, hasValues);
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateAddAllBoolDelegate<TEnumtr, TFilterBase>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        var isNullable  = elementType.IsNullable();
        var filterType  = typeof(TFilterBase);
        var itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
        if (!itemType.IsBool()) throw new ArgumentException("Expected to receive a Boolean(?) collection");

        var toInvokeMethodName = isNullable ? nameof(AddFilteredIterateNullableBool) : nameof(AddFilteredIterateBool);

        return CreateSingleToSingleGenericInvokerDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType, elementType, filterType, toInvokeMethodName);
    }
    
    internal static InputTypeInvoke<TEnumtr, TFilterBase> GetAddAllSpanFormattable<TEnumtr, TFilterBase>(Type enumblType)
        where TEnumtr : IEnumerator?
    {
        var enumblParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumtr, TFilterBase>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllSpanFormattableDelegate<TEnumtr, TFilterBase>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateAddAllSpanFormattableDelegate<TEnumtr, TFilterBase>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        var isNullable  = elementType.IsNullable();
        var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
        if (!itemType.IsSpanFormattableCached()) throw new ArgumentException("Expected to receive a ISpanFormattable collection");

        var toInvokeMethodName = isNullable ? nameof(AddFilteredIterateNullable) : nameof(AddFilteredIterate);

        return CreateSingleToDoubleGenericDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType, elementType, toInvokeMethodName);
    }

    private static CloakedRevealerInvoker<TEnumtr, TFilterBase, TRevealBase> GetAddAllCloakedRevealer<TEnumtr, TFilterBase, TRevealBase>(Type enumblType)
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumtr);
        var revealType      = typeof(TRevealBase);
        var callAsFactory   = true;
        var invoker =
            (CloakedRevealerInvoker<TEnumtr, TFilterBase, TRevealBase>)
            CloakedInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType, revealType)
                   , static ((Type enumblParamType, Type enumblType, Type revealType) key, bool _) => 
                         CreateAddAllCloakedRevealerDelegate<TEnumtr, TFilterBase, TRevealBase>(key.enumblParamType, key.enumblType, key.revealType)
                   , callAsFactory);
        return invoker;
    }

    private static CloakedRevealerInvoker<TEnumtr, TFilterBase, TRevealBase> CreateAddAllCloakedRevealerDelegate<TEnumtr, TFilterBase, TRevealBase>
        (Type enumblParamType, Type enumblType, Type revealType) 
        where TEnumtr : IEnumerator? 
        where TRevealBase : notnull
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
                     
        if (!elementType.IsAssignableTo(revealType)) 
            throw new ArgumentException($"Expected to receive a enumerable element " +
                                        $"{elementType.Name} to be equatable to {revealType.Name}");

        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
        genericParamTypes[0] = enumblType;
        genericParamTypes[1] = elementType;
        genericParamTypes[2] = revealType;
        genericParamTypes[3] = revealType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(7);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(PalantírReveal<TRevealBase>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);
        methodParamTypes[6] = typeof(bool?);

        var toInvokeOn = GetStaticMethodInfo(nameof(RevealFilteredIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);

        var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllCloakedRevealerInvoker)));
        genericParamTypes[0] = enumblParamType;
        var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;

        using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
        invokeReflectedArgs[0] = toInvokeOn;
        invokeReflectedArgs[1] = enumblParamType;
        invokeReflectedArgs[2] = enumblType;
        invokeReflectedArgs[3] = methodParamTypes.AsArray;

        return (CloakedRevealerInvoker<TEnumtr, TFilterBase, TRevealBase>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static CloakedRevealerInvoker<TEnumtr, TFilterBase, TRevealBase> BuildAddAllCloakedRevealerInvoker<TEnumtr, TElement, TFilterBase, TRevealBase>(
        MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumtr : IEnumerator<TElement>?
        where TElement : TFilterBase?, TRevealBase?
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
        ilGenerator.Emit(OpCodes.Ldarg_S, 6);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CloakedRevealerInvoker<TEnumtr, TElement, TFilterBase, TRevealBase>));
        var createInvoker = (CloakedRevealerInvoker<TEnumtr, TElement, TFilterBase, TRevealBase>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumtr? enumbl, OrderedCollectionPredicate<TFilterBase> filterPredicate
          , PalantírReveal<TRevealBase> revealer, string? valueFmtStr = null, FormatFlags flags = DefaultCallerTypeFlags, bool? hasValue = null) =>
            createInvoker(mws, enumbl, filterPredicate, revealer, valueFmtStr, flags, hasValue);
    }

    internal static InputTypeInvoke<TEnumtr, TFilterBase> GetAddAllStringBearer<TEnumtr, TFilterBase>(Type enumblType)
        where TEnumtr : IEnumerator?
    {
        var enumblParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumtr, TFilterBase>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllStringBearerDelegate<TEnumtr, TFilterBase>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateAddAllStringBearerDelegate<TEnumtr, TFilterBase>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        bool isNullable  = elementType.IsNullable();
        var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
        if (!itemType.IsStringBearer()) throw new ArgumentException("Expected to receive a IStringBearer collection");

        string toInvokeMethodName = isNullable ? nameof(RevealFilteredIterateNullable) : nameof(RevealFilteredIterate);

        return CreateSingleToDoubleGenericDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType, elementType, toInvokeMethodName);
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateAddAllStringDelegate<TEnumtr, TFilterBase>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         
        if (!elementType.IsString()) throw new ArgumentException("Expected to receive a string collection");

        return CreateSingleToSingleGenericInvokerDelegate<TEnumtr, TFilterBase>
            (enumblParamType, enumblType, elementType, elementType, nameof(AddFilteredIterateString));
    }

    internal static InputTypeInvoke<TEnumtr, TFilterBase> GetAddAllCharSequence<TEnumtr, TFilterBase>(Type enumblType)
        where TEnumtr : IEnumerator?
    {
        var enumblParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumtr, TFilterBase>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllCharSequenceDelegate<TEnumtr, TFilterBase>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateAddAllCharSequenceDelegate<TEnumtr, TFilterBase>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
                         
        if (!elementType.IsCharSequence()) throw new ArgumentException("Expected to receive a ICharSequence collection");

        return CreateSingleToDoubleGenericDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType, elementType, nameof(AddFilteredIterateCharSeq));
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateAddAllStringBuilderDelegate<TEnumtr, TFilterBase>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         
        if (!elementType.IsStringBuilder()) throw new ArgumentException("Expected to receive a StringBuilder collection");

        return CreateSingleToSingleGenericInvokerDelegate<TEnumtr, TFilterBase>
            (enumblParamType, enumblType, elementType, elementType, nameof(AddFilteredIterateStringBuilder));
    }

    internal static InputTypeInvoke<TEnumtr, TFilterBase> GetAddAllMatch<TEnumtr, TFilterBase>(Type enumblType)
        where TEnumtr : IEnumerator?
    {
        var enumblParamType = typeof(TEnumtr);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumtr, TFilterBase>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllMatchDelegate<TEnumtr, TFilterBase>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumtr, TFilterBase> CreateAddAllMatchDelegate<TEnumtr, TFilterBase>(Type enumblParamType, Type enumblType) 
        where TEnumtr : IEnumerator?
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        var itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();

        if (itemType.IsSpanFormattable())
        {
            return CreateAddAllSpanFormattableDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType);
        }
        if (itemType.IsStringBearer())
        {
            return CreateAddAllStringBearerDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType);
        }
        if (itemType.IsString())
        {
            return CreateAddAllStringDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType);
        }
        if (itemType.IsStringBuilder())
        {
            return CreateAddAllStringBuilderDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType);
        }
        if (itemType.IsCharSequence())
        {
            return CreateAddAllCharSequenceDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType);
        }
        if (itemType.IsBool())
        {
            return CreateAddAllBoolDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType);
        }

        return CreateSingleToDoubleGenericDelegate<TEnumtr, TFilterBase>(enumblParamType, enumblType, elementType, nameof(AddFilteredIterateMatch));
    }

    internal static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(OrderedCollectionAddFilteredIterateExtensions).GetMethods( NonPublic | Public | Static);

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

    public static void AddFilteredIterateBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<bool>
    {
        if (value != null)
        {
            mws.AddFilteredIterateBool(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(bool);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<bool> filterPredicate
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
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = 
                mws.ConditionalCollectionPrefix
                    (value, elementType, false, new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredIterateNullableBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<bool?>
    {
        if (value != null)
        {
            mws.AddFilteredIterateNullableBool(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(bool?);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateNullableBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<bool?> filterPredicate
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
        var  elementType     = typeof(bool?);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) mws.ConditionalCollectionPrefix(value, elementType, false
                                                    , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredIterate<TEnumtr, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.AddFilteredIterate(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TFmtBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterate<TEnumtr, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllSpanFormattable<TEnumtr, TFmtBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags, hasValue);
    }

    public static void AddFilteredIterate<TEnumtr, TFmt, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TFmt>
        where TFmt : TFmtBase?, ISpanFormattable?
    {
        if (value != null)
        {
            mws.AddFilteredIterate<TEnumtr, TFmt, TFmtBase>(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TFmt);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterate<TEnumtr, TFmt, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TFmt>?
        where TFmt : TFmtBase?, ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TFmt);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredIterateNullable<TEnumtr, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.AddFilteredIterateNullable(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TFmtBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateNullable<TEnumtr, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllSpanFormattable<TEnumtr, TFmtBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags, hasValue);
    }

    public static void AddFilteredIterateNullable<TEnumtr, TFmtStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable
    {
        if (value != null)
        {
            mws.AddFilteredIterateNullable(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TFmtStruct);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateNullable<TEnumtr, TFmtStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
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
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealFilteredIterate<TEnumtr, TCloakedFilterBase, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
        where TRevealBase : notnull
    {
        if (value != null)
        {
            mws.RevealFilteredIterate(value.Value, filterPredicate, palantírReveal, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TRevealBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredIterate<TEnumtr, TCloakedFilterBase, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllCloakedRevealer<TEnumtr, TCloakedFilterBase, TRevealBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, palantírReveal, formatString, formatFlags, hasValue);
    }

    public static void RevealFilteredIterate<TEnumtr, TCloaked, TCloakedFilterBase, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCloaked>
        where TCloaked : TCloakedFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        if (value != null)
        {
            mws.RevealFilteredIterate<TEnumtr, TCloaked, TCloakedFilterBase, TRevealBase>
                (value.Value, filterPredicate, palantírReveal, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TRevealBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredIterate<TEnumtr, TCloaked, TCloakedFilterBase, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TCloaked>?
        where TCloaked : TCloakedFilterBase?, TRevealBase?
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
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCloakedStruct?>
        where TCloakedStruct : struct
    {
        if (value != null)
        {
            mws.RevealFilteredIterateNullable(value.Value, filterPredicate, palantírReveal, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TCloakedStruct);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
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
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealFilteredIterate<TEnumtr, TFilterBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.RevealFilteredIterate(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TFilterBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredIterate<TEnumtr, TFilterBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllStringBearer<TEnumtr, TFilterBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags, hasValue);
    }

    public static void RevealFilteredIterate<TEnumtr, TBearer, TFilterBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TBearer?>
        where TBearer : TFilterBase?, IStringBearer?
    {
        if (value != null)
        {
            mws.RevealFilteredIterate<TEnumtr, TBearer, TFilterBase>(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TFilterBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredIterate<TEnumtr, TBearer, TFilterBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TBearer?>?
        where TBearer : TFilterBase?, IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TBearer);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealFilteredIterateNullable<TEnumtr, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer
    {
        if (value != null)
        {
            mws.RevealFilteredIterateNullable(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TBearerStruct);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredIterateNullable<TEnumtr, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
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
        var  elementType     = typeof(TBearerStruct);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredIterateString<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<string> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<string?>
    {
        if (value != null)
        {
            mws.AddFilteredIterateString(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateString<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<string> filterPredicate
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
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredIterateCharSeq<TEnumtr, TCharSeqBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.AddFilteredIterateCharSeq(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TCharSeqBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateCharSeq<TEnumtr, TCharSeqBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllCharSequence<TEnumtr, TCharSeqBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags, hasValue);
    }

    public static void AddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCharSeq>
        where TCharSeq : TCharSeqBase?, ICharSequence?
    {
        if (value != null)
        {
            mws.AddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>
                (value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TCharSeq);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TCharSeq>?
        where TCharSeq : TCharSeqBase?, ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TCharSeq);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredIterateStringBuilder<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<StringBuilder?>
    {
        if (value != null)
        {
            mws.AddFilteredIterateStringBuilder(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(StringBuilder);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateStringBuilder<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
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
        var  elementType     = typeof(StringBuilder);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredIterateMatch<TEnumtr, TAnyBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            mws.AddFilteredIterateMatch(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TAnyBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateMatch<TEnumtr, TAnyBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumtr);
        var callGenericInvoker = GetAddAllMatch<TEnumtr, TAnyBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags, hasValue);
    }

    public static void AddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TAny?>
        where TAny : TAnyBase?
    {
        if (value != null)
        {
            mws.AddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>
                (value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(TAnyBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TAny?>?
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TAny);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    [CallsObjectToString]
    public static void AddFilteredIterateObject<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<object> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<object?>
    {
        if (value != null)
        {
            mws.AddFilteredIterateMatch(value.Value, filterPredicate, formatString, formatFlags, hasValue);
            return;
        }
        var elementType = typeof(object);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    [CallsObjectToString]
    public static void AddFilteredIterateObject<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , OrderedCollectionPredicate<object> filterPredicate
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
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            while (hasValue.Value)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true
                                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                                , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
            collectionItems = count;
        }
        value?.Dispose();
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }
}
