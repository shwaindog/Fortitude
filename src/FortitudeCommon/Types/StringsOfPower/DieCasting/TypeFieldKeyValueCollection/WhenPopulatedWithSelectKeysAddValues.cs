using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var hasValue    = selectKeys.MoveNext();
        if (value == null)
        {
            return stb.StyleTypeBuilder;
        }
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
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
                ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var hasValue    = selectKeys.MoveNext();
        if (value == null)
        {
            return stb.StyleTypeBuilder;
        }
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
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
                ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var hasValue    = selectKeys.MoveNext();
        if (value == null)
        {
            return stb.StyleTypeBuilder;
        }
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
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
                ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
        }
        return stb.StyleTypeBuilder;
    }
}
