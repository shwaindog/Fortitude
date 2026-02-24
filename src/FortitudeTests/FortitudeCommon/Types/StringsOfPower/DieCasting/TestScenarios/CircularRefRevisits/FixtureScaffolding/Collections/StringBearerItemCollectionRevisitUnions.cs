// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.Collections;

public readonly struct StringBearerOrArrayStructUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrArrayStructUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrArrayStructUnion(TBearer item, bool asSimple, bool asValue = true, PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrArrayStructUnion(TBearer[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrArrayStructUnion(StringBearerOrArrayStructUnion<TBearer, TBearerRevealer>[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<StringBearerOrArrayStructUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    public StringBearerOrArrayStructUnion(StringBearerOrArrayStructUnion<TBearer, TBearerRevealer>?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<StringBearerOrArrayStructUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNullableNode         = true;
        isNode                 = true;
        nullableNodeCollection = nodeColl;
        nodeRevealer           = withNodeRevealer;
        isSimple               = asSimple;
        isValue                = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly TBearer    item;
    private readonly TBearer[]? itemCollection;

    private readonly StringBearerOrArrayStructUnion<TBearer, TBearerRevealer>[]?  nodeCollection         = null;
    private readonly StringBearerOrArrayStructUnion<TBearer, TBearerRevealer>?[]? nullableNodeCollection = null;

    private static readonly char[] LogComplexOnlyStaticInstance
        = "\"This\" is only shown when StringStyle is Log and in a Complex Mold".ToCharArray();

    private readonly char[] logComplexOnlyInstance = "\"This\" is only shown when StringStyle is Log and in a Complex Mold".ToCharArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(this).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nullableNodeCollection).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nullableNodeCollection).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection).Complete();
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
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct NullableStructStringBearerOrArrayStructUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrArrayStructUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrArrayStructUnion(TBearerStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrArrayStructUnion(TBearerStruct?[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrArrayStructUnion(NullableStructStringBearerOrArrayStructUnion<TBearerStruct>[]? nodeColl
      , bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructStringBearerOrArrayStructUnion<TBearerStruct>>? withNodeRevealer = null)
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

    public NullableStructStringBearerOrArrayStructUnion(NullableStructStringBearerOrArrayStructUnion<TBearerStruct>?[]? nodeColl
      , bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructStringBearerOrArrayStructUnion<TBearerStruct>>? withNodeRevealer = null)
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

    private readonly TBearerStruct?    item;
    private readonly TBearerStruct?[]? itemCollection;

    private readonly NullableStructStringBearerOrArrayStructUnion<TBearerStruct>[]?  nodeCollection         = null;
    private readonly NullableStructStringBearerOrArrayStructUnion<TBearerStruct>?[]? nullableNodeCollection = null;

    private static readonly MutableString LogComplexOnlyStaticInstance = new("\"This\" is only shown when StringStyle is Log and in a Complex Mold");

    private readonly CharArrayStringBuilder logComplexOnlyInstance = new("\"This\" is only shown when StringStyle is Log and in a Complex Mold");

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(this).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nullableNodeCollection).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nullableNodeCollection).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection).Complete();
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
                           .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class StringBearerOrArrayClassUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrArrayClassUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrArrayClassUnion(TBearer item, bool asSimple, bool asValue = true, PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrArrayClassUnion(TBearer[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrArrayClassUnion(StringBearerOrArrayClassUnion<TBearer, TBearerRevealer>?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<StringBearerOrArrayClassUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly TBearer    item;
    private readonly TBearer[]? itemCollection;

    private readonly StringBearerOrArrayClassUnion<TBearer, TBearerRevealer>?[]? nodeCollection;

    private static readonly string LogComplexOnlyStaticInstance = "\"This\" is only shown when StringStyle is Log and in a Complex Mold";

    private readonly bool logComplexOnlyInstance = true;

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection).Complete();
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
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class NullableStructStringBearerOrArrayClassUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrArrayClassUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrArrayClassUnion(TBearerStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrArrayClassUnion(TBearerStruct?[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrArrayClassUnion(NullableStructStringBearerOrArrayClassUnion<TBearerStruct>?[]? nodeColl
      , bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructStringBearerOrArrayClassUnion<TBearerStruct>>? withNodeRevealer = null)
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

    private readonly TBearerStruct?    item;
    private readonly TBearerStruct?[]? itemCollection;

    private readonly NullableStructStringBearerOrArrayClassUnion<TBearerStruct>?[]? nodeCollection;

    private static readonly int LogComplexOnlyStaticInstance = 7;

    private readonly bool? logComplexOnlyInstance = null;

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection).Complete();
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
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class StringBearerOrSpanClassUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrSpanClassUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrSpanClassUnion(TBearer item, bool asSimple, bool asValue = true, PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrSpanClassUnion(TBearer[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrSpanClassUnion(StringBearerOrSpanClassUnion<TBearer, TBearerRevealer>?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<StringBearerOrSpanClassUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly TBearer    item;
    private readonly TBearer[]? itemCollection;

    private readonly StringBearerOrSpanClassUnion<TBearer, TBearerRevealer>?[]? nodeCollection;

    private static readonly int? LogComplexOnlyStaticInstance = null;

    private readonly DateTime logComplexOnlyInstance = new(2026, 2, 16, 7, 6, 23);

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection.AsSpan(), nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection.AsSpan(), nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection.AsSpan()).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection.AsSpan()).Complete();
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
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection.AsSpan()).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection.AsSpan(), null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class NullableStructStringBearerOrSpanClassUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNode;
    private readonly bool isSimple;
    private readonly bool isItem;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrSpanClassUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrSpanClassUnion(TBearerStruct? item, bool asSimple, PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrSpanClassUnion(TBearerStruct?[]? itemColl, bool asSimple, PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrSpanClassUnion(NullableStructStringBearerOrSpanClassUnion<TBearerStruct>[]? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrSpanClassUnion<TBearerStruct>>? withNodeRevealer = null)
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

    private readonly TBearerStruct?    item;
    private readonly TBearerStruct?[]? itemCollection;

    private readonly NullableStructStringBearerOrSpanClassUnion<TBearerStruct>[]? nodeCollection;

    private static readonly int? LogComplexOnlyStaticInstance = 7;

    private readonly decimal? logComplexOnlyInstance = null;

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection.AsSpan(), nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection.AsSpan(), nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection.AsSpan()).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection.AsSpan()).Complete();
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
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection.AsSpan()).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection.AsSpan(), null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrReadOnlySpanClassUnion(TBearer item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrReadOnlySpanClassUnion(TBearer[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrReadOnlySpanClassUnion(StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer>?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly TBearer    item;
    private readonly TBearer[]? itemCollection;

    private readonly StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer>?[]? nodeCollection;

    private static readonly Complex LogComplexOnlyStaticInstance = new(12.24, 23.34);

    private readonly char[] logComplexOnlyInstance = "New String Per Instance".ToCharArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAll((ReadOnlySpan<StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer>?>)nodeCollection
                                       , nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAll((ReadOnlySpan<StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer>?>)nodeCollection
                                       , nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAll((ReadOnlySpan<StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer>?>)nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAll((ReadOnlySpan<StringBearerOrReadOnlySpanClassUnion<TBearer, TBearerRevealer>?>)nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAll((ReadOnlySpan<TBearer>)itemCollection, itemRevealer, null
                                       , isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
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
                       .RevealAll((ReadOnlySpan<TBearer>)itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll((ReadOnlySpan<TBearer>)itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll((ReadOnlySpan<TBearer>)itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNode;
    private readonly bool isSimple;
    private readonly bool isItem;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrReadOnlySpanClassUnion(TBearerStruct? item, bool asSimple
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrReadOnlySpanClassUnion(TBearerStruct?[]? itemColl, bool asSimple
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrReadOnlySpanClassUnion(
        NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct>?[]? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct>>? withNodeRevealer = null)
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

    private readonly TBearerStruct?    item;
    private readonly TBearerStruct?[]? itemCollection;

    private readonly NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct>?[]? nodeCollection;

    private static readonly Complex? LogComplexOnlyStaticInstance = new(12.24, 23.34);

    private readonly string logComplexOnlyInstance = new("New String Per Instance");

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return
                        tos
                            .StartSimpleCollectionType(this)
                            .RevealAll((ReadOnlySpan<NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct>?>)nodeCollection
                                     , nodeRevealer)
                            .Complete();
                else
                    return
                        tos
                            .StartComplexCollectionType(this)
                            .RevealAll((ReadOnlySpan<NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct>?>)nodeCollection
                                     , nodeRevealer)
                            .Complete();
            else if (isSimple)
                return
                    tos
                        .StartSimpleCollectionType(this)
                        .RevealAll((ReadOnlySpan<NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct>?>)nodeCollection)
                        .Complete();
            else
                return
                    tos
                        .StartComplexCollectionType(this)
                        .RevealAll((ReadOnlySpan<NullableStructStringBearerOrReadOnlySpanClassUnion<TBearerStruct>?>)nodeCollection.AsSpan())
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
                              .RevealAll((ReadOnlySpan<TBearerStruct?>)itemCollection, itemRevealer, null
                                       , isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return
                        tos
                            .StartComplexContentType(this)
                            .RevealAsValue(nameof(item), item, itemRevealer)
                            .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                            .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                            .Complete();
                else
                    return
                        tos
                            .StartComplexContentType(this)
                            .RevealAsString(nameof(item), item, itemRevealer)
                            .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                            .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                            .Complete();
            else
                return
                    tos
                        .StartComplexCollectionType(this)
                        .RevealAll((ReadOnlySpan<TBearerStruct?>)itemCollection, itemRevealer, null
                                 , isValue ? DefaultCallerTypeFlags : AsStringContent)
                        .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                        .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                        .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll((ReadOnlySpan<TBearerStruct?>)itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll((ReadOnlySpan<TBearerStruct?>)itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct StringBearerOrListStructUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNullableNode = false;

    private readonly bool isNode   = false;
    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrListStructUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrListStructUnion(TBearer item, bool asSimple, bool asValue = true, PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrListStructUnion(List<TBearer>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrListStructUnion(List<StringBearerOrListStructUnion<TBearer, TBearerRevealer>>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<StringBearerOrListStructUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    public StringBearerOrListStructUnion(List<StringBearerOrListStructUnion<TBearer, TBearerRevealer>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<StringBearerOrListStructUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNullableNode         = true;
        isNode                 = true;
        nullableNodeCollection = nodeColl;
        nodeRevealer           = withNodeRevealer;
        isSimple               = asSimple;
        isValue                = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly TBearer        item;
    private readonly List<TBearer>? itemCollection;

    private readonly List<StringBearerOrListStructUnion<TBearer, TBearerRevealer>>?  nodeCollection         = null;
    private readonly List<StringBearerOrListStructUnion<TBearer, TBearerRevealer>?>? nullableNodeCollection = null;

    private static readonly IPAddress LogComplexOnlyStaticInstance = IPAddress.Loopback;

    private readonly decimal logComplexOnlyInstance = 2.123m;

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(this).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nullableNodeCollection).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nullableNodeCollection).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection).Complete();
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
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct NullableStructStringBearerOrListStructUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrListStructUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrListStructUnion(TBearerStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrListStructUnion(List<TBearerStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrListStructUnion(List<NullableStructStringBearerOrListStructUnion<TBearerStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrListStructUnion<TBearerStruct>>? withNodeRevealer = null)
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

    public NullableStructStringBearerOrListStructUnion(List<NullableStructStringBearerOrListStructUnion<TBearerStruct>?>? nodeColl
      , bool asSimple, bool asValue = true, PalantírReveal<NullableStructStringBearerOrListStructUnion<TBearerStruct>>? withNodeRevealer = null)
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

    private readonly TBearerStruct?        item;
    private readonly List<TBearerStruct?>? itemCollection;

    private readonly List<NullableStructStringBearerOrListStructUnion<TBearerStruct>>?  nodeCollection         = null;
    private readonly List<NullableStructStringBearerOrListStructUnion<TBearerStruct>?>? nullableNodeCollection = null;

    private static readonly IPAddress? LogComplexOnlyStaticInstance = null;

    private readonly BigInteger? logComplexOnlyInstance = null;

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(this).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAll(nullableNodeCollection, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nullableNodeCollection).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nullableNodeCollection).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection).Complete();
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
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class StringBearerOrListClassUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrListClassUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrListClassUnion(TBearer item, bool asSimple, bool asValue = true, PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrListClassUnion(List<TBearer>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrListClassUnion(List<StringBearerOrListClassUnion<TBearer, TBearerRevealer>>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<StringBearerOrListClassUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly TBearer        item;
    private readonly List<TBearer>? itemCollection;

    private readonly List<StringBearerOrListClassUnion<TBearer, TBearerRevealer>>? nodeCollection;

    private static readonly int[] LogComplexOnlyStaticInstance = [1, 2, 3];

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
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection).Complete();
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
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class NullableStructStringBearerOrListClassUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrListClassUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrListClassUnion(TBearerStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrListClassUnion(List<TBearerStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrListClassUnion(List<NullableStructStringBearerOrListClassUnion<TBearerStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrListClassUnion<TBearerStruct>>? withNodeRevealer = null)
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

    private readonly TBearerStruct?        item;
    private readonly List<TBearerStruct?>? itemCollection;

    private readonly List<NullableStructStringBearerOrListClassUnion<TBearerStruct>>? nodeCollection;

    private static readonly int?[] LogComplexOnlyStaticInstance = [null, 1, 2, 3, null];

    private readonly Complex?[] logComplexOnlyInstance =
    [
        new(1.1, 1.1)
      , new(2.2, 2.2)
      , null
      , new(3.3, 3.3)
      , new(4.4, 4.4)
    ];

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAll(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAll(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAll(nodeCollection).Complete();
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
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).RevealAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNullableNode = false;

    private readonly bool isNode   = false;
    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrEnumerableStructUnion(TBearer item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrEnumerableStructUnion(List<TBearer>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrEnumerableStructUnion(List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    public StringBearerOrEnumerableStructUnion(List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNullableNode         = true;
        isNode                 = true;
        nullableNodeCollection = nodeColl;
        nodeRevealer           = withNodeRevealer;
        isSimple               = asSimple;
        isValue                = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly TBearer        item;
    private readonly List<TBearer>? itemCollection;

    private readonly List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>?  nodeCollection         = null;
    private readonly List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>?>? nullableNodeCollection = null;

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
                        return tos.StartSimpleCollectionType(this)
                                  .RevealAllEnumerateNullable(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                    else
                        return tos.StartComplexCollectionType(this)
                                  .RevealAllEnumerateNullable(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate<
                                  List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>?
                                , StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>
                                , StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>
                              >(nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos
                           .StartComplexCollectionType(this)
                           .RevealAllEnumerate<
                               List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>?
                             , StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>
                             , StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>
                           >(nodeCollection, nodeRevealer)
                           .Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos
                           .StartSimpleCollectionType(this)
                           .RevealAllEnumerateNullable<
                               List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>?>?
                             , StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>
                           >(nullableNodeCollection)
                           .Complete();
                else
                    return tos
                           .StartComplexCollectionType(this)
                           .RevealAllEnumerateNullable<
                               List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>?>?
                             , StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>
                           >(nullableNodeCollection)
                           .Complete();
            else if (isSimple)
                return tos
                       .StartSimpleCollectionType(this)
                       .RevealAllEnumerate<
                           List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>?
                         , StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>
                       >(nodeCollection)
                       .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerate<
                           List<StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>>?
                         , StringBearerOrEnumerableStructUnion<TBearer, TBearerRevealer>
                       >(nodeCollection)
                       .Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos
                               .StartSimpleContentType(this)
                               .RevealAsValue(item, itemRevealer)
                               .Complete();
                    else
                        return tos
                               .StartSimpleContentType(this)
                               .RevealAsString(item, itemRevealer)
                               .Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate<
                                  List<TBearer>?
                                , TBearer
                                , TBearerRevealer
                              >(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                           List<TBearer>?
                         , TBearer
                         , TBearerRevealer
                       >(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerate<List<TBearer>?, TBearer>(itemCollection)
                          .Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAllEnumerate<List<TBearer>?, TBearer>(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrEnumerableStructUnion(TBearerStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrEnumerableStructUnion(List<TBearerStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrEnumerableStructUnion(
        List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>? withNodeRevealer = null)
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

    public NullableStructStringBearerOrEnumerableStructUnion(
        List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>?>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>? withNodeRevealer = null)
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

    private readonly TBearerStruct?        item;
    private readonly List<TBearerStruct?>? itemCollection;

    private readonly List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>?  nodeCollection         = null;
    private readonly List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>?>? nullableNodeCollection = null;

    private static readonly string?[] LogComplexOnlyStaticInstance =
    [
        null
      , "2nd entry"
      , "3rd entry"
      , "4th entry"
      , null
    ];

    private readonly CharArrayStringBuilder[] logComplexOnlyInstance =
    [
        new("1st entry")
      , new("2nd entry")
      , new("3rd entry")
      , new("4th entry")
    ];

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(this).RevealAllEnumerateNullable(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAllEnumerateNullable(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate<
                                  List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>?
                                , NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>
                                , NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>
                              >(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllEnumerate<
                                  List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>?
                                , NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>
                                , NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>
                              >(nodeCollection, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerateNullable<
                                  List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>?>?
                                , NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>(nullableNodeCollection)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllEnumerateNullable<
                           List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>?>?
                      , NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>(nullableNodeCollection).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerate<
                              List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>?
                            , NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllEnumerate<
                              List<NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>?
                            , NullableStructStringBearerOrEnumerableStructUnion<TBearerStruct>>(nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerateNullable(itemCollection, itemRevealer, null
                                                        , isValue ? DefaultCallerTypeFlags : AsStringContent).Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerateNullable(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerateNullable<List<TBearerStruct?>?, TBearerStruct>(itemCollection)
                          .Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAllEnumerateNullable<List<TBearerStruct?>?, TBearerStruct>(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrEnumerableClassUnion(TBearer item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrEnumerableClassUnion(List<TBearer>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public StringBearerOrEnumerableClassUnion(List<StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode         = true;
        nodeCollection = nodeColl;
        nodeRevealer   = withNodeRevealer;
        isSimple       = asSimple;
        isValue        = asValue;

        item           = default!;
        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly TBearer        item;
    private readonly List<TBearer>? itemCollection;

    private readonly List<StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>>? nodeCollection;

    private static readonly List<IPAddress?> LogComplexOnlyStaticInstance =
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
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate<
                                  List<StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>>?
                                , StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>
                                , StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>
                              >(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllEnumerate<
                                  List<StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>>?
                                , StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>
                                , StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>
                              >(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerate<
                              List<StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>>?
                            , StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>
                          >(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllEnumerate<
                              List<StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>>?
                            , StringBearerOrEnumerableClassUnion<TBearer, TBearerRevealer>
                          >(nodeCollection).Complete();
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
                                  List<TBearer>?
                                , TBearer
                                , TBearerRevealer
                              >(itemCollection, itemRevealer, null
                              , isValue ? DefaultCallerTypeFlags : AsStringContent).Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerate<
                           List<TBearer>?
                         , TBearer
                         , TBearerRevealer
                       >(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerate<List<TBearer>?, TBearer>(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAllEnumerate<List<TBearer>?, TBearer>(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrEnumerableClassUnion(TBearerStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrEnumerableClassUnion(List<TBearerStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructStringBearerOrEnumerableClassUnion(
        List<NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>>? withNodeRevealer = null)
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

    private readonly TBearerStruct?        item;
    private readonly List<TBearerStruct?>? itemCollection;

    private readonly List<NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>>? nodeCollection;

    private static readonly List<Complex?> LogComplexOnlyStaticInstance =
    [
        new(1.0, 1.1111)
      , new(2.0, 2.2222)
      , null
      , new(4.0, 4.4444)
    ];

    private readonly ConcurrentDictionary<string, Complex?> logComplexOnlyInstance =
        new(
            new Dictionary<string, Complex?>()
            {
                { "FirstKey", new Complex(1.1, 1.1) }
              , { "SecondKey", new Complex(2.2, 2.2) }
              , { "ThirdKey", null }
            });

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate<
                                  List<NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>>?
                                , NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>
                                , NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>
                              >(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllEnumerate<
                                  List<NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>>?
                                , NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>
                                , NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>
                              >(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerate<
                              List<NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>>?
                            , NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>
                          >(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllEnumerate<
                              List<NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>>?
                            , NullableStructStringBearerOrEnumerableClassUnion<TBearerStruct>
                          >(nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this)
                                  .RevealAsValue(item, itemRevealer)
                                  .Complete();
                    else
                        return tos.StartSimpleContentType(this)
                                  .RevealAsString(item, itemRevealer)
                                  .Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerateNullable(itemCollection, itemRevealer, null
                                                        , isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerateNullable(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerateNullable<List<TBearerStruct?>?, TBearerStruct>(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAllEnumerateNullable<List<TBearerStruct?>?, TBearerStruct>(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrEnumeratorStructUnion(TBearer item, bool asSimple, bool asValue = true, PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringBearerOrEnumeratorStructUnion(List<TBearer>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem = false;
        item   = default!;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<TBearer>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        item         = default!;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringBearerOrEnumeratorStructUnion(
        List<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator
                = new ReusableWrappingEnumerator<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>();
            nodeCollectionEnumerator.DisposeRecycles           = false;
            nodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nodeCollectionEnumerator.ProxiedEnumerator         = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        item         = default!;
        isItem       = false;
        itemRevealer = null;
    }

    public StringBearerOrEnumeratorStructUnion(
        List<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator
                = new ReusableWrappingEnumerator<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>?>();
            nullableNodeCollectionEnumerator.DisposeRecycles           = false;
            nullableNodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nullableNodeCollectionEnumerator.ProxiedEnumerator         = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        item         = default!;
        isItem       = false;
        itemRevealer = null;
    }

    private readonly TBearer item;

    private readonly ReusableWrappingEnumerator<TBearer>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>?
        nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>?>?
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
                        return tos.StartSimpleCollectionType(this)
                                  .RevealAllIterateNullable(nullableNodeCollectionEnumerator, nodeRevealer)
                                  .Complete();
                    else
                        return tos.StartComplexCollectionType(this)
                                  .RevealAllIterateNullable(nullableNodeCollectionEnumerator, nodeRevealer)
                                  .Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>?
                                , StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>
                                , StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>?
                                , StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>
                                , StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>?
                            , StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>
                          >(nodeCollectionEnumerator)
                          .Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>>?
                            , StringBearerOrEnumeratorStructUnion<TBearer, TBearerRevealer>
                          >(nodeCollectionEnumerator)
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
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<TBearer>?
                                , TBearer
                                , TBearerRevealer
                              >(itemCollectionEnumerator, itemRevealer, null, null
                              , isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllIterate<ReusableWrappingEnumerator<TBearer>?, TBearer>(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<TBearer>?
                            , TBearer
                          >(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAllIterate<ReusableWrappingEnumerator<TBearer>?, TBearer>(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrEnumeratorStructUnion(TBearerStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem = true;

        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructStringBearerOrEnumeratorStructUnion(List<TBearerStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<TBearerStruct?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructStringBearerOrEnumeratorStructUnion(
        List<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator = new ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>();
            nodeCollectionEnumerator.DisposeRecycles = false;
            nodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nodeCollectionEnumerator.ProxiedEnumerator = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        isItem       = false;
        itemRevealer = null;
    }

    public NullableStructStringBearerOrEnumeratorStructUnion(
        List<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator
                = new ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>?>();
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

    private readonly TBearerStruct? item;

    private readonly ReusableWrappingEnumerator<TBearerStruct?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>? nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>?>?
        nullableNodeCollectionEnumerator = null;

    private static readonly Dictionary<char[], ICharSequence?> LogComplexOnlyStaticInstance = new()
    {
        { "FirstKey".ToCharArray(), new MutableString("FirstValue") }
      , { "SecondKey".ToCharArray(), new CharArrayStringBuilder("SecondValue") }
      , { "ThirdKey".ToCharArray(), null }
    };

    private readonly KeyValuePair<string, char[]?>[] logComplexOnlyInstance = new Dictionary<string, char[]?>()
    {
        { "FirstKey", "FirstValue".ToCharArray() }
      , { "SecondKey", "SecondValue".ToCharArray() }
      , { "ThirdKey", null }
    }.ToArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isNullableNode)
                    if (isSimple)
                        return tos.StartSimpleCollectionType(this)
                                  .RevealAllIterateNullable(nullableNodeCollectionEnumerator, nodeRevealer)
                                  .Complete();
                    else
                        return tos.StartComplexCollectionType(this)
                                  .RevealAllIterateNullable(nullableNodeCollectionEnumerator, nodeRevealer)
                                  .Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAllIterate<
                                  ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>?
                                , NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>
                                , NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>?
                                , NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>
                                , NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>?
                            , NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>>?
                            , NullableStructStringBearerOrEnumeratorStructUnion<TBearerStruct>
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
                              .RevealAllIterateNullable(itemCollectionEnumerator, itemRevealer, null, null
                                                      , isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllIterateNullable(itemCollectionEnumerator, itemRevealer, null, null
                                               , isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterateNullable<
                              ReusableWrappingEnumerator<TBearerStruct?>?
                            , TBearerStruct
                          >(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAllIterateNullable<
                   ReusableWrappingEnumerator<TBearerStruct?>?
                 , TBearerStruct
               >(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer> : IStringBearer
    where TBearer : TBearerRevealer?
    where TBearerRevealer : IStringBearer
{
    private readonly bool isNode;

    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerRevealer>? itemRevealer;

    private readonly PalantírReveal<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>>? nodeRevealer;

    public StringBearerOrEnumeratorClassUnion(TBearer item, bool asSimple, bool asValue = true, PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringBearerOrEnumeratorClassUnion(List<TBearer>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerRevealer>? withRevealer = null)
    {
        isItem = false;
        item   = default!;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<TBearer>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public StringBearerOrEnumeratorClassUnion(List<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>?>? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator = new ReusableWrappingEnumerator<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>?>();
            nodeCollectionEnumerator.DisposeRecycles = false;
            nodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nodeCollectionEnumerator.ProxiedEnumerator = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        item         = default!;
        isItem       = false;
        itemRevealer = null;
    }

    private readonly TBearer item;

    private readonly ReusableWrappingEnumerator<TBearer>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>?>? nodeCollectionEnumerator;

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
                                  ReusableWrappingEnumerator<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>?>?
                                , StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>
                                , StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>?>?
                                , StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>
                                , StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>?>?
                            , StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>?>?
                            , StringBearerOrEnumeratorClassUnion<TBearer, TBearerRevealer>
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
                                  ReusableWrappingEnumerator<TBearer>?
                                , TBearer
                                , TBearerRevealer
                              >(itemCollectionEnumerator, itemRevealer, null, null
                              , isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                           ReusableWrappingEnumerator<TBearer>?
                         , TBearer
                         , TBearerRevealer
                       >(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<TBearer>?
                            , TBearer
                          >(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValue(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsString(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAllIterate<
                   ReusableWrappingEnumerator<TBearer>?
                 , TBearer
               >(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct> : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    private readonly bool isNode;

    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TBearerStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>? nodeRevealer;

    public NullableStructStringBearerOrEnumeratorClassUnion(TBearerStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem = true;

        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructStringBearerOrEnumeratorClassUnion(List<TBearerStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TBearerStruct>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<TBearerStruct?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructStringBearerOrEnumeratorClassUnion(
        List<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator = new ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>();
            nodeCollectionEnumerator.DisposeRecycles = false;
            nodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nodeCollectionEnumerator.ProxiedEnumerator = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        isItem       = false;
        itemRevealer = null;
    }

    private readonly TBearerStruct? item;

    private readonly ReusableWrappingEnumerator<TBearerStruct?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>? nodeCollectionEnumerator;

    private static readonly Dictionary<bool, StringBuilder> LogComplexOnlyStaticInstance = new()
    {
        { true, new("FirstValue") }
      , { false, new("SecondValue") }
    };

    private readonly List<KeyValuePair<int, IPAddress?>> logComplexOnlyInstance = new Dictionary<int, IPAddress?>()
    {
        { 1, new IPAddress([1, 1, 1, 1]) }
      , { 2, new IPAddress([2, 2, 2, 2]) }
      , { 3, new IPAddress([3, 3, 3, 3]) }
      , { 4, new IPAddress([4, 4, 4, 4]) }
    }.ToList();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>?
                                , NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>
                                , NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>?
                                , NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>
                                , NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>?
                            , NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>>?
                            , NullableStructStringBearerOrEnumeratorClassUnion<TBearerStruct>
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
                              .RevealAllIterateNullable(itemCollectionEnumerator, itemRevealer, null, null
                                                      , isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllIterateNullable(itemCollectionEnumerator, itemRevealer, null, null
                                               , isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).RevealAsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).RevealAsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterateNullable<
                              ReusableWrappingEnumerator<TBearerStruct?>?
                            , TBearerStruct
                          >(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .RevealAsValueOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .RevealAsStringOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .RevealAllIterateNullable<
                   ReusableWrappingEnumerator<TBearerStruct?>?
                 , TBearerStruct
               >(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}
