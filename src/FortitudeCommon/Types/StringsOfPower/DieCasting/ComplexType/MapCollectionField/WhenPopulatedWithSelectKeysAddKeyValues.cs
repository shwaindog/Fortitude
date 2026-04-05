// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(string fieldName
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(string fieldName
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(string fieldName
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
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
}
