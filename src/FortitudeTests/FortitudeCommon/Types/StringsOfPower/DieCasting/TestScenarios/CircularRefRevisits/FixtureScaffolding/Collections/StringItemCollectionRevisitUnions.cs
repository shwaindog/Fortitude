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

public readonly struct StringOrArrayStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrArrayStructUnion>? nodeRevealer;

    public StringOrArrayStructUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringOrArrayStructUnion(string?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringOrArrayStructUnion(StringOrArrayStructUnion[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrArrayStructUnion>? withNodeRevealer = null)
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

    public StringOrArrayStructUnion(StringOrArrayStructUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrArrayStructUnion>? withNodeRevealer = null)
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

    private readonly string? item;
    private readonly string?[]? itemCollection;

    private readonly StringOrArrayStructUnion[]?  nodeCollection         = null;
    private readonly StringOrArrayStructUnion?[]? nullableNodeCollection = null;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
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

public class StringOrArrayClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrArrayClassUnion>? nodeRevealer;

    public StringOrArrayClassUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringOrArrayClassUnion(string?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringOrArrayClassUnion(StringOrArrayClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrArrayClassUnion>? withNodeRevealer = null)
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

    private readonly string? item;
    private readonly string?[]? itemCollection;

    private readonly StringOrArrayClassUnion?[]? nodeCollection;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
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

public class StringOrSpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrSpanClassUnion>? nodeRevealer;

    public StringOrSpanClassUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringOrSpanClassUnion(string?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringOrSpanClassUnion(StringOrSpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrSpanClassUnion>? withNodeRevealer = null)
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

    private readonly string? item;
    private readonly string?[]? itemCollection;

    private readonly StringOrSpanClassUnion?[]? nodeCollection;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllNullable(itemCollection.AsSpan()).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
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

public class StringOrReadOnlySpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrReadOnlySpanClassUnion>? nodeRevealer;

    public StringOrReadOnlySpanClassUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringOrReadOnlySpanClassUnion(string?[]? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringOrReadOnlySpanClassUnion(StringOrReadOnlySpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrReadOnlySpanClassUnion>? withNodeRevealer = null)
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

    private readonly string? item;
    private readonly string?[]? itemCollection;

    private readonly StringOrReadOnlySpanClassUnion?[]? nodeCollection;

    private static readonly Complex LogComplexOnlyStaticInstance = new(12.24, 23.34);

    private readonly char[] logComplexOnlyInstance = "New String Per Instance".ToCharArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<StringOrReadOnlySpanClassUnion?>)nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<StringOrReadOnlySpanClassUnion?>)nodeCollection, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<StringOrReadOnlySpanClassUnion?>)nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<StringOrReadOnlySpanClassUnion?>)nodeCollection)
                          .Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAll((ReadOnlySpan<string?>)itemCollection, itemRevealer, null
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
                       .RevealAll((ReadOnlySpan<string?>)itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllNullable((ReadOnlySpan<string?>)itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllNullable((ReadOnlySpan<string?>)itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct StringOrListStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrListStructUnion>? nodeRevealer;

    public StringOrListStructUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringOrListStructUnion(List<string?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringOrListStructUnion(List<StringOrListStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrListStructUnion>? withNodeRevealer = null)
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

    public StringOrListStructUnion(List<StringOrListStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrListStructUnion>? withNodeRevealer = null)
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

    private readonly string? item;
    private readonly List<string?>? itemCollection;

    private readonly List<StringOrListStructUnion>?  nodeCollection         = null;
    private readonly List<StringOrListStructUnion?>? nullableNodeCollection = null;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
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

public class StringOrListClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrListClassUnion>? nodeRevealer;

    public StringOrListClassUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringOrListClassUnion(List<string?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringOrListClassUnion(List<StringOrListClassUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrListClassUnion>? withNodeRevealer = null)
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

    private readonly string? item;
    private readonly List<string?>? itemCollection;

    private readonly List<StringOrListClassUnion>? nodeCollection;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                          , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
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

public readonly struct StringOrEnumerableStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrEnumerableStructUnion>? nodeRevealer;

    public StringOrEnumerableStructUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringOrEnumerableStructUnion(List<string?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringOrEnumerableStructUnion(List<StringOrEnumerableStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrEnumerableStructUnion>? withNodeRevealer = null)
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

    public StringOrEnumerableStructUnion(List<StringOrEnumerableStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrEnumerableStructUnion>? withNodeRevealer = null)
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

    private readonly string? item;
    private readonly List<string?>? itemCollection;

    private readonly List<StringOrEnumerableStructUnion>?  nodeCollection         = null;
    private readonly List<StringOrEnumerableStructUnion?>? nullableNodeCollection = null;

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
                                  List<StringOrEnumerableStructUnion>?
                                , StringOrEnumerableStructUnion
                                , StringOrEnumerableStructUnion
                              >(nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAllEnumerate<
                                  List<StringOrEnumerableStructUnion>?
                                , StringOrEnumerableStructUnion
                                , StringOrEnumerableStructUnion
                              >(nodeCollection, nodeRevealer)
                              .Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nullableNodeCollection)
                              .RevealAllEnumerateNullable<
                                  List<StringOrEnumerableStructUnion?>?
                                , StringOrEnumerableStructUnion
                              >(nullableNodeCollection)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nullableNodeCollection)
                              .RevealAllEnumerateNullable<
                                  List<StringOrEnumerableStructUnion?>?
                                , StringOrEnumerableStructUnion
                              >(nullableNodeCollection)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAllEnumerate<
                              List<StringOrEnumerableStructUnion>?
                            , StringOrEnumerableStructUnion
                          >(nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAllEnumerate<
                              List<StringOrEnumerableStructUnion>?
                            , StringOrEnumerableStructUnion
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
                              .RevealAllEnumerate<List<string?>?, string?, string>(itemCollection, itemRevealer, null
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
                       .RevealAllEnumerate<List<string?>?, string?, string>(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllStringEnumerate(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllStringEnumerate(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringOrEnumerableClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrEnumerableClassUnion>? nodeRevealer;

    public StringOrEnumerableClassUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public StringOrEnumerableClassUnion(List<string?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public StringOrEnumerableClassUnion(List<StringOrEnumerableClassUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrEnumerableClassUnion>? withNodeRevealer = null)
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

    private readonly string? item;
    private readonly List<string?>? itemCollection;

    private readonly List<StringOrEnumerableClassUnion?>? nodeCollection;

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
                                  List<StringOrEnumerableClassUnion?>?
                                , StringOrEnumerableClassUnion?
                                , StringOrEnumerableClassUnion
                              >(nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAllEnumerate<
                                  List<StringOrEnumerableClassUnion?>?
                                , StringOrEnumerableClassUnion?
                                , StringOrEnumerableClassUnion
                              >(nodeCollection, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAllEnumerate<
                              List<StringOrEnumerableClassUnion?>?
                            , StringOrEnumerableClassUnion?
                          >(nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAllEnumerate<
                              List<StringOrEnumerableClassUnion?>?
                            , StringOrEnumerableClassUnion?
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
                              .RevealAllEnumerate<List<string?>?, string?, string>(itemCollection, itemRevealer, null
                              , isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllEnumerate<List<string?>?, string?, string>
                           (itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllStringEnumerate(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllStringEnumerate(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
               .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct StringOrEnumeratorStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrEnumeratorStructUnion>? nodeRevealer;

    public StringOrEnumeratorStructUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringOrEnumeratorStructUnion(List<string?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<string?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringOrEnumeratorStructUnion(List<StringOrEnumeratorStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<StringOrEnumeratorStructUnion>();
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

    public StringOrEnumeratorStructUnion(List<StringOrEnumeratorStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator                           = new ReusableWrappingEnumerator<StringOrEnumeratorStructUnion?>();
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

    private readonly string? item;

    private readonly ReusableWrappingEnumerator<string?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<StringOrEnumeratorStructUnion>? nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<StringOrEnumeratorStructUnion?>?
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
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringOrEnumeratorStructUnion>?
                                , StringOrEnumeratorStructUnion
                                , StringOrEnumeratorStructUnion
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringOrEnumeratorStructUnion>?
                                , StringOrEnumeratorStructUnion
                                , StringOrEnumeratorStructUnion
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterateNullable<
                                  ReusableWrappingEnumerator<StringOrEnumeratorStructUnion?>?
                                , StringOrEnumeratorStructUnion
                              >(nullableNodeCollectionEnumerator).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterateNullable<
                                  ReusableWrappingEnumerator<StringOrEnumeratorStructUnion?>?
                                , StringOrEnumeratorStructUnion
                              >(nullableNodeCollectionEnumerator).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringOrEnumeratorStructUnion>?
                            , StringOrEnumeratorStructUnion
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringOrEnumeratorStructUnion>?
                            , StringOrEnumeratorStructUnion
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
                                  ReusableWrappingEnumerator<string?>?
                                , string?
                                , string
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
                           ReusableWrappingEnumerator<string?>?
                         , string?
                         , string
                       >(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllStringIterate(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllStringIterate(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringOrEnumeratorClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly  PalantírReveal<string>? itemRevealer;

    private readonly PalantírReveal<StringOrEnumeratorClassUnion>? nodeRevealer;

    public StringOrEnumeratorClassUnion(string? item, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringOrEnumeratorClassUnion(List<string?>? itemColl, bool asSimple, bool asValue = true,  PalantírReveal<string>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<string?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringOrEnumeratorClassUnion(List<StringOrEnumeratorClassUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<StringOrEnumeratorClassUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<StringOrEnumeratorClassUnion?>();
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

    private readonly string? item;

    private readonly ReusableWrappingEnumerator<string?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<StringOrEnumeratorClassUnion?>? nodeCollectionEnumerator;

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
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringOrEnumeratorClassUnion?>?
                                , StringOrEnumeratorClassUnion?
                                , StringOrEnumeratorClassUnion
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringOrEnumeratorClassUnion?>?
                                , StringOrEnumeratorClassUnion?
                                , StringOrEnumeratorClassUnion
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringOrEnumeratorClassUnion?>?
                            , StringOrEnumeratorClassUnion?
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringOrEnumeratorClassUnion?>?
                            , StringOrEnumeratorClassUnion?
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
                                  ReusableWrappingEnumerator<string?>?
                                , string?
                                , string
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
                           ReusableWrappingEnumerator<string?>?
                         , string?
                         , string
                       >(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValueOrNull(item, 0).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item, 0).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllStringIterate(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item, 0)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item, 0)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllStringIterate(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}