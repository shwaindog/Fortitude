using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry
{

    public void AndFinalValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }
    
    public void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        ReplaceTokenWithValueCollection(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        ReplaceTokenWithValueCollection(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?
      , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        ReplaceTokenWithValueCollection(value, filter, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TToStyle, TToStyleBase, TStylerType>((TToStyle[]?
      , OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParam<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        ReplaceTokenWithValueCollection(value, filter, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParam<TToStyle, TToStyleBase, TStylerType>(
        (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParamEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParamEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParamEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParamEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParamEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        ReplaceTokenWithValueCollectionEnumerate(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParamEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalValueCollectionParamEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        ReplaceTokenWithValueCollectionEnumerate(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalValueCollectionParamEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T>(T[]? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T>((T[]?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParamEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParamEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParamEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParamEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalObjectCollectionParam<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        ReplaceTokenWithObjectCollection(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        ReplaceTokenWithObjectCollection(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        ReplaceTokenWithObjectCollection(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>
      ,  CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParam<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        ReplaceTokenWithObjectCollection(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParam<T, TBase1, TBase2>((IReadOnlyList<T>?, 
        OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalObjectCollectionParamEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParamEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalObjectCollectionParamEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, customTypeStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalObjectCollectionParamEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>
      , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>
      , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>
      , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?, 
        CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>
      , string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }
    
    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        this.CallEnsureNoMoreTokensAndComplete(value);
    }

    public void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        ReplaceTokenNumber().CallEnsureNoMoreTokensAndComplete(valueTuple);
    }
    
    
}
