// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public readonly struct StringBuilderOrArrayStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrArrayStructUnion>? nodeRevealer;

    public StringBuilderOrArrayStructUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringBuilderOrArrayStructUnion(StringBuilder?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringBuilderOrArrayStructUnion(StringBuilderOrArrayStructUnion[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrArrayStructUnion>? withNodeRevealer = null)
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

    public StringBuilderOrArrayStructUnion(StringBuilderOrArrayStructUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrArrayStructUnion>? withNodeRevealer = null)
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

    private readonly StringBuilder? item;
    private readonly StringBuilder?[]? itemCollection;

    private readonly StringBuilderOrArrayStructUnion[]?  nodeCollection         = null;
    private readonly StringBuilderOrArrayStructUnion?[]? nullableNodeCollection = null;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringBuilderOrArrayClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrArrayClassUnion>? nodeRevealer;

    public StringBuilderOrArrayClassUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringBuilderOrArrayClassUnion(StringBuilder?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringBuilderOrArrayClassUnion(StringBuilderOrArrayClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrArrayClassUnion>? withNodeRevealer = null)
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

    private readonly StringBuilder? item;
    private readonly StringBuilder?[]? itemCollection;

    private readonly StringBuilderOrArrayClassUnion?[]? nodeCollection;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringBuilderOrSpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrSpanClassUnion>? nodeRevealer;

    public StringBuilderOrSpanClassUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringBuilderOrSpanClassUnion(StringBuilder?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringBuilderOrSpanClassUnion(StringBuilderOrSpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrSpanClassUnion>? withNodeRevealer = null)
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

    private readonly StringBuilder? item;
    private readonly StringBuilder?[]? itemCollection;

    private readonly StringBuilderOrSpanClassUnion?[]? nodeCollection;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllNullable(itemCollection.AsSpan()).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllNullable(itemCollection.AsSpan(), null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringBuilderOrReadOnlySpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrReadOnlySpanClassUnion>? nodeRevealer;

    public StringBuilderOrReadOnlySpanClassUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringBuilderOrReadOnlySpanClassUnion(StringBuilder?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringBuilderOrReadOnlySpanClassUnion(StringBuilderOrReadOnlySpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrReadOnlySpanClassUnion>? withNodeRevealer = null)
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

    private readonly StringBuilder? item;
    private readonly StringBuilder?[]? itemCollection;

    private readonly StringBuilderOrReadOnlySpanClassUnion?[]? nodeCollection;

    private static readonly Complex LogComplexOnlyStaticInstance = new(12.24, 23.34);

    private readonly char[] logComplexOnlyInstance = "New String Per Instance".ToCharArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion?>)nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion?>)nodeCollection, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion?>)nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<StringBuilderOrReadOnlySpanClassUnion?>)nodeCollection)
                          .Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAll((ReadOnlySpan<StringBuilder?>)itemCollection, itemRevealer, null
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
                       .RevealAll((ReadOnlySpan<StringBuilder?>)itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllNullable((ReadOnlySpan<StringBuilder?>)itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllNullable((ReadOnlySpan<StringBuilder?>)itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct StringBuilderOrListStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrListStructUnion>? nodeRevealer;

    public StringBuilderOrListStructUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringBuilderOrListStructUnion(List<StringBuilder?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringBuilderOrListStructUnion(List<StringBuilderOrListStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrListStructUnion>? withNodeRevealer = null)
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

    public StringBuilderOrListStructUnion(List<StringBuilderOrListStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrListStructUnion>? withNodeRevealer = null)
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

    private readonly StringBuilder? item;
    private readonly List<StringBuilder?>? itemCollection;

    private readonly List<StringBuilderOrListStructUnion>?  nodeCollection         = null;
    private readonly List<StringBuilderOrListStructUnion?>? nullableNodeCollection = null;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class StringBuilderOrListClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrListClassUnion>? nodeRevealer;

    public StringBuilderOrListClassUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringBuilderOrListClassUnion(List<StringBuilder?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringBuilderOrListClassUnion(List<StringBuilderOrListClassUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrListClassUnion>? withNodeRevealer = null)
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

    private readonly StringBuilder? item;
    private readonly List<StringBuilder?>? itemCollection;

    private readonly List<StringBuilderOrListClassUnion>? nodeCollection;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                          , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                          , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                  , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct StringBuilderOrEnumerableStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrEnumerableStructUnion>? nodeRevealer;

    public StringBuilderOrEnumerableStructUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringBuilderOrEnumerableStructUnion(List<StringBuilder?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringBuilderOrEnumerableStructUnion(List<StringBuilderOrEnumerableStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrEnumerableStructUnion>? withNodeRevealer = null)
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

    public StringBuilderOrEnumerableStructUnion(List<StringBuilderOrEnumerableStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrEnumerableStructUnion>? withNodeRevealer = null)
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

    private readonly StringBuilder? item;
    private readonly List<StringBuilder?>? itemCollection;

    private readonly List<StringBuilderOrEnumerableStructUnion>?  nodeCollection         = null;
    private readonly List<StringBuilderOrEnumerableStructUnion?>? nullableNodeCollection = null;

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
                              .RevealAllEnumerate<
                                  List<StringBuilderOrEnumerableStructUnion>?
                                , StringBuilderOrEnumerableStructUnion 
                                , StringBuilderOrEnumerableStructUnion 
                              >(nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAllEnumerate<
                                  List<StringBuilderOrEnumerableStructUnion>?
                                , StringBuilderOrEnumerableStructUnion 
                                , StringBuilderOrEnumerableStructUnion 
                              >(nodeCollection, nodeRevealer)
                              .Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nullableNodeCollection)
                              .RevealAllEnumerateNullable<
                                  List<StringBuilderOrEnumerableStructUnion?>?
                                , StringBuilderOrEnumerableStructUnion 
                              >(nullableNodeCollection)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nullableNodeCollection)
                              .RevealAllEnumerateNullable<
                                  List<StringBuilderOrEnumerableStructUnion?>?
                                , StringBuilderOrEnumerableStructUnion 
                              >(nullableNodeCollection)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAllEnumerate<
                              List<StringBuilderOrEnumerableStructUnion>?
                            , StringBuilderOrEnumerableStructUnion 
                          >(nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAllEnumerate<
                              List<StringBuilderOrEnumerableStructUnion>?
                            , StringBuilderOrEnumerableStructUnion 
                          >(nodeCollection)
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
                              .RevealAllEnumerate<
                                  List<StringBuilder?>?
                                , StringBuilder? 
                                , StringBuilder 
                              >(itemCollection, itemRevealer, null
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
                       .RevealAllEnumerate<
                           List<StringBuilder?>?
                         , StringBuilder? 
                         , StringBuilder 
                       >(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllStringBuilderEnumerate(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllStringBuilderEnumerate(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringBuilderOrEnumerableClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrEnumerableClassUnion>? nodeRevealer;

    public StringBuilderOrEnumerableClassUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringBuilderOrEnumerableClassUnion(List<StringBuilder?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringBuilderOrEnumerableClassUnion(List<StringBuilderOrEnumerableClassUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrEnumerableClassUnion>? withNodeRevealer = null)
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

    private readonly StringBuilder? item;
    private readonly List<StringBuilder?>? itemCollection;

    private readonly List<StringBuilderOrEnumerableClassUnion?>? nodeCollection;

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
                              .RevealAllEnumerate<
                                  List<StringBuilderOrEnumerableClassUnion?>?
                                , StringBuilderOrEnumerableClassUnion? 
                                , StringBuilderOrEnumerableClassUnion 
                              >(nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAllEnumerate<
                                  List<StringBuilderOrEnumerableClassUnion?>?
                                , StringBuilderOrEnumerableClassUnion? 
                                , StringBuilderOrEnumerableClassUnion 
                              >(nodeCollection, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAllEnumerate<
                              List<StringBuilderOrEnumerableClassUnion?>?
                            , StringBuilderOrEnumerableClassUnion? 
                          >(nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAllEnumerate<
                              List<StringBuilderOrEnumerableClassUnion?>?
                            , StringBuilderOrEnumerableClassUnion? 
                          >(nodeCollection)
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
                              .RevealAllEnumerate<
                                  List<StringBuilder?>?
                                , StringBuilder?
                                , StringBuilder
                              >(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                           .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                           .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerate<
                           List<StringBuilder?>?
                         , StringBuilder?
                         , StringBuilder
                       >(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllStringBuilderEnumerate(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllStringBuilderEnumerate(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
               .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct StringBuilderOrEnumeratorStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrEnumeratorStructUnion>? nodeRevealer;

    public StringBuilderOrEnumeratorStructUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringBuilderOrEnumeratorStructUnion(List<StringBuilder?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<StringBuilder?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringBuilderOrEnumeratorStructUnion(List<StringBuilderOrEnumeratorStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion>();
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

    public StringBuilderOrEnumeratorStructUnion(List<StringBuilderOrEnumeratorStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator                           = new ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion?>();
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

    private readonly StringBuilder? item;

    private readonly ReusableWrappingEnumerator<StringBuilder?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion>? nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion?>?
        nullableNodeCollectionEnumerator = null;

    private static readonly Dictionary<StringBuilder, char[]> LogComplexOnlyStaticInstance = new()
    {
        { new StringBuilder("FirstKey"), "FirstValue".ToCharArray() }
      , { new StringBuilder("SecondKey"), "SecondValue".ToCharArray() }
      , { new StringBuilder("ThirdKey"), "ThirdValue".ToCharArray() }
    };

    private readonly List<KeyValuePair<ICharSequence, string?>> logComplexOnlyInstance = new Dictionary<ICharSequence, string?>()
    {
        { new MutableString("FirstKey"), "FirstValue" }
      , { new CharArrayStringBuilder("SecondKey"), "SecondValue" }
      , { new CharArrayStringBuilder("ThirdKey"), null }
    }.ToList();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(this).RevealAllIterateNullable(nullableNodeCollectionEnumerator, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAllIterateNullable(nullableNodeCollectionEnumerator, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion>?
                                , StringBuilderOrEnumeratorStructUnion
                                , StringBuilderOrEnumeratorStructUnion
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion>?
                                , StringBuilderOrEnumeratorStructUnion
                                , StringBuilderOrEnumeratorStructUnion
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterateNullable<
                                  ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion?>?
                                , StringBuilderOrEnumeratorStructUnion
                              >(nullableNodeCollectionEnumerator).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterateNullable<
                                  ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion?>?
                                , StringBuilderOrEnumeratorStructUnion
                              >(nullableNodeCollectionEnumerator).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion>?
                            , StringBuilderOrEnumeratorStructUnion
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringBuilderOrEnumeratorStructUnion>?
                            , StringBuilderOrEnumeratorStructUnion
                          >(nodeCollectionEnumerator).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBuilder?>?
                                , StringBuilder
                                , StringBuilder
                              >(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllIterate<
                           ReusableWrappingEnumerator<StringBuilder?>?
                         , StringBuilder
                         , StringBuilder
                       >(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllStringBuilderIterate(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllStringBuilderIterate(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringBuilderOrEnumeratorClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<StringBuilder>? itemRevealer;

    private readonly PalantírReveal<StringBuilderOrEnumeratorClassUnion>? nodeRevealer;

    public StringBuilderOrEnumeratorClassUnion(StringBuilder? item, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringBuilderOrEnumeratorClassUnion(List<StringBuilder?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<StringBuilder>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<StringBuilder?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringBuilderOrEnumeratorClassUnion(List<StringBuilderOrEnumeratorClassUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringBuilderOrEnumeratorClassUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<StringBuilderOrEnumeratorClassUnion?>();
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

    private readonly StringBuilder? item;

    private readonly ReusableWrappingEnumerator<StringBuilder?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<StringBuilderOrEnumeratorClassUnion?>? nodeCollectionEnumerator;

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
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBuilderOrEnumeratorClassUnion?>?
                                , StringBuilderOrEnumeratorClassUnion?
                                , StringBuilderOrEnumeratorClassUnion
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBuilderOrEnumeratorClassUnion?>?
                                , StringBuilderOrEnumeratorClassUnion?
                                , StringBuilderOrEnumeratorClassUnion
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringBuilderOrEnumeratorClassUnion?>?
                            , StringBuilderOrEnumeratorClassUnion?
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringBuilderOrEnumeratorClassUnion?>?
                            , StringBuilderOrEnumeratorClassUnion?
                          >(nodeCollectionEnumerator).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBuilder?>?
                                , StringBuilder?
                                , StringBuilder
                              >(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllIterate<
                           ReusableWrappingEnumerator<StringBuilder?>?
                         , StringBuilder?
                         , StringBuilder
                       >(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllStringBuilderIterate(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllStringBuilderIterate(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}