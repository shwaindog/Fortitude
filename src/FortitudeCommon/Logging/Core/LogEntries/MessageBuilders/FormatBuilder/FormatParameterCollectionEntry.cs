using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FormatParameterEntry<TIFormatEntry, TFormatEntryImpl> 
    where TFormatEntryImpl : FLogEntryMessageBuilderBase<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
    where TIFormatEntry : class, IFLogMessageBuilder
{
    protected FLogAdditionalFormatterParameterEntry? AddValueCollectionParams<TFmtStruct>
        (TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TFmtStruct>(TFmtStruct[]? value, string? formatString) where TFmtStruct : struct, ISpanFormattable
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollection(tempStsa, value, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParams<TFmtStruct>
        (IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString) 
        where TFmtStruct : struct, ISpanFormattable
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollection(tempStsa, value, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParams<TToStyle, TStylerType>(TToStyle[]? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        ReplaceTokenWithValueCollection(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollection(tempStsa, value, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParams<TToStyle, TStylerType>
        (IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        ReplaceTokenWithValueCollection(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollection(tempStsa, value, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredValueCollectionParams<TFmtStruct>
    (TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter, string? formatString)
        where TFmtStruct : struct, ISpanFormattable
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredValueCollection(tempStsa, value, filter, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredValueCollectionParams<TFmtStruct>
    (IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollection(value, filter, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter, string? formatString)
        where TFmtStruct : struct, ISpanFormattable
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredValueCollection(tempStsa, value, filter, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredValueCollectionParams<TToStyle, TToStyleBase, TStylerType>
    (TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        ReplaceTokenWithValueCollection(value, filter, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TToStyleBase, TStylerType
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredValueCollection(tempStsa, value, filter, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredValueCollectionParams<TToStyle, TToStyleBase, TStylerType>
    (IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType 
    {
        ReplaceTokenWithValueCollection(value, filter, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TToStyleBase, TStylerType
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredValueCollection(tempStsa, value, filter, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddObjectCollectionParams<T>(T[]? value, string? formatString = null) 
        where T : class 
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T>(T[]? value, string? formatString) where T : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendObjectCollection(tempStsa, value, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddObjectCollectionParams<T>
        (IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T>(IReadOnlyList<T>? value, string? formatString) where T : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendObjectCollection(tempStsa, value, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddObjectCollectionParams<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class 
    {
        ReplaceTokenWithObjectCollection(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendObjectCollection(tempStsa, value, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddObjectCollectionParams<T, TBase>
        (IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class 
    {
        ReplaceTokenWithObjectCollection(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendObjectCollection(tempStsa, value, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredObjectCollectionParams<T, TBase>
        (T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class
    {
        ReplaceTokenWithObjectCollection(value, filter, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T, TBase>(T[]? value, OrderedCollectionPredicate<T> filter, string? formatString) 
        where T : class, TBase where TBase : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredObjectCollection(tempStsa, value, filter, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredObjectCollectionParams<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class 
    {
        ReplaceTokenWithObjectCollection(value, filter, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString) where T : class, TBase where TBase : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredObjectCollection(tempStsa, value, filter, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredObjectCollectionParams<T, TBase1, TBase2>
        (T[]? value, OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        ReplaceTokenWithObjectCollection(value, filter, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredObjectCollection(tempStsa, value, filter, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredObjectCollectionParams<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class 
    {
        ReplaceTokenWithObjectCollection(value, filter, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredObjectCollection(tempStsa, value, filter, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParamsEnumerate<TFmtStruct>
        (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable 
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollectionEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString)
        where TFmtStruct : struct, ISpanFormattable
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollectionEnumerate(tempStsa, value, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParamsEnumerate<TFmtStruct>
        (IEnumerator<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        ReplaceTokenWithValueCollectionEnumerate(value, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollectionEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString)
        where TFmtStruct : struct, ISpanFormattable
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollectionEnumerate(tempStsa, value, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParamsEnumerate<TToStyle, TStylerType>
        (IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        ReplaceTokenWithValueCollectionEnumerate(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollectionEnumerate<TToStyle, TStylerType>
        (IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollectionEnumerate(tempStsa, value, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParamsEnumerate<TToStyle, TStylerType>
        (IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        ReplaceTokenWithValueCollectionEnumerate(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollectionEnumerate(tempStsa, value, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddObjectCollectionParamsEnumerate<T>
        (IEnumerable<T>? value, string? formatString = null) where T : class 
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollectionEnumerate<T>(IEnumerable<T>? value, string? formatString) where T : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendObjectCollectionEnumerate(tempStsa, value, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddObjectCollectionParamsEnumerate<T>
        (IEnumerator<T>? value, string? formatString = null) 
        where T : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollectionEnumerate<T>(IEnumerator<T>? value, string? formatString) where T : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendObjectCollectionEnumerate(tempStsa, value, formatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddObjectCollectionParamsEnumerate<T, TBase>
        (IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class 
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollectionEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendObjectCollectionEnumerate(tempStsa, value, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddObjectCollectionParamsEnumerate<T, TBase>
        (IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        ReplaceTokenWithObjectCollectionEnumerate(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollectionEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendObjectCollectionEnumerate(tempStsa, value, customTypeStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
}
