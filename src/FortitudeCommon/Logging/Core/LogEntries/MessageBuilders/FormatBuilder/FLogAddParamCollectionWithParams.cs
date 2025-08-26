using FortitudeCommon.Types.StyledToString;
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
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct =>
        AddValueCollectionParams(value, customTypeStyler);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct =>
        AddValueCollectionParams(value, customTypeStyler);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
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
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        AddFilteredValueCollectionParams(value, filter, customTypeStyler);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        AddFilteredValueCollectionParams(value, filter, customTypeStyler);

    [MustUseReturnValue("Use AndFinalValueCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
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
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        AddValueCollectionParamsEnumerate(value, customTypeStyler);

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>((IEnumerable<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>(IEnumerator<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        AddValueCollectionParamsEnumerate(value, customTypeStyler);

    [MustUseReturnValue("Use AndFinalValueCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>((IEnumerator<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
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
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where T : class, TBase where TBase : class =>
        AddFilteredObjectCollectionParams(value, filter, formatString);
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(
        (T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(
        (T[]?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where T : class, TBase where TBase : class =>
        AddFilteredObjectCollectionParams(value, filter, formatString);
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
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
    
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class =>
        AddObjectCollectionParams(value, customTypeStyler);

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class =>
        AddObjectCollectionParams(value, customTypeStyler);

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) where T : class, TBase1, TBase2 where TBase1 : class  where TBase2 : class =>
        AddFilteredObjectCollectionParams(value, filter, customTypeStyler);
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase1, TBase2>(
        (T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class  where TBase2 : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) where T : class, TBase1, TBase2 where TBase1 : class  where TBase2 : class =>
        AddFilteredObjectCollectionParams(value, filter, customTypeStyler);
    
    [MustUseReturnValue("Use AndFinalObjectCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class  where TBase2 : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalObjectCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class =>
        AddObjectCollectionParamsEnumerate(value, customTypeStyler);

    [MustUseReturnValue("Use AndFinalObjectCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalObjectCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class =>
        AddObjectCollectionParamsEnumerate(value, customTypeStyler);

    [MustUseReturnValue("Use AndFinalObjectCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class
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
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase =>
    AddKeyedCollectionParamsEnumerate(value, valueStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase => AddKeyedCollectionParamsEnumerate(value, valueStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParamsEnumerate(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParamsEnumerate(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyFormatString);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyStyler);

    [MustUseReturnValue("Use AndFinalKeyedCollection to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

}
