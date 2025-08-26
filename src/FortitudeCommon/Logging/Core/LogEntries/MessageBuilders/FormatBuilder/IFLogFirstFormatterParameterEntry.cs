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
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

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
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
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
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

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
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>((IEnumerable<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>(IEnumerator<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TStruct>((IEnumerator<TStruct>? , CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple)  where T : class, TBase where TBase : class;

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
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase >(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>
      , CustomTypeStyler<TBase2>) valueTuple) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

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
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) 
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) 
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple) 
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) 
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) 
        where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TValue : TVBase where TKey : TKBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void WithOnlyMatchParam<T>(T value);
    void WithOnlyParam(bool? value);
    void WithOnlyParam(bool value);
    void WithOnlyParam<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;
    void WithOnlyParam<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;
    void WithOnlyParam<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;
    void WithOnlyParam<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;
    void WithOnlyParam<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;
    void WithOnlyParam<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;
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

    void WithOnlyValueCollectionParam<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

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
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>(
        (TStruct[]? , OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    void WithOnlyValueCollectionParam<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    void WithOnlyValueCollectionParamEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParamEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParamEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParamEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    void WithOnlyValueCollectionParamEnumerate<TStruct>((IEnumerable<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct;

    void WithOnlyValueCollectionParamEnumerate<TStruct>(IEnumerator<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    void WithOnlyValueCollectionParamEnumerate<TStruct>((IEnumerator<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct;

    void WithOnlyObjectCollectionParam<T>(T[]? value, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParam<T>((T[]?, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParam<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParam<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParam<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParamEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParamEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParamEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class;

    void WithOnlyObjectCollectionParamEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class;

    void WithOnlyObjectCollectionParam<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParam<T, TBase1, TBase2>(T[]? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void WithOnlyObjectCollectionParam<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void WithOnlyObjectCollectionParam<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void WithOnlyObjectCollectionParam<T, TBase1, TBase2>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void WithOnlyObjectCollectionParamEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParamEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParamEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    void WithOnlyObjectCollectionParamEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

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
    
    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null)
        where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;
    
    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;
    
    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;
    
    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;
    
    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple)where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>
      , CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>
      , CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>
      , CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>
      , CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void WithOnlyKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;


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
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyParamToStringAppender<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

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
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
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
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((TStruct[]?
      , OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((IReadOnlyList<TStruct>?
      , OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

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
      , CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>((IEnumerable<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>(IEnumerator<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>((IEnumerator<TStruct>?
      , CustomTypeStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>((T[]?
      , OrderedCollectionPredicate<TBase> , string?) valueTuple)  where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>((T[]?
      , OrderedCollectionPredicate<TBase>) valueTuple)  where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>((IReadOnlyList<T>?
      , OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

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

    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase1, TBase2>(T[]? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase1, TBase2>((T[]?
      , OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple)  
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T, TBase>(IEnumerable<T>? value
      , CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T, TBase>(
        (IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T, TBase>(
        IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;
    
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
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString) where TValue : TVBase; 

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase ;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString) where TValue : TVBase;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase ;

    [MustUseReturnValue("Use withOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TKBase, TVBase>(
        IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TKBase, TVBase>(
        IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string? , string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
        IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
        IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>
          , CustomTypeStyler<TKBase2>) valueTuple) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    // ReSharper restore UnusedMember.Global
}
