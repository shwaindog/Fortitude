// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.MapCollections;

public class MapDictBothFormatStringsUnion<TOther, TKey, TValue> : IStringBearer
    where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapDictBothFormatStringsUnion(IReadOnlyDictionary<TKey, TValue>? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapDictBothFormatStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapDictBothFormatStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapDictBothFormatStringsUnion(IReadOnlyDictionary<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>? nodeColl
      , string? valueFmtString = null
      , string? keyFmtString = null)
    {
        isNode            = true;
        nodeMap           = nodeColl;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;

        mapCollection = null;
    }

    private readonly IReadOnlyDictionary<TKey, TValue>? mapCollection;

    private readonly IReadOnlyDictionary<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPreCollectionField { get; set; }

    public IStringBearer? LogPostField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPostCollectionField { get; set; }

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isOtherUntypedItem)
            return otherUntypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isOtherTypedItem)
            return otherTypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isNode) return tos.StartKeyedCollectionType(this).AddAll(nodeMap, valueFormatString, keyFormatString).Complete();
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAll(mapCollection, valueFormatString, keyFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}

public class MapArrayBothFormatStringsUnion<TOther, TKey, TValue> : IStringBearer
    where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapArrayBothFormatStringsUnion(KeyValuePair<TKey, TValue>[]? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapArrayBothFormatStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapArrayBothFormatStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapArrayBothFormatStringsUnion(KeyValuePair<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>[]? nodeColl
      , string? valueFmtString = null
      , string? keyFmtString = null)
    {
        isNode            = true;
        nodeMap           = nodeColl;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;

        mapCollection = null;
    }

    private readonly KeyValuePair<TKey, TValue>[]? mapCollection;

    private readonly KeyValuePair<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>[]? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPreCollectionField { get; set; }

    public IStringBearer? LogPostField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPostCollectionField { get; set; }

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isOtherUntypedItem)
            return otherUntypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isOtherTypedItem)
            return otherTypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isNode) return tos.StartKeyedCollectionType(this).AddAll(nodeMap, valueFormatString, keyFormatString).Complete();
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAll(mapCollection, valueFormatString, keyFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}

public class MapListBothFormatStringsUnion<TOther, TKey, TValue> : IStringBearer
    where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapListBothFormatStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapListBothFormatStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapListBothFormatStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapListBothFormatStringsUnion(KeyValuePair<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>[]? nodeColl
      , string? valueFmtString = null
      , string? keyFmtString = null)
    {
        isNode            = true;
        nodeMap           = nodeColl;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;

        mapCollection = null;
    }

    private readonly List<KeyValuePair<TKey, TValue>>? mapCollection;

    private readonly KeyValuePair<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>[]? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPreCollectionField { get; set; }

    public IStringBearer? LogPostField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPostCollectionField { get; set; }

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isOtherUntypedItem)
            return otherUntypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isOtherTypedItem)
            return otherTypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isNode) return tos.StartKeyedCollectionType(this).AddAll(nodeMap, valueFormatString, keyFormatString).Complete();
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAll(mapCollection, valueFormatString, keyFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}


public class MapEnumeratorBothFormatStringsUnion<TOther, TKey, TValue> : IStringBearer
    where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapEnumeratorBothFormatStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapEnumeratorBothFormatStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapEnumeratorBothFormatStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapEnumeratorBothFormatStringsUnion(KeyValuePair<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>[]? nodeColl
      , string? valueFmtString = null
      , string? keyFmtString = null)
    {
        isNode            = true;
        nodeMap           = nodeColl;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;

        mapCollection = null;
    }

    private readonly List<KeyValuePair<TKey, TValue>>? mapCollection;

    private readonly KeyValuePair<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>[]? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPreCollectionField { get; set; }

    public IStringBearer? LogPostField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPostCollectionField { get; set; }

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isOtherUntypedItem)
            return otherUntypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isOtherTypedItem)
            return otherTypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isNode) return tos.StartKeyedCollectionType(this).AddAll(nodeMap, valueFormatString, keyFormatString).Complete();
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAllEnumerate<List<KeyValuePair<TKey, TValue>>, TKey, TValue>(mapCollection, valueFormatString, keyFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}


public class MapEnumerableBothFormatStringsUnion<TOther, TKey, TValue> : IStringBearer
    where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapEnumerableBothFormatStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value?.GetEnumerator();
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapEnumerableBothFormatStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapEnumerableBothFormatStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapEnumerableBothFormatStringsUnion(KeyValuePair<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>[]? nodeColl
      , string? valueFmtString = null
      , string? keyFmtString = null)
    {
        isNode            = true;
        nodeMap           = nodeColl;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;

        mapCollection = null;
    }

    private readonly List<KeyValuePair<TKey, TValue>>.Enumerator? mapCollection;

    private readonly KeyValuePair<TKey, MapDictBothFormatStringsUnion<TOther, TKey, TValue>>[]? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPreCollectionField { get; set; }

    public IStringBearer? LogPostField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPostCollectionField { get; set; }

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isOtherUntypedItem)
            return otherUntypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isOtherTypedItem)
            return otherTypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isNode) return tos.StartKeyedCollectionType(this).AddAll(nodeMap, valueFormatString, keyFormatString).Complete();
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAllIterate(mapCollection, valueFormatString, keyFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}
