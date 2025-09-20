using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase, TVBase>(
        string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase, TVBase>(
        string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;

        var hasValue  = value?.MoveNext() ?? false;
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase1, TVBase2>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , StringBearerRevealState<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;

        var hasValue  = value?.MoveNext() ?? false;
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TVBase1, TVBase2>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKey> keyStyler)
        where TKey : struct where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;

        var hasValue  = value?.MoveNext() ?? false;
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
}
