using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var hasValue    = selectKeys.MoveNext();
        if (value == null)
        {
            return stb.StyleTypeBuilder;
        }
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var hasValue    = selectKeys.MoveNext();
        if (value == null)
        {
            return stb.StyleTypeBuilder;
        }
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyFormatString);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var hasValue    = selectKeys.MoveNext();
        if (value == null)
        {
            return stb.StyleTypeBuilder;
        }
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueStyler, keyStyler);
        }
        return stb.StyleTypeBuilder;
    }
}
