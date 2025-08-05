using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        var foundValues = false;
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        var foundValues = false;
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        var foundValues = false;
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        var foundValues = false;
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull
    {
        var foundValues = false;
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                : stb.AppendOrNull(key).Append(": ");
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                : stb.AppendOrNull(keyValue);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
     ,  [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
     ,  [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
     ,  [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
     ,  [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
         , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        var foundValues = false;
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(key, keyFormatString).Append(": ")
                : stb.AppendOrNull(key).Append(": ");
            stb.AppendOrNull(keyValue, valueStructStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , TKey[] selectKeys , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        WhenPopulatedWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            stb.AppendOrNull(key, keyStructStyler).Append(": ");
            stb.AppendOrNull(keyValue, valueStructStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            stb.AppendOrNull(key, keyStructStyler).Append(": ");
            stb.AppendOrNull(keyValue, valueStructStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }
}