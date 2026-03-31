using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKeyFilterBase, TValueFilterBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
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

    public TExt AlwaysAddFilteredEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerate(fieldName, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredEnumerate(value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>
                (fieldName, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }
    
    public TExt AlwaysAddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>
                (value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterate(fieldName, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredIterate(value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>
                (fieldName, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>
                (value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase>(
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

    public TExt AlwaysAddFilteredEnumerateValueRevealer<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateValueRevealer
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateValueRevealer<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredEnumerateValueRevealer(value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateNullValueRevealer<TEnumbl, TValue, TKFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TValue : struct
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateNullValueRevealer
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateNullValueRevealer<TEnumbl, TValue, TKFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredEnumerateNullValueRevealer(value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?
        where TValue : struct
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateValueRevealer
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredIterateValueRevealer(value, filterPredicate, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateNullValueRevealer
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredIterateNullValueRevealer
                (value, filterPredicate, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?
        where TValue : struct
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>
                (value, filterPredicate, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TExt AlwaysAddFiltered<TKey, TValue>(
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

    public TExt AlwaysAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TExt AlwaysAddFilteredEnumerateBothRevealers<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateBothRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothRevealers<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothRevealers(value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKey : struct
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateBothWithNullKeyRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothWithNullKeyRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateBothWithNullValueRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothWithNullValueRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct
    {
        if (value != null)
        {
            return AlwaysAddFilteredEnumerateBothNullRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredEnumerateBothNullRevealers(value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateBothRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredIterateBothRevealers(value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKey : struct
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateBothWithNullKeyRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredIterateBothWithNullKeyRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateBothWithNullValueRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddFilteredIterateBothWithNullValueRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct
    {
        if (value != null)
        {
            return AlwaysAddFilteredIterateBothNullRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddFilteredIterateBothNullRevealers(value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString ?? "", formatFlags);
        return Mws.AddGoToNext();
    }
}
