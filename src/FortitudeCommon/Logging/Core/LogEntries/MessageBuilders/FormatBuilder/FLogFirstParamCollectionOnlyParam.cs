using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
    public void WithOnlyValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParam<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    
    public void WithOnlyValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParam<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, customTypeStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParam<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, customTypeStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParam<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParam<TFmtStruct>((TFmtStruct[]?
          , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParam<TFmtStruct>((TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    

    public void WithOnlyValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void WithOnlyValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParam<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> filter
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, filter, customTypeStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParam<TStruct>((TStruct[]?
      , OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filter
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, filter, customTypeStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParam<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyObjectCollectionParam<T>(T[]? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyObjectCollectionParam<T>((T[]?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyObjectCollectionParam<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyObjectCollectionParam<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyObjectCollectionParam<T>(T[]? value, OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyObjectCollectionParam<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyObjectCollectionParam<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyObjectCollectionParam<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter, string? formatString = null)
        where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyObjectCollectionParam<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyObjectCollectionParam<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParamEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParamEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParamEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParamEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParamEnumerate<TStruct>(IEnumerable<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollectionEnumerate(value, customTypeStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParamEnumerate<TStruct>((IEnumerable<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyValueCollectionParamEnumerate<TStruct>(IEnumerator<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollectionEnumerate(value, customTypeStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyValueCollectionParamEnumerate<TStruct>((IEnumerator<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyObjectCollectionParamEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyObjectCollectionParamEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyObjectCollectionParamEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyObjectCollectionParamEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    
    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , CustomTypeStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    
    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyFormatString);
        this.EnsureNoMoreTokensAndComplete(value);
    }
    
    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>
      , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        this.EnsureNoMoreTokensAndComplete(value);
    }

    public void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>
      , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().EnsureNoMoreTokensAndComplete(valueTuple);
    }
}
