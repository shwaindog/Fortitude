using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase>(
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
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : struct  
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : struct  
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : struct  
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm ??= stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? 
        where TValue : struct  
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
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
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase>(
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
        if (stb.SkipField<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
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
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TVFilterBase, TVRevealBase>(
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
        if (stb.SkipField<KeyValuePair<TKey?, TValue>[]>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<KeyValuePair<TKey?, TValue>[]>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<KeyValuePair<TKey?, TValue>[]>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase>(
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
        if (stb.SkipField<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue>(
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
        if (stb.SkipField<KeyValuePair<TKey?, TValue?>[]>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<KeyValuePair<TKey?, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<KeyValuePair<TKey?, TValue?>[]>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
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
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TVFilterBase, TVRevealBase>(
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
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKFilterBase, TKRevealBase>(
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
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue>(
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
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
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
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
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
                if (ekcm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVFilterBase?, TVRevealBase?  
        where TVRevealBase : notnull  
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerable<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : struct
        where TKRevealBase : notnull  
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        ExplicitKeyedCollectionMold<TKey, TValue>? ekcm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var kvp in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, kvp.Key!, kvp.Value);
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
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct  
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
                    ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerable<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
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
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVFilterBase?, TVRevealBase?  
        where TVRevealBase : notnull  
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerator<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : struct
        where TKRevealBase : notnull  
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct  
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);

        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
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
            if (ekcm == null)
            {
                stb.FieldNameJoin(fieldName);
                ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
            }
            ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            if (filterResult is { KeepProcessing: false }) break;
            skipCount = filterResult.SkipNextCount;
            hasValue  = value.MoveNext();
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.WasSkipped<IEnumerator<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);
    }
}
