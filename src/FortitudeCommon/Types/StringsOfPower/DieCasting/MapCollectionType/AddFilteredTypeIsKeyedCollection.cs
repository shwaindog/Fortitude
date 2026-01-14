// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public partial class KeyedCollectionMold
{
    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase where TValue : TVFilterBase? =>
        AddFilteredEnumerate(value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
    {
        if (stb.HasSkipBody<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);

        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
    {
        if (stb.HasSkipBody<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
    {
        if (stb.HasSkipBody<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
        (IReadOnlyDictionary<TKey, TValue>? value
          , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
          , PalantírReveal<TVRevealBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        AddFilteredEnumerate(value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase>
        (IReadOnlyDictionary<TKey, TValue?>? value
          , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
          , PalantírReveal<TValue> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : struct =>
        AddFilteredEnumerate(value, filterPredicate, valueStyler, keyFormatString, valueFormatString, formatFlags);

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase>
    (KeyValuePair<TKey, TValue?>[]? value, KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct
    {
        if (stb.HasSkipBody<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase>(
        IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct
    {
        if (stb.HasSkipBody<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var skipCount = 0;
            int count     = 0;
            ItemCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey?, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey?, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey?, TValue>);
            var skipCount = 0;
            int count     = 0;
            ItemCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue?>);
            var skipCount = 0;
            int count     = 0;
            ItemCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey?, TValue?>);
            var skipCount = 0;
            int count     = 0;
            ItemCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase>(
        IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct
    {
        if (stb.HasSkipBody<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AddFilteredEnumerate(value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull =>
        AddFilteredEnumerate(value, filterPredicate, valueStyler, keyStyler, valueFormatString, formatFlags);

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        KeyValuePair<TKey?, TValue>[]? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue>(
        KeyValuePair<TKey?, TValue?>[]? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.HasSkipBody<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFiltered<TKey, TValue>(
        IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.HasSkipBody<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i+1, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var skipCount = 0;
            int count     = 0;
            ItemCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var skipCount = 0;
            int count     = 0;
            ItemCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
        IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue?>);
            var skipCount = 0;
            int count     = 0;
            ItemCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue>(
        IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.HasSkipBody<IEnumerable<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey?, TValue?>);
            var skipCount = 0;
            int count     = 0;
            ItemCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        IEnumerator<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
        IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddFilteredEnumerate<TKey, TValue>(
        IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.HasSkipBody<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        return stb.StyleTypeBuilder;
    }
}
