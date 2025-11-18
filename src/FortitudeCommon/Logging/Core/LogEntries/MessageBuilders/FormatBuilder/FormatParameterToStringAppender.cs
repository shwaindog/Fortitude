// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public class FormatParameterToStringAppender : RecyclableObject, IStringAppenderCollectionBuilder
{
    private IStringBuilder          formattedString = null!;
    private FLogEntry               logEntry        = null!;
    private Action<IStringBuilder?> onComplete      = null!;

    private IFinalCollectionAppend wrappedCollectionApppender = null!;

    public FormatParameterToStringAppender Initialize(FLogEntry fLogEntry, Action<IStringBuilder?> callWhenComplete,
        IFinalCollectionAppend finalCollectionAppend, IStringBuilder stringBuilder)
    {
        wrappedCollectionApppender = finalCollectionAppend;
        formattedString            = stringBuilder;
        logEntry                   = fLogEntry;
        onComplete                 = callWhenComplete;

        return this;
    }

    public IFLogStringAppender Add<TStyleObj>(TStyleObj[]? value) where TStyleObj : class, IStringBearer
    {
        wrappedCollectionApppender.Add(value);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TStyleObj>(IReadOnlyList<TStyleObj>? value) where TStyleObj : class, IStringBearer
    {
        wrappedCollectionApppender.Add(value);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt>(TFmt[]? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        wrappedCollectionApppender.AddFormat(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt>((TFmt[]?, string?) valueTuple) where TFmt : ISpanFormattable
    {
        wrappedCollectionApppender.AddFormat(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt>(IReadOnlyList<TFmt>? value, string? formatString = null)
        where TFmt : ISpanFormattable
    {
        wrappedCollectionApppender.AddFormat(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt>((IReadOnlyList<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable
    {
        wrappedCollectionApppender.AddFormat(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TCloaked, TRevealBase>(TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.Add(value, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TCloaked, TRevealBase>((TCloaked?[]?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TCloaked, TRevealBase>(IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.Add(value, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TCloaked, TRevealBase>((IReadOnlyList<TCloaked?>?, PalantírReveal<TRevealBase>) valueTuple)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TStyleObj>(TStyleObj[]? value, OrderedCollectionPredicate<TStyleObj> filter)
        where TStyleObj : class, IStringBearer
    {
        wrappedCollectionApppender.Add(value, filter);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TStyleObj>(IReadOnlyList<TStyleObj>? value, OrderedCollectionPredicate<TStyleObj> filter)
        where TStyleObj : class, IStringBearer
    {
        wrappedCollectionApppender.Add(value, filter);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt, TBase>(TFmt[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where TFmt : ISpanFormattable, TBase
    {
        wrappedCollectionApppender.AddFormat(value, filter, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt, TBase>((TFmt[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where TFmt : ISpanFormattable, TBase
    {
        wrappedCollectionApppender.AddFormat(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt, TBase>((TFmt[]?, OrderedCollectionPredicate<TBase>) valueTuple)
        where TFmt : ISpanFormattable, TBase
    {
        wrappedCollectionApppender.AddFormat(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt, TBase>(IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null)
        where TFmt : ISpanFormattable, TBase
    {
        wrappedCollectionApppender.AddFormat(value, filter, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt, TBase>((IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where TFmt : ISpanFormattable, TBase
    {
        wrappedCollectionApppender.AddFormat(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormat<TFmt, TBase>((IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>) valueTuple)
        where TFmt : ISpanFormattable, TBase
    {
        wrappedCollectionApppender.AddFormat(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TCloaked, TFilterBase, TRevealBase>(TCloaked?[]? value, OrderedCollectionPredicate<TFilterBase> filter
      , PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.Add(value, filter, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TCloaked, TFilterBase, TRevealBase>((TCloaked?[]?, OrderedCollectionPredicate<TFilterBase>,
        PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TCloaked, TFilterBase, TRevealBase>(IReadOnlyList<TCloaked?>? value
      , OrderedCollectionPredicate<TFilterBase> filter
      , PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.Add(value, filter, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TCloaked, TFilterBase, TRevealBase>((IReadOnlyList<TCloaked>?, OrderedCollectionPredicate<TFilterBase>,
        PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TStyleObj>(IEnumerable<TStyleObj>? value) where TStyleObj : class, IStringBearer
    {
        wrappedCollectionApppender.AddEnumerate(value);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TStyleObj>(IEnumerator<TStyleObj>? value) where TStyleObj : class, IStringBearer
    {
        wrappedCollectionApppender.AddEnumerate(value);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormatEnumerate<TFmt>(IEnumerable<TFmt>? value, string? formatString = null)
        where TFmt : ISpanFormattable
    {
        wrappedCollectionApppender.AddFormatEnumerate(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormatEnumerate<TFmt>((IEnumerable<TFmt>?, string?)
        valueTuple) where TFmt : ISpanFormattable
    {
        wrappedCollectionApppender.AddFormatEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormatEnumerate<TFmt>(IEnumerator<TFmt>? value, string? formatString = null)
        where TFmt : ISpanFormattable
    {
        wrappedCollectionApppender.AddFormatEnumerate(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddFormatEnumerate<TFmt>((IEnumerator<TFmt>?, string?) valueTuple)
        where TFmt : ISpanFormattable
    {
        wrappedCollectionApppender.AddFormatEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TCloaked, TRevealBase>(IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.AddEnumerate(value, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TCloaked, TRevealBase>((IEnumerable<TCloaked?>?, PalantírReveal<TRevealBase>) valueTuple)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.AddEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TCloaked, TRevealBase>(IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.AddEnumerate(value, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TCloaked, TRevealBase>((IEnumerator<TCloaked>?, PalantírReveal<TRevealBase>) valueTuple)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        wrappedCollectionApppender.AddEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T>(T[]? value, string? formatString = null) where T : class
    {
        wrappedCollectionApppender.AddMatch(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T>((T[]?, string?) valueTuple) where T : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        wrappedCollectionApppender.AddMatch(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(T[]? value, PalantírReveal<TBase> palantírReveal)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((T[]?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(IReadOnlyList<T>? value, PalantírReveal<TBase> palantírReveal)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((IReadOnlyList<T>?, PalantírReveal<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter
      , PalantírReveal<TBase2> palantírReveal)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, PalantírReveal<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , PalantírReveal<TBase2> palantírReveal) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>
      , PalantírReveal<TBase2>) valueTuple) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>(IEnumerable<T>? value, PalantírReveal<TBase> palantírReveal)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>((IEnumerable<T>?, PalantírReveal<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>(IEnumerator<T>? value, PalantírReveal<TBase> palantírReveal)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, palantírReveal);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>((IEnumerator<T>?, PalantírReveal<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyed(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyed(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyed(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKRevealBase, TVRevealBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKRevealBase where TValue : TVRevealBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKRevealBase, TVRevealBase>
      , string?, string?) valueTuple) where TKey : TKRevealBase where TValue : TVRevealBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKRevealBase, TVRevealBase>
      , string?) valueTuple) where TKey : TKRevealBase where TValue : TVRevealBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?
      , KeyValuePredicate<TKRevealBase, TVRevealBase>) valueTuple) where TKey : TKRevealBase where TValue : TVRevealBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKRevealBase, TVRevealBase> filterPredicate, string? valueFormatString = null
      , string? keyFormatString = null) where TKey : TKRevealBase where TValue : TVRevealBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKRevealBase, TVRevealBase>
      , string?, string?) valueTuple) where TKey : TKRevealBase where TValue : TVRevealBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKRevealBase, TVRevealBase>
      , string?) valueTuple) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?
      , KeyValuePredicate<TKRevealBase, TVRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TFilterBase, TVFilterBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , KeyValuePredicate<TFilterBase, TVFilterBase>) valueTuple) 
        where TKey : TFilterBase 
        where TValue : TVFilterBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVRevealBase>((IEnumerable<KeyValuePair<TKey, TValue?>>?
      , PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVRevealBase>((IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>
      , string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>
      , PalantírReveal<TKRevealBase>) valueTuple) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, valueRevealer, keyRevealer);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>
      , PalantírReveal<TKRevealBase>) valueTuple) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>((IEnumerable<KeyValuePair<TKey, TValue?>>?
      , PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>((IEnumerator<KeyValuePair<TKey, TValue?>>?
      , PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue?>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    private IFLogStringAppender ConvertToStringAppender()
    {
        var styleTypeStringAppender = (Recycler?.Borrow<TheOneString>() ?? new TheOneString(logEntry.Style))
            .Initialize(formattedString, logEntry.Style);

        var convertedToStringAppender = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(logEntry, styleTypeStringAppender, onComplete);

        DecrementRefCount();
        return convertedToStringAppender;
    }
}
