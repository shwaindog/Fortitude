// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
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

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
    (ITheOneString? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null)
        where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
    (ITheOneString? toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple, ITheOneString? appender)
        where TValue : TVBase
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, StringBearerRevealState<TVBase> valueStyler
      , StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, StringBearerRevealState<TVBase> valueStyler
      , StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StringBearerRevealState<TVBase> valueStyler
      , StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple
      , ITheOneString? appender) where TKey : TKBase where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
    (ITheOneString? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, StringBearerRevealState<TVBase> valueStyler
      , StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
    ((IEnumerable<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
    (ITheOneString? toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, StringBearerRevealState<TVBase> valueStyler
      , StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
    ((IEnumerator<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple
      , ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple, ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple
      , ITheOneString? appender) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>) valueTuple
      , ITheOneString? appender) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple
      , ITheOneString? appender) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>) valueTuple
      , ITheOneString? appender) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple
      , ITheOneString? appender)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var (value, filterPredicate, customValueStyler, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, customValueStyler, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>) valueTuple
      , ITheOneString? appender) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var (value, filterPredicate, customTypeStyler) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>) valueTuple
      , ITheOneString? appender)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>) valueTuple
      , ITheOneString? appender)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>) valueTuple
      , ITheOneString? appender)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }
}
