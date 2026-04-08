// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerate(value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKeyFilterBase, TValueFilterBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKeyFilterBase, TValueFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKeyFilterBase? where TValue : TValueFilterBase?
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString ?? "", keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString ?? "", keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateValueRevealer<IReadOnlyDictionary<TKey, TValue>, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : struct 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateNullValueRevealer<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue, TKFilterBase>
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase>(
        string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue?>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase>(
        string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master
                          .StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue?>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothRevealers<IReadOnlyDictionary<TKey, TValue>, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothWithNullValueRevealers<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue, TKFilterBase, TKRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey?, TValue>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue?>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey?, TValue?>[]? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey?, TValue?>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master
                          .StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey?, TValue>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master
                          .StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue?>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TMold AlwaysAddFiltered<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm
                = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey?, TValue?>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i + 1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }
    
}
