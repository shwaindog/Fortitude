using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase? =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags);

    public TExt AlwaysAddFiltered<TKey, TValue, TKeyFilterBase, TValueFilterBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKeyFilterBase, TValueFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKeyFilterBase? where TValue : TValueFilterBase?
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase? where TValue : TVFilterBase?
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
                var count     = 0;
                var skipCount = 0;
                while (hasValue)
                {
                    count++;
                    if (skipCount-- > 0)
                    {
                        hasValue = value.MoveNext();
                        continue;
                    }
                    var kvp          = value.Current;
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
                    ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
                    if (filterResult is { KeepProcessing: false }) break;
                    skipCount = filterResult.SkipNextCount;
                    hasValue  = value.MoveNext();
                }
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : struct =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase>
    (string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
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
                var count     = 0;
                var skipCount = 0;

                while (hasValue)
                {
                    count++;
                    if (skipCount-- > 0)
                    {
                        hasValue = value.MoveNext();
                        continue;
                    }
                    var kvp          = value.Current;
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
                    ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                    if (filterResult is { KeepProcessing: false }) break;
                    skipCount = filterResult.SkipNextCount;
                    hasValue  = value.MoveNext();
                }
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate, PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
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
            if (hasValue)
            {
                var count     = 0;
                var skipCount = 0;

                while (hasValue)
                {
                    count++;
                    if (skipCount-- > 0)
                    {
                        hasValue = value.MoveNext();
                        continue;
                    }
                    var kvp          = value.Current;
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
                    ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
                    if (filterResult is { KeepProcessing: false }) break;
                    skipCount = filterResult.SkipNextCount;
                    hasValue  = value.MoveNext();
                }
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
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
        where TVRevealBase : notnull =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue>(string fieldName
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue>(
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
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
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
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
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
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue>(
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
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = stb.StyleFormatter.ResolveContentFormatFlags(stb.Sb, value, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, createFormatFlags);
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
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
                var count     = 0;
                var skipCount = 0;
                while (hasValue)
                {
                    count++;
                    if (skipCount-- > 0)
                    {
                        hasValue = value.MoveNext();
                        continue;
                    }
                    var kvp          = value.Current;
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
                    ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                    if (filterResult is { KeepProcessing: false }) break;
                    skipCount = filterResult.SkipNextCount;
                    hasValue  = value.MoveNext();
                }
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
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
                var count     = 0;
                var skipCount = 0;
                while (hasValue)
                {
                    count++;
                    if (skipCount-- > 0)
                    {
                        hasValue = value.MoveNext();
                        continue;
                    }
                    var kvp          = value.Current;
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
                    ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                    if (filterResult is { KeepProcessing: false }) break;
                    skipCount = filterResult.SkipNextCount;
                    hasValue  = value.MoveNext();
                }
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
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
                var count     = 0;
                var skipCount = 0;
                while (hasValue)
                {
                    count++;
                    if (skipCount-- > 0)
                    {
                        hasValue = value.MoveNext();
                        continue;
                    }
                    var kvp          = value.Current;
                    var filterResult = filterPredicate(count, kvp.Key!, kvp.Value);
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
                    ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                    if (filterResult is { KeepProcessing: false }) break;
                    skipCount = filterResult.SkipNextCount;
                    hasValue  = value.MoveNext();
                }
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue>(string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
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
            if (hasValue)
            {
                var count     = 0;
                var skipCount = 0;
                while (hasValue)
                {
                    count++;
                    if (skipCount-- > 0)
                    {
                        hasValue = value.MoveNext();
                        continue;
                    }
                    var kvp          = value.Current;
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
                    ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
                    if (filterResult is { KeepProcessing: false }) break;
                    skipCount = filterResult.SkipNextCount;
                    hasValue  = value.MoveNext();
                }
            }
            ekcm.AppendCollectionComplete();
        }
        else 
            stb.StyleFormatter.AppendFormattedNull(stb.Sb, valueFormatString  ?? "", formatFlags);
        return stb.AddGoToNext();
    }
}
