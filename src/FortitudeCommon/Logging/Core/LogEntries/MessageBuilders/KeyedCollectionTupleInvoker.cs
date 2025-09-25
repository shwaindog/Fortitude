// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder
{
    private Action<T, ITheOneString>? CheckKeyedCollectionForInvoker<T>(T maybeCollection, Type collectionType)
    {
        try
        {
            if (collectionType.IsValueType || !collectionType.IsCollection()) return null;
            var keyValueTypes = collectionType.GetKeyedCollectionTypes();
            if (keyValueTypes == null) return null;
            if (collectionType.IsIndexableOrderedCollection())
            {
                var structStylerInvoker =
                    TryBuildKeyedCollectionInvoker(maybeCollection, collectionType, keyValueTypes.Value);
                return structStylerInvoker;
            }
            if (collectionType.IsIterable())
            {
                var structStylerInvoker =
                    TryBuildKeyValueIterableInvoker(maybeCollection, collectionType, keyValueTypes.Value);
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

    private Action<T, ITheOneString>? CheckKeyedCollectionFor2ItemTupleInvoker<T>(T tuple, Type tupleType, Type item1Type, Type item2Type)
    {
        try
        {
            if (item1Type.IsValueType || !item1Type.IsCollection()) return null;
            var checkKeyValueTypes = item1Type.GetKeyedCollectionTypes();
            if (checkKeyValueTypes == null) return null;
            var keyValueTypes = checkKeyValueTypes.Value;

            if (item1Type.IsReadOnlyDictionaryType() || item1Type.IsKeyValueOrderedCollection())
            {
                if (item2Type.IsFormatterType())
                {
                    if (keyValueTypes.Value.IsValidFormatterForType(item2Type))
                    {
                        var structStylerInvoker =
                            TryBuildKeyedCollectionFormatInvoker(tuple, tupleType, item1Type, keyValueTypes, item2Type);
                        return structStylerInvoker;
                    }
                    var warningAppender = CreateWarningMessageAppender();
                    warningAppender.Append("Incompatible format appender '").AppendObject(item2Type).Append("' for tuple ")
                                   .Append(" ValueTuple<").Append(item1Type.Name).Append(", ").Append(item2Type.Name).FinalAppend(">");
                }
                if (item2Type.IsKeyValueFilterPredicate())
                {
                    var structStylerInvoker =
                        TryBuildFilteredKeyedCollectionFormatInvoker(tuple, tupleType, item1Type, keyValueTypes, item2Type);
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
                            TryBuildKeyValueIterableFormatInvoker(tuple, tupleType, item1Type, keyValueTypes, item2Type);
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

    private Action<T, ITheOneString>? CheckKeyedCollectionFor3ItemTupleInvoker<T>(T tuple, Type tupleType, Type item1Type, Type item2Type
      , Type item3Type)
    {
        try
        {
            if (item1Type.IsValueType || !item1Type.IsCollection()) return null;
            var checkKeyValueTypes = item1Type.GetKeyedCollectionTypes();
            if (checkKeyValueTypes == null) return null;
            var keyValueTypes = checkKeyValueTypes.Value;

            if (item1Type.IsReadOnlyDictionaryType() || item1Type.IsKeyValueOrderedCollection())
            {
                if (item2Type.IsFormatterType())
                {
                    if (keyValueTypes.Value.IsValidFormatterForType(item2Type))
                    {
                        var structStylerInvoker =
                            TryBuildKeyedCollectionFormatInvoker(tuple, tupleType, item1Type, keyValueTypes, item2Type, item3Type);
                        return structStylerInvoker;
                    }
                    var warningAppender = CreateWarningMessageAppender();
                    warningAppender.Append("Incompatible format appender '").AppendObject(item2Type).Append("' for tuple ")
                                   .Append(" ValueTuple<").Append(item1Type.Name).Append(", ").Append(item2Type.Name).FinalAppend(">");
                }
                if (item2Type.IsKeyValueFilterPredicate())
                {
                    var structStylerInvoker =
                        TryBuildFilteredKeyedCollectionFormatInvoker(tuple, tupleType, item1Type, keyValueTypes, item2Type, item3Type);
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
                            TryBuildKeyValueIterableFormatInvoker(tuple, tupleType, item1Type, keyValueTypes, item2Type, item3Type);
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
                .Append("' for tuple ").Append(" ValueTuple<").Append(item1Type.Name).Append(", ").Append(item2Type.Name).Append(", ")
                .Append(item3Type.Name).Append(">. Got ").FinalAppendObject(ex);
        }

        return null;
    }

    private Action<T, ITheOneString>? CheckKeyedCollectionFor4ItemTupleInvoker<T>(T tuple, Type tupleType, Type item1Type, Type item2Type
      , Type item3Type, Type item4Type)
    {
        try
        {
            if (item1Type.IsValueType || !item1Type.IsCollection()) return null;
            var checkKeyValueTypes = item1Type.GetKeyedCollectionTypes();
            if (checkKeyValueTypes == null) return null;
            var keyValueTypes = checkKeyValueTypes.Value;

            if (item1Type.IsReadOnlyDictionaryType() || item1Type.IsKeyValueOrderedCollection())
                if (item2Type.IsKeyValueFilterPredicate())
                {
                    var structStylerInvoker =
                        TryBuildFilteredKeyedCollectionFormatInvoker(tuple, tupleType, item1Type, keyValueTypes, item2Type, item3Type, item4Type);
                    return structStylerInvoker;
                }
        }
        catch (Exception ex)
        {
            var warningAppender = CreateWarningMessageAppender(FLogLevel.Warn);
            warningAppender
                .Append("Problem trying to create dynamic collection type appender for collection type '").Append(item1Type.Name)
                .Append("' for tuple ").Append(" ValueTuple<").Append(item1Type.Name).Append(", ").Append(item2Type.Name).Append(", ")
                .Append(item3Type.Name).Append(", ").Append(item4Type.Name).Append(">. Got ").FinalAppendObject(ex);
        }

        return null;
    }

    private Action<T, ITheOneString> TryBuildKeyValueIterableInvoker<T>(T iterableToAppend, Type iterableType
      , KeyValuePair<Type, Type> keyedCollectionTypes)
    {
        var iterableIsNotEnumerable = iterableType.IsNotEnumerable();
        var iterableIsNotEnumerator = iterableType.IsNotEnumerator();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != nameof(AppendKeyedCollectionEnumerate) || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2 = methodParams[1].ParameterType;
            if (methParam2 != typeof(ITheOneString)) continue;
            var methParam1 = methodParams[0].ParameterType;
            if (!methParam1.IsGenericType) continue;
            var p1Item1NotEnumerable = iterableType.IsNotEnumerable();
            var p1Item1NotEnumerator = iterableType.IsNotEnumerator();
            if (p1Item1NotEnumerable && p1Item1NotEnumerator) continue;
            if (!p1Item1NotEnumerable && iterableIsNotEnumerable) continue;
            if (!p1Item1NotEnumerator && iterableIsNotEnumerator) continue;
            if (genericParams[0].IsAssignableFrom(keyedCollectionTypes.Key)) continue;
            if (genericParams[1].IsAssignableFrom(keyedCollectionTypes.Value)) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend =
            CreateTwoGenericArgsMethodInvoker(iterableToAppend, iterableType, foundMatch, keyedCollectionTypes.Key, keyedCollectionTypes.Value);
        return invokeAppend;
    }

    private Action<T, ITheOneString> TryBuildKeyedCollectionInvoker<T>(T collectionToAppend, Type collectionType,
        KeyValuePair<Type, Type> keyedCollectionTypes)
    {
        var collectionIsNotReadOnlyDictionaryType = collectionType.IsNotReadOnlyDictionaryType();
        var collectionIsNotArrayType              = collectionType.IsNotArray();
        var collectionIsNotReadOnlyListType       = collectionType.IsNotReadOnlyList();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != nameof(AppendKeyedCollection) || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2Type = methodParams[1].ParameterType;
            if (methParam2Type != typeof(ITheOneString)) continue;
            var methParam1Type               = methodParams[0].ParameterType;
            var p1Item1NotReadOnlyDict       = methParam1Type.IsNotReadOnlyDictionaryType();
            var p1Item1NotArray              = methParam1Type.IsNotArray();
            var p1Item1NotKeyValueCollection = collectionType.IsNotReadOnlyList();
            if (p1Item1NotReadOnlyDict && p1Item1NotArray && p1Item1NotKeyValueCollection) continue;
            if (!p1Item1NotReadOnlyDict && collectionIsNotReadOnlyDictionaryType) continue;
            if (!p1Item1NotArray && collectionIsNotArrayType) continue;
            if (!p1Item1NotKeyValueCollection && collectionIsNotReadOnlyListType) continue;
            if (genericParams[0].IsAssignableFrom(keyedCollectionTypes.Key)) continue;
            if (genericParams[1].IsAssignableFrom(keyedCollectionTypes.Value)) continue;

            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend =
            CreateTwoGenericArgsMethodInvoker(collectionToAppend, collectionType, foundMatch
                                            , keyedCollectionTypes.Key, keyedCollectionTypes.Value);
        return invokeAppend;
    }

    private Action<T, ITheOneString> TryBuildKeyValueIterableFormatInvoker<T>(T toAppend, Type tupleType, Type item1IterableType
      , KeyValuePair<Type, Type> keyedCollectionTypes, Type item2Formatter, Type? item3Formatter = null)
    {
        var iterableIsNotEnumerable = item1IterableType.IsNotEnumerable();
        var iterableIsNotEnumerator = item1IterableType.IsNotEnumerator();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != nameof(AppendKeyedCollectionEnumerate) || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 2) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2 = methodParams[1].ParameterType;
            if (methParam2 != typeof(ITheOneString)) continue;
            var methParam1 = methodParams[0].ParameterType;
            if (!methParam1.IsGenericType) continue;
            var genericValueTuple = methParam1.GetGenericTypeDefinition();
            if (item3Formatter != null && genericValueTuple != typeof(ValueTuple<,,>)) continue;
            if (item3Formatter == null && genericValueTuple != typeof(ValueTuple<,>)) continue;
            var methParam1Item1Type  = methParam1.GenericTypeArguments[0];
            var p1Item1NotEnumerable = methParam1Item1Type.IsNotEnumerable();
            var p1Item1NotEnumerator = methParam1Item1Type.IsNotEnumerator();
            if (p1Item1NotEnumerable && p1Item1NotEnumerator) continue;
            if (!p1Item1NotEnumerable && iterableIsNotEnumerable) continue;
            if (!p1Item1NotEnumerator && iterableIsNotEnumerator) continue;
            var methParam1Item2Type = methParam1.GenericTypeArguments[1];
            if (methParam1Item2Type != item2Formatter) continue;
            if (genericParams[0].IsAssignableFrom(keyedCollectionTypes.Key)) continue;
            if (genericParams[1].IsAssignableFrom(keyedCollectionTypes.Value)) continue;
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
            CreateTwoGenericArgsMethodInvoker(toAppend, tupleType, foundMatch
                                            , keyedCollectionTypes.Key, keyedCollectionTypes.Value);
        return invokeAppend;
    }

    private Action<T, ITheOneString> TryBuildKeyedCollectionFormatInvoker<T>(T toAppend, Type typeOfT, Type item1KeyedCollection
      , KeyValuePair<Type, Type> keyedCollectionTypes, Type item2Formatter, Type? item3Formatter = null)
    {
        var collectionIsNotReadOnlyDictionaryType = item1KeyedCollection.IsNotReadOnlyDictionaryType();
        var collectionIsNotArrayType              = item1KeyedCollection.IsNotArray();
        var collectionIsNotReadOnlyListType       = item1KeyedCollection.IsNotReadOnlyList();

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != nameof(AppendKeyedCollection) || !mi.IsStatic) continue;
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
            var methParam1Item1Type          = methParam1.GenericTypeArguments[0];
            var p1Item1NotReadOnlyDict       = methParam1Item1Type.IsNotReadOnlyDictionaryType();
            var p1Item1NotArray              = methParam1Item1Type.IsNotArray();
            var p1Item1NotKeyValueCollection = methParam1Item1Type.IsNotReadOnlyList();
            if (p1Item1NotReadOnlyDict && p1Item1NotArray && p1Item1NotKeyValueCollection) continue;
            if (!p1Item1NotReadOnlyDict && collectionIsNotReadOnlyDictionaryType) continue;
            if (!p1Item1NotArray && collectionIsNotArrayType) continue;
            if (!p1Item1NotKeyValueCollection && collectionIsNotReadOnlyListType) continue;
            var methParam1Item2Type = methParam1.GenericTypeArguments[1];
            if (methParam1Item2Type != item2Formatter) continue;
            if (genericParams[0].IsAssignableFrom(keyedCollectionTypes.Key)) continue;
            if (genericParams[1].IsAssignableFrom(keyedCollectionTypes.Value)) continue;
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
            CreateTwoGenericArgsMethodInvoker(toAppend, typeOfT, foundMatch
                                            , keyedCollectionTypes.Key, keyedCollectionTypes.Value);
        return invokeAppend;
    }

    private Action<T, ITheOneString> TryBuildFilteredKeyedCollectionFormatInvoker<T>
    (T toAppend, Type tupleType, Type item1IndexableColl, KeyValuePair<Type, Type> keyedCollectionTypes
      , Type item2FilterPredicate, Type? item3Formatter = null, Type? item4Formatter = null)
    {
        var collectionIsNotReadOnlyDictionaryType = item1IndexableColl.IsNotReadOnlyDictionaryType();
        var collectionIsNotArrayType              = item1IndexableColl.IsNotArray();
        var collectionIsNotReadOnlyListType       = item1IndexableColl.IsNotReadOnlyList();

        var tupleNumItems                         = 2;
        if (item3Formatter != null) tupleNumItems = 3;
        if (item4Formatter != null) tupleNumItems = 4;

        MethodInfo? foundMatch = null;
        for (var i = 0; i < MyNonPubStaticMethods.Length; i++)
        {
            var mi = MyNonPubStaticMethods[i];
            if (mi.Name != nameof(AppendFilteredKeyedCollection) || !mi.IsStatic) continue;
            var genericParams = mi.GetGenericArguments();
            if (genericParams.Length != 1) continue;
            var methodParams = mi.GetParameters();
            if (methodParams.Length != 2) continue;
            var methParam2 = methodParams[1].ParameterType;
            if (methParam2 != typeof(ITheOneString)) continue;
            var methParam1 = methodParams[0].ParameterType;
            if (!methParam1.IsGenericType) continue;
            var genericValueTuple = methParam1.GetGenericTypeDefinition();
            if (tupleNumItems == 4 && genericValueTuple != typeof(ValueTuple<,,,>)) continue;
            if (tupleNumItems == 3 && genericValueTuple != typeof(ValueTuple<,,>)) continue;
            if (tupleNumItems == 2 && genericValueTuple != typeof(ValueTuple<,>)) continue;
            var methParam1Item1Type          = methParam1.GenericTypeArguments[0];
            var p1Item1NotReadOnlyDict       = methParam1Item1Type.IsNotReadOnlyDictionaryType();
            var p1Item1NotArray              = methParam1Item1Type.IsNotArray();
            var p1Item1NotKeyValueCollection = methParam1Item1Type.IsNotReadOnlyList();
            if (p1Item1NotReadOnlyDict && p1Item1NotArray && p1Item1NotKeyValueCollection) continue;
            if (!p1Item1NotReadOnlyDict && collectionIsNotReadOnlyDictionaryType) continue;
            if (!p1Item1NotArray && collectionIsNotArrayType) continue;
            if (!p1Item1NotKeyValueCollection && collectionIsNotReadOnlyListType) continue;
            var methParam1Item2Type = methParam1.GenericTypeArguments[1];
            if (!methParam1Item2Type.IsGenericType) continue;
            if (methParam1Item2Type.GetGenericTypeDefinition() != typeof(KeyValuePredicate<,>)) continue;
            if (genericParams[0].IsAssignableFrom(keyedCollectionTypes.Key)) continue;
            if (genericParams[1].IsAssignableFrom(keyedCollectionTypes.Value)) continue;
            if (tupleNumItems >= 3)
            {
                var methParam1Item3Type = methParam1.GenericTypeArguments[2];
                if (methParam1Item3Type != item3Formatter) continue;
            }
            if (tupleNumItems >= 4)
            {
                var methParam1Item3Type = methParam1.GenericTypeArguments[3];
                if (methParam1Item3Type != item4Formatter) continue;
            }
            foundMatch = mi;
            break;
        }
        if (foundMatch == null) throw new InvalidOperationException("Method does not exist");

        var invokeAppend =
            CreateTwoGenericArgsMethodInvoker(toAppend, tupleType, foundMatch
                                            , keyedCollectionTypes.Key, keyedCollectionTypes.Value);
        return invokeAppend;
    }
}
