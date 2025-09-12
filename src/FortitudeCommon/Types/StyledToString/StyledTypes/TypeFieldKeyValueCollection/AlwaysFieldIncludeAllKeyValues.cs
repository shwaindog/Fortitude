// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt>  where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString);

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, KeyValuePair<TKey, TValue>[]? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>
        (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>
        (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
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
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase =>
        AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyFormatString);

    public TExt AlwaysAddAll<TKey, TValue, TVBase>(string fieldName, KeyValuePair<TKey, TValue>[]? value
          , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVBase>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
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
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKBase, TVBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase =>
        AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyStyler);

    public TExt AlwaysAddAll<TKey, TValue, TKBase, TVBase>(string fieldName, KeyValuePair<TKey, TValue>[]? value
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKBase, TVBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKBase, TVBase> (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKBase, TVBase>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                stb.AppendOrNull(kvp.Value, valueStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }
}
