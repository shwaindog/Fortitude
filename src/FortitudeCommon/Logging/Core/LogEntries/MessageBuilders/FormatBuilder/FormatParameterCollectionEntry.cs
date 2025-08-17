using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FormatParameterEntry<TFormatEntry> 
    where TFormatEntry : FormatParameterEntry<TFormatEntry>
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
        tempStsa = AppendValueCollection(tempStsa, value, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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

    protected void ReplaceTokenWithValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString) where TFmtStruct : struct, ISpanFormattable
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollection(tempStsa, value, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParams<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollection(tempStsa, value, customTypeStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParams<TStruct>
        (IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollection(tempStsa, value, customTypeStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendFilteredValueCollection(tempStsa, value, filter, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendFilteredValueCollection(tempStsa, value, filter, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredValueCollectionParams<TStruct>
    (TStruct[]? value, OrderedCollectionPredicate<TStruct> filter
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollection(value, filter, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredValueCollection(tempStsa, value, filter, customTypeStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredValueCollectionParams<TStruct>
    (IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filter
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct 
    {
        ReplaceTokenWithValueCollection(value, filter, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollection<TStruct>(IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredValueCollection(tempStsa, value, filter, customTypeStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendObjectCollection(tempStsa, value, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendObjectCollection(tempStsa, value, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AddFilteredObjectCollectionParams<T>
        (T[]? value, OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class
    {
        ReplaceTokenWithObjectCollection(value, filter, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T>(T[]? value, OrderedCollectionPredicate<T> filter, string? formatString) where T : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredObjectCollection(tempStsa, value, filter, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredObjectCollectionParams<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter
      , string? formatString = null) where T : class 
    {
        ReplaceTokenWithObjectCollection(value, filter, formatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithObjectCollection<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter, string? formatString) where T : class
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredObjectCollection(tempStsa, value, filter, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendValueCollectionEnumerate(tempStsa, value, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendValueCollectionEnumerate(tempStsa, value, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParamsEnumerate<TStruct>
        (IEnumerable<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollectionEnumerate(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollectionEnumerate<TStruct>
        (IEnumerable<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollectionEnumerate(tempStsa, value, customTypeStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddValueCollectionParamsEnumerate<TStruct>
        (IEnumerator<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        ReplaceTokenWithValueCollectionEnumerate(value, customTypeStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithValueCollectionEnumerate<TStruct>(IEnumerator<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendValueCollectionEnumerate(tempStsa, value, customTypeStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendObjectCollectionEnumerate(tempStsa, value, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendObjectCollectionEnumerate(tempStsa, value, formatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
}
