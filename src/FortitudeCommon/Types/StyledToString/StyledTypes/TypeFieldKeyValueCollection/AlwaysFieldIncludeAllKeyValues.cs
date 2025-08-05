// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt>  where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, KeyValuePair<TKey, TValue>[]? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty() 
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
          , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, KeyValuePair<TKey, TValue>[]? value
          , StructStyler<TValue> valueStructStyler
          , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue> (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartDictionary();
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                stb.AppendOrNull(kvp.Value, valueStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
}
