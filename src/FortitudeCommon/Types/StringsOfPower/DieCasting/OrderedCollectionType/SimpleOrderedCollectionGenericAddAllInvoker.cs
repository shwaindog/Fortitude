// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public static class SimpleOrderedCollectionGenericAddAllInvoker
{
    private static readonly ConcurrentDictionary<Type, Delegate> OrderedCollAddAllInvokers = new();
    private static readonly ConcurrentDictionary<Type, Delegate> CallCollAddAll            = new();

    private const string AddAllMethodName                               = $"{nameof(SimpleOrderedCollectionMold.AddAll)}";
    private const string AddAllEnumerateMethodName                      = $"{nameof(SimpleOrderedCollectionMold.AddAllEnumerate)}";
    private const string AddAllIterateMethodName                        = $"{nameof(SimpleOrderedCollectionMold.AddAllIterate)}";
    private const string AddAllStringEnumerateMethodName        = $"{nameof(SimpleOrderedCollectionMold.AddAllStringEnumerate)}";
    private const string AddAllStringIterateMethodName          = $"{nameof(SimpleOrderedCollectionMold.AddAllStringIterate)}";
    private const string AddAllStringBuilderEnumerateMethodName = $"{nameof(SimpleOrderedCollectionMold.AddAllStringBuilderEnumerate)}";
    private const string AddAllStringBuilderIterateMethodName   = $"{nameof(SimpleOrderedCollectionMold.AddAllStringBuilderIterate)}";
    private const string AddAllEnumerateNullableMethodName              = $"{nameof(SimpleOrderedCollectionMold.AddAllEnumerateNullable)}";
    private const string AddAllIterateNullableMethodName                = $"{nameof(SimpleOrderedCollectionMold.AddAllIterateNullable)}";
    private const string AddAllCharSequenceMethodName                   = $"{nameof(SimpleOrderedCollectionMold.AddAllCharSeq)}";
    private const string AddAllCharSequenceEnumerateMethodName          = $"{nameof(SimpleOrderedCollectionMold.AddAllCharSeqEnumerate)}";
    private const string AddAllCharSequenceIterateMethodName            = $"{nameof(SimpleOrderedCollectionMold.AddAllCharSeqIterate)}";
    private const string RevealAllMethodName                            = $"{nameof(SimpleOrderedCollectionMold.RevealAll)}";
    private const string RevealAllEnumerateMethodName                   = $"{nameof(SimpleOrderedCollectionMold.RevealAllEnumerate)}";
    private const string RevealAllIterateMethodName                     = $"{nameof(SimpleOrderedCollectionMold.RevealAllIterate)}";
    private const string RevealAllEnumerateNullableMethodName           = $"{nameof(SimpleOrderedCollectionMold.RevealAllEnumerateNullable)}";
    private const string RevealAllIterateNullableMethodName             = $"{nameof(SimpleOrderedCollectionMold.RevealAllIterateNullable)}";
    private const string AddAllMatchMethodName                          = $"{nameof(SimpleOrderedCollectionMold.AddAllMatch)}";
    private const string AddAllMatchEnumerateMethodName                 = $"{nameof(SimpleOrderedCollectionMold.AddAllMatchEnumerate)}";
    private const string AddAllMatchIterateMethodName                   = $"{nameof(SimpleOrderedCollectionMold.AddAllMatchIterate)}";

    private const string MyCallAddAllMethodName = $"{nameof(CallAddAll)}";

    private static readonly MethodInfo BoolArrayAddAll;
    private static readonly MethodInfo NullableBoolArrayAddAll;
    private static readonly MethodInfo SpanFormattableArrayAddAll;
    private static readonly MethodInfo NullableSpanFormattableArrayAddAll;
    private static readonly MethodInfo StringArrayAddAll;
    private static readonly MethodInfo CharSequenceArrayAddAll;
    private static readonly MethodInfo StringBuilderArrayAddAll;
    private static readonly MethodInfo StyledToStringObjArrayAddAll;
    private static readonly MethodInfo ArrayAddAllMatch;

    private static readonly MethodInfo BoolReadOnlyListAddAll;
    private static readonly MethodInfo NullableBoolReadOnlyListAddAll;
    private static readonly MethodInfo SpanFormattableReadOnlyListAddAll;
    private static readonly MethodInfo NullableSpanFormattableReadOnlyListAddAll;
    private static readonly MethodInfo StringReadOnlyListAddAll;
    private static readonly MethodInfo CharSequenceReadOnlyListAddAll;
    private static readonly MethodInfo StringBuilderReadOnlyListAddAll;
    private static readonly MethodInfo StyledToStringObjReadOnlyListAddAll;
    private static readonly MethodInfo ReadOnlyListAddAllMatch;

    private static readonly MethodInfo BoolEnumerableAddAll;
    private static readonly MethodInfo NullableBoolEnumerableAddAll;
    private static readonly MethodInfo SpanFormattableEnumerableAddAll;
    private static readonly MethodInfo NullableSpanFormattableEnumerableAddAll;
    private static readonly MethodInfo StringEnumerableAddAll;
    private static readonly MethodInfo CharSequenceEnumerableAddAll;
    private static readonly MethodInfo StringBuilderEnumerableAddAll;
    private static readonly MethodInfo StyledToStringObjEnumerableAddAll;
    private static readonly MethodInfo EnumerableAddAllMatch;

    private static readonly MethodInfo BoolEnumeratorAddAll;
    private static readonly MethodInfo NullableBoolEnumeratorAddAll;
    private static readonly MethodInfo SpanFormattableEnumeratorAddAll;
    private static readonly MethodInfo NullableSpanFormattableEnumeratorAddAll;
    private static readonly MethodInfo StringEnumeratorAddAll;
    private static readonly MethodInfo CharSequenceEnumeratorAddAll;
    private static readonly MethodInfo StringBuilderEnumeratorAddAll;
    private static readonly MethodInfo StyledToStringObjEnumeratorAddAll;
    private static readonly MethodInfo EnumeratorAddAllMatch;

    private static readonly MethodInfo MyCallAddAll;

    private static readonly Type SimpleOrderedCollectionBuilderType = typeof(SimpleOrderedCollectionMold);

    private static readonly Type BoolType          = typeof(bool);
    private static readonly Type NullableBoolType  = typeof(bool?);
    private static readonly Type StringType        = typeof(string);
    private static readonly Type CharSequenceType  = typeof(ICharSequence);
    private static readonly Type StringBearerType  = typeof(IStringBearer);
    private static readonly Type StringBuilderType = typeof(StringBuilder);
    private static readonly Type Func5TypesType    = typeof(Func<,,,,>);
    private static readonly Type FormatFlagsType   = typeof(FormatFlags);

    static SimpleOrderedCollectionGenericAddAllInvoker()
    {
        var pubMethods = SimpleOrderedCollectionBuilderType.GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);

        BoolArrayAddAll                    = GetBoolArrayAddAllMethodInfo(pubMethods);
        NullableBoolArrayAddAll            = GetNullableBoolArrayAddAllMethodInfo(pubMethods);
        SpanFormattableArrayAddAll         = GetSpanFormattableArrayAddAllMethodInfo(pubMethods);
        NullableSpanFormattableArrayAddAll = GetNullableSpanFormattableArrayAddAllMethodInfo(pubMethods);
        StringArrayAddAll                  = GetStringArrayAddAllMethodInfo(pubMethods);
        CharSequenceArrayAddAll            = GetCharSequenceArrayAddAllMethodInfo(pubMethods);
        StringBuilderArrayAddAll           = GetStringBuilderArrayAddAllMethodInfo(pubMethods);
        StyledToStringObjArrayAddAll       = GetStringBearerArrayAddAllMethodInfo(pubMethods);
        ArrayAddAllMatch                   = GetArrayAddAllMatchMethodInfo(pubMethods);

        BoolReadOnlyListAddAll                    = GetBoolReadOnlyListAddAllMethodInfo(pubMethods);
        NullableBoolReadOnlyListAddAll            = GetNullableBoolReadOnlyListAddAllMethodInfo(pubMethods);
        SpanFormattableReadOnlyListAddAll         = GetSpanFormattableReadOnlyListAddAllMethodInfo(pubMethods);
        NullableSpanFormattableReadOnlyListAddAll = GetNullableSpanFormattableReadOnlyListAddAllMethodInfo(pubMethods);
        StringReadOnlyListAddAll                  = GetStringReadOnlyListAddAllMethodInfo(pubMethods);
        CharSequenceReadOnlyListAddAll            = GetCharSequenceReadOnlyListAddAllMethodInfo(pubMethods);
        StringBuilderReadOnlyListAddAll           = GetStringBuilderReadOnlyListAddAllMethodInfo(pubMethods);
        StyledToStringObjReadOnlyListAddAll       = GetStringBearerReadOnlyListAddAllMethodInfo(pubMethods);
        ReadOnlyListAddAllMatch                   = GetReadOnlyListAddAllMatchMethodInfo(pubMethods);

        BoolEnumerableAddAll                    = GetBoolEnumerableAddAllMethodInfo(pubMethods);
        NullableBoolEnumerableAddAll            = GetNullableBoolEnumerableAddAllMethodInfo(pubMethods);
        SpanFormattableEnumerableAddAll         = GetSpanFormattableEnumerableAddAllMethodInfo(pubMethods);
        NullableSpanFormattableEnumerableAddAll = GetNullableSpanFormattableEnumerableAddAllMethodInfo(pubMethods);
        StringEnumerableAddAll                  = GetStringEnumerableAddAllMethodInfo(pubMethods);
        CharSequenceEnumerableAddAll            = GetCharSequenceEnumerableAddAllMethodInfo(pubMethods);
        StringBuilderEnumerableAddAll           = GetStringBuilderEnumerableAddAllMethodInfo(pubMethods);
        StyledToStringObjEnumerableAddAll       = GetStringBearerEnumerableAddAllMethodInfo(pubMethods);
        EnumerableAddAllMatch                   = GetEnumerableAddAllMatchMethodInfo(pubMethods);

        BoolEnumeratorAddAll                    = GetBoolEnumeratorAddAllMethodInfo(pubMethods);
        NullableBoolEnumeratorAddAll            = GetNullableBoolEnumeratorAddAllMethodInfo(pubMethods);
        SpanFormattableEnumeratorAddAll         = GetSpanFormattableEnumeratorAddAllMethodInfo(pubMethods);
        NullableSpanFormattableEnumeratorAddAll = GetNullableSpanFormattableEnumeratorAddAllMethodInfo(pubMethods);
        StringEnumeratorAddAll                  = GetStringEnumeratorAddAllMethodInfo(pubMethods);
        CharSequenceEnumeratorAddAll            = GetCharSequenceEnumeratorAddAllMethodInfo(pubMethods);
        StringBuilderEnumeratorAddAll           = GetStringBuilderEnumeratorAddAllMethodInfo(pubMethods);
        StyledToStringObjEnumeratorAddAll       = GetStringBearerEnumeratorAddAllMethodInfo(pubMethods);
        EnumeratorAddAllMatch                   = GetEnumeratorAddAllMatchMethodInfo(pubMethods);

        MyCallAddAll
            = MyCallAddAllMethodInfo(typeof(SimpleOrderedCollectionGenericAddAllInvoker).GetMethods(BindingFlags.Public | BindingFlags.Static));
    }

    public static void CallAddAll<TOrdColl, TItem>(SimpleOrderedCollectionMold typeMolder, TOrdColl keyColl
      , string? valueFormatString = null, FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)
        where TOrdColl : IEnumerable<TItem>
    {
        var keyedCollType = keyColl.GetType();
        var callAddAllMatch = (Func<SimpleOrderedCollectionMold, TOrdColl, string?, SimpleOrderedCollectionMold>)
            OrderedCollAddAllInvokers.GetOrAdd(keyedCollType, CreateInvokeMethod);

        callAddAllMatch(typeMolder, keyColl, valueFormatString);
    }

    public static void CallAddAllEnumerator<TKeyColl, TItem>(
        SimpleOrderedCollectionMold typeMolder, TKeyColl keyColl, string? valueFormatString = null
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)
        where TKeyColl : IEnumerator<TItem>
    {
        var keyedCollType = keyColl.GetType();
        var callAddAllEnumerator = (Func<SimpleOrderedCollectionMold, TKeyColl, string?, FormatFlags, SimpleOrderedCollectionMold>)
            OrderedCollAddAllInvokers.GetOrAdd(keyedCollType, CreateInvokeMethod);

        callAddAllEnumerator(typeMolder, keyColl, valueFormatString, formatFlags);
    }

    public static void CallAddAll<TOrdColl>(SimpleOrderedCollectionMold typeMolder, TOrdColl keyColl, string? valueFormatString = null
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)
    {
        var tOrdType        = typeof(TOrdColl);
        var orderedCollType = keyColl!.GetType();
        if (tOrdType.IsEnumerable() || tOrdType.IsEnumerator())
        {
            var callAddAllDelegate = (Func<SimpleOrderedCollectionMold, TOrdColl, string?, FormatFlags, SimpleOrderedCollectionMold>)
                OrderedCollAddAllInvokers.GetOrAdd(orderedCollType, CreateInvokeMethod);

            callAddAllDelegate(typeMolder, keyColl, valueFormatString, formatFlags);
        }
        else
        {
            if (orderedCollType != tOrdType)
            {
                var callAddAllDelegate = (Action<object?[]>)
                    CallCollAddAll.GetOrAdd(orderedCollType, type =>
                    {
                        var genericMethod = MyCallAddAll.MakeGenericMethod(type);
                        void Converter(object?[] arguments) => genericMethod.Invoke(null, arguments);
                        return (Action<object?[]>)Converter;
                    });
                callAddAllDelegate([typeMolder, keyColl, valueFormatString, formatFlags]);
            }
        }
    }

    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record TypeCallCount(Type ForType)
    {
        public int CallCount { get; set; }
    };

    private static readonly ConcurrentDictionary<Type, TypeCallCount> NoOpCalls = new();

    private static SimpleOrderedCollectionMold NoOpNotASupportedOrderedCollection<T>(SimpleOrderedCollectionMold toReturn, T _, string? _1
      , string? _2)
    {
        var callCount = NoOpCalls.GetOrAdd(typeof(T), new TypeCallCount(typeof(T)));
        callCount.CallCount++;
        return toReturn;
    }

    private static MethodInfo GetBoolArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 0) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsBoolArray()) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException($"Method does not exist for {AddAllMethodName}");
        return foundMatch;
    }

    private static MethodInfo GetNullableBoolArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 0) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsNullableBoolArray()) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException($"Method does not exist for {AddAllMethodName}");
        return foundMatch;
    }

    private static MethodInfo GetSpanFormattableArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            if (!genericParam.IsSpanFormattable()) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsArrayOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetNullableSpanFormattableArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            if (!genericParam.IsSpanFormattable()) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsNullableArrayOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 0) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsArrayOf(StringType)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetCharSequenceArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllCharSequenceMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            if (!genericParam.Derives(CharSequenceType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsArrayOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringBuilderArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 0) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsArrayOf(StringBuilderType)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringBearerArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != RevealAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            if (!genericParam.Derives(StringBearerType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsArrayOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetArrayAddAllMatchMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMatchMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsArrayOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetBoolReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 0) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsBoolList()) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetNullableBoolReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 0) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsReadOnlyListOfNullable(BoolType)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetSpanFormattableReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            if (!genericParam.IsSpanFormattable()) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsReadOnlyListOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetNullableSpanFormattableReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            if (!genericParam.IsSpanFormattable()) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsReadOnlyListOfNullable(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 0) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsReadOnlyListOf(StringType)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetCharSequenceReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllCharSequenceMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            if (!genericParam.Derives(CharSequenceType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsReadOnlyListOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringBuilderReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 0) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsReadOnlyListOf(StringBuilderType)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringBearerReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != RevealAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            if (!genericParam.Derives(StringBearerType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsReadOnlyListOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetReadOnlyListAddAllMatchMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMatchMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsReadOnlyListOf(genericParam)) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetBoolEnumerableAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var enumblParam = genericParams[0];
            if (!enumblParam.IsEnumerableOf(BoolType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetNullableBoolEnumerableAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllEnumerateNullableMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var enumblParam = genericParams[0];
            if (!enumblParam.IsEnumerableOfNullable(BoolType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetSpanFormattableEnumerableAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam  = genericParams[0];
            var spanFmtParam = genericParams[1];
            if (!spanFmtParam.IsSpanFormattable()) continue;
            if (!enumblParam.IsEnumerableOf(spanFmtParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetNullableSpanFormattableEnumerableAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllEnumerateNullableMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam  = genericParams[0];
            var spanFmtParam = genericParams[1];
            if (!spanFmtParam.IsSpanFormattable() || !spanFmtParam.IsValueType) continue;
            if (!enumblParam.IsEnumerableOfNullable(spanFmtParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringEnumerableAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllStringEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var enumblParam = genericParams[0];
            if (!enumblParam.IsEnumerableOf(StringType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetCharSequenceEnumerableAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllCharSequenceEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam = genericParams[0];
            var charSeqParam = genericParams[1];
            if (!charSeqParam.Derives(CharSequenceType)) continue;
            if (!enumblParam.IsEnumerableOf(charSeqParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringBuilderEnumerableAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllStringBuilderEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var enumblParam = genericParams[0];
            if (!enumblParam.IsEnumerableOf(StringBuilderType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringBearerEnumerableAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != RevealAllEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam = genericParams[0];
            var bearerParam = genericParams[1];
            if (!bearerParam.Derives(StringBearerType)) continue;
            if (!enumblParam.IsEnumerableOf(bearerParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetEnumerableAddAllMatchMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMatchEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam  = genericParams[0];
            var anyParam    = genericParams[1];
            if (!enumblParam.IsEnumerableOf(anyParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetBoolEnumeratorAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllIterateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var enumblParam = genericParams[0];
            if (!enumblParam.IsEnumeratorOf(BoolType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsEnumeratorOf(BoolType)) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetNullableBoolEnumeratorAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllIterateNullableMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var enumblParam = genericParams[0];
            if (!enumblParam.IsEnumeratorOfNullable(BoolType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetSpanFormattableEnumeratorAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllIterateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam  = genericParams[0];
            var spanFmtParam = genericParams[1];
            if (!spanFmtParam.IsSpanFormattable()) continue;
            if (!enumblParam.IsEnumeratorOf(spanFmtParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetNullableSpanFormattableEnumeratorAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllIterateNullableMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam  = genericParams[0];
            var spanFmtParam = genericParams[1];
            if (!spanFmtParam.IsSpanFormattable()) continue;
            if (!enumblParam.IsEnumeratorOfNullable(spanFmtParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringEnumeratorAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllStringIterateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var enumblParam = genericParams[0];
            if (!enumblParam.IsEnumeratorOf(StringType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetCharSequenceEnumeratorAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllCharSequenceIterateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam  = genericParams[0];
            var charSeqParam = genericParams[1];
            if (!charSeqParam.Derives(CharSequenceType)) continue;
            if (!enumblParam.IsEnumeratorOf(charSeqParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringBuilderEnumeratorAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllStringBuilderIterateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var enumblParam = genericParams[0];
            if (!enumblParam.IsEnumeratorOf(StringBuilderType)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetStringBearerEnumeratorAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != RevealAllIterateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam = genericParams[0];
            var bearerParam = genericParams[1];
            if (!bearerParam.Derives(StringBearerType)) continue;
            if (!enumblParam.IsEnumeratorOf(bearerParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetEnumeratorAddAllMatchMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMatchIterateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var enumblParam = genericParams[0];
            var anyParam    = genericParams[1];
            if (!enumblParam.IsEnumeratorOf(anyParam)) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (methParam1Type != enumblParam) continue;
            if (methodParams[1].ParameterType != NullableBoolType) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo MyCallAddAllMethodInfo(MethodInfo[] myPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < myPubMethods.Length; i++)
        {
            var mi = myPubMethods[i];
            if (mi.Name != MyCallAddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var genericParam = genericParams[0];
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 4) continue;
            if (methodParams[0].ParameterType != SimpleOrderedCollectionBuilderType) continue;
            if (methodParams[1].ParameterType != genericParam) continue;
            if (methodParams[2].ParameterType != StringType) continue;
            if (methodParams[3].ParameterType != FormatFlagsType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    public static Delegate CreateInvokeMethod(Type collectionType)
    {
        var itemType = collectionType.GetIterableElementType()!;

        MethodInfo? selectedMethodInfo = null;

        bool isFormattable = false;
        if (itemType.IsBool())
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = BoolReadOnlyListAddAll;

            if (collectionType.IsArray()) selectedMethodInfo      = BoolArrayAddAll;
            if (collectionType.IsEnumerable()) selectedMethodInfo = BoolEnumerableAddAll;
            if (collectionType.IsEnumerator()) selectedMethodInfo = BoolEnumeratorAddAll;
        }
        if (itemType.IsNullableBool())
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = NullableBoolReadOnlyListAddAll;

            if (collectionType.IsArray()) selectedMethodInfo      = NullableBoolArrayAddAll;
            if (collectionType.IsEnumerable()) selectedMethodInfo = NullableBoolEnumerableAddAll;
            if (collectionType.IsEnumerator()) selectedMethodInfo = NullableBoolEnumeratorAddAll;
        }
        if (itemType.IsSpanFormattable())
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = SpanFormattableReadOnlyListAddAll;

            if (collectionType.IsArray()) selectedMethodInfo      = SpanFormattableArrayAddAll;
            if (collectionType.IsEnumerable()) selectedMethodInfo = SpanFormattableEnumerableAddAll;
            if (collectionType.IsEnumerator()) selectedMethodInfo = SpanFormattableEnumeratorAddAll;
            isFormattable = selectedMethodInfo != null;
        }
        if (itemType.IsNullableSpanFormattable())
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = NullableSpanFormattableReadOnlyListAddAll;

            if (collectionType.IsArray()) selectedMethodInfo      = NullableSpanFormattableArrayAddAll;
            if (collectionType.IsEnumerable()) selectedMethodInfo = NullableSpanFormattableEnumerableAddAll;
            if (collectionType.IsEnumerator()) selectedMethodInfo = NullableSpanFormattableEnumeratorAddAll;
            isFormattable = selectedMethodInfo != null;
        }
        if (itemType.IsString())
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = StringReadOnlyListAddAll;

            if (collectionType.IsArray()) selectedMethodInfo        = StringArrayAddAll;
            if (collectionType.IsReadOnlyList()) selectedMethodInfo = StringReadOnlyListAddAll;
            if (collectionType.IsEnumerable()) selectedMethodInfo   = StringEnumerableAddAll;
            if (collectionType.IsEnumerator()) selectedMethodInfo   = StringEnumeratorAddAll;
        }
        if (itemType.Derives(CharSequenceType))
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = CharSequenceReadOnlyListAddAll;

            if (collectionType.IsArray()) selectedMethodInfo      = CharSequenceArrayAddAll;
            if (collectionType.IsEnumerable()) selectedMethodInfo = CharSequenceEnumerableAddAll;
            if (collectionType.IsEnumerator()) selectedMethodInfo = CharSequenceEnumeratorAddAll;
        }
        if (itemType == StringBuilderType)
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = StringBuilderReadOnlyListAddAll;

            if (collectionType.IsArray()) selectedMethodInfo      = StringBuilderArrayAddAll;
            if (collectionType.IsEnumerable()) selectedMethodInfo = StringBuilderEnumerableAddAll;
            if (collectionType.IsEnumerator()) selectedMethodInfo = StringBuilderEnumeratorAddAll;
        }
        if (itemType.Derives(StringBearerType))
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = StyledToStringObjReadOnlyListAddAll;

            if (collectionType.IsArray()) selectedMethodInfo      = StyledToStringObjArrayAddAll;
            if (collectionType.IsEnumerable()) selectedMethodInfo = StyledToStringObjEnumerableAddAll;
            if (collectionType.IsEnumerator()) selectedMethodInfo = StyledToStringObjEnumeratorAddAll;
        }
        if (selectedMethodInfo == null)
        {
            if (collectionType.IsReadOnlyListAndNotArray()) selectedMethodInfo = ReadOnlyListAddAllMatch;

            if (collectionType.IsArray()) selectedMethodInfo      = ArrayAddAllMatch;
            if (collectionType.IsEnumerable()) selectedMethodInfo = EnumerableAddAllMatch;
            if (collectionType.IsEnumerator()) selectedMethodInfo = EnumeratorAddAllMatch;
            isFormattable = selectedMethodInfo != null;
        }
        if (selectedMethodInfo == null)
        {
            var myType = typeof(SimpleOrderedCollectionGenericAddAllInvoker);
            var noOpMethodToCall = myType.GetMethod($"{nameof(NoOpNotASupportedOrderedCollection)}", [
                SimpleOrderedCollectionBuilderType, collectionType, StringType, FormatFlagsType, SimpleOrderedCollectionBuilderType
            ]);
            noOpMethodToCall = noOpMethodToCall!.MakeGenericMethod(collectionType);

            var noOpInvokerType = Func5TypesType.MakeGenericType
                (SimpleOrderedCollectionBuilderType, collectionType, StringType, FormatFlagsType, SimpleOrderedCollectionBuilderType);
            var noOpInvoker = GetAddAllInvoker(SimpleOrderedCollectionBuilderType, collectionType, noOpMethodToCall, noOpInvokerType);
            OrderedCollAddAllInvokers.TryAdd(collectionType, noOpInvoker);
            return noOpInvoker;
        }
        if (selectedMethodInfo.IsGenericMethod) { selectedMethodInfo = selectedMethodInfo.GetGenericArguments().Length == 1 
             ? selectedMethodInfo.MakeGenericMethod(itemType)
             : selectedMethodInfo.MakeGenericMethod(collectionType, itemType); 
        }
        
        Type invokerType = Func5TypesType.MakeGenericType
            (SimpleOrderedCollectionBuilderType, collectionType, StringType, FormatFlagsType, SimpleOrderedCollectionBuilderType);
        var invoker = isFormattable
            ? GetAddAllFormattableInvoker(SimpleOrderedCollectionBuilderType, collectionType, selectedMethodInfo, invokerType)
            : GetAddAllInvoker(SimpleOrderedCollectionBuilderType, collectionType, selectedMethodInfo, invokerType);

        OrderedCollAddAllInvokers.TryAdd(collectionType, invoker);
        return invoker;
    }


    // Created with help from https://blog.adamfurmanek.pl/2020/01/11/net-inside-out-part-14-calling-virtual-method-without-dynamic-dispatch/index.html
    // Full credit and thanks for posting 


    public static Delegate GetAddAllInvoker(Type keyValueCollectionBuilder, Type collectionToAdd, MethodInfo methodToCall, Type invokerFunc)
    {
        var helperMethod =
            new DynamicMethod($"{keyValueCollectionBuilder.Name}_{collectionToAdd.Name}_AddAll", keyValueCollectionBuilder,
                              [keyValueCollectionBuilder, collectionToAdd, StringType, FormatFlagsType], keyValueCollectionBuilder.Module
                            , true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Call, methodToCall);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(invokerFunc);
        return methodInvoker;
    }

    public static Delegate GetAddAllFormattableInvoker(Type keyValueCollectionBuilder, Type collectionToAdd, MethodInfo methodToCall
      , Type invokerFunc)
    {
        var helperMethod =
            new DynamicMethod($"{keyValueCollectionBuilder.Name}_AddAll", keyValueCollectionBuilder,
                              [keyValueCollectionBuilder, collectionToAdd, StringType, FormatFlagsType]
                            , keyValueCollectionBuilder.Module, true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Call, methodToCall);
        ilGenerator.Emit(OpCodes.Ret);

        Delegate methodInvoker;
        try { methodInvoker = helperMethod.CreateDelegate(invokerFunc); }
        catch (Exception e)
        {
            Console.Out.WriteLine
                ($"Problem creating dynamic invoker for {keyValueCollectionBuilder.CachedCSharpNameWithConstraints()}.{methodToCall.Name} " +
                 $"with args(" +
                 $"({string.Join(",", methodToCall.GetParameters().Select(pi => pi.ParameterType.CachedCSharpNameWithConstraints()))})" +
                 $" Got {e}");
            throw;
        }
        return methodInvoker;
    }

    public static Func<SimpleOrderedCollectionMold, TOrdColl, string?, SimpleOrderedCollectionMold>
        GetNonVirtualDispatchStyledToString<TOrdColl>(
            MethodInfo methodToCall)
    {
        var methodInvoker
            = (Func<SimpleOrderedCollectionMold, TOrdColl, string?, SimpleOrderedCollectionMold>)
            GetAddAllInvoker(SimpleOrderedCollectionBuilderType, typeof(TOrdColl), methodToCall
                           , typeof(Func<SimpleOrderedCollectionMold, TOrdColl, string?, SimpleOrderedCollectionMold>));
        return methodInvoker;
    }
}
