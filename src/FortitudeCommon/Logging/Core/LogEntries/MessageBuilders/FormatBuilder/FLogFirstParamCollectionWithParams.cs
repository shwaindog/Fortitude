using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
        [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable =>
        AddValueCollectionParams(value, formatString);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>
        (IReadOnlyList<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddValueCollectionParams(value, formatString);
    
    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler)
        where TStruct : struct =>
        AddValueCollectionParams(value, structStyler);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>
        (IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddValueCollectionParams(value, structStyler);

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>
    (TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddFilteredValueCollectionParams(value, filter, formatString);
    

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>
    (IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddFilteredValueCollectionParams(value, filter, formatString);
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>
        (TStruct[]? value, OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddFilteredValueCollectionParams(value, filter, structStyler);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>
    (IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filter
      , StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddFilteredValueCollectionParams(value, filter, structStyler);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(T[]? value, string? formatString = null)
        where T : class =>
        AddObjectCollectionParams(value, formatString);

    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>((T[]?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>
        (IReadOnlyList<T>? value, string? formatString = null) where T : class =>
        AddObjectCollectionParams(value, formatString);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>
        (T[]? value, OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class =>
        AddFilteredObjectCollectionParams(value, filter, formatString);


    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(
        (T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(
        (T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter
      , string? formatString = null) where T : class =>
        AddFilteredObjectCollectionParams(value, filter, formatString);
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TFmtStruct>
        (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddValueCollectionParamsEnumerate(value, formatString);

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TFmtStruct>
        (IEnumerator<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AddValueCollectionParamsEnumerate(value, formatString);

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>
        (IEnumerable<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddValueCollectionParamsEnumerate(value, structStyler);

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>(
        (IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>
        (IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddValueCollectionParamsEnumerate(value, structStyler);

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>(
        (IEnumerator<TStruct>?, StructStyler<TStruct>) valueTuple) where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T>
        (IEnumerable<T>? value, string? formatString = null) where T : class =>
        AddObjectCollectionParamsEnumerate(value, formatString);

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T>
        (IEnumerator<T>? value, string? formatString = null) where T : class =>
        AddObjectCollectionParamsEnumerate(value, formatString);

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AddKeyedCollectionParams(value, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AddKeyedCollectionParams(value, valueFormatString, keyFormatString);


    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AddKeyedCollectionParams(value, valueFormatString, keyFormatString);
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AddKeyedCollectionParamsEnumerate(value, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AddKeyedCollectionParamsEnumerate(value, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple)
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple) where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple) where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyFormatString);

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct => 
    AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyFormatString);

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct => 
        AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyFormatString);

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParams(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStructStyler, keyStructStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TKey : struct where TValue : struct
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

}
