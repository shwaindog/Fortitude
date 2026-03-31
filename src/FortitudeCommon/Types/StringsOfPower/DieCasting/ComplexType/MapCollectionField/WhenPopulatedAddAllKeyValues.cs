// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !Mws.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);
    
    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !Mws.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);
    
    public TExt WhenPopulatedAddAllEnumerate<TEnumbl>(
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
    
    public TExt WhenPopulatedAddAllEnumerate<TEnumbl>(
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
    
    public TExt WhenPopulatedAddAllEnumerate<TEnumbl, TKey, TValue>(
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
    
    public TExt WhenPopulatedAddAllEnumerate<TEnumbl, TKey, TValue>(
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
    
    public TExt WhenPopulatedAddAllIterate<TEnumtr>(
        string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterate(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterate<TEnumtr>(
        string fieldName
      , TEnumtr? value
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
            ekcm.AddAllIterate(value, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterate<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterate<TEnumtr, TKey, TValue>(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterate<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterate<TEnumtr, TKey, TValue>(value, valueFormatString, keyFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull  =>
        !Mws.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct   =>
        !Mws.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct  =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct  =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
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
            ekcm.AddAllEnumerateValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
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
    
    public TExt WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
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
    
    public TExt WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
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
    
    public TExt WhenPopulatedAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
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
            ekcm.AddAllEnumerateNullValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterateValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
        string fieldName
      , TEnumtr? value
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
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TValue>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterateNullValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TValue>(
        string fieldName
      , TEnumtr? value
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
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags, keyFormatString ?? "");
        if (value != null)
        {
            var ekct = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekct).BeforeFirstElementWriteFieldName(fieldName);
            ekct.AddAllIterateNullValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekct.Complete();
            return Mws.AddGoToNext();
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TValue : struct 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
                (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateNullValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey?, TValue>[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey?, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct  =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey?, TValue?>[]), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue>>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct  =>
        !Mws.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue?>>), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
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
            ekcm.AddAllEnumerateBothRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
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
            ekcm.AddAllEnumerateBothWithNullKeyRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
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
            ekcm.AddAllEnumerateBothWithNullValueRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
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
    
    public TExt WhenPopulatedAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
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

    public TExt WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
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
            return WhenPopulatedAddAllIterateBothRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
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
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateBothRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull 
        where TVRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull 
        where TVRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateBothRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
        string fieldName
      , TEnumtr? value
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
            return WhenPopulatedAddAllIterateBothWithNullKeyRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
        string fieldName
      , TEnumtr? value
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
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateBothWithNullKeyRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateBothWithNullKeyRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
        string fieldName
      , TEnumtr? value
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
            return WhenPopulatedAddAllIterateBothWithNullValueRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
        string fieldName
      , TEnumtr? value
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
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateBothWithNullValueRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.Complete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase? 
        where TValue : struct
        where TKRevealBase : notnull 
    {
        if (value != null)
        {
            return WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase? 
        where TValue : struct
        where TKRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateBothWithNullValueRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
            var anyItems = ekcm.ItemCount > 0;
            ekcm.AppendCollectionComplete();
            if (anyItems)
            {
                return Mws.AddGoToNext();
            }
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
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
            return WhenPopulatedAddAllIterateBothNullRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        return Mws.Mold;
    }
    
    public TExt WhenPopulatedAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
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
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags | SuppressOpening);
            ((IKeyedCollectionExtendFunctionality)ekcm).BeforeFirstElementWriteFieldName(fieldName);
            ekcm.AddAllIterateBothNullRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
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
