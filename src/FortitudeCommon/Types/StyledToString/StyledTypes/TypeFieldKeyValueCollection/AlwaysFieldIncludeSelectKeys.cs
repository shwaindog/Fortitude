using System.Diagnostics.CodeAnalysis;
// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.Sb.Append(stb.Settings.NullStyle);
            return stb.AddGoToNext();
        }
        var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            hasValue = selectKeys.MoveNext();
        }
        ekcb.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysWithValueStyler<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysWithValueStyler<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysWithValueStyler<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerateWithValueStyler<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerateWithValueStyler<TKey, TValue, TKDerived, TVBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.Sb.Append(stb.Settings.NullStyle);
            return stb.AddGoToNext();
        }
        var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            hasValue = selectKeys.MoveNext();
        }
        ekcb.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysWithStylers<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysWithStylers<TKey, TValue, TKDerived, TKBase, TVBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysWithStylers<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerateWithStylers<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerateWithStylers<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.Sb.Append(stb.Settings.NullStyle);
            return stb.AddGoToNext();
        }
        var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            hasValue = selectKeys.MoveNext();
        }
        ekcb.AppendCollectionComplete();
        return stb.AddGoToNext();
    }
}
