using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Count; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Count; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
       , IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Count; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived>? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var hasValue = selectKeys?.MoveNext() ?? false;
        var ekcm     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        while (hasValue)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            hasValue = selectKeys.MoveNext();
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            hasValue = selectKeys.MoveNext();
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Length; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Count; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        for (var i = 0; i < selectKeys.Count; i++)
        {
            var key = selectKeys[i];
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        foreach (var key in selectKeys)
        {
            if (!value.TryGetValue(key, out var keyValue)) continue;
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived>? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var hasValue = selectKeys?.MoveNext() ?? false;
        
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        while (hasValue)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            hasValue = selectKeys.MoveNext();
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        if (value == null)
        {
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString, formatFlags);
            return stb.AddGoToNext();
        }
        var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
        while (hasValue)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            hasValue = selectKeys.MoveNext();
        }
        ekcm.AppendCollectionComplete();
        return stb.AddGoToNext();
    }
}
