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
using static System.Reflection.BindingFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemResult;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public static class OrderedCollectionAddFilteredEnumerateExtensions
{
    private static MethodInfo[]? myMethodInfosCached;

    private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> InputTypeInvokeCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type), Delegate> CloakedInvokeCache = new();

    private static readonly ConcurrentDictionary<(Type, Type, Type, Type, MethodInfo), Delegate> InputTypeCallStructEnumeratorCache = new();
    private static readonly ConcurrentDictionary<(Type, Type, Type, MethodInfo), Delegate> InputTypeNullableStructCallStructEnumeratorCache = new();

    internal delegate void InputTypeInvoke<in TEnumbl, out TFilterBase>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?;

    internal delegate void InputTypeInvoke<in TEnumbl, TElement, out TFilterBase>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?;

    internal delegate void InputTypeNullableInvoke<in TEnumbl, TElement>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TElement?> filterPredicate
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct;

    internal delegate void CloakedRevealerInvoker<in TEnumbl, out TFilterBase, out TRevealBase>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull;

    internal delegate void CloakedRevealerInvoker<in TEnumbl, TElement, out TFilterBase, out TRevealBase>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase>? cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?, TRevealBase?
        where TRevealBase : notnull;

    internal delegate void NullableCloakedRevealerInvoker<in TEnumbl, TElement>(
        ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TElement?> filterPredicate
      , PalantírReveal<TElement>? cloakedRevealer
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct;


    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>(
        Type enumblParamType, Type enumblType, Type elementType, Type filterType, string toInvokeMethodName)
        where TEnumbl : IEnumerable?
    {
        var itemType = elementType.IfNullableGetUnderlyingTypeOrThis();

        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(3);
        genericParamTypes[0] = enumblType;
        genericParamTypes[1] = itemType;
        genericParamTypes[2] = filterType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var toInvokeOn = GetStaticMethodInfo(toInvokeMethodName, genericParamTypes.AsArray, methodParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;
        var fullGenericInvoke =
            BuildAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>(toInvokeOn, enumblParamType, enumblType, methodParamTypes.AsArray);

        return fullGenericInvoke;
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAnyEnumblToDoubleGenericInvokerDelegate<TEnumbl, TFilterBase>(
        Type enumblParamType, Type enumblType, Type elementType, string toInvokeMethodName)
        where TEnumbl : IEnumerable?
    {
        var itemType = elementType.IfNullableGetUnderlyingTypeOrThis();

        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(2);
        genericParamTypes[0] = enumblType;
        genericParamTypes[1] = itemType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var toInvokeOn = GetStaticMethodInfo(toInvokeMethodName, genericParamTypes.AsArray, methodParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;
        var fullGenericInvoke =
            BuildAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>(toInvokeOn, enumblParamType, enumblType, methodParamTypes.AsArray);

        return fullGenericInvoke;
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAnyEnumblToSingleGenericInvokerDelegate<TEnumbl, TFilterBase>(
        Type enumblParamType, Type enumblType, string toInvokeMethodName)
        where TEnumbl : IEnumerable?
    {
        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(1);
        genericParamTypes[0] = enumblType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var toInvokeOn = GetStaticMethodInfo(toInvokeMethodName, genericParamTypes.AsArray, methodParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;
        var fullGenericInvoke =
            BuildAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>(toInvokeOn, enumblParamType, enumblType, methodParamTypes.AsArray);

        return fullGenericInvoke;
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> BuildAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>(
        MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", null,
                 methodParamTypes, typeof(OrderedCollectionAddFilteredEnumerateExtensions).Module, false);
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

        // call AddFilteredEnumerate(KeyedCollectionMold, TEnumbl, valueFmtStr, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumbl, TFilterBase>));
        var createInvoker = (InputTypeInvoke<TEnumbl, TFilterBase>)methodInvoker;

        return createInvoker;
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAddFilteredBoolDelegate<TEnumbl, TFilterBase>(Type enumblParamType
      , Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        bool isNullable  = elementType.IsNullable();
        var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();

        if (!itemType.IsBool()) throw new ArgumentException("Expected to receive a Boolean(?) collection");

        string toInvokeMethodName = isNullable ? nameof(AddFilteredEnumerateNullableBool) : nameof(AddFilteredEnumerateBool);

        return CreateAnyEnumblToSingleGenericInvokerDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType, toInvokeMethodName);
    }

    internal static InputTypeInvoke<TEnumbl, TFilterBase> GetAddFilteredSpanFormattable<TEnumbl, TFilterBase>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var filterType      = typeof(TFilterBase);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl, TFilterBase>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType, filterType)
                   , static ((Type enumblParamType, Type enumblType, Type filterType) key, bool _) =>
                         CreateAddFilteredSpanFormattableDelegate<TEnumbl, TFilterBase>
                             (key.enumblParamType, key.enumblType, key.filterType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAddFilteredSpanFormattableDelegate<TEnumbl, TFilterBase>(
        Type enumblParamType, Type enumblType, Type filterType)
        where TEnumbl : IEnumerable?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        bool isNullable  = elementType.IsNullable();
        var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();

        if (!itemType.IsSpanFormattableCached()) throw new ArgumentException("Expected to receive a ISpanFormattable collection");

        if (isNullable)
        {
            return CreateAnyEnumblToDoubleGenericInvokerDelegate<TEnumbl, TFilterBase>
                (enumblParamType, enumblType, elementType, nameof(AddFilteredEnumerateNullable));
        }
        return CreateAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>
            (enumblParamType, enumblType, elementType, filterType, nameof(AddFilteredEnumerate));
    }

    private static CloakedRevealerInvoker<TEnumbl, TFilterBase, TRevealBase> GetAddFilteredCloakedRevealer<TEnumbl, TFilterBase, TRevealBase>(
        Type enumblType)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var filterType      = typeof(TFilterBase);
        var revealType      = typeof(TRevealBase);
        var callAsFactory   = true;
        var invoker =
            (CloakedRevealerInvoker<TEnumbl, TFilterBase, TRevealBase>)
            CloakedInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType, filterType, revealType)
                   , static ((Type enumblParamType, Type enumblType, Type filterType, Type revealType) key, bool _) =>
                         CreateAddFilteredCloakedRevealerDelegate<TEnumbl, TFilterBase, TRevealBase>
                             (key.enumblParamType, key.enumblType, key.filterType, key.revealType)
                   , callAsFactory);
        return invoker;
    }

    private static CloakedRevealerInvoker<TEnumbl, TFilterBase, TRevealBase>
        CreateAddFilteredCloakedRevealerDelegate<TEnumbl, TFilterBase, TRevealBase>
        (Type enumblParamType, Type enumblType, Type filterType, Type revealType)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");

        if (!elementType.IsAssignableTo(revealType))
            throw new ArgumentException($"Expected to receive a enumerable element " +
                                        $"{elementType.Name} to be equatable to {revealType.Name}");

        using var genericParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(4);
        genericParamTypes[0] = enumblType;
        genericParamTypes[1] = elementType;
        genericParamTypes[2] = filterType;
        genericParamTypes[3] = revealType;

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(PalantírReveal<TRevealBase>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var toInvokeOn = GetStaticMethodInfo(nameof(RevealFilteredEnumerate), genericParamTypes.AsArray, methodParamTypes.AsArray);

        methodParamTypes[1] = enumblParamType;
        var fullGenericInvoke =
            BuildAddFilteredCloakedRevealerInvoker<TEnumbl, TFilterBase, TRevealBase>
                (toInvokeOn, enumblParamType, enumblType, methodParamTypes.AsArray);

        return fullGenericInvoke;
    }

    private static CloakedRevealerInvoker<TEnumbl, TFilterBase, TRevealBase>
        BuildAddFilteredCloakedRevealerInvoker<TEnumbl, TFilterBase, TRevealBase>(
            MethodInfo methodInfo, Type enumblParamType, Type enumblType, Type[] methodParamTypes)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var helperMethod =
            new DynamicMethod
                ($"{methodInfo.Name}_DynamicEnumeratorInvoke", null,
                 methodParamTypes, typeof(OrderedCollectionAddFilteredEnumerateExtensions).Module, false);
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

        // call AddFilteredEnumerate(KeyedCollectionMold, TEnumbl, valueFmtStr, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(requiresCast || requiresUnboxing ? OpCodes.Ldloc_0 : OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Call, methodInfo);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CloakedRevealerInvoker<TEnumbl, TFilterBase, TRevealBase>));
        var createInvoker = (CloakedRevealerInvoker<TEnumbl, TFilterBase, TRevealBase>)methodInvoker;

        return createInvoker;
    }

    internal static InputTypeInvoke<TEnumbl, TFilterBase> GetAddFilteredStringBearer<TEnumbl, TFilterBase>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var filterType      = typeof(TFilterBase);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl, TFilterBase>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType, filterType)
                   , static ((Type enumblParamType, Type enumblType, Type filterType) key, bool _) =>
                         CreateAddFilteredStringBearerDelegate<TEnumbl, TFilterBase>(key.enumblParamType, key.enumblType, key.filterType)
                   , callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAddFilteredStringBearerDelegate<TEnumbl, TFilterBase>(Type enumblParamType
      , Type enumblType, Type filterType)
        where TEnumbl : IEnumerable?
    {
        var  elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        bool isNullable  = elementType.IsNullable();
        var  itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();

        if (!itemType.IsStringBearer()) throw new ArgumentException("Expected to receive a IStringBearer collection");

        if (isNullable)
        {
            return CreateAnyEnumblToDoubleGenericInvokerDelegate<TEnumbl, TFilterBase>
                (enumblParamType, enumblType, elementType, nameof(RevealFilteredEnumerateNullable));
        }
        return CreateAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>
            (enumblParamType, enumblType, elementType, filterType, nameof(RevealFilteredEnumerate));
    }

    private static InputTypeInvoke<TEnumbl, TFilter> CreateAddFilteredStringDelegate<TEnumbl, TFilter>(Type enumblParamType, Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");

        if (!elementType.IsString()) throw new ArgumentException("Expected to receive a string collection");

        return CreateAnyEnumblToSingleGenericInvokerDelegate<TEnumbl, TFilter>(enumblParamType, enumblType, nameof(AddFilteredEnumerateString));
    }

    internal static InputTypeInvoke<TEnumbl, TFilterBase> GetAddFilteredCharSequence<TEnumbl, TFilterBase>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var filterType      = typeof(TFilterBase);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl, TFilterBase>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType, filterType)
                   , static ((Type enumblParamType, Type enumblType, Type filterType) key, bool _) =>
                         CreateAddFilteredCharSequenceDelegate<TEnumbl, TFilterBase>(key.enumblParamType, key.enumblType, key.filterType)
                   , callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAddFilteredCharSequenceDelegate<TEnumbl, TFilterBase>(
        Type enumblParamType, Type enumblType, Type filterType)
        where TEnumbl : IEnumerable?
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");

        if (!elementType.IsCharSequence()) throw new ArgumentException("Expected to receive a ICharSequence collection");

        return CreateAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType, elementType
                                                                                 , filterType, nameof(AddFilteredEnumerateCharSeq));
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAddFilteredStringBuilderDelegate<TEnumbl, TFilterBase>(
        Type enumblParamType, Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");

        if (!elementType.IsStringBuilder()) throw new ArgumentException("Expected to receive a StringBuilder collection");

        return CreateAnyEnumblToSingleGenericInvokerDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType
                                                                                 , nameof(AddFilteredEnumerateStringBuilder));
    }

    internal static InputTypeInvoke<TEnumbl, TFilterBase> GetAddFilteredMatch<TEnumbl, TFilterBase>(Type enumblType)
        where TEnumbl : IEnumerable?
    {
        var enumblParamType = typeof(TEnumbl);
        var filterType      = typeof(TFilterBase);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl, TFilterBase>)
            InputTypeInvokeCache
                .GetOrAdd
                    ((enumblParamType, enumblType, filterType)
                   , static ((Type enumblParamType, Type enumblType, Type filterType) key, bool _) =>
                         CreateAddFilteredMatchDelegate<TEnumbl, TFilterBase>(key.enumblParamType, key.enumblType, key.filterType), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl, TFilterBase> CreateAddFilteredMatchDelegate<TEnumbl, TFilterBase>(
        Type enumblParamType, Type enumblType, Type filterType)
        where TEnumbl : IEnumerable?
    {
        var elementType = enumblType.GetIterableElementType() ?? throw new ArgumentException("Expected IEnumerator<T>");
        var itemType    = elementType.IfNullableGetUnderlyingTypeOrThis();

        if (itemType.IsSpanFormattable())
        {
            return CreateAddFilteredSpanFormattableDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType, filterType);
        }
        if (itemType.IsStringBearer())
        {
            return CreateAddFilteredStringBearerDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType, filterType);
        }
        if (itemType.IsString()) { return CreateAddFilteredStringDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType); }
        if (itemType.IsStringBuilder()) { return CreateAddFilteredStringBuilderDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType); }
        if (itemType.IsCharSequence())
        {
            return CreateAddFilteredCharSequenceDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType, filterType);
        }
        if (itemType.IsBool()) { return CreateAddFilteredBoolDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType); }

        return CreateAnyEnumblToTripleGenericInvokerDelegate<TEnumbl, TFilterBase>(enumblParamType, enumblType
                                                                                 , elementType, filterType, nameof(AddFilteredEnumerateMatch));
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredBoolCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredBoolMethodInfo<TFilterBase>(typeof(TElement));
        return enumblType.GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredSpanFormattableCallStructEnumtrInvoker<
            TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredSpanFormattableMethodInfo<TFilterBase>(typeof(TElement));
        return enumblType.GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeNullableInvoke<TEnumbl, TElement> GetAddFilteredNullableSpanFormattableCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredNullableSpanFormattableMethodInfo<TElement>(typeof(TElement));
        return enumblType.GetAddStructFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredStringBearerCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredStringBearerMethodInfo<TFilterBase>(typeof(TElement));
        return enumblType.GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeNullableInvoke<TEnumbl, TElement> GetAddFilteredNullableStringBearerCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredNullableStringBearerMethodInfo<TElement?>(typeof(TElement?));
        return enumblType.GetAddStructFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredStringCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredStringMethodInfo(typeof(TElement));
        return enumblType.GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredCharSequenceCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredCharSequenceMethodInfo<TFilterBase>(typeof(TElement));
        return enumblType.GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredMatchCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredMatchMethodInfo<TFilterBase>(typeof(TElement));
        return enumblType.GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredObjectCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredObjectMethodInfo(typeof(TElement), typeof(object));
        return enumblType.GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredStringBuilderCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredStringBuilderMethodInfo(typeof(TElement));
        return enumblType.GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> GetAddFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumtrType, MethodInfo enumeratorMethodInf)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var enumblParamType = typeof(TEnumbl);
        var filterType      = typeof(TFilterBase);
        var callAsFactory   = true;
        var invoker =
            (InputTypeInvoke<TEnumbl, TElement, TFilterBase>)
            InputTypeCallStructEnumeratorCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, filterType, enumeratorMethodInf)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type filterType, MethodInfo enumtrMethInf) key
                                , bool _) =>
                              key.enumblType.BuildAddFilteredNoRevealersCallStructEnumtr
                                  <TEnumbl, TElement, TFilterBase>(key.enumblParamType, key.enumeratorType, key.enumtrMethInf), callAsFactory);
        return invoker;
    }

    private static InputTypeInvoke<TEnumbl, TElement, TFilterBase> BuildAddFilteredNoRevealersCallStructEnumtr<TEnumbl, TElement, TFilterBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType, MethodInfo callEnumtrInvokeMethInf)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var hasValuesType = typeof(bool?);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", null,
                 methodParamTypes.AsArray
               , typeof(OrderedCollectionAddFilteredEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        var hasValues = ilGenerator.DeclareLocal(hasValuesType);

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

        // call AddFilteredIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldloc, hasValues);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeInvoke<TEnumbl, TElement, TFilterBase>));
        return (InputTypeInvoke<TEnumbl, TElement, TFilterBase>)methodInvoker;
    }

    private static InputTypeNullableInvoke<TEnumbl, TNullStruct> GetAddStructFilteredBuiltInTypeCallStructEnumtrInvoker<TEnumbl, TNullStruct>
        (this Type enumblType, Type enumtrType, MethodInfo enumeratorMethodInf)
        where TEnumbl : IEnumerable<TNullStruct?>?
        where TNullStruct : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var callAsFactory   = true;
        var invoker =
            (InputTypeNullableInvoke<TEnumbl, TNullStruct>)
            InputTypeNullableStructCallStructEnumeratorCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, enumeratorMethodInf)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, MethodInfo enumtrMethInf) key
                                , bool _) =>
                              key.enumblType.BuildNullableStructAddFilteredNoRevealersCallStructEnumtr
                                  <TEnumbl, TNullStruct>(key.enumblParamType, key.enumeratorType, key.enumtrMethInf), callAsFactory);
        return invoker;
    }

    private static InputTypeNullableInvoke<TEnumbl, TElementStruct> BuildNullableStructAddFilteredNoRevealersCallStructEnumtr<TEnumbl, TElementStruct>
        (this Type enumblType, Type enumblParamType, Type enumeratorType, MethodInfo callEnumtrInvokeMethInf)
        where TEnumbl : IEnumerable<TElementStruct?>?
        where TElementStruct : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(5);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TElementStruct?>);
        methodParamTypes[3] = typeof(string);
        methodParamTypes[4] = typeof(FormatFlags);

        var hasValuesType = typeof(bool?);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", null,
                 methodParamTypes.AsArray
               , typeof(OrderedCollectionAddFilteredEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        var hasValues = ilGenerator.DeclareLocal(hasValuesType);

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

        // call AddFilteredIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldloc, hasValues);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(InputTypeNullableInvoke<TEnumbl, TElementStruct>));
        return (InputTypeNullableInvoke<TEnumbl, TElementStruct>)methodInvoker;
    }

    private static CloakedRevealerInvoker<TEnumbl, TElement, TFilterBase, TRevealBase> GetAddFilteredCloakedRevealerCallStructEnumtrInvoker
        <TEnumbl, TElement, TFilterBase, TRevealBase>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredCloakedRevealerMethodInfo<TFilterBase, TRevealBase>(typeof(TElement));
        return enumblType
            .GetAddFilteredCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase, TRevealBase>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static CloakedRevealerInvoker<TEnumbl, TElement, TFilterBase, TRevealBase>
        GetAddFilteredCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement, TFilterBase, TRevealBase>
        (this Type enumblType, Type enumtrType, MethodInfo enumeratorMethodInf)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var enumblParamType = typeof(TEnumbl);
        var filterType      = typeof(TFilterBase);
        var callAsFactory   = true;
        var invoker =
            (CloakedRevealerInvoker<TEnumbl, TElement, TFilterBase, TRevealBase>)
            InputTypeCallStructEnumeratorCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, filterType, enumeratorMethodInf)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type filterType, MethodInfo enumtrMethInf) key
                                , bool _) =>
                              key.enumblType.BuildAddFilteredCloakedRevealerCallStructEnumtr
                                  <TEnumbl, TElement, TFilterBase, TRevealBase>
                                  (key.enumblParamType, key.enumeratorType, key.enumtrMethInf), callAsFactory);
        return invoker;
    }

    private static CloakedRevealerInvoker<TEnumbl, TElement, TFilterBase, TRevealBase> BuildAddFilteredCloakedRevealerCallStructEnumtr
        <TEnumbl, TElement, TFilterBase, TRevealBase>
        (this Type enumblType, Type enumblParamType, Type enumeratorType, MethodInfo callEnumtrInvokeMethInf)
        where TEnumbl : IEnumerable<TElement>?
        where TElement : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TFilterBase>);
        methodParamTypes[3] = typeof(PalantírReveal<TRevealBase>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var hasValuesType = typeof(bool?);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", null,
                 methodParamTypes.AsArray
               , typeof(OrderedCollectionAddFilteredEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        var hasValues = ilGenerator.DeclareLocal(hasValuesType);

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

        // call AddFilteredIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldloc, hasValues);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(CloakedRevealerInvoker<TEnumbl, TElement, TFilterBase, TRevealBase>));
        return (CloakedRevealerInvoker<TEnumbl, TElement, TFilterBase, TRevealBase>)methodInvoker;
    }

    private static NullableCloakedRevealerInvoker<TEnumbl, TElement> GetAddFilteredNullableCloakedRevealerCallStructEnumtrInvoker
        <TEnumbl, TElement>(this Type enumblType, Type enumtrType)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct
    {
        var callEnumtrInvokeMethInf = enumtrType.GetAddFilteredNullableCloakedRevealerMethodInfo<TElement>(typeof(TElement?));
        return enumblType.GetAddFilteredNullableCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement>(enumtrType, callEnumtrInvokeMethInf);
    }

    private static NullableCloakedRevealerInvoker<TEnumbl, TElement> GetAddFilteredNullableCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TElement>
        (this Type enumblType, Type enumtrType, MethodInfo enumeratorMethodInf)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct
    {
        var enumblParamType = typeof(TEnumbl);
        var filterType      = typeof(TElement?);
        var callAsFactory   = true;
        var invoker =
            (NullableCloakedRevealerInvoker<TEnumbl, TElement>)
            InputTypeCallStructEnumeratorCache
                .GetOrAdd((enumblParamType, enumblType, enumtrType, filterType, enumeratorMethodInf)
                        , static ((Type enumblParamType, Type enumblType, Type enumeratorType, Type filterType, MethodInfo enumtrMethInf) key
                                , bool _) =>
                              key.enumblType.BuildAddFilteredNullableCloakedRevealerCallStructEnumtr
                                  <TEnumbl, TElement>(key.enumblParamType, key.enumeratorType, key.enumtrMethInf), callAsFactory);
        return invoker;
    }

    private static NullableCloakedRevealerInvoker<TEnumbl, TElement> BuildAddFilteredNullableCloakedRevealerCallStructEnumtr<TEnumbl, TElement>(
        this Type enumblType, Type enumblParamType, Type enumeratorType, MethodInfo callEnumtrInvokeMethInf)
        where TEnumbl : IEnumerable<TElement?>?
        where TElement : struct
    {
        var requiresCast     = enumblParamType != enumblType;
        var requiresUnboxing = !enumblParamType.IsValueType && enumblType.IsValueType;

        var enumtrInvokeParams       = callEnumtrInvokeMethInf.GetParameters();
        var boolRequiresNullableCast = enumtrInvokeParams[1].ParameterType.IsNullable() && !enumeratorType.IsNullable();

        using var methodParamTypes = RecyclingArrays.GetReusableArrayOf<Type>(6);
        methodParamTypes[0] = typeof(ICollectionMoldWriteState);
        methodParamTypes[1] = enumblParamType;
        methodParamTypes[2] = typeof(OrderedCollectionPredicate<TElement?>);
        methodParamTypes[3] = typeof(PalantírReveal<TElement>);
        methodParamTypes[4] = typeof(string);
        methodParamTypes[5] = typeof(FormatFlags);

        var hasValuesType = typeof(bool?);

        var helperMethod =
            new DynamicMethod
                ($"{enumblType.Name}_DynamicNoRevealersNoNullableStructInvoke_{enumblType.Name}", null,
                 methodParamTypes.AsArray
               , typeof(OrderedCollectionAddFilteredEnumerateExtensions).Module, false);
        var ilGenerator = helperMethod.GetILGenerator();
        // Make space for enumblType  and enumeratorType and if required Nullable<enumeratorType> local variables
        var enumblLocalType = ilGenerator.DeclareLocal(enumblType);
        ilGenerator.DeclareLocal(enumeratorType);
        LocalBuilder? castEnumtrToNullable = null;
        if (boolRequiresNullableCast) { castEnumtrToNullable = ilGenerator.DeclareLocal(typeof(Nullable<>).MakeGenericType(enumeratorType)); }

        // cast TEnumbl value => (enumblType)value
        ilGenerator.DeclareLocal(enumeratorType);
        var hasValues = ilGenerator.DeclareLocal(hasValuesType);

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

        // call AddFilteredIterateValueRevealer(KeyedCollectionMold, TEnumtr, valueRevealer, keyFmtStr, valueFmtStr, FormatFlags)
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(boolRequiresNullableCast ? OpCodes.Ldloc_2 : OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Ldarg_S, 4);
        ilGenerator.Emit(OpCodes.Ldarg_S, 5);
        ilGenerator.Emit(OpCodes.Ldloc, hasValues);
        ilGenerator.Emit(OpCodes.Call, callEnumtrInvokeMethInf);

        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(typeof(NullableCloakedRevealerInvoker<TEnumbl, TElement>));
        return (NullableCloakedRevealerInvoker<TEnumbl, TElement>)methodInvoker;
    }

    private static MethodInfo GetStaticMethodInfo(string findMethodName, Type[] findGenericParams, params Type[] findParamTypes)
    {
        myMethodInfosCached ??= typeof(OrderedCollectionAddFilteredEnumerateExtensions).GetMethods(NonPublic | Public | Static);

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

    public static void AddFilteredEnumerateBool<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool>
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateBool(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(bool);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateBool<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<bool> filterPredicate
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold =
                        mws.Master.GetTrackedInstanceMold
                            (value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredBoolCallStructEnumtrInvoker<TEnumbl, bool, bool>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredEnumerateNullableBool<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool?>
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateNullableBool(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(bool);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateNullableBool<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(bool?);
        var  any             = false;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            var enumeratorType = actualType.GetEnumeratorType();
            if (enumeratorType?.IsValueType ?? false)
            {
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold =
                        mws.Master.GetTrackedInstanceMold
                            (value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredBoolCallStructEnumtrInvoker<TEnumbl, bool?, bool?>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredEnumerate<TEnumbl, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            mws.AddFilteredEnumerate(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerate<TEnumbl, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddFilteredSpanFormattable<TEnumbl, TFmtBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags);
    }

    public static void AddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmt>
        where TFmt : TFmtBase?, ISpanFormattable?
    {
        if (value != null)
        {
            mws.AddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : TFmtBase?, ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmt>);
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold =
                        mws.Master.GetTrackedInstanceMold
                            (value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredSpanFormattableCallStructEnumtrInvoker<TEnumbl, TFmt, TFmtBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredEnumerateNullable<TEnumbl, TFmtStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateNullable(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateNullable<TEnumbl, TFmtStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold =
                        mws.Master.GetTrackedInstanceMold
                            (value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredNullableSpanFormattableCallStructEnumtrInvoker<TEnumbl, TFmtStruct>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealFilteredEnumerate<TEnumbl, TFilterBase, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull
    {
        if (value != null)
        {
            mws.RevealFilteredEnumerate(value.Value, filterPredicate, palantírReveal, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TRevealBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredEnumerate<TEnumbl, TFilterBase, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddFilteredCloakedRevealer<TEnumbl, TFilterBase, TRevealBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, palantírReveal, formatString, formatFlags);
    }

    public static void RevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        if (value != null)
        {
            mws.RevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
                (value.Value, filterPredicate, palantírReveal, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TRevealBase);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(
        this ICollectionMoldWriteState mws,
        TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TFilterBase?, TRevealBase?
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold =
                        mws
                            .Master
                            .GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags()
                                                  , AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TCloaked, TFilterBase, TRevealBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, palantírReveal, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(
        this ICollectionMoldWriteState mws,
        TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloakedStruct?>
        where TCloakedStruct : struct
    {
        if (value != null)
        {
            mws.RevealFilteredEnumerateNullable(value.Value, filterPredicate, palantírReveal, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TCloakedStruct);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold =
                        mws
                            .Master
                            .GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags()
                                                  , AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredNullableCloakedRevealerCallStructEnumtrInvoker<TEnumbl, TCloakedStruct>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, palantírReveal, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealFilteredEnumerate<TEnumbl, TFilterBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            mws.RevealFilteredEnumerate(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredEnumerate<TEnumbl, TFilterBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddFilteredStringBearer<TEnumbl, TFilterBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags);
    }

    public static void RevealFilteredEnumerate<TEnumbl, TBearer, TFilterBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : TFilterBase?, IStringBearer?
    {
        if (value != null)
        {
            mws.RevealFilteredEnumerate<TEnumbl, TBearer, TFilterBase>(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TBearer);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredEnumerate<TEnumbl, TBearer, TFilterBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : TFilterBase?, IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearer>);
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold =
                        mws.Master
                           .GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredStringBearerCallStructEnumtrInvoker<TEnumbl, TBearer, TFilterBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer
    {
        if (value != null)
        {
            mws.RevealFilteredEnumerateNullable(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(TBearerStruct);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void RevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>);
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags()
                                                                , AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredNullableStringBearerCallStructEnumtrInvoker<TEnumbl, TBearerStruct>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredEnumerateString<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<string> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?>
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateString(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateString<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<string> filterPredicate
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags()
                                                                , AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredStringCallStructEnumtrInvoker<TEnumbl, string?, string>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredEnumerateCharSeq<TEnumbl, TCharSeqBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateCharSeq(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(ISpanFormattable);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateCharSeq<TEnumbl, TCharSeqBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddFilteredCharSequence<TEnumbl, TCharSeqBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags);
    }

    public static void AddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : TCharSeqBase?, ICharSequence?
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : TCharSeqBase?, ICharSequence?
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold
                        (value, formatFlags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredCharSequenceCallStructEnumtrInvoker<TEnumbl, TCharSeq, TCharSeqBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredEnumerateStringBuilder<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<StringBuilder?>
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateStringBuilder(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateStringBuilder<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags()
                                                                , AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredStringBuilderCallStructEnumtrInvoker<TEnumbl, StringBuilder?, StringBuilder>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddFilteredEnumerateMatch<TEnumbl, TAnyBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateMatch(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateMatch<TEnumbl, TAnyBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType         = value?.GetType() ?? typeof(TEnumbl);
        var callGenericInvoker = GetAddFilteredMatch<TEnumbl, TAnyBase>(actualType);
        callGenericInvoker(mws, value, filterPredicate, formatString, formatFlags);
    }

    public static void AddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny>
        where TAny : TAnyBase?
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    public static void AddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>?
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TAny?>);
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags()
                                                                , AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredMatchCallStructEnumtrInvoker<TEnumbl, TAny, TAnyBase>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    [CallsObjectToString]
    public static void AddFilteredEnumerateObject<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<object> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<object?>
    {
        if (value != null)
        {
            mws.AddFilteredEnumerateObject(value.Value, filterPredicate, formatString, formatFlags);
            return;
        }
        var elementType = typeof(string);
        var valueMold   = mws.ConditionalCollectionPrefix(value, elementType, null, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, null, null, "", formatFlags);
    }

    [CallsObjectToString]
    public static void AddFilteredEnumerateObject<TEnumbl>(
        this ICollectionMoldWriteState mws
      , TEnumbl? value
      , OrderedCollectionPredicate<object> filterPredicate
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
                if (!actualType.IsValueType && !mws.BuildingInstanceEquals(value))
                {
                    valueMold = mws.Master.GetTrackedInstanceMold(value, formatFlags.RemoveEmbeddedContentFlags()
                                                                , AsSimple | WrittenAsFlags.AsCollection
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
                    actualType.GetAddFilteredObjectCallStructEnumtrInvoker<TEnumbl, object?, object>(enumeratorType);
                // ReSharper disable once GenericEnumeratorNotDisposed
                structEnumtrInvoker(mws, value, filterPredicate, formatString, formatFlags);
                valueMold?.Complete();
                return;
            }
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
            collectionItems = count;
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, mws.ItemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }
}
