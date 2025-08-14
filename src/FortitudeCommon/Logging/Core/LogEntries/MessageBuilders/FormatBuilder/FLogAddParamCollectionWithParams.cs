using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry
{
    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable =>
        AddValueCollectionParams(value, formatString);
    
    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable =>
        AddValueCollectionParams(value, formatString);
    
    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler)
        where TStruct : struct =>
        AddValueCollectionParams(value, structStyler);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler)
        where TStruct : struct =>
        AddValueCollectionParams(value, structStyler);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddFilteredValueCollectionParams(value, filter, formatString);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddFilteredValueCollectionParams(value, filter, formatString);
    
    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> filter
      , StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddFilteredValueCollectionParams(value, filter, structStyler);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddFilteredValueCollectionParams(value, filter, structStyler);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(T[]? value, string? formatString = null) where T : class =>
        AddObjectCollectionParams(value, formatString);

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>((T[]?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class =>
        AddObjectCollectionParams(value, formatString);

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(T[]? value, OrderedCollectionPredicate<T> filter
      , string? formatString = null) where T : class =>
        AddFilteredObjectCollectionParams(value, filter, formatString);
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(
        (T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(
        (T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter
      , string? formatString = null) where T : class =>
        AddFilteredObjectCollectionParams(value, filter, formatString);
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddValueCollectionParamsEnumerate(value, formatString);

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddValueCollectionParamsEnumerate(value, formatString);

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>(IEnumerable<TStruct>? value
      , StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddValueCollectionParamsEnumerate(value, structStyler);

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>(IEnumerator<TStruct>? value
      , StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddValueCollectionParamsEnumerate(value, structStyler);

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>((IEnumerator<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class =>
        AddObjectCollectionParamsEnumerate(value, formatString);

    [MustUseReturnValue("Use AndFinalObjectCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T>(IEnumerator<T>? value, string? formatString = null)
        where T : class =>
        AddObjectCollectionParamsEnumerate(value, formatString);

    [MustUseReturnValue("Use AndFinalObjectCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AddKeyedCollectionParams(value, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AddKeyedCollectionParams(value, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AddKeyedCollectionParams(value, valueFormatString, keyFormatString);
    
    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AddKeyedCollectionParamsEnumerate(value, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AddKeyedCollectionParamsEnumerate(value, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct =>
    AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct =>
        AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple) where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

}
