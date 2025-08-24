using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender
{
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollection(MessageStsa, value, formatString).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollection(MessageStsa, value, formatString).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct =>
        AppendValueCollection(MessageStsa, value, customTypeStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct =>
        AppendValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct =>
        AppendValueCollection(MessageStsa, value, customTypeStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct =>
        AppendValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(MessageStsa, value, filter, formatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable  =>
        AppendFilteredValueCollection(MessageStsa, value, filter, formatString).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> filter
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct  =>
        AppendFilteredValueCollection(MessageStsa, value, filter, customTypeStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        AppendFilteredValueCollection(MessageStsa, value, filter, customTypeStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>(T[]? value, string? formatString = null) where T : class =>
        AppendObjectCollection(MessageStsa, value, formatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>((T[]?, string?) valueTuple) where T : class =>
        AppendObjectCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollection(MessageStsa, value, formatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class =>
        AppendObjectCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>(T[]? value, OrderedCollectionPredicate<T> filter, string? formatString = null) 
        where T : class =>
        AppendFilteredObjectCollection(MessageStsa, value, filter, formatString).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter
      , string? formatString = null) where T : class =>
        AppendFilteredObjectCollection(MessageStsa, value, filter, formatString).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionLine<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerateLine<TFmtStruct>(IEnumerable<TFmtStruct>? value
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollectionEnumerate(MessageStsa, value, formatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerateLine<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerateLine<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollectionEnumerate(MessageStsa, value, formatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerateLine<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerateLine<TStruct>(IEnumerable<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        AppendValueCollectionEnumerate(MessageStsa, value, customTypeStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerateLine<TStruct>((IEnumerable<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct =>
        AppendValueCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerateLine<TStruct>(IEnumerator<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        AppendValueCollectionEnumerate(MessageStsa, value, customTypeStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerateLine<TStruct>((IEnumerator<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct =>
        AppendValueCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerateLine<T>(IEnumerable<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollectionEnumerate(MessageStsa, value, formatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerateLine<T>((IEnumerable<T>?, string?) valueTuple) where T : class =>
        AppendObjectCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerateLine<T>(IEnumerator<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollectionEnumerate(MessageStsa, value, formatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerateLine<T>((IEnumerator<T>?, string?) valueTuple) where T : class=>
        AppendObjectCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollection(MessageStsa, value, valueFormatString, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null)  =>
        AppendKeyedCollection(MessageStsa, value, valueFormatString, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null)  =>
        AppendKeyedCollection(MessageStsa, value, valueFormatString, keyFormatString).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null)  =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueFormatString, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueFormatString, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueFormatString, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple) =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple) =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple) =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)  =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueFormatString, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple) =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple) =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple) =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)  =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueFormatString, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple) =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)=>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple)=>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct  =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct  =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);
    

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueStyler, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct  =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueStyler, keyFormatString).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct =>
        AppendKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueStyler, keyStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueStyler, keyStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyStyler).AppendLine(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).AppendLine(this);

}
