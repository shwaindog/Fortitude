// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public readonly struct ObjectOrArrayStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrArrayStructUnion>? nodeRevealer;

    public ObjectOrArrayStructUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public ObjectOrArrayStructUnion(object?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public ObjectOrArrayStructUnion(ObjectOrArrayStructUnion[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrArrayStructUnion>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    public ObjectOrArrayStructUnion(ObjectOrArrayStructUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrArrayStructUnion>? withNodeRevealer = null)
    {
        isNullableNode         = true;
        isNode                 = true;
        nullableNodeCollection = nodeColl;
        nodeRevealer           = withNodeRevealer;
        isSimple               = asSimple;
        isValue                = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly object? item;
    private readonly object?[]? itemCollection;

    private readonly ObjectOrArrayStructUnion[]?  nodeCollection         = null;
    private readonly ObjectOrArrayStructUnion?[]? nullableNodeCollection = null;

    private static readonly char[] LogComplexOnlyStaticInstance
        = "\"This\" is only shown when StringStyle is Log and in a Complex Mold".ToCharArray();

    private readonly char[] logComplexOnlyInstance = "\"This\" is only shown when StringStyle is Log and in a Complex Mold".ToCharArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(nullableNodeCollection).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(nullableNodeCollection).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nullableNodeCollection).RevealAll(nullableNodeCollection).Complete();
                else
                    return tos.StartComplexCollectionType(nullableNodeCollection).RevealAll(nullableNodeCollection).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAll(itemCollection, itemRevealer, null
                                                                       , isValue ? DefaultCallerTypeFlags : AsStringContent).Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllObject(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllObject(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class ObjectOrArrayClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrArrayClassUnion>? nodeRevealer;

    public ObjectOrArrayClassUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public ObjectOrArrayClassUnion(object?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public ObjectOrArrayClassUnion(ObjectOrArrayClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrArrayClassUnion>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly object? item;
    private readonly object?[]? itemCollection;

    private readonly ObjectOrArrayClassUnion?[]? nodeCollection;

    private static readonly string LogComplexOnlyStaticInstance = "\"This\" is only shown when StringStyle is Log and in a Complex Mold";

    private readonly bool logComplexOnlyInstance = true;

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAll(itemCollection, itemRevealer, null
                                                                       , isValue ? DefaultCallerTypeFlags : AsStringContent).Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllObject(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllObject(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class ObjectOrSpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrSpanClassUnion>? nodeRevealer;

    public ObjectOrSpanClassUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public ObjectOrSpanClassUnion(object?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public ObjectOrSpanClassUnion(ObjectOrSpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrSpanClassUnion>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly object? item;
    private readonly object?[]? itemCollection;

    private readonly ObjectOrSpanClassUnion?[]? nodeCollection;

    private static readonly int? LogComplexOnlyStaticInstance = null;

    private readonly DateTime logComplexOnlyInstance = new(2026, 2, 16, 7, 6, 23);

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection.AsSpan(), nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection.AsSpan(), nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection.AsSpan()).Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection.AsSpan()).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAll(itemCollection.AsSpan(), itemRevealer, null
                                                                       , isValue ? DefaultCallerTypeFlags : AsStringContent).Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection.AsSpan(), itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllObjectNullable(itemCollection.AsSpan()).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllObjectNullable(itemCollection.AsSpan(), null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class ObjectOrReadOnlySpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrReadOnlySpanClassUnion>? nodeRevealer;

    public ObjectOrReadOnlySpanClassUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public ObjectOrReadOnlySpanClassUnion(object?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public ObjectOrReadOnlySpanClassUnion(ObjectOrReadOnlySpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrReadOnlySpanClassUnion>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly object? item;
    private readonly object?[]? itemCollection;

    private readonly ObjectOrReadOnlySpanClassUnion?[]? nodeCollection;

    private static readonly Complex LogComplexOnlyStaticInstance = new(12.24, 23.34);

    private readonly char[] logComplexOnlyInstance = "New String Per Instance".ToCharArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<ObjectOrReadOnlySpanClassUnion?>)nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<ObjectOrReadOnlySpanClassUnion?>)nodeCollection, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<ObjectOrReadOnlySpanClassUnion?>)nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<ObjectOrReadOnlySpanClassUnion?>)nodeCollection)
                          .Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAll((ReadOnlySpan<object?>)itemCollection, itemRevealer, null
                                                                       , isValue ? DefaultCallerTypeFlags : AsStringContent).Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll((ReadOnlySpan<object?>)itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllObjectNullable((ReadOnlySpan<Object?>)itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllObjectNullable((ReadOnlySpan<object?>)itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct ObjectOrListStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrListStructUnion>? nodeRevealer;

    public ObjectOrListStructUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public ObjectOrListStructUnion(List<object?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public ObjectOrListStructUnion(List<ObjectOrListStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrListStructUnion>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    public ObjectOrListStructUnion(List<ObjectOrListStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrListStructUnion>? withNodeRevealer = null)
    {
        isNullableNode         = true;
        isNode                 = true;
        nullableNodeCollection = nodeColl;
        nodeRevealer           = withNodeRevealer;
        isSimple               = asSimple;
        isValue                = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly object? item;
    private readonly List<object?>? itemCollection;

    private readonly List<ObjectOrListStructUnion>?  nodeCollection         = null;
    private readonly List<ObjectOrListStructUnion?>? nullableNodeCollection = null;

    private static readonly IPAddress LogComplexOnlyStaticInstance = IPAddress.Loopback;

    private readonly decimal logComplexOnlyInstance = 2.123m;

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(nullableNodeCollection).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(nullableNodeCollection).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nullableNodeCollection).RevealAll(nullableNodeCollection).Complete();
                else
                    return tos.StartComplexCollectionType(nullableNodeCollection).RevealAll(nullableNodeCollection).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAll(itemCollection, itemRevealer, null
                                                                       , isValue ? DefaultCallerTypeFlags : AsStringContent).Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllObject(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllObject(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class ObjectOrListClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrListClassUnion>? nodeRevealer;

    public ObjectOrListClassUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public ObjectOrListClassUnion(List<object?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public ObjectOrListClassUnion(List<ObjectOrListClassUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrListClassUnion>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly object? item;
    private readonly List<object?>? itemCollection;

    private readonly List<ObjectOrListClassUnion>? nodeCollection;

    private static readonly int[] LogComplexOnlyStaticReadOnlySpanInstance = [1, 2, 3];

    private readonly BigInteger?[] logComplexOnlyInstance =
    [
        new(1.1)
      , new(2.2)
      , null
      , new(3.3)
      , new(4.4)
    ];

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection).RevealAll(nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                              , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                              , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                          , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllObject(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                          , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                          , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllObject(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                  , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct ObjectOrEnumerableStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrEnumerableStructUnion>? nodeRevealer;

    public ObjectOrEnumerableStructUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public ObjectOrEnumerableStructUnion(List<object?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public ObjectOrEnumerableStructUnion(List<ObjectOrEnumerableStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrEnumerableStructUnion>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    public ObjectOrEnumerableStructUnion(List<ObjectOrEnumerableStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrEnumerableStructUnion>? withNodeRevealer = null)
    {
        isNullableNode         = true;
        isNode                 = true;
        nullableNodeCollection = nodeColl;
        nodeRevealer           = withNodeRevealer;
        isSimple               = asSimple;
        isValue                = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly object? item;
    private readonly List<object?>? itemCollection;

    private readonly List<ObjectOrEnumerableStructUnion>?  nodeCollection         = null;
    private readonly List<ObjectOrEnumerableStructUnion?>? nullableNodeCollection = null;

    private static readonly CharArrayStringBuilder?[] LogComplexOnlyStaticInstance =
    [
        null
      , new("2nd entry")
      , new("3rd entry")
      , new("4th entry")
      , null
    ];

    private readonly IPAddress?[] logComplexOnlyInstance =
    [
        new([1, 1, 1, 1])
      , new([2, 2, 2, 2])
      , null
      , new([3, 3, 3, 3])
      , new([4, 4, 4, 4])
    ];

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(nullableNodeCollection)
                                  .RevealAllEnumerateNullable(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                    else
                        return tos.StartComplexCollectionType(nullableNodeCollection)
                                  .RevealAllEnumerateNullable(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection)
                              .RevealAllEnumerate(nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAllEnumerate(nodeCollection, nodeRevealer)
                              .Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nullableNodeCollection)
                              .RevealAllEnumerateNullable(nullableNodeCollection)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nullableNodeCollection)
                              .RevealAllEnumerateNullable(nullableNodeCollection)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAllEnumerate(nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAllEnumerate(nodeCollection)
                          .Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate(itemCollection, itemRevealer, null
                              , isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerate(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllEnumerateObject(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllEnumerateObject(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class ObjectOrEnumerableClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrEnumerableClassUnion>? nodeRevealer;

    public ObjectOrEnumerableClassUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public ObjectOrEnumerableClassUnion(List<object?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public ObjectOrEnumerableClassUnion(List<ObjectOrEnumerableClassUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrEnumerableClassUnion>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly object? item;
    private readonly List<object?>? itemCollection;

    private readonly List<ObjectOrEnumerableClassUnion?>? nodeCollection;

    private static readonly IPAddress?[] LogComplexOnlyStaticSpanInstance =
    [
        IPAddress.Loopback
      , null
      , null
      , new([192, 158, 1, 0])
    ];

    private readonly List<string?> logComplexOnlyInstance =
    [
        null
      , "SecondItem"
      , "ThirdItem"
      , null
      , null
      , "SixthItem"
    ];

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection)
                              .RevealAllEnumerate(nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAllEnumerate(nodeCollection, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAllEnumerate(nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAllEnumerate(nodeCollection)
                          .Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                           .LogOnlyCollectionField.AlwaysAddAllStringEnumerateString(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                           .LogOnlyCollectionField.AlwaysAddAllStringEnumerateString(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerate(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerateString(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllEnumerateObject(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerateString(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerateString(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllEnumerateObject(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
               .LogOnlyCollectionField.AlwaysAddAllStringEnumerateString(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct ObjectOrEnumeratorStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrEnumeratorStructUnion>? nodeRevealer;

    public ObjectOrEnumeratorStructUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public ObjectOrEnumeratorStructUnion(List<object?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<Object?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public ObjectOrEnumeratorStructUnion(List<ObjectOrEnumeratorStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<ObjectOrEnumeratorStructUnion>();
            nodeCollectionEnumerator.DisposeRecycles           = false;
            nodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nodeCollectionEnumerator.ProxiedEnumerator         = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        isItem       = false;
        itemRevealer = null;
    }

    public ObjectOrEnumeratorStructUnion(List<ObjectOrEnumeratorStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator                           = new ReusableWrappingEnumerator<ObjectOrEnumeratorStructUnion?>();
            nullableNodeCollectionEnumerator.DisposeRecycles           = false;
            nullableNodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nullableNodeCollectionEnumerator.ProxiedEnumerator         = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        isItem       = false;
        itemRevealer = null;
    }

    private readonly object? item;

    private readonly ReusableWrappingEnumerator<Object?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<ObjectOrEnumeratorStructUnion>? nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<ObjectOrEnumeratorStructUnion?>?
        nullableNodeCollectionEnumerator = null;

    private static readonly Dictionary<Object, char[]> LogComplexOnlyStaticInstance = new()
    {
        { new MyOtherTypeClass("FirstKey"), "FirstValue".ToCharArray() }
      , { new MyOtherTypeClass("SecondKey"), "SecondValue".ToCharArray() }
      , { new MyOtherTypeClass("ThirdKey"), "ThirdValue".ToCharArray() }
    };

    private readonly List<KeyValuePair<ICharSequence, string?>> logComplexOnlyInstance = new Dictionary<ICharSequence, string?>()
    {
        { new MutableString("FirstKey"), "FirstValue" }
      , { new CharArrayStringBuilder("SecondKey"), "SecondValue" }
      , { new CharArrayStringBuilder("ThirdKey"), null }
    }.ToList();

    public AppendSummary RevealState(ITheOneString tos)
    {
        itemCollectionEnumerator?.Reset();
        nodeCollectionEnumerator?.Reset();
        nullableNodeCollectionEnumerator?.Reset();
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(this).RevealAllIterateNullable(nullableNodeCollectionEnumerator, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAllIterateNullable(nullableNodeCollectionEnumerator, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterateNullable(nullableNodeCollectionEnumerator).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterateNullable(nullableNodeCollectionEnumerator).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate(nodeCollectionEnumerator).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllIterate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllIterateObject(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllIterateObject(itemCollectionEnumerator, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class ObjectOrEnumeratorClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<object>? itemRevealer;

    private readonly PalantírReveal<ObjectOrEnumeratorClassUnion>? nodeRevealer;

    public ObjectOrEnumeratorClassUnion(object? item, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public ObjectOrEnumeratorClassUnion(List<object?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<object>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<Object?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public ObjectOrEnumeratorClassUnion(List<ObjectOrEnumeratorClassUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<ObjectOrEnumeratorClassUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<ObjectOrEnumeratorClassUnion?>();
            nodeCollectionEnumerator.DisposeRecycles           = false;
            nodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nodeCollectionEnumerator.ProxiedEnumerator         = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        isItem       = false;
        itemRevealer = null;
    }

    private readonly object? item;

    private readonly ReusableWrappingEnumerator<Object?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<ObjectOrEnumeratorClassUnion?>? nodeCollectionEnumerator;

    private static readonly Dictionary<IPAddress, Complex?> LogComplexOnlyStaticInstance = new()
    {
        { new([1, 1, 1, 1]), new(1.1, 1.1) }
      , { new([2, 2, 2, 2]), new(2.2, 2.2) }
      , { new([3, 3, 3, 3]), null }
      , { new([4, 4, 4, 4]), new(4.4, 4.4) }
    };

    private readonly KeyValuePair<Complex, decimal?>[] logComplexOnlyInstance = new Dictionary<Complex, decimal?>()
    {
        { new(1.1, 1.1), 1.1m }
      , { new(2.2, 2.2), 2.2m }
      , { new(3.3, 3.3), 3.3m }
      , { new(4.4, 4.4), 4.4m }
    }.ToArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        itemCollectionEnumerator?.Reset();
        nodeCollectionEnumerator?.Reset();
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate(nodeCollectionEnumerator).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllIterate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueMatchOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringMatchOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllIterateObject(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueMatchOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringMatchOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllIterateObject(itemCollectionEnumerator, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}