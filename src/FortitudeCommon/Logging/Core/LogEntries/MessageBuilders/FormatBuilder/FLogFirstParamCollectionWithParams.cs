using FortitudeCommon.Types.StyledToString;
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
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TToStyle, TStylerType>(TToStyle[]? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        AddValueCollectionParams(value, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TToStyle, TStylerType>((TToStyle[]?
      , CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TToStyle, TStylerType>
        (IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        AddValueCollectionParams(value, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?
      , CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType
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
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TToStyle, TToStyleBase, TStylerType>
        (TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TToStyleBase, TStylerType =>
        AddFilteredValueCollectionParams(value, filter, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TToStyle, TToStyleBase, TStylerType>(
        (TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TToStyle, TToStyleBase, TStylerType>
    (IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType =>
        AddFilteredValueCollectionParams(value, filter, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParams<TToStyle, TToStyleBase, TStylerType>(
        (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TToStyleBase, TStylerType
    {
        FormatSb.Clear();
        AppendFilteredValueCollection(valueTuple, FormatStsa!);
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
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TToStyle, TStylerType>
        (IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        AddValueCollectionParamsEnumerate(value, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TToStyle, TStylerType>(
        (IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TToStyle, TStylerType>
        (IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        AddValueCollectionParamsEnumerate(value, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyValueCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithValueCollectionParamsEnumerate<TToStyle, TStylerType>(
        (IEnumerator<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType
    {
        FormatSb.Clear();
        AppendValueCollectionEnumerate(valueTuple, FormatStsa!);
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
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>
        (T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase  where TBase : class =>
        AddFilteredObjectCollectionParams(value, filter, formatString);


    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(
        (T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase  where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(
        (T[]?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase  where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null)  where T : class, TBase  where TBase : class =>
        AddFilteredObjectCollectionParams(value, filter, formatString);
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple)  where T : class, TBase  where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple)   where T : class, TBase  where TBase : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
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
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class =>
        AddObjectCollectionParams(value, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>
        (IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class =>
        AddObjectCollectionParams(value, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase1, TBase2>
        (T[]? value, OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2  where TBase1 : class  where TBase2 : class =>
        AddFilteredObjectCollectionParams(value, filter, customTypeStyler);


    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase1, TBase2>(
        (T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple)
        where T : class, TBase1, TBase2  where TBase1 : class  where TBase2 : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler)  
        where T : class, TBase1, TBase2  where TBase1 : class  where TBase2 : class =>
        AddFilteredObjectCollectionParams(value, filter, customTypeStyler);
    
    [MustUseReturnValue("Use WithOnlyObjectCollectionParams if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParams<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2  where TBase1 : class  where TBase2 : class
    {
        FormatSb.Clear();
        AppendFilteredObjectCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T, TBase>
        (IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class =>
        AddObjectCollectionParamsEnumerate(value, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class
    {
        FormatSb.Clear();
        AppendObjectCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T, TBase>
        (IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class =>
        AddObjectCollectionParamsEnumerate(value, customTypeStyler);

    [MustUseReturnValue("Use WithOnlyObjectCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithObjectCollectionParamsEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class
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
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>
    (KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyFormatString);

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase => 
    AddKeyedCollectionParamsEnumerate(value, valueStyler, keyFormatString);

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase => 
        AddKeyedCollectionParamsEnumerate(value, valueStyler, keyFormatString);

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase 
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase 
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParams(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParamsEnumerate(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParamEnumerate if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AddKeyedCollectionParamsEnumerate(value, valueStyler, keyStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendKeyedCollectionEnumerate(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueFormatString, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }
    
    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyFormatString);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AddFilteredKeyedCollectionParams(value, filterPredicate, valueStyler, keyStyler);

    [MustUseReturnValue("Use WithOnlyKeyedCollectionParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        FormatSb.Clear();
        AppendFilteredKeyedCollection(valueTuple, FormatStsa!);
        return ReplaceTokenNumber().ToAdditionalFormatBuilder(valueTuple);
    }

}
