// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

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

    public IFLogStringAppender Add<TToStyle, TStylerType>(TToStyle[]? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.Add(value, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TStylerType>((TToStyle[]?, StringBearerRevealState<TStylerType>) valueTuple) where TToStyle : TStylerType
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.Add(value, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple)
        where TToStyle : TStylerType
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

    public IFLogStringAppender Add<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filter
      , StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TToStyleBase, TStylerType
    {
        wrappedCollectionApppender.Add(value, filter, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TToStyleBase, TStylerType>((TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>,
        StringBearerRevealState<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter
      , StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TToStyleBase, TStylerType
    {
        wrappedCollectionApppender.Add(value, filter, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TToStyleBase, TStylerType>((IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>,
        StringBearerRevealState<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType
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

    public IFLogStringAppender AddEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.AddEnumerate(value, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.AddEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.AddEnumerate(value, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple)
        where TToStyle : TStylerType
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

    public IFLogStringAppender AddMatch<T, TBase>(T[]? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((T[]?, StringBearerRevealState<TBase>) valueTuple) where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(IReadOnlyList<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((IReadOnlyList<T>?, StringBearerRevealState<TBase>) valueTuple)
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
      , StringBearerRevealState<TBase2> stringBearerRevealState)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, StringBearerRevealState<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , StringBearerRevealState<TBase2> stringBearerRevealState) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>
      , StringBearerRevealState<TBase2>) valueTuple) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>(IEnumerable<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>((IEnumerable<T>?, StringBearerRevealState<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>(IEnumerator<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, stringBearerRevealState);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>((IEnumerator<T>?, StringBearerRevealState<TBase>) valueTuple)
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

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>
      , string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>
      , string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null
      , string? keyFormatString = null) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>
      , string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>
      , string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>, string?) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>, string?) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, string?) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>
      , string?) valueTuple) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>
      , string?) valueTuple) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>
      , StringBearerRevealState<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>
      , StringBearerRevealState<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value, StringBearerRevealState<TVBase> valueStyler
      , StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>
      , StringBearerRevealState<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>
      , StringBearerRevealState<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, StringBearerRevealState<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>)
            valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
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
