using System.Collections;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm ??= Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm ??= Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm ??= Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
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
            return WhenPopulatedWithFilterEnumerate(fieldName, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerate(value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
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
            return WhenPopulatedWithFilterEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>
                (fieldName, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
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
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterate<TEnumtr, TKFilterBase, TVFilterBase>(
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
            return WhenPopulatedWithFilterIterate(fieldName, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterate(value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
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
            return WhenPopulatedWithFilterIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>
                (fieldName, value.Value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
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
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(value, filterPredicate, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm ??= Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm ??= Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue?>[], TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm ??= Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm ??= 
                        Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue?>>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerateValueRevealer<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterEnumerateValueRevealer
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateValueRevealer<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateValueRevealer
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateNullValueRevealer<TEnumbl, TValue, TKFilterBase>(
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
            return WhenPopulatedWithFilterEnumerateNullValueRevealer
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateNullValueRevealer<TEnumbl, TValue, TKFilterBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateNullValueRevealer
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>(
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
            return WhenPopulatedWithFilterEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterIterateValueRevealer
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateValueRevealer
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
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
            return WhenPopulatedWithFilterIterateNullValueRevealer
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateNullValueRevealer
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
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
            return WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>
                (value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey?, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey?, TValue>[], TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue?>[], TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey?, TValue?>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey?, TValue?>[], TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey?, TValue>>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue?>>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue?>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
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
                    Mws.FieldNameJoin(fieldName);
                    ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey?, TValue?>>, TKey, TValue>(value, createFormatFlags);
                }
                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (ekcm != null)
        {
            ekcm.AppendCollectionComplete();
            return Mws.AddGoToNext();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithFilterEnumerateBothRevealers<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterEnumerateBothRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothRevealers<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateBothRevealers(value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVFilterBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVFilterBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateBothWithNullKeyRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKFilterBase, TKRevealBase>(
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
            return WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKFilterBase, TKRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateBothWithNullValueRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(
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
            return WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
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
            return WhenPopulatedWithFilterEnumerateBothNullRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);

        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredEnumerateBothNullRevealers(value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterIterateBothRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateBothRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterIterateBothWithNullKeyRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateBothWithNullKeyRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
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
            return WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
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
            return WhenPopulatedWithFilterIterateBothWithNullValueRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateBothWithNullValueRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
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
            return WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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
            return WhenPopulatedWithFilterIterateBothNullRevealers
                (fieldName, value.Value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithFilterIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddFilteredIterateBothNullRevealers
                (value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
}
