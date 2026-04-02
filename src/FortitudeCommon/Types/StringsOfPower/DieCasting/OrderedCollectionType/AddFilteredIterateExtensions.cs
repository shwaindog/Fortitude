// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections.Concurrent;
using System.Reflection;
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
                         methodName           = nameof(RevealFilteredIterateNullableStringBearer);
                         genericParamTypes[1] = itemType;
                     }
                     else
                     {
                         methodName = nameof(RevealFilteredIterateStringBearer);
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

                     return GetStaticMethodInfo(nameof(AddFilteredMatchIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
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

                     return GetStaticMethodInfo(nameof(AddFilteredObjectIterate), genericParamTypes.AsArray, methodParamTypes.AsArray);
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

    public static void RevealFilteredIterateStringBearer<TEnumtr, TBearer, TFilterBase>(
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

    public static void RevealFilteredIterateNullableStringBearer<TEnumtr, TBearerStruct>(
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

    public static void AddFilteredMatchIterate<TEnumtr, TAny, TAnyBase>(
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
    public static void AddFilteredObjectIterate<TEnumtr>(
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
