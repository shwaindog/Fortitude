using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
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
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
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
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
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
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
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
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyStyler);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }
    
    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            ekcm.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }
}
