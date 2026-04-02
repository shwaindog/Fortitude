// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections.Concurrent;
using System.Reflection;
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
                         methodName = nameof(RevealAllIterateStringBearer);
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

                     return GetStaticMethodInfo(nameof(AddAllMatchIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
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

                     return GetStaticMethodInfo(nameof(AddAllObjectIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | FormatFlags.AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateNullableBool<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null
    )
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | FormatFlags.AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | FormatFlags.AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | FormatFlags.AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCloaked?>
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Value.Current;
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                hasValue = value.Value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TCloaked?>?
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = 
                mws.ConditionalCollectionPrefix(value, elementType, false
                                              , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            mws.WasSkipped(actualType, "", formatFlags);
            return;
        }
        var  elementType     = typeof(TCloakedStruct);
        var  any             = false;
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Value.Current;
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                hasValue = value.Value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllIterateStringBearer<TEnumtr, TBearer>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TBearer?>
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Value.Current;

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                hasValue = value.Value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllIterateStringBearer<TEnumtr, TBearer>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : IEnumerator<TBearer?>?
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void RevealAllIterateNullableStringBearer<TEnumtr, TBearerStruct>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null)
        where TEnumtr : struct
      , IEnumerator<TBearerStruct?>
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Value.Current;

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                hasValue = value.Value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateCharSeq<TEnumtr, TCharSeq>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null
    )
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                any = true;
                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllIterateStringBuilder<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null
    )
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    public static void AddAllMatchIterate<TEnumtr, TAny>(
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
        var itemCount   = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }

    [CallsObjectToString]
    public static void AddAllObjectIterate<TEnumtr>(
        this ICollectionMoldWriteState mws
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , bool? hasValue = null
    )
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
        var  itemCount       = 0;
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
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                    formatFlags = formatFlags.RemoveEmbeddedContentFlags();
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        value?.Dispose();
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false
                                                      , new CreateContext(formatFlags: formatFlags, displayAsType: mws.DisplayAsType));
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        if (mws.SupportsMultipleFields) { mws.AppendGoToNex(); }
    }
}
