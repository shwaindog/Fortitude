// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public interface IAlwaysFieldIncludeAllKeyValues<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;
}

public class AlwaysFieldIncludeAllKeyValues<TExt> : RecyclableObject, IAlwaysFieldIncludeAllKeyValues<TExt> 
    where TExt : StyledTypeBuilder
    
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    public AlwaysFieldIncludeAllKeyValues<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder)
    {
        stb  = styledComplexTypeBuilder;

        return this;
    }

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
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

    public TExt WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
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

    public TExt WithName<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
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

    public TExt WithName<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
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

    public TExt WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>
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

    public TExt WithName<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
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

    public TExt WithName<TKey, TValue> (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
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

    public TExt WithName<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
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

    public override void StateReset()
    {
        stb = null!;
        base.StateReset();
    }
}
