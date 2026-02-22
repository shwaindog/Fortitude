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

public readonly struct BoolOrArrayStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrArrayStructUnion>? nodeRevealer;

    public BoolOrArrayStructUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public BoolOrArrayStructUnion(bool[]? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public BoolOrArrayStructUnion(BoolOrArrayStructUnion[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrArrayStructUnion>? withNodeRevealer = null)
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

    public BoolOrArrayStructUnion(BoolOrArrayStructUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrArrayStructUnion>? withNodeRevealer = null)
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

    private readonly bool    item;
    private readonly bool[]? itemCollection;

    private readonly BoolOrArrayStructUnion[]?  nodeCollection         = null;
    private readonly BoolOrArrayStructUnion?[]? nullableNodeCollection = null;

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

public readonly struct NullableStructBoolOrArrayStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrArrayStructUnion>? nodeRevealer;

    public NullableStructBoolOrArrayStructUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public NullableStructBoolOrArrayStructUnion(bool?[]? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public NullableStructBoolOrArrayStructUnion(NullableStructBoolOrArrayStructUnion[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructBoolOrArrayStructUnion>? withNodeRevealer = null)
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

    public NullableStructBoolOrArrayStructUnion(NullableStructBoolOrArrayStructUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructBoolOrArrayStructUnion>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        nullableNodeCollection = nodeColl;

        nodeRevealer = withNodeRevealer;
        isSimple     = asSimple;
        isValue      = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly bool?    item;
    private readonly bool?[]? itemCollection;

    private readonly NullableStructBoolOrArrayStructUnion[]?  nodeCollection         = null;
    private readonly NullableStructBoolOrArrayStructUnion?[]? nullableNodeCollection = null;

    private static readonly MutableString LogComplexOnlyStaticInstance = new("\"This\" is only shown when StringStyle is Log and in a Complex Mold");

    private readonly CharArrayStringBuilder logComplexOnlyInstance = new("\"This\" is only shown when StringStyle is Log and in a Complex Mold");

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

public class BoolOrArrayClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrArrayClassUnion>? nodeRevealer;

    public BoolOrArrayClassUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public BoolOrArrayClassUnion(bool[]? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public BoolOrArrayClassUnion(BoolOrArrayClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrArrayClassUnion>? withNodeRevealer = null)
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

    private readonly bool    item;
    private readonly bool[]? itemCollection;

    private readonly BoolOrArrayClassUnion?[]? nodeCollection;

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

public class NullableStructBoolOrArrayClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrArrayClassUnion>? nodeRevealer;

    public NullableStructBoolOrArrayClassUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public NullableStructBoolOrArrayClassUnion(bool?[]? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public NullableStructBoolOrArrayClassUnion(NullableStructBoolOrArrayClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructBoolOrArrayClassUnion>? withNodeRevealer = null)
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

    private readonly bool?    item;
    private readonly bool?[]? itemCollection;

    private readonly NullableStructBoolOrArrayClassUnion?[]? nodeCollection;

    private static readonly int LogComplexOnlyStaticInstance = 7;

    private readonly bool? logComplexOnlyInstance = null;

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
                              .RevealAll(itemCollection, itemRevealer, null
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

public class BoolOrSpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrSpanClassUnion>? nodeRevealer;

    public BoolOrSpanClassUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public BoolOrSpanClassUnion(bool[]? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public BoolOrSpanClassUnion(BoolOrSpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrSpanClassUnion>? withNodeRevealer = null)
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

    private readonly bool    item;
    private readonly bool[]? itemCollection;

    private readonly BoolOrSpanClassUnion?[]? nodeCollection;

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

public class NullableStructBoolOrSpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrSpanClassUnion>? nodeRevealer;

    public NullableStructBoolOrSpanClassUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public NullableStructBoolOrSpanClassUnion(bool?[]? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public NullableStructBoolOrSpanClassUnion(NullableStructBoolOrSpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructBoolOrSpanClassUnion>? withNodeRevealer = null)
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

    private readonly bool?    item;
    private readonly bool?[]? itemCollection;

    private readonly NullableStructBoolOrSpanClassUnion?[]? nodeCollection;

    private static readonly int? LogComplexOnlyStaticInstance = 7;

    private readonly decimal? logComplexOnlyInstance = null;


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
                    return tos.StartSimpleCollectionType(this)
                              .RevealAll(itemCollection.AsSpan(), itemRevealer, null
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
                       .RevealAll(itemCollection.AsSpan(), itemRevealer, null
                                , isValue ? DefaultCallerTypeFlags : AsStringContent)
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

public class BoolOrReadOnlySpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrReadOnlySpanClassUnion>? nodeRevealer;

    public BoolOrReadOnlySpanClassUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public BoolOrReadOnlySpanClassUnion(bool[]? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public BoolOrReadOnlySpanClassUnion(BoolOrReadOnlySpanClassUnion?[]? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrReadOnlySpanClassUnion>? withNodeRevealer = null)
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

    private readonly bool    item;
    private readonly bool[]? itemCollection;

    private readonly BoolOrReadOnlySpanClassUnion?[]? nodeCollection;

    private static readonly Complex LogComplexOnlyStaticInstance = new(12.24, 23.34);

    private readonly char[] logComplexOnlyInstance = "New String Per Instance".ToCharArray();

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<BoolOrReadOnlySpanClassUnion?>)nodeCollection, nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<BoolOrReadOnlySpanClassUnion?>)nodeCollection, nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<BoolOrReadOnlySpanClassUnion?>)nodeCollection)
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<BoolOrReadOnlySpanClassUnion?>)nodeCollection)
                          .Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this).RevealAll((ReadOnlySpan<bool>)itemCollection, itemRevealer, null
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
                       .RevealAll((ReadOnlySpan<bool>)itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this).AddAll((ReadOnlySpan<bool>)itemCollection).Complete();
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
               .AddAll((ReadOnlySpan<bool>)itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class NullableStructBoolOrReadOnlySpanClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrReadOnlySpanClassUnion>? nodeRevealer;

    public NullableStructBoolOrReadOnlySpanClassUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public NullableStructBoolOrReadOnlySpanClassUnion(bool?[]? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public NullableStructBoolOrReadOnlySpanClassUnion(NullableStructBoolOrReadOnlySpanClassUnion?[]? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<NullableStructBoolOrReadOnlySpanClassUnion>? withNodeRevealer = null)
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

    private readonly bool?    item;
    private readonly bool?[]? itemCollection;

    private readonly NullableStructBoolOrReadOnlySpanClassUnion?[]? nodeCollection;

    private static readonly Complex? LogComplexOnlyStaticInstance = new(12.24, 23.34);

    private readonly string logComplexOnlyInstance = new("New String Per Instance");

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isNode)
            if (nodeRevealer != null)
                if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion?>)nodeCollection.AsSpan(), nodeRevealer)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection)
                              .RevealAll((ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion?>)nodeCollection.AsSpan(), nodeRevealer)
                              .Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion?>)nodeCollection.AsSpan())
                          .Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection)
                          .RevealAll((ReadOnlySpan<NullableStructBoolOrReadOnlySpanClassUnion?>)nodeCollection.AsSpan())
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
                              .RevealAll((ReadOnlySpan<bool?>)itemCollection.AsSpan(), itemRevealer, null
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
                       .RevealAll((ReadOnlySpan<bool?>)itemCollection.AsSpan(), itemRevealer, null
                                , isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this).AddAll((ReadOnlySpan<bool?>)itemCollection.AsSpan()).Complete();
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
               .AddAll((ReadOnlySpan<bool?>)itemCollection.AsSpan(), null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct BoolOrListStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrListStructUnion>? nodeRevealer;

    public BoolOrListStructUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public BoolOrListStructUnion(List<bool>? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public BoolOrListStructUnion(List<BoolOrListStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrListStructUnion>? withNodeRevealer = null)
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

    public BoolOrListStructUnion(List<BoolOrListStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrListStructUnion>? withNodeRevealer = null)
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

    private readonly bool        item;
    private readonly List<bool>? itemCollection;

    private readonly List<BoolOrListStructUnion>?  nodeCollection         = null;
    private readonly List<BoolOrListStructUnion?>? nullableNodeCollection = null;

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
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyField.AlwaysAdd(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyField.AlwaysAdd(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct NullableStructBoolOrListStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;

    private readonly bool isNode   = false;
    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrListStructUnion>? nodeRevealer;

    public NullableStructBoolOrListStructUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public NullableStructBoolOrListStructUnion(List<bool?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public NullableStructBoolOrListStructUnion(List<NullableStructBoolOrListStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructBoolOrListStructUnion>? withNodeRevealer = null)
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

    public NullableStructBoolOrListStructUnion(List<NullableStructBoolOrListStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructBoolOrListStructUnion>? withNodeRevealer = null)
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

    private readonly bool?        item;
    private readonly List<bool?>? itemCollection;

    private readonly List<NullableStructBoolOrListStructUnion>?  nodeCollection         = null;
    private readonly List<NullableStructBoolOrListStructUnion?>? nullableNodeCollection = null;

    private static readonly IPAddress? LogComplexOnlyStaticInstance = null;

    private readonly BigInteger? logComplexOnlyInstance = null;


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
                    return tos.StartSimpleCollectionType(this)
                              .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
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

public class BoolOrListClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrListClassUnion>? nodeRevealer;

    public BoolOrListClassUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public BoolOrListClassUnion(List<bool>? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public BoolOrListClassUnion(List<BoolOrListClassUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrListClassUnion>? withNodeRevealer = null)
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

    private readonly bool        item;
    private readonly List<bool>? itemCollection;

    private readonly List<BoolOrListClassUnion>? nodeCollection;

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
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                          , (ReadOnlySpan<int>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
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

public class NullableStructBoolOrListClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrListClassUnion>? nodeRevealer;

    public NullableStructBoolOrListClassUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public NullableStructBoolOrListClassUnion(List<bool?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public NullableStructBoolOrListClassUnion(List<NullableStructBoolOrListClassUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<NullableStructBoolOrListClassUnion>? withNodeRevealer = null)
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

    private readonly bool?        item;
    private readonly List<bool?>? itemCollection;

    private readonly List<NullableStructBoolOrListClassUnion>? nodeCollection;

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

public readonly struct BoolOrEnumerableStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrEnumerableStructUnion>? nodeRevealer;

    public BoolOrEnumerableStructUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public BoolOrEnumerableStructUnion(List<bool>? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public BoolOrEnumerableStructUnion(List<BoolOrEnumerableStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrEnumerableStructUnion>? withNodeRevealer = null)
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

    public BoolOrEnumerableStructUnion(List<BoolOrEnumerableStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrEnumerableStructUnion>? withNodeRevealer = null)
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

    private readonly bool        item;
    private readonly List<bool>? itemCollection;

    private readonly List<BoolOrEnumerableStructUnion>?  nodeCollection         = null;
    private readonly List<BoolOrEnumerableStructUnion?>? nullableNodeCollection = null;

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
                                  .RevealAllEnumerate(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                    else
                        return tos.StartComplexCollectionType(nullableNodeCollection)
                                  .RevealAllEnumerate(nullableNodeCollection, nodeRevealer)
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
                              .RevealAllEnumerate(nullableNodeCollection)
                              .Complete();
                else
                    return tos.StartComplexCollectionType(nullableNodeCollection)
                              .RevealAllEnumerate(nullableNodeCollection)
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
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllEnumerate(itemCollection).Complete();
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
               .AddAllEnumerate(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct NullableStructBoolOrEnumerableStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;
    private readonly bool isItem         = false;
    private readonly bool isSimple       = false;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrEnumerableStructUnion>? nodeRevealer;

    public NullableStructBoolOrEnumerableStructUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public NullableStructBoolOrEnumerableStructUnion(List<bool?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public NullableStructBoolOrEnumerableStructUnion(List<NullableStructBoolOrEnumerableStructUnion>? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<NullableStructBoolOrEnumerableStructUnion>? withNodeRevealer = null)
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

    public NullableStructBoolOrEnumerableStructUnion(List<NullableStructBoolOrEnumerableStructUnion?>? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<NullableStructBoolOrEnumerableStructUnion>? withNodeRevealer = null)
    {
        isNode                 = true;
        nullableNodeCollection = nodeColl;
        nodeRevealer           = withNodeRevealer;
        isSimple               = asSimple;
        isValue                = asValue;

        isItem         = false;
        itemCollection = null;
        itemRevealer   = null;
    }

    private readonly bool?        item;
    private readonly List<bool?>? itemCollection;

    private readonly List<NullableStructBoolOrEnumerableStructUnion>?  nodeCollection         = null;
    private readonly List<NullableStructBoolOrEnumerableStructUnion?>? nullableNodeCollection = null;

    private static readonly string?[] LogComplexOnlyStaticReadOnlySpanInstance =
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
                        return tos.StartSimpleCollectionType(nullableNodeCollection)
                                  .RevealAllEnumerate(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                    else
                        return tos.StartComplexCollectionType(nullableNodeCollection)
                                  .RevealAllEnumerate(nullableNodeCollection, nodeRevealer)
                                  .Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(nodeCollection).RevealAllEnumerate(nodeCollection, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(nodeCollection).RevealAllEnumerate(nodeCollection, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(nodeCollection).RevealAllEnumerate(nodeCollection).Complete();
            else
                return tos.StartComplexCollectionType(nodeCollection).RevealAllEnumerate(nodeCollection).Complete();
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
                           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyCollectionField.AlwaysAddAllNullable(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                                      , (ReadOnlySpan<string?>)LogComplexOnlyStaticReadOnlySpanInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .LogOnlyCollectionField.AlwaysAddAllNullable(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                                      , (ReadOnlySpan<string?>)LogComplexOnlyStaticReadOnlySpanInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAllNullable(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                                  , (ReadOnlySpan<string?>)LogComplexOnlyStaticReadOnlySpanInstance)
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
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAllNullable(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                                  , (ReadOnlySpan<string?>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsStringOrNull(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .LogOnlyCollectionField.AlwaysAddAllNullable(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                                  , (ReadOnlySpan<string?>)LogComplexOnlyStaticReadOnlySpanInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAllCharSeq(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyCollectionField.AlwaysAddAllNullable(nameof(LogComplexOnlyStaticReadOnlySpanInstance)
                                                          , (ReadOnlySpan<string?>)LogComplexOnlyStaticReadOnlySpanInstance)
               .Complete();
    }
}

public class BoolOrEnumerableClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrEnumerableClassUnion>? nodeRevealer;

    public BoolOrEnumerableClassUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public BoolOrEnumerableClassUnion(List<bool>? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public BoolOrEnumerableClassUnion(List<BoolOrEnumerableClassUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrEnumerableClassUnion>? withNodeRevealer = null)
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

    private readonly bool        item;
    private readonly List<bool>? itemCollection;

    private readonly List<BoolOrEnumerableClassUnion>? nodeCollection;

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
                              .RevealAllEnumerate(itemCollection, itemRevealer, null
                                                , isValue ? DefaultCallerTypeFlags : AsStringContent)
                              .Complete();
            else if (isItem)
                if (isValue)
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsValue(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                           .LogOnlyCollectionField.AlwaysAddAllEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
                else
                    return tos
                           .StartComplexContentType(this)
                           .RevealAsString(nameof(item), item, itemRevealer)
                           .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                           .LogOnlyCollectionField.AlwaysAddAllEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                           .Complete();
            else
                return tos
                       .StartComplexCollectionType(this)
                       .RevealAllEnumerate(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllEnumerate(itemCollection).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
                       .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
                       .LogOnlyCollectionField.AlwaysAddAllEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllEnumerate(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticSpanInstance), LogComplexOnlyStaticSpanInstance.AsSpan())
               .LogOnlyCollectionField.AlwaysAddAllEnumerate(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class NullableStructBoolOrEnumerableClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrEnumerableClassUnion>? nodeRevealer;

    public NullableStructBoolOrEnumerableClassUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;

        itemCollection = null;
        isNode         = false;
    }

    public NullableStructBoolOrEnumerableClassUnion(List<bool?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<bool>? withRevealer = null)
    {
        itemCollection = itemColl;
        isSimple       = asSimple;
        isValue        = asValue;
        itemRevealer   = withRevealer;

        isItem = false;
        isNode = false;
    }

    public NullableStructBoolOrEnumerableClassUnion(List<NullableStructBoolOrEnumerableClassUnion?>? nodeColl, bool asSimple
      , bool asValue = true
      , PalantírReveal<NullableStructBoolOrEnumerableClassUnion>? withNodeRevealer = null)
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

    private readonly bool?        item;
    private readonly List<bool?>? itemCollection;

    private readonly List<NullableStructBoolOrEnumerableClassUnion?>? nodeCollection;

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
                       .RevealAll(itemCollection, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this).AddAll(itemCollection).Complete();
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
               .AddAll(itemCollection, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public readonly struct BoolOrEnumeratorStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrEnumeratorStructUnion>? nodeRevealer;

    public BoolOrEnumeratorStructUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public BoolOrEnumeratorStructUnion(List<bool>? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<bool>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public BoolOrEnumeratorStructUnion(List<BoolOrEnumeratorStructUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<BoolOrEnumeratorStructUnion>();
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

    public BoolOrEnumeratorStructUnion(List<BoolOrEnumeratorStructUnion?>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator                           = new ReusableWrappingEnumerator<BoolOrEnumeratorStructUnion?>();
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

    private readonly bool item;

    private readonly ReusableWrappingEnumerator<bool>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<BoolOrEnumeratorStructUnion>? nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<BoolOrEnumeratorStructUnion?>?
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
                        return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nullableNodeCollectionEnumerator, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAllEnumerate(nullableNodeCollectionEnumerator, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nullableNodeCollectionEnumerator).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAllEnumerate(nullableNodeCollectionEnumerator).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllEnumerate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        if (isSimple)
            if (isItem)
                if (isValue)
                    return tos.StartSimpleContentType(this).AsValue(item).Complete();
                else
                    return tos.StartSimpleContentType(this).AsString(item).Complete();
            else
                return tos.StartSimpleCollectionType(this).AddAllEnumerate(itemCollectionEnumerator).Complete();
        if (isItem)
            if (isValue)
                return tos
                       .StartComplexContentType(this)
                       .AsValue(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
            else
                return tos
                       .StartComplexContentType(this)
                       .AsString(nameof(item), item)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
                       .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
                       .Complete();
        return tos
               .StartComplexCollectionType(this)
               .AddAllEnumerate(itemCollectionEnumerator, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public readonly struct NullableStructBoolOrEnumeratorStructUnion : IStringBearer
{
    private readonly bool isNullableNode = false;
    private readonly bool isNode         = false;

    private readonly bool isItem   = false;
    private readonly bool isSimple = false;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrEnumeratorStructUnion>? nodeRevealer;

    public NullableStructBoolOrEnumeratorStructUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructBoolOrEnumeratorStructUnion(List<bool?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<bool>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<bool?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructBoolOrEnumeratorStructUnion(List<NullableStructBoolOrEnumeratorStructUnion>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructBoolOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator
                = new ReusableWrappingEnumerator<NullableStructBoolOrEnumeratorStructUnion>();
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

    public NullableStructBoolOrEnumeratorStructUnion(List<NullableStructBoolOrEnumeratorStructUnion?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructBoolOrEnumeratorStructUnion>? withNodeRevealer = null)
    {
        isNullableNode = true;
        isNode         = true;

        if (nodeColl != null)
        {
            nullableNodeCollectionEnumerator
                = new ReusableWrappingEnumerator<NullableStructBoolOrEnumeratorStructUnion?>();
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

    private readonly bool? item;

    private readonly ReusableWrappingEnumerator<bool?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<NullableStructBoolOrEnumeratorStructUnion>?  nodeCollectionEnumerator;
    private readonly ReusableWrappingEnumerator<NullableStructBoolOrEnumeratorStructUnion?>? nullableNodeCollectionEnumerator = null;


    private static readonly KeyValuePair<char[], ICharSequence?>[] LogComplexOnlyStaticInstance = new Dictionary<char[], ICharSequence?>()
    {
        { "FirstKey".ToCharArray(), new MutableString("FirstValue") }
      , { "SecondKey".ToCharArray(), new CharArrayStringBuilder("SecondValue") }
      , { "ThirdKey".ToCharArray(), null }
    }.ToArray();

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
                        return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nullableNodeCollectionEnumerator, nodeRevealer).Complete();
                    else
                        return tos.StartComplexCollectionType(this).RevealAllEnumerate(nullableNodeCollectionEnumerator, nodeRevealer).Complete();
                else if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isNullableNode)
                if (isSimple)
                    return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nullableNodeCollectionEnumerator).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAllEnumerate(nullableNodeCollectionEnumerator).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllEnumerate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this).AddAllEnumerate(itemCollectionEnumerator).Complete();
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
               .AddAllEnumerate(itemCollectionEnumerator, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}

public class BoolOrEnumeratorClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<BoolOrEnumeratorClassUnion>? nodeRevealer;

    public BoolOrEnumeratorClassUnion(bool item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public BoolOrEnumeratorClassUnion(List<bool>? itemColl, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<bool>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public BoolOrEnumeratorClassUnion(List<BoolOrEnumeratorClassUnion>? nodeColl, bool asSimple, bool asValue = true
      , PalantírReveal<BoolOrEnumeratorClassUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<BoolOrEnumeratorClassUnion>();
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

    private readonly bool item;

    private readonly ReusableWrappingEnumerator<bool>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<BoolOrEnumeratorClassUnion>? nodeCollectionEnumerator;

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
                    return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllEnumerate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this).AddAllEnumerate(itemCollectionEnumerator).Complete();
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
               .AddAllEnumerate(itemCollectionEnumerator, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .Complete();
    }
}

public class NullableStructBoolOrEnumeratorClassUnion : IStringBearer
{
    private readonly bool isNode;
    private readonly bool isItem;
    private readonly bool isSimple;
    private readonly bool isValue;

    private readonly PalantírReveal<bool>? itemRevealer;

    private readonly PalantírReveal<NullableStructBoolOrEnumeratorClassUnion>? nodeRevealer;

    public NullableStructBoolOrEnumeratorClassUnion(bool? item, bool asSimple, bool asValue = true, PalantírReveal<bool>? withRevealer = null)
    {
        isItem       = true;
        this.item    = item;
        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructBoolOrEnumeratorClassUnion(List<bool?>? itemColl, bool asSimple, bool asValue = true
      , PalantírReveal<bool>? withRevealer = null)
    {
        isItem = false;

        if (itemColl != null)
        {
            itemCollectionEnumerator                           = new ReusableWrappingEnumerator<bool?>();
            itemCollectionEnumerator.DisposeRecycles           = false;
            itemCollectionEnumerator.AutoRecycleAtRefCountZero = false;
            itemCollectionEnumerator.ProxiedEnumerator         = itemColl.GetEnumerator();
        }

        isSimple     = asSimple;
        isValue      = asValue;
        itemRevealer = withRevealer;
    }

    public NullableStructBoolOrEnumeratorClassUnion(List<NullableStructBoolOrEnumeratorClassUnion?>? nodeColl, bool asSimple
      , bool asValue = true, PalantírReveal<NullableStructBoolOrEnumeratorClassUnion>? withNodeRevealer = null)
    {
        isNode = true;

        if (nodeColl != null)
        {
            nodeCollectionEnumerator                           = new ReusableWrappingEnumerator<NullableStructBoolOrEnumeratorClassUnion?>();
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

    private readonly bool? item;

    private readonly ReusableWrappingEnumerator<bool?>? itemCollectionEnumerator;

    private readonly ReusableWrappingEnumerator<NullableStructBoolOrEnumeratorClassUnion?>? nodeCollectionEnumerator;

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
                    return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator, nodeRevealer).Complete();
                else
                    return tos.StartComplexCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator, nodeRevealer).Complete();
            else if (isSimple)
                return tos.StartSimpleCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator).Complete();
            else
                return tos.StartComplexCollectionType(this).RevealAllEnumerate(nodeCollectionEnumerator).Complete();
        if (itemRevealer != null)
            if (isSimple)
                if (isItem)
                    if (isValue)
                        return tos.StartSimpleContentType(this).RevealAsValue(item, itemRevealer).Complete();
                    else
                        return tos.StartSimpleContentType(this).RevealAsString(item, itemRevealer).Complete();
                else
                    return tos.StartSimpleCollectionType(this)
                              .RevealAllEnumerate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                       .RevealAllEnumerate(itemCollectionEnumerator, itemRevealer, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
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
                return tos.StartSimpleCollectionType(this).AddAllEnumerate(itemCollectionEnumerator).Complete();
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
               .AddAllEnumerate(itemCollectionEnumerator, null, isValue ? DefaultCallerTypeFlags : AsStringContent)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(LogComplexOnlyStaticInstance), LogComplexOnlyStaticInstance)
               .LogOnlyKeyedCollectionField.AlwaysAddAll(nameof(logComplexOnlyInstance), logComplexOnlyInstance)
               .Complete();
    }
}
