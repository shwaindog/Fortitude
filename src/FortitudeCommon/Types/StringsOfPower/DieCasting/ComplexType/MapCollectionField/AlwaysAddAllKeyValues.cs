// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            foreach (var kvp in value) { ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString); }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, IEnumerable<KeyValuePair<TKey, TValue>>? valueAgain
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            foreach (var kvp in value) { ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString); }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString, formatFlags);

    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString, formatFlags);

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            foreach (var kvp in value) { ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString); }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            foreach (var kvp in value) { ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString); }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, formatFlags);

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, formatFlags);

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(string fieldName, KeyValuePair<TKey?, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, KeyValuePair<TKey?, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            foreach (var kvp in value) { ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer); }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(string fieldName, IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            foreach (var kvp in value) { ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer); }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            foreach (var kvp in value) { ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer); }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            foreach (var kvp in value) { ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer); }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value!, formatFlags);
            var hasValue = value?.MoveNext() ?? false;
            if (hasValue)
            {
                while (hasValue)
                {
                    var kvp = value!.Current;
                    ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                    hasValue = value.MoveNext();
                }
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase>(string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }
}
