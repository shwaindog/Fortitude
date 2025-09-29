using System.Diagnostics.CodeAnalysis;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.Sb.Append(stb.Settings.NullStyle);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            hasValue = selectKeys.MoveNext();
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.Sb.Append(stb.Settings.NullStyle);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            hasValue = selectKeys.MoveNext();
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.Sb.Append(stb.Settings.NullStyle);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            hasValue = selectKeys.MoveNext();
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }
}
