// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder
{
    private Action<T, ITheOneString>? CheckCollectionForInvoker<T>(T maybeCollection, Type collectionType)
    {
        try
        {
            if (collectionType.IsValueType || collectionType.IsNotCollection()) return null;
            var collElementType = collectionType.GetIndexedCollectionElementType() ?? collectionType.GetIterableElementType();
            if (collElementType == null || collectionType.GenericTypeArguments.Length > 1 || collElementType.IsKeyValuePair()) return null;
            if (collectionType.IsIndexableOrderedCollection())
            {
                var structStylerInvoker =
                    TryBuildIndexedCollectionInvoker(maybeCollection, collectionType, collElementType);
                return structStylerInvoker;
            }
            if (collectionType.IsIterable())
            {
                var structStylerInvoker =
                    TryBuildIterableInvoker(maybeCollection, collectionType, collElementType);
                return structStylerInvoker;
            }
        }
        catch (Exception ex)
        {
            var warningAppender = CreateWarningMessageAppender(FLogLevel.Warn);
            warningAppender
                .Append("Problem trying to create dynamic collection type appender for collection type '").Append(collectionType.Name)
                .Append("'. Got ").FinalAppendObject(ex);
        }
        return null;
    }

    private Action<T, ITheOneString>? CheckCollectionFor2ItemTupleInvoker<T>(T tuple, Type tupleType, Type item1Type, Type item2Type)
    {
        try
        {
            if (item1Type.IsValueType || item1Type.IsNotCollection()) return null;
            var collElementType = item1Type.GetIndexedCollectionElementType() ?? item1Type.GetIterableElementType();
            if (collElementType == null || item1Type.GenericTypeArguments.Length > 1 || collElementType.IsKeyValuePair()) return null;

            if (item1Type.IsIndexableOrderedCollection())
            {
                if (item2Type.IsFormatterType())
                {
                    if (item1Type.IsValidFormatterForType(item2Type))
                    {
                        var structStylerInvoker = TryBuildIndexedCollectionFormatInvoker(tuple, tupleType, item1Type, item2Type);
                        return structStylerInvoker;
                    }
                    var warningAppender = CreateWarningMessageAppender();
                    warningAppender.Append("Incompatible format appender '").AppendObject(item2Type).Append("' for tuple ")
                                   .Append(" ValueTuple<").Append(item1Type.Name).Append(", ").Append(item2Type.Name).FinalAppend(">");
                }
                if (item2Type.IsOrderedCollectionFilterPredicate())
                {
                    var structStylerInvoker =
                        TryBuildFilteredIndexedCollectionFormatInvoker(tuple, tupleType, item1Type, item2Type);
                    return structStylerInvoker;
                }
            }
            else if (item1Type.IsIterable())
            {
                if (item2Type.IsFormatterType())
                {
                    if (item1Type.IsValidFormatterForType(item2Type))
                    {
                        var structStylerInvoker =
                            TryBuildIterableFormatInvoker(tuple, tupleType, item1Type, item2Type);
                        return structStylerInvoker;
                    }
                    var warningAppender = CreateWarningMessageAppender();
                    warningAppender.Append("Incompatible format appender '").AppendObject(item2Type).Append("' for tuple ")
                                   .Append(" ValueTuple<").Append(item1Type.Name).Append(", ").Append(item2Type.Name).FinalAppend(">");
                }
            }
        }
        catch (Exception ex)
        {
            var warningAppender = CreateWarningMessageAppender(FLogLevel.Warn);
            warningAppender
                .Append("Problem trying to create dynamic collection type appender for collection type '").Append(item1Type.Name)
                .Append("' for tuple ").Append(" ValueTuple<").Append(item1Type.Name).Append(", ")
                .Append(item2Type.Name).Append(">. Got ").FinalAppendObject(ex);
        }

        return null;
    }

    private Action<T, ITheOneString>? CheckCollectionFor3ItemTupleInvoker<T>(T tuple, Type tupleType, Type item1Type, Type item2Type
      , Type item3Type)
    {
        try
        {
            if (item1Type.IsValueType || !item1Type.IsCollection()) return null;
            if (item1Type.IsIndexableOrderedCollection())
                if (item3Type.IsFormatterType())
                    if (item2Type.IsOrderedCollectionFilterPredicate())
                    {
                        var structStylerInvoker =
                            TryBuildFilteredIndexedCollectionFormatInvoker(tuple, tupleType, item1Type, item2Type, item3Type);
                        return structStylerInvoker;
                    }
        }
        catch (Exception ex)
        {
            var warningAppender = CreateWarningMessageAppender(FLogLevel.Warn);
            warningAppender
                .Append("Problem trying to create dynamic collection type appender for collection type '").Append(item1Type.Name)
                .Append("' for tuple ").Append(" ValueTuple<").Append(item1Type.Name).Append(", ").Append(item2Type.Name).Append(", ")
                .Append(item3Type.Name).Append(">. Got ").FinalAppendObject(ex);
        }

        return null;
    }

    private Action<T, ITheOneString> TryBuildIterableInvoker<T>(T iterableToAppend, Type iterableType, Type indexedCollElementType)
    {
        var methodName = indexedCollElementType.IsValueType ? nameof(AppendValueCollectionEnumerate) : nameof(AppendObjectCollectionEnumerate);

        var iterableNotEnumerable = iterableType.IsNotEnumerable();
        var iterableNotEnumerator = iterableType.IsNotEnumerator();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != methodName || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2Type = methodParams[1].ParameterType;
            if (methParam2Type != typeof(ITheOneString)) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsGenericType) continue;
            var p1Item1NotEnumerable = methParam1Type.IsNotEnumerable();
            var p1Item1NotEnumerator = methParam1Type.IsNotEnumerator();
            if (p1Item1NotEnumerable && p1Item1NotEnumerator) continue;
            if (!p1Item1NotEnumerable && iterableNotEnumerable) continue;
            if (!p1Item1NotEnumerator && iterableNotEnumerator) continue;
            if (genericParams[0].IsAssignableFrom(indexedCollElementType)) continue;
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend = CreateSingleGenericArgMethodInvoker(iterableToAppend, iterableType, foundMatch, indexedCollElementType);
        return invokeAppend;
    }

    private Action<T, ITheOneString> TryBuildIndexedCollectionInvoker<T>(T collectionToAppend, Type collectionType
      , Type indexedCollElementType)
    {
        var methodName = indexedCollElementType.IsValueType ? nameof(AppendValueCollection) : nameof(AppendObjectCollection);

        var collectionNotReadOnlyList = collectionType.IsNotReadOnlyList();
        var collectionNotArray        = collectionType.IsNotArray();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];

            if (mi.Name != methodName || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2Type = methodParams[1].ParameterType;
            if (methParam2Type != typeof(ITheOneString)) continue;
            var methParam1Type         = methodParams[0].ParameterType;
            var p1Item1NotReadOnlyList = methParam1Type.IsNotReadOnlyList();
            var p1Item1NotArray        = methParam1Type.IsNotArray();
            if (p1Item1NotReadOnlyList && p1Item1NotArray) continue;
            if (!p1Item1NotReadOnlyList && collectionNotReadOnlyList) continue;
            if (!p1Item1NotArray && collectionNotArray) continue;
            if (genericParams[0].IsAssignableFrom(indexedCollElementType)) continue;
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend = CreateSingleGenericArgMethodInvoker(collectionToAppend, collectionType, foundMatch, indexedCollElementType);
        return invokeAppend;
    }

    private Action<T, ITheOneString> TryBuildIterableFormatInvoker<T>(T toAppend, Type typeOfT, Type item1Iterable
      , Type item2Formatter)
    {
        var indexedCollElementType = item1Iterable.GetIndexedCollectionElementType()
                                  ?? throw new InvalidOperationException("Expected item1IndexableColl to be either an array or List");
        var methodName = indexedCollElementType.IsValueType ? nameof(AppendValueCollectionEnumerate) : nameof(AppendObjectCollectionEnumerate);

        var iterableIsNotEnumerable = item1Iterable.IsNotEnumerable();
        var iterableIsNotEnumerator = item1Iterable.IsNotEnumerator();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != methodName || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2Type = methodParams[1].ParameterType;
            if (methParam2Type != typeof(ITheOneString)) continue;
            var methParam1Type = methodParams[0].ParameterType;
            if (!methParam1Type.IsGenericType) continue;
            if (methParam1Type.GetGenericTypeDefinition() != typeof(ValueTuple<,>)) continue;
            var methParam1Item1Type  = methParam1Type.GenericTypeArguments[0];
            var p1Item1NotEnumerable = methParam1Item1Type.IsNotEnumerable();
            var p1Item1NotEnumerator = methParam1Item1Type.IsNotEnumerator();
            if (p1Item1NotEnumerable && p1Item1NotEnumerator) continue;
            if (!p1Item1NotEnumerable && iterableIsNotEnumerable) continue;
            if (!p1Item1NotEnumerator && iterableIsNotEnumerator) continue;
            var methParam1Item2Type = methParam1Type.GenericTypeArguments[1];
            if (methParam1Item2Type != item2Formatter) continue;
            if (genericParams[0].IsAssignableFrom(indexedCollElementType)) continue;
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend = CreateSingleGenericArgMethodInvoker(toAppend, typeOfT, foundMatch, indexedCollElementType);
        return invokeAppend;
    }

    private Action<T, ITheOneString> TryBuildIndexedCollectionFormatInvoker<T>(T toAppend, Type typeOfT, Type item1IndexableColl
      , Type item2Formatter)
    {
        var indexedCollElementType = item1IndexableColl.GetIndexedCollectionElementType()
                                  ?? throw new InvalidOperationException("Expected item1IndexableColl to be either an array or List");
        var methodName = indexedCollElementType.IsValueType ? nameof(AppendValueCollection) : nameof(AppendObjectCollection);

        var collectionIsNotArrayType        = item1IndexableColl.IsNotArray();
        var collectionIsNotReadOnlyListType = item1IndexableColl.IsNotReadOnlyList();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != methodName || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2 = methodParams[1].ParameterType;
            if (methParam2 != typeof(ITheOneString)) continue;
            var methParam1 = methodParams[0].ParameterType;
            if (!methParam1.IsGenericType) continue;
            if (methParam1.GetGenericTypeDefinition() != typeof(ValueTuple<,>)) continue;
            var methParam1Item1Type    = methParam1.GenericTypeArguments[0];
            var p1Item1NotReadOnlyList = methParam1Item1Type.IsNotReadOnlyList();
            var p1Item1NotArray        = methParam1Item1Type.IsNotArray();
            if (p1Item1NotReadOnlyList && p1Item1NotArray) continue;
            if (!p1Item1NotReadOnlyList && collectionIsNotReadOnlyListType) continue;
            if (!p1Item1NotArray && collectionIsNotArrayType) continue;
            var methParam1Item2Type = methParam1.GenericTypeArguments[1];
            if (methParam1Item2Type != item2Formatter) continue;
            if (genericParams[0].IsAssignableFrom(indexedCollElementType)) continue;
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend = CreateSingleGenericArgMethodInvoker(toAppend, typeOfT, foundMatch, indexedCollElementType);
        return invokeAppend;
    }

    private Action<T, ITheOneString> TryBuildFilteredIndexedCollectionFormatInvoker<T>
        (T toAppend, Type tupleType, Type item1IndexableColl, Type item2FilterPredicate, Type? item3Formatter = null)
    {
        var indexedCollElementType = item1IndexableColl.GetIndexedCollectionElementType()
                                  ?? throw new InvalidOperationException("Expected item1IndexableColl to be either an array or List");
        var methodName = indexedCollElementType.IsValueType ? nameof(AppendFilteredValueCollection) : nameof(AppendFilteredObjectCollection);

        var collectionIsNotArrayType        = item1IndexableColl.IsNotArray();
        var collectionIsNotReadOnlyListType = item1IndexableColl.IsNotReadOnlyList();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != methodName || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2 = methodParams[1].ParameterType;
            if (methParam2 != typeof(ITheOneString)) continue;
            var methParam1 = methodParams[0].ParameterType;
            if (!methParam1.IsGenericType) continue;
            var genericValueTuple = methParam1.GetGenericTypeDefinition();
            if (item3Formatter != null && genericValueTuple != typeof(ValueTuple<,,>)) continue;
            if (item3Formatter == null && genericValueTuple != typeof(ValueTuple<,>)) continue;
            var methParam1Item1Type    = methParam1.GenericTypeArguments[0];
            var p1Item1NotReadOnlyList = methParam1Item1Type.IsNotReadOnlyList();
            var p1Item1NotArray        = methParam1Item1Type.IsNotArray();
            if (p1Item1NotReadOnlyList && p1Item1NotArray) continue;
            if (!p1Item1NotReadOnlyList && collectionIsNotReadOnlyListType) continue;
            if (!p1Item1NotArray && collectionIsNotArrayType) continue;
            var methParam1Item2Type = methParam1.GenericTypeArguments[1];
            if (!methParam1Item2Type.IsGenericType) continue;
            if (methParam1Item2Type.GetGenericTypeDefinition() != typeof(OrderedCollectionPredicate<>)) continue;
            var methParam1Item2TypeGen1 = methParam1Item2Type.GenericTypeArguments[0];
            if (methParam1Item2TypeGen1.IsAssignableFrom(indexedCollElementType)) continue;
            if (methParam1Item2TypeGen1.IsAssignableFrom(item2FilterPredicate.GenericTypeArguments[0])) continue;
            if (genericParams[0].IsAssignableFrom(indexedCollElementType)) continue;
            if (item3Formatter != null)
            {
                var methParam1Item3Type = methParam1.GenericTypeArguments[2];
                if (methParam1Item3Type != item3Formatter) continue;
            }
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend =
            CreateSingleGenericArgMethodInvoker(toAppend, tupleType, foundMatch, indexedCollElementType);
        return invokeAppend;
    }
}
