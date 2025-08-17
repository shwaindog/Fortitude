// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

public partial class KeyValueCollectionBuilder
{
    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull
    {
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                : stb.AppendOrNull(key).Append(": ");
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                : stb.AppendOrNull(keyValue);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
          , CustomTypeStyler<TValue> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys, CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, CustomTypeStyler<TValue> valueStyler 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                : stb.AppendOrNull(key).Append(": ");
            stb.AppendOrNull(keyValue, valueStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct 
    {
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendOrNull(key, keyStyler).Append(": ");
            stb.AppendOrNull(keyValue, valueStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }
}
