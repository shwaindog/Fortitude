using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
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
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value!);
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

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, KeyValuePair<TKey, TValue?>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value!);
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

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                var filterResult = filterPredicate(i, kvp.Key, kvp.Value!);
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

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
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
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value!);
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

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase
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

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull  
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
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value!);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
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

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull  
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value!);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
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

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull  
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value!);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
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

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull  
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
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value!);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
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

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull  
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
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
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

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull  
        where TVRevealBase : notnull  
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
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value!);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
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

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(string fieldName
      , KeyValuePair<TKey, TValue?>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull  
        where TVRevealBase : notnull  
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value!);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
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

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull  
        where TVRevealBase : notnull  
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp          = value[i];
                var filterResult = filterPredicate(i, kvp.Key, kvp.Value!);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
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

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull  
        where TVRevealBase : notnull  
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
                var filterResult = filterPredicate(count, kvp.Key, kvp.Value!);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
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

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull  
        where TVRevealBase : notnull  
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
            var filterResult = filterPredicate(count, kvp.Key, kvp.Value!);
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
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
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
