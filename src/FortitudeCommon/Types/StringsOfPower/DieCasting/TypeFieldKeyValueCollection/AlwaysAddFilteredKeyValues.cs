using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKeyFilterBase, TValueFilterBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKeyFilterBase, TValueFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKeyFilterBase where TValue : TValueFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            var count     = 0;
            var skipCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            var count     = 0;
            var skipCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value!);

            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer);

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }
    
    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            var count     = 0;
            var skipCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue  = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var kvp          = value!.Current;
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullString);
        }
        return stb.AddGoToNext();
    }
}
