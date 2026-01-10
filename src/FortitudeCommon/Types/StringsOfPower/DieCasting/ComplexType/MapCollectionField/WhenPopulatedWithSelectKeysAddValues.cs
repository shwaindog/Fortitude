using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[]? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived>? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived>? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys ?? [])
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived>? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value == null)
        {
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        }
        var hasValue = selectKeys?.MoveNext() ?? false;
        
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcb     = null;
        while (hasValue)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
            hasValue = selectKeys.MoveNext();
        }
        if (ekcb != null) ekcb.AppendCollectionComplete();
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[]? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, TKSelectDerived[]? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived>? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IReadOnlyList<TKSelectDerived>? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived>? selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys ?? [])
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IEnumerable<TKSelectDerived>? selectKeys, PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys ?? [])
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived>? selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value == null)
        {
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        }
        var hasValue = selectKeys?.MoveNext() ?? false;
        
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
        while (hasValue)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            hasValue = selectKeys.MoveNext();
        }
        if (ekcb != null) ekcb.AppendCollectionComplete();
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value, IEnumerator<TKSelectDerived>? selectKeys, PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value == null)
        {
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        }
        var hasValue = selectKeys?.MoveNext() ?? false;
        
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcb     = null;
        while (hasValue)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString);
            hasValue = selectKeys.MoveNext();
        }
        if (ekcb != null) ekcb.AppendCollectionComplete();
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[]? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, TKSelectDerived[]? selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys, PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value, ReadOnlySpan<TKSelectDerived> selectKeys, PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived>? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IReadOnlyList<TKSelectDerived>? selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived>? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys ?? [])
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IEnumerable<TKSelectDerived>? selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            foreach (var key in selectKeys ?? [])
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived>? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value == null)
        {
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        }
        var hasValue    = selectKeys?.MoveNext() ?? false;
        
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
        while (hasValue)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            hasValue = selectKeys.MoveNext();
        }
        if (ekcb != null) ekcb.AppendCollectionComplete();
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        var hasValue    = selectKeys.MoveNext();
        if (value == null)
        {
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
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
                ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer);
            hasValue = selectKeys.MoveNext();
        }
        if (ekcb != null) ekcb.AppendCollectionComplete();
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value.GetType(), fieldName, formatFlags);
    }
}
