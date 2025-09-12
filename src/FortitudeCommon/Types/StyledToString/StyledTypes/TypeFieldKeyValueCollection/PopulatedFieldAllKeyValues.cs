// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAll(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
        !stb.SkipFields && (value?.Any() ?? false) ?  AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                stb.AppendOrNull(kvp.Key, keyStyler, true);
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

}
