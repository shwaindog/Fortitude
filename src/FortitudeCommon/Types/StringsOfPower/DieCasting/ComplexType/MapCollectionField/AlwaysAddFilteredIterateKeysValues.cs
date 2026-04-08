// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold AlwaysAddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
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

    public TMold AlwaysAddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
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

    public TMold AlwaysAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
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

    public TMold AlwaysAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
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

    public TMold AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
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

    public TMold AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
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

    public TMold AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
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

    public TMold AlwaysAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
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
    
    public TMold AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
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

    public TMold AlwaysAddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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

    public TMold AlwaysAddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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
