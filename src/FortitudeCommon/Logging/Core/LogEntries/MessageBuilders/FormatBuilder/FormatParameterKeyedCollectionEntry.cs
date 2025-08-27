using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FormatParameterEntry<TIFormatEntry, TFormatEntryImpl> 
    where TFormatEntryImpl : FLogEntryMessageBuilderBase<TIFormatEntry, TFormatEntryImpl>, TIFormatEntry
    where TIFormatEntry : class, IFLogMessageBuilder
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
        tempStsa = AppendKeyedCollection(tempStsa, value, valueFormatString, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendKeyedCollection(tempStsa, value, valueFormatString, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendKeyedCollection(tempStsa, value, valueFormatString, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueFormatString, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
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
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueFormatString, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue, TVBase>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString)
        where TValue : TVBase 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueStyler, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue, TVBase>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase 
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue, TVBase>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString) where TValue : TVBase 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value, valueStyler, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    public IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase 
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString) where TValue : TVBase
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value, valueStyler, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue, TVBase>
    (KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase 
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString) where TValue : TVBase
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue, TVBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase 
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue, TVBase>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString)
        where TValue : TVBase
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler)  where TKey : TKBase where TValue : TVBase  
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue, TKBase, TVBase>
        (IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler)  where TKey : TKBase where TValue : TVBase  
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue, TKBase, TVBase>
        (KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler)  where TKey : TKBase where TValue : TVBase  
    {
        ReplaceTokenWithKeyedCollection(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollection<TKey, TValue, TKBase, TVBase>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollection(tempStsa, value,  valueStyler, keyStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler)  where TKey : TKBase where TValue : TVBase  
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value,  valueStyler, keyStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddKeyedCollectionParamsEnumerate<TKey, TValue, TKBase, TVBase>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler)  where TKey : TKBase where TValue : TVBase  
    {
        ReplaceTokenWithKeyedCollectionEnumerate(value, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendKeyedCollectionEnumerate(tempStsa, value,  valueStyler, keyStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value,
        KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString, string? keyFormatString) 
        where TKey : TKBase where TValue : TVBase
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueFormatString, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString
      , string? keyFormatString)
        where TKey : TKBase where TValue : TVBase
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueFormatString, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase, TVBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueFormatString, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString
      , string? keyFormatString)
        where TKey : TKBase where TValue : TVBase
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueFormatString, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString) 
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStyler, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString) 
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStyler, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase, TVBase1, TVBase2>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyFormatString);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate, valueStyler, keyFormatString)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler)  where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
        (IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
          , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStyler, keyStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler)  where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
        (KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
          , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStyler, keyStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
    
    protected IFLogAdditionalFormatterParameterEntry? AddFilteredKeyedCollectionParams<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler)  where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2  
    {
        ReplaceTokenWithFilteredKeyedCollection(value, filterPredicate, valueStyler, keyStyler);
        var toReturn = ToAdditionalFormatBuilder(value);
        return toReturn;
    }

    protected void ReplaceTokenWithFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
          , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 
    {
        var tempStsa = TempStyledTypeAppender;
        tempStsa.ClearAndReinitialize(stringStyle: StringBuildingStyle.Default);
        tempStsa = AppendFilteredKeyedCollection(tempStsa, value, filterPredicate,  valueStyler, keyStyler)!;
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }
}
