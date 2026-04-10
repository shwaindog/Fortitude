// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.MapCollections;

public class MapDictNoRevealersStringsUnion<TKey, TValue> : MapDictNoRevealersStringsUnion<MapDictNoRevealersStringsUnion<TKey, TValue>, TKey, TValue>
{
    public MapDictNoRevealersStringsUnion(IReadOnlyDictionary<TKey, TValue>? value, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueFmtString, keyFmtString) { }

    public MapDictNoRevealersStringsUnion(MapDictNoRevealersStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherTyped, valueFmtString, formatFlags) { }

    public MapDictNoRevealersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapDictNoRevealersStringsUnion(
        IReadOnlyDictionary<TKey, MapDictNoRevealersStringsUnion<TKey, TValue>>? nodeColl
      , string? valueFmtString = null
      , string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapDictNoRevealersStringsUnion<TOther, TKey, TValue> : IStringBearer where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapDictNoRevealersStringsUnion(IReadOnlyDictionary<TKey, TValue>? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapDictNoRevealersStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapDictNoRevealersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapDictNoRevealersStringsUnion(IReadOnlyDictionary<TKey, TOther>? nodeColl
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

    private readonly IReadOnlyDictionary<TKey, TOther>? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public KeyValuePair<TKey, TValue>[]? LogPreCollectionField { get; set; }

    public IStringBearer? LogPostField { get; set; }
    public List<KeyValuePair<TKey, TValue>>? LogPostCollectionField { get; set; }

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

public class MapArrayNoRevealersStringsUnion<TKey, TValue> : MapArrayNoRevealersStringsUnion<MapArrayNoRevealersStringsUnion<TKey, TValue>, TKey, TValue>
{
    public MapArrayNoRevealersStringsUnion(KeyValuePair<TKey, TValue>[]? value, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueFmtString, keyFmtString) { }

    public MapArrayNoRevealersStringsUnion(MapArrayNoRevealersStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) : base(otherTyped, valueFmtString, formatFlags) { }

    public MapArrayNoRevealersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapArrayNoRevealersStringsUnion(
        KeyValuePair<TKey, MapArrayNoRevealersStringsUnion<TKey, TValue>>[]? nodeColl
      , string? valueFmtString = null, string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapArrayNoRevealersStringsUnion<TOther, TKey, TValue> : IStringBearer where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapArrayNoRevealersStringsUnion(KeyValuePair<TKey, TValue>[]? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapArrayNoRevealersStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapArrayNoRevealersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapArrayNoRevealersStringsUnion(KeyValuePair<TKey, TOther>[]? nodeColl
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

    private readonly KeyValuePair<TKey, TOther>[]? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public List<KeyValuePair<TKey, TValue>>? LogPreCollectionField { get; set; }

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

public class MapListNoRevealersStringsUnion<TKey, TValue> : MapListNoRevealersStringsUnion<MapListNoRevealersStringsUnion<TKey, TValue>, TKey, TValue>
{
    public MapListNoRevealersStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueFmtString, keyFmtString) { }

    public MapListNoRevealersStringsUnion(MapListNoRevealersStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) : base(otherTyped, valueFmtString, formatFlags) { }

    public MapListNoRevealersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapListNoRevealersStringsUnion(
        List<KeyValuePair<TKey, MapListNoRevealersStringsUnion<TKey, TValue>>>? nodeColl
      , string? valueFmtString = null, string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapListNoRevealersStringsUnion<TOther, TKey, TValue> : IStringBearer where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapListNoRevealersStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapListNoRevealersStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapListNoRevealersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapListNoRevealersStringsUnion(List<KeyValuePair<TKey, TOther>>? nodeColl
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

    private readonly List<KeyValuePair<TKey, TOther>>? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public IReadOnlyDictionary<TKey, TValue>? LogPreCollectionField { get; set; }

    public IStringBearer? LogPostField { get; set; }
    public KeyValuePair<TKey, TValue>[]? LogPostCollectionField { get; set; }

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
               .LogOnlyKeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}

public class MapEnumerateNoRevealersStringsUnion<TKey, TValue> : 
    MapEnumerateNoRevealersStringsUnion<MapEnumerateNoRevealersStringsUnion<TKey, TValue>, TKey, TValue>
{
    public MapEnumerateNoRevealersStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueFmtString, keyFmtString) { }

    public MapEnumerateNoRevealersStringsUnion(MapListNoRevealersStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) : base(otherTyped, valueFmtString, formatFlags) { }

    public MapEnumerateNoRevealersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapEnumerateNoRevealersStringsUnion(
        List<KeyValuePair<TKey, MapEnumerateNoRevealersStringsUnion<TKey, TValue>>>? nodeColl
      , string? valueFmtString = null, string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapEnumerateNoRevealersStringsUnion<TOther, TKey, TValue> : IStringBearer where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapEnumerateNoRevealersStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapEnumerateNoRevealersStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapEnumerateNoRevealersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapEnumerateNoRevealersStringsUnion(List<KeyValuePair<TKey, TOther>>? nodeColl
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

    private readonly List<KeyValuePair<TKey, TOther>>? nodeMap;

    public IStringBearer? LogPreField { get; set; }
    public List<KeyValuePair<TKey, TValue>>? LogPreCollectionField { get; set; }

    public IStringBearer? LogPostField { get; set; }
    public KeyValuePair<TKey, TValue>[]? LogPostCollectionField { get; set; }

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
               .LogOnlyKeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAllEnumerate(mapCollection, valueFormatString, keyFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAllIterate(nameof(LogPostCollectionField), LogPostCollectionField?.GetEnumerator())
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}

public class MapIterateNoRevelersStringsUnion<TKey, TValue> : 
    MapIterateNoRevelersStringsUnion<MapIterateNoRevelersStringsUnion<TKey, TValue>, TKey, TValue>
{
    public MapIterateNoRevelersStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueFmtString, keyFmtString) { }

    public MapIterateNoRevelersStringsUnion(MapListNoRevealersStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) : base(otherTyped, valueFmtString, formatFlags) { }

    public MapIterateNoRevelersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapIterateNoRevelersStringsUnion(
        List<KeyValuePair<TKey, MapIterateNoRevelersStringsUnion<TKey, TValue>>>? nodeColl
      , string? valueFmtString = null, string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapIterateNoRevelersStringsUnion<TOther, TKey, TValue> : IStringBearer where TOther : IStringBearer?
{
    private readonly bool isNode;
    private readonly bool isOtherTypedItem;
    private readonly bool isOtherUntypedItem;

    private readonly TOther         otherTypedItem   = default!;
    private readonly IStringBearer? otherUntypedItem;

    private readonly string?     valueFormatString;
    private readonly FormatFlags otherFormatFlags;
    private readonly string?     keyFormatString;

    public MapIterateNoRevelersStringsUnion(List<KeyValuePair<TKey, TValue>>? value, string? valueFmtString = null
      , string? keyFmtString = null)
    {
        mapCollection     = value?.GetEnumerator();
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapIterateNoRevelersStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapIterateNoRevelersStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapIterateNoRevelersStringsUnion(List<KeyValuePair<TKey, TOther>>? nodeColl
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

    private readonly List<KeyValuePair<TKey, TOther>>? nodeMap;

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
               .LogOnlyKeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAllIterate(mapCollection, valueFormatString, keyFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAllIterate(nameof(LogPostCollectionField), LogPostCollectionField?.GetEnumerator())
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}
