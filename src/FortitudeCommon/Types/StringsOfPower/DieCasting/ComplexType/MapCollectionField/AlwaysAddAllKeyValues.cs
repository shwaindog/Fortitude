// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddAllEnumerate(fieldName, value, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags);

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            foreach (var kvp in value) { ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString); }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
                hasValue = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            foreach (var kvp in value)
            {
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            foreach (var kvp in value)
            {
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                hasValue = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                hasValue = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey?, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            foreach (var kvp in value)
            {
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            foreach (var kvp in value)
            {
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            foreach (var kvp in value)
            {
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            foreach (var kvp in value)
            {
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            var hasValue = value.MoveNext();
            if (hasValue)
            {
                while (hasValue)
                {
                    var kvp = value.Current;
                    ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                    hasValue = value.MoveNext();
                }
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                hasValue = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                hasValue = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
            var hasValue = value.MoveNext();
            while (hasValue)
            {
                var kvp = value.Current;
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                hasValue = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }
}
