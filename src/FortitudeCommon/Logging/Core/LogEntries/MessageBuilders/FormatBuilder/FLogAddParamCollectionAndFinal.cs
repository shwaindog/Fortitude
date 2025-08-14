using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry
{

    public void AndFinalValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    
    public void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, structStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, structStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> filter
      , StructStyler<TStruct> structStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, filter, structStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TStruct>((TStruct[]?
      , OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filter
      , StructStyler<TStruct> structStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, filter, structStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T>(T[]? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T>((T[]?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T>(T[]? value, OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter, string? formatString = null)
        where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParamEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParamEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParamEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParamEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParamEnumerate<TStruct>(IEnumerable<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollectionEnumerate(value, structStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParamEnumerate<TStruct>((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParamEnumerate<TStruct>(IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollectionEnumerate(value, structStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParamEnumerate<TStruct>((IEnumerator<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParamEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParamEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParamEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParamEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    
    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, string?) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyStructStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyStructStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
}
