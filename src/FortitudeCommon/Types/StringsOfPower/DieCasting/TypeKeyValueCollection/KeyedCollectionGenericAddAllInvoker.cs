// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

public static class KeyedCollectionGenericAddAllInvoker
{
    private static readonly ConcurrentDictionary<Type, Delegate> KeyedCollAddAllInvokers = new();

    private const string AddAllMethodName = $"{nameof(KeyedCollectionMold.AddAll)}";

    private const string AddAllEnumerateMethodName = $"{nameof(KeyedCollectionMold.AddAllEnumerate)}";

    private static readonly MethodInfo KeyValueReadOnlyDictionaryBuilderArrayAddAll;
    private static readonly MethodInfo KeyValueCollectionBuilderArrayAddAll;
    private static readonly MethodInfo KeyValueCollectionBuilderReadOnlyListAddAll;
    private static readonly MethodInfo KeyValueCollectionEnumerableAddAllEnumerate;
    private static readonly MethodInfo KeyValueCollectionEnumeratorAddAllEnumerate;

    private static readonly Type KeyValueCollectionBuilderType = typeof(KeyedCollectionMold);
    private static readonly Type StringType                    = typeof(string);
    private static readonly Type ReadOnlyListTypeDef           = typeof(IReadOnlyList<>);
    private static readonly Type ReadOnlyDictionaryTypeDef     = typeof(IReadOnlyDictionary<,>);
    private static readonly Type EnumerableTypeDef             = typeof(IEnumerable<>);
    private static readonly Type EnumeratorTypeDef             = typeof(IEnumerator<>);
    private static readonly Type KeyValuePairTypeDef           = typeof(KeyValuePair<,>);
    private static readonly Type Func5TypesType                = typeof(Func<,,,,>);

    static KeyedCollectionGenericAddAllInvoker()
    {
        var pubMethods = KeyValueCollectionBuilderType.GetMethods(BindingFlags.Public);
        KeyValueReadOnlyDictionaryBuilderArrayAddAll = GetReadOnlyDictionaryAddAllMethodInfo(pubMethods);
        KeyValueCollectionBuilderArrayAddAll         = GetArrayAddAllMethodInfo(pubMethods);
        KeyValueCollectionBuilderReadOnlyListAddAll  = GetReadOnlyListAddAllMethodInfo(pubMethods);
        KeyValueCollectionEnumerableAddAllEnumerate  = GetEnumerableAddAllEnumerateMethodInfo(pubMethods);
        KeyValueCollectionEnumeratorAddAllEnumerate  = GetEnumeratorAddAllEnumerateMethodInfo(pubMethods);
    }

    public static void CallAddAll<TKeyColl, TKey, TValue>(KeyedCollectionMold typeMolder, TKeyColl keyColl, string? valueFormatString = null)
        where TKeyColl : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        var keyedCollType = keyColl.GetType();
        var callBaseToString = (Func<KeyedCollectionMold, TKeyColl, string?, string?, KeyedCollectionMold>)
            KeyedCollAddAllInvokers.GetOrAdd(keyedCollType, CreateInvokeMethod);

        callBaseToString(typeMolder, keyColl, valueFormatString, null);
    }

    public static void CallAddAllEnumerate<TKeyColl, TKey, TValue>(KeyedCollectionMold typeMolder, TKeyColl keyColl, string? valueFormatString = null)
        where TKeyColl : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        var keyedCollType = keyColl.GetType();
        var callBaseToString = (Func<KeyedCollectionMold, TKeyColl, string?, string?, KeyedCollectionMold>)
            KeyedCollAddAllInvokers.GetOrAdd(keyedCollType, CreateInvokeMethod);

        callBaseToString(typeMolder, keyColl, valueFormatString, null);
    }

    public static void CallAddAll<TKeyColl>(KeyedCollectionMold typeMolder, TKeyColl keyColl, string? valueFormatString = null
        , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags)
    {
        var keyedCollType = keyColl!.GetType();
        if (keyedCollType.IsKeyedCollection())
        {
            var callBaseToString = (Func<KeyedCollectionMold, TKeyColl, string?, string?, KeyedCollectionMold>)
                KeyedCollAddAllInvokers.GetOrAdd(keyedCollType, CreateInvokeMethod);

            callBaseToString(typeMolder, keyColl, valueFormatString, null);
        }
    }

    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record TypeCallCount(Type ForType)
    {
        public int CallCount { get; set; }
    };

    private static readonly ConcurrentDictionary<Type, TypeCallCount> NoOpCalls = new();

    private static KeyedCollectionMold NoOpNotASupportedKeyedCollection<T>(KeyedCollectionMold toReturn, T _ , string? _1, string? _2)
    {
        var callCount = NoOpCalls.GetOrAdd(typeof(T), new TypeCallCount(typeof(T)));
        callCount.CallCount++;
        return toReturn;
    }

    private static MethodInfo GetReadOnlyDictionaryAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsGenericType || methParam1Type.GetGenericTypeDefinition() != ReadOnlyDictionaryTypeDef) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != StringType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetArrayAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsArray) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != StringType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetReadOnlyListAddAllMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsGenericType || methParam1Type.GetGenericTypeDefinition() != ReadOnlyListTypeDef) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != StringType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetEnumerableAddAllEnumerateMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsGenericType || methParam1Type.GetGenericTypeDefinition() != EnumerableTypeDef) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != StringType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    private static MethodInfo GetEnumeratorAddAllEnumerateMethodInfo(MethodInfo[] allPubMethods)
    {
        MethodInfo? foundMatch = null;
        for (var i = 0; i < allPubMethods.Length; i++)
        {
            var mi = allPubMethods[i];
            if (mi.Name != AddAllEnumerateMethodName) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 3) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsGenericType || methParam1Type.GetGenericTypeDefinition() != EnumeratorTypeDef) continue;
            if (methodParams[1].ParameterType != StringType) continue;
            if (methodParams[2].ParameterType != StringType) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");
        return foundMatch;
    }

    public static Delegate CreateInvokeMethod(Type keyedCollType)
    {
        var keyValueTypes = keyedCollType.GetKeyedCollectionTypes()!.Value;
        var keyType       = keyValueTypes.Key;
        var valueType     = keyValueTypes.Value;

        if (keyedCollType.IsArray())
        {
            var methodToCall = KeyValueCollectionBuilderArrayAddAll.MakeGenericMethod(keyType, valueType);
            var invokerFunc = Func5TypesType.MakeGenericType
                (KeyValueCollectionBuilderType, keyedCollType, StringType, StringType, KeyValueCollectionBuilderType);
            var invoker = GetAddAllInvoker(KeyValueCollectionBuilderType, keyedCollType, methodToCall, invokerFunc);
            KeyedCollAddAllInvokers.TryAdd(keyedCollType, invoker);
            return invoker;
        }
        if (keyedCollType.IsReadOnlyList())
        {
            var keyValueType     = KeyValuePairTypeDef.MakeGenericType(keyType, valueType);
            var readOnlyListType = ReadOnlyListTypeDef.MakeGenericType(keyValueType);
            if (KeyedCollAddAllInvokers.TryGetValue(readOnlyListType, out var genericReadOnlyListInvoker))
            {
                KeyedCollAddAllInvokers.TryAdd(keyedCollType, genericReadOnlyListInvoker);
                return genericReadOnlyListInvoker;
            }
            var methodToCall = KeyValueCollectionBuilderReadOnlyListAddAll.MakeGenericMethod(keyType, valueType);
            var invokerFunc = Func5TypesType.MakeGenericType
                (KeyValueCollectionBuilderType, readOnlyListType, StringType, StringType, KeyValueCollectionBuilderType);
            var invoker = GetAddAllInvoker(KeyValueCollectionBuilderType, readOnlyListType, methodToCall, invokerFunc);
            KeyedCollAddAllInvokers.TryAdd(readOnlyListType, invoker);
            KeyedCollAddAllInvokers.TryAdd(keyedCollType, invoker);
            return invoker;
        }
        if (keyedCollType.IsReadOnlyDictionaryType())
        {
            var readOnlyDictType = ReadOnlyDictionaryTypeDef.MakeGenericType(keyType, valueType);
            if (KeyedCollAddAllInvokers.TryGetValue(readOnlyDictType, out var genericReadOnlyDictInvoker))
            {
                KeyedCollAddAllInvokers.TryAdd(keyedCollType, genericReadOnlyDictInvoker);
                return genericReadOnlyDictInvoker;
            }
            var methodToCall = KeyValueReadOnlyDictionaryBuilderArrayAddAll.MakeGenericMethod(keyType, valueType);
            var invokerFunc = Func5TypesType.MakeGenericType
                (KeyValueCollectionBuilderType, readOnlyDictType, StringType, StringType, KeyValueCollectionBuilderType);
            var invoker = GetAddAllInvoker(KeyValueCollectionBuilderType, readOnlyDictType, methodToCall, invokerFunc);
            KeyedCollAddAllInvokers.TryAdd(readOnlyDictType, invoker);
            KeyedCollAddAllInvokers.TryAdd(keyedCollType, invoker);
            return invoker;
        }
        if (keyedCollType.IsEnumerable())
        {
            var keyValueType           = KeyValuePairTypeDef.MakeGenericType(keyType, valueType);
            var keyValueEnumerableType = EnumerableTypeDef.MakeGenericType(keyValueType);
            if (KeyedCollAddAllInvokers.TryGetValue(keyValueEnumerableType, out var genericKeyValueEnumerableInvoker))
            {
                KeyedCollAddAllInvokers.TryAdd(keyedCollType, genericKeyValueEnumerableInvoker);
                return genericKeyValueEnumerableInvoker;
            }
            var methodToCall = KeyValueCollectionEnumerableAddAllEnumerate.MakeGenericMethod(keyType, valueType);
            var invokerFunc = Func5TypesType.MakeGenericType
                (KeyValueCollectionBuilderType, keyValueEnumerableType, StringType, StringType, KeyValueCollectionBuilderType);
            var invoker = GetAddAllInvoker(KeyValueCollectionBuilderType, keyValueEnumerableType, methodToCall, invokerFunc);
            KeyedCollAddAllInvokers.TryAdd(keyValueEnumerableType, invoker);
            KeyedCollAddAllInvokers.TryAdd(keyedCollType, invoker);
            return invoker;
        }
        if (keyedCollType.IsEnumerator())
        {
            var keyValueType           = KeyValuePairTypeDef.MakeGenericType(keyType, valueType);
            var keyValueEnumeratorType = EnumeratorTypeDef.MakeGenericType(keyValueType);
            if (KeyedCollAddAllInvokers.TryGetValue(keyValueEnumeratorType, out var genericKeyValueEnumeratorInvoker))
            {
                KeyedCollAddAllInvokers.TryAdd(keyedCollType, genericKeyValueEnumeratorInvoker);
                return genericKeyValueEnumeratorInvoker;
            }
            var methodToCall = KeyValueCollectionEnumeratorAddAllEnumerate.MakeGenericMethod(keyType, valueType);
            var invokerFunc = Func5TypesType.MakeGenericType
                (KeyValueCollectionBuilderType, keyValueEnumeratorType, StringType, StringType, KeyValueCollectionBuilderType);
            var invoker = GetAddAllInvoker(KeyValueCollectionBuilderType, keyValueEnumeratorType, methodToCall, invokerFunc);
            KeyedCollAddAllInvokers.TryAdd(keyValueEnumeratorType, invoker);
            KeyedCollAddAllInvokers.TryAdd(keyedCollType, invoker);
            return invoker;
        }
        var myType = typeof(KeyedCollectionGenericAddAllInvoker);
        var noOpMethodToCall = myType.GetMethod($"{nameof(NoOpNotASupportedKeyedCollection)}", [
            KeyValueCollectionBuilderType, keyedCollType, StringType, StringType
          , KeyValueCollectionBuilderType
        ]);
        noOpMethodToCall = noOpMethodToCall!.MakeGenericMethod(keyedCollType);
        var noOpInvokerFunc = Func5TypesType.MakeGenericType
            (KeyValueCollectionBuilderType, keyedCollType, StringType, StringType, KeyValueCollectionBuilderType);
        var noOpInvoker = GetAddAllInvoker(KeyValueCollectionBuilderType, keyedCollType, noOpMethodToCall, noOpInvokerFunc);
        KeyedCollAddAllInvokers.TryAdd(keyedCollType, noOpInvoker);
        return noOpInvoker;
    }


    // Created with help from https://blog.adamfurmanek.pl/2020/01/11/net-inside-out-part-14-calling-virtual-method-without-dynamic-dispatch/index.html
    // Full credit and thanks for posting 
    public static Delegate GetAddAllInvoker(Type keyValueCollectionBuilder, Type collectionToAdd, MethodInfo methodToCall, Type invokerFunc)
    {
        var helperMethod =
            new DynamicMethod($"{keyValueCollectionBuilder.Name}_AddAll", keyValueCollectionBuilder,
                              [keyValueCollectionBuilder, collectionToAdd, StringType, StringType], keyValueCollectionBuilder.Module, true);
        var ilGenerator = helperMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldarg_3);
        ilGenerator.Emit(OpCodes.Call, methodToCall);
        ilGenerator.Emit(OpCodes.Ret);
        var methodInvoker = helperMethod.CreateDelegate(invokerFunc);
        return methodInvoker;
    }

    public static Func<KeyedCollectionMold, T, string?, string?, KeyedCollectionMold> GetNonVirtualDispatchStyledToString<T>(
        MethodInfo methodToCall)
    {
        var methodInvoker
            = (Func<KeyedCollectionMold, T, string?, string?, KeyedCollectionMold>)
            GetAddAllInvoker(KeyValueCollectionBuilderType, typeof(T), methodToCall
                           , typeof(Func<KeyedCollectionMold, T, string?, string?, KeyedCollectionMold>));
        return methodInvoker;
    }
}
