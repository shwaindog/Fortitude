using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[]? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived>? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueFormatString, keyFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerate(fieldName, value, selectKeys.Value, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerate(value, selectKeys, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull
        where TKSelectDerived : TKey
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
                (fieldName, value, selectKeys.Value, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(value, selectKeys, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterate(fieldName, value, selectKeys.Value, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterate(value, selectKeys, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull
        where TKSelectDerived : TKey
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
                (fieldName, value, selectKeys.Value, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(value, selectKeys, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[]? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[]? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived>? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived>? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerateValueRevealer
                (fieldName, value, selectKeys.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerateValueRevealer(value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>
                (fieldName, value, selectKeys.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>
                (value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>
                (value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull 
        where TValue : struct 
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerateNullValueRevealer
                (fieldName, value, selectKeys.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull 
        where TValue : struct 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerateNullValueRevealer
                (value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull 
        where TValue : struct
        where TKSelectDerived : TKey 
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
                (fieldName, value, selectKeys.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TValue : struct
        where TKSelectDerived : TKey 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>
                (value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
                (value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterateValueRevealer
                (fieldName, value, selectKeys.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterateValueRevealer(value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull 
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey 
        where TVRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>
                (fieldName, value, selectKeys.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull 
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey 
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>
                (value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>
                (value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull 
        where TValue : struct
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterateNullValueRevealer
                (fieldName, value, selectKeys.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull 
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterateNullValueRevealer(value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull 
        where TValue : struct
        where TKSelectDerived : TKey 
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
                (fieldName, value, selectKeys.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull 
        where TValue : struct
        where TKSelectDerived : TKey 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>
                (value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
                (value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[]? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[]? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Length ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived>? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;
            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived>? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            ExplicitKeyedCollectionMold<TKey, TValue>? ekcb = null;

            var keyCount = selectKeys?.Count ?? 0;
            for (var i = 0; i < keyCount; i++)
            {
                var key = selectKeys![i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ekcb == null)
                {
                    Mws.FieldNameJoin(fieldName);
                    ekcb = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(key, keyValue, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        return Mws.WasSkipped(actualType, fieldName, formatFlags);
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerateBothRevealers
                (fieldName, value, selectKeys.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerateBothRevealers
                (value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
                (fieldName, value, selectKeys.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>
                (value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
                (value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerateBothWithNullValueRevealers
                (fieldName, value, selectKeys.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerateBothWithNullValueRevealers
                (value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
                (fieldName, value, selectKeys.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>
                (value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
                (value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterateBothRevealers
                (fieldName, value, selectKeys.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterateBothRevealers
                (value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>
                (fieldName, value, selectKeys.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>
                (value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>
                (value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers
                (fieldName, value, selectKeys.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterateBothWithNullValueRevealers(value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (selectKeys != null)
        {
            return WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>
                (fieldName, value, selectKeys.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>
                (value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>
                (value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
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
