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
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public static class OrderedCollectionAddAllEnumerateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly ConcurrentDictionary<(Type, Type), Delegate> InputTypeInvokeCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> CloakedInvokeCache = new();
    
    private static readonly ConcurrentDictionary<(Type, Type, Type, MethodInfo), Delegate> InputTypeCallStructEnumeratorCache = new();

    internal delegate void InputTypeInvoke<in TEnumbl>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?;

    internal delegate void InputTypeInvoke<in TEnumbl, TElement>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TElement>?;

    internal delegate void CloakedRevealerInvoker<in TEnumbl, out TRevealBase>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , PalantírReveal<TRevealBase> cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull;

    internal delegate void CloakedRevealerInvoker<in TEnumbl, TElement, out TRevealBase>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , PalantírReveal<TRevealBase>? cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TRevealBase?
        where TRevealBase : notnull;

    internal delegate void NullableCloakedRevealerInvoker<in TEnumbl, out TElement>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , PalantírReveal<TElement>? cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct;


    private static InputTypeInvoke<TEnumbl> CreateInputTypeInvokerDelegate<TEnumbl>(Type enumblParamType, Type enumblType, Type elementType
      , string toInvokeMethodName)
        where TEnumbl : IEnumerable?
    {
        var itemType = elementType.IfNullableGetUnderlyingTypeOrThis();
        
        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
        genericParamTypes[0] = enumblType;
        genericParamTypes[1] = itemType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(string);
        methodParamTypes[3] = typeof(FormatFlags);

        var toInvokeOn = GetStaticMethodInfo(toInvokeMethodName, genericParamTypes.AsArray, methodParamTypes.AsArray);

        var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllSingleToDoubleGenericEnumerableInvoker)));
        genericParamTypes[0] = enumblParamType;
        genericParamTypes[1] = elementType;
        var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;

        using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
        invokeReflectedArgs[0] = toInvokeOn;
        invokeReflectedArgs[1] = enumblParamType;
        invokeReflectedArgs[2] = enumblType;
        invokeReflectedArgs[3] = methodParamTypes.AsArray;

        return (InputTypeInvoke<TEnumbl>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static InputTypeInvoke<TEnumbl> BuildAddAllSingleToDoubleGenericEnumerableInvoker<TEnumbl, TElement>(MethodInfo methodInfo, Type enumblParamType
      , Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<TElement>?
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
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumbl>));
        var createInvoker = (InputTypeInvoke<TEnumbl>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumbl? enumbl, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(mws, enumbl, valueFmtStr, flags);
    }

    private static InputTypeInvoke<TEnumbl> CreateSingleInputTypeInvokerDelegate<TEnumbl>(Type enumblParamType, Type enumblType, Type itemType
      , string toInvokeMethodName)
        where TEnumbl : IEnumerable?
    {
        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
        genericParamTypes[0] = enumblType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(string);
        methodParamTypes[3] = typeof(FormatFlags);

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

        return (InputTypeInvoke<TEnumbl>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static InputTypeInvoke<TEnumbl> BuildAddAllSingleToSingleGenericEnumerableInvoker<TEnumbl, TElement>(MethodInfo methodInfo, Type enumblParamType
      , Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<TElement>?
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
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumbl>));
        var createInvoker = (InputTypeInvoke<TEnumbl>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumbl? enumbl, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(mws, enumbl, valueFmtStr, flags);
    }

    private static InputTypeInvoke<TEnumbl> CreateAddAllBoolDelegate<TEnumbl>(Type enumblParamType, Type enumblType) 
        where TEnumbl : IEnumerable?
    {
         var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         bool isNullable  = elementType.IsNullable();
         var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
         if (!itemType.IsBool()) throw new ArgumentException("Expected to receive a Boolean(?) collection");

         string toInvokeMethodName = isNullable ? nameof(AddAllEnumerateNullableBool) : nameof(AddAllEnumerateBool);

         return CreateSingleInputTypeInvokerDelegate<TEnumbl>(enumblParamType, enumblType, elementType, toInvokeMethodName);
    }

    internal static InputTypeInvoke<TEnumbl> GetAddAllSpanFormattable<TEnumbl>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllSpanFormattableDelegate<TEnumbl>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl> CreateAddAllSpanFormattableDelegate<TEnumbl>(Type enumblParamType, Type enumblType) 
        where TEnumbl : IEnumerable?
    {
         var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         bool isNullable  = elementType.IsNullable();
         var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
         if (!itemType.IsSpanFormattableCached()) 
             throw new ArgumentException($"Expected to receive a ISpanFormattable collection. Got {itemType.Name}");

         string toInvokeMethodName = isNullable ? nameof(AddAllEnumerateNullable) : nameof(AddAllEnumerate);

         return CreateInputTypeInvokerDelegate<TEnumbl>(enumblParamType, enumblType, elementType, toInvokeMethodName);
    }

    private static CloakedRevealerInvoker<TEnumbl, TRevealBase> GetAddAllCloakedRevealer<TEnumbl, TRevealBase>(Type enumblType)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var revealType      = typeof(TRevealBase);
        var callAsFactory   = true;
        var invoker =
            (CloakedRevealerInvoker<TEnumbl, TRevealBase>)
            CloakedInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType, revealType)
                   , static ((Type enumblParamType, Type enumblType, Type revealType) key, bool _) => 
                         CreateAddAllCloakedRevealerDelegate<TEnumbl, TRevealBase>(key.enumblParamType, key.enumblType, key.revealType)
                   , callAsFactory);
        return invoker;
    }

    private static CloakedRevealerInvoker<TEnumbl, TRevealBase> CreateAddAllCloakedRevealerDelegate<TEnumbl, TRevealBase>
        (Type enumblParamType, Type enumblType, Type revealType) 
        where TEnumbl : IEnumerable? 
        where TRevealBase : notnull
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
                     
        if (!elementType.IsAssignableTo(revealType)) 
            throw new ArgumentException($"Expected to receive a enumerable element " +
                                        $"{elementType.ShortNameInCSharpFormat()} to be equatable to {revealType.ShortNameInCSharpFormat()}");

        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
        genericParamTypes[0] = enumblType;
        genericParamTypes[1] = elementType;
        genericParamTypes[2] = revealType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(PalantírReveal<TRevealBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var toInvokeOn = GetStaticMethodInfo(nameof(RevealAllEnumerate), genericParamTypes.AsArray, methodParamTypes.AsArray);

        var genGenMethod = myMethodInfosCached!.First(mi => mi.Name.Contains(nameof(BuildAddAllCloakedRevealerInvoker)));
        genericParamTypes[0] = enumblParamType;
        var concreteGenMethod = genGenMethod.MakeGenericMethod(genericParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;

        using var invokeReflectedArgs = RecyclingArrays.GetReusableArrayOf<object>(4);
        invokeReflectedArgs[0] = toInvokeOn;
        invokeReflectedArgs[1] = enumblParamType;
        invokeReflectedArgs[2] = enumblType;
        invokeReflectedArgs[3] = methodParamTypes.AsArray;

        return (CloakedRevealerInvoker<TEnumbl, TRevealBase>)concreteGenMethod.Invoke(null, invokeReflectedArgs.AsArray)!;
    }

    private static CloakedRevealerInvoker<TEnumbl, TRevealBase> BuildAddAllCloakedRevealerInvoker<TEnumbl, TElement, TRevealBase>(
        MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable<TElement>?
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
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CloakedRevealerInvoker<TEnumbl, TElement, TRevealBase>));
        var createInvoker = (CloakedRevealerInvoker<TEnumbl, TElement, TRevealBase>)methodInvoker;

        return Wrapped;

        void Wrapped(ICollectionMoldWriteState mws, TEnumbl? enumbl, PalantírReveal<TRevealBase> revealer, string? valueFmtStr, FormatFlags flags) =>
            createInvoker(mws, enumbl, revealer, valueFmtStr, flags);
    }

    internal static InputTypeInvoke<TEnumbl> GetAddAllStringBearer<TEnumbl>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllStringBearerDelegate<TEnumbl>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl> CreateAddAllStringBearerDelegate<TEnumbl>(Type enumblParamType, Type enumblType) 
        where TEnumbl : IEnumerable?
    {
         var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         bool isNullable  = elementType.IsNullable();
         var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();
         
         if (!itemType.IsStringBearer()) throw new ArgumentException("Expected to receive a IStringBearer collection");

         string toInvokeMethodName = isNullable ? nameof(RevealAllEnumerateNullable) : nameof(RevealAllEnumerate);

         return CreateInputTypeInvokerDelegate<TEnumbl>(enumblParamType, enumblType, elementType, toInvokeMethodName);
    }

    private static InputTypeInvoke<TEnumbl> CreateAddAllStringDelegate<TEnumbl>(Type enumblParamType, Type enumblType) 
        where TEnumbl : IEnumerable?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         
        if (!elementType.IsString()) throw new ArgumentException("Expected to receive a string collection");

        return CreateSingleInputTypeInvokerDelegate<TEnumbl>(enumblParamType, enumblType, elementType, nameof(AddAllEnumerateString));
    }

    internal static InputTypeInvoke<TEnumbl> GetAddAllCharSequence<TEnumbl>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllCharSequenceDelegate<TEnumbl>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl> CreateAddAllCharSequenceDelegate<TEnumbl>(Type enumblParamType, Type enumblType) 
        where TEnumbl : IEnumerable?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
                         
        if (!elementType.IsCharSequence()) throw new ArgumentException("Expected to receive a ICharSequence collection");

        return CreateInputTypeInvokerDelegate<TEnumbl>(enumblParamType, enumblType, elementType, nameof(AddAllEnumerateCharSeq));
    }

    private static InputTypeInvoke<TEnumbl> CreateAddAllStringBuilderDelegate<TEnumbl>(Type enumblParamType, Type enumblType) 
        where TEnumbl : IEnumerable?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
         
        if (!elementType.IsStringBuilder()) throw new ArgumentException("Expected to receive a StringBuilder collection");

        return CreateSingleInputTypeInvokerDelegate<TEnumbl>(enumblParamType, enumblType, elementType, nameof(AddAllEnumerateStringBuilder));
    }

    internal static InputTypeInvoke<TEnumbl> GetAddAllMatch<TEnumbl>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType)
                   , static ((Type enumblParamType, Type enumblType) key, bool _) => 
                         CreateAddAllMatchDelegate<TEnumbl>(key.enumblParamType, key.enumblType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl> CreateAddAllMatchDelegate<TEnumbl>(Type enumblParamType, Type enumblType) 
        where TEnumbl : IEnumerable?
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        var itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();

        if (itemType.IsSpanFormattable())
        {
            return GetAddAllSpanFormattable<TEnumbl>(enumblType);
        }
        if (itemType.IsStringBearer())
        {
            return GetAddAllStringBearer<TEnumbl>(enumblType);
        }
        if (itemType.IsString())
        {
            return CreateAddAllStringDelegate<TEnumbl>(enumblParamType, enumblType);
        }
        if (itemType.IsStringBuilder())
        {
            return CreateAddAllStringBuilderDelegate<TEnumbl>(enumblParamType, enumblType);
        }
        if (itemType.IsCharSequence())
        {
            return GetAddAllCharSequence<TEnumbl>(enumblType);
        }
        if (itemType.IsBool())
        {
            return CreateAddAllBoolDelegate<TEnumbl>(enumblParamType, enumblType);
        }

        return CreateInputTypeInvokerDelegate<TEnumbl>(enumblParamType, enumblType, elementType, nameof(AddAllEnumerateMatch));
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllBoolCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllBoolMethodInfo(typeof(TElement));
        return enumblType.GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllSpanFormattableCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllSpanFormattableMethodInfo(typeof(TElement));
        return enumblType.GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllStringBearerCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllStringBearerMethodInfo(typeof(TElement));
        return enumblType.GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllStringCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllStringMethodInfo(typeof(TElement));
        return enumblType.GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllCharSequenceCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllCharSequenceMethodInfo(typeof(TElement));
        return enumblType.GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllMatchCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllMatchMethodInfo(typeof(TElement));
        return enumblType.GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllObjectCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllObjectMethodInfo(typeof(TElement));
        return enumblType.GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllStringBuilderCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllStringBuilderMethodInfo(typeof(TElement));
        return enumblType.GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement> GetAddAllBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType, MethodInfo enumeratorMethodInf)
        where TEnumbl : IEnumerable<TElement>?
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl, TElement>)
            InputTypeCallStructEnumeratorCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, enumeratorMethodInf)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, MethodInfo enumtrMethInf) key, bool _) =>
                              key.enumblType.BuildAddAllNoRevealersCallStructEnumtr
                                  <TEnumbl, TElement>(key.enumblParamType, key.enumeratorType, key.enumtrMethInf), callAsFactory);
        return invoker;
    }
    
    private static InputTypeInvoke<TEnumbl, TElement> BuildAddAllNoRevealersCallStructEnumtr<TEnumbl, TElement>
        (this Type enumblType, Type enumblParamType, Type enumeratorType, MethodInfo callEnumtrInvokeMethInf)
        where TEnumbl : IEnumerable<TElement>?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(string);
        methodParamTypes[3] = typeof(FormatFlags);

        var hasValuesType = typeof(bool?);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", null,
                 methodParamTypes.AsArray
               , typeof(OrderedCollectionAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        var hasValues =  ilGenerator.DeclareLocal(hasValuesType);
        
        ilGenerator.Emit(OpCodes.Ldloca_S, hasValues);
        ilGenerator.Emit(OpCodes.Initobj, hasValuesType);
        ilGenerator.Emit(OpCodes.Ldarg_1);
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

        // call AddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldloc, hasValues);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumbl, TElement>));
        return (InputTypeInvoke<TEnumbl, TElement>)methodInvoker;
    }

    private static CloakedRevealerInvoker<TEnumbl, TElement, TRevealBase> GetAddAllCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement, TRevealBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TRevealBase?
        where TRevealBase : notnull
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllCloakedRevealerMethodInfo<TRevealBase>(typeof(TElement));
        return enumblType.GetAddAllCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement, TRevealBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static CloakedRevealerInvoker<TEnumbl, TElement, TRevealBase> GetAddAllCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement, TRevealBase>
        (this Type enumblType, Type enumtrType, MethodInfo enumeratorMethodInf)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TRevealBase?
        where TRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (CloakedRevealerInvoker<TEnumbl, TElement, TRevealBase>)
            InputTypeCallStructEnumeratorCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, enumeratorMethodInf)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, MethodInfo enumtrMethInf) key, bool _) =>
                              key.enumblType.BuildAddAllCloakedRevealerCallStructEnumtr
                                  <TEnumbl, TElement, TRevealBase>(key.enumblParamType, key.enumeratorType, key.enumtrMethInf), callAsFactory);
        return invoker;
    }
    
    private static CloakedRevealerInvoker<TEnumbl, TElement, TRevealBase> BuildAddAllCloakedRevealerCallStructEnumtr<TEnumbl, TElement, TRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType, MethodInfo callEnumtrInvokeMethInf)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TRevealBase?
        where TRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TRevealBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var hasValuesType = typeof(bool?);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", null,
                 methodParamTypes.AsArray
               , typeof(OrderedCollectionAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        var hasValues =  ilGenerator.DeclareLocal(hasValuesType);
        
        ilGenerator.Emit(OpCodes.Ldloca_S, hasValues);
        ilGenerator.Emit(OpCodes.Initobj, hasValuesType);
        ilGenerator.Emit(OpCodes.Ldarg_1);
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

        // call AddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldloc, hasValues);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CloakedRevealerInvoker<TEnumbl, TElement, TRevealBase>));
        return (CloakedRevealerInvoker<TEnumbl, TElement, TRevealBase>)methodInvoker;
    }

    private static NullableCloakedRevealerInvoker<TEnumbl, TElement> GetAddAllNullableCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct
    {
        var callEnumtrInvokeMethInf  = enumtrType.GetAddAllNullableCloakedRevealerMethodInfo<TElement>(typeof(TElement?));
        return enumblType.GetAddAllNullableCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static NullableCloakedRevealerInvoker<TEnumbl, TElement> GetAddAllNullableCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType, MethodInfo enumeratorMethodInf)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (NullableCloakedRevealerInvoker<TEnumbl, TElement>)
            InputTypeCallStructEnumeratorCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, enumeratorMethodInf)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, MethodInfo enumtrMethInf) key, bool _) =>
                              key.enumblType.BuildAddAllNullableCloakedRevealerCallStructEnumtr
                                  <TEnumbl, TElement>(key.enumblParamType, key.enumeratorType, key.enumtrMethInf), callAsFactory);
        return invoker;
    }
    
    private static NullableCloakedRevealerInvoker<TEnumbl, TElement> BuildAddAllNullableCloakedRevealerCallStructEnumtr<TEnumbl, TElement>(
            this Type enumblType, Type enumblParamType, Type enumeratorType, MethodInfo callEnumtrInvokeMethInf)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(PalantírReveal<TElement>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var hasValuesType = typeof(bool?);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", null,
                 methodParamTypes.AsArray
               , typeof(OrderedCollectionAddAllEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        var hasValues =  ilGenerator.DeclareLocal(hasValuesType);
        
        ilGenerator.Emit(OpCodes.Ldloca_S, hasValues);
        ilGenerator.Emit(OpCodes.Initobj, hasValuesType);
        ilGenerator.Emit(OpCodes.Ldarg_1);
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

        // call AddAllIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldloc, hasValues);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NullableCloakedRevealerInvoker<TEnumbl, TElement>));
        return (NullableCloakedRevealerInvoker<TEnumbl, TElement>)methodInvoker;
    }

    private static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(OrderedCollectionAddAllEnumerateExtensions).GetMethods( NonPublic | Public | Static);

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
            var enumeratorType = checkParameterInfos[1].ParameterType;
            if (enumeratorType.IsNullable()) continue;
            var isParameterMatch = true;
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

    public static void AddAllEnumerateBool<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool>
    {
        if (value != null)
        {
            mws.AddAllEnumerateBool(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(bool);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateBool<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(bool);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = actualType.GetAddAllBoolCallStructEnumtrInvoker<TEnumbl, bool>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllEnumerateNullableBool<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool?>
    {
        if (value != null)
        {
            mws.AddAllEnumerateNullableBool(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(bool?);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateNullableBool<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(bool);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = actualType.GetAddAllBoolCallStructEnumtrInvoker<TEnumbl, bool?>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllEnumerate<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            mws.AddAllEnumerate(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerate<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddAllSpanFormattable<TEnumbl>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags);
    }

    public static void AddAllEnumerate<TEnumbl, TFmt>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmt>
        where TFmt : ISpanFormattable?
    {
        if (value != null)
        {
            mws.AddAllEnumerate<TEnumbl, TFmt>(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerate<TEnumbl, TFmt>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmt?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TFmt);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                             , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllSpanFormattableCallStructEnumtrInvoker<TEnumbl, TFmt>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllEnumerateNullable<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        AddAllEnumerate(mws, value, formatString, formatFlags);
    }

    public static void AddAllEnumerateNullable<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        AddAllEnumerate(mws, value, formatString, formatFlags);
    }

    public static void AddAllEnumerateNullable<TEnumbl, TFmtStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable
    {
        if (value != null)
        {
            mws.AddAllEnumerateNullable<TEnumbl, TFmtStruct>(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateNullable<TEnumbl, TFmtStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TFmtStruct?);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllSpanFormattableCallStructEnumtrInvoker<TEnumbl, TFmtStruct?>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllEnumerate<TEnumbl, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull
    {
        if (value != null)
        {
            mws.RevealAllEnumerate(value.Value, palantírReveal, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TRevealBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllEnumerate<TEnumbl, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddAllCloakedRevealer<TEnumbl, TRevealBase>(actualType);
        callGenericInvoker(mws, value, palantírReveal, formatString, formatFlags);
    }

    public static void RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (value != null)
        {
            mws.RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(value.Value, palantírReveal, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TRevealBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(
        this ICollectionMoldWriteState mws,
        TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloaked?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TCloaked);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;
                    
                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TCloaked, TRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, palantírReveal, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(
        this ICollectionMoldWriteState mws,
        TEnumbl? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloakedStruct?>
        where TCloakedStruct : struct
    {
        if (value != null)
        {
            mws.RevealAllEnumerateNullable(value.Value, palantírReveal, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TCloakedStruct);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TCloakedStruct);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllNullableCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TCloakedStruct>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, palantírReveal, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllEnumerate<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            mws.RevealAllEnumerate(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllEnumerate<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddAllStringBearer<TEnumbl>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags);
    }

    public static void RevealAllEnumerate<TEnumbl, TBearer>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : IStringBearer?
    {
        if (value != null)
        {
            mws.RevealAllEnumerate<TEnumbl, TBearer>(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TBearer);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllEnumerate<TEnumbl, TBearer>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearer?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TBearer);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllStringBearerCallStructEnumtrInvoker<TEnumbl, TBearer>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllEnumerateNullable<TEnumbl, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer
    {
        if (value != null)
        {
            mws.RevealAllEnumerateNullable<TEnumbl, TBearerStruct>(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TBearerStruct);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealAllEnumerateNullable<TEnumbl, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TBearerStruct);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllStringBearerCallStructEnumtrInvoker<TEnumbl, TBearerStruct?>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllEnumerateString<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?>
    {
        if (value != null)
        {
            mws.AddAllEnumerateString(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateString<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(string);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllStringCallStructEnumtrInvoker<TEnumbl, string?>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllEnumerateCharSeq<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            mws.AddAllEnumerateCharSeq(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateCharSeq<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddAllCharSequence<TEnumbl>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags);
    }

    public static void AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : ICharSequence?
    {
        if (value != null)
        {
            mws.AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCharSeq>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TCharSeq);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllCharSequenceCallStructEnumtrInvoker<TEnumbl, TCharSeq>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, any ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllEnumerateStringBuilder<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<StringBuilder?>
    {
        if (value != null)
        {
            mws.AddAllEnumerateStringBuilder(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateStringBuilder<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(StringBuilder);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllStringBuilderCallStructEnumtrInvoker<TEnumbl, StringBuilder?>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllEnumerateMatch<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            mws.AddAllEnumerateMatch(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateMatch<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddAllMatch<TEnumbl>(actualType);
        callGenericInvoker(mws, value, formatString, formatFlags);
    }

    public static void AddAllEnumerateMatch<TEnumbl, TAny>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny>
    {
        if (value != null)
        {
            mws.AddAllEnumerateMatch<TEnumbl, TAny>(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddAllEnumerateMatch<TEnumbl, TAny>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TAny);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;

                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllMatchCallStructEnumtrInvoker<TEnumbl, TAny>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }
    
    [CallsObjectToString]
    public static void AddAllEnumerateObject<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<object?>
    {
        if (value != null)
        {
            mws.AddAllEnumerateObject(value.Value, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    [CallsObjectToString]
    public static void AddAllEnumerateObject<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(object);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if(!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                                                                , formatFlags.RemoveEmbeddedContentFlags());

                    if (valueMold.ShouldSuppressBody)
                    {
                        valueMold.Complete();
                        return;
                    }
                    mws.InnerSameAsOuterType = true;
                    
                    formatFlags |= AsEmbeddedContent;
                }
                mws.DisplayAsType = actualType;
                var structEnumtrInvoker = 
                    actualType.GetAddAllObjectCallStructEnumtrInvoker<TEnumbl, object?>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
            collectionItems = mws.ItemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? mws.ItemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }
}
