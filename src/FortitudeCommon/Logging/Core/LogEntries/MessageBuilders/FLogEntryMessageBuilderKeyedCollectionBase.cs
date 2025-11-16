// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder
{
    protected ITheOneString? AppendKeyedCollection<TKey, TValue>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
    (ITheOneString? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
    (ITheOneString? toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue?>>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
        ((IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
        ((IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, IEnumerator<KeyValuePair<TKey, TValue?>>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
        ((IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
        ((IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue?>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue?>[]? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue?>? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
        ((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple, ITheOneString? appender)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue?>[]? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
        ((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple, ITheOneString? appender)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue?>>? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    ((IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IEnumerator<KeyValuePair<TKey, TValue?>>? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    ((IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue?>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue?>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue?>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue?>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, customValueStyler, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, customValueStyler, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, customTypeStyler) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue?>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    ((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue?>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    ((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }
}
