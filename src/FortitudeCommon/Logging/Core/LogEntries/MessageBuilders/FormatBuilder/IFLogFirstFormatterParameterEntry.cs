using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public interface IFLogFirstFormatterParameterEntry : IFLogFormatterParameterEntry
{
    // ReSharper disable UnusedMember.Global


    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithMatchParams<T>(T value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(bool? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(bool value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((string?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((string?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(object? value);

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TFmtStruct>
    (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>(IEnumerator<TStruct>? value
      , StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>((IEnumerator<TStruct>? , StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T>(IEnumerator<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamsEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void WithOnlyMatchParam<T>(T value);
    void WithOnlyParam(bool? value);
    void WithOnlyParam(bool value);
    void WithOnlyParam<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;
    void WithOnlyParam<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;
    void WithOnlyParam<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void WithOnlyParam<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    void WithOnlyParam<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void WithOnlyParam<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    void WithOnlyParam(ReadOnlySpan<char> value);
    void WithOnlyParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(string? value);
    void WithOnlyParam((string?, int) valueTuple);
    void WithOnlyParam((string?, int, int) valueTuple);
    void WithOnlyParam(string? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(char[]? value);
    void WithOnlyParam((char[]?, int) valueTuple);
    void WithOnlyParam((char[]?, int, int) valueTuple);
    void WithOnlyParam(char[]? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(ICharSequence? value);
    void WithOnlyParam((ICharSequence?, int) valueTuple);
    void WithOnlyParam((ICharSequence?, int, int) valueTuple);
    void WithOnlyParam(ICharSequence? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(StringBuilder? value);
    void WithOnlyParam((StringBuilder?, int) valueTuple);
    void WithOnlyParam((StringBuilder?, int, int) valueTuple);
    void WithOnlyParam(StringBuilder? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(IStyledToStringObject? value);
    void WithOnlyParam(object? value);

    void WithOnlyValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    void WithOnlyValueCollectionParam<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TFmtStruct>((TFmtStruct[]?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TFmtStruct>((TFmtStruct[]?
      , OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?
      , OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParam<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>(
        (TStruct[]? , OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    void WithOnlyObjectCollectionParam<T>(T[]? value, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParam<T>((T[]?, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParam<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParam<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParam<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParam<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParam<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    void WithOnlyObjectCollectionParam<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParam<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParam<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    void WithOnlyValueCollectionParamEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParamEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParamEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParamEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParamEnumerate<TStruct>((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    void WithOnlyValueCollectionParamEnumerate<TStruct>(IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    void WithOnlyValueCollectionParamEnumerate<TStruct>((IEnumerator<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    void WithOnlyObjectCollectionParamEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParamEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParamEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParamEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);
    
    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple);
    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);
    
    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);
    
    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);
    
    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;
    
    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void WithOnlyKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>
      , StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;


    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamMatchToStringAppender<T>(T value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(bool? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(bool value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(string? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((string?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((string?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((char[]?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(IStyledToStringObject? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender(object? value);
    
    
    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;
    
    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;
    
    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((TFmtStruct[]?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((TFmtStruct[]?
      , OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((IReadOnlyList<TFmtStruct>?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((IReadOnlyList<TFmtStruct>?
      , OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((TStruct[]?
      , OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((IReadOnlyList<TStruct>?
      , OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((T[]?
      , OrderedCollectionPredicate<T> , string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((T[]?
      , OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?
      , OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>(IEnumerable<TStruct>? value
      , StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>(IEnumerator<TStruct>? value
      , StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>((IEnumerator<TStruct>?
      , StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T>(IEnumerator<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string? , string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    // ReSharper restore UnusedMember.Global
}
