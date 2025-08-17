using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FormatParameterEntry<TFormatEntry> 
    where TFormatEntry : FormatParameterEntry<TFormatEntry>
{
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null, string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString, string? keyFormatString)
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value, valueFormatString, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString, string? keyFormatString)
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value, valueFormatString, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollection(value, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString, string? keyFormatString)
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value, valueFormatString, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString, string? keyFormatString)
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueFormatString, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString, string? keyFormatString)
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueFormatString, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString, string? keyFormatString)
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueFormatString, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString
      , string? keyFormatString)
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueFormatString, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString
      , string? keyFormatString)
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueFormatString, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler, string? keyFormatString)
        where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler, string? keyFormatString) where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString) where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString) where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString) where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    public IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString) where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value, valueStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TValue> valueStyler, string? keyFormatString)
        where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler, string? keyFormatString)
        where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value,  valueStyler, keyStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value,  valueStyler, keyStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStyler, keyStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStyler, keyStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStyler, keyStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
}
