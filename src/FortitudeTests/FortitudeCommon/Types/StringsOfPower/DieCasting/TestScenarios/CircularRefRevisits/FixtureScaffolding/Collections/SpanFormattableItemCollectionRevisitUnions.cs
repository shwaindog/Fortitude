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

public readonly struct SpanFormattableOrArrayStructUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrArrayStructUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrArrayStructUnion(TFmt item, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrArrayStructUnion(TFmt[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrArrayStructUnion(SpanFormattableOrArrayStructUnion<TFmt, TFmtRevealer>[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<SpanFormattableOrArrayStructUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    public SpanFormattableOrArrayStructUnion(SpanFormattableOrArrayStructUnion<TFmt, TFmtRevealer>?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<SpanFormattableOrArrayStructUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    private readonly TFmt    item;
    private readonly TFmt[]? itemCollection;

    private readonly SpanFormattableOrArrayStructUnion<TFmt, TFmtRevealer>[]?  nodeCollection         = null;
    private readonly SpanFormattableOrArrayStructUnion<TFmt, TFmtRevealer>?[]? nullableNodeCollection = null;

    private static readonly char[] LogComplexOnlyStaticInstance = "\"This\" is only shown when StringStyle is Log and in a Complex Mold".ToCharArray();
    
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
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
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

public readonly struct NullableStructSpanFormattableOrArrayStructUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrArrayStructUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrArrayStructUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrArrayStructUnion(TFmtStruct?[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrArrayStructUnion(NullableStructSpanFormattableOrArrayStructUnion<TFmtStruct>[]? nodeColl
      , bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructSpanFormattableOrArrayStructUnion<TFmtStruct>>? withNodeRevealer = null)
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

    public NullableStructSpanFormattableOrArrayStructUnion(NullableStructSpanFormattableOrArrayStructUnion<TFmtStruct>?[]? nodeColl
      , bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructSpanFormattableOrArrayStructUnion<TFmtStruct>>? withNodeRevealer = null)
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

    private readonly TFmtStruct?    item;
    private readonly TFmtStruct?[]? itemCollection;

    private readonly NullableStructSpanFormattableOrArrayStructUnion<TFmtStruct>[]?  nodeCollection         = null;
    private readonly NullableStructSpanFormattableOrArrayStructUnion<TFmtStruct>?[]? nullableNodeCollection = null;

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
                       .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAddCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAddCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class SpanFormattableOrArrayClassUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrArrayClassUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrArrayClassUnion(TFmt item, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrArrayClassUnion(TFmt[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrArrayClassUnion(SpanFormattableOrArrayClassUnion<TFmt, TFmtRevealer>?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<SpanFormattableOrArrayClassUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    private readonly TFmt    item;
    private readonly TFmt[]? itemCollection;

    private readonly        SpanFormattableOrArrayClassUnion<TFmt, TFmtRevealer>?[]? nodeCollection;
    
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
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
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

public class NullableStructSpanFormattableOrArrayClassUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrArrayClassUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrArrayClassUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrArrayClassUnion(TFmtStruct?[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrArrayClassUnion(NullableStructSpanFormattableOrArrayClassUnion<TFmtStruct>?[]? nodeColl
      , bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructSpanFormattableOrArrayClassUnion<TFmtStruct>>? withNodeRevealer = null)
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

    private readonly TFmtStruct?    item;
    private readonly TFmtStruct?[]? itemCollection;

    private readonly NullableStructSpanFormattableOrArrayClassUnion<TFmtStruct>?[]? nodeCollection;

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

public class SpanFormattableOrSpanClassUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrSpanClassUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrSpanClassUnion(TFmt item, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrSpanClassUnion(TFmt[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrSpanClassUnion(SpanFormattableOrSpanClassUnion<TFmt, TFmtRevealer>?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<SpanFormattableOrSpanClassUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    private readonly TFmt    item;
    private readonly TFmt[]? itemCollection;

    private readonly SpanFormattableOrSpanClassUnion<TFmt, TFmtRevealer>?[]? nodeCollection;

    private static readonly int? LogComplexOnlyStaticInstance = null;
    
    private readonly DateTime logComplexOnlyInstance = new (2026, 2, 16, 7, 6, 23);

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
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection.AsSpan()).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection.AsSpan(), null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class NullableStructSpanFormattableOrSpanClassUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isSimple;
    private readonly bool isItem;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrSpanClassUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrSpanClassUnion(TFmtStruct? item, bool asSimple, bool asValue = true, PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrSpanClassUnion(TFmtStruct?[]? itemColl, bool asSimple, bool asValue = true, PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrSpanClassUnion(NullableStructSpanFormattableOrSpanClassUnion<TFmtStruct>[]? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrSpanClassUnion<TFmtStruct>>? withNodeRevealer = null)
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

    private readonly TFmtStruct?    item;
    private readonly TFmtStruct?[]? itemCollection;

    private readonly NullableStructSpanFormattableOrSpanClassUnion<TFmtStruct>[]? nodeCollection;

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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection.AsSpan()).Complete();
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
               .AddAll(itemCollection.AsSpan(), null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrReadOnlySpanClassUnion(TFmt item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrReadOnlySpanClassUnion(TFmt[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrReadOnlySpanClassUnion(SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer>?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    private readonly TFmt    item;
    private readonly TFmt[]? itemCollection;

    private readonly SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer>?[]? nodeCollection;

    private static readonly Complex LogComplexOnlyStaticInstance = new(12.24, 23.34);
    
    private readonly char[] logComplexOnlyInstance = "New String Per Instance".ToCharArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAll((ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer>?>)nodeCollection
                                       , nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAll((ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer>?>)nodeCollection
                                       , nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAll((ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer>?>)nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAll((ReadOnlySpan<SpanFormattableOrReadOnlySpanClassUnion<TFmt, TFmtRevealer>?>)nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAll((ReadOnlySpan<TFmt>)itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAll((ReadOnlySpan<TFmt>)itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll((ReadOnlySpan<TFmt>)itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll((ReadOnlySpan<TFmt>)itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isSimple;
    private readonly bool isItem;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrReadOnlySpanClassUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrReadOnlySpanClassUnion(TFmtStruct?[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrReadOnlySpanClassUnion(
        NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct>?[]? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct>>? withNodeRevealer = null)
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

    private readonly TFmtStruct?    item;
    private readonly TFmtStruct?[]? itemCollection;

    private readonly NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct>?[]? nodeCollection;

    private static readonly Complex? LogComplexOnlyStaticInstance = new(12.24, 23.34);
    
    private readonly string logComplexOnlyInstance = new ("New String Per Instance");

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return
                        tos
                            .StartSimpleCollectionType(this)
                            .RevealAll((ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct>?>)nodeCollection
                                     , nodeRevealer)
                            .Complete();
                else
                    return
                        tos
                            .StartComplexCollectionType(this)
                            .RevealAll((ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct>?>)nodeCollection
                                     , nodeRevealer)
                            .Complete();
            else if (isSimple)
                return
                    tos
                        .StartSimpleCollectionType(this)
                        .RevealAll((ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct>?>)nodeCollection)
                        .Complete();
            else
                return
                    tos
                        .StartComplexCollectionType(this)
                        .RevealAll((ReadOnlySpan<NullableStructSpanFormattableOrReadOnlySpanClassUnion<TFmtStruct>?>)nodeCollection.AsSpan())
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
                              .RevealAll((ReadOnlySpan<TFmtStruct?>)itemCollection, itemRevealer, null
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
                        .RevealAll((ReadOnlySpan<TFmtStruct?>)itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this).AddAll((ReadOnlySpan<TFmtStruct?>)itemCollection).Complete();
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
               .AddAll((ReadOnlySpan<TFmtStruct?>)itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct SpanFormattableOrListStructUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNullableNode = false;

    private readonly bool isNode   = false;
    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrListStructUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrListStructUnion(TFmt item, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrListStructUnion(List<TFmt>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrListStructUnion(List<SpanFormattableOrListStructUnion<TFmt, TFmtRevealer>>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<SpanFormattableOrListStructUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    public SpanFormattableOrListStructUnion(List<SpanFormattableOrListStructUnion<TFmt, TFmtRevealer>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<SpanFormattableOrListStructUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    private readonly TFmt        item;
    private readonly List<TFmt>? itemCollection;

    private readonly List<SpanFormattableOrListStructUnion<TFmt, TFmtRevealer>>?  nodeCollection         = null;
    private readonly List<SpanFormattableOrListStructUnion<TFmt, TFmtRevealer>?>? nullableNodeCollection = null;

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
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
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

public readonly struct NullableStructSpanFormattableOrListStructUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrListStructUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrListStructUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrListStructUnion(List<TFmtStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrListStructUnion(List<NullableStructSpanFormattableOrListStructUnion<TFmtStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrListStructUnion<TFmtStruct>>? withNodeRevealer = null)
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

    public NullableStructSpanFormattableOrListStructUnion(List<NullableStructSpanFormattableOrListStructUnion<TFmtStruct>?>? nodeColl
      , bool asSimple, bool asValue = true, PalantírReveal<NullableStructSpanFormattableOrListStructUnion<TFmtStruct>>? withNodeRevealer = null)
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

    private readonly TFmtStruct?        item;
    private readonly List<TFmtStruct?>? itemCollection;

    private readonly List<NullableStructSpanFormattableOrListStructUnion<TFmtStruct>>?  nodeCollection         = null;
    private readonly List<NullableStructSpanFormattableOrListStructUnion<TFmtStruct>?>? nullableNodeCollection = null;

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

public class SpanFormattableOrListClassUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrListClassUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrListClassUnion(TFmt item, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrListClassUnion(List<TFmt>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrListClassUnion(List<SpanFormattableOrListClassUnion<TFmt, TFmtRevealer>>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<SpanFormattableOrListClassUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    private readonly TFmt        item;
    private readonly List<TFmt>? itemCollection;

    private readonly List<SpanFormattableOrListClassUnion<TFmt, TFmtRevealer>>? nodeCollection;

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
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class NullableStructSpanFormattableOrListClassUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrListClassUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrListClassUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrListClassUnion(List<TFmtStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrListClassUnion(List<NullableStructSpanFormattableOrListClassUnion<TFmtStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrListClassUnion<TFmtStruct>>? withNodeRevealer = null)
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

    private readonly TFmtStruct?        item;
    private readonly List<TFmtStruct?>? itemCollection;

    private readonly List<NullableStructSpanFormattableOrListClassUnion<TFmtStruct>>? nodeCollection;

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
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNullableNode = false;

    private readonly bool isNode   = false;
    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrEnumerableStructUnion(TFmt item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrEnumerableStructUnion(List<TFmt>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrEnumerableStructUnion(List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    public SpanFormattableOrEnumerableStructUnion(List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    private readonly TFmt        item;
    private readonly List<TFmt>? itemCollection;

    private readonly List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>?  nodeCollection         = null;
    private readonly List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>?>? nullableNodeCollection = null;

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
        new([1,1,1,1])
      , new([2,2,2,2])
      , null  
      , new([3,3,3,3])
      , new([4,4,4,4])
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
                                  List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>?
                                , SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>
                                , SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>
                              >(nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos
                           .StartComplexCollectionType(this)
                           .RevealAllEnumerate<
                               List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>?
                             , SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>
                             , SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>
                           >(nodeCollection, nodeRevealer)
                           .Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos
                           .StartSimpleCollectionType(this)
                           .RevealAllEnumerateNullable<
                               List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>?>?
                             , SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>
                           >(nullableNodeCollection)
                           .Complete();
                else
                    return tos
                           .StartComplexCollectionType(this)
                           .RevealAllEnumerateNullable<
                               List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>?>?
                             , SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>
                           >(nullableNodeCollection)
                           .Complete();
            else if (isSimple)
                return tos
                       .StartSimpleCollectionType(this)
                       .RevealAllEnumerate<
                           List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>?
                         , SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>
                       >(nodeCollection)
                       .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerate<
                           List<SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>>?
                         , SpanFormattableOrEnumerableStructUnion<TFmt, TFmtRevealer>
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
                                  List<TFmt>?
                                , TFmt
                                , TFmtRevealer
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
                           List<TFmt>?
                         , TFmt
                         , TFmtRevealer
                       >(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .AddAllEnumerate<List<TFmt>?, TFmt>(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllEnumerate<List<TFmt>?, TFmt>(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrEnumerableStructUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrEnumerableStructUnion(List<TFmtStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrEnumerableStructUnion(
        List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>? withNodeRevealer = null)
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

    public NullableStructSpanFormattableOrEnumerableStructUnion(
        List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>?>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>? withNodeRevealer = null)
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

    private readonly TFmtStruct?        item;
    private readonly List<TFmtStruct?>? itemCollection;

    private readonly List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>?  nodeCollection         = null;
    private readonly List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>?>? nullableNodeCollection = null;

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
                                  List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>?
                                , NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>
                                , NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>
                              >(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllEnumerate<
                                  List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>?
                                , NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>
                                , NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>
                              >(nodeCollection, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerateNullable<
                                  List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>?>?
                                , NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>
                              >(nullableNodeCollection).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllEnumerateNullable<
                                  List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>?>?
                                , NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>
                              >(nullableNodeCollection).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerate<
                              List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>?
                            , NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>
                          >(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllEnumerate<
                              List<NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>>?
                            , NullableStructSpanFormattableOrEnumerableStructUnion<TFmtStruct>
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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this)
                          .AddAllEnumerateNullable<List<TFmtStruct?>?, TFmtStruct>(itemCollection)
                          .Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllEnumerateNullable<List<TFmtStruct?>?, TFmtStruct>(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrEnumerableClassUnion(TFmt item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrEnumerableClassUnion(List<TFmt>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem         = false;
        item           = default!;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public SpanFormattableOrEnumerableClassUnion(List<SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
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

    private readonly TFmt        item;
    private readonly List<TFmt>? itemCollection;

    private readonly List<SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>?>? nodeCollection;

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
                                  List<SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>?>?
                                , SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>
                                , SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>
                              >(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllEnumerate<
                                  List<SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>?>?
                                , SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>
                                , SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>
                              >(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerate<
                              List<SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>?>?
                            , SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>
                          >(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllEnumerate<
                              List<SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>?>?
                            , SpanFormattableOrEnumerableClassUnion<TFmt, TFmtRevealer>
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
                                  List<TFmt>?
                                , TFmt
                                , TFmtRevealer
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
                           List<TFmt>?
                         , TFmt
                         , TFmtRevealer
                       >(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllEnumerate<List<TFmt>?, TFmt>(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllEnumerate<List<TFmt>?, TFmt>(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAllStringEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrEnumerableClassUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = true;
        this.item      = item;
        itemCollection = null;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrEnumerableClassUnion(List<TFmtStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem         = false;
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;
    }

    public NullableStructSpanFormattableOrEnumerableClassUnion(
        List<NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>?>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>>? withNodeRevealer = null)
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

    private readonly TFmtStruct?        item;
    private readonly List<TFmtStruct?>? itemCollection;

    private readonly List<NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>?>? nodeCollection;

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
                                  List<NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>?>?
                                , NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>
                                , NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>
                              >(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllEnumerate<
                                  List<NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>?>?
                                , NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>
                                , NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>
                              >(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllEnumerate<
                              List<NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>?>?
                            , NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>
                          >(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllEnumerate<
                              List<NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>?>?
                            , NullableStructSpanFormattableOrEnumerableClassUnion<TFmtStruct>
                          >(nodeCollection).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAllEnumerateNullable(itemCollection, itemRevealer, null
                                                                                , isValue ? DefaultCallerTypeFlags : AsStringContent).Complete();
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
                    return tos.StartSimpleContentType(this).AsValueOrNull(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsStringOrNull(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllEnumerateNullable<List<TFmtStruct?>?, TFmtStruct>(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValueOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllEnumerateNullable<List<TFmtStruct?>?, TFmtStruct>(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrEnumeratorStructUnion(TFmt item, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public SpanFormattableOrEnumeratorStructUnion(List<TFmt>? itemColl, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem = false;
        item   = default!;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<TFmt>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        item         = default!;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public SpanFormattableOrEnumeratorStructUnion(
        List<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator
                = new ReusableWrappingEnumerator<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>();
            nodeCollectionEnumerator.DisposeRecycles           = false;
            nodeCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            nodeCollectionEnumerator.ProxiedEnumerator         = nodeColl.GetEnumerator();
        }

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        item    = default!;
        isItem       = false;
        itemRevealer = null;
    }

    public SpanFormattableOrEnumeratorStructUnion(
        List<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator
                = new ReusableWrappingEnumerator<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>?>();
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

    private readonly TFmt item;

    private readonly ReusableWrappingEnumerator<TFmt>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>?
        nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>?>?
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
                                  ReusableWrappingEnumerator<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>?
                                , SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>
                                , SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>?
                                , SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>
                                , SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>?
                            , SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>
                          >(nodeCollectionEnumerator)
                          .Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>>?
                            , SpanFormattableOrEnumeratorStructUnion<TFmt, TFmtRevealer>
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
                                  ReusableWrappingEnumerator<TFmt>?
                                , TFmt
                                , TFmtRevealer
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
                           ReusableWrappingEnumerator<TFmt>?
                         , TFmt
                         , TFmtRevealer
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
                return tos.StartSimpleCollectionType(this).AddAllIterate<ReusableWrappingEnumerator<TFmt>?, TFmt>(itemCollectionEnumerator).Complete();
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
               .AddAllIterate<ReusableWrappingEnumerator<TFmt>?, TFmt>(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrEnumeratorStructUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem = true;

        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructSpanFormattableOrEnumeratorStructUnion(List<TFmtStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<TFmtStruct?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructSpanFormattableOrEnumeratorStructUnion(
        List<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator = new ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>();
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

    public NullableStructSpanFormattableOrEnumeratorStructUnion(
        List<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator
                = new ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>?>();
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

    private readonly TFmtStruct? item;

    private readonly ReusableWrappingEnumerator<TFmtStruct?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>? nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>?>?
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
        itemCollectionEnumerator?.Reset();
        nodeCollectionEnumerator?.Reset();
        nullableNodeCollectionEnumerator?.Reset();
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
                                  ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>?
                                , NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>
                                , NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>?
                                , NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>
                                , NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>?
                            , NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>>?
                            , NullableStructSpanFormattableOrEnumeratorStructUnion<TFmtStruct>
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
                              .RevealAllIterateNullable(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllIterateNullable(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this)
                          .AddAllIterateNullable<
                              ReusableWrappingEnumerator<TFmtStruct?>?
                            , TFmtStruct
                          >(itemCollectionEnumerator).Complete();
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
               .AddAllIterateNullable<
                   ReusableWrappingEnumerator<TFmtStruct?>?
                 , TFmtStruct
               >(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer> : IStringBearer
    where TFmt : TFmtRevealer?
    where TFmtRevealer : ISpanFormattable
{
    private readonly bool isNode;
    
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtRevealer>? itemRevealer;

    private readonly PalantírReveal<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>>? nodeRevealer;

    public SpanFormattableOrEnumeratorClassUnion(TFmt item, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public SpanFormattableOrEnumeratorClassUnion(List<TFmt>? itemColl, bool asSimple, bool asValue = true, PalantírReveal<TFmtRevealer>? withRevealer = null)
    {
        isItem = false;
        item   = default!;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<TFmt>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public SpanFormattableOrEnumeratorClassUnion(List<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?>();
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

    private readonly TFmt item;

    private readonly ReusableWrappingEnumerator<TFmt>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?>? nodeCollectionEnumerator;

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
                                  ReusableWrappingEnumerator<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?>?
                                , SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?
                                , SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>
                              >(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?>?
                                , SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?
                                , SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?>?
                            , SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?>?
                            , SpanFormattableOrEnumeratorClassUnion<TFmt, TFmtRevealer>?
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
                                  ReusableWrappingEnumerator<TFmt>?
                                , TFmt
                                , TFmtRevealer
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
                           ReusableWrappingEnumerator<TFmt>?
                         , TFmt
                         , TFmtRevealer
                       >(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllIterate<ReusableWrappingEnumerator<TFmt>?, TFmt>(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllIterate<ReusableWrappingEnumerator<TFmt>?, TFmt>(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct> : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    private readonly bool isNode;

    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<TFmtStruct>? itemRevealer;

    private readonly PalantírReveal<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>>? nodeRevealer;

    public NullableStructSpanFormattableOrEnumeratorClassUnion(TFmtStruct? item, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem = true;

        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructSpanFormattableOrEnumeratorClassUnion(List<TFmtStruct?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<TFmtStruct>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<TFmtStruct?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructSpanFormattableOrEnumeratorClassUnion(
        List<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>?>? nodeColl
      , bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator = new ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>?>();
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

    private readonly TFmtStruct? item;

    private readonly ReusableWrappingEnumerator<TFmtStruct?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>?>? nodeCollectionEnumerator;

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
        itemCollectionEnumerator?.Reset();
        nodeCollectionEnumerator?.Reset();
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>?>?
                                , NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>
                                , NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(this)
                              .RevealAllIterate<
                                  ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>?>?
                                , NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>
                                , NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>
                              >(nodeCollectionEnumerator, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>?>?
                            , NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>
                          >(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this)
                          .RevealAllIterate<
                              ReusableWrappingEnumerator<NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>?>?
                            , NullableStructSpanFormattableOrEnumeratorClassUnion<TFmtStruct>
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
                              .RevealAllIterateNullable(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllIterateNullable(itemCollectionEnumerator, itemRevealer, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this)
                          .AddAllIterateNullable<
                              ReusableWrappingEnumerator<TFmtStruct?>?
                            , TFmtStruct
                          >(itemCollectionEnumerator).Complete();
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
               .AddAllIterateNullable<
                   ReusableWrappingEnumerator<TFmtStruct?>?
                 , TFmtStruct
               >(itemCollectionEnumerator, null, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}
