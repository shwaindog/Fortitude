// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    
    public TMold WhenPopulatedAddAllEnumerate<TEnumbl>(
        string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerate(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerate<TEnumbl>(
        string fieldName
      , TEnumbl? value
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
            ekcm.AddAllEnumerate(value, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerate<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerate<TEnumbl, TKey, TValue>(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    } 
    
    public TMold WhenPopulatedAddAllEnumerate<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllEnumerate<TEnumbl, TKey, TValue>(value, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TMold WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TVRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerateValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
        string fieldName
      , TEnumbl? value
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
            ekcm.AddAllEnumerateValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
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
            ekcm.AddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
                (value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TValue : struct 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerateNullValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
        string fieldName
      , TEnumbl? value
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
            ekcm.AddAllEnumerateNullValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TValue : struct 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
                (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
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
            ekcm.AddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
                (value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TMold WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
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
            return WhenPopulatedAddAllEnumerateBothRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
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
            ekcm.AddAllEnumerateBothRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull 
        where TVRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
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
            ekcm.AddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
        string fieldName
      , TEnumbl? value
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
            return WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
        string fieldName
      , TEnumbl? value
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
            ekcm.AddAllEnumerateBothWithNullKeyRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue>>
        where TKey : struct 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct 
        where TValue : TVRevealBase?
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
            ekcm.AddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
        string fieldName
      , TEnumbl? value
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
            return WhenPopulatedAddAllEnumerateBothWithNullValueRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
        string fieldName
      , TEnumbl? value
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
            ekcm.AddAllEnumerateBothWithNullValueRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase? 
        where TValue : struct
        where TKRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
        string fieldName
      , TEnumbl? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase? 
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
            ekcm.AddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
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
            return WhenPopulatedAddAllEnumerateBothNullRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TMold WhenPopulatedAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
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
            ekcm.AddAllEnumerateBothNullRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
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
