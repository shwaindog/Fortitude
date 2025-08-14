using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry
{
    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>
        (TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
    
    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>
        (IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>
        (TStruct[]? value, StructStyler<TStruct> structStyler) where TStruct : struct 
    {
        ReplaceTokenWithValueCollection(value, structStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>
        (IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct 
    {
        ReplaceTokenWithValueCollection(value, structStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>
    (TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable 
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((TFmtStruct[]?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable 
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple)
        where TFmtStruct : struct, ISpanFormattable 
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>
    (IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable 
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>((IReadOnlyList<TFmtStruct>?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable 
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple)
        where TFmtStruct : struct, ISpanFormattable 
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>
    (TStruct[]? value, OrderedCollectionPredicate<TStruct> filter
      , StructStyler<TStruct> structStyler) where TStruct : struct 
    {
        ReplaceTokenWithValueCollection(value, filter, structStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((TStruct[]?
      , OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple)
        where TStruct : struct 
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>
    (IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filter
      , StructStyler<TStruct> structStyler) where TStruct : struct 
    {
        ReplaceTokenWithValueCollection(value, filter, structStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamToStringAppender<TStruct>((IReadOnlyList<TStruct>?
      , OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple)
        where TStruct : struct 
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(T[]? value, string? formatString = null) where T : class 
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((T[]?, string?) valueTuple) where T : class 
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class 
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class 
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>
        (T[]? value, OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class 
    {
        ReplaceTokenWithObjectCollection(value, filter, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) 
        where T : class 
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) 
        where T : class 
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter
      , string? formatString = null) where T : class 
    {
        ReplaceTokenWithObjectCollection(value, filter, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class 
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) 
        where T : class 
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TFmtStruct>
        (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable 
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TFmtStruct>
        ((IEnumerable<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable 
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TFmtStruct>
        (IEnumerator<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable 
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TFmtStruct>
        ((IEnumerator<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable 
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>
        (IEnumerable<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct 
    {
        ReplaceTokenWithValueCollectionEnumerate(value, structStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>
        ((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple) where TStruct : struct 
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>
        (IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct 
    {
        ReplaceTokenWithValueCollectionEnumerate(value, structStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyValueCollectionParamEnumerateToStringAppender<TStruct>
        ((IEnumerator<TStruct>?, StructStyler<TStruct>) valueTuple) where TStruct : struct 
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T>
        (IEnumerable<T>? value, string? formatString = null) where T : class 
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T>((IEnumerable<T>?, string?) valueTuple) where T : class 
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T>
        (IEnumerator<T>? value, string? formatString = null) where T : class 
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyObjectCollectionParamEnumerateToStringAppender<T>((IEnumerator<T>?, string?) valueTuple) where T : class 
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null, string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple) 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?,KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?,KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)  where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        return this.ToStringAppender(value, this);
    }

    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString) where TKey : struct where TValue : struct
    {
        AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , StructStyler<TValue>, string?) valueTuple) where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString) where TKey : struct where TValue : struct 
    {
        AddKeyedCollectionParamsEnumerate(value, valueStructStyler, keyFormatString);
        return this.ToStringAppender(value, this);
    }

    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)  where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)  where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)  where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyStructStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)  where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyStructStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)  where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)  
        where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)  
        where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        return this.ToStringAppender(value, this);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    public IFLogStringAppender AfterOnlyKeyedCollectionParamToStringAppender<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)  
        where TKey : struct where TValue : struct 
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToStringAppender(valueTuple, this);
    }
}
