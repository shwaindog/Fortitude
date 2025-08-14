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
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, string? keyFormatString)
        where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueStructStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, string? keyFormatString) where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueStructStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString) where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStructStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString) where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStructStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString) where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStructStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    public IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString) where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value, valueStructStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler, string? keyFormatString)
        where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStructStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, string? keyFormatString)
        where TValue : struct
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStructStyler, keyFormatString);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStructStyler, keyStructStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStructStyler, keyStructStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollection(value, valueStructStyler, keyStructStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStructStyler, keyStructStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyStructStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value,  valueStructStyler, keyStructStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStructStyler, keyStructStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value,  valueStructStyler, keyStructStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStructStyler, keyStructStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStructStyler, keyStructStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)  where TKey : struct where TValue : struct  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStructStyler, keyStructStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStructStyler, keyStructStyler);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
}
