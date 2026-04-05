// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{

    public TMold WhenPopulatedWithFilterIterate<TEnumtr, TKFilterBase, TVFilterBase>(
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

    public TMold WhenPopulatedWithFilterIterate<TEnumtr, TKFilterBase, TVFilterBase>(
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

    public TMold WhenPopulatedWithFilterIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
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

    public TMold WhenPopulatedWithFilterIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
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

    public TMold WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
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

    public TMold WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
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

    public TMold WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
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

    public TMold WhenPopulatedWithFilterIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
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

    public TMold WhenPopulatedWithFilterIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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

    public TMold WhenPopulatedWithFilterIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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
