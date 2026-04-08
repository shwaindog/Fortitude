// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.MapCollections;

public class MapDictValueRevealerStringsUnion<TKey, TValue> : 
    MapDictValueRevealerStringsUnion<MapDictNoRevealersStringsUnion<TKey, TValue>, TKey, TValue, TValue>
    where TValue : notnull
{
    public MapDictValueRevealerStringsUnion(IReadOnlyDictionary<TKey, TValue>? value, PalantírReveal<TValue> valueRevealer, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueRevealer, keyFmtString, valueFmtString) { }

    public MapDictValueRevealerStringsUnion(MapDictNoRevealersStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherTyped, valueFmtString, formatFlags) { }

    public MapDictValueRevealerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapDictValueRevealerStringsUnion(
        IReadOnlyDictionary<TKey, MapDictNoRevealersStringsUnion<TKey, TValue>>? nodeColl
      , string? valueFmtString = null
      , string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapDictValueRevealerStringsUnion<TOther, TKey, TValue, TVRevealBase> : IStringBearer
    where TValue : TVRevealBase?
    where TVRevealBase :  notnull
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

    private PalantírReveal<TVRevealBase>? valueRevealer; 

    public MapDictValueRevealerStringsUnion(IReadOnlyDictionary<TKey, TValue>? value, PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFmtString = null, string? valueFmtString = null)
    {
        this.valueRevealer = valueRevealer; 
        
        mapCollection      = value;
        valueFormatString  = valueFmtString;
        keyFormatString    = keyFmtString;
    }

    public MapDictValueRevealerStringsUnion(TOther otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapDictValueRevealerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapDictValueRevealerStringsUnion(IReadOnlyDictionary<TKey, TOther>? nodeColl
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
        if (valueRevealer == null) throw new ArgumentException("Must have a value revealer");
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAll(mapCollection, valueRevealer, keyFormatString, valueFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}

public class MapArrayValueRevealerStringsUnion<TKey, TValue> : 
    MapArrayValueRevealerStringsUnion<MapArrayValueRevealerStringsUnion<TKey, TValue>, TKey, TValue, TValue>
    where TValue : notnull
{
    public MapArrayValueRevealerStringsUnion(KeyValuePair<TKey, TValue>[]? value, PalantírReveal<TValue> valueRevealer, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueRevealer, keyFmtString, valueFmtString) { }

    public MapArrayValueRevealerStringsUnion(MapArrayValueRevealerStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherTyped, valueFmtString, formatFlags) { }

    public MapArrayValueRevealerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapArrayValueRevealerStringsUnion(
        KeyValuePair<TKey, MapArrayValueRevealerStringsUnion<TKey, TValue>>[]? nodeColl
      , string? valueFmtString = null, string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapArrayValueRevealerStringsUnion<TOther, TKey, TValue, TVRevealBase> : IStringBearer 
    where TValue : TVRevealBase?
    where TVRevealBase :  notnull
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

    private PalantírReveal<TVRevealBase>? valueRevealer; 

    public MapArrayValueRevealerStringsUnion(KeyValuePair<TKey, TValue>[]? value, PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFmtString = null, string? valueFmtString = null)
    {
        this.valueRevealer = valueRevealer; 
        
        mapCollection      = value;
        valueFormatString  = valueFmtString;
        keyFormatString    = keyFmtString;
    }

    public MapArrayValueRevealerStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapArrayValueRevealerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapArrayValueRevealerStringsUnion(KeyValuePair<TKey, TOther>[]? nodeColl
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
        if (valueRevealer == null) throw new ArgumentException("Must have a value revealer");
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAll(mapCollection, valueRevealer, keyFormatString, valueFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}

public class MapListValueRevealerStringsUnion<TKey, TValue> : 
    MapListValueRevealerStringsUnion<MapListValueRevealerStringsUnion<TKey, TValue>, TKey, TValue, TValue>
    where TValue : notnull
{
    public MapListValueRevealerStringsUnion(List<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TValue> valueRevealer, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueRevealer, keyFmtString, valueFmtString) { }

    public MapListValueRevealerStringsUnion(MapListValueRevealerStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherTyped, valueFmtString, formatFlags) { }

    public MapListValueRevealerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapListValueRevealerStringsUnion(
        List<KeyValuePair<TKey, MapListValueRevealerStringsUnion<TKey, TValue>>>? nodeColl
      , string? valueFmtString = null, string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapListValueRevealerStringsUnion<TOther, TKey, TValue, TVRevealBase> : IStringBearer
    where TValue : TVRevealBase?
    where TVRevealBase :  notnull
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

    private PalantírReveal<TVRevealBase>? valueRevealer; 

    public MapListValueRevealerStringsUnion(List<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TVRevealBase> valueRevealer
      , string? keyFmtString = null, string? valueFmtString = null)
    {
        this.valueRevealer = valueRevealer; 
        
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapListValueRevealerStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapListValueRevealerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapListValueRevealerStringsUnion(List<KeyValuePair<TKey, TOther>>? nodeColl
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
    public IReadOnlyDictionary<TKey, TValue>? LogPostCollectionField { get; set; }

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isOtherUntypedItem)
            return otherUntypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isOtherTypedItem)
            return otherTypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isNode) return tos.StartKeyedCollectionType(this).AddAll(nodeMap, valueFormatString, keyFormatString).Complete();
        if (valueRevealer == null) throw new ArgumentException("Must have a value revealer");
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAll(mapCollection, valueRevealer, keyFormatString, valueFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}

public class MapEnumerateValueRevealerStringsUnion<TKey, TValue> : 
    MapEnumerateValueRevealerStringsUnion<MapEnumerateValueRevealerStringsUnion<TKey, TValue>, TKey, TValue, TValue>
    where TValue : notnull
{
    public MapEnumerateValueRevealerStringsUnion(List<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TValue> valueRevealer
      , string? keyFmtString = null, string? valueFmtString = null) : base(value, valueRevealer, keyFmtString, valueFmtString) { }

    public MapEnumerateValueRevealerStringsUnion(MapEnumerateValueRevealerStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherTyped, valueFmtString, formatFlags) { }

    public MapEnumerateValueRevealerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapEnumerateValueRevealerStringsUnion(
        List<KeyValuePair<TKey, MapEnumerateValueRevealerStringsUnion<TKey, TValue>>>? nodeColl
      , string? valueFmtString = null, string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}

public class MapEnumerateValueRevealerStringsUnion<TOther, TKey, TValue, TVRevealBase> : IStringBearer
    where TValue : TVRevealBase?
    where TVRevealBase :  notnull
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

    private PalantírReveal<TVRevealBase>? valueRevealer; 

    public MapEnumerateValueRevealerStringsUnion(List<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TVRevealBase> valueRevealer
      , string? valueFmtString = null, string? keyFmtString = null)
    {
        this.valueRevealer = valueRevealer; 
        
        mapCollection     = value;
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapEnumerateValueRevealerStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapEnumerateValueRevealerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapEnumerateValueRevealerStringsUnion(List<KeyValuePair<TKey, TOther>>? nodeColl
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
    public IReadOnlyDictionary<TKey, TValue>? LogPostCollectionField { get; set; }

    public AppendSummary RevealState(ITheOneString tos)
    {
        if (isOtherUntypedItem)
            return otherUntypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isOtherTypedItem)
            return otherTypedItem!.RevealState(tos.WithNextCallValueFormatString(valueFormatString).WithNextCallFormatFlags(otherFormatFlags));
        if (isNode) return tos.StartKeyedCollectionType(this).AddAll(nodeMap, valueFormatString, keyFormatString).Complete();
        if (valueRevealer == null) throw new ArgumentException("Must have a value revealer");
        return tos
               .StartKeyedCollectionType(this)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPreField), LogPreField)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAllEnumerateValueRevealer(mapCollection, valueRevealer, keyFormatString, valueFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}

public class MapIterateValueRevelerStringsUnion<TKey, TValue> : 
    MapIterateValueRevelerStringsUnion<MapIterateValueRevelerStringsUnion<TKey, TValue>, TKey, TValue, TValue>
    where TValue : notnull
{
    public MapIterateValueRevelerStringsUnion(List<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TValue> valueRevealer, string? valueFmtString = null
      , string? keyFmtString = null) : base(value, valueRevealer, keyFmtString, valueFmtString) { }

    public MapIterateValueRevelerStringsUnion(MapIterateValueRevelerStringsUnion<TKey, TValue> otherTyped, string? valueFmtString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherTyped, valueFmtString, formatFlags) { }

    public MapIterateValueRevelerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        : base(otherUntyped, valueFmtString, formatFlags) { }

    public MapIterateValueRevelerStringsUnion(
        List<KeyValuePair<TKey, MapIterateValueRevelerStringsUnion<TKey, TValue>>>? nodeColl
      , string? valueFmtString = null, string? keyFmtString = null) : base(nodeColl, valueFmtString, keyFmtString) { }
}
public class MapIterateValueRevelerStringsUnion<TOther, TKey, TValue, TVRevealBase> : IStringBearer
    where TValue : TVRevealBase?
    where TVRevealBase :  notnull
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

    private PalantírReveal<TVRevealBase>? valueRevealer; 

    public MapIterateValueRevelerStringsUnion(List<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TVRevealBase> valueRevealer
       , string? keyFmtString = null, string? valueFmtString = null)
    {
        this.valueRevealer = valueRevealer; 

        mapCollection     = value?.GetEnumerator();
        valueFormatString = valueFmtString;
        keyFormatString   = keyFmtString;
    }

    public MapIterateValueRevelerStringsUnion(TOther otherTyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherTypedItem  = true;
        otherTypedItem    = otherTyped;
        mapCollection     = null;
        valueFormatString = valueFmtString;
        otherFormatFlags  = formatFlags;
        keyFormatString   = null;
    }

    public MapIterateValueRevelerStringsUnion(IStringBearer otherUntyped, string? valueFmtString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        isOtherUntypedItem = true;
        otherUntypedItem   = otherUntyped;
        mapCollection      = null;
        valueFormatString  = valueFmtString;
        otherFormatFlags   = formatFlags;
        keyFormatString    = null;
    }

    public MapIterateValueRevelerStringsUnion(List<KeyValuePair<TKey, TOther>>? nodeColl
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
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPreCollectionField), LogPreCollectionField)
               .AddAllIterate(mapCollection, valueFormatString, keyFormatString)
               .LogOnlyKeyedCollectionField.WhenNonNullAddAll(nameof(LogPostCollectionField), LogPostCollectionField)
               .LogOnlyField.WhenNonNullReveal(nameof(LogPostField), LogPostField)
               .Complete();
    }
}
